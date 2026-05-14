namespace QrAssignment.Application.Features.QrLocations.Queries.GetList
{
    public class GetListQrLocationResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public string? LocationName { get; set; }

        public Guid? ParentLocationId { get; set; }
        public string? ParentLocationName { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
