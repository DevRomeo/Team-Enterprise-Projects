using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccountingWeb.Models
{
    public class BalanceSheetModel
    {
        public string Branch { get; set; }
        public string BranchAddress { get; set; }
        [Display(Name = "Corporate Name")]
        public string CorporateName { get; set; }

        public string Period { get; set; }

        //[Display(Name = "Month")]
        //public string Month { get; set; }
        //[Display(Name = "Year")]
        //public string Year { get; set; }
        [Display(Name = "CASH & CASH EQUIVALENT")]
        public decimal CashEquivalents { get; set; }
        [Display(Name = "MERCHANDISE INVENTORY")]
        public decimal Merchandise { get; set; }
        [Display(Name = "PLEDGE LOANS")]
        public decimal PledgeLoan { get; set; }
        [Display(Name = "PAST DUE LOANS")]
        public decimal PastDueLoan { get; set; }
        [Display(Name = "ACCOUNT RECEIVABLE-PARTNERS")]
        public decimal AccountReceivable { get; set; }
        [Display(Name = "INPUT TAX")]
        public decimal InputTax { get; set; }
        [Display(Name = "CREDITABLE WITHHOLDING TAX")]
        public decimal CreditableWithHoldingTax { get; set; }
        [Display(Name = "STATIONERY UNISSUED")]
        public decimal Stationery { get; set; }
        [Display(Name = "DUE FROM BRANCHES")]
        public decimal DueFromBranches { get; set; }
        [Display(Name = "DEFERRED CHARGES")]
        public decimal Deferred { get; set; }
        [Display(Name = "TOTAL CURRENT ASSETS")]
        public decimal TotalCurrentAssets
        {
            get
            {
                return (
                    this.DueFromBranches +
                    this.CashEquivalents +
                    this.Merchandise +
                    this.PledgeLoan +
                    this.PastDueLoan +
                    this.AccountReceivable +
                    this.InputTax +
                    this.CreditableWithHoldingTax +
                    this.Stationery +
                    this.Deferred);
            }
        }
        [Display(Name = "INVESTMENTS")]
        public decimal Investments { get; set; }
        [Display(Name = "OTHER ASSETS")]
        public decimal OtherAssets { get; set; }
        [Display(Name = "TOTAL NON-CURRENT ASSETS")]
        public decimal TotalNonCurrentAssets
        {
            get
            {
                return (
                    this.Investments +
                    this.OtherAssets);
            }
        }
        [Display(Name = "BUILDING")]
        public decimal Building { get; set; }
        [Display(Name = "ACCU. DEP - BUILDING")]
        public decimal AccuDefBuilding { get; set; }
        [Display(Name = "BUILDING - NET")]
        public decimal AccuBuildingNet
        {
            get
            {
                return this.Building + this.AccuDefBuilding;
            }
        }

        [Display(Name = "FURNITURE, FIXTURES & EQPT.")]
        public decimal Furniture { get; set; }
        [Display(Name = "ACCU. DEP. - FFE")]
        public decimal AccuFFE { get; set; }
        [Display(Name = "FURNITURE, FIXTURES & EQPT.-NET")]
        public decimal FurnitureNet
        {
            get
            {
                return this.Furniture + this.AccuFFE;
            }
        }
        [Display(Name = "COMPUTER EQUIPMENT")]
        public decimal CompEquipment { get; set; }
        [Display(Name = "ACCU. DEP. - CE")]
        public decimal AccuCE { get; set; }
        [Display(Name = "COMPUTER EQUIPMENT-NET")]
        public decimal CompEquipmentNet
        {
            get
            {
                return this.CompEquipment + this.AccuCE;
            }
        }
        [Display(Name = "OFFICE EQUIPMENT")]
        public decimal OfficeEquipment { get; set; }
        [Display(Name = "ACCU. DEP - OFFICE EQUIPMENT")]
        public decimal AccuOfficeEquipment { get; set; }
        [Display(Name = "OFFICE EQUIPMENT - NET")]
        public decimal OfficeEquipmentNet
        {
            get
            {
                return this.OfficeEquipment + this.AccuOfficeEquipment;
            }
        }
        [Display(Name = "TRANSPORTATION EQUIPMENT")]
        public decimal TransportationEquipment { get; set; }
        [Display(Name = "ACCU.DEP - TRANSPORTATION EQUIPMENT")]
        public decimal AccuTransportationEquipment { get; set; }
        [Display(Name = "TRANSPORTATION EQUIPMENT - NET")]
        public decimal TransportationEquipmentNet
        {
            get
            {
                return this.TransportationEquipment + this.AccuTransportationEquipment;
            }
        }

        [Display(Name = "OTHER FIXED ASSETS")]
        public decimal OtherFixedAssets { get; set; }

        [Display(Name = "ACCU.DEF - OTHER FIXED ASSETS")]
        public decimal AccuOtherFixedAssets { get; set; }
        [Display(Name = "OTHER FIXED ASSETS - NET")]
        public decimal OtherFixedAssetsNET
        {
            get
            {
                return this.OtherFixedAssets + this.AccuOtherFixedAssets;
            }
        }


        [Display(Name = "LEASEHOLD RIGHTS & IMPROVEMENT")]
        public decimal Leasehold { get; set; }
        [Display(Name = "TOTAL FIXED ASSETS - NET")]
        public decimal TotalFixedAssets
        {
            get
            {
                return (
                    this.FurnitureNet +
                    this.CompEquipmentNet +
                    this.OfficeEquipmentNet +
                    this.OtherFixedAssetsNET +
                    this.TransportationEquipmentNet +
                    this.AccuDefBuilding +
                    this.Leasehold);
            }
        }
        [Display(Name = "TOTAL ASSETS")]
        public decimal TotalAssets
        {
            get
            {
                return this.TotalCurrentAssets
                    + this.TotalNonCurrentAssets
                    + this.TotalFixedAssets;
            }
        }
        [Display(Name = "PERCENTAGE TAX PAYABLE")]
        public decimal TaxPayable { get; set; }
        [Display(Name = "DST PAYABLE - PR")]
        public decimal PayablePR { get; set; }
        [Display(Name = "DST PAYABLE - MR")]
        public decimal PayableMR { get; set; }
        [Display(Name = "OUTPUT TAX PAYABLE")]
        public decimal OutputTax { get; set; }
        [Display(Name = "WITHHOLDING TAX PAYABLE")]
        public decimal WithholdingTax { get; set; }
        [Display(Name = "INCOME TAX PAYABLE")]
        public decimal IncomeTax { get; set; }
        [Display(Name = "SSS PREMIUM PAYABLE")]
        public decimal SSS { get; set; }
        [Display(Name = "PAG-IBIG FUND PAYABLE")]
        public decimal PagIbig { get; set; }
        [Display(Name = "PHILHEALTH PREMIUM PAYABLE")]
        public decimal PhilHealth { get; set; }
        [Display(Name = "TOTAL CURRENT LIABILITIES")]
        public decimal TotalCurrentLiabilities
        {
            get
            {
                return this.TaxPayable
                    + this.PayablePR
                    + this.PayableMR
                    + this.OutputTax
                    + this.WithholdingTax
                    + this.IncomeTax
                    + this.SSS
                    + this.PagIbig
                    + this.PhilHealth
                    + this.Prefunding
                    + this.MLKP;
            }
        }
        [Display(Name = "LOAN PAYABLE")]
        public decimal LoanPayable { get; set; }
        [Display(Name = "ACCOUNTS PAYABLE-PREFUNDING")]
        public decimal Prefunding { get; set; }
        [Display(Name = "ACCOUNTS PAYABLE-MLKP")]
        public decimal MLKP { get; set; }
        [Display(Name = "PENSION PLAN LIABLITY")]
        public decimal Pension { get; set; }
        [Display(Name = "OTHER LIABILITIES")]
        public decimal OtherLiabilities { get; set; }
        [Display(Name = "DIVIDENDS PAYABLE")]
        public decimal Division { get; set; }
        [Display(Name = "TOTAL NON-CURRENT LIABILITIES")]
        public decimal TotalNonCurrentLiabilities
        {
            get
            {
                return this.LoanPayable
                    + this.Pension
                    + this.OtherLiabilities
                    + this.Division;
            }
        }
        [Display(Name = "TOTAL LIABILITIES")]
        public decimal TotalLiabilities
        {
            get
            {
                return this.TotalCurrentLiabilities + this.TotalNonCurrentLiabilities;
            }
        }
        [Display(Name = "CAPITAL STOCK")]
        public decimal CapitalStock { get; set; }
        [Display(Name = "RETAINED EARNINGS - APPROPRIATED")]
        public decimal Appropriated { get; set; }
        [Display(Name = "RETAINED EARNINGS - UNAPPROPRIATED BEG")]
        public decimal UnappropriatedBeg { get; set; }
        [Display(Name = "ADD: NET INCOME AFTER TAX")]
        public decimal AfterTax { get; set; }
        [Display(Name = "RETAINED EARNINGS - UNAPPROPRIATED")]
        public decimal UnappropriatedEnd
        {
            get
            {
                return this.UnappropriatedBeg
                    + this.AfterTax;
            }
        }
        public decimal Unappropriated
        {
            get
            {
                return this.UnappropriatedBeg
                    + this.AfterTax;
            }
        }
        [Display(Name = "TOTAL STOCKHOLDERS' EQUITY")]
        public decimal TotalStockHolders
        {
            get
            {
                return this.CapitalStock
                    + this.Appropriated
                    + this.UnappropriatedEnd;
            }
        }
        [Display(Name = "TOTAL LIABILITIES & STOCKHOLDERS' EQUITY")]
        public decimal TotalLiabilitiesStockHolders
        {
            get
            {
                return this.TotalLiabilities + this.TotalStockHolders;
            }
        }
    }
}