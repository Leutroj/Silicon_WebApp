using Infrastructure.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.ViewModels;
using AppContext = Infrastructure.Contexts.AppContext;

namespace WebApp.Controllers;

[Authorize]
public class AccountController(UserManager<UserEntity> userManager, AppContext context) : Controller
{

	private readonly UserManager<UserEntity> _userManager = userManager;
	private readonly AppContext _context = context;

	public object PostalCode { get; private set; }

	public async Task <IActionResult> Details()
	{

		var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);

		var viewModel = new AccountDetailsViewModel
		{
			Basic = new AccountBasicInfo
			{
				FirstName = user!.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				PhoneNumber = user.PhoneNumber,
				Bio = user.Bio,
			},

			Address = new AccountAddressInfo
			{
				Addressline_1 = user.Address?.AddressLine_1!,
				Addressline_2 = user.Address?.AddressLine_2!,	
				PostalCode = user.Address?.PostalCode!,
				City = user.Address?.City!
			}
		};
		return View(viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateBasicInfo(AccountDetailsViewModel model)
	{
		if (TryValidateModel(model.Basic!))
		{
			var user = await _userManager.GetUserAsync(User);
			if (user != null) 
			{ 
				user.FirstName = user.Basic!.FirstName;
				user.LastName = user.LastName;
				user.Email = user.Email;
				user.PhoneNumber = user.PhoneNumber;
				user.UserName= user.Email;
				user.Bio = user.Bio;

				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded) { TempData["StatusMessage"] = "Updated basic information."; }
					else
		{
			TempData["StatusMessage"] = "Unable to save basic information.";
		}

			}
		}
		else
		{
			TempData["StatusMessage"] = "Unable to save basic information.";
		}

		return RedirectToAction("Details", "Account");
	}

	[HttpPost]
	public async Task<IActionResult> UpdateAddressInfo(AccountDetailsViewModel model)
	{
		if (TryValidateModel(model.Address!))
		{
			var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);
			if (user != null)
			{
				try
				{
					if (user.Address != null)
					{
						user.Address.AddressLine_1 = model.Address!.AddressLine_1;
						user.Address.AddressLine_2 = model.Address!.AddressLine_2;
						user.Address.PostalCode = model.Address!.PostalCode;
						user.Address.City = model.Address!.City;
					}
					else
					{
						user.Address = new AddressEntity
						{
							AddressLine_1 = model.Address!.Addressline_1,
							AddressLine_2 = model.Address!.AddressLine_2,
							PostalCode = model.Address!.PostalCode,
							City = model.Address!.City,
						};
					}
					_context.Update(user);
					await _context.SaveChangesAsync();
					if (result.Succeeded) { TempData["StatusMessage"] = "Updated basic address information."; }
				}
				catch 
				{
					TempData["StatusMessage"] = "Unable to save basic address information.";
				}

			}
		}
		else
		{
			TempData["StatusMessage"] = "Unable to save basic information.";
		}

		return RedirectToAction("Details", "Account");
	}
}
