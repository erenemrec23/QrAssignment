using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetById
{

    public class GetQrLocationByIdQuery : IRequest<Result<List<QrLocationItemGetByIdDto>>>
    {

        public Guid Id { get; set; }

        public GetQrLocationByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
