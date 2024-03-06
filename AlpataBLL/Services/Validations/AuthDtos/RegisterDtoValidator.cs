using AlpataBLL.Constants;
using AlpataEntities.Dtos.AuthDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.Validations.AuthDtos
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
           .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "E-mail address"))
           .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

            RuleFor(x => x.EmailConfirm)
                .Equal(x => x.Email).WithMessage("Email adresleri eşleşmiyor.")
              .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "EmailConfirm"));

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
            .MaximumLength(12).WithMessage("Şifre en fazla 12 karakter olmalıdır.");

            RuleFor(x => x.PasswordConfirm)
                .Equal(x => x.Password).WithMessage("Şifre ve şifre tekrarı eşleşmiyor.");


            RuleFor(x => x.Name)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Name"));
            RuleFor(x => x.Surname)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Surname"));
        }
    }
}
