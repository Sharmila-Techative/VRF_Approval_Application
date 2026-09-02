using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Optimization;
using System.Data.Common;
using SAPbobsCOM;
using System.Web.Configuration;
using Sap.Data.Hana;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.SqlServer.Server;
using System.Web.Services.Description;
using LogIn.Model;
using System.Collections;
using System.Net.Mail;
using System.Text;
using DocumentFormat.OpenXml.ExtendedProperties;
using Company = SAPbobsCOM.Company;
using SelectPdf;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using DocumentFormat.OpenXml.VariantTypes;

namespace LogIn.Pages
{
    public partial class VendorForm : System.Web.UI.Page
    {
        public string Type = WebConfigurationManager.AppSettings["ServerType"];
        DbConnection conn = new DbConnection();
        DbConnection dBConnection = new DbConnection();
        public DataTable dataTable3 = null;
        SAPbobsCOM.Documents oBP = null;
        SAPbobsCOM.Items oItem = null;
        public SAPbobsCOM.Company p_oCompany;
        Log Log = new Log();
        public string sErrDesc;
        string CN1, CN2, CV1, CV2;
        public static string user;
        public double fSize = Convert.ToDouble(ConfigurationManager.AppSettings["fileSzie"]);
        public string sRejectType = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] == null)
                {
                    conn.writeLog("VendorForm_Page_Load", "Session username is null. Redirecting to LoginPage.", "Debug");
                    Session.Clear();
                    Session.Abandon();

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                    Response.Cache.SetNoStore();

                    Response.Redirect("~/Pages/LoginPage.aspx", false);
                    return;
                }
                DataTable dt1 = null;
                string query1 = "select \"TName\",\"GstNo\" from tec_oled where \"Approval\" = 'Y'";
                if (Type == "HANA")
                    dt1 = conn.ExecuteQueryForDataTable(query1);
                else
                    dt1 = conn.SQL_ExecuteQueryForDataTable(query1);
                if (dt1.Rows.Count == 0) btnPerformOperation.Visible = false;
                if (!IsPostBack)
                {
                    if (Session["username"] != null)
                    {
                        lblUserName.Text = Session["username"].ToString();
                    }
                    BindGroup();
                    Session["LastPushToSAP"] = null;
                    LoadVendorData();
                    LoadCompletedData();
                    LoadRejectedData();
                    LoadSAPPosted();
                }
                else
                {
                    string searchValue = hdnSearch.Value;
                    ClientScript.RegisterStartupScript(this.GetType(), "setSearchValue",
                        $"document.getElementById('txtSearch').value = '{searchValue.Replace("'", "\\'")}';", true);
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "VendorForm_Page_Load");
            }
        }

        public void writeLog(string ex, string filename)
        {
            try
            {
                conn.writeLog(ex, filename);
            }
            catch
            {
            }
        }

        private void LoadCompletedData()
        {
            try
            {
                string username = Session["username"].ToString();
                user = Session["username"].ToString();
                conn.writeLog("LoadCompletedData", "Loading completed data for user: " + username, "Debug");
                DataTable completedData = conn.ExecuteQueryForDataTable("call \"TEC_GetAprrovedDetails\" ('" + username + "')");
                if (completedData.Rows.Count > 0)
                {
                    gvCompleted.DataSource = completedData;
                    gvCompleted.DataBind();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "LoadCompletedData");
            }
        }

        private void LoadSAPPosted()
        {
            try
            {
                string username = Session["username"].ToString();
                conn.writeLog("LoadSAPPosted", "Loading SAP posted vendors for user: " + username, "Debug");
                DataTable completedData = conn.ExecuteQueryForDataTable("call \"TEC_GetPostedVendors\" ('" + username + "')");
                if (completedData.Rows.Count > 0)
                {
                    GridView2.DataSource = completedData;
                    GridView2.DataBind();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "LoadSAPPosted");
            }
        }

        private void LoadRejectedData()
        {
            try
            {
                string username = Session["username"].ToString();
                conn.writeLog("LoadRejectedData", "Loading rejected vendors for user: " + username, "Debug");
                DataTable RejectedData = conn.ExecuteQueryForDataTable("call \"TEC_GetRejectedDetails\" ('" + username + "')");
                if (RejectedData.Rows.Count > 0)
                {
                    gvRejected.DataSource = RejectedData;
                    gvRejected.DataBind();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "LoadRejectedData");
            }
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            try
            {
                string activeTab = TabContainer1.ActiveTab.HeaderText;
                conn.writeLog("TabContainer1_ActiveTabChanged", "Active tab changed to: " + activeTab, "Debug");

                switch (activeTab)
                {
                    case "Approval Waiting Status":
                        string username = Session["username"].ToString();
                        string query = "Call \"TEC_GetApprovalWaitingDetails\" ('" + username + "')";
                        DataTable dt = conn.ExecuteQueryForDataTable(query);

                        if (dt.Rows.Count > 0)
                        {
                            GridView4.DataSource = dt;
                            GridView4.DataBind();
                        }
                        break;

                    case "Pending Status":
                        LoadVendorData();
                        break;

                    case "Draft Status":
                        username = Session["username"].ToString();
                        query = "Call \"TEC_GetDraftDetails\" ('" + username + "')";
                        dt = conn.ExecuteQueryForDataTable(query);

                        if (dt.Rows.Count > 0)
                        {
                            GridView3.DataSource = dt;
                            GridView3.DataBind();
                        }
                        break;

                    case "Completed Status":
                        LoadCompletedData();
                        break;

                    case "Rejected Status":
                        LoadRejectedData();
                        break;

                    case "Push To Sap":
                        username = Session["username"].ToString();
                        query = "Call \"TEC_GetSapPostDetails\" ('" + username + "')";
                        dt = conn.ExecuteQueryForDataTable(query);

                        if (dt.Rows.Count > 0)
                        {
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                        break;

                    case "Created Vendor in SAP":
                        LoadSAPPosted();
                        break;
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "TabContainer1_ActiveTabChanged");
            }
        }

        protected void gvCompleted_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = gvCompleted.Rows[index];
                    string gstNo = row.Cells[3].Text;
                    conn.writeLog("gvCompleted_RowCommand", "Navigating to CompletedViewDoc for GST: " + gstNo, "Debug");
                    //Commented By Sharmila Ravi(02-09-2026)
                    //Response.Redirect($"/Pages/CompletedViewDoc.aspx?gstNumber={gstNo}");
                    Response.Redirect($"/Pages/CompletedViewDoc.aspx?gstNumber={gstNo}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                else if (e.CommandName == "Preview")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = gvCompleted.Rows[index];
                    string gstNo = row.Cells[3].Text;
                    conn.writeLog("gvCompleted_RowCommand", "Opening Preview for GST: " + gstNo, "Debug");
                    btnPreview_Click(gstNo);
                    string script = @"
            <script>
                window.open('VendorPreview.aspx', '_blank');
            </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "OpenPreview", script, false);
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "gvCompleted_RowCommand");
            }
        }

        protected void gvRejected_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = gvRejected.Rows[index];
                    string gstNo = row.Cells[4].Text;
                    conn.writeLog("gvRejected_RowCommand", "Viewing rejected GST: " + gstNo, "Debug");
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "gvRejected_RowCommand");
            }
        }

        protected void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox clicked = (CheckBox)sender;

                if (!clicked.Checked)
                {
                    return;
                }

                if (clicked == chkVI)
                {
                    chkVT.Checked = false;
                    chkVC.Checked = false;
                    chkIHB.Checked = false;
                }
                else if (clicked == chkVT)
                {
                    chkVI.Checked = false;
                    chkVC.Checked = false;
                    chkIHB.Checked = false;
                }
                else if (clicked == chkVC)
                {
                    chkVI.Checked = false;
                    chkVT.Checked = false;
                    chkIHB.Checked = false;
                }
                else if (clicked == chkIHB)
                {
                    chkVI.Checked = false;
                    chkVT.Checked = false;
                    chkVC.Checked = false;
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "CheckBox_CheckedChanged");
            }
        }

        public class Vendor
        {
            public string TName { get; set; }
            public string Bstate { get; set; }
            public string NatureOfBusinessActivity { get; set; }
            public string GstNo { get; set; }
        }

        private void LoadVendorData()
        {
            try
            {
                DataTable dt = null;
                DataTable dt1 = null;
                string query;
                string username = Session["username"].ToString();
                conn.writeLog("LoadVendorData", "Loading vendor data for: " + username, "Debug");

                if (Type == "HANA")
                    query = "Call \"TEC_GetDraftDetails\" ('" + username + "')";
                else
                    query = "Exec \"TEC_GetDraftDetails\" '" + username + "'";

                if (Type == "HANA")
                    dt = conn.ExecuteQueryForDataTable(query);
                else
                    dt = conn.SQL_ExecuteQueryForDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    GridView3.DataSource = dt;
                    GridView3.DataBind();
                }

                if (Type == "HANA")
                    query = "Call \"TEC_GetApprovalWaitingDetails\" ('" + username + "')";
                else
                    query = "Exec \"TEC_GetApprovalWaitingDetails\" '" + username + "'";

                if (Type == "HANA")
                    dt = conn.ExecuteQueryForDataTable(query);
                else
                    dt = conn.SQL_ExecuteQueryForDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    GridView4.DataSource = dt;
                    GridView4.DataBind();
                }

                if (Type == "HANA")
                    query = "Call \"TEC_GetApprovalDetails\" ('" + username + "')";
                else
                    query = "Exec \"TEC_GetApprovalDetails\" '" + username + "'";

                if (Type == "HANA")
                    dt = conn.ExecuteQueryForDataTable(query);
                else
                    dt = conn.SQL_ExecuteQueryForDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    gvUserDetails.DataSource = dt;
                    gvUserDetails.DataBind();
                }

                pnlActions.Visible = dt.Rows.Count > 0;

                string query1 = "call \"TEC_GetSapPostDetails\"('" + username + "')";

                if (Type == "HANA")
                    dt1 = conn.ExecuteQueryForDataTable(query1);
                else
                    dt1 = conn.SQL_ExecuteQueryForDataTable(query1);
                if (dt1.Rows.Count > 0)
                {
                    ViewState["GridData"] = dt1;
                    btnPerformOperation.Visible = true;
                    GridView1.DataSource = dt1;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "LoadVendorData");
            }
        }

        protected void SelectddlType(object sender, EventArgs e)
        {
            try
            {
                string query1 = "";
                if (Type == "HANA")
                    query1 = "call \"TEC_GetSapPostDetails\"";
                else
                    query1 = "Exec TEC_GetSapPostDetails";

                DataTable dt1;
                if (Type == "HANA")
                    dt1 = conn.ExecuteQueryForDataTable(query1);
                else
                    dt1 = conn.SQL_ExecuteQueryForDataTable(query1);
                GridView1.DataSource = dt1;
                dataTable3 = GridView1.DataSource as DataTable;
                ViewState["SAPTable"] = dataTable3;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "SelectddlType");
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox clickedCheckBox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)clickedCheckBox.NamingContainer;

                foreach (GridViewRow r in GridView1.Rows)
                {
                    CheckBox cb = (CheckBox)r.FindControl("chkSelect");
                    TextBox txtVendorName = (TextBox)r.FindControl("txtVendorName");
                    if (txtVendorName != null)
                        txtVendorName.Text = string.Empty;

                    if (cb != null && cb != clickedCheckBox)
                    {
                        cb.Checked = false;
                    }
                    else
                    {
                        string GstNo = r.Cells[3].Text;
                        string Query = "Call \"TEC_GetGSTCardCode\"('" + GstNo + "')";
                        DataTable dt4 = dBConnection.ExecuteQueryForDataTable(Query);
                        if (dt4.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt4.Rows)
                            {
                                string value = dr[0].ToString();
                                string[] parts = value.Split('-');
                                string lhs = parts.Length > 0 ? parts[0].Trim() : "";
                                DisableMatchingCheckbox(lhs);
                            }
                        }
                    }
                }
                btnPerformOperation.Visible = true;
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "chkSelect_CheckedChanged");
            }
        }

        private void DisableMatchingCheckbox(string lhs)
        {
            if (chkVI.Text == lhs)
                chkVI.Enabled = false;

            if (chkVT.Text == lhs)
                chkVT.Enabled = false;

            if (chkVC.Text == lhs)
                chkVC.Enabled = false;

            if (chkIHB.Text == lhs)
                chkIHB.Enabled = false;
        }

        private void SelectedRowChanged(GridViewRow row)
        {
            string gstNo = row.Cells[2].Text;
            string level = row.Cells[3].Text;
            string dept = row.Cells[4].Text;
        }

        protected void FillSeries(object sender, EventArgs e)
        {
            try
            {
                string query1 = "";
                if (Type == "HANA")
                    query1 = "call \"TEC_GetSapPostDetails\"";
                else
                    query1 = "Exec TEC_GetSapPostDetails";

                DataTable dt1;
                if (Type == "HANA")
                    dt1 = conn.ExecuteQueryForDataTable(query1);
                else
                    dt1 = conn.SQL_ExecuteQueryForDataTable(query1);
                GridView1.DataSource = dt1;
                dataTable3 = GridView1.DataSource as DataTable;
                ViewState["SAPTable"] = dataTable3;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "FillSeries");
            }
        }

        protected void gvUserDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Approve" || e.CommandName == "Reject" || e.CommandName == "View" || e.CommandName == "Preview")
                {
                    int rowIndex;
                    int.TryParse(e.CommandArgument.ToString(), out rowIndex);

                    string gstNo = gvUserDetails.DataKeys[rowIndex].Value.ToString();
                    Session["GSTNo"] = gstNo;
                    conn.writeLog("gvUserDetails_RowCommand", "Command: " + e.CommandName + ", GST: " + gstNo, "Debug");

                    if (e.CommandName == "Approve")
                    {
                    }
                    else if (e.CommandName == "Reject")
                    {
                    }
                    else if (e.CommandName == "View")
                    {
                        Session["Refresh"] = "N";
                        string gstNumber = e.CommandArgument.ToString();
                        Response.Redirect($"/Pages/View.aspx?gstNumber={gstNo}");
                    }
                    else if (e.CommandName == "Preview")
                    {
                        btnPreview_Click(gstNo);
                        string script = @"
            <script>
                window.open('VendorPreview.aspx', '_blank');
            </script>";

                        ClientScript.RegisterStartupScript(this.GetType(), "OpenPreview", script, false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "BankDetailspopup", "BankDetailspopup();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "gvUserDetails_RowCommand");
            }
        }

        protected void gvUserDetails_RowCommand2(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Approve" || e.CommandName == "Reject" || e.CommandName == "View" || e.CommandName == "Preview")
                {
                    int rowIndex;
                    int.TryParse(e.CommandArgument.ToString(), out rowIndex);

                    string gstNo = gvUserDetails.DataKeys[rowIndex].Value.ToString();
                    Session["GSTNo"] = gstNo;
                    conn.writeLog("gvUserDetails_RowCommand2", "Command: " + e.CommandName + ", GST: " + gstNo, "Debug");

                    if (e.CommandName == "Approve")
                    {
                    }
                    else if (e.CommandName == "Reject")
                    {
                    }
                    else if (e.CommandName == "View")
                    {
                        Session["Refresh"] = "N";
                        string gstNumber = e.CommandArgument.ToString();
                        Response.Redirect($"/Pages/View.aspx?gstNumber={gstNo}");
                    }
                    else if (e.CommandName == "Preview")
                    {
                        btnPreview_Click(gstNo);
                        string script = @"
            <script>
                window.open('VendorPreview.aspx', '_blank');
            </script>";

                        ClientScript.RegisterStartupScript(this.GetType(), "OpenPreview", script, false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "BankDetailspopup", "BankDetailspopup();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "gvUserDetails_RowCommand2");
            }
        }

        protected void gvUserDetails_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Approve" || e.CommandName == "Reject" || e.CommandName == "View" || e.CommandName == "Preview")
                {
                    int rowIndex;
                    int.TryParse(e.CommandArgument.ToString(), out rowIndex);

                    string gstNo = GridView3.DataKeys[rowIndex].Value.ToString();
                    Session["GSTNo"] = gstNo;
                    conn.writeLog("gvUserDetails_RowCommand1", "Command: " + e.CommandName + ", GST: " + gstNo, "Debug");

                    if (e.CommandName == "Approve")
                    {
                    }
                    else if (e.CommandName == "Reject")
                    {
                    }
                    //Commented By Sharmila (02-09-2026)
                    //else if (e.CommandName == "View")
                    //{
                    //    string gstNumber = e.CommandArgument.ToString();
                    //    Session["draftValue"] = "Draft";
                    //    Response.Redirect($"/Pages/View.aspx?gstNumber={gstNo}");
                    //}
                    else if (e.CommandName == "View")
                    {
                        string gstNumber = e.CommandArgument.ToString();
                        Session["draftValue"] = "Draft";
                        Response.Redirect($"/Pages/View.aspx?gstNumber={gstNo}", false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else if (e.CommandName == "Preview")
                    {
                        string gstNumber = e.CommandArgument.ToString();
                        btnPreview_Click(gstNo);
                        string script = @"
            <script>
                window.open('VendorPreview.aspx', '_blank');
            </script>";

                        ClientScript.RegisterStartupScript(this.GetType(), "OpenPreview", script, false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "BankDetailspopup", "BankDetailspopup();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "gvUserDetails_RowCommand1");
            }
        }

        protected void btnPreview_Click(string GstNo)
        {
            try
            {
                string gstNo = GstNo;
                conn.writeLog("btnPreview_Click", "Preview click for GST: " + gstNo, "Debug");
                if (string.IsNullOrEmpty(gstNo))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('GST Number missing');", true);
                    return;
                }

                DataTable ds = GetVendorDetails(gstNo);
                if (ds == null || ds.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('No data found for this GST number');", true);
                    return;
                }
                if (ds.Rows.Count > 0)
                {
                    DataRow dr = ds.Rows[0];
                    Dictionary<string, object> data = new Dictionary<string, object>();

                    data["GST Number"] = dr["GstNo"]?.ToString();
                    data["PAN Number"] = dr["PanNo"]?.ToString();
                    data["Trade Name"] = dr["TName"]?.ToString();
                    data["Nature of Business"] = dr["NatureOfBusinessActivity"]?.ToString();
                    data["Date of Establishment"] = dr["DateOfEstablishment"]?.ToString();
                    data["NHFS Contact Person"] = dr["ContactPerson"]?.ToString();
                    data["Designation"] = dr["DeclarationDesignation"]?.ToString();
                    data["Email ID"] = dr["EmailId"]?.ToString();
                    data["Mobile Number"] = dr["MobileNo"]?.ToString();
                    data["Office Telephone"] = dr["VerificationNo"]?.ToString();
                    data["TAN Number"] = dr["TANNo"]?.ToString();
                    data["Contact Person"] = dr["ContactPersonName"]?.ToString();

                    data["Registered Address"] = dr["Raddress1"]?.ToString() + "," + dr["Raddress2"]?.ToString() + "," + dr["Raddress3"]?.ToString() + "," + dr["registeredOfficeCity"]?.ToString() + "," + dr["Rstate"]?.ToString() + "," + dr["Rcountry"]?.ToString() + "-" + dr["Rzipcode"]?.ToString();
                    data["Billing Address"] = dr["Baddress1"]?.ToString() + "," + dr["Baddress2"]?.ToString() + "," + dr["Baddress3"]?.ToString() + "," + dr["businessBillingCity"]?.ToString() + "," + dr["Bstate"]?.ToString() + "," + dr["Bcountry"]?.ToString() + "-" + dr["Bzipcode"]?.ToString();
                    data["Shipping Address"] = dr["Saddress1"]?.ToString() + "," + dr["Saddress2"]?.ToString() + "," + dr["Saddress3"]?.ToString() + "," + dr["Scity"]?.ToString() + "," + dr["Sstate"]?.ToString() + "," + dr["Scountry"]?.ToString() + "-" + dr["Szipcode"]?.ToString();
                    data["Goods Return Address"] = dr["Gaddress1"]?.ToString() + "," + dr["Gaddress2"]?.ToString() + "," + dr["Gaddress3"]?.ToString() + "," + dr["Gcity"]?.ToString() + "," + dr["Gstate"]?.ToString() + "," + dr["Gcountry"]?.ToString() + "-" + dr["Gzipcode"]?.ToString();

                    data["Bank Name"] = dr["BankName"]?.ToString();
                    data["Account Name"] = dr["AccountName"]?.ToString();
                    data["Account Number"] = dr["AccountNumber"]?.ToString();
                    data["IFSC Code"] = dr["IfscCode"]?.ToString();
                    data["Branch Code"] = dr["BranchCode"]?.ToString();
                    data["Bank Address"] = dr["BankAddress"]?.ToString();

                    data["MSME Status"] = dr["MsmeRegistrationStatus"]?.ToString();
                    data["MSME Number"] = dr["MSMENo"]?.ToString();
                    data["Enterprise Type"] = dr["EnterpriseType"]?.ToString();
                    string Remarks = conn.GetSingleValue("Call \"GetRemarks\"('" + gstNo + "')");
                    data["Remarks"] = Remarks;

                    data["date"] = dr["AppliedDate"]?.ToString();
                    data["location"] = "TamilNadu";

                    string Id = dBConnection.GetSingleValue("select * from \"TEC_OLED\" where \"GstNo\"='" + gstNo + "'");
                    DataTable dt = dBConnection.ExecuteQueryForDataTable("Select * from \"PaymentDetails\" where \"Id\"='" + Id + "'");
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr1 = dt.Rows[0];
                        data["Credit Days"] = dr1["CreditDays"]?.ToString();
                        data["Discount"] = dr1["DisCount"]?.ToString();
                        data["md0_with"] = dr1["MarkDownTax0"]?.ToString();
                        data["md0_without"] = dr1["MarkDownWithoutTax0"]?.ToString();

                        data["md3_with"] = dr1["MarkDownTax3"]?.ToString();
                        data["md3_without"] = dr1["MarkDownWithoutTax3"]?.ToString();

                        data["md5_with"] = dr1["MarkDownTax5"]?.ToString();
                        data["md5_without"] = dr1["MarkDownWithoutTax5"]?.ToString();

                        data["md18_with"] = dr1["MarkDownTax18"]?.ToString();
                        data["md18_without"] = dr1["MarkDownWithoutTax18"]?.ToString();
                    }

                    data["Business Type"] = dr["BusinessType"]?.ToString();
                    data["Agency Email"] = dr["AgencyEmail"]?.ToString();
                    data["Agency Name"] = dr["AgencyName"]?.ToString();

                    data["Name"] = dr["DeclarationName"]?.ToString();
                    data["Designation"] = dr["DeclarationDesignation"]?.ToString();
                    data["Mobile No"] = dr["VerificationNo"]?.ToString();

                    List<GoodItem> goods = LoadGoodsByGST(gstNo);
                    Session["MajorGoods"] = goods;

                    Session["PreviewData"] = JsonConvert.SerializeObject(data);
                }
                string script = "<script>window.open('VendorPreview.aspx', '_blank');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "OpenPreview", script, false);
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnPreview_Click");
            }
        }

        private DataTable GetVendorDetails(string gstNo)
        {
            try
            {
                string query = "select * from \"TEC_OLED\" where \"GstNo\"='" + gstNo + "'";
                writeLog("GetVendorDetails Query : " + query, "Mail");
                DataTable dt = dBConnection.ExecuteQueryForDataTable(query);
                return dt;
            }
            catch (Exception ex)
            {
                dBConnection.LogError(ex, "GetVendorDetails");
                return new DataTable();
            }
        }

        private List<GoodItem> LoadGoodsByGST(string gstNo)
        {
            List<GoodItem> goods = new List<GoodItem>();
            try
            {
                string Id = dBConnection.GetSingleValue("select * from \"TEC_OLED\" where \"GstNo\"='" + gstNo + "'");
                string query = "select * from \"TEC_LED4\" where \"Id\"='" + Id + "'";
                DataTable dt = dBConnection.ExecuteQueryForDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        goods.Add(new GoodItem
                        {
                            SerialNo = i++,
                            Product = row["Product"]?.ToString(),
                            Brand = row["Brand"]?.ToString(),
                            Size = row["Size"]?.ToString(),
                            MaterialDescription = row["MaterialDescription"]?.ToString(),
                            HSNCode = row["HSNCode"]?.ToString(),
                            TaxPercentage = row["TaxPercentage"]?.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                dBConnection.LogError(ex, "LoadGoodsByGST");
            }
            return goods;
        }

        private void AddGridToData(Dictionary<string, object> data, GridView grid, string gridTitle)
        {
            if (grid == null || grid.Rows.Count == 0)
                return;

            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

            foreach (GridViewRow row in grid.Rows)
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>();

                for (int i = 0; i < grid.HeaderRow.Cells.Count; i++)
                {
                    string header = grid.HeaderRow.Cells[i].Text.Trim();
                    if (header != "Action" & header != "ImageUpload")
                    {
                        string value = string.Empty;

                        value = row.Cells[i].Text.Trim();

                        if (string.IsNullOrEmpty(value) || value == "&nbsp;")
                        {
                            foreach (System.Web.UI.Control ctrl in row.Cells[i].Controls)
                            {
                                if (ctrl is Label lbl) value = lbl.Text.Trim();
                                else if (ctrl is TextBox txt) value = txt.Text.Trim();
                                else if (ctrl is DropDownList ddl) value = ddl.SelectedItem.Text.Trim();
                                else if (ctrl is CheckBox chk) value = chk.Checked ? "Yes" : "No";
                            }
                        }

                        if (value == "&nbsp;") value = "";
                        rowData[header] = value;
                    }
                }

                rows.Add(rowData);
            }

            data[gridTitle] = rows;
        }

        protected void btnAcceptAll_Click(object sender, EventArgs e)
        {
            try
            {
                conn.writeLog("btnAcceptAll_Click", "Accept all initiated.", "Debug");
                foreach (GridViewRow row in gvUserDetails.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                    if (chk != null && chk.Checked)
                    {
                        string gstNo = gvUserDetails.DataKeys[row.RowIndex].Value.ToString();
                        string username = Session["username"].ToString();

                        DbConnection conn = new DbConnection();
                        string level = conn.GetSingleValue("Select \"Level\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                        string Department = string.Empty;
                        if (Type == "HANA")
                            Department = conn.GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                        else
                            Department = conn.SQL_GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                        string IsDepartment = conn.GetSingleValue("Select \"ApprovedDepartment\" from \"ApprovalCheck\" where \"ApprovedDepartment\"='" + Department + "'");
                        if (IsDepartment != "" && IsDepartment != null)
                        {
                            conn.ExecuteNonQuery("insert into \"ApprovalCheck\"  (\"UserName\",\"ApprovedDepartment\",\"DepartmentApprovedCount\",\"GSTNO\",\"Level\") values('" + username + "','" + Department + "',1,'" + gstNo + "','" + level + "')");
                        }
                        else
                        {
                            conn.ExecuteNonQuery("insert into \"ApprovalCheck\"  (\"UserName\",\"ApprovedDepartment\",\"DepartmentApprovedCount\",\"GSTNO\",\"Level\") values('" + username + "','" + Department + "',1,'" + gstNo + "','" + level + "')");
                        }
                        conn.ExecuteNonQuery("insert into \"ApprovalTrace\"  (\"User\",\"GstNo\",\"ApproveStatus\",\"Level\") values('" + username + "','" + gstNo + "','Y','" + level + "')");

                        if (Type == "HANA")
                            conn.ExecuteNonQuery("call \"IsApproved\"('" + username + "','" + Department + "','" + gstNo + "')");
                        else
                            conn.SQL_ExecuteNonQuery("Exec  \"IsApproved\" '" + username + "','" + Department + "','" + gstNo + "'");
                    }
                }
                string script = "alert('Approval Successfull');";
                ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);
                Response.Redirect("/Pages/VendorForm.aspx");
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnAcceptAll_Click");
            }
        }

        protected void btnRejectAll_Click(object sender, EventArgs e)
        {
            try
            {
                string reason = popuptext.Text;
                conn.writeLog("btnRejectAll_Click", "Reject all initiated. Reason: " + reason, "Debug");
                foreach (GridViewRow row in gvUserDetails.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                    if (chk != null && chk.Checked)
                    {
                        string gstNo = gvUserDetails.DataKeys[row.RowIndex].Value.ToString();
                        string username = Session["username"].ToString();
                        string Department = string.Empty;
                        string level = conn.GetSingleValue("Select \"Level\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                        if (Type == "HANA")
                            Department = conn.GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                        else
                            Department = conn.SQL_GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                        conn.ExecuteNonQuery("insert into \"ApprovalTrace\"  (\"User\",\"GstNo\",\"ApproveStatus\",\"RjectedReason\",\"Level\") values('" + username + "','" + gstNo + "','N','" + reason + "','" + level + "')");

                        string query = "UPDATE TEC_OLED SET \"Approval\"='N',\"Draft\"='Y',\"DraftApproved\"='N',\"RejectionStatus\"='Y', \"RejectionReason\"='" + reason + "', \"RejectedUser\"='" + username + "',\"ApprovedDepartment\"='" + Department + "' WHERE \"GstNo\"='" + gstNo + "'";
                        if (Type == "HANA")
                            conn.ExecuteNonQuery(query);
                        else
                            conn.SQL_ExecuteNonQuery(query);
                    }
                }
                string script = "alert('Rejected Successfully');";
                ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);
                Response.Redirect("/Pages/VendorForm.aspx");
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnRejectAll_Click");
            }
        }

        protected void ApproveVendor1(object sender, EventArgs e)
        {
            try
            {
                string remarks = txtRemarks.Text.Trim();
                string username = Session["username"].ToString();
                var typeName = sender.GetType().Name;
                ImageButton btn = sender as ImageButton;
                string gstNo = hdnGSTNo.Value;
                conn.writeLog("ApproveVendor1", "Approving vendor: " + gstNo + ", Remarks: " + remarks, "Debug");
                DbConnection conn1 = new DbConnection();
                string Department = string.Empty;
                string level = conn1.GetSingleValue("Select \"Level\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");

                if (Type == "HANA")
                    Department = conn1.GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                else
                    Department = conn1.SQL_GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");

                string IsDepartment = conn1.GetSingleValue("Select \"ApprovedDepartment\" from \"ApprovalCheck\" where \"ApprovedDepartment\"='" + Department + "'");
                if (IsDepartment != "" && IsDepartment != null)
                {
                    conn1.ExecuteNonQuery("insert into \"ApprovalCheck\"  (\"UserName\",\"ApprovedDepartment\",\"DepartmentApprovedCount\",\"GSTNO\",\"Level\",\"Reason\") values('" + username + "','" + Department + "',1,'" + gstNo + "','" + level + "','" + remarks + "')");
                }
                else
                {
                    conn1.ExecuteNonQuery("insert into \"ApprovalCheck\"  (\"UserName\",\"ApprovedDepartment\",\"DepartmentApprovedCount\",\"GSTNO\",\"Level\",\"Reason\") values('" + username + "','" + Department + "',1,'" + gstNo + "','" + level + "','" + remarks + "')");
                }

                if (Type == "HANA")
                    conn1.ExecuteNonQuery("call \"IsApproved\"('" + username + "','" + Department + "','" + gstNo + "')");
                else
                    conn1.SQL_ExecuteNonQuery("Exec  \"IsApproved\" '" + username + "','" + Department + "','" + gstNo + "'");

                conn1.ExecuteNonQuery("insert into \"ApprovalTrace\"  (\"User\",\"GstNo\",\"ApproveStatus\",\"Level\") values('" + username + "','" + gstNo + "','Y','" + level + "')");

                string script = "alert('Approval Successfull');";
                ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);
                Response.Redirect("/Pages/VendorForm.aspx");
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "ApproveVendor1");
            }
        }

        protected void btnReject(object sender, EventArgs e)
        {
            try
            {
                DbConnection conn = new DbConnection();
                string gstNo = hiddenGSTNo.Value;
                string username = Session["username"].ToString();
                string reason = popuptext1.Text;

                string gstAllowedStatus = ddlGstRecreate.SelectedValue;
                conn.writeLog("btnReject", "Rejecting vendor: " + gstNo + ", Reason: " + reason, "Debug");

                string level = conn.GetSingleValue("Select \"Level\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                string Department = string.Empty;
                if (Type == "HANA")
                    Department = conn.GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                else
                    Department = conn.SQL_GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");

                string query = "UPDATE TEC_OLED SET \"Approval\"='N', \"Draft\"='Y',\"RejectionStatus\"='Y',\"DraftApproved\"='N',\"RejectionReason\"='" + reason + "', \"RejectedUser\"='" + username + "',\"ApprovedDepartment\" = '" + Department + "' WHERE \"GstNo\"='" + gstNo + "'";
                if (Type == "HANA")
                    conn.ExecuteNonQuery(query);
                else
                    conn.SQL_ExecuteNonQuery(query);
                conn.ExecuteNonQuery("insert into \"ApprovalTrace\"  (\"User\",\"GstNo\",\"ApproveStatus\",\"RjectedReason\",\"Level\",\"ReApplySts\") values('" + username + "','" + gstNo + "','N','" + reason + "','" + level + "','" + gstAllowedStatus + "')");

                string toMail = conn.GetSingleValue("Select \"EmailId\" from \"TEC_OLED\" where \"GstNo\" = '" + gstNo + "'");

                if (!string.IsNullOrEmpty(toMail))
                {
                    sRejectType = "REJECT";
                    Session["RejectRemarks"] = reason;
                    SentMail(toMail, gstNo, "");
                    sRejectType = string.Empty;
                }

                string script = "alert('Rejected Successfully');";
                ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);
                Response.Redirect("/Pages/VendorForm.aspx");
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnReject");
            }
        }

        protected void ServerButton_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "ServerButton_Click");
            }
        }

        protected void BindGroup()
        {
            try
            {
                conn.writeLog("BindGroup", "Binding BP Groups.", "Debug");
                string query = "CALL TEC_VRF_GETBPGROUPLIST('" + Session["username"].ToString() + "')";
                DataTable dt = dBConnection.ExecuteQueryForDataTable(query);

                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "Name";
                ddlGroup.DataValueField = "Code";
                ddlGroup.DataBind();

                ddlGroup.Items.Insert(0, new ListItem("--Select Group--", ""));
            }
            catch (Exception ex)
            {
                dBConnection.LogError(ex, "BindGroup");
            }
        }

        protected void btnPerformOperation_Click(object sender, EventArgs e)
        {
            string Id = string.Empty;
            string CardCode = string.Empty;
            DataTable dataTable = null;
            DataTable dataTable1 = null;
            DataTable dataTable2 = null;
            string selectedGST = string.Empty;
            string VendorCode = string.Empty;
            string VendorName = string.Empty;
            string groupCode = ddlGroup.SelectedValue;

            try
            {
                conn.writeLog("btnPerformOperation_Click", "Push to SAP operation initiated.", "Debug");
                if (string.IsNullOrEmpty(ddlGroup.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select Group Code');", true);
                    return;
                }
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                    CheckBox chk1 = chkVI;
                    CheckBox chk2 = chkVT;
                    CheckBox chk3 = chkVC;
                    CheckBox chk4 = chkIHB;
                    if (chk != null && chk.Checked)
                    {
                        TextBox txtVendor = (TextBox)row.FindControl("txtVendorName");
                        VendorName = txtVendor.Text.Trim();
                        if (chk1.Checked) VendorCode += "VI-" + VendorName + ",";
                        if (chk2.Checked) VendorCode += "VT-" + VendorName + ",";
                        if (chk3.Checked) VendorCode += "VC-" + VendorName + ",";
                        if (chk4.Checked) VendorCode += "IHB-" + VendorName + ",";

                        selectedGST = row.Cells[3].Text.Trim();
                        break;
                    }
                }
                if (Type == "HANA")
                {
                    dataTable = dBConnection.ExecuteQueryForDataTable("call \"BPDetails\" ('CARDDETAILS')");
                    dataTable1 = dBConnection.ExecuteQueryForDataTable("call \"VendorCreation\"('" + selectedGST + "','" + VendorCode + "')");
                    dataTable2 = dBConnection.ExecuteQueryForDataTable("call \"BPDetails\" ('SERIES')");
                }
                else
                {
                    dataTable = dBConnection.SQL_ExecuteQueryForDataTable("EXEC [dbo].[BPDetails] 'CARDDETAILS'");
                    dataTable1 = dBConnection.SQL_ExecuteQueryForDataTable("EXEC [dbo].[VendorCreation] '" + selectedGST + "'");
                    dataTable2 = dBConnection.SQL_ExecuteQueryForDataTable("EXEC [dbo].[BPDetails] 'SERIES'");
                }
                dataTable3 = ViewState["SAPTable"] as DataTable;
                string DBName = WebConfigurationManager.AppSettings["DBName"];
                string DB = WebConfigurationManager.AppSettings["DBName1"];
                string user = DbConnection.DecryptFun(WebConfigurationManager.AppSettings["SAPUserName"]);
                string Pass = DbConnection.DecryptFun(WebConfigurationManager.AppSettings["SAPPassword"]);
                string TransURL = WebConfigurationManager.AppSettings["TransURL"];
                string loginURL = WebConfigurationManager.AppSettings["loginURL"];
                string StrRouteVal = "";

                Company oCompany;
                if (Type == "HANA")
                {
                    oCompany = new Company
                    {
                        Server = WebConfigurationManager.AppSettings["ServerIP"],
                        LicenseServer = WebConfigurationManager.AppSettings["LicenseServer"],
                        DbServerType = BoDataServerTypes.dst_HANADB,
                        CompanyDB = WebConfigurationManager.AppSettings["DBName1"],
                        UserName = DbConnection.DecryptFun(WebConfigurationManager.AppSettings["SAPUserName"]),
                        Password = DbConnection.DecryptFun(WebConfigurationManager.AppSettings["SAPPassword"]),
                        language = BoSuppLangs.ln_English,
                        UseTrusted = false
                    };
                }
                else
                {
                    oCompany = new Company
                    {
                        Server = WebConfigurationManager.AppSettings["Server"],
                        DbServerType = BoDataServerTypes.dst_MSSQL2019,
                        CompanyDB = WebConfigurationManager.AppSettings["DBName1"],
                        UserName = DbConnection.DecryptFun(WebConfigurationManager.AppSettings["SAPUserName"]),
                        Password = DbConnection.DecryptFun(WebConfigurationManager.AppSettings["SAPPassword"]),
                        language = BoSuppLangs.ln_English,
                        UseTrusted = false
                    };
                }

                if (oCompany.Connect() != 0)
                {
                    string err = oCompany.GetLastErrorDescription();
                    conn.writeLog("btnPerformOperation_Click", "SAP Company Connect failed: " + err, "Debug");
                    return;
                }

                Console.WriteLine("Connected to SAP Business One!");
                conn.writeLog("btnPerformOperation_Click", "Connected to SAP Business One successfully.", "Debug");

                try
                {
                    DbConnection db = new DbConnection();
                    BusinessPartners oBusinessPartner = (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                    for (int i = 0; i < dataTable1.Rows.Count; i++)
                    {
                        Id = dataTable1.Rows[i]["Id"].ToString();
                        CardCode = dataTable1.Rows[i]["CardCode"].ToString();
                        string tempFolderPath = db.GetSingleValue("select \"AttachPath\" from " + DB + ".OADP");
                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        List<Attachments2_Lines> attachmentLines = new List<Attachments2_Lines>();

                        string count;
                        string Type = WebConfigurationManager.AppSettings["ServerType"];
                        if (Type == "HANA")
                            count = db.GetSingleValue("select Count(*) from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "'");
                        else
                            count = db.SQL_GetSingleValue("select Count(*) from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "'");
                        int count1 = Convert.ToInt32(count);
                        for (int j = 1; j <= count1; j++)
                        {
                            string base64String;
                            string fileNameFromDB;
                            string fileTypeFromDB;
                            if (Type == "HANA")
                                base64String = db.GetSingleValue("select \"FileData\" from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "' and \"LineId\"='" + j + "'");
                            else
                                base64String = db.SQL_GetSingleValue("select \"FileData\" from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "' and \"LineId\"='" + j + "'");
                            if (Type == "HANA")
                                fileNameFromDB = db.GetSingleValue("select \"DocumentType\" from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "' and \"LineId\"='" + j + "'");
                            else
                                fileNameFromDB = db.SQL_GetSingleValue("select \"DocumentType\" from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "' and \"LineId\"='" + j + "'");
                            if (Type == "HANA")
                                fileTypeFromDB = db.GetSingleValue("select \"DocumentName\" from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "' and \"LineId\"='" + j + "'");
                            else
                                fileTypeFromDB = db.SQL_GetSingleValue("select \"DocumentName\" from " + DBName + ".tec_led7 where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "' and \"LineId\"='" + j + "'");
                            if (!string.IsNullOrWhiteSpace(base64String) && !string.IsNullOrWhiteSpace(fileTypeFromDB))
                            {
                                string fileExtension = Path.GetExtension(fileTypeFromDB);
                                string uniqueFileName = $"BPMaster_Doc-{dataTable1.Rows[i]["Id"].ToString()}-Line{j}-{fileNameFromDB}{fileExtension}";
                                string fullFilePath = Path.Combine(tempFolderPath, uniqueFileName);

                                string filepath = base64String;
                                if (File.Exists(filepath))
                                {
                                    base64String = Convert.ToBase64String(File.ReadAllBytes(filepath));
                                    Console.WriteLine(base64String);
                                }
                                else
                                {
                                    Console.WriteLine("File not found: " + filepath);
                                }

                                byte[] fileBytes = Convert.FromBase64String(base64String);
                                File.WriteAllBytes(fullFilePath, fileBytes);

                                Attachments2_Lines line = new Attachments2_Lines()
                                {
                                    FileName = Path.GetFileNameWithoutExtension(uniqueFileName),
                                    FileExtension = fileExtension.TrimStart('.'),
                                    SourcePath = tempFolderPath
                                };

                                attachmentLines.Add(line);
                            }
                        }
                        AttachmentsWrapper wrapper = new AttachmentsWrapper
                        {
                            Attachments2_Lines = attachmentLines
                        };

                        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
                        writeLog("Attachment Json : " + json, "VendorCreation");
                        string Discount = conn.GetSingleValue("Select ifnull(\"DisCount\",'0') from \"PaymentDetails\" where \"Id\" = '" + dataTable1.Rows[i]["Id"].ToString() + "'");
                        writeLog("Discount : " + Discount, "VendorCreation");
                        if (string.IsNullOrEmpty(Discount)) Discount = "0";
                        string CreditDays = conn.GetSingleValue("Call \"TEC_GetCreditDaysDetails\"('" + dataTable1.Rows[i]["Id"].ToString() + "')") != "" ? conn.GetSingleValue("Call \"TEC_GetCreditDaysDetails\"('" + dataTable1.Rows[i]["Id"].ToString() + "')") : "0";
                        writeLog("CreditDays : " + CreditDays, "VendorCreation");
                        string creditcode = conn.GetSingleValue("Select Trim(\"CreditDays\") from \"PaymentDetails\" where \"Id\"='" + dataTable1.Rows[i]["Id"].ToString() + "'");
                        writeLog("CreditCode : " + creditcode, "VendorCreation");
                        string creditDaysNumber = new string(CreditDays.Where(char.IsDigit).ToArray());
                        string creditCodeNumber = new string(creditcode.Where(char.IsDigit).ToArray());

                        string result = (creditDaysNumber == creditCodeNumber) ? "nodefault" : "default";

                        string GroupNum = conn.GetSingleValue("call \"TEC_GetGroupNum\" ('" + CreditDays + "')") != "" ? conn.GetSingleValue("call \"TEC_GetGroupNum\" ('" + CreditDays + "')") : "0";
                        writeLog("GroupNum : " + GroupNum, "VendorCreation");
                        writeLog("GroupCode : " + groupCode, "VendorCreation");
                        string Remarks = conn.GetSingleValue("Call \"GetRemarks\"('" + dataTable1.Rows[i]["GstNo"].ToString() + "')");
                        writeLog("Remarks : " + Remarks, "VendorCreation");
                        string rDoc = "0";
                        string sessionId = Login(TransURL, DB, user, Pass, out StrRouteVal);
                        string strRoutevalue = "";
                        string Result = TransactionPosting(TransURL + "Attachments2", json, sessionId, "Attachment", strRoutevalue, DB);
                        writeLog("Attachment Result : " + Result, "VendorCreation");
                        int AbsEntry = Convert.ToInt32(Result);
                        writeLog("Attachment Entry : " + AbsEntry, "VendorCreation");
                        Id = dataTable1.Rows[i]["Id"].ToString();
                        Vendor1 vendor = new Vendor1
                        {
                            CardCode = CardCode,
                            CardName = dataTable1.Rows[i]["TName"].ToString(),
                            CardType = "cSupplier",
                            GroupCode = Convert.ToInt32(groupCode.ToString()),
                            Phone1 = dataTable1.Rows[i]["MobileNo"].ToString(),
                            Phone2 = dataTable1.Rows[i]["VerificationNo"].ToString(),
                            EmailAddress = dataTable1.Rows[i]["EmailId"].ToString(),
                            FederalTaxID = dataTable1.Rows[i]["PanNo"].ToString(),
                            DiscountPercent = Convert.ToDouble(Discount),
                            PayTermsGrpCode = Convert.ToInt32(GroupNum),
                            FreeText = Remarks,
                            AttachmentEntry = AbsEntry,
                            U_MSMENo = dataTable1.Rows[i]["MSMENo"].ToString(),
                            U_VRFAppover = Session["username"].ToString(),
                            ContactEmployees = new List<ContactEmployee>
                            {
                                new ContactEmployee
                                {
                                    Name = dataTable1.Rows[i]["ContactPersonName"].ToString()
                                }
                            },
                            BPAddresses = new List<Address>
                            {
                                new Address
                                {
                                    AddressName = "Billing",
                                    AddressType = "bo_BillTo",
                                    Street = dataTable1.Rows[i]["Raddress1"].ToString(),
                                    City = dataTable1.Rows[i]["registeredOfficeCity"].ToString(),
                                    ZipCode = dataTable1.Rows[i]["Rzipcode"].ToString(),
                                    Country = dataTable1.Rows[i]["Rcountry"].ToString(),
                                    State = dataTable1.Rows[i]["Rstate"].ToString(),
                                    GSTIN = dataTable1.Rows[i]["GstNo"].ToString(),
                                }
                            },
                            BPBankAccounts = new List<BankAccount>
                            {
                                new BankAccount
                                {
                                    BankCode = dataTable1.Rows[i]["BankName"].ToString(),
                                    AccountNo = dataTable1.Rows[i]["AccountNumber"].ToString(),
                                    AccountName = dataTable1.Rows[i]["AccountName"].ToString(),
                                    BICSwiftCode = dataTable1.Rows[i]["IfscCode"].ToString()
                                }
                            }
                        };
                        string json1 = JsonConvert.SerializeObject(vendor);
                        LogVendorJson(vendor.CardCode, json1);

                        {
                            CardCode = dataTable1.Rows[i]["CardCode"].ToString();
                            oBusinessPartner.CardCode = CardCode;
                            oBusinessPartner.CardName = dataTable1.Rows[i]["TName"].ToString();
                            oBusinessPartner.CardType = SAPbobsCOM.BoCardTypes.cSupplier;
                            oBusinessPartner.GroupCode = Convert.ToInt32(groupCode);
                            string PanNumber = dataTable1.Rows[i]["PanNo"].ToString();
                            oBusinessPartner.UserFields.Fields.Item("U_MSMENo").Value = dataTable1.Rows[i]["MSMENo"].ToString();

                            oBusinessPartner.Phone1 = dataTable1.Rows[i]["MobileNo"].ToString();
                            oBusinessPartner.Phone2 = dataTable1.Rows[i]["VerificationNo"].ToString();
                            oBusinessPartner.EmailAddress = dataTable1.Rows[i]["EmailId"].ToString();
                            oBusinessPartner.DiscountPercent = Convert.ToDouble(Discount);
                            oBusinessPartner.PayTermsGrpCode = Convert.ToInt32(GroupNum);
                            oBusinessPartner.FiscalTaxID.TaxId0 = dataTable1.Rows[i]["PanNo"].ToString();

                            oBusinessPartner.FreeText = Remarks;
                            oBusinessPartner.ContactPerson = dataTable1.Rows[i]["ContactPersonName"].ToString();
                            oBusinessPartner.ContactEmployees.Name = dataTable1.Rows[i]["ContactPersonName"].ToString();

                            oBusinessPartner.Addresses.AddressType = BoAddressType.bo_ShipTo;
                            oBusinessPartner.Addresses.AddressName = dataTable1.Rows[i]["TName"].ToString();
                            oBusinessPartner.Addresses.Street = dataTable1.Rows[i]["Baddress1"].ToString();
                            oBusinessPartner.Addresses.Street = dataTable1.Rows[i]["Baddress2"].ToString();
                            oBusinessPartner.Addresses.StreetNo = dataTable1.Rows[i]["Raddress3"].ToString();
                            oBusinessPartner.Addresses.City = dataTable1.Rows[i]["businessBillingCity"].ToString();
                            oBusinessPartner.Addresses.ZipCode = dataTable1.Rows[i]["Bzipcode"].ToString();
                            oBusinessPartner.Addresses.Country = dataTable1.Rows[i]["Bcountry"].ToString();
                            oBusinessPartner.Addresses.State = dataTable1.Rows[i]["Bstate"].ToString();
                            oBusinessPartner.Addresses.GSTIN = dataTable1.Rows[i]["GstNo"].ToString();
                            oBusinessPartner.Addresses.GstType = SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                            oBusinessPartner.Addresses.UserFields.Fields.Item("U_IsVgst").Value = "Y";
                            oBusinessPartner.Addresses.Add();

                            oBusinessPartner.Addresses.AddressType = BoAddressType.bo_BillTo;
                            oBusinessPartner.Addresses.AddressName = dataTable1.Rows[i]["TName"].ToString();
                            oBusinessPartner.Addresses.Street = dataTable1.Rows[i]["Raddress1"].ToString();
                            oBusinessPartner.Addresses.Block = dataTable1.Rows[i]["Raddress2"].ToString();
                            oBusinessPartner.Addresses.StreetNo = dataTable1.Rows[i]["Raddress3"].ToString();
                            oBusinessPartner.Addresses.City = dataTable1.Rows[i]["registeredOfficeCity"].ToString();
                            oBusinessPartner.Addresses.ZipCode = dataTable1.Rows[i]["Rzipcode"].ToString();
                            oBusinessPartner.Addresses.Country = dataTable1.Rows[i]["Rcountry"].ToString();
                            oBusinessPartner.Addresses.State = dataTable1.Rows[i]["Rstate"].ToString();
                            oBusinessPartner.Addresses.GSTIN = dataTable1.Rows[i]["GstNo"].ToString();
                            oBusinessPartner.Addresses.GstType = SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                            oBusinessPartner.Addresses.UserFields.Fields.Item("U_IsVgst").Value = "Y";
                            oBusinessPartner.Addresses.Add();

                            oBusinessPartner.BPBankAccounts.AccountNo = dataTable1.Rows[i]["AccountNumber"].ToString();
                            oBusinessPartner.BPBankAccounts.BankCode = dataTable1.Rows[i]["BankName"].ToString();
                            oBusinessPartner.BPBankAccounts.BICSwiftCode = dataTable1.Rows[i]["IfscCode"].ToString();
                            oBusinessPartner.BPBankAccounts.AccountName = dataTable1.Rows[i]["AccountName"].ToString();
                            oBusinessPartner.BPBankAccounts.Add();
                            oBusinessPartner.UserFields.Fields.Item("U_VRFAppover").Value = Session["username"].ToString();

                            oBusinessPartner.AttachmentEntry = AbsEntry;
                        }
                        if (oBusinessPartner.Add() != 0)
                        {
                            string err = oCompany.GetLastErrorDescription();
                            conn.writeLog("btnPerformOperation_Click", "BP Add error: " + err, "Debug");
                            string error = err.Replace("'", "") + "'||'Vendor-'||'" + dataTable1.Rows[i]["CardCode"].ToString();
                            string update = "update tec_oled set \"SAPRejReason\"='" + error + "' where \"Id\"=" + dataTable1.Rows[i]["Id"].ToString() + "";
                            if (Type == "HANA")
                                dBConnection.ExecuteNonQuery(update);
                            else
                                dBConnection.SQL_ExecuteNonQuery(update);
                            string script = $@"
Swal.fire({{
    title: 'Error',
    text: '{err.Replace("'", " ")}',
    icon: 'error',
    confirmButtonText: 'OK'
}}).then((result) => {{
    if (result.isConfirmed) {{
        location.reload();
    }}
}});
";
                            ClientScript.RegisterStartupScript(this.GetType(), "SwalTest", script, true);
                        }
                        else
                        {
                            Log.WriteToLogFile_Debug("Posting Completed for the Traders" + dataTable1.Rows[i]["TName"].ToString() + "    Completed Successfully.", "SAP Posting");
                            string update = "";
                            if (result == "nodefault")
                            {
                                update = "update tec_oled set \"SAPRejReason\"=''||'Vendor-'||'" + dataTable1.Rows[i]["CardCode"].ToString() + "'||'->Created' where \"Id\"=" + dataTable1.Rows[i]["Id"].ToString() + "";
                            }
                            else
                            {
                                update = "update tec_oled set \"SAPRejReason\"=''||'Vendor-'||'" + dataTable1.Rows[i]["CardCode"].ToString() + "'||'->Created With Default CreditDays' where \"Id\"=" + dataTable1.Rows[i]["Id"].ToString() + "";
                            }
                            string toMail = conn.GetSingleValue("Select \"EmailId\" from \"TEC_OLED\" where \"Id\" = '" + dataTable1.Rows[i]["Id"].ToString() + "'");
                            if (Type == "HANA")
                                dBConnection.ExecuteNonQuery(update);
                            else
                                dBConnection.SQL_ExecuteNonQuery(update);
                            SentMail(toMail, selectedGST, dataTable1.Rows[i]["CardCode"].ToString());
                            conn.ExecuteNonQuery("Insert into \"Mail_Log\" (\"GstNo\",\"Type\",\"ActionDate\") values('" + dataTable1.Rows[i]["GstNo"].ToString() + "','Created',Current_Date)");
                        }
                    }
                    if (!(string.IsNullOrEmpty(CardCode)))
                    {
                        string message = "Business Partner added successfully!";
                        string script = $@"Swal.fire({{title:'Success!',text:'{message}',icon:'success',confirmButtonText:'OK'}}).then((result) => 
                                        {{if (result.isConfirmed) 
                                        {{  location.reload();}}}});";

                        ClientScript.RegisterStartupScript(this.GetType(), "SwalTest", script, true);
                    }
                }
                catch (Exception ex)
                {
                    conn.LogError(ex, "btnPerformOperation_Click");
                    string update = "update tec_oled set \"SAPRejReason\"='" + ex.Message.Replace("'", "") + "'||'-->Vendor-->'||'" + CardCode + "' where \"Id\"=" + Id + "";
                    if (Type == "HANA")
                        dBConnection.ExecuteNonQuery(update);
                    else
                        dBConnection.SQL_ExecuteNonQuery(update);

                    string script = $@"
Swal.fire({{
    title: 'Error!',
    text: '{ex.Message}',
    icon: 'error',
    confirmButtonText: 'OK'
}}).then((result) => {{
    if (result.isConfirmed) {{
        location.reload();
    }}
}});
";
                    ClientScript.RegisterStartupScript(this.GetType(), "SwalTest", script, true);
                }
                finally
                {
                    oCompany.Disconnect();
                    Console.WriteLine("Disconnected from SAP Business One.");
                }
                Session["LastPushToSAP"] = "Y";
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnPerformOperation_Click");
            }
        }

        public static void LogVendorJson(string cardCode, string json)
        {
            try
            {
                string folderPath = ConfigurationManager.AppSettings["VendorJsonLogPath"];

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, "VendorLog.txt");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                string logMessage = $"Date: {DateTime.Now}\r\n" +
                                    $"CardCode: {cardCode}\r\n" +
                                    $"JSON: {json}\r\n" +
                                    $"-----------------------------------------\r\n";

                File.AppendAllText(filePath, logMessage);
            }
            catch (Exception ex)
            {
                DbConnection.StaticLogError(ex, "LogVendorJson");
            }
        }

        public class Vendor1
        {
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string CardType { get; set; }
            public int GroupCode { get; set; }
            public string Phone1 { get; set; }
            public string Phone2 { get; set; }
            public string EmailAddress { get; set; }
            public string FederalTaxID { get; set; }
            public double DiscountPercent { get; set; }
            public int PayTermsGrpCode { get; set; }
            public string FreeText { get; set; }
            public int AttachmentEntry { get; set; }

            public List<ContactEmployee> ContactEmployees { get; set; }
            public List<Address> BPAddresses { get; set; }
            public List<BankAccount> BPBankAccounts { get; set; }

            public string U_MSMENo { get; set; }
            public string U_VRFAppover { get; set; }
        }

        public class ContactEmployee
        {
            public string Name { get; set; }
        }

        public class Address
        {
            public string AddressName { get; set; }
            public string AddressType { get; set; }
            public string Street { get; set; }
            public string Block { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string GSTIN { get; set; }
        }

        public class BankAccount
        {
            public string BankCode { get; set; }
            public string AccountNo { get; set; }
            public string AccountName { get; set; }
            public string BICSwiftCode { get; set; }
        }

        protected void SentMail(string toMail, string selectedGST, string CardCode)
        {
            try
            {
                string gstNo = selectedGST;
                string agentMail = string.Empty;
                var data1 = new Dictionary<string, object>();
                List<GoodItem> goods = new List<GoodItem>();
                writeLog("SentMail started for GST: " + gstNo, "Mail");
                DataTable ds = GetVendorDetails(gstNo);
                writeLog("Getting data", "Mail");
                if (ds.Rows.Count > 0)
                {
                    writeLog("Getting Row Data", "Mail");
                    DataRow dr = ds.Rows[0];

                    Dictionary<string, object> data = new Dictionary<string, object>();

                    data["GST Number"] = dr["GstNo"]?.ToString();
                    data["PAN Number"] = dr["PanNo"]?.ToString();
                    data["Trade Name"] = dr["TName"]?.ToString();
                    data["Nature of Business"] = dr["NatureOfBusinessActivity"]?.ToString();
                    data["Date of Establishment"] = dr["DateOfEstablishment"]?.ToString();
                    data["NHFS Contact Person"] = dr["ContactPerson"]?.ToString();
                    data["Designation"] = dr["DeclarationDesignation"]?.ToString();
                    data["Email ID"] = dr["EmailId"]?.ToString();
                    data["Mobile Number"] = dr["MobileNo"]?.ToString();
                    data["Office Telephone"] = dr["VerificationNo"]?.ToString();
                    data["TAN Number"] = dr["TANNo"]?.ToString();
                    data["Contact Person"] = dr["ContactPersonName"]?.ToString();

                    data["Registered Address"] = dr["Raddress1"]?.ToString() + "," + dr["Raddress2"]?.ToString() + "," + dr["Raddress3"]?.ToString() + "," + dr["registeredOfficeCity"]?.ToString() + "," + dr["Rstate"]?.ToString() + "," + dr["Rcountry"]?.ToString() + "-" + dr["Rzipcode"]?.ToString();
                    data["Billing Address"] = dr["Baddress1"]?.ToString() + "," + dr["Baddress2"]?.ToString() + "," + dr["Baddress3"]?.ToString() + "," + dr["businessBillingCity"]?.ToString() + "," + dr["Bstate"]?.ToString() + "," + dr["Bcountry"]?.ToString() + "-" + dr["Bzipcode"]?.ToString();
                    data["Shipping Address"] = dr["Saddress1"]?.ToString() + "," + dr["Saddress2"]?.ToString() + "," + dr["Saddress3"]?.ToString() + "," + dr["Scity"]?.ToString() + "," + dr["Sstate"]?.ToString() + "," + dr["Scountry"]?.ToString() + "-" + dr["Szipcode"]?.ToString();
                    data["Goods Return Address"] = dr["Gaddress1"]?.ToString() + "," + dr["Gaddress2"]?.ToString() + "," + dr["Gaddress3"]?.ToString() + "," + dr["Gcity"]?.ToString() + "," + dr["Gstate"]?.ToString() + "," + dr["Gcountry"]?.ToString() + "-" + dr["Gzipcode"]?.ToString();

                    data["Bank Name"] = dr["BankName"]?.ToString();
                    data["Account Name"] = dr["AccountName"]?.ToString();
                    data["Account Number"] = dr["AccountNumber"]?.ToString();
                    data["IFSC Code"] = dr["IfscCode"]?.ToString();
                    data["Branch Code"] = dr["BranchCode"]?.ToString();
                    data["Bank Address"] = dr["BankAddress"]?.ToString();

                    data["MSME Status"] = dr["MsmeRegistrationStatus"]?.ToString();
                    data["MSME Number"] = dr["MSMENo"]?.ToString();
                    data["Enterprise Type"] = dr["EnterpriseType"]?.ToString();
                    string Remarks = conn.GetSingleValue("Call \"GetRemarks\"('" + gstNo + "')");
                    data["Remarks"] = Remarks;

                    data["date"] = DateTime.Now.ToString("yyyy-MM-dd");
                    data["location"] = "TamilNadu";

                    string Id = dBConnection.GetSingleValue("select * from \"TEC_OLED\" where \"GstNo\"='" + gstNo + "'");
                    DataTable dt = dBConnection.ExecuteQueryForDataTable("Select * from \"PaymentDetails\" where \"Id\"='" + Id + "'");

                    writeLog("Getting Payment details", "Mail");
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr1 = dt.Rows[0];
                        data["Credit Days"] = dr1["CreditDays"]?.ToString();
                        data["Discount"] = dr1["DisCount"]?.ToString();
                        data["md0_with"] = dr1["MarkDownTax0"]?.ToString();
                        data["md0_without"] = dr1["MarkDownWithoutTax0"]?.ToString();
                        data["md3_with"] = dr1["MarkDownTax3"]?.ToString();
                        data["md3_without"] = dr1["MarkDownWithoutTax3"]?.ToString();
                        data["md5_with"] = dr1["MarkDownTax5"]?.ToString();
                        data["md5_without"] = dr1["MarkDownWithoutTax5"]?.ToString();
                        data["md18_with"] = dr1["MarkDownTax18"]?.ToString();
                        data["md18_without"] = dr1["MarkDownWithoutTax18"]?.ToString();
                    }

                    data["Business Type"] = dr["BusinessType"]?.ToString();
                    data["Agency Email"] = dr["AgencyEmail"]?.ToString();
                    data["Agency Name"] = dr["AgencyName"]?.ToString();
                    agentMail = dr["AgencyEmail"]?.ToString();

                    data["Name"] = dr["DeclarationName"]?.ToString();
                    data["Designation"] = dr["DeclarationDesignation"]?.ToString();
                    data["Mobile No"] = dr["VerificationNo"]?.ToString();
                    data["Code"] = CardCode;

                    goods = LoadGoodsByGST(gstNo);
                    Session["MajorGoods"] = goods;

                    data1 = data as Dictionary<string, object>;
                    Session["PreviewData"] = JsonConvert.SerializeObject(data);
                }

                string htmlContent = GenerateVendorHtmlWithData(data1, goods);
                writeLog("Getting HtmlContent", "Mail");
                byte[] pdfBytes = ConvertHtmlToPdf(htmlContent);
                writeLog("Getting pdf", "Mail");
                if (!string.IsNullOrEmpty(sRejectType) && sRejectType == "REJECT")
                {
                    try
                    {
                        DataTable dt = dBConnection.ExecuteQueryForDataTable("Call \"Mail_BOSY&SUBJECT\"('REJECT')");
                        string body = "", subject = "", ccMails = "";

                        foreach (DataRow row in dt.Rows)
                        {
                            body = row["Body"].ToString();
                            subject = row["Subject"].ToString();
                            if (dt.Columns.Contains("CCMail")) ccMails = row["CCMail"].ToString();
                        }
                        writeLog("REJECT Mail Started", "Mail");
                        using (MailMessage mail = new MailMessage())
                        {
                            string frommail = ConfigurationManager.AppSettings["MAILID"];
                            string username = ConfigurationManager.AppSettings["SMTPUSER"];
                            string password = ConfigurationManager.AppSettings["SMTPPWD"];
                            string server = ConfigurationManager.AppSettings["SMTPSERVER"];
                            int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPORT"]);

                            mail.From = new MailAddress(frommail);
                            writeLog("To Mail :" + toMail, "Mail");
                            writeLog("Agent Mail :" + agentMail, "Mail");
                            writeLog("CC Mail :" + ccMails, "Mail");
                            if (!string.IsNullOrWhiteSpace(toMail))
                                mail.To.Add(toMail.Trim());

                            if (!string.IsNullOrWhiteSpace(agentMail))
                                mail.To.Add(agentMail.Trim());

                            if (!string.IsNullOrWhiteSpace(ccMails))
                            {
                                foreach (var cc in ccMails.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                                    mail.CC.Add(cc.Trim());
                            }
                            
                            mail.Subject = subject;
                            body = body.Replace("{Vendor Name}", data1["Trade Name"].ToString());
                            body = body.Replace("{Remarks}", Session["RejectRemarks"].ToString());
                            mail.Body = body;
                            mail.IsBodyHtml = true;

                            using (MemoryStream ms = new MemoryStream(pdfBytes))
                            {
                                mail.Attachments.Add(new System.Net.Mail.Attachment(ms, "VendorRegistrationForm.pdf", "application/pdf"));

                                using (SmtpClient smtp = new SmtpClient(server, port))
                                {
                                    smtp.Credentials = new System.Net.NetworkCredential(username, password);
                                    smtp.EnableSsl = true;
                                    writeLog("Mail sending", "Mail");
                                    smtp.Send(mail);
                                    writeLog("Mail Ended", "Mail");
                                }
                            }
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "mailSuccess", "alert('Mail sent successfully!');", true);
                    }
                    catch (Exception ex)
                    {
                        writeLog("Error while sending mail : " + ex.Message, "Mail");
                        conn.LogError(ex, "SentMail_Reject");
                        ScriptManager.RegisterStartupScript(this, GetType(), "mailError", $"alert('Error sending mail: {ex.Message}');", true);
                    }
                }
                else
                {
                    try
                    {
                        DataTable dt = dBConnection.ExecuteQueryForDataTable("Call \"Mail_BOSY&SUBJECT\"('SAP')");
                        string body = "", subject = "", ccMails = "";

                        foreach (DataRow row in dt.Rows)
                        {
                            body = row["Body"].ToString();
                            subject = row["Subject"].ToString();
                            if (dt.Columns.Contains("CCMail")) ccMails = row["CCMail"].ToString();
                        }
                        writeLog("SAP Mail Started", "Mail");
                        using (MailMessage mail = new MailMessage())
                        {
                            string frommail = ConfigurationManager.AppSettings["MAILID"];
                            string username = ConfigurationManager.AppSettings["SMTPUSER"];
                            string password = ConfigurationManager.AppSettings["SMTPPWD"];
                            string server = ConfigurationManager.AppSettings["SMTPSERVER"];
                            int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPORT"]);

                            mail.From = new MailAddress(frommail);
                            writeLog("To Mail :" + toMail, "Mail");
                            writeLog("Agent Mail :" + agentMail, "Mail");
                            writeLog("CC Mail :" + ccMails, "Mail");
                            mail.To.Add(toMail);
                            mail.To.Add(agentMail);
                            if (!string.IsNullOrWhiteSpace(ccMails))
                            {
                                foreach (var cc in ccMails.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                                    mail.CC.Add(cc.Trim());
                            }

                            mail.Subject = subject;
                            body = body.Replace("{Vendor Name}", data1["Trade Name"].ToString());
                            body = body.Replace("{Vendor Code}", data1["Code"].ToString());
                            mail.Body = body;
                            mail.IsBodyHtml = true;

                            using (MemoryStream ms = new MemoryStream(pdfBytes))
                            {
                                mail.Attachments.Add(new System.Net.Mail.Attachment(ms, "VendorRegistrationForm.pdf", "application/pdf"));

                                using (SmtpClient smtp = new SmtpClient(server, port))
                                {
                                    smtp.Credentials = new System.Net.NetworkCredential(username, password);
                                    smtp.EnableSsl = true;
                                    writeLog("Mail sending", "Mail");
                                    smtp.Send(mail);
                                    writeLog("Mail Ended", "Mail");
                                }
                            }
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "mailSuccess", "alert('Mail sent successfully!');", true);
                    }
                    catch (Exception ex)
                    {
                        writeLog("Error while sending mail : " + ex.Message, "Mail");
                        conn.LogError(ex, "SentMail_SAP");
                        ScriptManager.RegisterStartupScript(this, GetType(), "mailError", $"alert('Error sending mail: {ex.Message}');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "SentMail");
            }
        }

        private string GenerateVendorHtmlWithData(Dictionary<string, object> data, List<GoodItem> goods)
        {
            try
            {
                string htmlTemplate = System.IO.File.ReadAllText(Server.MapPath("~/Design/VendorForm1.html"));
                htmlTemplate = Regex.Replace(htmlTemplate, @"<div class=""watermark""[^>]*>.*?</div>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                htmlTemplate = PopulateFields(htmlTemplate, data);
                htmlTemplate = htmlTemplate.Replace(
                    "<div class=\"items-section\" id=\"items_supplied\"></div>",
                    GenerateItemsHtml(goods)
                );
                return htmlTemplate;
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "GenerateVendorHtmlWithData");
                return string.Empty;
            }
        }

        private string GenerateItemsHtml(List<GoodItem> goods)
        {
            try
            {
                StringBuilder items = new StringBuilder();
                string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
                for (int i = 0; i < goods.Count && i < letters.Length; i++)
                {
                    var item = goods[i];
                    items.Append($@"
                <div style=""margin-bottom: 12px; line-height: 1.4;"">
                    <div style=""margin-bottom: 6px;""><strong>{letters[i]}) Product:</strong> {HttpUtility.HtmlEncode(item.Product ?? "")}</div>
                    <div style=""margin-bottom: 6px;""><strong>Brand:</strong> {HttpUtility.HtmlEncode(item.Brand ?? "")}</div>
                    <div><strong>Size:</strong> {HttpUtility.HtmlEncode(item.Size ?? "")}</div>
                </div>");
                }
                return $"<div class=\"items-section\" id=\"items_supplied\">{items}</div>";
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "GenerateItemsHtml");
                return "<div class=\"items-section\" id=\"items_supplied\"></div>";
            }
        }

        private string PopulateFields(string html, Dictionary<string, object> data)
        {
            try
            {
                var fieldMappings = new Dictionary<string, string>
                {
                    ["vendor_name"] = "Trade Name",
                    ["address"] = "Billing Address",
                    ["registered_office"] = "Registered Address",
                    ["business"] = "Nature of Business",
                    ["contact1"] = "Mobile Number",
                    ["contact2"] = "Office Telephone",
                    ["email1"] = "Email ID",
                    ["email2"] = "Agency Email",
                    ["proprietor"] = "Contact Person",
                    ["prop_phone"] = "Mobile No",
                    ["bank_branch"] = "Bank Name",
                    ["account_no"] = "Account Number",
                    ["ifsc"] = "IFSC Code",
                    ["return_address"] = "Goods Return Address",
                    ["days"] = "Credit Days",
                    ["md0_with"] = "md0_with",
                    ["md0_without"] = "md0_without",
                    ["md3_with"] = "md3_with",
                    ["md3_without"] = "md3_without",
                    ["md5_with"] = "md5_with",
                    ["md5_without"] = "md5_without",
                    ["md18_with"] = "md18_with",
                    ["md18_without"] = "md18_without",
                    ["discount"] = "Discount",
                    ["gst"] = "GST Number",
                    ["pan"] = "PAN Number",
                    ["msme"] = "MSME Number",
                    ["msme_date"] = "Date of Establishment",
                    ["enterprise"] = "Enterprise Type",
                    ["agency_direct"] = "Business Type",
                    ["agency_email"] = "Agency Email",
                    ["contact_person"] = "NHFS Contact Person",
                    ["location"] = "location",
                    ["form_date"] = "date",
                    ["code_no"] = "Code",
                    ["Remarks"] = "Remarks"
                };

                foreach (var mapping in fieldMappings)
                {
                    string value = GetSafeValue(data, mapping.Value);
                    string pattern = $@"<span class=""readonly-field"" id=""{mapping.Key}""></span>";
                    string replacement = $@"<span class=""readonly-field"" id=""{mapping.Key}"">{HttpUtility.HtmlEncode(value)}</span>";
                    html = html.Replace(pattern, replacement);
                }
                string remarksValue = GetSafeValue(data, "Remarks");

                string remarksPattern = @"<span class=""remarks-field"" id=""Remarks""></span>";
                string formattedRemarks = HttpUtility.HtmlEncode(remarksValue)
                            .Replace("\r\n", "<br>")
                            .Replace("\n", "<br>");

                string remarksReplacement = $@"<span class=""remarks-field"" id=""Remarks"">{formattedRemarks}</span>";
                html = html.Replace(remarksPattern, remarksReplacement);

                return html;
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "PopulateFields");
                return html;
            }
        }

        private string GetSafeValue(Dictionary<string, object> data, string key)
        {
            data.TryGetValue(key, out object value);
            return value?.ToString() ?? "";
        }

        private byte[] ConvertHtmlToPdf(string htmlContent)
        {
            try
            {
                string logoPath = Server.MapPath("~/Images/Logo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    byte[] logoBytes = System.IO.File.ReadAllBytes(logoPath);
                    string base64Logo = Convert.ToBase64String(logoBytes);
                    htmlContent = htmlContent.Replace("../Images/Logo.png", $"data:image/png;base64,{base64Logo}");
                }

                HtmlToPdf converter = new HtmlToPdf();

                converter.Options.MaxPageLoadTime = 180;
                converter.Options.MinPageLoadTime = 5;
                converter.Options.KeepTextsTogether = true;

                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                converter.Options.MarginTop = 30;
                converter.Options.MarginBottom = 5;
                converter.Options.MarginLeft = 28;
                converter.Options.MarginRight = 28;

                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                using (MemoryStream ms = new MemoryStream())
                {
                    doc.Save(ms);
                    doc.Close();
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "ConvertHtmlToPdf");
                return new byte[0];
            }
        }

        public class Attachments2_Lines
        {
            public string FileName { get; set; }
            public string FileExtension { get; set; }
            public string SourcePath { get; set; }
        }

        public class AttachmentsWrapper
        {
            public List<Attachments2_Lines> Attachments2_Lines { get; set; }
        }

        public string Login(string URL, string CompanyDB, string UserName, string Password, out string strRouteVal)
        {
            string str_Response = string.Empty;
            string ResponseMessage = string.Empty;

            try
            {
                string strFun = "Login";
                string sURL = URL + strFun;
                string json = "{\"CompanyDB\": \"" + CompanyDB + "\", \"UserName\": \"" + UserName + "\", \"Password\": \"" + Password + "\"}";
                var client = new RestClient(sURL);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();
                if (value != null)
                {
                    str_Response = JsonStringToDataTable(value, strFun);
                }
                CookieContainer cookie = new CookieContainer();
                var cookie_1 = response.Cookies.FirstOrDefault();
                var cookie_2 = response.Cookies.LastOrDefault();
                CN1 = cookie_1.Name;
                CN2 = cookie_2.Name;
                CV1 = cookie_1.Value;
                CV2 = cookie_2.Value;
                strRouteVal = CV2;
                if (str_Response == "Company Connected")
                {
                    ResponseMessage = CV1;
                }
                else
                {
                    ResponseMessage = str_Response;
                }

                return ResponseMessage;
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "Login_ServiceLayer");
                throw;
            }
        }

        public string TransactionPosting(string URL, string MasterData, string str_SessionID, string TransactionType, string strRoutevalue, string strCompDB)
        {
            string str_Response = string.Empty;
            int absoluteEntry = 0;
            try
            {
                CV1 = str_SessionID;
                string strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", MasterData, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;
                JObject jsonResponse = JObject.Parse(response.Content);

                absoluteEntry = (int)jsonResponse["AbsoluteEntry"];
                str_Response = Convert.ToString(absoluteEntry);

                if (value != null)
                {
                    str_Response = JsonStringToDataTable(value, strFun);
                }
            }
            catch (WebException ex)
            {
                conn.LogError(ex, "TransactionPosting");
                str_Response = ex.ToString();
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "TransactionPosting");
            }
            str_Response = Convert.ToString(absoluteEntry);
            return str_Response;
        }

        public string TransactionPosting1(string URL, string MasterData, string str_SessionID, string TransactionType, string strRoutevalue, string strCompDB)
        {
            string str_Response = string.Empty;
            int absoluteEntry = 0;
            try
            {
                CV1 = str_SessionID;
                string strFun = TransactionType;
                var client = new RestClient(URL + "?SessionId=" + CV1);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", MasterData, ParameterType.RequestBody);

                request.AddParameter("B1SESSION", CV1, ParameterType.Cookie);
                request.AddParameter("ROUTEID", strRoutevalue, ParameterType.Cookie);
                request.AddParameter("CompanyDB", strCompDB, ParameterType.Cookie);

                IRestResponse response = client.Execute(request);
                dynamic value = JsonConvert.DeserializeObject(response.Content);
                value = (value == null) ? null : value.ToString();

                str_Response = value;
                JObject jsonResponse = JObject.Parse(response.Content);

                str_Response = Convert.ToString(absoluteEntry);

                if (value != null)
                {
                    str_Response = JsonStringToDataTable(value, strFun);
                }
            }
            catch (WebException ex)
            {
                conn.LogError(ex, "TransactionPosting1");
                str_Response = ex.ToString();
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "TransactionPosting1");
            }
            str_Response = Convert.ToString(absoluteEntry);
            return str_Response;
        }

        private void RejectVendor(string gstNo)
        {
        }

        private void ViewVendorDetails(string gstNo)
        {
        }

        public class SerialList
        {
            public string seriesName { get; set; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [WebMethod]
        public static List<string> SearchSerial(string prefixText, int count)
        {
            string Type = WebConfigurationManager.AppSettings["ServerType"];
            DbConnection conn = new DbConnection();
            List<string> SerialListItems = new List<string>();

            try
            {
                conn.writeLog("SearchSerial", "Searching series for prefix: " + prefixText, "Debug");
                string query;
                if (Type == "HANA")
                    query = "call \"GetSeries\" ('" + prefixText.ToUpper() + "')";
                else
                    query = "Exec GetSeries '" + prefixText.ToUpper() + "'";
                DataTable dt;
                if (Type == "HANA")
                    dt = conn.ExecuteQueryForDataTable(query);
                else
                    dt = conn.SQL_ExecuteQueryForDataTable(query);
                foreach (DataRow row in dt.Rows)
                {
                    string seriesName = row["SeriesName"].ToString();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(seriesName, seriesName);
                    SerialListItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "SearchSerial");
                throw new Exception($"Error in SearchSerial: {ex.Message}");
            }

            return SerialListItems;
        }

        public string JsonStringToDataTable(string jsonString, string strFun)
        {
            try
            {
                DataTable dt = new DataTable();
                var trgArray = new JArray();
                var cleanRow1 = new JObject();
                var cleanRow = new JObject();
                var Rows = new JObject();
                var js = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
                var jsonLinq = JObject.Parse(jsonString);
                string Cnd = string.Empty;
                string errval = string.Empty;
                string DocEntry = string.Empty;
                string status = string.Empty;
                string message = string.Empty;
                string DocNum = string.Empty;
                string CardCode = string.Empty;
                string CardName = string.Empty;
                string DocumentEntry = string.Empty;
                string DocumentNumber = string.Empty;
                string ServiceCallId = string.Empty;
                var ssdf = Newtonsoft.Json.JsonConvert.SerializeObject(jsonLinq);

                foreach (var lin in jsonLinq)
                {
                    if (lin.Key == "error")
                    {
                        Rows.Add(lin.Key, lin.Key);
                        var srcArray = jsonLinq.Descendants().Where(d => d is JObject).First();
                        var errorCode = srcArray.ToList().First().ToList()[0].ToString();
                        if (errorCode == "100000027")
                        {
                            errval = errorCode;
                            break;
                        }

                        foreach (JObject row in srcArray.Last())
                        {
                            cleanRow = new JObject();
                            foreach (JProperty column in row.Properties())
                            {
                                if (column.Value is JValue)
                                {
                                    if (column.Name == "value")
                                    {
                                        if (column.Value.ToString() == "Fail to get DB Credentials from SLD")
                                        {
                                            errval = "100000027";
                                        }
                                        else
                                        {
                                            cleanRow.Add(column.Name, column.Value);
                                            errval = Convert.ToString(column.Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lin.Key == "DocEntry")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "DocNum")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        else if (lin.Key == "Code")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "CardCode")
                        {
                            CardCode = lin.Value.ToString();
                        }
                        else if (lin.Key == "CardName")
                        {
                            CardName = lin.Value.ToString();
                        }
                        else if (lin.Key == "AbsEntry")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "DepositNumber")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        else if (lin.Key == "ReconNum")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        else if (lin.Key == "DocumentEntry")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "DocumentNumber")
                        {
                            DocNum = lin.Value.ToString();
                        }
                        else if (lin.Key == "ServiceCallID")
                        {
                            ServiceCallId = lin.Value.ToString();
                        }
                        else if (lin.Key == "AbsoluteEntry")
                        {
                            DocEntry = lin.Value.ToString();
                        }
                        else if (lin.Key == "Status")
                        {
                            status = lin.Value.ToString();
                        }
                        else if (lin.Key == "Message")
                        {
                            message = lin.Value.ToString();
                        }
                        if (strFun == "Login")
                        {
                            errval = "Company Connected";
                            break;
                        }
                        else if (strFun == "BPMasterCreation")
                        {
                            if (CardCode != "" && CardName != "")
                            {
                                errval = CardCode + "#" + CardName + "#" + "Customer created successfully";
                                break;
                            }
                        }
                        else if (strFun == "ServiceCalls")
                        {
                            if (ServiceCallId != "" && DocNum != "")
                            {
                                errval = ServiceCallId + "#" + DocNum + "#" + "Service Call created Sucessfully";
                                break;
                            }
                        }
                        else if (strFun == "Attachments2")
                        {
                            if (DocEntry != "")
                            {
                                errval = DocEntry + "#" + "Added attachments successfully";
                                break;
                            }
                        }
                        else if (strFun == "DownPayments")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "DownPayments created successfully";
                                break;
                            }
                        }
                        else if (strFun == "SalesInvoice")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "ARInvoice created successfully";
                                break;
                            }
                        }
                        else if (strFun == "SalesReturn")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "SalesReturn created successfully";
                                break;
                            }
                        }
                        else if (strFun == "Incoming")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Payment created successfully";
                                break;
                            }
                        }
                        else if (strFun == "SalesOrder")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "SalesOrder Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "SalesReturn")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "SalesReturn Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "GRPO")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "GRPO created successfully";
                                break;
                            }
                        }
                        else if (strFun == "StockTransferRequest")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Stocktransferrequest created successfully";
                                break;
                            }
                        }
                        else if (strFun == "Stocktransfer")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Stocktransfer created successfully";
                                break;
                            }
                        }
                        else if (strFun == "OutgoingPayment")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "OutgoingPayment created Sucessfully";
                                break;
                            }
                        }
                        else if (strFun == "Deposit")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Deposit created Sucessfully";
                                break;
                            }
                        }
                        else if (strFun == "InternalReconciliations")
                        {
                            errval = "Reconciliation(s) done successfully";
                            break;
                        }
                        else if (strFun == "InventoryCountings")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "InventoryCountings created Sucessfully";
                                break;
                            }
                        }
                        else if (strFun == "ODEF")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Defective Document Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "GIS_OSPN")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Shipping Note Document Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "DENOMINATION")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "Denomination Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "PurchaseReturns")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "PurchaseReturns Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "GIS_IDFC")
                        {
                            if (DocEntry != "" && DocNum != "")
                            {
                                errval = DocEntry + "#" + DocNum + "#" + "IDFC Indagration Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "U_GIS_ADVPAY")
                        {
                            if (DocEntry != "")
                            {
                                errval = DocEntry + "#" + "Advance Payment Created successfully";
                                break;
                            }
                        }
                        else if (strFun == "GSTN")
                        {
                            if (status == "False")
                            {
                                if (message != "")
                                {
                                    errval = message;
                                    break;
                                }
                            }
                            else
                            {
                                errval = "GSTN Is Valid";
                                break;
                            }
                        }
                    }
                }
                return errval;
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "JsonStringToDataTable");
                throw;
            }
        }
    }
}
