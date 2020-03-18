using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nibo.App.ViewModels
{
    public class BankAccountViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Agency")]
        public string AgencyCode { get; set; }

        [DisplayName("Account")]
        public string AccountCode { get; set; }

        public Guid BankId { get; set; }

        public BankViewModel Bank { get; set; }

        //Formatted
        public virtual string AgencyCodeFormatted
        {
            get
            {
                return (!string.IsNullOrEmpty(AgencyCode)) ? AgencyCode : "--";
            }
        }
    }
}
