using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppUser.Queries.GetById
{
    public class GetByIdAppUserQuery : IRequest<Result<AppUserItemDto>>, ISecuredRequest
    {

        public string PageName => "Page_AppUsers";
        public PagePermissions RequiredPermission => PagePermissions.View;
        public GetByIdAppUserQuery(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }
    }
} 