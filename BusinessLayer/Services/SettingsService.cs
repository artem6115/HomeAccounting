using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SettingsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SettingsService> _logger;

        public SettingsService(UserManager<ApplicationUser> userManager, ILogger<SettingsService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Settings> Update(Settings settings)
        {
            throw new NotImplementedException();
        }
    }
}
