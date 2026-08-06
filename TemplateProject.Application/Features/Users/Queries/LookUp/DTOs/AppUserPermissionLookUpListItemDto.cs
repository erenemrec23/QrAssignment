namespace QrAssignment.Application.Features.Users.Queries.LookUp.DTOs
{
    public class PermissionLookUpListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool HasPermission { get; set; }
    }
}
