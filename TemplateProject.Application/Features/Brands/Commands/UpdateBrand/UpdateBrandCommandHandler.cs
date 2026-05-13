using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Exceptions;

namespace QrAssignment.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Result>
    {
        private readonly IBrandRepository _brandRepository; // Veritabanı kapısı
        private readonly IMapper _mapper;
        private readonly IAppLocalizer _localizer;
        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new BusinessException(_localizer["Messages.IdIsNull"]);

            var brand = await _brandRepository.GetByIdAsync(request.Id.Value, cancellationToken);
             
            if (brand == null)
                throw new BusinessException(_localizer["Messages.BrandNotFound"]);

            _mapper.Map(request, brand);

            _brandRepository.Update(brand); 

            return Result.Success();
        }
    }
}
