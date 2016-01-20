using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountingWeb.Models
{
    public class BooksOfAccountModel
    {
        public IEnumerable<SelectListItem> CorporateNames
        {
            get
            {
                return new[]
                {                    
                    new SelectListItem { Value="", Text=""}                    
                };
            }
        }
        public IEnumerable<SelectListItem> Branchs
        {
            get
            {
                return new[]
                {
                    new SelectListItem{Value="", Text=""}
                };
            }
        }
        public IEnumerable<SelectListItem> Months
        {

            get
            {
                return Enumerable.Range(1, 12)
                    .Select(x => new SelectListItem
                    {
                        Text = new DateTimeFormatInfo().GetMonthName(x),
                        //Value = new DateTimeFormatInfo().GetMonthName(x)
                        Value = x.ToString()
                    }).ToArray();
            }

        }
        public IEnumerable<SelectListItem> Years
        {
            get
            {
                return Enumerable.Range(2015, DateTime.Now.Year - 2014)
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    }).OrderBy(x => x.Text).ToArray();
            }
        }
        public IEnumerable<SelectListItem> Reports
        {
            get
            {
                return new[]
                {
                    new SelectListItem{Value="", Text=""},
                    new SelectListItem{Value="JOURNAL", Text="JOURNAL"},
                    new SelectListItem{Value="GENERAL LEDGER", Text="GENERAL LEDGER"},
                    new SelectListItem{Value="CLOSING OF ACCOUNTS", Text="CLOSING OF ACCOUNTS"}                    
                };
            }
        }

        [Display(Name = "Report Type")]
        public string Report { get; set; }
        [Display(Name = "Corporate Name")]
        public string CorporateName { get; set; }
        [Display(Name = "Branch")]
        public string Branch { get; set; }
        [Display(Name = "Month")]
        public int? Month { get; set; }
        [Display(Name = "Year")]
        public string Year { get; set; }
        [Display(Name = "GL Number")]
        public string GLAccount { get; set; }
        public string GLDescription { get; set; }
        public string Address { get; set; }
        public string BranchCode { get; set; }
        public string Zone { get; set; }

    }
}