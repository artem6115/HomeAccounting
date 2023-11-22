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
        void Delete(long id);
        Task<Category>Get(long id);
        Task<List<Category>> GetAll();

        //Наличие категории с таким наименованием
        bool CheckExistName(string name);
    }
}
