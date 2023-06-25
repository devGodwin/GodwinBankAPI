using AutoMapper;
using GodwinBankAPI.Data.AccountData;
using GodwinBankAPI.Data.TransactionData;
using GodwinBankAPI.Model.TransactionModel.RequestModel;
using GodwinBankAPI.Model.TransactionModel.ResponseModel;

namespace GodwinBankAPI.Mapping;

public class TransactionMapping:Profile
{
    public TransactionMapping()
    {
        CreateMap<Transaction, TransactionRequestModel>().ReverseMap();
        CreateMap<Transaction, TransactionResponseModel>().ReverseMap();
        CreateMap<Transaction, Account>().ReverseMap();
    }
}