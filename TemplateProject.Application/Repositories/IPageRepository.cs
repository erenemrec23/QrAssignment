namespace QrAssignment.Application.Repositories
{
    public interface IPageRepository
    {
        Task<List<PageCatalogItemDto>> GetCatalogAsync(CancellationToken ct = default);
    }
}
 