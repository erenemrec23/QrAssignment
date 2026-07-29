using MediatR;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.GetById
{
    public class GetAppUserByIdQuery : IRequest<Result<AppUserItemDto>>
    {

        public GetAppUserByIdQuery(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }
    }
} 