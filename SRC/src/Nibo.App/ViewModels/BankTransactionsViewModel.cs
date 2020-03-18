using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nibo.App.ViewModels
{
    public class BankTransactionsViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Type { get; set; }

        public DateTime? Date { get; set; }

        [DisplayName("Transaction Value")]
        public double TransactionValue { get; set; }

        public string Description { get; set; }

        [DisplayName("Check sum")]
        public long Checksum { get; set; }

        //Formatted
        public virtual string DateFormatted
        {
            get
            {
                return (Date.HasValue) ? Date.Value.ToString("dd/MM/yyyy") : "--";
            }
        }

        public virtual string TransactionValueFormatted
        {
            get
            {
                return TransactionValue.ToString("C2");
            }
        }
    }
}
