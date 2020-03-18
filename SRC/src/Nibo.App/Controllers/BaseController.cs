using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nibo.Business.Interfaces;

namespace Nibo.App.Controllers
{
    public class BaseController : Controller
    {
        protected IBankRepository _bankRepository;
        protected IBankAccountRepository _bankAccountRepository;
        protected IBankStatementRepository _bankStatementRepository;
        protected IBankTransactionsRepository _bankTransactionsRepository;
        protected IMapper _mapper;
    }
}