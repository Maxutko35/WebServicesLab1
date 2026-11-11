using FluentValidation;
using System;

namespace WebServicesLab1.Models
{
    public class ClientRegistrationValidator : AbstractValidator<ClientRegistrationModel>
    {
        public ClientRegistrationValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ім'я є обов'язковим.")
                .Length(2, 50).WithMessage("Ім'я повинно містити від 2 до 50 символів.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Прізвище є обов'язковим.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email є обов'язковим.")
                .EmailAddress().WithMessage("Невірний формат Email.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Номер телефону є обов'язковим.")
                .Matches(@"^\+380\d{9}$").WithMessage("Телефон повинен бути у форматі +380XXXXXXXXX.");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 120).WithMessage("Вік повинен бути від 18 до 120 років.");
            
            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today).When(x => x.DateOfBirth.HasValue)
                .WithMessage("Дата народження повинна бути в минулому.");

            RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage("Адреса не може перевищувати 100 символів.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим.")
                .MinimumLength(8).WithMessage("Пароль повинен містити щонайменше 8 символів.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Паролі не співпадають.");

            
            RuleFor(x => x.AgreeToTerms)
                .Equal(true).WithMessage("Ви повинні погодитися з умовами.");
        }
    }
}