using AutoMapper;
using CoreBanking.Database.Models;

namespace CoreBanking.Models.Mapper
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
