using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Create
{

    public class CreateQrLocationCommand : ICommand<Result<Guid>>
    {

        public string Name { get; set; } 

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public string? LocationName { get; set; }

        public Guid? ParentLocationId { get; set; }


    }
}
