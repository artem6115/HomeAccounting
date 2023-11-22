using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> GetAccountInventories(long accountId);
        Task<Inventory> Get(long Id);
        Task<Inventory> Add(Inventory inventory);
        void Delete(long Id);
        Task<Inventory> GetLastInventory(long accountId, DateTime ByDate);
        Task RebuildInventories(long accountId, DateTime EditTransactionDate, double differenceValue);

        //Наличие инвенторизации на указаную дату (секунды и млс не учитываются)
        bool CheckExistData(DateTime date);
    }
}
