using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Cars.Commands.UpdateCar
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
