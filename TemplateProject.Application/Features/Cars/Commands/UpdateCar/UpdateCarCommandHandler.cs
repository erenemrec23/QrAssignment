using AutoMapper;
using MediatR;
using TemplateProject.Application.Interfaces;
using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.Cars.Commands.UpdateCar
{
    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, Result>
    {
        private readonly ICarRepository  _carRepository;
        private readonly IMapper _mapper;

        private readonly IAppLocalizer _localizer;  
        public UpdateCarCommandHandler(ICarRepository carRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);
            var car = await _carRepository.GetByIdAsync(request.Id.Value, cancellationToken);
             
            if (car == null)
                throw new Exception(_localizer["Messages.CarNotFound"]);
             
            _mapper.Map(request, car);
             
            //await _carRepository.Update(car, cancellationToken);

            _carRepository.Update(car);

            return Result.Success();
        }
    }
}
