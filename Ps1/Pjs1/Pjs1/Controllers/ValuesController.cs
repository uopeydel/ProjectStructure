using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL.Models;

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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _userServ.AddUser(new User
            {
                Email = "test@test.com",
                FirstName = "testFirstName",
                LastName = "testLastName",
                UserName = "test"
            });

            var simple = await _userServ.GetUserAll();
            return Ok(simple);
        }

    }
}
