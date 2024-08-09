using static System.Collections.Specialized.BitVector32;

namespace emujv2Api.Constructor
{
    public class InputCons
    {

        public string loginID { get; set; }
        public string staffID { get; set; }
        public string dept { get; set; }
        public string userlevel { get; set; }
        public string staffname { get; set; }
        public string position { get; set; }
        public string updDate { get; set; }
        public string userid { get; set; }
        public string staffStatus { get; set; }
        public string section { get; set; }
        public string muj { get; set; }
        public string kmuj { get; set; }

    }

    public class R1FormCons
    {

        public string Region { get; set; }
        public string Kmuj { get; set; }
        public string Section { get; set; }


        public string Gang { get; set; }
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
