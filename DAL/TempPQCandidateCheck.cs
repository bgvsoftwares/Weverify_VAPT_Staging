using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TempPQCandidateCheck
    {
        public TempPQCandidateCheck()
        {
            CandCheckRowID = 0;
            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            ClientCheckRowID = 0;
            Status = 1;
        }

        public Int64 CandCheckRowID { get; set; }

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public int PersonalRowID { get; set; }
        public virtual TempPQPersonal TempPQPersonal { get; set; }

        public int ClientPackageRowID { get; set; }
        public int ClientCheckRowID { get; set; }
        public int CheckFamilyRowID { get; set; }
        public int SubCheckRowID { get; set; }

        public byte Status { get; set; }

        public string CheckInfoStatus { get; set; }
        public string CheckDocStatus { get; set; }
        public string SubCheckName { get; set; }

        public IEnumerable<TempPQCandidateCheck> GetTempCandidateChecks(Int64 Personalrowid = 0)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "getTempPQCandidateChecks",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );           
            foreach (DataRow row in dt.Rows)
            {
                yield return new TempPQCandidateCheck
                {
                    ClientRowID = Convert.ToInt16(row["ClientRowID"].ToString()),
                    PersonalRowID = Convert.ToInt32(row["PersonalRowID"].ToString()),
                    SubCheckRowID = Convert.ToInt32(row["SubCheckFamilyID"].ToString()),                   
                    ClientPackageRowID = Convert.ToInt32(row["ClientPackageRowID"].ToString()),
                    ClientCheckRowID = Convert.ToInt32(row["ClientCheckRowID"].ToString()),
                    Status = Convert.ToByte(row["Status"]),
                    CheckInfoStatus = Convert.ToString(row["CheckInfoStatus"]),
                    CheckDocStatus = Convert.ToString(row["CheckDocStatus"]),
                    CheckFamilyRowID = Convert.ToInt16(row["CheckFamilyRowID"]),
                    SubCheckName = Convert.ToString(row["SubCheckName"]),
                };
            }
        }

        public DataSet GetTempCandidateChecksDT(Int64 Personalrowid = 0)
        {
            DataSet ds = new DataSet();
            ds = DAS.ExecuteDataset(DAS.ConnectionString, CommandType.StoredProcedure, "getTempPQCandidateChecksDT",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }

        public DataSet AutoCheckInitiation_proc(Int64 Personalrowid = 0)
        {
            DataSet ds = new DataSet();
            ds = DAS.ExecuteDataset(DAS.ConnectionString, CommandType.StoredProcedure, "AutoCheckInitiation_proc",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        public void SaveResearchData(int Personalrowid = 0, int subCheckRowid=0, int researchID=0)
        {            
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "saveTempResearchData",
            DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@subcheckRowid", SqlDbType.Int, subCheckRowid, ParameterDirection.Input, 10, false),
            DAS.CreateParameter("@ResearchID", SqlDbType.Int, researchID, ParameterDirection.Input, 10, false)
            );  
            
        }

        public DataTable GetCandidateFormCompletenessStatus(Int64 Personalrowid = 0)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCandidateChecksCompletenessStatus",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        public DataTable AutoCandidateChecksCompletenessStatus(Int64 Personalrowid = 0)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "AutoCandidateChecksCompletenessStatus",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }

        public string checkAllSubcheckNotApplicable(int Personalrowid = 0)
        {
            string Resultant = string.Empty;
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "checkAllSubcheckNotApplicable",
                DAS.CreateParameter("@personalRowid", SqlDbType.Int, Personalrowid, ParameterDirection.Input, 10, false)
            );
            if(ds.Rows.Count > 0)
            {
                Resultant = ds.Rows[0]["StatusResult"].ToString();
            }

            return Resultant;
        }
        public DataTable GetNewCandidateDetails(Int64 Personalrowid = 0)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetNewCandidateDetails",
                DAS.CreateParameter("@PersonalRowID", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        
        public string ApproveOrFinalSubmitByCandidate(Int64 Personalrowid = 0,int CreatedBy=0)
        {
            string retval = "";
            retval = (String)DAS.ExecuteScalar(DAS.ConnectionString, CommandType.StoredProcedure, "ApproveOrFinalSubmitByCandidate",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@createdBy", SqlDbType.Int, CreatedBy, ParameterDirection.Input, 10, false)
            );
            return retval;
        }

        public string ApproveOrFinalSubmitByCandidateUANAcuity(Int64 Personalrowid = 0, int CreatedBy = 0)
        {
            string retval = "";
            retval = (String)DAS.ExecuteScalar(DAS.ConnectionString, CommandType.StoredProcedure, "ApproveOrFinalSubmitByCandidateUANAcuity",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@createdBy", SqlDbType.Int, CreatedBy, ParameterDirection.Input, 10, false)
            );
            return retval;
        }

        public string ApproveOrFinalSubmitByCandidateNewN(Int64 Personalrowid = 0, int CreatedBy = 0)
        {
            string retval = "";
            retval = (String)DAS.ExecuteScalar(DAS.ConnectionString, CommandType.StoredProcedure, "ApproveOrFinalSubmitByCandidateNewN",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@createdBy", SqlDbType.Int, CreatedBy, ParameterDirection.Input, 10, false)
            );
            return retval;
        }
        public string ApproveOrFinalSubmitByCandidateToHR(Int64 Personalrowid = 0)
        {
            string retval = "";
            retval = (String)DAS.ExecuteScalar(DAS.ConnectionString, CommandType.StoredProcedure, "ApproveOrFinalSubmitByCandidateToHR",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return retval;
        }
        
        public DataTable GetCandidateFormDigitalStatus(Int64 Personalrowid = 0)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCandidateChecksDigitalStatus",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }
        public DataSet getTempPQCandidateChecksDT_Fractal(Int64 Personalrowid = 0)
        {
            DataSet ds = new DataSet();
            ds = DAS.ExecuteDataset(DAS.ConnectionString, CommandType.StoredProcedure, "getTempPQCandidateChecksDT_Fractal",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }

        public DataTable GetCandidateChecksCompletenessStatus_Fractal(Int64 Personalrowid = 0)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetCandidateChecksCompletenessStatus_Fractal",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false)
            );
            return ds;
        }

        public string ApproveOrFinalSubmitByCandidate_Fractal(Int64 Personalrowid = 0, int CreatedBy = 0)
        {
            string retval = "";
            retval = (String)DAS.ExecuteScalar(DAS.ConnectionString, CommandType.StoredProcedure, "ApproveOrFinalSubmitByCandidate_Fractal",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@createdBy", SqlDbType.Int, CreatedBy, ParameterDirection.Input, 10, false)
            );
            return retval;
        }
        public string ApproveOrFinalSubmitByCandidate_Addcheck(Int64 Personalrowid = 0, int CreatedBy = 0)
        {
            string retval = "";
            retval = (String)DAS.ExecuteScalar(DAS.ConnectionString, CommandType.StoredProcedure, "ApproveOrFinalSubmitByCandidate_Addcheck",
                DAS.CreateParameter("@personalRowid", SqlDbType.BigInt, Personalrowid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@createdBy", SqlDbType.Int, CreatedBy, ParameterDirection.Input, 10, false)
            );
            return retval;
        }

       


    }
}

