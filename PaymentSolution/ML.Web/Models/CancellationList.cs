﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class CancellationList
    {
        public CancellationModel Cancellation { get; set; }
        public List<CancellationModel> Cancellations { get; set; }
    }
}