using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetById
{

    public class QrLocationGetByIdQuery : IRequest<Result<List<QrLocationItemGetByIdDto>>>
    {

        public Guid Id { get; set; }

        public QrLocationGetByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
