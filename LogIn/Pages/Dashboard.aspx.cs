using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace LogIn.Pages
{
    public partial class Dashboard : System.Web.UI.Page
    {
        public string Type = WebConfigurationManager.AppSettings["ServerType"];
        DbConnection db = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                db.writeLog("Dashboard_Page_Load", "Dashboard loaded. IsPostBack: " + IsPostBack, "Debug");
                if (!IsPostBack)
                {
                    LoadUserProfile();

                    if (Session["username"] != null)
                    {
                        lblUserName.Text = Session["username"].ToString();
                        db.writeLog("Dashboard_Page_Load", "Logged in user: " + Session["username"].ToString(), "Debug");
                    }
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "Dashboard_Page_Load");
            }
        }

        public string LoadUserProfile()
        {
            try
            {
                if (Session["username"] == null)
                {
                    db.writeLog("LoadUserProfile", "Session username is null.", "Debug");
                    return null;
                }

                string username = Session["username"].ToString();
                db.writeLog("LoadUserProfile", "Loading profile for user: " + username, "Debug");
                string query = "SELECT \"ProfileUpload\" FROM TEC_OUSR WHERE \"User_Name\" = '" + username + "'";
                string result;
                if (Type == "HANA")
                    result = db.GetSingleValue(query);
                else
                    result = db.SQL_GetSingleValue(query);

                if (string.IsNullOrEmpty(result))
                {
                    db.writeLog("LoadUserProfile", "No profile upload image found for user: " + username, "Debug");
                    imgProfile.Visible = false;
                    return null;
                }
                else
                {
                    db.writeLog("LoadUserProfile", "Profile image loaded successfully for user: " + username, "Debug");
                    string imageUrl = "data:image/png;base64," + result;
                    imgProfile.ImageUrl = imageUrl;
                    imgProfile.Visible = true;
                    return imageUrl;
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "LoadUserProfile");
                return null;
            }
        }
    }
}
