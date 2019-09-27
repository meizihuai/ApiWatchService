using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiWatchService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiWatchController : ControllerBase
    {

        [HttpGet]
        public NormalResponse GetAll()
        {
            return new NormalResponse(true, "", "",Module.ApiWatchInfos);
        }
    }
}