using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class AccountDetailsViewModel
{
	public AccountBasicInfo? BasicInfo { get; set; }	
	public AccountAddressInfo? AddressInfo { get; set; }
	public object Basic { get; internal set; }
	public object Address { get; internal set; }
}


public class AccountBasicInfo
{
	[Required]
	[Display(Name = "First Name", Prompt = "Enter your first name")]
	public string FirstName { get; set; } = null!;

	[Required]
	[Display(Name = "Last Name", Prompt = "Enter your last name")]

	public string LastName { get; set; } = null!;
	[Required]
	[Display(Name = "E-mail address", Prompt = "Enter your e-mail address")]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; } = null!;

	[Display(Name = "Phone", Prompt = "Enter your phonenumber")]
	public string? PhoneNumber { get; set; }

	[Display(Name = "Bio (Optional)", Prompt = "Add a short bio...")]
	public string? Bio { get; set; }
}

public class AccountAddressInfo
{
	[Required]
	[Display(Name = "Addressline 1", Prompt = "Enter your first address line")]
	public string Addressline_1 { get; set; } = null!;

	[Display(Name = "Addressline 2", Prompt = "Enter your second address line")]
	public string? Addressline_2 { get; set; }

	[Required]
	[Display(Name = "Postalcode", Prompt = "Enter your postal code")]
	public string PostalCode { get; set; } = null!;

	[Required]
	[Display(Name = "City", Prompt = "Enter your city")]
	public string City { get; set; } = null!;
}