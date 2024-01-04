using BusinessLayer.Services;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
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


        public UserManagerController(IUserRepository userRepository,EmailSenderService sender , ILogger<UserManagerController> logger)
        {
            _emailService = sender;
            _logger = logger;
            _userRepository = userRepository;
        }

       
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(model.Email == null) return View("InputEmail", model);

            if ( await _userRepository.UserExist(model.Email)) {
                if (model.Code == null)
                {
                    string code = await _userRepository.AddSecretCode(model.Email);
                    string url = $"https://localhost:7177/UserManager/ResetPassword?email={model.Email}&code={code}";
                    _emailService.Send(model.Email,$"Код востановления пароля: {code}\nТак же вы можете сменить пароль перейдя по ссылке:\n{url}","Востановление пароля");
                    return View("VerifiCode", model);
                }
                if(await _userRepository.VerifiCode(model.Email,model.Code)) { 

                    if(model.Password == null || !ModelState.IsValid)
                    {
                        return View("ResetPassword", model);
                    }
                    _userRepository.ResetPassword(model.Email, model.Password);
                    _logger.LogInformation("Password reset");

                    return RedirectToAction("Index","Home");
                }
                else
                {
                    TempData["error"] = "Код не верный";
                    return View("VerifiCode", model);
                }
            }
            TempData["error"] = "Учетная запись на данную почту отсутвует";
            return View("InputEmail", model);
        }
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            _userRepository.DeleteUser();
            return RedirectToAction("Index", "Home");
        }

    }
}
