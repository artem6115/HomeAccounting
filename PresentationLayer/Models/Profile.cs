using AutoMapper;
using BusinessLayer.Models;
using DataLayer.Entities;

namespace PresentationLayer.Models
{
    public class MappintProfile : Profile
    {
        public MappintProfile()
        {
            CreateMap<AccountEditModel, Account>();
            CreateMap<CategoryEditModel, Category>();
            CreateMap<TransactionEditModel, Transaction>();

        }
    }
}
