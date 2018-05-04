using System.Net.WebSockets; 

namespace Pjs1.Main.PubSub.Models
{
    //use when regis new websocket on start connection
    internal class ConnectionSocketDataModel
    {
        public WebSocket WebSocket { get; set; }
        public string ConnectionId { get; set; }
        public string ChannelSlugUrl { get; set; }
    }
}
