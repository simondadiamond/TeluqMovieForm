using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TeluqMovieForm.Models;

public class MovieRegistrationModel
{
    private string _email = string.Empty;
    private string? _postalCode;

    [Required(ErrorMessage = "Ce champ est requis.")]
    [Url(ErrorMessage = "Le lien doit être une URL valide.")]
    [StringLength(200, ErrorMessage = "L'URL est trop longue.")]
    public string MovieUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ce champ est requis.")]
    [StringLength(50, ErrorMessage = "La longueur maximale est de 50 caractères.")]
    [RegularExpression(@"^[a-zA-ZàâäéèêëïîôöùûüçÀÂÄÉÈÊËÏÎÔÖÙÛÜÇ\s'-]+$", ErrorMessage = "Caractères invalides détectés.")]
    public string ApplicantLastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ce champ est requis.")]
    [StringLength(50, ErrorMessage = "La longueur maximale est de 50 caractères.")]
    [RegularExpression(@"^[a-zA-ZàâäéèêëïîôöùûüçÀÂÄÉÈÊËÏÎÔÖÙÛÜÇ\s'-]+$", ErrorMessage = "Caractères invalides détectés.")]
    public string ApplicantFirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ce champ est requis.")]
    [EmailAddress(ErrorMessage = "Adresse courriel invalide.")]
    [StringLength(100, ErrorMessage = "L'adresse courriel est trop longue.")]
    public string Email
    {
        get => _email;
        set => _email = value?.Replace(" ", "").Trim().ToLower() ?? string.Empty;
    }

    [CanadianPhone(ErrorMessage = "Le numéro de téléphone est invalide.")]
    public string? PhoneNumber { get; set; }

    [CanadianPostalCode(ErrorMessage = "Le code postal est invalide.")]
    public string? PostalCode
    {
        get
        {
            if (string.IsNullOrEmpty(_postalCode))
            {
                return _postalCode;
            }

            string sanitized = _postalCode.Replace(" ", string.Empty).ToUpper();

            if (sanitized.Length != 6)
            {
                return _postalCode;
            }

            return $"{sanitized.Substring(0, 3)} {sanitized.Substring(3)}";
        }
        set => _postalCode = value?.ToUpper();
    }
    

    [Required(ErrorMessage = "Ce champ est requis.")]
    [OddNumber(ErrorMessage = "Le nombre doit être impair.")]
    public int? OddNumber { get; set; }

    public class CanadianPostalCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string postalCode || string.IsNullOrWhiteSpace(postalCode))
            {
                return ValidationResult.Success;
            }

            if (Regex.IsMatch(postalCode, @"^[A-Za-z]\d[A-Za-z][ ]?\d[A-Za-z]\d$"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName! });
        }
    }
    public class OddNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Allow null - let [Required] handle that
            if (value is null)
            {
                return ValidationResult.Success;
            }

            // Validate the actual value
            if (value is int number)
            {
                if (number == 0 || number % 2 == 0)
                {
                    return new ValidationResult(ErrorMessage ?? "Le nombre doit être impair.", new[] { validationContext.MemberName! });
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Format invalide.", new[] { validationContext.MemberName! });
        }
    }
    public class CanadianPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string phoneNumber || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return ValidationResult.Success;
            }

            if (!Regex.IsMatch(phoneNumber, @"^[\d\s\-\(\)]+$"))
            {
                return new ValidationResult("Caractères invalides. Utilisez seulement chiffres, espaces, tirets ou parenthèses.", new[] { validationContext.MemberName! });
            }

            string digits = Regex.Replace(phoneNumber, @"\D", "");

            if (digits.Length != 10)
            {
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName! });
            }

            if (digits[0] == '0' || digits[0] == '1')
            {
                return new ValidationResult("L'indicatif régional ne peut pas commencer par 0 ou 1.", new[] { validationContext.MemberName! });
            }
            
            if (digits[3] == '0' || digits[3] == '1')
            {
                return new ValidationResult("Le numéro local ne peut pas commencer par 0 ou 1.", new[] { validationContext.MemberName! });
            }
    
            return ValidationResult.Success;
        }
    }

}
