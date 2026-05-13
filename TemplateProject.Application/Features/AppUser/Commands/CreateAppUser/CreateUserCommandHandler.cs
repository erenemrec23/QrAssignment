using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Services;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.AppUser.Commands.CreateAppUser
{
    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Unit>>
    {
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _authService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
