using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using static LogIn.Repository;
using System.Drawing;
using System.Data.Odbc;
using LogIn.Pages;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System.Web.Services.Description;
using Sap.Data.Hana;
using static LogIn.Pages.VendorForm;
using System.IO;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Text;

namespace LogIn
{
    public class DbConnection
    {
        static string sServer = ConfigurationManager.AppSettings["Server"];
        static string sDBUser = DecryptFun(ConfigurationManager.AppSettings["DBUser"]);
        static string sDBPwd = DecryptFun(ConfigurationManager.AppSettings["DBPwd"]);
        public static string sDBName = ConfigurationManager.AppSettings["DBName"];
        public int fSize = 5;
        public string HanaConstr = "DRIVER={HDBODBC};UID=" + sDBUser + "PWD=" + sDBPwd + "DATABASENAME=NDB;SERVERNODE=" + sServer + "CS=" + sDBName + ";";
        public string sConstr = "Data Source=" + sServer + ";Initial Catalog=" + sDBName + ";User ID=" + sDBUser + ";Password=" + sDBPwd;
        public static long lretcode;
        HanaConnection HanaCon;
        HanaConnection HanaSAPCon;
        HanaCommand cmd;
        HanaDataAdapter sa;
        SqlConnection sqlCon;
        SqlConnection sqlSAPCon;
        SqlCommand cmd1;
        SqlDataAdapter sa1;
        DataTable dtlog;
        DataSet dslog;
        OdbcConnection oCon;

        #region Hana
        public DataTable GetDataTable(string sQuery)
        {
            String sFuncName = "HanaExecuteQueryReturnDataTable";
            HanaConnection SAP_Con = null;
            DataTable dt = new DataTable();
            try
            {
                writeLog(sFuncName, "Select qry:" + sQuery, "Debug");
                string SAP_Constr = sConstr;
                SAP_Con = new HanaConnection(SAP_Constr);
                SAP_Con.Open();
                HanaCommand SAP_Cmd = new HanaCommand();
                SAP_Cmd.CommandType = CommandType.Text;
                SAP_Cmd.CommandText = sQuery;
                SAP_Cmd.Connection = SAP_Con;
                SAP_Cmd.CommandTimeout = 0;
                if (SAP_Con.State == ConnectionState.Closed)
                    SAP_Con.Open();
                HanaDataAdapter SAP_da = new HanaDataAdapter();
                SAP_da.SelectCommand = SAP_Cmd;
                SAP_da.Fill(dt);
                writeLog(sFuncName, "Query executed successfully. Rows returned: " + dt.Rows.Count, "Debug");
                return dt;
            }
            catch (Exception ex)
            {
                LogError(ex, sFuncName, sQuery);
                throw new Exception(ex.Message);
            }
            finally
            {
                if ((SAP_Con != null))
                {
                    SAP_Con.Close();
                    SAP_Con.Dispose();
                }
            }
        }
        public DataTable ExecuteQueryForDataTable(string sQuery)
        {
            String sFuncName = "HanaExecuteQueryReturnDataTable";
            HanaConnection SAP_Con = null;
            DataTable dt = new DataTable();
            try
            {
                writeLog(sFuncName, "Select qry:" + sQuery, "Debug");
                string SAP_Constr = HanaConstr;
                SAP_Con = new HanaConnection(SAP_Constr);
                SAP_Con.Open();
                HanaCommand SAP_Cmd = new HanaCommand();
                SAP_Cmd.CommandType = CommandType.Text;
                SAP_Cmd.CommandText = sQuery;
                SAP_Cmd.Connection = SAP_Con;
                SAP_Cmd.CommandTimeout = 0;
                if (SAP_Con.State == ConnectionState.Closed)
                    SAP_Con.Open();
                HanaDataAdapter SAP_da = new HanaDataAdapter();
                SAP_da.SelectCommand = SAP_Cmd;
                SAP_da.Fill(dt);
                writeLog(sFuncName, "Query executed successfully. Rows returned: " + dt.Rows.Count, "Debug");
                return dt;
            }
            catch (Exception ex)
            {
                LogError(ex, sFuncName, sQuery);
                throw new Exception(ex.Message);
            }
            finally
            {
                if ((SAP_Con != null))
                {
                    SAP_Con.Close();
                    SAP_Con.Dispose();
                }
            }
        }
        public void ExecuteNonQuery(string sQuery)
        {
            String sFuncName = "ExecuteNonQuery";
            string SAP_Constr = HanaConstr;
            HanaConnection oCon = new HanaConnection(SAP_Constr);
            HanaCommand oCmd = new HanaCommand();
            HanaDataAdapter oSQLAdapter = new HanaDataAdapter();

            try
            {
                writeLog(sFuncName, "ExecuteNonQuery qry:" + sQuery, "Debug");
                oCmd.CommandType = CommandType.Text;
                oCmd.CommandText = sQuery;
                oCmd.Connection = oCon;
                oCmd.CommandTimeout = 0;
                if (oCon.State == ConnectionState.Closed)
                    oCon.Open();
                oCmd.ExecuteNonQuery();
                writeLog(sFuncName, "ExecuteNonQuery completed successfully.", "Debug");
            }
            catch (Exception ex)
            {
                LogError(ex, sFuncName, sQuery);
                throw ex;
            }
            finally
            {
                if (oCon != null)
                {
                    oCon.Close();
                    oCon.Dispose();
                }
            }
        }
        public string DeleteByCode(string procedureName, string code)
        {
            String sFuncName = "DeleteByCode";
            string result = "Success";
            try
            {
                writeLog(sFuncName, "Stored Procedure:" + procedureName + ", Parameters: Code=" + code, "Debug");
                using (HanaCon = new HanaConnection(HanaConstr))
                {
                    cmd = new HanaCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = HanaCon;
                    cmd.CommandText = procedureName;
                    cmd.Parameters.AddWithValue("@Code", code);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
                writeLog(sFuncName, "Procedure completed successfully.", "Debug");
            }
            catch (Exception ex)
            {
                LogError(ex, sFuncName, "Procedure: " + procedureName + ", Code: " + code);
                throw;
            }
            return result;
        }
        public string GetSingleValue(string sQuery)
        {
            String sFuncName = "GetSingleValue";
            HanaConnection SAP_Con = null;
            DataTable dt = new DataTable();
            string sSingleValue = string.Empty;

            try
            {
                writeLog(sFuncName, "Select qry:" + sQuery, "Debug");
                string SAP_Constr = HanaConstr;
                SAP_Con = new HanaConnection(SAP_Constr);
                SAP_Con.Open();
                HanaCommand SAP_Cmd = new HanaCommand();
                SAP_Cmd.CommandType = CommandType.Text;
                SAP_Cmd.CommandText = sQuery;
                SAP_Cmd.Connection = SAP_Con;
                SAP_Cmd.CommandTimeout = 0;
                if (SAP_Con.State == ConnectionState.Closed)
                    SAP_Con.Open();
                HanaDataAdapter SAP_da = new HanaDataAdapter();
                SAP_da.SelectCommand = SAP_Cmd;
                SAP_da.Fill(dt);

                if (dt.Rows.Count > 0)
                    sSingleValue = dt.Rows[0][0].ToString().Trim();

                writeLog(sFuncName, "Query completed successfully. Result: " + sSingleValue, "Debug");
                return sSingleValue;
            }
            catch (Exception ex)
            {
                LogError(ex, sFuncName, sQuery);
                throw ex;
            }
            finally
            {
                if ((SAP_Con != null))
                {
                    SAP_Con.Close();
                    SAP_Con.Dispose();
                }
            }
        }
        #endregion

        #region DecryptFun
        public static string DecryptFun(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;
            try
            {
                string key = "TechativeSolutions04December2023";
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(password);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StaticLogError(ex, "DecryptFun");
                return string.Empty;
            }
        }
        #endregion
        #region SQL
        public DataTable SQL_ExecuteQueryForDataTable(string sQuery)
        {
            string sFunct = "ExecuteQueryForDataTable";
            string sValue = string.Empty;
            dtlog = new DataTable();
            try
            {
                writeLog(sFunct, "Select qry:" + sQuery, "Debug");
                using (sqlCon = new SqlConnection(sConstr))
                {
                    sqlCon.Open();
                    cmd1 = new SqlCommand(sQuery, sqlCon);
                    sa1 = new SqlDataAdapter(cmd1);
                    sa1.Fill(dtlog);
                }
                writeLog(sFunct, "SQL Query completed successfully. Rows returned: " + dtlog.Rows.Count, "Debug");
            }
            catch (Exception ex)
            {
                LogError(ex, sFunct, sQuery);
            }
            return dtlog;
        }
        public void SQL_ExecuteNonQuery(string sQuery)
        {
            string sFunct = "ExecuteNonQuery";

            try
            {
                writeLog(sFunct, "ExecuteNonQuery qry:" + sQuery, "Debug");
                using (sqlCon = new SqlConnection(sConstr))
                {
                    sqlCon.Open();
                    cmd1 = new SqlCommand();
                    cmd1.CommandText = sQuery;
                    cmd1.Connection = sqlCon;
                    cmd1.CommandTimeout = 0;
                    cmd1.ExecuteNonQuery();
                }
                writeLog(sFunct, "SQL ExecuteNonQuery completed successfully.", "Debug");
            }
            catch (Exception ex)
            {
                LogError(ex, sFunct, sQuery);
            }

        }
        public string SQL_DeleteByCode(string procedureName, string code)
        {
            string sFunct = "SQL_DeleteByCode";
            string result = "Success";
            try
            {
                writeLog(sFunct, "SQL Stored Procedure:" + procedureName + ", Parameters: Code=" + code, "Debug");
                using (sqlCon = new SqlConnection(sConstr))
                {
                    cmd1 = new SqlCommand();
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Connection = sqlCon;
                    cmd1.CommandText = procedureName;
                    cmd1.Parameters.AddWithValue("@Code", code);
                    cmd1.Connection.Open();
                    cmd1.ExecuteNonQuery();
                }
                writeLog(sFunct, "SQL Procedure completed successfully.", "Debug");
            }
            catch (Exception ex)
            {
                LogError(ex, sFunct, "Procedure: " + procedureName + ", Code: " + code);
            }
            return result;
        }
        public string SQL_GetSingleValue(string sQuery)
        {
            string sFunct = "GetSingleValue";
            string sValue = string.Empty;
            dtlog = new DataTable();
            try
            {
                writeLog(sFunct, "Select qry:" + sQuery, "Debug");
                using (sqlCon = new SqlConnection(sConstr))
                {
                    sqlCon.Open();
                    cmd1 = new SqlCommand(sQuery, sqlCon);
                    sa1 = new SqlDataAdapter(cmd1);
                    sa1.Fill(dtlog);
                    if (dtlog.Rows.Count > 0)
                    {
                        sValue = dtlog.Rows[0][0].ToString();
                    }
                }
                writeLog(sFunct, "SQL Query completed successfully. Result: " + sValue, "Debug");
            }
            catch (Exception ex)
            {
                LogError(ex, sFunct, sQuery);
            }
            return sValue;
        }
        #endregion
        public DataTable SQL_UserDetailsForDataTable()
        {
            DataTable dt = new DataTable();
            string query = @"Exec [TEC_UserDetails] ";
            dt = SQL_ExecuteQueryForDataTable(query);
            return dt;
        }
        public DataTable SQL_UserEditDetails(String code)
        {
            DataTable dt = new DataTable();
            string query = @"Exec [TEC_Editing]'EditUser','" + code + "'";
            dt = SQL_ExecuteQueryForDataTable(query);
            return dt;
        }
        public DataTable UserDetailsForDataTable()
        {
            DataTable dt = new DataTable();
            string query = "call \"TEC_UserDetails\" ";
            dt = ExecuteQueryForDataTable(query);
            return dt;
        }
        public DataTable UserEditDetails(String code)
        {
            DataTable dt = new DataTable();
            string query = "call \"TEC_Editing\" ('EditUser','" + code + "')";
            dt = ExecuteQueryForDataTable(query);
            return dt;
        }

        #region COMMAN CLASS
        private string GetLogFolder()
        {
            try
            {
                string configPath = ConfigurationManager.AppSettings["Log"];
                if (!string.IsNullOrWhiteSpace(configPath))
                {
                    if (!Directory.Exists(configPath))
                    {
                        Directory.CreateDirectory(configPath);
                    }
                    return configPath;
                }
                string folder = HttpContext.Current != null && HttpContext.Current.Server != null ? HttpContext.Current.Server.MapPath("~/ErrorLog") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
            catch
            {
                return HttpContext.Current != null && HttpContext.Current.Server != null ? HttpContext.Current.Server.MapPath("~/ErrorLog") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog");
            }
        }

        private string GetActiveLogFilePath(string folder, string prefix, string dateStr, double maxSizeMB)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string baseFileName = prefix + "_" + dateStr;
                string searchPattern = baseFileName + "*.txt";
                string[] existingFiles = Directory.GetFiles(folder, searchPattern);

                if (existingFiles == null || existingFiles.Length == 0)
                {
                    return Path.Combine(folder, baseFileName + ".txt");
                }

                int maxIndex = -1;
                string latestFile = null;

                foreach (string file in existingFiles)
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                    if (fileNameWithoutExt.Equals(baseFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (maxIndex < 0)
                        {
                            maxIndex = 0;
                            latestFile = file;
                        }
                    }
                    else if (fileNameWithoutExt.StartsWith(baseFileName + "(", StringComparison.OrdinalIgnoreCase) && fileNameWithoutExt.EndsWith(")"))
                    {
                        string indexPart = fileNameWithoutExt.Substring(baseFileName.Length + 1, fileNameWithoutExt.Length - baseFileName.Length - 2);
                        int idx;
                        if (int.TryParse(indexPart, out idx))
                        {
                            if (idx > maxIndex)
                            {
                                maxIndex = idx;
                                latestFile = file;
                            }
                        }
                    }
                }

                if (latestFile != null && File.Exists(latestFile))
                {
                    FileInfo fi = new FileInfo(latestFile);
                    double sizeInMB = ((double)fi.Length / 1024.0) / 1024.0;
                    if (sizeInMB < maxSizeMB)
                    {
                        return latestFile;
                    }
                    else
                    {
                        int nextIndex = (maxIndex <= 0) ? 1 : (maxIndex + 1);
                        return Path.Combine(folder, baseFileName + "(" + nextIndex + ").txt");
                    }
                }

                return Path.Combine(folder, baseFileName + ".txt");
            }
            catch
            {
                return Path.Combine(folder, prefix + "_" + dateStr + ".txt");
            }
        }

        public void LogError(Exception ex)
        {
            LogError(ex, string.Empty, string.Empty);
        }

        public void LogError(Exception ex, string methodName, string query = "")
        {
            if (ex == null)
                return;
            try
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                if (!string.IsNullOrEmpty(methodName))
                {
                    message += string.Format("Method Name: {0}", methodName);
                    message += Environment.NewLine;
                }
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("Exception Type: {0}", ex.GetType().FullName);
                message += Environment.NewLine;
                if (!string.IsNullOrEmpty(query))
                {
                    message += string.Format("Query: {0}", query);
                    message += Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    message += string.Format("StackTrace: {0}", ex.StackTrace);
                    message += Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(ex.Source))
                {
                    message += string.Format("Source: {0}", ex.Source);
                    message += Environment.NewLine;
                }
                if (ex.TargetSite != null)
                {
                    message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                    message += Environment.NewLine;
                }
                if (ex.InnerException != null)
                {
                    message += string.Format("InnerException: {0}", ex.InnerException.Message);
                    message += Environment.NewLine;
                }
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                string folder = GetLogFolder();
                string dateStr = DateTime.Now.ToString("dd-MM-yyyy");
                string path = GetActiveLogFilePath(folder, "Error", dateStr, fSize);

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch
            {
            }
        }

        public void writeLog(string ex, string filename = "Debug")
        {
            try
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex);
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                string folder = GetLogFolder();
                string dateStr = DateTime.Now.ToString("dd-MM-yyyy");
                string prefix = string.IsNullOrEmpty(filename) ? "Debug" : filename;
                string path = GetActiveLogFilePath(folder, prefix, dateStr, fSize);

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch
            {
            }
        }

        public void writeLog(string methodName, string message, string filename = "Debug")
        {
            try
            {
                string logMsg = string.Format("Time: {0}", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
                logMsg += Environment.NewLine;
                logMsg += "-----------------------------------------------------------";
                logMsg += Environment.NewLine;
                if (!string.IsNullOrEmpty(methodName))
                {
                    logMsg += string.Format("Method Name: {0}", methodName);
                    logMsg += Environment.NewLine;
                }
                logMsg += string.Format("Message: {0}", message);
                logMsg += Environment.NewLine;
                logMsg += "-----------------------------------------------------------";
                logMsg += Environment.NewLine;

                string folder = GetLogFolder();
                string dateStr = DateTime.Now.ToString("dd-MM-yyyy");
                string prefix = string.IsNullOrEmpty(filename) ? "Debug" : filename;
                string path = GetActiveLogFilePath(folder, prefix, dateStr, fSize);

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(logMsg);
                }
            }
            catch
            {
            }
        }

        public static void StaticLogError(Exception ex, string methodName = "", string query = "")
        {
            DbConnection conn = new DbConnection();
            conn.LogError(ex, methodName, query);
        }

        public static void StaticWriteLog(string ex, string filename = "Debug")
        {
            DbConnection conn = new DbConnection();
            conn.writeLog(ex, filename);
        }

        public static void StaticWriteLog(string methodName, string message, string filename = "Debug")
        {
            DbConnection conn = new DbConnection();
            conn.writeLog(methodName, message, filename);
        }
        #endregion
    }
}
