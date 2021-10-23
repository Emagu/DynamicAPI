using Account.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        
        private readonly ILogger<AccountController> _logger;
        private readonly AccountService service;

        public AccountController(ILogger<AccountController> logger, AccountService accountService)
        {
            _logger = logger;
            this.service = accountService;
        }

        [HttpGet("Get")]
        public string Get()
        {
            return "AccountApi";
        }

        [HttpGet("List")]
        public IEnumerable<AccountService.Account> List()
        {
            return service.Storge;
        }

        [HttpPost]
        public bool Add(AccountService.Account data)
        {
            try
            {
                data.SN = service.Storge.Count + 1;
                service.Storge.Add(data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
