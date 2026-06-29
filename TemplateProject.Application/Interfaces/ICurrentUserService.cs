namespace QrAssignment.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        IEnumerable<string> GetClaims(string claimType);
        string GetClaim(string claimType);
    }
}
