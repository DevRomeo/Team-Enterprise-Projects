using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class GroupViewModel
    {
        [Display(Name="ID")]
        public string groupId { get; set; }
        [Display(Name="Group Name")]
        public string groupname { get; set; }
        [Display(Name="No. Of Employee")]
        
        public int noofemployees { get; set; }
        [Display(Name="Total")]
        public string totalsalary { get; set; }
    }
}