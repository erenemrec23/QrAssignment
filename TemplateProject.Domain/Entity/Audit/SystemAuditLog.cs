using System.Configuration;

namespace TemplateProject.Domain.Entities;

public class SystemAuditLog
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string TableName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string? PrimaryKey { get; set; }
    public string? OldValues { get; set; } 
    public string? NewValues { get; set; } 

    public string ColumnValues { get; set; }
    public string? UserId { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
}