using CoreBanking.Business.Interface;
using CoreBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreBanking.Controllers
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
        public async Task<IActionResult> Get()
        {
            return Ok(_accountService.GetAccounts(await _accountService.GetAccounts(), await GetAddress()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountModel model)
        {
            var account = await _accountService.CreateAccount(model);

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

        //TODO : Update accounts
    }
}
