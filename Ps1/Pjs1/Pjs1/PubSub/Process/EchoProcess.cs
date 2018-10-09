using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pjs1.Main.PubSub.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.GenericDbContext;
using Pjs1.DAL.Implementations;
using Pjs1.DAL.Interfaces;
using System.Reflection.Emit;

namespace Pjs1.Main.PubSub.Process
{
    internal sealed class EchoProcess : MainChannelData
    {
        private List<ChannelModel> GetPublicChannelLists => GetChannelList();
        public static async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var channelSlugUrl = context.Request.Path.Value.Substring(1);
            var connectionId = context.Connection.Id;
            //regis user subscript websocket  Connection
            var userConnectionData =
                RegisWebSocketProcess.GetConnectionRegis(connectionId, channelSlugUrl)
                ??
                RegisWebSocketProcess.RegisWebSocket(channelSlugUrl, connectionId, webSocket);

            ////#region Reply Connecttion Id
            ////var replyConnectionData = new ReceiveSocketDataModel
            ////{
            ////    ConnectionId = userConnectionData.ConnectionId,
            ////    ConnectionName = "",
            ////    MessageJson = new[] { "connection", "successful" },
            ////    InvokeMethodName = "InitialConnection"
            ////};
            ////await SendToWebSocket(webSocket, replyConnectionData, CancellationToken.None);
            ////#endregion
            ////SetConnectionSocketList(userConnectionData);


            //await FlushAllConnectionList(channelSlugUrl);

            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    //var chanelList = GetChannelList();
                    var content = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var receiveDataModel = JsonConvert.DeserializeObject<ReceiveSocketDataModel>(content);

                    #region UseWhenInvoke method and return data to client 
                    // var typeVar = Type.GetType("SignalService.MyTestHub");
                    // TODO : "InvokeMethod
                    var invokeResult = await InvokeProcess.InvokeMethod(userConnectionData, receiveDataModel);
                    //TODO : get this varaible [invokeResult] type  check before SendString()  // //  // Convert.ChangeType(mainValue, mainMethod.ReturnType) ;
                    if (invokeResult != null)
                    {
                        var invokeResultString = invokeResult as string;
                        var replySubscriptData = new ReceiveSocketDataModel
                        {
                            ConnectionId = receiveDataModel.ConnectionId,
                            ConnectionName = receiveDataModel.ConnectionName,
                            MessageJson = new[] { invokeResultString },
                            InvokeMethodName = receiveDataModel.InvokeMethodName
                        };
                        await SendToClientWebSocket(webSocket, replySubscriptData, CancellationToken.None);
                    }
                    // --//
                    #endregion
                }
                //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            RegisWebSocketProcess.RemoveConnectionSocket(userConnectionData.ConnectionId, channelSlugUrl);
            //await FlushAllConnectionList(channelSlugUrl);
        }

        private static async Task FlushAllConnectionList(string channelSlugUrl)
        {
            #region FlushAllConnectionList
            var allConnectionList = RegisWebSocketProcess.GetConnectionRegisListFromSlug(channelSlugUrl);
            foreach (var connection in allConnectionList)
            {
                var FlushAllConnectionList = new ReceiveSocketDataModel
                {
                    ConnectionId = connection.ConnectionId,
                    ConnectionName = "",
                    MessageJson = new[] { JsonConvert.SerializeObject(allConnectionList.Select(s => s.ConnectionId)) },
                    InvokeMethodName = "FlushAllConnectionList"
                };
                await SendToClientWebSocket(connection.WebSocket, FlushAllConnectionList, CancellationToken.None);
            }
            #endregion
        }
        private static Task SendToClientWebSocket(WebSocket ws, ReceiveSocketDataModel data, CancellationToken cancellation)
        {
            var dataJson = JsonConvert.SerializeObject(data);
            var encoded = Encoding.UTF8.GetBytes(dataJson);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            return ws.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
        }

        public static Task SendToClientConnectionId(string channelSlugUrl, ReceiveSocketDataModel receivedData, CancellationToken? cancellation = null)
        {
            var dataJson = JsonConvert.SerializeObject(receivedData);
            var tarGetConnectionId = RegisWebSocketProcess.GetConnectionRegis(receivedData.ConnectionId, channelSlugUrl);
            var encoded = Encoding.UTF8.GetBytes(dataJson);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            return tarGetConnectionId.WebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation ?? CancellationToken.None);
        }

        private class InvokeProcess
        {
            private static bool IsAsyncMethod(MethodInfo method, string methodName)
            {
                Type attType = typeof(AsyncStateMachineAttribute);
                var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);
                return (attrib != null);
            }

            public static async Task<object> InvokeMethod(ConnectionSocketDataModel userConnectionData, ReceiveSocketDataModel receiveDataModel)
            {
                try
                {
                    var channel = GetChannelList().Where(w => w.ChannelSlugUrl == userConnectionData.ChannelSlugUrl).FirstOrDefault();

                    var mainTypeData = Type.GetType(channel.ChannelClassFullName);

                    var mainParamConstructor = SummonParameter(mainTypeData);
                    var mainConstructor = mainTypeData.GetConstructors().FirstOrDefault();
                    var mainConstructorDeclare = mainConstructor.Invoke(mainParamConstructor);

                    #region SetPropertieValueAfterNewInstance
                    var hubContext = mainTypeData.GetProperty(nameof(Hub.Context));

                    var hubContextInstance = Activator.CreateInstance(hubContext.PropertyType, userConnectionData.ConnectionId, userConnectionData.ChannelSlugUrl);
                    //mainConstructorDeclare is same a class varaible new instance
                    hubContext.SetValue(mainConstructorDeclare, hubContextInstance as HubContext);
                    #endregion

                    var mainMethod = mainTypeData.GetMethod(receiveDataModel.InvokeMethodName);

                    var IsNotReturn = (mainMethod.ReturnType == typeof(void) || mainMethod.ReturnType == typeof(Task));
                    object response = null;
                    if (IsAsyncMethod(mainMethod, receiveDataModel.InvokeMethodName))
                    {

                        // check invoke async
                        dynamic invokeAsync = mainMethod.Invoke(mainConstructorDeclare, receiveDataModel.MessageJson);
                        if (IsNotReturn)
                        {
                            await invokeAsync;
                        }
                        else
                        {
                            response = await invokeAsync;
                        }
                    }
                    else
                    {
                        if (IsNotReturn)
                        {
                            mainMethod.Invoke(mainConstructorDeclare, receiveDataModel.MessageJson);
                        }
                        else
                        {
                            response = mainMethod.Invoke(mainConstructorDeclare, receiveDataModel.MessageJson);
                        }
                    }
                    return response;

                }
                catch (Exception e)
                {
                    //TODO : log 
                    return e;
                }
            }
            private static IEnumerable<MethodInfo> GetAllInterfaceMethods(Type interfaceType)
            {
                var aInf = interfaceType.GetInterfaces();
                foreach (var parent in aInf)
                {
                    foreach (var parentMethod in GetAllInterfaceMethods(parent))
                    {
                        yield return parentMethod;
                    }
                }

                var aMth = interfaceType.GetMethods();
                foreach (var method in aMth)
                {
                    yield return method;
                }
            }

            private static object[] SummonParameter(Type classTypeData)
            {

                if (classTypeData == null)
                {
                    return null;
                }
                var constructorsOfType = classTypeData.GetConstructors();
                if (constructorsOfType.Length > 1)
                {
                    //TODO: Make is Support in future
                    throw new Exception($"Warning <Not Support> : Found more than one Constructor of this class { classTypeData.FullName }"
                                + "\n >> Please have only one constructor each class");
                }
                var firstConstructor = constructorsOfType.FirstOrDefault();
                var parametersInConstructor = firstConstructor.GetParameters();
                var result = new List<object>();
                foreach (var param in parametersInConstructor)
                {
                    var paramType = param.ParameterType;

                    //foreach (var method in GetAllInterfaceMethods(paramType))
                    //{

                    //}

                    if (paramType.IsInterface)
                    {





                        var implName = paramType.Name.Substring(1);
                        var implNamespace = paramType.Namespace.Split('.').LastOrDefault();
                        var implClassList = AppDomain.CurrentDomain.GetAssemblies()
                           .SelectMany(s => s.GetTypes())
                           .Where(w =>
                                (
                                    paramType.IsAssignableFrom(w)
                                ||
                                    (
                                     w.Name.Equals(implName)
                                     ||
                                     (
                                      !string.IsNullOrEmpty(w.Namespace)
                                      &&
                                      w.Namespace.Contains(implNamespace)
                                     )
                                    )
                                )
                                &&
                                !w.IsInterface
                                )
                            .ToList();
                        if (implClassList.Count > 1)
                        {
                            //TODO: Make is Support in future
                            throw new Exception("Warning <Not Support>: Found more than one implement class of interface "
                                + "\n >> Please extended interface to class only one class [< ClassNameImpl : IClassName >]");
                        }
                        var implClass = implClassList.FirstOrDefault();

                        var assemblyName = new AssemblyName(paramType.Namespace);
                        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                        var moduleBuilder = assemblyBuilder.DefineDynamicModule(paramType.Namespace);

                        var type = moduleBuilder.DefineType(
                               implClass.FullName,
                               TypeAttributes.Public,
                               typeof(Object),
                               new[] { paramType });
                        var method = type.DefineMethod(".ctor", System.Reflection.MethodAttributes.Public | System.Reflection.MethodAttributes.HideBySig);
                        foreach (var meInfo in paramType.GetMethods())
                        {
                            var methodAttributes =
                                  MethodAttributes.Public
                                | MethodAttributes.Virtual
                                | MethodAttributes.Final
                                | MethodAttributes.HideBySig
                                | MethodAttributes.NewSlot;

                            var parameters = meInfo.GetParameters();
                            var paramTypes = parameters.Select(p => p.ParameterType).ToArray();

                            var methodBuilder = type.DefineMethod(meInfo.Name, methodAttributes);
                             
                            methodBuilder.SetReturnType(meInfo.ReturnType);
                            methodBuilder.SetParameters(paramTypes);

                            // Sets the number of generic type parameters
                            var genericTypeNames =
                                paramTypes.Where(p => p.IsGenericParameter).Select(p => p.Name).Distinct().ToArray();

                            if (genericTypeNames.Any())
                            {
                                methodBuilder.DefineGenericParameters(genericTypeNames);
                            }
                        }


                        var parameteDatar = SummonParameter(implClass);

                        #region generic
                        if (implClass.IsGenericType || implClass.IsGenericTypeDefinition)
                        {

                            implClass = implClass.MakeGenericType(typeof(Contact), typeof(MsSqlGenericDb));
                        }
                        #endregion

                        var instanceOfImplement = (parameteDatar == null || parameteDatar.Length == 0)
                             ?
                             Activator.CreateInstance(implClass)
                             :
                             Activator.CreateInstance(implClass, parameteDatar);

                        result.Add(instanceOfImplement);
                    }
                    else
                    {
                        //TODO: Make is Support in future
                        throw new Exception("Warning <Not Support>: Shouldn't inject class to parameter."
                                + $"\n >> {paramType.FullName} > {param.Name} ");
                        //var injectNormalClass = Activator.CreateInstance(paramType);
                        //intfOfConstrucINJECT.Add(injectNormalClass);
                    }
                }
                return result.ToArray();
            }


        }

    }
}
