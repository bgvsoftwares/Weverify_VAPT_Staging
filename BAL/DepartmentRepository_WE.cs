using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    class DepartmentRepository_WE
    {
        private const string SP_PARAM_RETURN_VALUE = "@ReturnValue";

        public int testTransaction(int testid)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            SqlConnection con = new SqlConnection(DAS.ConnectionString);
            con.Open();
            SqlTransaction t2;
            t2 = con.BeginTransaction();
            DAS.ExecuteNonQuery(t2, CommandType.StoredProcedure, "testProcedure",
            DAS.CreateParameter("@Id", SqlDbType.Int, testid, ParameterDirection.Input, 10, false),
            objReturnValueParam
            );
            //assign the return value
            intReturnValue = (int)objReturnValueParam.Value;

            //return the value returned from the stored procedure

            if (intReturnValue == 0)
            {
                t2.Commit();
            }
            else
            {
                t2.Rollback();
            }
            return intReturnValue;
        }

        public Int64 getTestId(long code)
        {
            Int64 aa = 0;
            aa = (long)DAS.ExecuteScalar(DAS.ConnectionString, CommandType.StoredProcedure, "testprocedure1",
                DAS.CreateParameter("@code", SqlDbType.BigInt, code, ParameterDirection.Input, 20, false)
                );
            return aa;
        }        

        public DataSet getTestDataset(string test)
        {
            DataSet ds = new DataSet();
            ds = DAS.ExecuteDataset(DAS.ConnectionString, CommandType.StoredProcedure, "testProc",
            DAS.CreateParameter("@code", SqlDbType.VarChar, test, ParameterDirection.Input, 25, false));
            return ds;
        }       

        public SqlDataReader gettesid()
        {
            SqlDataReader dr;
            dr = DAS.ExecuteReader(DAS.ConnectionString, CommandType.StoredProcedure, "test");
            return dr;
        }

        public int Department_delete(int departmentid)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "Department_delete",
            DAS.CreateParameter("@departmentID", SqlDbType.Int, departmentid, ParameterDirection.Input, 10, false),           
            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }

        public int BillingCycle_delete(int BillingRowID)
        {
            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "BillingCycle_delete",
            DAS.CreateParameter("@BillingRowID", SqlDbType.Int, BillingRowID, ParameterDirection.Input, 10, false),
            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;
        }


    }
}
