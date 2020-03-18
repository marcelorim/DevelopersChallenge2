using AutoMapper;
using Nibo.App.ViewModels;
using Nibo.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nibo.App.Automapper
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<BankAccount, BankAccountViewModel>().ReverseMap();
            CreateMap<BankTransactions, BankTransactionsViewModel>().ReverseMap();
            CreateMap<Bank, BankViewModel>().ReverseMap();
            CreateMap<BankStatement, BankStatementViewModel>().ReverseMap();

        }
    }
}
