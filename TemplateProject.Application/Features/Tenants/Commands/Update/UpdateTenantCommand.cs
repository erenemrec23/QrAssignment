using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Tenants.Commands.Update
{
    public class UpdateTenantCommand : ICommand<Result<UpdateTenantResponse>>
    {
        public Guid? Id { get; set; }
        public required string Name { get; set; }
         
        public byte[] RowVersion { get; set; }
    }
}
