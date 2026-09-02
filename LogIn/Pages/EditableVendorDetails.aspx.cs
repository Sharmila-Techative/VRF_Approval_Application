using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogIn.Pages
{
    public partial class EditableVendorDetails : System.Web.UI.Page
    {
        public string Type = WebConfigurationManager.AppSettings["ServerType"];
        DbConnection conn = new DbConnection();
        DbConnection dBConnection = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] == null)
                {
                    conn.writeLog("EditableVendorDetails_Page_Load", "Session username is null. Redirecting to LoginPage.", "Debug");
                    Session.Clear();
                    Session.Abandon();

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                    Response.Cache.SetNoStore();

                    Response.Redirect("~/Pages/LoginPage.aspx", false);
                    return;
                }
                if (!IsPostBack)
                {
                    if (Session["username"] != null)
                    {
                        lblUserName.Text = Session["username"].ToString();
                    }
                    RejectedDetails();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "EditableVendorDetails_Page_Load");
            }
        }

        private void RejectedDetails()
        {
            try
            {
                string username = Session["username"].ToString();
                conn.writeLog("RejectedDetails", "Loading rejected vendor details for user: " + username, "Debug");
                DataTable dt = null;
                string query = string.Empty;
                if (Type == "HANA")
                    query = "Call \"TEC_GetEdit_RejectedDetails\" ('" + username + "')";
                else
                    query = "Exec \"TEC_GetEdit_RejectedDetails\" '" + username + "'";

                if (Type == "HANA")
                    dt = conn.ExecuteQueryForDataTable(query);
                else
                    dt = conn.SQL_ExecuteQueryForDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    GridView4.DataSource = dt;
                    GridView4.DataBind();
                    conn.writeLog("RejectedDetails", "Rejected details bound successfully. Count: " + dt.Rows.Count, "Debug");
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "RejectedDetails");
            }
        }

        protected void gvUserDetails_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Approve" || e.CommandName == "Reject" || e.CommandName == "View")
                {
                    int rowIndex;
                    int.TryParse(e.CommandArgument.ToString(), out rowIndex);

                    string gstNo = GridView4.DataKeys[rowIndex].Value.ToString();
                    Session["GSTNo"] = gstNo;
                    Session["GSTNumber"] = gstNo;
                    conn.writeLog("gvUserDetails_RowCommand1", "Command: " + e.CommandName + ", GST: " + gstNo, "Debug");

                    if (e.CommandName == "Approve")
                    {
                    }
                    else if (e.CommandName == "Reject")
                    {
                    }
                    else if (e.CommandName == "View")
                    {
                        Response.Redirect($"/Pages/EditDetails.aspx");
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
    }
}