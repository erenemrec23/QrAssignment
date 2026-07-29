using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.BulkDelete
{
    public class BulkDeleteAppRoleCommandHandler : IRequestHandler<BulkDeleteRoleCommand, Result>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public BulkDeleteAppRoleCommandHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result> Handle(BulkDeleteRoleCommand request, CancellationToken cancellationToken)
        {
            await _appRoleRepository.BulkDelete(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}