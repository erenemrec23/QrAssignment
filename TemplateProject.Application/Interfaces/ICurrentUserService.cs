namespace QrAssignment.Application.Interfaces
{
    public interface ICurrentUserService
    { 
        IEnumerable<string> GetClaims(string claimType);
        string GetClaim(string claimType);


        string? UserId { get; }
        IReadOnlyDictionary<string, int> GetPagePermissions();
    }
}
