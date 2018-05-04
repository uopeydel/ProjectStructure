using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.PubSub.Models
{
    //use when receive data when subscript / pub websocket
    public class ReceiveSocketDataModel
    {
        public string ConnectionId { get; set; }
        public string ConnectionName { get; set; }
        public object[] MessageJson { get; set; }
        public string InvokeMethodName { get; set; }
    }
}
