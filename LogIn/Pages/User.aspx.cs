using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Antlr.Runtime.Misc;
using Sap.Data.Hana;
using SAPbobsCOM;
using WebGrease.Activities;

namespace LogIn.Pages
{
    public partial class User : System.Web.UI.Page
    {
        public string Type = WebConfigurationManager.AppSettings["ServerType"];
        DbConnection db = new DbConnection();
        string strID = string.Empty;
        string strMode = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    strID = Request.QueryString["id"];
                    strMode = Request.QueryString["Mode"];
                    db.writeLog("User_Page_Load", "Loading User page. Mode=" + strMode + ", ID=" + strID, "Debug");
                    LoadDepartmentsToDropdown();
                    if (!string.IsNullOrEmpty(strID) && strMode == "Edit")
                    {
                        EditDetails(strID);
                    }

                    if (strMode == "Create")
                    {
                        btnSave.Visible = true;
                        btnClear.Visible = true;
                        update.Visible = false;
                        erase.Visible = false;
                        btnViewAttachments.Visible = false;
                        Invalidselect.Visible = false;
                    }
                    else if (strMode == "Edit")
                    {
                        btnSave.Visible = false;
                        btnClear.Visible = false;
                        update.Visible = true;
                        erase.Visible = true;
                        Invalidselect.Visible = false;

                        DataTable dt = db.ExecuteQueryForDataTable("Select * from \"TEC_OUSR\" where \"User_Mail_Id\"='" + strID + "'");
                        foreach (DataRow row in dt.Rows)
                        {
                            txtUserId.Text = row["User_Id"].ToString();
                            txtUserName.Text = row["User_Name"].ToString();
                            txtUserMail.Text = row["User_Mail_Id"].ToString();
                            ddlDepartment.SelectedValue = row["Department"].ToString();
                            txtMobileNo.Text = row["Mobile_No"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "User_Page_Load");
            }
        }

        private void LoadDepartmentsToDropdown()
        {
            try
            {
                db.writeLog("LoadDepartmentsToDropdown", "Loading department dropdown items.", "Debug");
                DataTable dt = db.ExecuteQueryForDataTable("CALL \"Get_Department\"");
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("-- Select Department --", "0"));
            }
            catch (Exception ex)
            {
                db.LogError(ex, "LoadDepartmentsToDropdown");
            }
        }

        [WebMethod]
        public static List<Department> GetAllDepartments()
        {
            try
            {
                DbConnection db = new DbConnection();
                db.writeLog("GetAllDepartments", "Fetching all departments via WebMethod.", "Debug");
                DataTable dt = db.ExecuteQueryForDataTable("CALL \"Get_Department\"");
                return dt.AsEnumerable().Select(r => new Department
                {
                    DepartmentCode = r["DepartmentCode"].ToString(),
                    DepartmentName = r["DepartmentName"].ToString()
                }).ToList();
            }
            catch (Exception ex)
            {
                DbConnection.StaticLogError(ex, "GetAllDepartments");
                return new List<Department>();
            }
        }

        [WebMethod]
        public static string AddDepartment(string deptName)
        {
            try
            {
                DbConnection db = new DbConnection();
                db.writeLog("AddDepartment", "Adding department via WebMethod: " + deptName, "Debug");
                string deptCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                string query = $"INSERT INTO \"Department\" (\"DepartmentCode\",\"DepartmentName\") VALUES ('{deptCode}','{deptName}')";
                db.ExecuteNonQuery(query);
                db.writeLog("AddDepartment", "Department added successfully: " + deptCode, "Debug");
                return "success";
            }
            catch (Exception ex)
            {
                DbConnection.StaticLogError(ex, "AddDepartment");
                return $"error: {ex.Message}";
            }
        }

        [WebMethod]
        public static string DeleteDepartment(string deptCode)
        {
            try
            {
                DbConnection db = new DbConnection();
                db.writeLog("DeleteDepartment", "Deleting department via WebMethod: " + deptCode, "Debug");
                string query = $"DELETE FROM \"Department\" WHERE \"DepartmentCode\"='{deptCode}'";
                db.ExecuteNonQuery(query);
                db.writeLog("DeleteDepartment", "Department deleted successfully: " + deptCode, "Debug");
                return "success";
            }
            catch (Exception ex)
            {
                DbConnection.StaticLogError(ex, "DeleteDepartment");
                return $"error: {ex.Message}";
            }
        }

        public class Department
        {
            public string DepartmentCode { get; set; }
            public string DepartmentName { get; set; }
        }

        private void EditDetails(string Code)
        {
            try
            {
                db.writeLog("EditDetails", "Loading edit details for User Code: " + Code, "Debug");
                DataTable dt;
                if (Type == "HANA")
                    dt = db.UserEditDetails(Code);
                else
                    dt = db.SQL_UserEditDetails(Code);

                if (dt.Rows.Count > 0)
                {
                    txtUserName.Text = dt.Rows[0]["User_Name"].ToString();
                    txtUserMail.Text = dt.Rows[0]["User_Mail_Id"].ToString();

                    txtPassword.Attributes["value"] = Decryptpass(dt.Rows[0]["Password"].ToString());
                    txtConfirmPassword.Attributes["value"] = Decryptpass(dt.Rows[0]["Confirm_Password"].ToString());
                    txtMobileNo.Text = dt.Rows[0]["Mobile_No"].ToString();
                    txtCount.Text = dt.Rows[0]["Level"].ToString();
                    string departmentValue = dt.Rows[0]["Department"].ToString();

                    if (ddlDepartment.Items.FindByValue(departmentValue) != null)
                    {
                        ddlDepartment.SelectedValue = departmentValue;
                    }
                    else if (ddlDepartment.Items.FindByText(departmentValue) != null)
                    {
                        ddlDepartment.SelectedItem.Text = departmentValue;
                    }

                    chkact.Checked = dt.Rows[0]["Active"].ToString() == "True";
                    string base64 = dt.Rows[0]["ProfileUpload"].ToString();
                    var fileName = dt.Rows[0]["FileName"].ToString();
                    ViewState["ProfileUpload"] = base64;
                    ViewState["FileName"] = fileName;

                    txtUserMail.Enabled = false;
                    db.writeLog("EditDetails", "User details loaded into form for: " + Code, "Debug");
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "EditDetails");
            }
        }

        static string Decryptpass(string encodedPassword)
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(encodedPassword);
                string decodedPassword = Encoding.UTF8.GetString(decodedBytes);
                return decodedPassword;
            }
            catch (Exception ex)
            {
                DbConnection.StaticLogError(ex, "Decryptpass");
                return string.Empty;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                db.writeLog("btnClear_Click", "Clearing user fields.", "Debug");
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                txtUserMail.Text = string.Empty;
                txtMobileNo.Text = string.Empty;
                ddlDepartment.ClearSelection();
                ddlDepartment.SelectedIndex = 0;

                chkact.Checked = false;

                Invaliduserlabel.Visible = false;
                Invalidmaillabel.Visible = false;
            }
            catch (Exception ex)
            {
                db.LogError(ex, "btnClear_Click");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    db.writeLog("btnSave_Click", "Saving user: " + txtUserName.Text + ", Mail: " + txtUserMail.Text, "Debug");
                    string FileName = string.Empty;
                    byte[] ProfileUpload = null;

                    if (fileUpload.HasFile)
                    {
                        FileName = fileUpload.FileName;
                        ProfileUpload = new byte[fileUpload.FileContent.Length];
                        fileUpload.FileContent.Read(ProfileUpload, 0, ProfileUpload.Length);
                        db.writeLog("btnSave_Click", "Profile image file: " + FileName + ", Size: " + ProfileUpload.Length + " bytes", "Debug");
                    }
                    if (ddlDepartment.SelectedIndex == 0 || string.IsNullOrWhiteSpace(ddlDepartment.SelectedValue))
                    {
                        db.writeLog("btnSave_Click", "Validation failed: Department not selected.", "Debug");
                        Invalidselect.Visible = true;
                        Invalidselect.Text = "Please select a Department.";
                        return;
                    }

                    bool active = chkact.Checked;

                    string query = "SELECT COUNT(*) FROM TEC_OUSR WHERE \"User_Name\" = '" + txtUserName.Text + "'";
                    string query1 = "SELECT COUNT(*) FROM TEC_OUSR WHERE \"User_Mail_Id\" = '" + txtUserMail.Text + "'";
                    string userCount;
                    if (Type == "HANA")
                        userCount = db.GetSingleValue(query);
                    else
                        userCount = db.SQL_GetSingleValue(query1);
                    string userCount1;
                    if (Type == "HANA")
                        userCount1 = db.GetSingleValue(query1);
                    else
                        userCount1 = db.SQL_GetSingleValue(query1);

                    if (Convert.ToInt32(userCount1) > 0)
                    {
                        db.writeLog("btnSave_Click", "Validation failed: Email already exists - " + txtUserMail.Text, "Debug");
                        Invalidmaillabel.Text = "E-mail already exists!";
                        Invalidmaillabel.Visible = true;
                    }
                    else if (Convert.ToInt32(userCount) > 0)
                    {
                        db.writeLog("btnSave_Click", "Validation failed: Username already exists - " + txtUserName.Text, "Debug");
                        Invaliduserlabel.Text = "UserName already exists.";
                        Invaliduserlabel.Visible = true;
                    }
                    else
                    {
                        string pass = Encryptpass(txtPassword.Text);
                        string conpass = Encryptpass(txtConfirmPassword.Text);
                        string fileupload = string.Empty;

                        byte[] fileData;
                        using (var binaryReader = new System.IO.BinaryReader(fileUpload.PostedFile.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(fileUpload.PostedFile.ContentLength);
                        }

                        fileupload = Convert.ToBase64String(fileData);

                        string QUERY = "INSERT INTO TEC_OUSR(\"User_Name\",\"Password\",\"Confirm_Password\",\"User_Mail_Id\",\"Mobile_No\",\"Active\",\"FileName\",\"ProfileUpload\",\"Department\",\"Level\") " +
                                       "VALUES('" + txtUserName.Text + "','" + pass + "','" + conpass + "','" + txtUserMail.Text + "','" + txtMobileNo.Text + "'," + active + ",'" + FileName + "','" + fileupload + "','" + ddlDepartment.SelectedItem.Text + "'," + txtCount.Text + ")";
                        if (Type == "HANA")
                            db.ExecuteNonQuery(QUERY);
                        else
                            db.SQL_ExecuteNonQuery(QUERY);
                        db.writeLog("btnSave_Click", "User created successfully: " + txtUserName.Text, "Debug");
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "btnSave_Click");
            }
        }

        private void ClearFields()
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtUserMail.Text = string.Empty;
            txtMobileNo.Text = string.Empty;
            ddlDepartment.ClearSelection();
            ddlDepartment.SelectedValue = "0";
            chkact.Checked = false;
        }

        private static string Encryptpass(string password)
        {
            try
            {
                string msg = "";
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                msg = Convert.ToBase64String(encode);
                return msg;
            }
            catch (Exception ex)
            {
                DbConnection.StaticLogError(ex, "Encryptpass");
                return string.Empty;
            }
        }

        protected void erase_Click(object sender, EventArgs e)
        {
            try
            {
                db.writeLog("erase_Click", "Navigating to Dashboard.aspx", "Debug");
                Response.Redirect("/Pages/Dashboard.aspx");
            }
            catch (Exception ex)
            {
                db.LogError(ex, "erase_Click");
            }
        }

        protected void btnViewAttachments_Click(object sender, EventArgs e)
        {
            try
            {
                string code = txtUserMail.Text;
                db.writeLog("btnViewAttachments_Click", "Viewing attachment for user: " + code, "Debug");
                DataTable dt;
                if (Type == "HANA")
                    dt = db.UserEditDetails(code);
                else
                    dt = db.SQL_UserEditDetails(code);
                if (dt.Rows.Count > 0)
                {
                    string fileName = dt.Rows[0]["FileName"].ToString();
                    string base64 = dt.Rows[0]["ProfileUpload"].ToString();

                    if (!string.IsNullOrEmpty(base64))
                    {
                        lblAttachment.Text = fileName;

                        string imageUrl = "data:image/png;base64," + base64;
                        imgAttachment.ImageUrl = imageUrl;
                        imgAttachment.Visible = true;
                        db.writeLog("btnViewAttachments_Click", "Attachment loaded: " + fileName, "Debug");
                    }
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "btnViewAttachments_Click");
            }
        }

        protected void ValidateFileType(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (fileUpload.HasFile)
                {
                    string fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".jfif" };

                    if (allowedExtensions.Contains(fileExtension))
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "ValidateFileType");
            }
        }

        protected void update_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    db.writeLog("update_Click", "Updating user: " + txtUserName.Text + ", Mail: " + txtUserMail.Text, "Debug");
                    string FileName;
                    byte[] ProfileUpload = null;

                    if (fileUpload.HasFile)
                    {
                        ProfileUpload = fileUpload.FileBytes;
                        FileName = fileUpload.FileName;
                    }
                    else
                    {
                        if (ViewState["ProfileUpload"] == null) FileName = ViewState["FileName"] == null ? null : ViewState["FileName"].ToString();
                        else
                        {
                            ProfileUpload = Convert.FromBase64String(ViewState["ProfileUpload"].ToString());
                            FileName = ViewState["FileName"].ToString();
                        }
                    }

                    string existingUsername;
                    if (Type == "HANA")
                        existingUsername = db.GetSingleValue("SELECT \"User_Name\" FROM TEC_OUSR WHERE \"User_Mail_Id\" = '" + txtUserMail.Text + "'");
                    else
                        existingUsername = db.SQL_GetSingleValue("SELECT \"User_Name\" FROM TEC_OUSR WHERE \"User_Mail_Id\" = '" + txtUserMail.Text + "'");
                    if (txtUserName.Text == existingUsername)
                    {
                        UpdateUserDetails(FileName, ProfileUpload);
                    }
                    else
                    {
                        string query = "SELECT COUNT(*) FROM TEC_OUSR WHERE \"User_Name\" = '" + txtUserName.Text + "'";
                        string userCount;
                        if (Type == "HANA")
                            userCount = db.GetSingleValue(query);
                        else
                            userCount = db.SQL_GetSingleValue(query);
                        if (Convert.ToInt32(userCount) > 0)
                        {
                            db.writeLog("update_Click", "Validation failed: Username already exists - " + txtUserName.Text, "Debug");
                            Invaliduserlabel.Text = "Username already exists.";
                            Invaliduserlabel.Visible = true;
                        }
                        else
                        {
                            UpdateUserDetails(FileName, ProfileUpload);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "update_Click");
            }
        }

        private void UpdateUserDetails(string FileName, byte[] ProfileUpload)
        {
            try
            {
                bool active = chkact.Checked;
                string pass = Encryptpass(txtPassword.Text);
                string conpass = Encryptpass(txtConfirmPassword.Text);
                string file = null;
                if (ProfileUpload != null)
                    file = Convert.ToBase64String(ProfileUpload);
                string QUERY = "CALL \"TEC_UPDATEUSER\"('" + txtUserName.Text + "','" + pass + "','" + conpass + "','" + txtMobileNo.Text + "', " + active + ", '" + FileName + "','" + file + "','" + (ddlDepartment.SelectedItem.Text.Trim() == "-- Select Department --" ? null : ddlDepartment.SelectedItem.Text.Trim() )+ "','" + (string.IsNullOrEmpty(txtCount.Text.Trim()) ? null : txtCount.Text.Trim()) + "','" + txtUserMail.Text + "')";
                if (Type == "HANA")
                    db.ExecuteNonQuery(QUERY);
                else db.SQL_ExecuteNonQuery(QUERY);
                db.writeLog("UpdateUserDetails", "User updated successfully: " + txtUserName.Text, "Debug");
                Response.Redirect("~/Pages/UserList.aspx");
            }
            catch (Exception ex)
            {
                db.LogError(ex, "UpdateUserDetails");
            }
        }
    }
}
