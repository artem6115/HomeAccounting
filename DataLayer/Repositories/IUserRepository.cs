namespace DataLayer.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExist(string name);
        Task<string> AddSecretCode(string name);
        Task<bool> VerifiCode(string name, string code);
        void DeleteCode(string name);
        void ResetPassword(string name,string password);
        void DeleteUser();
    }
}