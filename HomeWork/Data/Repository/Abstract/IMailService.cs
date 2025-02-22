namespace HomeWork.Data.Repository.Abstract
{
    public interface IMailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
