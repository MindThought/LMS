using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace LMS.Models
{
	public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "E-postadress är obligatorisk")]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required(ErrorMessage = "Kod är obligatorisk")]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Kom ihåg denna webbläsare?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "E-postadress är obligatorisk")]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-postadress är obligatorisk")]
        [Display(Name = "E-postadress")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatorisk")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [Display(Name = "Kommer ihåg mig?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Namn är obligatorisk")]
        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-postadress är obligatorisk")]
        [EmailAddress(ErrorMessage = "Ogiltig E-postadress")]
        [Display(Name = "E-postadress")]

        public string Email { get; set; }

        public string courseID { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatorisk")]
        [MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Ditt lösenord måste innehålla minst en symbol (!, @, #, Etc).", ErrorMessage = "Ditt lösenord måste vara 6 tecken långt och innehålla minst en symbol (!, @, #, Etc).", MinRequiredPasswordLength = 6)]

        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "Lösenordet och det bekräftade lösenord matchar inte.")]

        public string ConfirmPassword { get; set; }

    }

    public class ExitAccountViewModel
    {
        [Required(ErrorMessage = "E-postadress är obligatorisk")]
        [Display(Name = "E-postadress")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatorisk")]
        [StringLength(100, ErrorMessage = "Detta {0} måste vara minst {2} tecken långt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "E-postadress är obligatorisk")]
        [EmailAddress]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatorisk")]
        [StringLength(100, ErrorMessage = "Detta {0} måste vara minst {2} tecken långt.", MinimumLength = 6)]

        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "Lösenordet och det bekräftade lösenord matchar inte.")]

        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-postadress är obligatorisk")]
        [EmailAddress]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }
    }

    public class PasswordRequestViewModel
    {
        [Required(ErrorMessage = "Lösenord är obligatorisk")]
        [StringLength(100, ErrorMessage = "Detta {0} måste vara minst {2} tecken långt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
    }
}
