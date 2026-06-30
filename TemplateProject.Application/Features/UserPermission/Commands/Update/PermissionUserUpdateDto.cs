using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    public sealed record PermissionUserUpdateDto
    {
        public string PageName { get; init; } = string.Empty;

        public int PermissionValue { get; init; }
    }
}
