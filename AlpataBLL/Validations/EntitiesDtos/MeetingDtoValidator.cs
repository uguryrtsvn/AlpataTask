using AlpataEntities.Dtos.MeetingDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Validations.EntitiesDtos
{
    public class MeetingDtoValidator:AbstractValidator<MeetingDto>
    {
        public MeetingDtoValidator()
        {
            RuleFor(z => z.StartTime)
            .NotEmpty().WithMessage("Toplantı başlangıç tarihi boş geçilemez")
            .LessThan(z => z.EndTime).WithMessage("Toplantı başlangıç tarihi, bitiş tarihinden önce olmalıdır");

            RuleFor(z => z.EndTime).NotEmpty().WithMessage("Toplantı bitiş tarihi boş geçilemez");
            RuleFor(z => z.Name).NotEmpty().WithMessage("Toplantı adı boş geçilemez.");
            RuleFor(z => z.Description).NotEmpty().WithMessage("Toplantı açıklaması boş geçilemez.");

        }
    }
}
