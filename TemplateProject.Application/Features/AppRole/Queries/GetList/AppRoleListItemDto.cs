namespace QrAssignment.Application.Features.AppRole.Queries.GetList
{
    // DTO (Client'a tüm AppRole nesnesini değil, sadece gerekenleri dönmek için)
    public sealed record AppRoleListItemDto(string Id, string Name);
}