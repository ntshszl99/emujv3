﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace emujv2Api.Constructor
{
    public class UserCons
    {
        public string Userid { get; set; }
        public string StaffId { get; set; }
        public string Nama { get; set; }
        public string Grade { get; set; }
        public string Designation { get; set; }
        public string YrsService { get; set; }
        public string IC { get; set; }
        public string Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string Deptid { get; set; }
        public string DeptName { get; set; }
        public string Levelid { get; set; }
        public string UserLevel { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }
        public string RegionEng { get; set; }
        public string kmujEng { get; set; }
        public string KMUJ { get; set; }
        public string Section { get; set; }
        public string RegionID { get; set; }
        public string KMUJVal { get; set; }
        public string SectionVal { get; set; }
        public string StatusCuti { get; set; }
        public string GangId { get; set; }
        public string UpdBy { get; set; }
        public string TokenAdmin { get; set; }
        public string ErrCode { get; set; }
        public string ErrDtl { get; set; }

    }
    public class GangPaxRequest
    {
        public string Kmuj { get; set; }
        public string Section { get; set; }
        public List<string> Gang { get; set; }
    }

}
