using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pjs1.Main.PubSub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                ?? new ConnectionSocketDataModel
                {
                    ChannelSlugUrl = channelSlugUrl,
                    ConnectionId = connectionId,
                    WebSocket = webSocket
                };

            #region Reply Connecttion Id
            var replyConnectionData = new ReceiveSocketDataModel
            {
                ConnectionId = userConnectionData.ConnectionId,
                ConnectionName = "",
                MessageJson = new[] { "connection", "successful" },
                InvokeMethodName = "InitialConnection"
            };
            await SendToWebSocket(webSocket, replyConnectionData, CancellationToken.None);
            #endregion
            SetConnectionSocketList(userConnectionData);


            await FlushAllConnectionList(channelSlugUrl);

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
                        var invokeResultString = invokeResult.ToString();
                        var replySubscriptData = new ReceiveSocketDataModel
                        {
                            ConnectionId = receiveDataModel.ConnectionId,
                            ConnectionName = receiveDataModel.ConnectionName,
                            MessageJson = new[] { invokeResultString },
                            InvokeMethodName = receiveDataModel.InvokeMethodName
                        };
                        await SendToWebSocket(webSocket, replySubscriptData, CancellationToken.None);
                    }
                    // --//
                    #endregion
                }
                //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            RegisWebSocketProcess.RemoveConnectionSocket(userConnectionData.ConnectionId, channelSlugUrl);
            await FlushAllConnectionList(channelSlugUrl);
        }

        private static async Task FlushAllConnectionList(string channelSlugUrl)
        {
            #region FlushAllConnectionList
            var allConnectionList = RegisWebSocketProcess.GetConnectionSocketListFromSlug(channelSlugUrl);
            foreach (var connection in allConnectionList)
            {
                var FlushAllConnectionList = new ReceiveSocketDataModel
                {
                    ConnectionId = connection.ConnectionId,
                    ConnectionName = "",
                    MessageJson = new[] { JsonConvert.SerializeObject(allConnectionList.Select(s => s.ConnectionId)) },
                    InvokeMethodName = "FlushAllConnectionList"
                };
                await SendToWebSocket(connection.WebSocket, FlushAllConnectionList, CancellationToken.None);
            }
            #endregion
        }
        private static Task SendToWebSocket(WebSocket ws, ReceiveSocketDataModel data, CancellationToken cancellation)
        {
            var dataJson = JsonConvert.SerializeObject(data);
            var encoded = Encoding.UTF8.GetBytes(dataJson);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            return ws.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
        }

        public static Task SendToConnectionId(string channelSlugUrl, ReceiveSocketDataModel receivedData, CancellationToken? cancellation = null)
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
                    if (paramType.IsInterface)
                    {
                        var implClassList = AppDomain.CurrentDomain.GetAssemblies()
                           .SelectMany(s => s.GetTypes())
                           .Where(w => paramType.IsAssignableFrom(w) & !w.IsInterface).ToList();
                        if (implClassList.Count > 1)
                        {
                            //TODO: Make is Support in future
                            throw new Exception("Warning <Not Support>: Found more than one implement class of interface "
                                + "\n >> Please extended interface to class only one class [< ClassNameImpl : IClassName >]");
                        }
                        var implClass = implClassList.FirstOrDefault();

                        var parameteDatar = SummonParameter(implClass);

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
