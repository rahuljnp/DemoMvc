using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoMvc.Models
{
    public class tbl_CategoryValidation
    {
        //this class need to create for validation and preserve extra added 
        //paramenters so that it will not remove from model when it is updated
        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]

        public string Description { get; set; }
    }

    [MetadataType(typeof(tbl_CategoryValidation))]
    public partial class tbl_Category
    {

    }
}
