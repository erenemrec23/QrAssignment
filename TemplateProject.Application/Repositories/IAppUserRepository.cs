using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Interfaces;
using TemplateProject.Domain.Entity;
using TemplateProject.Domain.Entity.App;

namespace TemplateProject.Application.Repositories
{

    public interface IAppUserRepository  
    {
        Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AppUser?> GetByEmailWithRefreshTokenAsync (string email, CancellationToken cancellationToken = default);
    }
}
