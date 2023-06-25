using AutoMapper;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Model;
using GodwinBankAPI.Model.AccountModel.RequestModel;
using GodwinBankAPI.Model.AccountModel.ResponseModel;
using GodwinBankAPI.Model.TransactionModel.ResponseModel;

namespace GodwinBankAPI.Mapping;

public class AccountMapping:Profile
{
    public AccountMapping()
    {
        CreateMap<Account, AccountRequestModel>().ReverseMap();
        CreateMap<Account, AccountUpdateModel>().ReverseMap();
        CreateMap<Account, AccountResponseModel>().ReverseMap();
        CreateMap<Account, CachedAccount>().ReverseMap();
        CreateMap<Account, ResetPasswordRequestModel>().ReverseMap();
        CreateMap<Account, TransactionResponseModel>().ReverseMap();
    }
}