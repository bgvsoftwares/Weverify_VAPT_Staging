using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TeamMember
    {
        public TeamMember()
        {
            TeamMemberRowID = 0;
            TMTitle = string.Empty;
            TMFirstName = string.Empty;
            TMLastName = string.Empty;
            FTitle = string.Empty;
            FFirstName = string.Empty;
            FLastName = string.Empty;
            SpouseFirstName = string.Empty;
            SpouseLastName = string.Empty;
            CountryRowID = 0;
            StateRowID = 0;
            DistrictRowID = 0;
            LocationRowID = 0;
            PinCode = 0;
            EmployeeID = string.Empty;
            //DepartmentRowID = 0;
            //DesignationRowID = 0;
            CategoryRowID = 0;
            PresentAddress = string.Empty;
            PermanentAddress = string.Empty;
            STD = string.Empty;
            ResidancePhoneNo = string.Empty;
            MobileNo = string.Empty;
            EmailID = string.Empty;
            DirectTelNo = string.Empty;
            ProcessStatus = string.Empty;
            UserType = string.Empty;
            Status = 1;
            BORowID = 0;
        }

        public short TeamMemberRowID { get; set; }

        public string TMTitle { get; set; }
        public string TMFirstName { get; set; }
        public string TMLastName { get; set; }
        public string FTitle { get; set; }
        public string FFirstName { get; set; }
        public string FLastName { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseLastName { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DOJ { get; set; }

        public short CountryRowID { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }

        public short StateRowID { get; set; }
        public virtual MasterState MasterState { get; set; }

        public short DistrictRowID { get; set; }
        public virtual MasterDistrict MasterDistrict { get; set; }

        public int LocationRowID { get; set; }
        public virtual MasterLocation MasterLocation { get; set; }

        public int PinCode { get; set; }
        public string EmployeeID { get; set; }

        //public byte DepartmentRowID { get; set; }
        //public virtual MasterDepartment MasterDepartment { get; set; }

        //public byte DesignationRowID { get; set; }
        //public virtual MasterDesignation MasterDesignation { get; set; }

        public byte CategoryRowID { get; set; }
        public virtual MasterCategory MasterCategory { get; set; }

        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string STD { get; set; }
        public string ResidancePhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string DirectTelNo { get; set; }
        public string ProcessStatus { get; set; }
        public string UserType { get; set; }
        public byte Status { get; set; }

        public short BORowID { get; set; }
        public MasterCompanyBranch MasterCompanyBranch { get; set; }
    }
}
