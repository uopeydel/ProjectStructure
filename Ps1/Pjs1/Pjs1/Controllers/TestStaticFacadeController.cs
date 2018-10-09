using Microsoft.AspNetCore.Mvc;
using Pjs1.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.Controllers
{
    [Route("api/TestStaticFacade")]
    public class TestStaticFacadeController : Controller
    {
        IStaticFacade _staticFacade;
        public TestStaticFacadeController(IStaticFacade staticFacade)
        {
            _staticFacade = staticFacade;

        }


        [HttpGet]
        [Route("stFacade/{id}")]
        public async Task< IActionResult> Get([FromQuery]int id)
        {
            IGenericEFRepository
            return Ok(await _staticFacade.stFacade(id));
        }


    }
}
