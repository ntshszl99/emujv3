using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace emujv2.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please Provide StaffID", AllowEmptyStrings = false)]
        public string StaffID { get; set; }

        [Required(ErrorMessage = "Please provide password", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public bool Password { get; set; }
      
    }
}