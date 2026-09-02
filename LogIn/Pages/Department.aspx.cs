using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace LogIn.Pages
{
    public partial class Department : System.Web.UI.Page
    {
        DbConnection db = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] == null)
                {
                    db.writeLog("Department_Page_Load", "Session expired or empty. Redirecting to LoginPage.", "Debug");
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
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "Department_Page_Load");
            }
        }

        private void LoadDepartments()
        {
            try
            {
                db.writeLog("LoadDepartments", "Loading departments list.", "Debug");
                string query = "SELECT \"DepartmentID\", \"DepartmentName\", \"IsActive\" FROM \"Department\" ORDER BY \"DepartmentName\"";
                DataTable dt = db.ExecuteQueryForDataTable(query);
                gvDepartments.DataSource = dt;
                gvDepartments.DataBind();
                db.writeLog("LoadDepartments", "Departments loaded successfully. Count: " + dt.Rows.Count, "Debug");
            }
            catch (Exception ex)
            {
                db.LogError(ex, "LoadDepartments");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string deptID = txtDeptID.Text.Trim();
                    string deptName = txtDeptName.Text.Trim();
                    bool isActive = chkIsActive.Checked ? true : false;

                    db.writeLog("btnAdd_Click", "Adding Department: ID=" + deptID + ", Name=" + deptName + ", IsActive=" + isActive, "Debug");
                    string query = "INSERT INTO \"Department\" (\"DepartmentID\", \"DepartmentName\", \"IsActive\") " +
                                   "VALUES ('" + deptID + "', '" + deptName + "', " + (isActive ? "TRUE" : "FALSE") + ")";

                    db.ExecuteNonQuery(query);
                    LoadDepartments();
                    ClearFields();
                    db.writeLog("btnAdd_Click", "Department added successfully: " + deptID, "Debug");
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "btnAdd_Click");
            }
        }

        protected void gvDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string deptID = gvDepartments.DataKeys[e.RowIndex].Value.ToString();
                db.writeLog("gvDepartments_RowDeleting", "Deleting Department: " + deptID, "Debug");

                string query = "DELETE FROM \"Department\" WHERE \"DepartmentID\" = '" + deptID + "'";
                db.ExecuteNonQuery(query);

                LoadDepartments();
                db.writeLog("gvDepartments_RowDeleting", "Department deleted successfully: " + deptID, "Debug");
            }
            catch (Exception ex)
            {
                db.LogError(ex, "gvDepartments_RowDeleting");
            }
        }

        protected void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chk.NamingContainer;
                string deptID = gvDepartments.DataKeys[row.RowIndex].Value.ToString();
                bool newStatus = chk.Checked;

                db.writeLog("chkActive_CheckedChanged", "Updating Department status: " + deptID + " to " + newStatus, "Debug");
                string query = "UPDATE \"Department\" SET \"IsActive\" = " + (newStatus ? "TRUE" : "FALSE") +
                               " WHERE \"DepartmentID\" = '" + deptID + "'";
                db.ExecuteNonQuery(query);

                LoadDepartments();
                db.writeLog("chkActive_CheckedChanged", "Department status updated successfully: " + deptID, "Debug");
            }
            catch (Exception ex)
            {
                db.LogError(ex, "chkActive_CheckedChanged");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                db.writeLog("btnClear_Click", "Clearing department input fields.", "Debug");
                ClearFields();
            }
            catch (Exception ex)
            {
                db.LogError(ex, "btnClear_Click");
            }
        }

        private void ClearFields()
        {
            txtDeptID.Text = "";
            txtDeptName.Text = "";
            chkIsActive.Checked = true;
        }
    }
}
