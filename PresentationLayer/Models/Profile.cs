using AutoMapper;
using DataLayer.Entities;

namespace PresentationLayer.Models
{
    public class MappintProfile : Profile
    {
        public MappintProfile()
        {
            CreateMap<AccountEditModel, Account>().ReverseMap();
            CreateMap<CategoryEditModel, Category>().ReverseMap();
            CreateMap<TransactionEditModel, Transaction>().ReverseMap();

        }
    }
}
