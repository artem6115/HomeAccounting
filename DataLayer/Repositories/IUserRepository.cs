namespace DataLayer.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExist(string name);
        void AddSecretCode(string name);
        Task<bool> VerifiCode(string name, string code);
        void DeleteCode(string name);

    }
}