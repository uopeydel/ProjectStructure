using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.PubSub
{
    public class HubContext
    {
        public HubContext(string connectionId , string channelSlugUrl) {
            ConnectionId = connectionId;
            ChannelSlugUrl = channelSlugUrl;
        }
        public string ConnectionId { get; }
        public string ChannelSlugUrl { get; }
    }
}
