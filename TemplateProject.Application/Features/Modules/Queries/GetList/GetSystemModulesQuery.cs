// Application/Features/Modules/Queries/GetSystemModules/PageCatalogItemDto.cs
using MediatR;
using QrAssignment.Domain.Shared;
// GetSystemModulesQuery.cs
public sealed record GetSystemModulesQuery : IRequest<Result<List<PageCatalogItemDto>>>;
