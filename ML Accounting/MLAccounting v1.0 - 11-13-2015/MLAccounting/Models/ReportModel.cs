using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AccountingWeb.Models
{
    public class ReportModel
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
                        Value = new DateTimeFormatInfo().GetMonthName(x)
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
                    new SelectListItem{Value="TRIAL BALANCE", Text="TRIAL BALANCE"},
                    new SelectListItem{Value="BALANCE SHEET", Text="BALANCE SHEET"},
                    new SelectListItem{Value="INCOME STATEMENT", Text="INCOME STATEMENT"}
                    
                };
            }
        }
        [Display(Name = "Corporate Name")]
        public string CorporateName { get; set; }
        [Display(Name = "Branch")]
        public string Branch { get; set; }
        [Display(Name = "Month")]
        public string Month { get; set; }
        [Display(Name = "Year")]
        public string Year { get; set; }
        [Display(Name = "Report Type")]
        public string ReportType { get; set; }
        public string Address { get; set; }
        public string BranchCode { get; set; }
        public string Zone { get; set; }

    }
}