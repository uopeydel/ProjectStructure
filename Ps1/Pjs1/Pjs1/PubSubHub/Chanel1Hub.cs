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


        public string Send(string text)
        {
            return $"Server send {text}.";
        }

    }
}
