using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateProject.Application.Features.Cars.Queries.GetList
{
    public class GetListCarResponse
    {
        public Guid? CarId { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string BrandName { get; set; } // SQL JOIN ile gelecek alan
        public byte[] BrandVersion { get; set; }
        public byte[] CarVersion { get; set; }
        public Guid BrandId { get; set; }
    }
}
