using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nibo.App.ViewModels
{
    public class BankViewModel
    {
        [Key]
        public Guid Id { get; set; }


        public int Code { get; set; }

        [DisplayName("Bank")]
        public string Name { get; set; }
    }
}
