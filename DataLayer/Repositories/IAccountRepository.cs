using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IAccountRepository
    {
        Task<Account> Add(Account account);
        void Edit(Account account);
        void Delete(long id);
        Task<Account> Get(long id);
        Task<List<Account>> GetAll();

        //Наличие счета с таким наименованием
        bool CheckExistName(string name);




    }
}
