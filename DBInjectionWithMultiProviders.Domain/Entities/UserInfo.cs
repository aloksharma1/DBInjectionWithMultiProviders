using System;
using System.Collections.Generic;
using System.Text;

namespace DBInjectionWithMultiProviders.Domain.Entities
{
    public class UserInfo : BaseEntity
    {
        public UserInfo()
        {
            UserAddresses = new List<UserAddress>();
        }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<UserAddress> UserAddresses { get; set; }
    }
}
