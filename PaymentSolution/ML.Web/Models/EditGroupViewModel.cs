using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ML.PaymentSolution.Services.Contracts.Responses;

namespace ML.Web.Models
{
    public class EditGroupViewModel
    {
        public IEnumerable<SearchEmployeeResponse> employee { get; set; }
        public GroupViewModel group { get; set; }
        
    }
}