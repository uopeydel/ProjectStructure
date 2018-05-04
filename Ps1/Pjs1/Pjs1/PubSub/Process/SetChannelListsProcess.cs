using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.PubSub.Process
{
    internal sealed class SetChannelListsProcess : MainChannelData
    {
        internal static void SetChannelData(string channelClassFullName, string channelSlugUrl)
            => SetChannelList(channelClassFullName, channelSlugUrl);
    }
}
