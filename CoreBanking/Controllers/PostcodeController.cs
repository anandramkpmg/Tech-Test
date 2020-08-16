using CoreBanking.Business.Interface;
using CoreBanking.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBanking.Controllers
{
    [ApiController, Route("[controller]")]
    public class PostcodeController : ControllerBase
    {
        private readonly IPostcodeService postcodeService;

        public PostcodeController(IPostcodeService postcodeService)
        {
            this.postcodeService = postcodeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var address = await postcodeService.GetAddress();
            return Ok(address);
        }

        //[HttpGet]
        //public async Task<AddressModel> Get()
        //{
        //    return await postcodeService.GetAddress();
        //}
    }
}
