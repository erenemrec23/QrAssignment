using MediatR;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

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
