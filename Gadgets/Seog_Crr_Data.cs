using System;
using System.Web;
using System.Data;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_Data
    {
        Seog_Crr_Log objLog = new Seog_Crr_Log();
        Seog_Crr_Utility objUtility = new Seog_Crr_Utility();

        public void SalAddParemeter(ref Hashtable hshParameter, string strFieldname, object objValue, bool bolNotAllowNullOrEmptyParam = true)
        {
            if (!bolNotAllowNullOrEmptyParam || (objValue != null && String.Compare(objValue.ToString().Trim(), "", true) != 0)) //errors add the logic objValue != null
            {
                if (objValue == null)
                {
                    hshParameter[strFieldname] = "NULL";
                }
                else
                {
                    hshParameter[strFieldname] = $@"N'{SalEscapeQuotation(objValue.ToString().Trim())}'";
                }
            }
        }

        private string SalGetDBConnString(string strDBConnName)
        {
            if (!string.IsNullOrEmpty(strDBConnName))
                return ConfigurationManager.ConnectionStrings[strDBConnName]?.ToString();

            return ConfigurationManager.ConnectionStrings["CompensationConsumer"]?.ToString();
        }

        public DataSet SalGetDataSet(Hashtable hshParam, string strUSP, string strDbConnName)
        {
            if (string.IsNullOrEmpty(strDbConnName))
                throw new ArgumentException("DB connect  string is empty: " + strDbConnName);

            if (!string.IsNullOrEmpty(strUSP) && strUSP.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                strUSP = "('" + strUSP.Replace("'", "''") + "')";
            }

            DataSet dstData = new DataSet();
            try
            {
                if (hshParam != null)
                {
                    string strDbConnString = SalGetDBConnString(strDbConnName) + "salary;"; //Passwrod hardcode
                    using (SqlConnection objConn = new SqlConnection(strDbConnString))
                    {
                        using (SqlCommand objCmd = new SqlCommand(strUSP, objConn))
                        {
                            objConn.Open();
                            objCmd.CommandType = CommandType.StoredProcedure;
                            foreach (string strKey in hshParam.Keys)
                            {
                                objCmd.Parameters.AddWithValue(strKey, hshParam[strKey]);
                            }

                            SqlDataAdapter adpData = new SqlDataAdapter();
                            adpData.SelectCommand = objCmd;
                            adpData.Fill(dstData);
                        }

                        objConn.Close();
                    }
                }
                return dstData;
            }
            catch (Exception ex)
            {
                objLog.SalError("SalProcessDataSet", ex);
                throw ex;
            }
        } // method


        public string SalEscapeQuotation(string strInput)
        {
            return strInput.Replace("'", "''");
        }

        public bool SalIsEmptyDataSet(DataSet dstData)
        {
            if (dstData != null && dstData.Tables.Count > 0 && dstData.Tables[0].Rows.Count > 0 && !dstData.Tables[0].Columns.Contains("row"))
                return false;
            else
                return true;
        }

        public bool SalIsEmptyDataTable(DataTable dtbDataTable)
        {
            if (dtbDataTable == null || dtbDataTable.Rows.Count == 0)
            {
                return true;
            }
            return false;
        }

        public DataTable SalGetDataTableFormatDatasetByExpression(DataTable dtInput, string strExpression)
        {
            DataTable dtNew = dtInput.Clone();
            foreach (var dtRow in dtInput.Select(strExpression))
            {
                dtNew.Rows.Add(dtRow.ItemArray);
            }
            return dtNew;
        }

        public DataRow[] SalGetFormatDatasetByExpression(DataTable dtInput, string strExpression)
        {
            return dtInput.Select(strExpression);
        }

        public void SalCleanDataSet(ref DataSet dstData)
        {
            if (dstData != null)
            {
                dstData.Dispose();
                dstData = null;
            }
        }

        public void SalCleanDataTable(ref DataTable dtbData)
        {
            if (dtbData != null)
            {
                dtbData.Dispose();
                dtbData = null;
            }
        }

        public int SalGetDataSetRowsNum(DataSet dstData)
        {
            if (!SalIsEmptyDataSet(dstData))
            {
                return dstData.Tables[0].Rows.Count;
            }
            else
                return 0;
        }

        public DataRow SalGetFirstDataRow(DataSet dstData)
        {
            if (SalGetDataSetRowsNum(dstData) > 0)
            {
                return dstData.Tables[0].Rows[0];
            }
            return null;
        }

        public string SalGetStringFromQueryString(string strControlName, string strDefaultValue = "")
        {
            if (HttpContext.Current.Request.QueryString[strControlName] != null && HttpContext.Current.Request.QueryString[strControlName].ToString().Trim() != "")
                return HttpContext.Current.Request.QueryString[strControlName].ToString().Trim();
            else
                return strDefaultValue;
        }

        public string SalGetStringFromForm(string strControlName, string strDefaultValue = "")
        {
            if (HttpContext.Current.Request.Form[strControlName] != null && HttpContext.Current.Request.Form[strControlName].ToString().Trim() != "")
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Form[strControlName].ToString().Trim());
            else
                return strDefaultValue;
        }

        public double SalConvertDBFieldToDouble(object objParam)
        {
            if (objParam == null || objParam == DBNull.Value || objParam.ToString().Trim().Length <= 0)
                return 0;

            double dblOutput;
            if (double.TryParse(objParam.ToString(), out dblOutput))
            {
                return Convert.ToDouble(objParam);
            }
            return 0;
        }

    }
}