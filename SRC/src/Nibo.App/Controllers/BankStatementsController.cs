using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nibo.App.ViewModels;
using Nibo.Business;
using Nibo.Business.Interfaces;
using Nibo.Business.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nibo.App.Controllers
{
    public class BankStatementsController : BaseController
    {
        protected ConverterRules ConverterRules = new ConverterRules();
        private IHostingEnvironment _env;

        bool includeBankStatementData = false;
        Guid bankStatementId;

        public BankStatementsController(IBankStatementRepository bankStatementRepository, IBankTransactionsRepository bankTransactionsRepository, 
                                        IBankRepository bankRepository, IBankAccountRepository bankAccountRepository, IHostingEnvironment env, IMapper mapper)
        {
            _bankRepository = bankRepository;
            _bankAccountRepository = bankAccountRepository;
            _bankStatementRepository = bankStatementRepository;
            _bankTransactionsRepository = bankTransactionsRepository;
            _env = env;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FileUploadAsync()
        {
            try
            {
                IFormFileCollection files = HttpContext.Request.Form.Files;

                if (files.Count() == 0)
                    throw new Exception("Please attach the files.");

                BankStatement importFile = await UploadArquivo(files);
                if (importFile == null)
                    throw new Exception("Error importing file.");

                var bankStatement = _mapper.Map<BankStatementViewModel>(importFile);

                return View("BankStatements", bankStatement);
            }
            catch (Exception ex)
            {
                ViewData["MessageError"] = ex.Message;
            }

            return View("Index");
        }

        private async Task<BankStatement> UploadArquivo(IFormFileCollection fileCollection)
        {
            if (fileCollection.Count == 0)
                return null;

            BankStatement bankStatementEntity = new BankStatement();
            List<BankTransactions> transactions = new List<BankTransactions>();
            var webRoot = _env.WebRootPath;
            var uploadPath = Path.Combine(webRoot, "OFX");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var file in fileCollection)
            {
                if (file != null && file.Length > 0)
                {
                    var currentFile = file;
                    if (currentFile.Length > 0)
                    {
                        var newFileName = Guid.NewGuid().ToString() + "_" + currentFile.FileName;
                        var fullPath = Path.Combine(uploadPath, newFileName);

                        if (System.IO.File.Exists(fullPath))
                            return null;

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, newFileName), FileMode.Create))
                        {
                            await currentFile.CopyToAsync(fileStream);
                        }

                        bankStatementEntity = ConverterRules.GetBankStatement(fullPath.ToString());
                        transactions.AddRange(bankStatementEntity.Transactions);

                        if (!includeBankStatementData)
                        {
                            await AddBankStatementData(bankStatementEntity);
                            bankStatementId = bankStatementEntity.Id;
                        }
                    }
                }
            }

            List<BankTransactions> newListTransactions = GetFilterdTransactions(transactions);
            foreach (var item in newListTransactions)
            {
                item.BankStatementId = bankStatementId;
                await _bankTransactionsRepository.Add(item);
            }

            bankStatementEntity.Transactions = newListTransactions;
            return bankStatementEntity;
        }

        private async Task AddBankStatementData(BankStatement currentBankStatement)
        {
            try
            {
                Bank bankEntity = _bankRepository.GetByAgencyCode(currentBankStatement.BankAccount.Bank.Code);
                if (bankEntity == null)
                    await _bankRepository.Add(currentBankStatement.BankAccount.Bank);
                else
                {
                    currentBankStatement.BankAccount.Bank = bankEntity;
                    currentBankStatement.BankAccount.BankId = bankEntity.Id;
                }

                BankAccount accountEntity = _bankAccountRepository.GetByAccountBankId(currentBankStatement.BankAccount.AccountCode, currentBankStatement.BankAccount.BankId);
                if (accountEntity == null)
                    await _bankAccountRepository.Add(currentBankStatement.BankAccount);
                else
                {
                    currentBankStatement.BankAccount = accountEntity;
                    currentBankStatement.BankAccount.Id = accountEntity.Id;
                }

                currentBankStatement.Transactions = null;
                await _bankStatementRepository.Add(currentBankStatement);
                includeBankStatementData = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        private static List<BankTransactions> GetFilterdTransactions(List<BankTransactions> transactions)
        {
            List<BankTransactions> newListTransactions = new List<BankTransactions>();
            var filteredList = transactions.Select(d => new { d.Date, d.TransactionValue, d.Type, d.Description }).Distinct().OrderBy(d => d.Date).ToList();
            foreach (var item in filteredList)
            {
                BankTransactions bankTran = new BankTransactions()
                {
                    Id = new Guid(),
                    Description = item.Description,
                    Date = item.Date,
                    Type = item.Type,
                    TransactionValue = item.TransactionValue
                };
                newListTransactions.Add(bankTran);
            }

            return newListTransactions;
        }
    }
}