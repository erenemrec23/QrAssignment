namespace QrAssignment.Application.Features.QrLocations.Commands.Update
{
    public class UpdateQrLocationResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
         
        public byte[] RowVersion { get; set; }


    }
}
