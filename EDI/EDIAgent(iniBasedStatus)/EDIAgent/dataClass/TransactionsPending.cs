using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIAgent.dataClass
{
    class TransactionsPending
    {
        public String paymentmethod { get; set; }
        public String freefield4 { get; set; }
        public String freefield5 { get; set; }
        public String CompanyCode { get; set; }
        public String TransactionType { get; set; }
        public String TransactionDate { get; set; }
        public String FinYear { get; set; }
        public String FinPeriod { get; set; }
        public String Description { get; set; }
        public String CompanyAccountCode { get; set; }
        public String EntryNumber { get; set; }
        public String CurrencyAliasAC { get; set; }
        public String AmountDebitAC { get; set; }
        public String AmountCreditAC { get; set; }
        public String CurrencyAliasFC { get; set; }
        public String AmountDebitFC { get; set; }
        public String AmountCreditFC { get; set; }
        public String VATCode { get; set; }
        public String ProcessNumber { get; set; }
        public String ProcessLineCode { get; set; }
        public String res_id { get; set; }
        public String oorsprong { get; set; }
        public String docdate { get; set; }
        public String vervdatfak { get; set; }
        public String faktuurnr { get; set; }
        public String syscreator { get; set; }
        public String sysmodifier { get; set; }
        public String EntryGuid { get; set; }
        public String companycostcentercode { get; set; }
        public String batchNo { get; set; }
    }
}
