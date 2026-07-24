using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
public class TenantNameUniquenessValidator : IExcelRowBusinessValidator<CreateTenantInputDto>
{
    private readonly ITenantRepository _tenantRepository;

    public TenantNameUniquenessValidator(ITenantRepository tenantRepository)
        => _tenantRepository = tenantRepository;

    public async Task ValidateAsync(
        IReadOnlyList<ExcelRowResultDto<CreateTenantInputDto>> rows,
        CancellationToken cancellationToken)
    {
        // Sadece hâlâ geçerli ve adı dolu olan satırları sorgula
        var candidates = rows
            .Where(r => r.IsValid && r.Data != null && !string.IsNullOrWhiteSpace(r.Data.Name))
            .ToList();

        if (candidates.Count == 0)
            return;

        var names = candidates
            .Where(w=> !w.Data!.Code!.HasValue)
            .Select(r => r.Data!.Name!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existing = await _tenantRepository.GetByNamesAsync(names, cancellationToken);

        var existingNames = new HashSet<string>(
            existing.Select(e => e.Name),
            StringComparer.OrdinalIgnoreCase);

        foreach (var row in candidates)
        {
            if (existingNames.Contains(row.Data!.Name!))
            {
                row.IsValid = false;
                row.Errors.Add($"'{row.Data.Name}' isimli firma zaten kayıtlı.");
            }
        }
    }
}