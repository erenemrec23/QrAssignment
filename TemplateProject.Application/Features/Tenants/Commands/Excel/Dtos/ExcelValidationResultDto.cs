using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos 
{
    public class ExcelTenantRowResultDto
    {
        public int RowNumber { get; set; }
        public long? Code { get; set; }
        public string? Name { get; set; } 
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class ExcelValidationResponseDto
    {
        public List<ExcelTenantRowResultDto> Rows { get; set; } = new();
        public bool AllValid => Rows.All(r => r.IsValid);
    }
}
