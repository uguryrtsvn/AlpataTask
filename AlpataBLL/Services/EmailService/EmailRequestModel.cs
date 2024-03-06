using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.EmailService
{
    public class EmailRequestModel
    {
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }     
}
