using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQNationalIdentity
    {
        public PQNationalIdentity()
        {
            NationalIdentityRowID = 0;
            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            CheckFamilyRowID = 0;
            SubCheckRowID = 0;
            UniqueComponentID = string.Empty;

            NIC_Cand_Name = string.Empty;
            NIC_Sec_Ref_Id = string.Empty;
            NIC_ID_No = string.Empty;
            NIC_Passport_No = string.Empty;
            NIC_SSN_NO = string.Empty;
            NIC_Pan_No = string.Empty;
            NIC_Voter_Id_No = string.Empty;
            NIC_Dvng_Liscense_No = string.Empty;
            NIC_Aadhar_Card_No = string.Empty;
            NIC_Place_Issue = string.Empty;
            NIC_Place_Residnce = string.Empty;
            NIC_Type_Id_Card = string.Empty;
            NIC_Reg_Doc_Type = string.Empty;
            NIC_RegNo = string.Empty;
            NIC_PinCode = string.Empty;
            NIC_Name_Key_Personnel = string.Empty;
            NIC_AliasFname = string.Empty;
            NIC_Alias_Lname = string.Empty;
            NIC_Country_Iss_Passport = string.Empty;
            NIC_Gender = string.Empty;
            NIC_Id_CardNo = string.Empty;
            NIC_Identity_Prov = string.Empty;
            NIC_Nationality = string.Empty;
            NIC_AadhaarCard = 0;
            NIC_DrivingLicense = 0;
            NIC_PANCard = 0;
            NIC_Passport = 0;
            NIC_VoterID = 0;
            NIC_OtherProof = 0;
            NIC_OtherDetails = string.Empty;
            NIC_OtherDetails2 = string.Empty;
            NIC_OtherDetails3 = string.Empty;
            NIC_OtherDetails4 = string.Empty;
            NIC_OtherDetails5 = string.Empty;
            NIC_OtherDetails6 = string.Empty;
            NIC_OtherDetails7 = string.Empty;
            NIC_OtherDetails8 = string.Empty;
            NIC_OtherDetails9 = string.Empty;
            NIC_OtherDetails10 = string.Empty;
            ATA_CID_No = string.Empty;
            ATA_Cmpny_Addr = string.Empty;

            CreatedBy = 0;
            ModifiedBy = 0;
            Status = 1;

            CheckStatus = string.Empty;
            ReWorkCheckStatus = string.Empty;
            Remarks = string.Empty;
        }

        public int NationalIdentityRowID { get; set; }

        #region Common for all Checks 

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public int PersonalRowID { get; set; }
        public virtual PQPersonal PQPersonal { get; set; }

        public int ClientPackageRowID { get; set; }

        public short CheckFamilyRowID { get; set; }
        public virtual MasterCheckFamily MasterCheckFamily { get; set; }

        public short SubCheckRowID { get; set; }
        public virtual MasterSubCheckFamily MasterSubCheckFamily { get; set; }

        public string UniqueComponentID { get; set; }

        #endregion

        public string NIC_Cand_Name { get; set; }       //Text Field
        public string NIC_Sec_Ref_Id { get; set; }       //Text Field
        public DateTime? NIC_DOB { get; set; }       //Date
        public DateTime? NIC_DOB1 { get; set; }      //Date
        public string NIC_ID_No { get; set; }       //Text Field
        public DateTime? NIC_Date_Issue { get; set; }       //Text Field
        public DateTime? NIC_Expiry_Date { get; set; }       //Text Field
        public string NIC_Passport_No { get; set; }       //Text Field
        public string NIC_SSN_NO { get; set; }       //Text Field
        public string NIC_Pan_No { get; set; }       //Text Field
        public string NIC_Voter_Id_No { get; set; }       //Text Field
        public string NIC_Dvng_Liscense_No { get; set; }       //Text Field
        public string NIC_Aadhar_Card_No { get; set; }       //Text Field
        public string NIC_Place_Issue { get; set; }       //Text Field
        public string NIC_Place_Residnce { get; set; }       //Text Field
        public string NIC_Type_Id_Card { get; set; }       //Text Field
        public string NIC_Reg_Doc_Type { get; set; }       //Text Field
        public string NIC_RegNo { get; set; }       //Text Field
        public string NIC_PinCode { get; set; }       //Text Field
        public string NIC_Name_Key_Personnel { get; set; }       //Text Field
        public string NIC_AliasFname { get; set; }       //Text Field
        public string NIC_Alias_Lname { get; set; }       //Text Field
        public string NIC_Country_Iss_Passport { get; set; }       //Text Field
        public string NIC_Gender { get; set; }       //Drop Down
        public string NIC_Id_CardNo { get; set; }       //Text Field
        public string NIC_Identity_Prov { get; set; }       //Text Field
        public DateTime? NIC_Name_Changed_Effect_Frm { get; set; }       //Date
        public string NIC_Nationality { get; set; }      //Text Field
        public byte NIC_AadhaarCard { get; set; }                       // CheckBox     Default - 0 otherwise 1      
        public byte NIC_DrivingLicense { get; set; }     // CheckBox     Default - 0 otherwise 1       
        public byte NIC_PANCard { get; set; }     //      // CheckBox     Default - 0 otherwise 1       
        public byte NIC_Passport { get; set; }     //     // CheckBox     Default - 0 otherwise 1      
        public byte NIC_VoterID { get; set; }     //      // CheckBox     Default - 0 otherwise 1       
        public byte NIC_OtherProof { get; set; }                        // CheckBox     Default - 0 otherwise 1
        public string NIC_OtherDetails { get; set; }       //text Area
        public string NIC_OtherDetails2 { get; set; }       //text Area
        public string NIC_OtherDetails3 { get; set; }       //text Area
        public string NIC_OtherDetails4 { get; set; }       //text Area
        public string NIC_OtherDetails5 { get; set; }       //text Area
        public string NIC_OtherDetails6 { get; set; }       //text Area
        public string NIC_OtherDetails7 { get; set; }       //text Area
        public string NIC_OtherDetails8 { get; set; }       //text Area
        public string NIC_OtherDetails9 { get; set; }       //text Area
        public string NIC_OtherDetails10 { get; set; }       //text Area
        public string ATA_CID_No { get; set; }       //Text Field
        public string ATA_Cmpny_Addr { get; set; }       //text Area      

        //following properties not show on page
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? InternalOutDate { get; set; }
        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CheckStatus { get; set; }
        public string ReWorkCheckStatus { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }
        
    }
}
