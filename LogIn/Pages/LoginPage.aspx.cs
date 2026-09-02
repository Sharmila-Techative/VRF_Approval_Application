using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LogIn.Pages
{
    public partial class LoginPage : System.Web.UI.Page
    {
        string Type = WebConfigurationManager.AppSettings["ServerType"];
        DbConnection db = new DbConnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["LoggedIn"] = false;
        }
      
        private int IsValidUser(string username, string password)
        {


            string sServer = ConfigurationManager.AppSettings["Server"];
            string sDBUser = DbConnection.DecryptFun(ConfigurationManager.AppSettings["DBUser"]);
            string sDBPwd = DbConnection.DecryptFun(ConfigurationManager.AppSettings["DBPwd"]);
            string sDBName = ConfigurationManager.AppSettings["DBName"];


            string HanaConstr = "DRIVER={HDBODBC};UID=" + sDBUser + "PWD=" + sDBPwd + "DATABASENAME=NDB;SERVERNODE=" + sServer + "CS=" + sDBName + ";";


            string query = "SELECT \"Password\" FROM TEC_OUSR WHERE  \"Active\"=true and (\"User_Name\" = '" + username + "' or \"User_Mail_Id\" ='" + username + "' )";
            string pass = "";
            if (Type == "HANA")
                pass = db.GetSingleValue(query);
            else
            {
                pass = db.SQL_GetSingleValue(query);
            }
            string pass1 = Decryptpass(pass);
            if (pass1 == password)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        static string Decryptpass(string encodedPassword)
        {
            byte[] decodedBytes = Convert.FromBase64String(encodedPassword);
            string decodedPassword = Encoding.UTF8.GetString(decodedBytes);
            return decodedPassword;
        }

        protected void LoginValidate_Click(object sender, EventArgs e)
        {
            Session["LoggedIn"] = true;

            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            Session["username"] = username;
            string query = "SELECT \"Active\" FROM TEC_OUSR WHERE  (\"User_Name\" = '" + username + "' or \"User_Mail_Id\" ='" + username + "' )";
            string pass = "";
            if (Type == "HANA")
                pass = db.GetSingleValue(query);
            else
            {
                pass = db.SQL_GetSingleValue(query);

            }
            if (pass == "False")
            {
                ErrorMessageLabel.Text = "❌ Inactive User.";
                ErrorMessageLabel.Visible = true;
                return;
            }
            else
            {
                if (IsValidUser(username, password) == 1)
                {
                    Response.Redirect("/Pages/Dashboard.aspx");
                }
                else
                {
                    ErrorMessageLabel.Text = "❌ Invalid Username or Password.";
                    ErrorMessageLabel.Visible = true;
                }
            }
        }


















        protected void ResetPass_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                ErrorMessageLabel.Text = "❌ Please enter the Username / E-mail.";
                ErrorMessageLabel.Visible = true;
                return;
            }

            bool isResetMode = ViewState["ResetMode"] != null && (bool)ViewState["ResetMode"];

            if (!isResetMode)
            {
                pnlLogin.Visible = false;
                LoginValidate.Visible = false;

                pnlResetPassword.Visible = true;
                btnUpdatePassword.Visible = true;
                ResetPass.Text = "CANCEL";
                ViewState["ResetMode"] = true;
                ErrorMessageLabel.Visible = false;
            }
            else
            {
                pnlLogin.Visible = true;

                pnlResetPassword.Visible = false;
                btnUpdatePassword.Visible = false;

                txtOldPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
                LoginValidate.Visible = true;
                ResetPass.Text = "RESET PASSWORD";
                ViewState["ResetMode"] = false;
                ErrorMessageLabel.Visible = false;
            }
        }
        private static string Encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }
        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (IsValidUser(username, oldPassword) != 1)
            {
                ErrorMessageLabel.Text = "❌ Old password is incorrect.";
                ErrorMessageLabel.Visible = true;
                return;
            }

            if (newPassword != confirmPassword)
            {
                ErrorMessageLabel.Text = "❌ New Password and Confirm Password do not match.";
                ErrorMessageLabel.Visible = true;
                return;
            }

            string encryptedPassword = Encryptpass(newPassword);
                
            string query =
                "UPDATE TEC_OUSR " +
                "SET \"Password\" = '" + encryptedPassword + "' ," +
                "\"Confirm_Password\" = '" + encryptedPassword + "' " +
                "WHERE (\"User_Name\" = '" + username +
                "' OR \"User_Mail_Id\" = '" + username + "')";

            if (Type == "HANA")
            {
                db.ExecuteNonQuery(query);
            }
            else
            {
                db.SQL_ExecuteNonQuery(query);
            }

            ErrorMessageLabel.Text = "✅ Password updated successfully.";
            ErrorMessageLabel.Visible = true;

            pnlLogin.Visible = true;
            LoginValidate.Visible = true;

            pnlResetPassword.Visible = false;
            btnUpdatePassword.Visible = false;

            ResetPass.Text = "Reset Password";
        }
    }
}