using MediatR;
using QrAssignment.Application.Features.QrLocations.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Users.Queries.GetList
{
    public class GetUserListQuery : IRequest<Result<List<AppUserListItemDto>>>;
}
