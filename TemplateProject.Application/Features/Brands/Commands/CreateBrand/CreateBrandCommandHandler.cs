using AutoMapper;
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Result<Guid>>
{
    private readonly IBrandRepository _brandRepository; // Veritabanı kapısı
    private readonly IMapper _mapper;
    public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = _mapper.Map<Brand>(request);
        await _brandRepository.AddAsync(brand, cancellationToken);

        return Result.Success(brand.Id);
    }
}