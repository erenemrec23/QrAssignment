using AutoMapper;
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Create
{
    public class CreateQrLocationCommandHandler : IRequestHandler<CreateQrLocationCommand, Result<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IQrLocationRepository _qrApplicantRepository;
        public CreateQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _qrApplicantRepository = qrLocationRepository;
        }

        public async Task<Result<Guid>> Handle(CreateQrLocationCommand request, CancellationToken cancellationToken)
        {

            var qrApplicant = _mapper.Map<QrLocation>(request);
            await _qrApplicantRepository.AddAsync(qrApplicant, cancellationToken);
            return Result.Success(qrApplicant.Id);
        }
    }
}
