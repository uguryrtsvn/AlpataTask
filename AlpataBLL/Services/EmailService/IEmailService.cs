using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.EmailService
{
    public interface IEmailService
    {
        Task<IBaseResult> SendEmail(EmailModel EmailRequestModel);

    }
}
