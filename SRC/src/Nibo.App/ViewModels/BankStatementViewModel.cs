using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nibo.App.ViewModels
{

    public class BankStatementViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Initial Date")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public DateTime? InitialDate { get; set; }

        [DisplayName("Final Date")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public DateTime? FinalDate { get; set; }

        public Guid BankAccountId { get; set; }

        public BankAccountViewModel BankAccount { get; set; }
        public IList<BankTransactionsViewModel> Transactions { get; set; }

        //Formatted
        public virtual string InitialDateFormatted
        {
            get
            {
                return (InitialDate.HasValue) ? InitialDate.Value.ToString("dd/MM/yyyy") : "--";
            }
        }

        public virtual string FinalDateFormatted
        {
            get
            {
                return (FinalDate.HasValue) ? FinalDate.Value.ToString("dd/MM/yyyy") : "--";
            }
        }
    }
}
