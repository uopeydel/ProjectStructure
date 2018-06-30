using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pjs1.Main.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }


        [HttpPost("CreateInterlocutor")]
        public async Task<IActionResult> CreateInterlocutor([FromBody]Interlocutor interlocutor)
        {
            interlocutor.InterlocutorType = InterlocutorType.User;
            await _chatService.CreateInterlocutor(interlocutor);
            return Ok();
        }

        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact([FromBody]Contact contact)
        {
            contact.ReceiverStatus = ContactStatus.Contacted;
            contact.SenderStatus = ContactStatus.Contacted;
            contact.ActionTime = DateTimeOffset.Now;
            await _chatService.AddContact(contact);
            return Ok();
        }

        [HttpGet("GetInterlocutorWithContact/{interlocutorId}")]
        public async Task<IActionResult> GetInterlocutorWithContact(int interlocutorId)
        {
            return Ok(await _chatService.GetInterlocutorWithContact(interlocutorId));

        }
    }
}
