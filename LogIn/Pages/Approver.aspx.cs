using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI.WebControls;

namespace LogIn.Pages
{
    public partial class Approver : System.Web.UI.Page
    {
        DbConnection conn = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] == null)
                {
                    conn.writeLog("Approver_Page_Load", "Session username is null. Redirecting to LoginPage.", "Debug");
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
                    LoadDepartments();
                    LoadApprovers();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "Approver_Page_Load");
            }
        }

        private void LoadDepartments()
        {
            try
            {
                conn.writeLog("LoadDepartments", "Loading department dropdown for approver.", "Debug");
                DataTable dt = conn.ExecuteQueryForDataTable("CALL \"Get_Department\"");
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("-- Select Department --", ""));
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "LoadDepartments");
                lblMessage.Text = "Error loading departments: " + ex.Message;
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
            }
        }

        private void LoadApprovers()
        {
            try
            {
                conn.writeLog("LoadApprovers", "Loading approver master list.", "Debug");
                string query = "SELECT * FROM \"ApproverMaster\" ORDER BY \"ID\" ASC";
                gvApprover.DataSource = conn.ExecuteQueryForDataTable(query);
                gvApprover.DataBind();
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "LoadApprovers");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlDepartment.SelectedValue))
                    throw new Exception("Please select a department.");
                DbConnection db = new DbConnection();
                string checkQuery = $"SELECT COUNT(*) FROM \"ApproverMaster\" WHERE \"ApproverDepartment\" = '{ddlDepartment.SelectedItem.Text}' and \"Level\"='{txtLevel.Text}'";
                int count = Convert.ToInt32(conn.GetSingleValue(checkQuery));
                if (count > 0)
                    throw new Exception("This department already exists in the Approver list.");
                string username = Session["username"].ToString();
                string query = "Call \"TEC_GetApprovalWaitingDetails\" ('" + username + "')";
                DataTable dt = conn.ExecuteQueryForDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("Kindly approve all waiting approvals before change the approver master");
                }
                string count1 = db.GetSingleValue("Select ifnull(max(\"ID\"),0)+1 from \"ApproverMaster\" ");
                Int64 ID = Convert.ToInt64(count1);
                string insertQuery = $"INSERT INTO \"ApproverMaster\" (\"ID\",\"ApproverDepartment\", \"DepartmentApproverCount\",\"Level\") VALUES ('{ID}','{ddlDepartment.SelectedItem.Text}', {txtCount.Text},{txtLevel.Text})";
                conn.ExecuteNonQuery(insertQuery);

                conn.writeLog("btnAdd_Click", "Approver added successfully: " + ddlDepartment.SelectedItem.Text + ", Level: " + txtLevel.Text, "Debug");
                lblMessage.Text = "Approver added successfully!";
                lblMessage.CssClass = "text-success";
                lblMessage.Visible = true;
                LoadApprovers();
                ClearFields();
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnAdd_Click");
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
            }
        }

        protected void gvApprover_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    conn.writeLog("gvApprover_RowCommand", "Editing Approver ID: " + id, "Debug");
                    string query = $"SELECT * FROM \"ApproverMaster\" WHERE \"ID\" = {id}";
                    DataTable dt = conn.ExecuteQueryForDataTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        hdnID.Value = dr["ID"].ToString();
                        ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(
                            ddlDepartment.Items.FindByText(dr["ApproverDepartment"].ToString()));
                        txtLevel.Text = dr["Level"].ToString();
                        txtCount.Text = dr["DepartmentApproverCount"].ToString();

                        btnAdd.Visible = false;
                        btnUpdate.Visible = true;
                    }
                }
                else
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    conn.writeLog("gvApprover_RowCommand", "Deleting Approver ID: " + id, "Debug");
                    string username = Session["username"].ToString();
                    string query1 = "Call \"TEC_GetApprovalWaitingDetails\" ('" + username + "')";
                    DataTable dt = conn.ExecuteQueryForDataTable(query1);

                    if (dt.Rows.Count > 0)
                    {
                        throw new Exception("Kindly approve all waiting approvals before change the approver master");
                    }
                    string query = $"Delete FROM \"ApproverMaster\" WHERE \"ID\" = {id}";
                    conn.ExecuteNonQuery(query);
                    LoadApprovers();
                    string script = "alert('Record deleted successfully');";
                    ClientScript.RegisterStartupScript(this.GetType(), "deleteSuccess", script, true);
                    conn.writeLog("gvApprover_RowCommand", "Approver deleted successfully ID: " + id, "Debug");
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "gvApprover_RowCommand");
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hdnID.Value))
                    throw new Exception("Invalid record ID.");
                string checkQuery = $"SELECT COUNT(*) FROM \"ApproverMaster\" WHERE \"ApproverDepartment\" = '{ddlDepartment.SelectedItem.Text}' AND \"ID\" <> {hdnID.Value} and \"Level\"={txtLevel.Text}";
                int count = Convert.ToInt32(conn.GetSingleValue(checkQuery));
                if (count > 0)
                    throw new Exception("This department already exists in another record.");
                string username = Session["username"].ToString();
                string query = "Call \"TEC_GetApprovalWaitingDetails\" ('" + username + "')";
                DataTable dt = conn.ExecuteQueryForDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    throw new Exception("Kindly approve all waiting approvals before change the approver master");
                }
                string updateQuery = $"UPDATE \"ApproverMaster\" SET \"ApproverDepartment\" = '{ddlDepartment.SelectedItem.Text}', \"DepartmentApproverCount\" = {txtCount.Text},\"Level\"={txtLevel.Text} WHERE \"ID\" = {hdnID.Value}";
                conn.ExecuteNonQuery(updateQuery);

                conn.writeLog("btnUpdate_Click", "Approver updated successfully: ID=" + hdnID.Value, "Debug");
                lblMessage.Text = "Approver updated successfully!";
                lblMessage.CssClass = "text-success";
                lblMessage.Visible = true;

                LoadApprovers();
                ClearFields();

                btnAdd.Visible = true;
                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnUpdate_Click");
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "text-danger";
                lblMessage.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                conn.writeLog("btnCancel_Click", "Approver edit cancelled.", "Debug");
                ClearFields();
                btnAdd.Visible = true;
                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnCancel_Click");
            }
        }

        private void ClearFields()
        {
            hdnID.Value = "";
            ddlDepartment.SelectedIndex = 0;
            txtCount.Text = "1";
        }
    }
}
