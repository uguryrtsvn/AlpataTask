using AlpataBLL.Constants;
using AlpataEntities.Dtos.AuthDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Validations.AuthDtos
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "E-mail"))
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Şifre"))
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .MaximumLength(12).WithMessage("Şifre en fazla 12 karakter olmalıdır.");
        }
    }
}
