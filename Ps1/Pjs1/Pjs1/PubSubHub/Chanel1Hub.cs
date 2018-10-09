using Pjs1.Main.PubSub;
using Pjs1.Main.PubSub.Models;
using Pjs1.Main.PubSub.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.Models;
using Pjs1.Common.GenericDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
 

namespace Pjs1.Main.PubSubHub
{
    public class Chanel1Hub : Hub
    {
        
        public Chanel1Hub(IUserService u )
        {
             
        }
         
        public override async Task OnConnectedAsync()
        {
           
            const string methodName = nameof(this.OnConnectedAsync);
             
            //var options = new DbContextOptions<MsSqlGenericDb>();
            //try
            //{
            //    var coms = options.FindExtension<SqlServerOptionsExtension>().ConnectionString;
            //}
            //catch (Exception e)
            //{

            //}
            //using (var db = new MsSqlGenericDb(options))
            //{
            //    var res = await db.Role.Where(w => w.Id > 0).FirstOrDefaultAsync();
            //}

            #region Reply Connecttion Id
            var replyConnectionData = new ReceiveSocketDataModel
            {
                ConnectionId = Context.ConnectionId,
                ConnectionName = "", //todo set
                MessageJson = new object[] { RegisWebSocketProcess.GetConnectionRegisListFromSlug(Context.ChannelSlugUrl) },
                InvokeMethodName = methodName
            };
            await EchoProcess.SendToClientConnectionId(Context.ChannelSlugUrl, replyConnectionData);
            #endregion 

            //todo flush this to any client
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            
        }

        public string Read(string text, long number)
        {

            var response = $"Server read {text}  {number}  {Context?.ConnectionId}.";
            return response;
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
            await EchoProcess.SendToClientConnectionId(channelSlugUrl, data);
        }

        public async Task GetInterlocutorWithContact(string interlocutorIdText, string channelSlugUrl, string connectionId)
        {
            var isInt = int.TryParse(interlocutorIdText, out int interlocutorId);
            InterlocutorModel interlocutorWithContact = new InterlocutorModel();
            if (isInt)
            {
                //interlocutorWithContact = await _chatService.GetInterlocutorWithContact(interlocutorId);
            }

            
            var data = new ReceiveSocketDataModel
            {
                ConnectionId = connectionId,
                MessageJson = new[] { JsonConvert.SerializeObject(interlocutorWithContact) },
                InvokeMethodName = "GetInterlocutorWithContact", //System.Reflection.MethodBase.GetCurrentMethod().Name

            };
            await EchoProcess.SendToClientConnectionId(channelSlugUrl, data);
        }

    }
}
