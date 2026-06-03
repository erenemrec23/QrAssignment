using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.AppUser.Queries.GetList
{
    public class AppUserListItemDto
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
    }
}
