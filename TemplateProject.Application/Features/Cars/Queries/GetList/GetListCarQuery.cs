using MediatR;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Cars.Queries.GetList
{
    public class GetListCarQuery : IRequest<Result<List<GetListCarResponse>>>
    {
    }
}
