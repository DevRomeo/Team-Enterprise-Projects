using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ML.AccountingSystemV1.Contract;
using ML.AccountingSystemV1.Service;
using Ninject.Modules;

namespace Accounting
{
    public class MLAccountingModules : NinjectModule
    {
        public override void Load()
        {
            Bind<iAccountingService>().To<AccountingService>();
        }
    }
}