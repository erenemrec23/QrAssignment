using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.Cars.Commands.UpdateCar
{
    public class UpdateCarCommand : ICommand<Result>
    {
        public Guid? Id { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Guid BrandId { get; set; }




        public byte[] RowVersion { get; set; }
    }
}
