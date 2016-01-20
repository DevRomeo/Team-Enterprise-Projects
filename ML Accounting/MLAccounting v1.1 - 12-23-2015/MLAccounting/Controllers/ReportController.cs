using AccountingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PDFGeneration;
using ML.AccountingSystemV1.Service;
using ML.AccountingSystemV1.Contract.Parameters;
using System.Globalization;
using Accounting.Models;
using ML.AccountingSystemV1.Contract;

namespace AccountingWeb.Controllers
{
    [HandleError(View = "UnexpectedError")]
    [Authorize]
    public class ReportController : PdfViewController
    {
        private readonly iAccountingService service;

        public ReportController(iAccountingService service)
        {
            this.service = service;
        }

        //
        // GET: /Report/
        [Route("Report/Financial")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Report/Component")]
        [HttpGet]
        [OutputCache(Duration = 30)]
        public ActionResult ReportComponent()
        {
            //AccountingService srvc = new AccountingService();
            var dataEntryComponent = this.service.CorporanameReturnValue();

            return Json(new
            {
                code = dataEntryComponent.respcode,
                component = dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC." && m.branchName == "ML COLON 1"),
                corporatenames = dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC.").Select(m => m.Corpname).Distinct().ToArray(),
                page = PartialToString.RenderPartialViewToString(this, "_PartialReportComponent", new ReportModel())
            }, JsonRequestBehavior.AllowGet);
        }
        [Route("Report/View")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateReport(ReportModel report)
        {
            if (ModelState.IsValid)
            {
                switch (report.ReportType)
                {
                    case "BALANCE SHEET":
                        return this.BalanceSheet(report);
                    case "INCOME STATEMENT":
                        return this.IncomeStatement(report);
                    case "TRIAL BALANCE":
                        return this.TrialBalance(report);
                    default:
                        return Json(new { Message = "Unknown Report Type" });
                }
            }
            else
            {
                return Json(new { Message = "Unknown Report Type" });
            }
        }

        private ActionResult IncomeStatement(ReportModel report)
        {
            IncomeStatementModel IncomeStatement = new IncomeStatementModel();
            //AccountingService srvc = new AccountingService();

            var response = this.service.GenerateIncomeStatement(report.BranchCode, report.Zone, report.CorporateName, new DateTime(Convert.ToInt32(report.Year), DateTime.ParseExact(report.Month, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("MM-dd-yyyy"));
            if (response.respcode == 1)
            {
                IncomeStatement = (from m in response.RetrieveIncomeStatementReport
                                   select new IncomeStatementModel
                                   {
                                       CorporateName = report.CorporateName,
                                       Month = report.Month,
                                       Year = report.Year,
                                       Branch = report.Branch,
                                       BranchAddress = report.Address,
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
            }
            //return this.ViewPdf("", "IncomeStatement", IncomeStatement);
            return File(this.BufferPdf("", "IncomeStatement", IncomeStatement), "application/pdf", string.Format("IncomeStatement {0}.pdf", DateTime.Now));
        }

        private ActionResult BalanceSheet(ReportModel report)
        {
            //AccountingService srvc = new AccountingService();
            BalanceSheetModel BS = new BalanceSheetModel();
            var response = this.service.GenerateBalanceSheet(report.BranchCode, report.Zone, report.CorporateName, new DateTime(Convert.ToInt32(report.Year), DateTime.ParseExact(report.Month, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("MM-dd-yyyy"));
            if (response.respcode == 1)
            {
                BS = (from m in response.RetrieveBalanceSheetReport
                      select new BalanceSheetModel
                      {
                          CorporateName = report.CorporateName,
                          Period = "AS OF " + report.Month + ", " + report.Year,
                          Branch = report.Branch,
                          BranchAddress = report.Address,
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
                          AccuFFE = m.Accu_Dep_FFE,
                          //FurnitureNet = (m.Furniture_Fixtures_Eqpt + m.Accu_Dep_FFE),
                          CompEquipment = m.Computer_EQPT,
                          AccuCE = m.Accu_Dep_CE,
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
                          TransportationEquipment = m.Transportation_Equipment,
                          OtherFixedAssets = m.Other_Fixed_Assets,
                          OfficeEquipment = m.Office_Equipment,
                          DueFromBranches = m.Due_From_Braches,
                          AccuTransportationEquipment = m.ACCU_DEP_TE,
                          AccuOtherFixedAssets = m.ACCU_DEP_OFA,
                          AccuOfficeEquipment = m.Accu_Dep_OF,
                          Building = m.Building,
                          AccuDefBuilding = m.AD_Building * -1
                      }).FirstOrDefault();
            }
            //return this.ViewPdf("", "BalanceSheet", BS);
            return File(this.BufferPdf("", "BalanceSheet", BS), "application/pdf", string.Format("BalanceSheet {0}.pdf", DateTime.Now));
        }

        private ActionResult TrialBalance(ReportModel report)
        {
            //AccountingService srvc = new AccountingService();
            BalanceSheetModel BS = new BalanceSheetModel();
            IncomeStatementModel IncomeStatement = new IncomeStatementModel();
            var response = this.service.GenerateBalanceSheet(report.BranchCode, report.Zone, report.CorporateName, new DateTime(Convert.ToInt32(report.Year), DateTime.ParseExact(report.Month, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("MM-dd-yyyy"));
            if (response.respcode == 1)
            {
                BS = (from m in response.RetrieveBalanceSheetReport
                      select new BalanceSheetModel
                      {
                          CorporateName = report.CorporateName,
                          Period = "AS OF " + report.Month + ", " + report.Year,
                          Branch = report.Branch,

                          BranchAddress = report.Address,
                          CashEquivalents = m.Cash_Equivalents,
                          Merchandise = m.Merchandise_Inventory,
                          PledgeLoan = m.Pledge_Loans,
                          PastDueLoan = m.Pledge_DueLoans,
                          AccountReceivable = m.Accounts_ReceivablePartners,
                          InputTax = m.Input_Tax,
                          CreditableWithHoldingTax = m.Creditable_withholdigTax,
                          Stationery = m.Stationery_Unissued,
                          Deferred = m.Deferred_charges,
                          Investments = m.Investments,
                          OtherAssets = m.other_assets,
                          Furniture = m.Furniture_Fixtures_Eqpt,
                          AccuFFE = m.Accu_Dep_FFE * -1,
                          CompEquipment = m.Computer_EQPT,
                          AccuCE = m.Accu_Dep_CE * -1,
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
                          LoanPayable = m.loans_payable * -1,
                          Prefunding = m.accounts_payable_prefunding * -1,
                          MLKP = m.accounts_payable_MLKP * -1,
                          Pension = m.Pension_Plan_Liability * -1,
                          OtherLiabilities = m.Other_Liabilities * -1,
                          Division = m.Dividends_payable * -1,
                          CapitalStock = m.Capital_Stock * -1,
                          Appropriated = m.Retained_Earnings_appropriated * -1,
                          UnappropriatedBeg = m.Retained_Earnings_unappropriatedBEG * -1,
                          AfterTax = m.Add_net_income_afterTax,
                          AccuOfficeEquipment = m.Accu_Dep_OF,
                          AccuOtherFixedAssets = m.ACCU_DEP_OFA,
                          AccuTransportationEquipment = m.ACCU_DEP_TE,
                          DueFromBranches = m.Due_From_Braches,
                          OfficeEquipment = m.Office_Equipment,
                          OtherFixedAssets = m.Other_Fixed_Assets,
                          TransportationEquipment = m.Transportation_Equipment,
                          AccuDefBuilding = m.AD_Building * -1,
                          Building = m.Building
                      }).FirstOrDefault();
            }

            var IncomeStatementResponse = this.service.GenerateIncomeStatement(report.BranchCode, report.Zone, report.CorporateName, new DateTime(Convert.ToInt32(report.Year), DateTime.ParseExact(report.Month, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("MM-dd-yyyy"));
            if (IncomeStatementResponse.respcode == 1)
            {
                IncomeStatement = (from m in IncomeStatementResponse.RetrieveIncomeStatementReport
                                   select new IncomeStatementModel
                                   {
                                       CorporateName = report.CorporateName,
                                       Month = report.Month,
                                       Year = report.Year,
                                       Branch = report.Branch,
                                       BranchAddress = report.Address,
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
                                       Advertising1 = m.Advertising_Publicity1,
                                       Advertising2 = m.Advertising_Publicity2,
                                       AmortizationDefCharge = m.Amortization_deferredCharges,
                                       DepreciationBldg = m.Depreciation_BLDG,
                                       DepreciationOFA = m.Depreciation_OFA,
                                       DepreciationTE = m.Depreciation_TE,
                                       Marketing1 = m.Marketing_Expenses1,
                                       Marketing2 = m.Marketing_Expenses2,
                                       Miscellaneous1 = m.Miscellaneous_Expenses1,
                                       Miscellaneous2 = m.Miscellaneous_Expenses2,
                                       OtherSale = m.Other_Sales * -1,
                                       ProfessionalFee = m.Professional_fees,
                                       ProfessionalFee1 = m.Professional_Fees1,
                                       ProfessionalFee2 = m.Professional_Fees2,
                                       ProfessionalFeeExempt = m.Professional_Fees_Exempt,
                                       Provision = m.Provision_for_income_tax,
                                       SPD = m.Sales_SPD * -1
                                   }).FirstOrDefault();
            }
            return File(this.BufferPdf("", "TrialBalance", new TrialBalanceViewModel { BalanceSheet = BS, IncomeStatement = IncomeStatement }), "application/pdf", string.Format("Trial Balance {0}.pdf", DateTime.Now));
        }

        [Route("Report/BooksOfAccounts")]
        public ActionResult AccountsView()
        {
            return View("Accounts");
        }
        [Route("AccountsComponent")]
        [OutputCache(Duration = 30)]
        [HttpGet]
        public ActionResult BooksOfAccouts()
        {
            //AccountingService srvc = new AccountingService();
            var dataEntryComponent = this.service.CorporanameReturnValue();

            List<GeneralLedgerModel> GeneralLedger = (from m in this.service.GlAccount_Description().data
                                                      select new GeneralLedgerModel
                                                      {
                                                          GeneralLedger = m.GLcode,
                                                          Description = m.Description
                                                      }).ToList();

            return Json(new
            {
                code = dataEntryComponent.respcode,
                account = PartialToString.RenderPartialViewToString(this, "_partialGL", GeneralLedger),
                component = dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC." && m.branchName == "ML COLON 1"),
                corporatenames = dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC.").Select(m => m.Corpname).Distinct().ToArray(),
                page = PartialToString.RenderPartialViewToString(this, "_BooksOfAccounts", new BooksOfAccountModel())
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("Reports/BooksOfAccounts")]
        public ActionResult BooksOfAccout(BooksOfAccountModel data)
        {
            if (ModelState.IsValid)
            {
                if (data.Report == "GENERAL LEDGER")
                {
                    return this.Ledger(data);
                }
                if (data.Report == "JOURNAL")
                {
                    return this.Journal(data);
                }
                if (data.Report == "CLOSING OF ACCOUNTS")
                {
                    return this.ClosingOfAccounts(data);
                }
            }

            return Json(new { Message = "Unknown Report Type" });
        }

        private ActionResult Ledger(BooksOfAccountModel data)
        {
            //AccountingService srvc = new AccountingService();
            var response = this.service.Get_LedgerData(data.CorporateName, data.Month, Convert.ToInt32(data.Year), data.BranchCode, data.GLAccount);
            
            if (response.respcode == 1)
            {
                GLReportViewModel GLReports = new GLReportViewModel();
                GLReports.GeneralLedger = new GeneralLedgerModel { GeneralLedger = data.GLAccount, Description = data.GLDescription };
                GLReports.BranchCode = data.BranchCode;
                GLReports.BranchName = data.Branch;
                GLReports.Corporate = data.CorporateName;


                //fiscal year
                if (data.Month == null)
                {
                    GLReports.periodFrom = new DateTime(Convert.ToInt32(data.Year), 1, 31).ToString("MMM dd yyyy");
                    GLReports.periodTo = new DateTime(Convert.ToInt32(data.Year), 12, 31).ToString("MMM dd yyyy");
                }
                else
                {
                    GLReports.periodFrom = new DateTime(Convert.ToInt32(data.Year), Convert.ToInt32(data.Month), DateTime.DaysInMonth(Convert.ToInt32(data.Year), Convert.ToInt32(data.Month))).ToString("MMM dd yyyy");
                    GLReports.periodTo = new DateTime(Convert.ToInt32(data.Year), Convert.ToInt32(data.Month), DateTime.DaysInMonth(Convert.ToInt32(data.Year), Convert.ToInt32(data.Month))).ToString("MMM dd yyyy");
                }

                GLReports.GLEntrys = (from m in response.dataLedgerList
                                      orderby Convert.ToDateTime(m.transdate), m.resetNoOfsameDate descending
                                      select new GLReportModel
                                      {
                                          Credit = m.Credit,
                                          Debit = m.Debit,
                                          NetChange = m.totalNetchange,
                                          date = m.transdate,
                                          Reference = m.Reference,
                                          Remarks = m.Remark,
                                          BeginningBalance = m.SumBegBalance,
                                          isLastDayOfMonth = m.resetNoOfsameDate
                                      }).ToList();
                GLReports.GLBeginningBalance = response.dataLedgerList.Select(m => m.BegBalance).FirstOrDefault();
                return File(this.BufferPdf("", "GeneralLedger", GLReports), "application/pdf", string.Format("General Ledger {0}.pdf", DateTime.Now));
            }

            ViewData["Message"] = response.message;
            return View("NoReport");
        }

        private ActionResult Journal(BooksOfAccountModel data)
        {
            //AccountingService srvc = new AccountingService();
            var srvcResponse = this.service.Get_JournalData(data.CorporateName, Convert.ToInt32(data.Month), Convert.ToInt32(data.Year), data.BranchCode);

            if (srvcResponse.respcode == 1)
            {
                List<string> entryDates = (from m in srvcResponse.data select m.transdate).Distinct().ToList();
                List<Journal> journals;

                int year = Convert.ToInt32(data.Year), month = Convert.ToInt32(data.Month);
                
                JournalViewModel journalView = new JournalViewModel();
                journalView.Corporate = data.CorporateName;
                journalView.BatchNo = month;
                journalView.EntryDate = month + " " + new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("MMMM yyyy - ") + data.Branch;
                journalView.CreationDate = new DateTime(year, 12, 31).ToString("MMM dd yy");
                journalView.BranchCode = data.BranchCode;
                journalView.BranchName = data.Branch;
                journalView.Journals = new Dictionary<int, List<Journal>>();
                for (int i = 1; i <= entryDates.Count(); i++)
                {
                    journals = (from m in srvcResponse.data
                                where m.transdate == entryDates[i - 1]
                                select new Journal
                                {
                                    Credit = m.Credit,
                                    Debit = m.Debit,
                                    Description = m.Description,
                                    EntryDate = m.transdate,
                                    GL = m.GLNumber,
                                    Reference = m.Reference,
                                    Remarks = m.Remark
                                }).ToList();
                    journalView.Journals.Add(i, journals);
                }

                return File(this.BufferPdf("", "journal", journalView), "application/pdf", string.Format("Journal {0}.pdf", DateTime.Now));
            }
            ViewData["Message"] = srvcResponse.message;
            return View("NoReport");
        }

        private ActionResult ClosingOfAccounts(BooksOfAccountModel report)
        {
            //AccountingService srvc = new AccountingService();
            BalanceSheetModel BS = new BalanceSheetModel();
            IncomeStatementModel IncomeStatement;
            String Month = "December";
            var ClosingOfAccounts = this.service.Closing_Accounts(report.BranchCode, report.Zone, report.CorporateName, new DateTime(Convert.ToInt32(report.Year), 12, 1).ToString("MM-dd-yyyy"));
            if (ClosingOfAccounts.respcode == 1)
            {
                IncomeStatement = (from m in ClosingOfAccounts.Incomestatement
                                   select new IncomeStatementModel
                                   {
                                       CorporateName = report.CorporateName,
                                       Month = Month,
                                       Year = report.Year,
                                       Branch = report.Branch,
                                       BranchAddress = report.Address,
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
                                       Advertising1 = m.Advertising_Publicity1,
                                       Advertising2 = m.Advertising_Publicity2,
                                       AmortizationDefCharge = m.Amortization_deferredCharges,
                                       DepreciationBldg = m.Depreciation_BLDG,
                                       DepreciationOFA = m.Depreciation_OFA,
                                       DepreciationTE = m.Depreciation_TE,
                                       Marketing1 = m.Marketing_Expenses1,
                                       Marketing2 = m.Marketing_Expenses2,
                                       Miscellaneous1 = m.Miscellaneous_Expenses1,
                                       Miscellaneous2 = m.Miscellaneous_Expenses2,
                                       OtherSale = m.Other_Sales * -1,
                                       ProfessionalFee1 = m.Professional_Fees1,
                                       ProfessionalFee2 = m.Professional_Fees2,
                                       ProfessionalFeeExempt = m.Professional_Fees_Exempt,
                                       Provision = m.Provision_for_income_tax,
                                       SPD = m.Sales_SPD * -1
                                   }).FirstOrDefault();


                BS = (from m in ClosingOfAccounts.BalanceSheet
                      select new BalanceSheetModel
                      {
                          CorporateName = report.CorporateName,
                          Period = "For the year ended December 31, " + report.Year,
                          Branch = report.Branch,
                          BranchAddress = report.Address,
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
                          AccuFFE = m.Accu_Dep_FFE,
                          //FurnitureNet = (m.Furniture_Fixtures_Eqpt + m.Accu_Dep_FFE),
                          CompEquipment = m.Computer_EQPT,
                          AccuCE = m.Accu_Dep_CE,
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
                          AfterTax = IncomeStatement.NetIncomeAfterTax,// m.Add_net_income_afterTax,//Income Statement Net Income After Tax
                          TransportationEquipment = m.Transportation_Equipment,
                          OtherFixedAssets = m.Other_Fixed_Assets,
                          OfficeEquipment = m.Office_Equipment,
                          DueFromBranches = m.Due_From_Braches,
                          AccuTransportationEquipment = m.ACCU_DEP_TE,
                          AccuOtherFixedAssets = m.ACCU_DEP_OFA,
                          AccuOfficeEquipment = m.Accu_Dep_OF,
                          Building = m.Building,
                          AccuDefBuilding = m.AD_Building * -1
                      }).FirstOrDefault();
                return File(this.BufferPdf("", "BalanceSheet", BS), "application/pdf", string.Format("Closing of Accounts {0}.pdf", DateTime.Now));

            }
            else
            {
                ViewData["Message"] = ClosingOfAccounts.message;
                return View("NoReport");
            }
        }
    }
}