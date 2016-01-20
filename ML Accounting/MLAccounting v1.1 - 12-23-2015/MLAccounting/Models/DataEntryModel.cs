
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AccountingWeb.Models
{
    public class DataEntryModel
    {
        [Display(Name = "Entry No. :")]
        public string EntryNo { get; set; }
        [Display(Name = "Currency ")]
        public IEnumerable<SelectListItem> Currency
        {
            get
            {
                return new[]
                {
                   new  SelectListItem{ Value="PHP", Text="PHP"},
                   new SelectListItem{ Value="USD",Text="USD"}
                };
            }
        }
        [Display(Name = "Branches :")]
        public IEnumerable<SelectListItem> Branches
        {
            get
            {
                return new[]
                {
                    new SelectListItem{ Value="", Text =""}
                };
            }
        }
        [Display(Name = "Corporate Names :")]
        public IEnumerable<SelectListItem> CorpName
        {
            get
            {
                return new[] 
                {
                    new SelectListItem{ Value="", Text=""}                    
                };
            }
        }
        public EntryModel Entry { get; set; }
        public List<EntryModel> Entries { get; set; }
        [Display(Name = "Date :")]
        public DateTime Date { get; set; }
        [Display(Name = "Branch :")]
        public string Branch { get; set; }
        [Display(Name = "Corporate Name :")]
        public string CorporateName { get; set; }

        public string BranchAddress { get; set; }

        public string BranchCode { get; set; }
        public string Zone { get; set; }
    }
}