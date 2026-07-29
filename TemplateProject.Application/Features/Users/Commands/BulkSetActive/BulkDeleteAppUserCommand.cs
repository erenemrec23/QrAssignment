using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.BulkDelete
{
    public class BulkSetActiveAppUserCommand : IdListValidationBase, ICommand<Result>
    {
        public new required List<Guid> IdList { get; set; }
    }
}
