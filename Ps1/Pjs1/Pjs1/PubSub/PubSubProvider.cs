using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pjs1.Main.PubSub.Models;
using Pjs1.Main.PubSub.Process;

namespace Pjs1.Main.PubSub
{
    public static class PubSubProvider
    {
        private static List<ChannelModel> ChannelLists { get; set; }
        public static IApplicationBuilder UsePubSubProvider(this IApplicationBuilder app, Action<SingUpChannel> singOnChannel)
        {
            singOnChannel(new SingUpChannel());
            app.Use(AcceptHub);
            return app;
        }

        private static async Task AcceptHub(HttpContext context, Func<Task> next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                #region subscript
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await EchoProcess.Echo(context, webSocket);
                #endregion
            }
            else
            {
                await next();
            }
        }

        #region DoInvokeMethod Have no use just sample

        private class DoInvokeMethod
        {
            public void TryInvoke()
            {
                Type magicType3 = Type.GetType("ConsoleApp2.Point");
                ConstructorInfo magicConstructor2 = magicType3.GetConstructor(Type.EmptyTypes);

                Type magicType2 = Type.GetType("ConsoleApp2.Point");
                Type magicType = Type.GetType("ConsoleApp2.Point");
                Type[] types = new Type[2];
                types[0] = typeof(string);
                types[1] = typeof(string);
                ConstructorInfo magicConstructor = magicType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public, null,
                    CallingConventions.HasThis, types, null);

                object magicClassObject = magicConstructor.Invoke(new object[] { "1", "2" });

                // Get the ItsMagic method and invoke with a parameter value of 100

                MethodInfo magicMethod = magicType.GetMethod("Getss");
                object magicValue = magicMethod.Invoke(magicClassObject, new object[] { "100" });

            }
        }


        #endregion
    }

    public class SingUpChannel
    {
        public void MapChannel<TChannelClassName>(string channelSlugUrl) where TChannelClassName : Hub
        {
            var channelClassFullName = typeof(TChannelClassName).FullName;
            SetChannelListsProcess.SetChannelData(channelClassFullName, channelSlugUrl);
        }
    }

    internal class MainChannelData
    {
        protected MainChannelData() { }

        #region ChannelList 
        private static readonly List<ChannelModel> ChannelList = new List<ChannelModel>();

        protected static void SetChannelList(string channelClassFullName, string channelSlugUrl)
        {
            ChannelList.Add(new ChannelModel { ChannelClassFullName = channelClassFullName, ChannelSlugUrl = channelSlugUrl });
        }

        protected static List<ChannelModel> GetChannelList()
        {
            return ChannelList;
        }
        #endregion

        #region ConnectionSocketList
        private static readonly List<ConnectionSocketDataModel> ConnectionSocketList = new List<ConnectionSocketDataModel>();
        protected static void SetConnectionSocketList(ConnectionSocketDataModel regisData)
        {
            ConnectionSocketList.Add(regisData);
        }

        protected static IEnumerable<ConnectionSocketDataModel> GetConnectionSocketList()
        {
            return ConnectionSocketList;
        }

        protected static IEnumerable<ConnectionSocketDataModel> GetConnectionSocketListFromSlug(string slug)
        {
            return ConnectionSocketList.Where(w => w.ChannelSlugUrl.Equals(slug));
        }

        protected static ConnectionSocketDataModel GetConnectionSocket(string ConnectionId, string ChannelSlugUrl)
        {
            return ConnectionSocketList.Where(w => w.ConnectionId == ConnectionId && w.ChannelSlugUrl == ChannelSlugUrl).FirstOrDefault();
        }

        protected static void RemoveConnectionSocket(string ConnectionId, string ChannelSlugUrl)
        {
            ConnectionSocketList.Remove(ConnectionSocketList.FirstOrDefault(w => w.ConnectionId == ConnectionId && w.ChannelSlugUrl == ChannelSlugUrl));
        }
        #endregion
    }


    public class MyTestHub : Hub
    {
        public MyTestHub()
        {

        }
        public override async Task OnConnectedAsync()
        {

        }

        public string Read(string text, long number)
        {
            return $"Server read {text}  {number}.";
        }


        public string Send(string text)
        {
            return $"Server send {text}.";
        }

    }

}
