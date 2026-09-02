using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogIn
{
    public partial class SiteMaster : MasterPage
    {
        DbConnection db = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();
                sidebar.Visible = false;
                if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"] == true)
                {
                    sidebar.Visible = true;
                    string userRole = Session["username"]?.ToString() ?? "";
                    if (!userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase) && !userRole.Equals("Admin@gmail.com", StringComparison.OrdinalIgnoreCase))
                    {
                        liUser.Visible = false;
                        liUser1.Visible = false;
                        liApprover.Visible = false;
                    }               
                }
                else
                {
                    if (currentPage != "loginpage.aspx")
                    {
                        db.writeLog("SiteMaster_Page_Load", "Unauthorized access to " + currentPage + ". Redirecting to LoginPage.", "Debug");
                        Response.Redirect("~/Pages/LoginPage.aspx");
                    }
                    if (currentPage == "loginpage.aspx" || string.IsNullOrEmpty(currentPage))
                    {
                        sidebar.Visible = false;
                    }
                    else
                    {
                        sidebar.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "SiteMaster_Page_Load");
            }
        }
    }
}