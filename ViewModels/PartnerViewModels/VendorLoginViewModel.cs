using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.PartnerViewModels
{
    public class VendorLoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string CRPUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string CRPPassWord { get; set; }
    }
    
    public class VendorUserLoggedViewModel
    {
        public int VendorLoginRowID { get; set; }
        public short VendorRowID { get; set; }
        public string VendorName { get; set; }
        public string VendorEmaiID { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string UserID { get; set; }
        public string UPass { get; set; }
        public byte UserType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public byte Status { get; set; }
    }

    public class VendorUserLoggedListPagedModel
    {
        public IEnumerable<VendorUserLoggedViewModel> VendorUser { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class UpdateVendorUserBlockStatusViewModel
    {
        public int VendorLoginRowID { get; set; }
        public string UserID { get; set; }
        public DateTime? BlockedDate { get; set; }
        public byte Status { get; set; }
    }
}
