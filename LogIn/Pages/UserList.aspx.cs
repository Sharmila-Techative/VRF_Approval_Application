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
    public partial class UserList : System.Web.UI.Page
    {
        public string Type = WebConfigurationManager.AppSettings["ServerType"];
        DbConnection db = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                db.writeLog("UserList_Page_Load", "UserList page loaded. IsPostBack: " + IsPostBack, "Debug");
                if (!IsPostBack)
                {
                    BindUserDetails();
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "UserList_Page_Load");
            }
        }

        private void BindUserDetails()
        {
            try
            {
                db.writeLog("BindUserDetails", "Fetching user details list.", "Debug");
                DataTable dt = new DataTable();
                if (Type == "HANA")
                    dt = db.UserDetailsForDataTable();
                else
                    dt = db.SQL_UserDetailsForDataTable();

                gvUserDetails.DataSource = dt;
                gvUserDetails.DataBind();
                db.writeLog("BindUserDetails", "Users bound successfully. Count: " + dt.Rows.Count, "Debug");
            }
            catch (Exception ex)
            {
                db.LogError(ex, "BindUserDetails");
            }
        }

        protected void createnew_Click(object sender, EventArgs e)
        {
            try
            {
                db.writeLog("createnew_Click", "Redirecting to User.aspx?Mode=Create", "Debug");
                Response.Redirect("/Pages/User.aspx?Mode=Create", false);
            }
            catch (Exception ex)
            {
                db.LogError(ex, "createnew_Click");
            }
        }

        protected void edit_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;
                string User_Mail_Id = gvUserDetails.Rows[index].Cells[1].Text;
                db.writeLog("edit_Click", "Navigating to edit user: " + User_Mail_Id, "Debug");
                Response.Redirect("/Pages/User.aspx?id=" + User_Mail_Id + "&Mode=Edit", false);
            }
            catch (Exception ex)
            {
                db.LogError(ex, "edit_Click");
            }
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton button = (LinkButton)sender;
                string User_Mail_Id = button.CommandArgument;
                string procudureName = "TEC_DeleteUser";
                db.writeLog("delete_Click", "Deleting user: " + User_Mail_Id + " via procedure " + procudureName, "Debug");
                if (Type == "HANA")
                    db.DeleteByCode(procudureName, User_Mail_Id);
                else
                    db.SQL_DeleteByCode(procudureName, User_Mail_Id);
                BindUserDetails();
                db.writeLog("delete_Click", "User deleted successfully: " + User_Mail_Id, "Debug");
            }
            catch (Exception ex)
            {
                db.LogError(ex, "delete_Click");
            }
        }
    }
}