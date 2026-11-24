using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class TeamProductivityReportViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public short SubCheckRowID { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<string> TimeSlotLabels { get; set; } = new List<string>();
        public Dictionary<string, int> TimeSlotCounts { get; set; }
        public int TotalCount { get; set; }
    }

    public static class TimeSlotHelper
    {
        public static readonly List<Tuple<TimeSpan, TimeSpan, string>> TimeSlots = new List<Tuple<TimeSpan, TimeSpan, string>>()
            {
                Tuple.Create(new TimeSpan(0,0,0), new TimeSpan(9,30,59).Add(TimeSpan.FromMilliseconds(999)),"12:01AM-09:30AM"),
                Tuple.Create(new TimeSpan(9,31,0), new TimeSpan(10,30,59).Add(TimeSpan.FromMilliseconds(999)),"09:31AM-10:30AM"),
                Tuple.Create(new TimeSpan(10,31,0), new TimeSpan(11,30,59).Add(TimeSpan.FromMilliseconds(999)),"10:31AM-11:30AM"),
                Tuple.Create(new TimeSpan(11,31,0), new TimeSpan(12,30,59).Add(TimeSpan.FromMilliseconds(999)),"11:31AM-12:30PM"),
                Tuple.Create(new TimeSpan(12,31,0), new TimeSpan(13,30,59).Add(TimeSpan.FromMilliseconds(999)),"12:31PM-01:30PM"),
                Tuple.Create(new TimeSpan(13,31,0), new TimeSpan(14,30,59).Add(TimeSpan.FromMilliseconds(999)),"01:31PM-02:30PM"),
                Tuple.Create(new TimeSpan(14,31,0), new TimeSpan(15,30,59).Add(TimeSpan.FromMilliseconds(999)),"02:31PM-03:30PM"),
                Tuple.Create(new TimeSpan(15,31,0), new TimeSpan(16,30,59).Add(TimeSpan.FromMilliseconds(999)),"03:31PM-04:30PM"),
                Tuple.Create(new TimeSpan(16,31,0), new TimeSpan(17,30,59).Add(TimeSpan.FromMilliseconds(999)),"04:31PM-05:30PM"),
                Tuple.Create(new TimeSpan(17,31,0), new TimeSpan(18,30,59).Add(TimeSpan.FromMilliseconds(999)),"05:31PM-06:30PM"),
                Tuple.Create(new TimeSpan(18,31,0), new TimeSpan(19,30,59).Add(TimeSpan.FromMilliseconds(999)),"06:31PM-07:30PM"),
                Tuple.Create(new TimeSpan(19,31,0), new TimeSpan(20,30,59).Add(TimeSpan.FromMilliseconds(999)),"07:31PM-08:30PM"),
                Tuple.Create(new TimeSpan(20,31,0), new TimeSpan(23,59,59).Add(TimeSpan.FromMilliseconds(999)),"08:31PM-12:00AM"),
        };
    }
 
    public class UserCheckDetailViewModel
    {
        public string ClientName { get; set; }
        public string CandidateName { get; set; }
        public string SubCheckName { get; set; }
        public string UniqueComponentID { get; set; }
        public string CompanyRefNo { get; set; }
        public string UpdatedByNameDesig { get; set; }
        public string CheckStatus { get; set; }
        public string Remarks { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TimeSlotLabel { get; set; }
    }

    public class TeamProductivityExportViewModel
    {
        public string UserName { get; set; }
        public Dictionary<string, int> TimeSlotCounts { get; set; }
        public int Total { get; set; }
    }

}
