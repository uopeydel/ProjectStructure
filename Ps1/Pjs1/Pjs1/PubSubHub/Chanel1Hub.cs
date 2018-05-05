using Pjs1.Main.PubSub;
using Pjs1.Main.PubSub.Models;
using Pjs1.Main.PubSub.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.PubSubHub
{
    public class Chanel1Hub : Hub
    {
        public Chanel1Hub()
        {

        }
        public override async Task OnConnectedAsync()
        {

        }

        public string Read(string text, long number)
        {
            return $"Server read {text}  {number}.";
        }

        public async Task<string> ReadAsync(string text, long number)
        {
            var result = await Task.Run(async () =>
            {
                await Task.Delay(5000);
                return $"Server ReadAsync {text}  {number}.";
            });

            return result;
        }

        public string Send(string text)
        {
            return $"Server send {text}.";
        }

        public async Task SendMessageToId(string text, string channelSlugUrl, string connectionId)
        {
            var data = new ReceiveSocketDataModel
            {
                ConnectionId = connectionId,
                MessageJson = new[] { text },
                InvokeMethodName = "SendMessageToId", //System.Reflection.MethodBase.GetCurrentMethod().Name

            };
            await EchoProcess.SendToConnectionId(channelSlugUrl, data);
        }

    }
}
