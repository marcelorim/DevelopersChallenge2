using Nibo.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nibo.Business.Interfaces
{
    interface IConverterRules
    {
        BankStatement GenerateBankStatement(string ofxFile);
    }
}
