using MediatR;
using QrAssignment.Application.Features.QrLocations.Queries.GetList;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.AppUser.Queries.GetList
{
    public class GetListAppUserQuery : IRequest<Result<List<AppUserListItemDto>>>
    {
    }
}
