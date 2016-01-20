using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountingWeb.Models
{
    public class TrialBalanceViewModel
    {
        public BalanceSheetModel BalanceSheet { get; set; }
        public IncomeStatementModel IncomeStatement { get; set; }

        public decimal TotalDebit
        {
            get
            {
                return this.BalanceSheet.CashEquivalents +
                    this.BalanceSheet.Merchandise +
                    this.BalanceSheet.PledgeLoan +
                    this.BalanceSheet.PastDueLoan +
                    this.BalanceSheet.AccountReceivable +
                    this.BalanceSheet.InputTax +
                    this.BalanceSheet.CreditableWithHoldingTax +
                    this.BalanceSheet.Stationery +
                    this.BalanceSheet.DueFromBranches +
                    this.BalanceSheet.Deferred +
                    this.BalanceSheet.Investments +
                    this.BalanceSheet.OtherAssets +
                    this.BalanceSheet.Building +
                    this.BalanceSheet.Furniture +
                    this.BalanceSheet.CompEquipment +
                    this.BalanceSheet.OfficeEquipment +
                    this.BalanceSheet.TransportationEquipment +
                    this.BalanceSheet.OtherFixedAssets +
                    this.BalanceSheet.Leasehold +
                    this.IncomeStatement.SalesReturns +
                    this.IncomeStatement.SalesDiscount +
                    this.IncomeStatement.CostOfSales +
                    //this.IncomeStatement.BeginInventory +
                    //this.IncomeStatement.Reacquired +
                    //this.IncomeStatement.EndInventory +
                    this.IncomeStatement.BorrowedFunds +
                    this.IncomeStatement.Salaries +
                    this.IncomeStatement.Staff +
                    this.IncomeStatement.MonthPay +
                    this.IncomeStatement.SSSExpense +
                    this.IncomeStatement.Uniforms +
                    this.IncomeStatement.Rent +
                    this.IncomeStatement.Power +
                    this.IncomeStatement.Communication +
                    this.IncomeStatement.OfficeSupply +
                    this.IncomeStatement.SecurityService +
                    this.IncomeStatement.AgencyFee +
                    this.IncomeStatement.DepreciationFee +
                    this.IncomeStatement.DepreciationCe +
                    this.IncomeStatement.DepreciationBldg +
                    this.IncomeStatement.DepreciationTE +
                    this.IncomeStatement.DepreciationOFA +
                    this.IncomeStatement.Amortization +
                    this.IncomeStatement.AmortizationDefCharge +
                    this.IncomeStatement.ProfessionalFee1 +
                    this.IncomeStatement.ProfessionalFee2 +
                    this.IncomeStatement.ProfessionalFeeExempt +
                    //this.IncomeStatement.ProfessionalFee +
                    this.IncomeStatement.TaxesLincenses +
                    this.IncomeStatement.PercentageTax +
                    this.IncomeStatement.Prenda +
                    this.IncomeStatement.Remittance +
                    this.IncomeStatement.BankLoan +
                    this.IncomeStatement.Insurance +
                    this.IncomeStatement.FuelLubricants +
                    this.IncomeStatement.TravelExpenses +
                    this.IncomeStatement.TranspoExpenses +
                    this.IncomeStatement.Maintenance1 +
                    this.IncomeStatement.Maintenance2 +
                    //this.IncomeStatement.Advertising +
                    this.IncomeStatement.Advertising1 +
                    this.IncomeStatement.Advertising2 +
                    this.IncomeStatement.RepresentationExpense +
                    this.IncomeStatement.FreightExpense +
                    this.IncomeStatement.AlarmExpense +
                    //this.IncomeStatement.Marketing +
                    this.IncomeStatement.Marketing1 +
                    this.IncomeStatement.Marketing2 +
                    this.IncomeStatement.AlliedServices +
                    this.IncomeStatement.Medical +
                    this.IncomeStatement.Prizes +
                    this.IncomeStatement.Donations +
                    this.IncomeStatement.OtherExpenses +
                    //this.IncomeStatement.Miscellaneous +                                                           
                    //this.IncomeStatement.Amortization +                                                            
                    this.IncomeStatement.Miscellaneous1 +
                    this.IncomeStatement.Miscellaneous2 +
                    this.IncomeStatement.Provision;
            }
        }
        public decimal TotalCredit
        {
            get
            {
                return this.BalanceSheet.AccuFFE +
                    this.BalanceSheet.AccuCE +
                    this.BalanceSheet.TaxPayable +
                    this.BalanceSheet.PayablePR +
                    this.BalanceSheet.PayableMR +
                    this.BalanceSheet.OutputTax +
                    this.BalanceSheet.WithholdingTax +
                    this.BalanceSheet.IncomeTax +
                    this.BalanceSheet.SSS +
                    this.BalanceSheet.PagIbig +
                    this.BalanceSheet.PhilHealth +
                    this.BalanceSheet.LoanPayable +
                    this.BalanceSheet.Prefunding +
                    this.BalanceSheet.MLKP +
                    this.BalanceSheet.Pension +
                    this.BalanceSheet.OtherLiabilities +
                    this.BalanceSheet.Division +
                    this.BalanceSheet.CapitalStock +
                    this.BalanceSheet.UnappropriatedBeg +
                    this.BalanceSheet.Appropriated +
                    this.IncomeStatement.PledgeLoan +
                    this.IncomeStatement.ServiceCharge +
                    this.IncomeStatement.AuctionSale +
                    this.IncomeStatement.LiquidatedDamages +
                    this.IncomeStatement.CommissionIncome +
                    this.IncomeStatement.Forex +                    
                    this.IncomeStatement.InsuranceIncome +
                    this.IncomeStatement.RentIncome +
                    this.IncomeStatement.OtherIncome +
                    this.IncomeStatement.PastDueLoan +
                    this.BalanceSheet.AccuDefBuilding +
                    this.IncomeStatement.JewelrySales;
            }
        }
    }
}