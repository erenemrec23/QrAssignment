using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Users.Queries.DTOs
{
    public class AppUserListItemDto
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}";  } }
        public string Email { get; set; }
    }
}
