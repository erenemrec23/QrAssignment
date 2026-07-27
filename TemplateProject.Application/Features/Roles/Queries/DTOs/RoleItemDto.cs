
using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.Roles.DTOs
{
    public class RoleItemDto : BaseItemDto
    {
        public RoleItemDto() { }
        public RoleItemDto(Guid? id, string name)
        {
            Id = id;
            Name = name;
        }
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}