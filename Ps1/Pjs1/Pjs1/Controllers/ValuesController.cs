﻿using Microsoft.AspNetCore.Mvc;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.Enums;
using Pjs1.Common.GenericDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.Controllers
{
    [Route("api/Values")]
    public class ValuesController : Controller
    {
        private readonly IUserService _userServ;
        public ValuesController(IUserService userServ)
        {
            _userServ = userServ;
        }
        [HttpGet("test")]
        public async Task<IActionResult> tests() {
            await _userServ.TestGenericEf();
            return Ok( );
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var re = await _userServ.AddUser(new GenericUser
            {
                Email = "test@test.com",
                FirstName = "testFirstName",
                LastName = "testLastName",
                UserName = "test",
                OnlineStatus = UserOnlineStatus.Online
            });

            var simple = await _userServ.GetUserAll();
            return Ok(simple);
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var simple = await _userServ.GetUserAll();
            return Ok(simple);
        }

        [HttpGet("UpdateSomePropNotWork")]
        public async Task<IActionResult> UpdateSomeProp()
        {
            var simple = await _userServ.UpdateUserSomeProperties(null);
            return Ok(simple);
        }

        [HttpGet("UpdateSomePropWork")]
        public async Task<IActionResult> UpdateSomePropWork()
        {
            var simple = await _userServ.UpdateUserSomePropertiesWork(null);
            return Ok(simple);
        }

        [HttpGet("UpdateOnlineStatus")]
        public async Task<IActionResult> UpdateOnlineStatus(UserOnlineStatus onlneStatus)
        {
            var simple = await _userServ.UpdateOnlineStatus(onlneStatus);
            return Ok(simple);
        }

        [HttpGet("UpdateOnlineStatusMultiType")]
        public async Task<IActionResult> UpdateOnlineStatusMultiType(UserOnlineStatus onlneStatus, string lastName)
        {
            var simple = await _userServ.UpdateOnlineStatusMultiType(onlneStatus, lastName);
            return Ok(simple);
        }


        [HttpGet("ReturnError")]
        public async Task<IActionResult> ReturnError()
        {

            return Ok();
        }
    }
}
