using AutoMapper;
using CoreBanking.Services.Database.Models;

namespace CoreBanking.Services.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountModel, Account>();
            CreateMap<Account, AccountModel>();
        }
    }
}
