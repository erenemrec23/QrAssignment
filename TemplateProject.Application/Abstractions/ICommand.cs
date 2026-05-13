using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateProject.Application.Abstractions
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
    public interface ICommand : IRequest<Unit>
    {
    }
}
