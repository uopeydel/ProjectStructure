using Pjs1.Main.PubSub.Models;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace Pjs1.Main.PubSub.Process
{
    internal sealed class RegisWebSocketProcess : MainChannelData
    {
        internal static ConnectionSocketDataModel RegisWebSocket(string channelSlugUrl, string connectionId, WebSocket webSocket)
            => SetConnectionSocketList(channelSlugUrl, connectionId, webSocket);
        internal static IEnumerable<ConnectionSocketDataModel> GetConnectionRegisList
            => GetConnectionSocketList();

        internal static IEnumerable<ConnectionSocketDataModel> GetConnectionRegisListFromSlug(string slug)
           => GetConnectionSocketListFromSlug(slug);

        internal static ConnectionSocketDataModel GetConnectionRegis(string ConnectionId, string ChannelSlugUrl)
            => GetConnectionSocket(ConnectionId, ChannelSlugUrl);


    }
}
