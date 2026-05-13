namespace TemplateProject.Application.Interfaces
{
    public interface IUserContext
    {
        Guid? GetCurrentUserId();
    }
}
