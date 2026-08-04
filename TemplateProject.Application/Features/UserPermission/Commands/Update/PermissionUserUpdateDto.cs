namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    public sealed record PermissionUserUpdateDto
    {
        public string? PageName { get; init; }   // sayfa hedefli (Page.PageKey)
        public string? GroupKey { get; init; }   // grup hedefli (MenuGroup.Key)
        public int PermissionValue { get; init; }
    }
}
