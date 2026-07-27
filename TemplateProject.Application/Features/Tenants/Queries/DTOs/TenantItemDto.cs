
using Microsoft.VisualBasic;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;

namespace QrAssignment.Application.Features.Tenants.DTOs
{
    public class TenantItemDto : TenantListItemDto
    {
        public TenantItemDto(){ }


        public TenantItemDto(Guid? id, string name, long revNum, string modifiedUserFullName, byte[] rowVersion)
        {

            Id = id;
            Name = name;
            RevNum = revNum;
            ModifiedUserFullName = modifiedUserFullName;
            RowVersion = rowVersion;
        }

        public byte[] RowVersion { get; set; }
    }
}
 