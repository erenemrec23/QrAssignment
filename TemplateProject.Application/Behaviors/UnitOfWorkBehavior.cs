using MediatR;
using TemplateProject.Application.Abstractions;

namespace TemplateProject.Application.Behaviors
{
    public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, ICommand<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        { 
            var response = await next();
             
            await _unitOfWork.SaveChangesAsync(cancellationToken);
             
            return response;
        }
    }
}
