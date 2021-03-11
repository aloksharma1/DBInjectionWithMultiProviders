using DBInjectionWithMultiProviders.Abstractions;
using DBInjectionWithMultiProviders.Domain.Entities;
using DBInjectionWithMultiProviders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync([FromServices]IRepository<UserInfo> repository)
        {
            await repository.AddAsync(new UserInfo { 
                FullName="ALOK"
            });
            await repository.SaveChangesAsync().ConfigureAwait(false);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
