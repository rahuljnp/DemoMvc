using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoMvc.Models
{
    public class tbl_UserValidation
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        //[Required]
        public string Gender { get; set; }
        public Nullable<bool> IsInterestedIncSharp { get; set; }
        public Nullable<bool> IsInterestedInJava { get; set; }
        public Nullable<bool> IsInterestedInRuby { get; set; }
       // [Required]
        public Nullable<int> CityId { get; set; }
        public SelectList CityList { get; set; }
       // [Required]
        public Nullable<System.DateTime> DOB { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase UserImageFile { get; set; }
        public virtual tbl_City tbl_City { get; set; }
    }
    [MetadataType(typeof(tbl_UserValidation))]
    public partial class tbl_User
    {
    }
}