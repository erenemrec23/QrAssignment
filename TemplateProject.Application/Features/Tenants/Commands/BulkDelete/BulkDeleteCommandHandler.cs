using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.BulkDelete
{
    public class BulkDeleteCommandHandler : IRequestHandler<BulkDeleteTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository; 

        public BulkDeleteCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository; 
        }

        public async Task<Result> Handle(BulkDeleteTenantCommand request, CancellationToken cancellationToken)
        {  
            await _tenantRepository.BulkDelete(request.IdList, cancellationToken); 
            return Result.Success();
        }
    }
}