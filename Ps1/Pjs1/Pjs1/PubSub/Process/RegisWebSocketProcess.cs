using Pjs1.Main.PubSub.Models;
using System.Collections.Generic;


namespace Pjs1.Main.PubSub.Process
{
    internal sealed class RegisWebSocketProcess : MainChannelData
    {
        internal static void RegisWebSocket(ConnectionSocketDataModel regisData)
            => SetConnectionSocketList(regisData);
        internal static List<ConnectionSocketDataModel> GetConnectionRegisList
            => GetConnectionSocketList();
        internal static ConnectionSocketDataModel GetConnectionRegis(string ConnectionId, string ChannelSlugUrl)
            => GetConnectionSocket(ConnectionId, ChannelSlugUrl);
    }
}
