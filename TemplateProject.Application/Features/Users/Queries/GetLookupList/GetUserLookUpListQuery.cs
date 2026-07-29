using MediatR;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.GetLookupList
{
    public class GetUserLookUpListQuery : IRequest<Result<List<AppUserLookUpListItemDto>>>;
}
