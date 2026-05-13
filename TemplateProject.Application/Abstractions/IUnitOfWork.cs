using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateProject.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
