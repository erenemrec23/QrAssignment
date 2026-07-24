using MediatR;
using QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using MiniExcelLibs;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.Validate
{
    public class ValidateTenantExcelQueryHandler : IRequestHandler<ValidateTenantExcelQuery, Result<ExcelValidationResponseDto>>
    {
        private readonly ITenantRepository _tenantRepository;

        public ValidateTenantExcelQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<ExcelValidationResponseDto>> Handle(ValidateTenantExcelQuery request, CancellationToken cancellationToken)
        {
            if (request.FileBytes == null || request.FileBytes.Length == 0)
                return Result.Failure<ExcelValidationResponseDto>(new Error("Doğrulanacak dosya içeriği boş.","" ));

            var response = new ExcelValidationResponseDto();

            try
            {
                // Byte array'i stream'e dönüştürüp MiniExcel ile okuyoruz
                using (var stream = new MemoryStream(request.FileBytes))
                {
                    var rows = stream.Query(useHeaderRow: true).ToList();
                    int rowCounter = 1;

                    foreach (IDictionary<string, object> row in rows)
                    {
                        rowCounter++;

                        // Kolon isim güvenliği
                        var code = row.ContainsKey("code") ? long.Parse(row["code"]?.ToString()?.Trim()) : long.Parse(row.Values.FirstOrDefault()?.ToString()?.Trim());
                        var name = row.ContainsKey("name") ? row["name"]?.ToString().Trim() : row.Values.Skip(1)?.ToString()?.Trim();
                        
                        // Tamamen boş satırları listeye eklemeyelim
                        if (string.IsNullOrEmpty(name))
                            continue;

                        var rowResult = new ExcelTenantRowResultDto
                        {
                            RowNumber = rowCounter,
                            Code = code,
                            Name = name, 
                            IsValid = true
                        };

                        // --- İŞ DOĞRULAMA KURALLARI (VALIDATIONS) ---

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            rowResult.IsValid = false;
                            rowResult.ErrorMessage += "Firma adı boş olamaz. ";
                        }

                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            //var isExist = await _tenantRepository.AnyAsync(x => x.Name == name, cancellationToken);
                            //if (isExist)
                            //{
                            //    rowResult.IsValid = false;
                            //    rowResult.ErrorMessage += $"'{name}' isimli firma zaten kayıtlı. ";
                            //}
                        }

                        response.Rows.Add(rowResult);
                    }
                }

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<ExcelValidationResponseDto>(new Error($"Excel doğrulama sürecinde hata oluştu: {ex.Message}", ""));
            }
        }
    }
}
