using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ViewModels.ClientViewModel
{
    public class ClientPackageViewModel
    {
        [Display(Name = "ID")]
        public int ClientPackageRowID { get; set; }

        [Display(Name = "Package Name")]
        public string ClientPackageName { get; set; }

        [Display(Name = "Client Name")]
        public short ClientRowID { get; set; }

        [Display(Name = "Check Family")]
        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }

        [Display(Name = "Sub Check")]
        public short SubCheckRowID { get; set; }
        public string SubCheckFamillyName { get; set; }

        [Display(Name = "TAT")]
        public byte TAT { get; set; }

        [Display(Name = "Internal TAT")]
        public byte InternalTAT { get; set; }

        [Display(Name = "Billing Per Check")]
        public double BillingPerCheck { get; set; }

        [Display(Name = "Cost Per Check")]
        public double CostPerCheck { get; set; }

        [Display(Name = "Report Sequence")]
        public byte ReportSequence { get; set; }

        [Display(Name = "Antecedents")]
        public byte AntecedentSelected { get; set; }

        [Display(Name = "Template")]
        public byte SelectedTemplate { get; set; }

        [Display(Name = "Status")]
        public byte Status { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? AddedDate { get; set; }
        public string CheckStatusId { get; set; }
        [Required]
        [Display(Name = "ExternalTAT: ")]
        public string ExternalTAT { get; set; }
        [Required]
        [Display(Name = "Price : ")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Package Name In Report")]
        [Display(Name = "Package Short Name : ")]
        public string ClientPackageShortName { get; set; }
        public string Visiblity { get; set; }
    }

    public class AddClientPackageViewModel
    {
        [Display(Name = "ID : ")]
        public int ClientPackageRowID { get; set; }

        [Required(ErrorMessage ="Enter Package Name")]
        [Display(Name = "Package Name : ")]
        public string ClientPackageName { get; set; }

        [Required(ErrorMessage = "Enter Package Short Name")]
        [Display(Name = "Package Short Name : ")]
        public string ClientPackageShortName { get; set; }

        [Display(Name = "Client Name : ")]
        public short ClientRowID { get; set; }

        [Display(Name = "Check Family : ")]
        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyID { get; set; }
        
        [Display(Name = "Sub Check : ")]
        public short SubCheckRowID { get; set; }
        public string SubCheckID { get; set; }

        public string CheckNameInReport { get; set; }

        [Display(Name = "TAT : ")]
        public byte TAT { get; set; }

        [Display(Name = "Internal TAT : ")]
        public byte InternalTAT { get; set; }

        [Display(Name = "Billing Per Check : ")]
        public double BillingPerCheck { get; set; }

        [Display(Name = "Cost Per Check : ")]
        public double CostPerCheck { get; set; }

        [Display(Name = "Report Sequence : ")]
        public byte ReportSequence { get; set; }

        [Display(Name = "Antecedents : ")]
        public byte AntecedentSelected { get; set; }

        public byte DispositionSelected { get; set; }

        [Display(Name = "Template : ")]
        public byte SelectedTemplate { get; set; }

        [Display(Name = "Status : ")]
        public byte Status { get; set; }

        [Display(Name = "Created Date : ")]
        public DateTime? AddedDate { get; set; }

        [Required]
        [Display(Name = "Price : ")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "InternalTAT: ")]
        public string InternalTATPack { get; set; }

        [Required]
        [Display(Name = "ExternalTAT: ")]
        public string ExternalTAT { get; set; }

        [Required]
        [Display(Name = "IsTATInWorkingDays : ")]
        [MaxLength(20)]
        public string IsTATInWorkingDays { get; set; }
    }

    public class ClientPackageListPagedModel
    {
        public IEnumerable<ClientPackageViewModel> ClientPackages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class PackageListPagedModel
    {
        public IEnumerable<ClientPackageViewModel> Packages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class PackagesDDLViewModel
    {
        [Display(Name = "Package Name")]
        public string PackageNameTxt { get; set; }

        public string PackageNameVal { get; set; }
    }

    public class ExportClientPackageViewModel
    {
        public int ClientPackageRowID { get; set; }
        public string ClientPackageName { get; set; }
        public string CheckFamilyName { get; set; }
        public string SubCheckFamillyName { get; set; }
        public byte ReportSequence { get; set; }
        public byte AntecedentSelected { get; set; }
        public string PackageVisiblity { get; set; }
    }


    public class EditPQClientChecksViewModel
    {    
        public int ClientCheckRowID { get; set; }
        public int ClientRowID { get; set; }
        public string ClientName { get; set; }
        public int CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; } 
        public int SubCheckRowID { get; set; }     
        public string ClientPackageName { get; set; }
        [Display(Name = "Sub Check Name")]
        public string SubCheckFamilyName { get; set; }
        [Display(Name = "Check Name In Report")]
        public string CheckNameInReport { get; set; }
        [Display(Name = "Cost Per Check")]
        public decimal CostPerCheck { get; set; }
        [Display(Name = "TAT")]
        public byte TAT { get; set; }
        [Display(Name = "Internal TAT")]
        public byte InternalTAT { get; set; }
        [Display(Name = "Billing Per Check")]
        public decimal BillingPerCheck { get; set; }
        [Display(Name = "Status")]
        public bool Status { get; set; }
    }

    public class EditPackageClientChecksViewModel
    {
        public int ClientPackageRowID { get; set; }
        [Display(Name = "Package Name")]
        public string ClientPackageName { get; set; }
        [Display(Name = "Package Name In Report")]
        public string PackageNameInReport { get; set; }
        [Display(Name = "Check Name In Report")]
        public string CheckNameInReport { get; set; }        
        public int ClientRowID { get; set; }
        public int CheckFamilyRowID { get; set; }

        public string CheckFamilyName { get; set; }
        public int SubCheckRowID { get; set; }
        [Display(Name = "Sub Check Name")]
        public string SubCheckFamilyName { get; set; }        
        [Display(Name = "TAT")]
        public byte TAT { get; set; }
        [Display(Name = "Internal TAT")]
        public byte InternalTAT { get; set; }
        [Display(Name = "Cost Per Check")]
        public decimal CostPerCheck { get; set; }
        [Display(Name = "Billing Per Check")]
        public decimal BillingPerCheck { get; set; }
        [Display(Name = "Visiblity")]
        public bool Status { get; set; }
        [Required]
        [Display(Name = "External TAT: ")]
        public string ExternalTAT { get; set; }
        [Required]
        [Display(Name = "Price : ")]
        public string Price { get; set; }        
        public string Visiblity { get; set; }
    }


    public class AddEditClientTemplateViewModel
    {
        public int AutoRowId { get; set; }       
        public int ClientRowID { get; set; }
        public string ClientName { get; set; }
        public string PackageID { get; set; }
        public string PackageName { get; set; }
        public int AutoAlertId { get; set; }        
        public string AlertType { get; set; }
        public int AutoMailId { get; set; }
        public string MailTypeName { get; set; }
        public string MailSubject { get; set; }
        public string MailTemplate { get; set; }
        public string TemplateName { get; set; }        
        public bool Status { get; set; }
        public string TempStatus { get; set; }

        public HttpPostedFileBase UploadDoc { get; set; }
    }
    public class ClientEmailTemplateDetailsViewModel
    {
        public IEnumerable<AddEditClientTemplateViewModel> Packages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
    public class AlertTypeMaster
    {
        public int AutoAlertId { get; set; }
        public string AlertTypeName { get; set; }
      
    }
    public class MailTypeMaster
    {
        public int AutoMailId { get; set; }
        public string MailTypeName { get; set; }

    }
    
    public class PackageNameMater
    {
        public string PackageId { get; set; }
        public string PackageName { get; set; }

    }
    
}
