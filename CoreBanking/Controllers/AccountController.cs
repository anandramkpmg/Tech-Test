using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreBanking.Services.Business.Interface;
using CoreBanking.Services.Database.Enum;
using CoreBanking.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreBanking.Services.Controllers
{
    [ApiController, Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly string postcodeUrl = "https://randomuser.me/api/?nat=gb";
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet]
        [Route("GetAccountTypes")]
        public IActionResult GetAccountTypes()
        {
            var accountTypes =  Enum.GetNames(typeof(AccountType));
            return Ok(accountTypes);
        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts()
        {
            return Ok(_accountService.GetAccounts(await _accountService.GetAccounts(), await GetAddress()));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(AccountModel model)
        {
            var account = await _accountService.CreateAccount(model);

            return Created($"account/account/{account.Id}", account);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(AccountModel model)
        {
            var account = await _accountService.UpdateAccount(model);

            return Created($"account/{model.Id}", account);
        }

        private async Task<AddressModel> GetAddress()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(postcodeUrl))
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AddressModel>(json);
                }
            }
        }
    }
}
