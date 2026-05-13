using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.Cars.Queries.GetList
{
    public class GetListCarQuery : IRequest<Result<List<GetListCarResponse>>>
    {
    }
}
