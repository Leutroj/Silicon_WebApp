using Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;
using AppContext = Infrastructure.Contexts.AppContext;

namespace WebApp.Controllers;

public class AuthController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly AppContext _context = context;

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (!await _context.Users.AnyAsync(x => x.Email == model.Email))
            {
                var userEntity = new UserEntity
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                if ((await _userManager.CreateAsync(userEntity, model.Password)).Succeeded)
                {
                    if ((await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false)).Succeeded)
                        return LocalRedirect("/");
                    else                   
                        return LocalRedirect("/signin");   
                }
                else
                {
                    ViewData["StatusMessage"] = "Something went wrong. Try again later or contact customer service";
                }
            }
            else
            {
                ViewData["StatusMessage"] = "User with the same email already exists";
            }
        }
        return View(model);
    }


    public IActionResult SignIn(string returnUrl)
    {


        ViewData["ReturnUrl"] = returnUrl ?? "/";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
    {
        if (ModelState.IsValid) 
        {
            if((await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.IsPresistent, false )).Succeeded)
                return LocalRedirect(returnUrl);
        }
        ViewData["ReturnUrl"] = returnUrl;    
        ViewData["StatusMEssage"] = "Incorrect Email or Password";
        return View(model);
    }

    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();    
        return RedirectToAction("Home", "Default");
    }
}
