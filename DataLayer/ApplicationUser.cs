using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    }
}
