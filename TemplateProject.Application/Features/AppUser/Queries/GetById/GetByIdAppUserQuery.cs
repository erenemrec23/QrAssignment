using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppUser.Queries.GetById
{
    public class GetByIdAppUserQuery : IRequest<Result<AppUserItemDto>>
    {

        public GetByIdAppUserQuery(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }
    }
} 