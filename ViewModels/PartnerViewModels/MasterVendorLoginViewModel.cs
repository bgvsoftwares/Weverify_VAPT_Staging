using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ViewModels.PartnerViewModels
{
    public class MasterVendorLoginViewModel
    {
        public int VendorLoginRowID { get; set; }
        public short VendorRowID { get; set; }
        public string VendorName { get; set; }       
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string UserID { get; set; }      
        public string UserType { get; set; }
        public string CreatedBy { get; set; }       
        public byte Status { get; set; }
    }

    public class MasterVendorNameMobileViewModel
    {

        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
    }

    public class AddMasterVendorLoginViewModel
    {
        [ScaffoldColumn(false)]
        public int VendorLoginRowID { get; set; }

        [ScaffoldColumn(false)]
        public short VendorRowID { get; set; }

        [Required]
        [Display(Name = "Contact Person :")]
        public string ContactPerson { get; set; }

        [Display(Name = "Mobile No:")]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name ="User Name :")]
        public string UserID { get; set; }

        [Required]
        [Display(Name ="Password :")]
        public string UPass { get; set; }

        [Required]
        [Display(Name = "User Type :")]
        public byte UserType { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreatedTime { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedTime { get; set; }

        [ScaffoldColumn(false)]
        [AllowHtml]
        public byte Status { get; set; }

    }

    public class UpdateMasterVendorLoginViewModel
    {
        [ScaffoldColumn(false)]
        public int VendorLoginRowID { get; set; }

        [Required]
        [Display(Name = "User Name :")]
        public string UserID { get; set; } 

        [ScaffoldColumn(false)]
        public short VendorRowID { get; set; }
         
        [Required]
        [Display(Name = "Password :")]
        public string UPass { get; set; }

        [Required]
        [Display(Name = "User Type :")]
        public byte UserType { get; set; }

        [Required]
        [Display(Name = "Contact Person :")]
        public string ContactPerson { get; set; }

        [Display(Name = "Mobile No:")]
        public string MobileNo { get; set; }


        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }
      
        [ScaffoldColumn(false)]
        public DateTime? ModifiedTime { get; set; }

        [ScaffoldColumn(false)]
        public byte Status { get; set; }

    }

    public class MasterVendorLoginListPage
    {
        public IEnumerable<MasterVendorLoginViewModel> VendorLogins { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}