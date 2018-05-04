using Pjs1.Main.PubSub;
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

    }
}
