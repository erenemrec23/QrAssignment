namespace QrAssignment.Application.Features.Permission.Queries.GetByUserId
{
    public class PermissionUserPageItemDto
    {

        public string PageName { get; init; } = string.Empty;

        public int PermissionValue { get; init; }
    }

    public class PermissionUserItemDto
    {
        public Guid? UserId { get; set; }
        public List<PermissionUserPageItemDto> PagePermissionList { get; set; } = new List<PermissionUserPageItemDto>();
    }



}
