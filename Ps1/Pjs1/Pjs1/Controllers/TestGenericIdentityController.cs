using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.Enums;
using Pjs1.Common.GenericDbContext;

namespace Pjs1.Main.Controllers
{
    [Route("api/[controller]")]
    public class TestGenericIdentityController : Controller
    {
        private readonly ITestGenericIdentityService _testGenericIdentityService;
        public TestGenericIdentityController(ITestGenericIdentityService testGenericIdentityService)
        {
            _testGenericIdentityService = testGenericIdentityService;
        }

        // GET: api/TestGenericIdentity
        [HttpGet]
        public async Task<IActionResult> Get()
        { 
            Random _r = new Random();
            var resultRandom = _r.Next(9005);
            var em = $"test{resultRandom}@email.com";
            var mock = new GenericUser
            {
                OnlineStatus = UserOnlineStatus.Online,
                Email = em,
                FirstName = "myFirstName",
                LastName = "myTestLastName",
                UserName = em, 
            };
            var result = await _testGenericIdentityService.TestUser(mock);
            return Ok(result);
        }


        // api/TestGenericIdentity/test
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> test()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();

            var result = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                    //.Where(x => x.CustomAttributes.Any(a => !a.AttributeType.Name.Equals(nameof(AllowAnonymousAttribute))))
                .SelectMany(type => type.GetMethods()).Where(method => method.IsPublic).ToList()
                /*.Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))*/;
            var allName = result.Select(s => s.Name).ToArray();
            return Ok(result);
        }



    }
}
