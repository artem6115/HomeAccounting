using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IRepository
    {
        Account AddAccountCommand(Account account);
        void EditAccountCommand(Account account);
        void DeleteAccountCommand(Account account);
        Account GetAccountQuery(long id);
        IEnumerable<Account> GetAllAccountsQuery();

        Category AddCategoryCommand(Category account);
        void EditCategoryCommand(Category account);
        void DeleteCategoryCommand(Category account);
        Category GetCategoryQuery(long id);
        IEnumerable<Category> GetAllCategoriesQuery();

        Transaction AddTransactionCommand(Transaction account);
        void EditTransactionCommand(Transaction account);
        void DeleteTransactionCommand(Transaction account);
        Transaction GetTransactionQuery(long id);
        IEnumerable<Transaction> GetAllTransactionsQuery();
        IEnumerable<Transaction> GetAllAccountTransactionsQuery(Account account);

    }
}
