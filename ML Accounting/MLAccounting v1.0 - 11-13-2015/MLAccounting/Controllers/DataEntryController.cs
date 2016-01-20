using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccountingWeb.Models;
using PDFGeneration;
using System.Globalization;
using ML.AccountingSystemV1.Contract.Parameters;
using ML.AccountingSystemV1.Service.Class;
using ML.AccountingSystemV1.Service;
using Newtonsoft.Json;
using Accounting.Models;

namespace AccountingWeb.Controllers
{
    [Authorize]
    public class DataEntryController : PdfViewController
    {

        [HttpGet]
        [Route("Create/Entry")]
        public ActionResult Index()
        {

            DataEntryModel dataEntry = new DataEntryModel();
            dataEntry.Date = DateTime.Now;
            dataEntry.Entries = Enumerable.Range(1, 6).Select(m => new EntryModel { Credit = 0, Debit = 0, Description = "", GL = "", Reference = "", Remarks = "" }).ToList();
            return View("DataEntry", dataEntry);
        }

        [Route("GeneralLedger")]
        [HttpGet]
        public ActionResult getGL()
        {
            AccountingService srvc = new AccountingService();
            var dataEntryComponent = srvc.CorporanameReturnValue();

            List<GeneralLedgerModel> GeneralLedgers = (from m in srvc.GlAccount_Description().data
                                                       select new GeneralLedgerModel
                                                       {
                                                           GeneralLedger = m.GLcode,
                                                           Description = m.Description
                                                       }).ToList();

            return Json(new
            {
                code = dataEntryComponent.respcode,
                entryno = dataEntryComponent.EntryNo,
                component = JsonConvert.SerializeObject(dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC." && m.branchName == "ML COLON 1")),
                corporatenames = dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC.").Select(m => m.Corpname).Distinct().ToArray(),
                EntryGL = PartialToString.RenderPartialViewToString(this, "_partialGL", GeneralLedgers)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Save/Entry")]
        public ActionResult SaveEntry(DataEntryModel dataEntry)
        {
            if (ModelState.IsValid && dataEntry.Entries.Count(m => m.Add == true) > 0)
            {
                AccountingService srvc = new AccountingService();
                List<Gl_Entry> entries = (from m in dataEntry.Entries.Where(m => m.Add == true)
                                          select new Gl_Entry
                                          {
                                              EntryNo = dataEntry.EntryNo,
                                              bcode = dataEntry.BranchCode,
                                              bdr_hfl = m.Credit != 0 ? m.Credit * -1 : m.Debit,
                                              date = dataEntry.Date,
                                              GLDescription = m.Description,
                                              GLNumber = m.GL,
                                              Reference = m.Reference,
                                              Remarks = m.Remarks,
                                              zcode = dataEntry.Zone,
                                              corpname = dataEntry.CorporateName
                                          }).ToList();

                var srvcResponse = srvc.InsertDataEntryGL(entries, dataEntry.CorporateName);
                if (srvcResponse.respcode == 1)
                {
                    return Json(new { code = srvcResponse.respcode, msg = srvcResponse.message, action = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = srvcResponse.message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("PartialReport/BalanceSheet")]
        [ValidateAntiForgeryToken]
        public ActionResult BalanceSheet(DataEntryModel dataEntry)
        {
            if (ModelState.IsValid && dataEntry.Entries.Count(m => m.Add == true) > 0)
            {
                AccountingService srvc = new AccountingService();
                List<DataEntryGL> dataEntryGL = (from m in dataEntry.Entries.Where(m => m.Add == true)
                                                 select new DataEntryGL
                                                 {
                                                     Amount = m.Credit != 0 ? m.Credit * -1 : m.Debit,
                                                     GLNumber = m.GL
                                                 }).ToList();

                var srvcResponse = srvc.PartialReport(dataEntry.BranchCode, dataEntry.Zone, dataEntry.CorporateName, dataEntry.Date.ToString("yyyy-MM-dd"), dataEntryGL, "BalanceSheet");
                if (srvcResponse.respcode == 1)
                {

                    BalanceSheetModel BSReport = (from m in srvcResponse.RetrieveBalanceSheetReport
                                                  select new BalanceSheetModel
                                                  {
                                                      CorporateName = dataEntry.CorporateName,
                                                      Period = "AS OF "+ new DateTimeFormatInfo().GetMonthName(dataEntry.Date.Month) + ", " + dataEntry.Date.Year.ToString(),
                                                      Branch = dataEntry.Branch,
                                                      BranchAddress = dataEntry.BranchAddress,
                                                      CashEquivalents = m.Cash_Equivalents,
                                                      Merchandise = m.Merchandise_Inventory,
                                                      PledgeLoan = m.Pledge_Loans,
                                                      PastDueLoan = m.Pledge_DueLoans,
                                                      AccountReceivable = m.Accounts_ReceivablePartners,
                                                      InputTax = m.Input_Tax,
                                                      CreditableWithHoldingTax = m.Creditable_withholdigTax,
                                                      Stationery = m.Stationery_Unissued,
                                                      Deferred = m.Deferred_charges,
                                                      //TotalAssets = (m.Cash_Equivalents + m.Merchandise_Inventory + m.Pledge_Loans + m.Pledge_DueLoans +
                                                      //m.Accounts_ReceivablePartners + m.Input_Tax + m.Creditable_withholdigTax + m.Stationery_Unissued + m.Deferred_charges),
                                                      Investments = m.Investments,
                                                      OtherAssets = m.other_assets,
                                                      //TotalNonCurrentLiabilities = (m.Investments + m.other_assets),
                                                      Furniture = m.Furniture_Fixtures_Eqpt,
                                                      AccuFFE = m.Accu_Dep_FFE * -1,
                                                      //FurnitureNet = (m.Furniture_Fixtures_Eqpt + m.Accu_Dep_FFE),
                                                      CompEquipment = m.Computer_EQPT,
                                                      AccuCE = m.Accu_Dep_CE * -1,
                                                      //CompEquipmentNet = (m.Computer_EQPT + m.Accu_Dep_CE),
                                                      Leasehold = m.LeaseHoldRight_improvement,
                                                      TaxPayable = m.Percentage_Tax_Payable * -1,
                                                      PayablePR = m.DST_Payable_PR * -1,
                                                      PayableMR = m.DST_Payable_MR * -1,
                                                      OutputTax = m.Output_tax_payable * -1,
                                                      WithholdingTax = m.withholding_tax_payable * -1,
                                                      IncomeTax = m.income_tax_payable * -1,
                                                      SSS = m.sss_premium_payable * -1,
                                                      PagIbig = m.pagibig_fund_payable * -1,
                                                      PhilHealth = m.philhealth_premium_payable * -1,
                                                      //TotalCurrentLiabilities
                                                      LoanPayable = m.loans_payable * -1,
                                                      Prefunding = m.accounts_payable_prefunding * -1,
                                                      MLKP = m.accounts_payable_MLKP * -1,
                                                      Pension = m.Pension_Plan_Liability * -1,
                                                      OtherLiabilities = m.Other_Liabilities * -1,
                                                      Division = m.Dividends_payable * -1,
                                                      //TotalNonCurrentLiabilities
                                                      //TotalLiabilities
                                                      CapitalStock = m.Capital_Stock * -1,
                                                      Appropriated = m.Retained_Earnings_appropriated * -1,
                                                      UnappropriatedBeg = m.Retained_Earnings_unappropriatedBEG * -1,
                                                      AfterTax = m.Add_net_income_afterTax,
                                                      DueFromBranches = m.Due_From_Braches,
                                                      OfficeEquipment = m.Office_Equipment,
                                                      AccuOfficeEquipment = m.Accu_Dep_OF,
                                                      TransportationEquipment = m.Transportation_Equipment,
                                                      AccuTransportationEquipment = m.ACCU_DEP_TE,
                                                      OtherFixedAssets = m.Other_Fixed_Assets,
                                                      AccuOtherFixedAssets = m.ACCU_DEP_OFA,
                                                      AccuDefBuilding = m.AD_Building,
                                                      Building = m.Building
                                                      //UnappropriatedEnd = (m.Capital_Stock + m.Retained_Earnings_appropriated)
                                                  }).FirstOrDefault();
                    return this.ViewPdf("", "BalanceSheet", BSReport);
                }
            }
            ViewData["Message"] = "No Report Found";
            return View("NoReport");
        }

        [HttpPost]
        [Route("PartialReport/IncomeStatement")]
        [ValidateAntiForgeryToken]
        public ActionResult IncomeStatement(DataEntryModel dataEntry)
        {
            if (ModelState.IsValid && dataEntry.Entries.Count(m => m.Add == true) > 0)
            {
                AccountingService srvc = new AccountingService();
                List<DataEntryGL> dataEntryGL = (from m in dataEntry.Entries.Where(m => m.Add == true)
                                                 select new DataEntryGL
                                                 {
                                                     Amount = m.Credit != 0 ? m.Credit * -1 : m.Debit,
                                                     GLNumber = m.GL
                                                 }).ToList();

                var srvcResponse = srvc.PartialReport(dataEntry.BranchCode, dataEntry.Zone, dataEntry.CorporateName, dataEntry.Date.ToString("yyyy-MM-dd"), dataEntryGL, "");
                if (srvcResponse.respcode == 1)
                {
                    IncomeStatementModel IncomeStatement = (from m in srvcResponse.RetrieveIncomeStatementReport
                                                            select new IncomeStatementModel
                                                            {
                                                                CorporateName = dataEntry.CorporateName,
                                                                Month = new DateTimeFormatInfo().GetMonthName(dataEntry.Date.Month),
                                                                Year = dataEntry.Date.Year.ToString(),
                                                                Branch = dataEntry.Branch,
                                                                BranchAddress = dataEntry.BranchAddress,
                                                                PledgeLoan = m.Interes_Income_PL * -1,
                                                                PastDueLoan = m.Interes_Income_PLD * -1,
                                                                ServiceCharge = m.serv_charge * -1,
                                                                AuctionSale = m.gain_loss_sale * -1,
                                                                LiquidatedDamages = m.Liquidated_Damage * -1,
                                                                JewelrySales = m.Jewel_Sales * -1,
                                                                SalesReturns = m.Sales_return_allow,
                                                                SalesDiscount = m.Sales_Discount,
                                                                ServiceIncome = m.Service_Income_MLKP * -1,
                                                                InsuranceIncome = m.Insurance_Income * -1,
                                                                RentIncome = m.Rent_Income * -1,
                                                                OtherIncome = m.Other_Income * -1,
                                                                CommissionIncome = m.Commission_Income * -1,
                                                                Forex = m.gain_loss_forex * -1,
                                                                BeginInventory = m.Beginning_inv,
                                                                Reacquired = m.Properties_Reacquired,
                                                                EndInventory = m.Ending_Inventory,
                                                                //CostOfSales = m.Cost_Sales,
                                                                BorrowedFunds = m.Interest_Borrowed_fund,
                                                                Salaries = m.Salary_Wages,
                                                                Staff = m.Staff_Benefits,
                                                                MonthPay = m.month_year_pay,
                                                                SSSExpense = m.SPHPG_Expense,
                                                                Uniforms = m.company_uniforms,
                                                                Rent = m.rent,
                                                                Power = m.Power_light_water,
                                                                Communication = m.Communication,
                                                                OfficeSupply = m.stationary_office_supply,
                                                                SecurityService = m.security_services,
                                                                AgencyFee = m.security_agency,
                                                                DepreciationFee = m.Depreciation_FFE,
                                                                DepreciationCe = m.Depreciation_CE,
                                                                Amortization = m.Amortization_LRI,
                                                                ProfessionalFee = m.Professional_fees,
                                                                TaxesLincenses = m.taxes_licenses,
                                                                PercentageTax = m.percentage_tax,
                                                                Prenda = m.DST_Prenda,
                                                                Remittance = m.DST_MoneyRemittance,
                                                                BankLoan = m.DST_BankLoan,
                                                                Insurance = m.Insurance,
                                                                FuelLubricants = m.Fuel_Lubricants,
                                                                TravelExpenses = m.Travelling_Expenses,
                                                                TranspoExpenses = m.Transportation_Expenses,
                                                                Maintenance1 = m.Repairs_maintenance1,
                                                                Maintenance2 = m.Repairs_maintenance2,
                                                                Advertising = m.Advertising_Publicity,
                                                                RepresentationExpense = m.Representaion_Expenses,
                                                                FreightExpense = m.Freight_Expenses,
                                                                AlarmExpense = m.Alarm_Expenses,
                                                                Marketing = m.Marketing_Expenses,
                                                                AlliedServices = m.Allied_Services,
                                                                Medical = m.Medical_Dental,
                                                                Prizes = m.Prizes_Exempt,
                                                                Donations = m.Donations,
                                                                OtherExpenses = m.Other_expenses,
                                                                Miscellaneous = m.Miscellaneous_expenses,
                                                                SPD = m.Sales_SPD,
                                                                OtherSale = m.Other_Sales,
                                                                DepreciationBldg = m.Depreciation_BLDG,
                                                                DepreciationTE = m.Depreciation_TE,
                                                                DepreciationOFA = m.Depreciation_OFA,
                                                                AmortizationDefCharge = m.Amortization_deferredCharges,
                                                                ProfessionalFee1 = m.Professional_Fees1,
                                                                ProfessionalFee2 = m.Professional_Fees2,
                                                                ProfessionalFeeExempt = m.Professional_Fees_Exempt,
                                                                Advertising1 = m.Advertising_Publicity1,
                                                                Advertising2 = m.Advertising_Publicity2,
                                                                Marketing1 = m.Marketing_Expenses1,
                                                                Marketing2 = m.Marketing_Expenses2,
                                                                Miscellaneous1 = m.Miscellaneous_Expenses1,
                                                                Miscellaneous2 = m.Miscellaneous_Expenses2,
                                                                Provision = m.Provision_for_income_tax
                                                            }).FirstOrDefault();

                    return this.ViewPdf("", "IncomeStatement", IncomeStatement);
                }
            }
            ViewData["Message"] = "No Report Found";
            return View("NoReport");
        }

        [HttpPost]
        [Route("PartialReport/TrialBalance")]
        [ValidateAntiForgeryToken]
        public ActionResult TrialBalance(DataEntryModel dataEntry)
        {
            if (ModelState.IsValid && dataEntry.Entries.Count(m => m.Add == true) > 0)
            {
                AccountingService srvc = new AccountingService();
                List<DataEntryGL> dataEntryGL = (from m in dataEntry.Entries.Where(m => m.Add == true)
                                                 select new DataEntryGL
                                                 {
                                                     Amount = m.Credit != 0 ? m.Credit * -1 : m.Debit,
                                                     GLNumber = m.GL
                                                 }).ToList();

                var srvcResponse = srvc.PartialReport(dataEntry.BranchCode, dataEntry.Zone, dataEntry.CorporateName, dataEntry.Date.ToString("yyyy-MM-dd"), dataEntryGL, "");
                if (srvcResponse.respcode == 1)
                {
                    IncomeStatementModel IncomeStatement = (from m in srvcResponse.RetrieveIncomeStatementReport
                                                            select new IncomeStatementModel
                                                            {
                                                                CorporateName = dataEntry.CorporateName,
                                                                Month = new DateTimeFormatInfo().GetMonthName(dataEntry.Date.Month),
                                                                Year = dataEntry.Date.Year.ToString(),
                                                                Branch = dataEntry.Branch,
                                                                BranchAddress = dataEntry.BranchAddress,
                                                                PledgeLoan = m.Interes_Income_PL * -1,
                                                                PastDueLoan = m.Interes_Income_PLD * -1,
                                                                ServiceCharge = m.serv_charge * -1,
                                                                AuctionSale = m.gain_loss_sale * -1,
                                                                LiquidatedDamages = m.Liquidated_Damage * -1,
                                                                JewelrySales = m.Jewel_Sales * -1,
                                                                SalesReturns = m.Sales_return_allow,
                                                                SalesDiscount = m.Sales_Discount,
                                                                ServiceIncome = m.Service_Income_MLKP * -1,
                                                                InsuranceIncome = m.Insurance_Income * -1,
                                                                RentIncome = m.Rent_Income * -1,
                                                                OtherIncome = m.Other_Income * -1,
                                                                CommissionIncome = m.Commission_Income * -1,
                                                                Forex = m.gain_loss_forex * -1,
                                                                BeginInventory = m.Beginning_inv,
                                                                Reacquired = m.Properties_Reacquired,
                                                                EndInventory = m.Ending_Inventory,
                                                                //CostOfSales = m.Cost_Sales,
                                                                BorrowedFunds = m.Interest_Borrowed_fund,
                                                                Salaries = m.Salary_Wages,
                                                                Staff = m.Staff_Benefits,
                                                                MonthPay = m.month_year_pay,
                                                                SSSExpense = m.SPHPG_Expense,
                                                                Uniforms = m.company_uniforms,
                                                                Rent = m.rent,
                                                                Power = m.Power_light_water,
                                                                Communication = m.Communication,
                                                                OfficeSupply = m.stationary_office_supply,
                                                                SecurityService = m.security_services,
                                                                AgencyFee = m.security_agency,
                                                                DepreciationFee = m.Depreciation_FFE,
                                                                DepreciationCe = m.Depreciation_CE,
                                                                Amortization = m.Amortization_LRI,
                                                                ProfessionalFee = m.Professional_fees,
                                                                TaxesLincenses = m.taxes_licenses,
                                                                PercentageTax = m.percentage_tax,
                                                                Prenda = m.DST_Prenda,
                                                                Remittance = m.DST_MoneyRemittance,
                                                                BankLoan = m.DST_BankLoan,
                                                                Insurance = m.Insurance,
                                                                FuelLubricants = m.Fuel_Lubricants,
                                                                TravelExpenses = m.Travelling_Expenses,
                                                                TranspoExpenses = m.Transportation_Expenses,
                                                                Maintenance1 = m.Repairs_maintenance1,
                                                                Maintenance2 = m.Repairs_maintenance2,
                                                                Advertising = m.Advertising_Publicity,
                                                                RepresentationExpense = m.Representaion_Expenses,
                                                                FreightExpense = m.Freight_Expenses,
                                                                AlarmExpense = m.Alarm_Expenses,
                                                                Marketing = m.Marketing_Expenses,
                                                                AlliedServices = m.Allied_Services,
                                                                Medical = m.Medical_Dental,
                                                                Prizes = m.Prizes_Exempt,
                                                                Donations = m.Donations,
                                                                OtherExpenses = m.Other_expenses,
                                                                Miscellaneous = m.Miscellaneous_expenses,
                                                                SPD = m.Sales_SPD * -1,
                                                                OtherSale = m.Other_Sales * -1,
                                                                DepreciationBldg = m.Depreciation_BLDG,
                                                                DepreciationTE = m.Depreciation_TE,
                                                                DepreciationOFA = m.Depreciation_OFA,
                                                                AmortizationDefCharge = m.Amortization_deferredCharges,
                                                                ProfessionalFee1 = m.Professional_Fees1,
                                                                ProfessionalFee2 = m.Professional_Fees2,
                                                                ProfessionalFeeExempt = m.Professional_Fees_Exempt,
                                                                Advertising1 = m.Advertising_Publicity1,
                                                                Advertising2 = m.Advertising_Publicity2,
                                                                Marketing1 = m.Marketing_Expenses1,
                                                                Marketing2 = m.Marketing_Expenses2,
                                                                Miscellaneous1 = m.Miscellaneous_Expenses1,
                                                                Miscellaneous2 = m.Miscellaneous_Expenses2,
                                                                Provision = m.Provision_for_income_tax
                                                            }).FirstOrDefault();
                    var BalanceSheetSrv = srvc.PartialReport(dataEntry.BranchCode, dataEntry.Zone, dataEntry.CorporateName, dataEntry.Date.ToString("yyyy-MM-dd"), dataEntryGL, "BalanceSheet");
                    BalanceSheetModel BSReport = (from m in BalanceSheetSrv.RetrieveBalanceSheetReport
                                                  select new BalanceSheetModel
                                                  {
                                                      CorporateName = dataEntry.CorporateName,
                                                      Period = "AS OF " + new DateTimeFormatInfo().GetMonthName(dataEntry.Date.Month) + ", " + dataEntry.Date.Year.ToString(),
                                                      Branch = dataEntry.Branch,
                                                      BranchAddress = dataEntry.BranchAddress,
                                                      CashEquivalents = m.Cash_Equivalents,
                                                      Merchandise = m.Merchandise_Inventory,
                                                      PledgeLoan = m.Pledge_Loans,
                                                      PastDueLoan = m.Pledge_DueLoans,
                                                      AccountReceivable = m.Accounts_ReceivablePartners,
                                                      InputTax = m.Input_Tax,
                                                      CreditableWithHoldingTax = m.Creditable_withholdigTax,
                                                      Stationery = m.Stationery_Unissued,
                                                      Deferred = m.Deferred_charges,
                                                      //TotalAssets = (m.Cash_Equivalents + m.Merchandise_Inventory + m.Pledge_Loans + m.Pledge_DueLoans +
                                                      //m.Accounts_ReceivablePartners + m.Input_Tax + m.Creditable_withholdigTax + m.Stationery_Unissued + m.Deferred_charges),
                                                      Investments = m.Investments,
                                                      OtherAssets = m.other_assets,
                                                      //TotalNonCurrentLiabilities = (m.Investments + m.other_assets),
                                                      Furniture = m.Furniture_Fixtures_Eqpt,
                                                      AccuFFE = m.Accu_Dep_FFE * -1,
                                                      //FurnitureNet = (m.Furniture_Fixtures_Eqpt + m.Accu_Dep_FFE),
                                                      CompEquipment = m.Computer_EQPT,
                                                      AccuCE = m.Accu_Dep_CE * -1,
                                                      //CompEquipmentNet = (m.Computer_EQPT + m.Accu_Dep_CE),
                                                      Leasehold = m.LeaseHoldRight_improvement,
                                                      TaxPayable = m.Percentage_Tax_Payable * -1,
                                                      PayablePR = m.DST_Payable_PR * -1,
                                                      PayableMR = m.DST_Payable_MR * -1,
                                                      OutputTax = m.Output_tax_payable * -1,
                                                      WithholdingTax = m.withholding_tax_payable * -1,
                                                      IncomeTax = m.income_tax_payable * -1,
                                                      SSS = m.sss_premium_payable * -1,
                                                      PagIbig = m.pagibig_fund_payable * -1,
                                                      PhilHealth = m.philhealth_premium_payable * -1,
                                                      //TotalCurrentLiabilities
                                                      LoanPayable = m.loans_payable * -1,
                                                      Prefunding = m.accounts_payable_prefunding * -1,
                                                      MLKP = m.accounts_payable_MLKP * -1,
                                                      Pension = m.Pension_Plan_Liability * -1,
                                                      OtherLiabilities = m.Other_Liabilities * -1,
                                                      Division = m.Dividends_payable * -1,
                                                      //TotalNonCurrentLiabilities
                                                      //TotalLiabilities
                                                      CapitalStock = m.Capital_Stock * -1,
                                                      Appropriated = m.Retained_Earnings_appropriated * -1,
                                                      UnappropriatedBeg = m.Retained_Earnings_unappropriatedBEG * -1,
                                                      AfterTax = m.Add_net_income_afterTax,
                                                      DueFromBranches = m.Due_From_Braches,
                                                      OfficeEquipment = m.Office_Equipment,
                                                      AccuOfficeEquipment = m.Accu_Dep_OF,
                                                      TransportationEquipment = m.Transportation_Equipment,
                                                      AccuTransportationEquipment = m.ACCU_DEP_TE,
                                                      OtherFixedAssets = m.Other_Fixed_Assets,
                                                      AccuOtherFixedAssets = m.ACCU_DEP_OFA,
                                                      AccuDefBuilding = m.AD_Building * -1,
                                                      Building = m.Building
                                                      //UnappropriatedEnd = (m.Capital_Stock + m.Retained_Earnings_appropriated)
                                                  }).FirstOrDefault();

                    return this.ViewPdf("", "TrialBalance", new TrialBalanceViewModel { BalanceSheet = BSReport, IncomeStatement = IncomeStatement });
                }
            }
            ViewData["Message"] = "No Report Found";
            return View("NoReport");
        }

        [HttpGet]
        [Route("DataEntry/Modify")]
        public ActionResult ModifyEntry(string entryNo, string corpName)
        {
            if (!string.IsNullOrEmpty(entryNo) && !string.IsNullOrEmpty(corpName))
            {
                AccountingService srvc = new AccountingService();
                var response = srvc.UnprocessEntriesPerRow(entryNo, corpName);
                if (response.respcode == 1)
                {
                    var range = response.dataval.Count();
                    var entries = (from m in response.dataval
                                   select new EntryModel
                                   {
                                       Credit = m.totalCredit * -1,
                                       Debit = m.totalDebit,
                                       EntryId = m.id,
                                       GL = m.GLNumber,
                                       Description = m.GLDescription,
                                       Reference = m.Reference,
                                       Remarks = m.Remarks,
                                       Add = true
                                   }).ToList();

                    DataEntryModel dataEntry = new DataEntryModel();
                    dataEntry.EntryNo = entryNo;
                    dataEntry.Branch = response.dataval.Select(m => m.branchname).FirstOrDefault();
                    dataEntry.BranchAddress = response.dataval.Select(m => m.address).FirstOrDefault();
                    dataEntry.BranchCode = response.dataval.Select(m => m.bcode).FirstOrDefault();
                    dataEntry.Zone = response.dataval.Select(m => m.zcode).FirstOrDefault();
                    dataEntry.CorporateName = corpName;
                    dataEntry.Date = Convert.ToDateTime(response.dataval.Select(m => m.transdate).FirstOrDefault());
                    dataEntry.Entries = entries;
                    return View("EditEntry", dataEntry);
                }
            }
            ViewData["Message"] = "No Record Found";
            return View("NoReport");
        }
        [HttpPost]
        [Route("DataEntry/Update")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEntry(DataEntryModel dataEntry)
        {
            AccountingService srvc = new AccountingService();
            List<Gl_Entry> entryDetails = (from m in dataEntry.Entries
                                           select new Gl_Entry
                                           {
                                               EntryNo = dataEntry.EntryNo,
                                               id = m.EntryId,
                                               bdr_hfl = m.Credit != 0 ? m.Credit * -1 : m.Debit,
                                               GLDescription = m.Description,
                                               GLNumber = m.GL,
                                               Reference = m.Reference,
                                               Remarks = m.Remarks,
                                           }
                                           ).ToList();
            var response = srvc.UpdateDataEntryInfo(dataEntry.CorporateName, entryDetails);
            if (response.respcode == 1)
            {
                return Json(new { code = response.respcode, msg = response.message, action = Url.Action("Index", "Home", Request.Url.Scheme) });
            }
            else
            {
                return Json(new { msg = response.message });
            }
        }

        [HttpGet]
        [Route("DataEntry/Remove")]
        public ActionResult Remove(string entryNo, string corpName)
        {
            if (!string.IsNullOrEmpty(entryNo.Trim()) && !string.IsNullOrEmpty(corpName.Trim()))
            {
                AccountingService srv = new AccountingService();
                var response = srv.DeleteEntryProcess(corpName, entryNo);
                if (response.respcode == 1)
                {
                    return Json(new { code = response.respcode, msg = response.message, action = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = response.message }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { msg = string.Format("Unable to DELETE Entry No. {0} ", entryNo) }, JsonRequestBehavior.AllowGet);
        }

        //[Route("PartialReport/Journal")]
        //public ActionResult Journal()
        //{
        //    AccountingService srvc = new AccountingService();
        //    var srvcResponse = srvc.Get_JournalData("MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC.", 9, 2015, "029");

        //    List<string> entryDates = (from m in srvcResponse.data select m.transdate).Distinct().ToList();
        //    List<Journal> journals;

        //    JournalViewModel journalView = new JournalViewModel();

        //    journalView.Journals = new Dictionary<int, List<Journal>>();
        //    for (int i = 1; i < entryDates.Count(); i++)
        //    {
        //        journals = (from m in srvcResponse.data
        //                    where m.transdate == entryDates[i - 1]
        //                    select new Journal
        //                    {
        //                        Credit = m.Credit,
        //                        Debit = m.Debit,
        //                        Description = m.Description,
        //                        EntryDate = m.transdate,
        //                        GL = m.GLNumber,
        //                        Reference = m.Remark,
        //                        Remarks = "REMARKS"
        //                    }).ToList();
        //        journalView.Journals.Add(i, journals);
        //    }

        //    return this.ViewPdf("", "journal", journalView);
        //}

        //[HttpPost]
        //[Route("PartialReport/GeneralLedger")]
        //public ActionResult GeneralLedger()
        //{

        //    AccountingService srvc = new AccountingService();
        //    var response = srvc.Get_LedgerData("MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC.", null, 2015, "029", "1029003");

        //    if (response.respcode == 1)
        //    {

        //        GLReportViewModel GLReports = new GLReportViewModel();
        //        GLReports.GeneralLedger = new GeneralLedgerModel { GeneralLedger = "1029003", Description = "PLEDGE LOANS - COLON 1" };
        //        GLReports.GLBeginningBalance = 2150225;
        //        GLReports.BranchCode = "029";                

        //        GLReports.GLEntrys = (from m in response.dataLedgerList
        //                              select new GLReportModel
        //                              {
        //                                  Credit = m.Credit,
        //                                  Debit = m.Debit,
        //                                  NetChange = m.Netchange,
        //                                  date = m.transdate,
        //                                  Reference = "",
        //                                  Remarks = m.Remark
        //                              }).ToList();
        //        return this.ViewPdf("", "GeneralLedger", GLReports);
        //    }
        //    ViewData["Message"] = response.message;
        //    return View("NoReport");
        //}
    }
}