using System;
using System.Collections.Generic;

namespace GtechTask.Models
{
    public class EMIUser
    {
        public string PlaneName { get; set; }
        public int Tenure { get; set; }
        public decimal ROI { get; set; }
        public int LoanAmount { get; set; }
        public DateTime LoanDate { get; set; }
        public List<EMI> EMISchedule { get; set; }
        public bool ScheduleGenerated { get; set; }

        public EMIUser()
        {
            EMISchedule = new List<EMI>();
        }
    }

    public class EMI
    {
        public int EmiNo { get; set; }
        public DateTime DueDate { get; set; }
        public double EmiAmount { get; set; }
    }

    
}
