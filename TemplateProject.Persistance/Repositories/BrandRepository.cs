
using Microsoft.EntityFrameworkCore;
using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity;
using TemplateProject.Persistance.Repositories;
using TemplateProject.Persistence.Context;

namespace TemplateProject.Persistence.Repositories;

internal sealed class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    public BrandRepository(AppDbContext context) : base(context)
    {
    }
}
