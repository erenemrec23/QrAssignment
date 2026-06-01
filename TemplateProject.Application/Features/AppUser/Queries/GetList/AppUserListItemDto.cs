using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.AppUser.Queries.GetList
{
    public class AppUserListItemDto
    {
        public Guid? Id { get; set; }
        public required string FirstName { get; set; } 
        public required string LastName { get; set; } 
    }
}
