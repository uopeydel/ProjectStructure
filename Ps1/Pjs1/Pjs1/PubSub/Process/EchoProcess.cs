using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pjs1.Main.PubSub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
                ??
                new ConnectionSocketDataModel
                {
                    ChannelSlugUrl = channelSlugUrl,
                    ConnectionId = connectionId,
                    WebSocket = webSocket
                };


            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    //var chanelList = GetChannelList();
                    var content = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var receiveDataModel = JsonConvert.DeserializeObject<ReceiveSocketDataModel>(content);

                    // var typeVar = Type.GetType("SignalService.MyTestHub");
                    // TODO : "InvokeMethod
                    var invokeResult = InvokeProcess.InvokeMethod(userConnectionData, receiveDataModel);
                    //TODO : get this varaible [invokeResult] type  check before SendString()  // //  // Convert.ChangeType(mainValue, mainMethod.ReturnType) ;
                    var invokeResultString = invokeResult.ToString();
                    await SendString(webSocket, invokeResultString, CancellationToken.None);
                    // --//
                }
                //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private static Task SendString(WebSocket ws, string data, CancellationToken cancellation)
        {
            var encoded = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            return ws.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
        }

        private class InvokeProcess
        {
            public static object InvokeMethod(ConnectionSocketDataModel userConnectionData, ReceiveSocketDataModel receiveDataModel)
            {
                try
                {
                    var channel = GetChannelList().Where(w => w.ChannelSlugUrl == userConnectionData.ChannelSlugUrl).FirstOrDefault();

                    var mainTypeData = Type.GetType(channel.ChannelClassFullName);
                    var mainParamConstructor = SummonParameter(mainTypeData);
                    var mainConstructor = mainTypeData.GetConstructors().FirstOrDefault();
                    var mainConstructorDeclare = mainConstructor.Invoke(mainParamConstructor);
                    var mainMethod = mainTypeData.GetMethod(receiveDataModel.InvokeMethodName);


                    var mainValue = mainMethod.Invoke(mainConstructorDeclare, receiveDataModel.MessageJson);

                    return mainValue;

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
