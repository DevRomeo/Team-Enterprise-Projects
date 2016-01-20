using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccountingWeb.Models
{
    public class IncomeStatementModel
    {
        public string Branch { get; set; }
        public string BranchAddress { get; set; }
        [Display(Name = "Corporate Name")]
        public string CorporateName { get; set; }
        [Display(Name = "Month")]
        public string Month { get; set; }
        [Display(Name = "Year")]
        public string Year { get; set; }
        [Display(Name = "INTEREST INCOME-PLEDGE LOANS")]
        public decimal PledgeLoan { get; set; }
        [Display(Name = "INTEREST INCOME-PAST DUE LOANS")]
        public decimal PastDueLoan { get; set; }
        [Display(Name = "SERVICE CHARGE")]
        public decimal ServiceCharge { get; set; }
        [Display(Name = "GAIN/LOSS ON AUCTION SALE")]
        public decimal AuctionSale { get; set; }
        [Display(Name = "LIQUIDATED DAMAGES")]
        public decimal LiquidatedDamages { get; set; }
        [Display(Name = "TOTAL")]
        public decimal Total
        {
            get
            {
                return
                        this.PledgeLoan
                    + this.PastDueLoan
                    + this.ServiceCharge
                    + this.AuctionSale
                    + this.LiquidatedDamages;
            }
        }
        [Display(Name = "JEWELRY SALES")]
        public decimal JewelrySales { get; set; }
        [Display(Name = "SALES RETURNS & ALLOW ")]
        public decimal SalesReturns { get; set; }
        [Display(Name = "SALES DISCOUNT")]
        public decimal SalesDiscount { get; set; }
        [Display(Name = "NET SALES")]
        public decimal NetSales
        {
            get
            {
                return this.JewelrySales + this.SalesReturns + this.SalesDiscount;
            }
        }

        [Display(Name = "OTHER SALES ")]
        public decimal OtherSale { get; set; }


        [Display(Name = "SERVICE INCOME-MLKP")]
        public decimal ServiceIncome { get; set; }
        [Display(Name = "INSURANCE INCOME")]
        public decimal InsuranceIncome { get; set; }
        [Display(Name = "RENT INCOME")]
        public decimal RentIncome { get; set; }
        [Display(Name = "OTHER INCOME")]
        public decimal OtherIncome { get; set; }
        [Display(Name = "TOTAL DOMESTIC INCOME")]
        public decimal TotalDomesticIncome
        {
            get
            {
                return this.Total
                    + this.NetSales
                    + this.OtherSale
                    + this.ServiceIncome
                    + this.InsuranceIncome
                    + this.RentIncome
                    + this.OtherIncome;
            }
        }
        [Display(Name = "COMISSION INCOME")]
        public decimal CommissionIncome { get; set; }
        [Display(Name = "GAIN/LOSS ON FOREX")]
        public decimal Forex { get; set; }
        [Display(Name = "TOTAL INTERNATIONAL INCOME")]
        public decimal TotalIntIncome
        {
            get
            {
                return this.CommissionIncome + this.Forex;
            }
        }
        [Display(Name = "GROSS REVENUE")]
        public decimal GrossRevenue
        {
            get
            {
                return this.TotalDomesticIncome + this.TotalIntIncome;
            }
        }
        [Display(Name = "BEGINNING INVENTORY")]
        public decimal BeginInventory { get; set; }
        //[Display(Name = "PROPERTIES REACQUIRED")]
        [Display(Name = "PURCHASES")]
        public decimal Reacquired { get; set; }
        [Display(Name = "TOTAL GOODS AVAILABLE FOR SALE")]
        public decimal TotalGoods
        {
            get
            {
                return this.BeginInventory + this.Reacquired;
            }
        }
        [Display(Name = "INVENTORY, END")]
        public decimal EndInventory { get; set; }
        [Display(Name = "COST OF SALES")]
        public decimal CostOfSales
        {
            get
            {
                return this.TotalGoods - this.EndInventory;
            }
        }
        [Display(Name = "INTEREST ON BORROWED FUNDS")]
        public decimal BorrowedFunds { get; set; }
        [Display(Name = "SALARIES & WAGES")]
        public decimal Salaries { get; set; }
        [Display(Name = "STAFF BENEFITS")]
        public decimal Staff { get; set; }
        [Display(Name = "13TH MONTH PAY")]
        public decimal MonthPay { get; set; }
        [Display(Name = "SSS/PHILHEALTH & PAG-IBIG EXPENSE")]
        public decimal SSSExpense { get; set; }
        [Display(Name = "COMPANY UNIFORMS")]
        public decimal Uniforms { get; set; }
        [Display(Name = "RENT")]
        public decimal Rent { get; set; }
        [Display(Name = "POWER, LIGHT & WATER")]
        public decimal Power { get; set; }
        [Display(Name = "COMMUNICATION")]
        public decimal Communication { get; set; }
        [Display(Name = "STATIONERY & OFFICE SUPPLIES USED")]
        public decimal OfficeSupply { get; set; }
        [Display(Name = "SECURITY SERVICE")]
        public decimal SecurityService { get; set; }
        [Display(Name = "SECURITY GUARD-AGENCY FEE")]
        public decimal AgencyFee { get; set; }
        [Display(Name = "DEPRECIATION-FEE")]
        public decimal DepreciationFee { get; set; }
        [Display(Name = "DEPRECIATION-CE")]
        public decimal DepreciationCe { get; set; }
        [Display(Name = "DEPRECIATION-BLDG")]
        public decimal DepreciationBldg { get; set; }
        [Display(Name = "DEPRECIATION-TE")]
        public decimal DepreciationTE { get; set; }
        [Display(Name = "DEPRECIATION-OFA")]
        public decimal DepreciationOFA { get; set; }

        [Display(Name = "AMORTIZATION-LRI")]
        public decimal Amortization { get; set; }
        [Display(Name = "AMORTIZATION-DEFERRED CHARGES")]
        public decimal AmortizationDefCharge { get; set; }
        [Display(Name = "TOTAL DIRECT COSTS")]
        public decimal TotalDirectCosts
        {
            get
            {
                return
                    //this.CostOfSales + 
                    this.BorrowedFunds
                    + this.Salaries
                    + this.Staff
                    + this.MonthPay
                    + this.SSSExpense
                    + this.Uniforms
                    + this.Rent
                    + this.Power
                    + this.Communication
                    + this.OfficeSupply
                    + this.SecurityService
                    + this.AgencyFee
                    + this.DepreciationFee
                    + this.DepreciationCe
                    + this.DepreciationBldg
                    + this.DepreciationTE
                    + this.DepreciationOFA
                    + this.Amortization
                    + this.AmortizationDefCharge;
            }
        }
        [Display(Name = "GROSS PROFIT")]
        public decimal GrossProfit
        {
            get
            {
                return this.GrossRevenue - this.TotalDirectCosts;// -this.TotalOperatingExpenses;
            }
        }
        [Display(Name = "PROFESSIONAL FEES")]
        public decimal ProfessionalFee { get; set; }
        [Display(Name = "PROFESSIONAL FEES 10%")]
        public decimal ProfessionalFee1 { get; set; }
        [Display(Name = "PROFESSIONAL FEES 15%")]
        public decimal ProfessionalFee2 { get; set; }
        [Display(Name = "PROFESSIONAL FEES - Exempt")]
        public decimal ProfessionalFeeExempt { get; set; }
        [Display(Name = "TAXES & LICENSES")]
        public decimal TaxesLincenses { get; set; }
        [Display(Name = "PERCENTAGE TAX")]
        public decimal PercentageTax { get; set; }
        [Display(Name = "DST-PRENDA")]
        public decimal Prenda { get; set; }
        [Display(Name = "DST-MONEY REMITTANCE")]
        public decimal Remittance { get; set; }
        [Display(Name = "DST-BANK LOAN")]
        public decimal BankLoan { get; set; }
        [Display(Name = "INSURANCE")]
        public decimal Insurance { get; set; }
        [Display(Name = "FUEL & LUBRICANTS")]
        public decimal FuelLubricants { get; set; }
        [Display(Name = "TRAVELLING EXPENSES")]
        public decimal TravelExpenses { get; set; }
        [Display(Name = "TRANSPORTATION EXPENSES")]
        public decimal TranspoExpenses { get; set; }
        [Display(Name = "REPAIRS & MAINTENANCE-1%")]
        public decimal Maintenance1 { get; set; }
        [Display(Name = "REPAIRS & MAINTENANCE-2%")]
        public decimal Maintenance2 { get; set; }
        [Display(Name = "ADVERTISING & PUBLICITY")]
        public decimal Advertising { get; set; }
        [Display(Name = "ADVERTISING & PUBLICITY 1%")]
        public decimal Advertising1 { get; set; }
        [Display(Name = "ADVERTISING & PUBLICITY 2%")]
        public decimal Advertising2 { get; set; }
        [Display(Name = "REPRESENTATION EXPENSES")]
        public decimal RepresentationExpense { get; set; }
        [Display(Name = "FREIGTH EXPENSES")]
        public decimal FreightExpense { get; set; }
        [Display(Name = "ALARM EXPENSES")]
        public decimal AlarmExpense { get; set; }
        [Display(Name = "MARKETING EXPENSES")]
        public decimal Marketing { get; set; }
        [Display(Name = "MARKETING EXPENSES 1%")]
        public decimal Marketing1 { get; set; }
        [Display(Name = "MARKETING EXPENSES 2%")]
        public decimal Marketing2 { get; set; }
        [Display(Name = "ALLIED SERVICES")]
        public decimal AlliedServices { get; set; }
        [Display(Name = "MEDICAL/DENTAL")]
        public decimal Medical { get; set; }
        [Display(Name = "PRIZES-EXEMPT")]
        public decimal Prizes { get; set; }
        [Display(Name = "DONATIONS")]
        public decimal Donations { get; set; }
        [Display(Name = "MISCELLANEOUS EXPENSES 1%")]
        public decimal Miscellaneous1 { get; set; }
        [Display(Name = "MISCELLANEOUS EXPENSES 2%")]
        public decimal Miscellaneous2 { get; set; }
        [Display(Name = "OTHER EXPENSES")]
        public decimal OtherExpenses { get; set; }
        [Display(Name = "MISCELLANEOUS EXPENSES")]
        public decimal Miscellaneous { get; set; }
        [Display(Name = "TOTAL OPERATING EXPENSES")]
        public decimal TotalOperatingExpenses
        {
            get
            {
                return this.ProfessionalFee
                    + this.ProfessionalFee1
                    + this.ProfessionalFee2
                    + this.TaxesLincenses
                    + this.PercentageTax
                    + this.Prenda
                    + this.Remittance
                    + this.BankLoan
                    + this.Insurance
                    + this.FuelLubricants
                    + this.TravelExpenses
                    + this.TranspoExpenses
                    + this.Maintenance1
                    + this.Maintenance2
                    + this.Advertising
                    + this.Advertising1
                    + this.Advertising2
                    + this.RepresentationExpense
                    + this.FreightExpense
                    + this.AlarmExpense
                    + this.Marketing
                    + this.Marketing1
                    + this.Marketing2
                    + this.AlliedServices
                    + this.Medical
                    + this.Prizes
                    + this.Donations
                    + this.OtherExpenses
                    + this.Miscellaneous1
                    + this.Miscellaneous2
                    + this.ProfessionalFeeExempt
                    + this.Miscellaneous;
            }
        }
        [Display(Name = "NET INCOME BEFORE TAX")]
        public decimal NetIncomeBeforeTax
        {
            get
            {
                return this.GrossProfit - this.TotalOperatingExpenses;
            }
        }
        [Display(Name = "PROVISION FOR INCOME TAX")]
        public decimal Provision { get; set; }
        [Display(Name = "NET INCOME AFTER TAX")]
        public decimal NetIncomeAfterTax
        {
            get
            {
                return this.NetIncomeBeforeTax - this.Provision;
            }
        }

        [Display(Name = "SALES - SPD")]
        public decimal SPD { get; set; }
    }
}