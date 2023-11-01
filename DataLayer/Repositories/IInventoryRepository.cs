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
        Task<Inventory> GetLastBalance(long accountId, DateTime Date);
    }
}
