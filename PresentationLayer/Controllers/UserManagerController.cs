using BusinessLayer.Services;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly EmailSenderService _emailService;
        private readonly ILogger<UserManagerController> _logger;
        private readonly IUserRepository _userRepository;

        public UserManagerController(IUserRepository userRepository,EmailSenderService sender, ILogger<UserManagerController> logger)
        {
            _emailService = sender;
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> GetEmail() => View("Email");

        [HttpPost]
        public async Task<IActionResult> SetEmail(string? email)
        {
            if(email == null)
            {
                if(await _userRepository.UserExist(email))
                {
                    return RedirectToAction("ResetPasswordPage", routeValues: new object[] { email });
                }
                _logger.LogWarning("Неудачная попытка создания аккаунта, этап проверки почты");
            }
            TempData["error"] = "Учетная запись на данную почту отсутвует";
            return RedirectToAction("GetEmail");
        }

        public async Task<IActionResult> ResetPasswordPage([DataType(DataType.EmailAddress)]string email)
        {
            if (!ModelState.IsValid) return RedirectToAction("GetEmail");
            _userRepository.AddSecretCode(email);
            return View("Password", email);
        }
        public async Task<IActionResult> ResetPasswordPage(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userRepository.VerifiCode(model.Email, model.Code))
                {
                    //Update password
                    _userRepository.DeleteCode(model.Email);
                }
                TempData["error"] = "Код не верный";
                return View("Password", model.Email);

            }

            TempData["error"] = "Пароль не корректный";
            return View("Password", model.Email);
        }

    }
}
