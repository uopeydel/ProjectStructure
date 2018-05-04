using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.PubSub
{
    public class Hub : IDisposable
    {
        public Hub() { }
        public void Dispose() { }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task OnConnectedAsync() { }
        public virtual async Task OnDisconnectedAsync(Exception exception) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
