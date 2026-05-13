using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateProject.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
