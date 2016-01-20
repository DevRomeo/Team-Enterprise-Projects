using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ML.Web.Models
{
    public class ReportModel
    {
        [Display(Name = "History")]
        public IEnumerable<SelectListItem> historyList
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Value = "All",Text = "Payroll History (All)" },
                    new SelectListItem {Value = "Group", Text = "Payroll History (By Group)"},
                    new SelectListItem{ Value="AllCancel",Text ="Payroll History (All Cancel) " },
                    new SelectListItem{Value = "CancelGroup",Text= "Payroll History (By Cancelled Group)"}

                };
            }
        }

        [Display(Name = "Group")]
        public IEnumerable<SelectListItem> items { get; set; }

        [Display(Name = "Date")]
        public string selectedDate { get; set; }
        public string selectedmonth { get; set; }
        public IEnumerable<SelectListItem> month { get; set; }

        public int selectedday { get; set; }

        [Display(Name = "Day")]
        public IEnumerable<SelectListItem> day { get; set; }

        public int selectedyear { get; set; }
        [Display(Name = "Year")]
        public IEnumerable<SelectListItem> year { get; set; }


        public DateTime dateselected { get; set; }

        [Display(Name = "KPTN")]
        public string KPTN { get; set; }


        public string employer { get; set; }
        public int counter { get; set; }
        public string message { get; set; }
        public string datecreate { get; set; }
        public string selectedHistory { get; set; }


        //public IEnumerable<SelectListItem> groupReports { get; set; }


        [Display(Name = "Invoice No.")]
        public string invoicenumber { get; set; }
        [Display(Name = "Group/Company")]
        public string groupname { get; set; }
        [Display(Name = "No. of Employee")]
        public int noofemployee { get; set; }
        [Display(Name = "Amount")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public double? amount { get; set; }
        //public decimal? amount { get; set; }
        [Display(Name = "Service Charge")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        //public decimal? charge { get; set; }
        public double? charge { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Processed")]
        public string processed { get; set; }


    }
}