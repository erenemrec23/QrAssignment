using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.Roles.Queries.GetList
{
    // DTO (Client'a tüm AppRole nesnesini değil, sadece gerekenleri dönmek için)
    public class RoleListItemDto : BaseListItemDto 
    {
        public RoleListItemDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; set; }
        public string  Name { get; set; }

    }
}