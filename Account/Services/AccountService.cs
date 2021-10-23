using BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Services
{
    public class AccountService : SingletonService
    {
        public class Account
        {
            public int SN { get; set; }
            public string Name { get; set; }
        }
        public List<Account> Storge { get; set; } = new List<Account>();
    }
}
