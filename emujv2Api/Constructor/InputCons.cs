using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace emujv2Api.Constructor
{
    public class MasukCons
    {
        public string UpdBy { get; set; }
        public string DailyId { get; set; }
        public List<string> AttId { get; set; }
        public string TotalPax { get; set; }
        public List<string> Gang { get; set; }
        public string Workers { get; set; }
        public string RptCode { get; set; }

    }

    public class R1FormCons
    {
        public string User { get; set; }
        public string Region { get; set; }
        public string Kmuj { get; set; }
        public string Section { get; set; }


        public List<string> Gang { get; set; }
        public string Date { get; set; }
        public string WorkType { get; set; }
        public string Total { get; set; }
        public string TotalUnit { get; set; }


        public string TimeStart { get; set; }
        public string TimeLast { get; set; }


        public string Category { get; set; }
        public string Condition { get; set; }
        public string Adds { get; set; }


        public string TimeTaken { get; set; }
        public string KMFrom { get; set; }
        public string KMTo { get; set; }
        public string KMTotal { get; set; }


        public string Station { get; set; }
        public string SPoint { get; set; }
        public string CatDetails { get; set; }


        public string Temp { get; set; }
        public string RptCode { get; set; }
        public string Workers { get; set; }
        public string UpdBy { get; set; }
        public string UpdDate { get; set; }


    }


}
