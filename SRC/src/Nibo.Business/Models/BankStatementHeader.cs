using System;
using System.Collections.Generic;
using System.Text;

namespace Nibo.Business.Models
{
    public class BankStatementHeader
    {
        public string Status { get; set; }

        public string Language { get; set; }

        public DateTime ServerDate { get; set; }

        public string BankName { get; set; }

        /* Constructor */
        public BankStatementHeader() { }

        public BankStatementHeader(string status, string language, DateTime serverDate, string bankName)
        {
            this.Status = status;
            this.Language = language;
            this.ServerDate = serverDate;
            this.BankName = bankName;
        }
    }
}
