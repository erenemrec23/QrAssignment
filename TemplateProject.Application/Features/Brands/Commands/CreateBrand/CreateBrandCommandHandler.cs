using AutoMapper;
using MediatR;
using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.Brands.Commands.CreateBrand;

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