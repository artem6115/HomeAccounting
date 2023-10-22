using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ICategoryRepository
    {
        Task<Category> Add(Category account);
        void Edit(Category account);
        void Delete(Category account);
        Task<Category>Get(long id);
        Task<IEnumerable<Category>> GetAll();

    }
}
