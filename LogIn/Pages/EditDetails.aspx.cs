using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;
namespace LogIn.Pages

{
    using System.Web.UI.WebControls;
    using System.Collections.Generic;
    using System.IO;
    using Sap.Data.Hana;
    using Newtonsoft.Json;
    using RestSharp;
    using System.Text;
    using System.Net.Mail;
    using System.Xml.Linq;
    using System.Web;
    using SAPbobsCOM;
    using static LogIn.Pages.View;
    using static System.Net.WebRequestMethods;
    using System.Drawing.Printing;
    using WebGrease.Css.Ast;
    using File = System.IO.File;
    using LogIn.Model;
    using System.Drawing;

    public partial class EditDetails : System.Web.UI.Page
    {
        DbConnection dBConnection = new DbConnection();


        protected void Page_Load(object sender, EventArgs e)
        {
            try { dBConnection.writeLog("EditDetails_Page_Load", "EditDetails page loaded. IsPostBack: " + IsPostBack, "Debug"); } catch {}


            if (IsPostBack)
            {
                string target = Request["__EVENTTARGET"];
                string arg = Request["__EVENTARGUMENT"];
                if (!string.IsNullOrEmpty(target) && target.Contains("fileUpload1"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowLoader", "document.getElementById('loader').style.display = 'flex';", true);

                    foreach (GridViewRow row in gvKYCDocuments.Rows)
                    {
                        var fu = (FileUpload)row.FindControl("fileUpload1");
                        var lbl = (Label)row.FindControl("DocumentName");

                        string documentType = "";
                        if (row.RowIndex == 0) documentType = "PAN Card";
                        if (row.RowIndex == 1) documentType = "GST Certificate";
                        if (row.RowIndex == 2) documentType = "Bank Account";
                        if (row.RowIndex == 3) documentType = "MSME Certificate";
                        string sessionPathKey = "Path_" + documentType;
                        string sessionBase64Key = "base64_" + documentType;
                        string sessionFileNameKey = "FileName_" + documentType;
                        if (lbl.ForeColor != System.Drawing.Color.Green)
                        {
                            if (fu != null && fu.HasFile)
                            {
                                string folderPath = Server.MapPath("~/TempFiles/");
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }

                                string fileName = fu.FileName;
                                string tempPath = Path.Combine(folderPath, "temp_" + Guid.NewGuid().ToString("N") + Path.GetExtension(fileName));
                                fu.SaveAs(tempPath);

                                byte[] fileBytes = File.ReadAllBytes(tempPath);
                                string base64File = Convert.ToBase64String(fileBytes);

                                Session[sessionPathKey] = tempPath;
                                Session[sessionBase64Key] = base64File;
                                Session[sessionFileNameKey] = fileName;

                                if (lbl != null)
                                {
                                    lbl.Text = fileName;
                                    lbl.ForeColor = System.Drawing.Color.Green;
                                    if (documentType != "Performa Invoice")
                                    {
                                        if (!string.IsNullOrEmpty(arg))
                                        {
                                            HandleFileSelection(arg);
                                        }
                                    }
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "HideLoader", "document.getElementById('loader').style.display = 'none';", true);
                                    return;
                                }
                            }

                            else if (Session[sessionPathKey] != null)
                            {
                                if (lbl != null)
                                {
                                    if (string.IsNullOrEmpty(lbl.Text))
                                    {
                                        lbl.Text = Session[sessionFileNameKey]?.ToString()
                                                   ?? Path.GetFileName(Session[sessionPathKey].ToString());
                                        lbl.ForeColor = System.Drawing.Color.Green;
                                    }

                                    if (!string.IsNullOrEmpty(arg))
                                    {
                                        HandleFileSelection(arg);
                                    }

                                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                        "HideLoader", "document.getElementById('loader').style.display = 'none';", true);
                                }
                            }

                        }

                    }



                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        var fu = (FileUpload)row.FindControl("fileUpload1");
                        var lbl = (Label)row.FindControl("DocumentName");

                        string documentType = "Performa Invoice";
                        string rowKeySuffix = "_" + row.RowIndex;

                        string sessionPathKey = "Path_" + documentType + rowKeySuffix;
                        string sessionBase64Key = "base64_" + documentType + rowKeySuffix;
                        string sessionFileNameKey = "FileName_" + documentType + rowKeySuffix;

                        if (lbl.ForeColor != System.Drawing.Color.Green)
                        {
                            if (fu != null && fu.HasFile)
                            {
                                string folderPath = Server.MapPath("~/TempFiles/");
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }

                                string fileName = fu.FileName;
                                string tempPath = Path.Combine(folderPath, "temp_" + Guid.NewGuid().ToString("N") + Path.GetExtension(fileName));
                                fu.SaveAs(tempPath);

                                byte[] fileBytes = File.ReadAllBytes(tempPath);
                                string base64File = Convert.ToBase64String(fileBytes);

                                Session[sessionPathKey] = tempPath;
                                Session[sessionBase64Key] = base64File;
                                Session[sessionFileNameKey] = fileName;

                                lbl.Text = fileName;
                                lbl.ForeColor = System.Drawing.Color.Green;
                            }
                            else if (Session[sessionPathKey] != null)
                            {
                                lbl.Text = Session[sessionFileNameKey]?.ToString()
                                           ?? Path.GetFileName(Session[sessionPathKey].ToString());
                                lbl.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }

                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "HideLoader",
                        "document.getElementById('loader').style.display = 'none';",
                        true
                    );

                }
            }
            if (!IsPostBack)
            {
                GSTNumber.Text = Session["GSTNumber"]?.ToString() ?? "";
                ClientScript.RegisterStartupScript(this.GetType(), "RestoreScroll", "restoreScrollPosition();", true);
                ShowPage(1);

                BindPartners();
                BindOperationalContacts();

                BindOtherInformation();

                BindKYCGrid1();
                BindKYCGrid11();
                LoadContactPersonDropdown();

                ViewState["BusinessDetails"] = null;

                var businessDetails = new List<BusinessDetails>
        {
            new BusinessDetails { BusinessState = "", GSTNumber = "", AddressOfPlace = "", GSTVendorClassification = "" }
        };

                var partnerDetails = new List<PartnerDetails>
        {
            new PartnerDetails { Name = "", Designation = "", Contact_No = "", Email_ID = "" }
        };

                var majorGoodsService = new List<MajorGoodsService>
        {
            new MajorGoodsService { Product =  "",Brand = "",Size = "",MaterialDescription = "", HSNCode = "", TaxPercentage = "" }

        };
                var MajorCustomers = new List<MajorCustomers>
        {
            new MajorCustomers {  CustomerName= "" }

        };
                ViewState["BusinessDetails"] = businessDetails;
                ViewState["PartnerDetails"] = partnerDetails;
                ViewState["MajorGoodsService"] = majorGoodsService;
                ViewState["MajorCustomer"] = MajorCustomers;

                BindGridViews();

                if (ViewState["BusinessDetails"] == null)
                {
                    ViewState["BusinessDetails"] = new List<BusinessDetails>();
                }

                LoadStates();
                LoadBanks();
                LoadCountries();
                refreshKYC();
                refreshKYC1();
                if (GSTNumber.Text != "") GSTNumber_TextChanged(sender, e);
                if (ViewState["DocumentDetails"] == null)
                {
                    InitializeGrid();
                }

            }
            else
            {
            }
        }

        public void refreshKYC()
        {
            foreach (GridViewRow row in gvKYCDocuments.Rows)
            {
                string documentType = "";
                if (row.RowIndex == 0) documentType = "PAN Card";
                if (row.RowIndex == 1) documentType = "GST Certificate";
                if (row.RowIndex == 2) documentType = "Bank Account";
                if (row.RowIndex == 3) documentType = "MSME Certificate";
                string sessionPathKey = "Path_" + documentType;
                string sessionBase64Key = "base64_" + documentType;
                string sessionFileNameKey = "FileName_" + documentType;
                Session[sessionPathKey] = null;
                Session[sessionBase64Key] = null;
                Session[sessionFileNameKey] = null;
            }
        }
        public void refreshKYC1()
        {
            foreach (GridViewRow row in gvKYCDocuments.Rows)
            {
                string documentType = "";
                documentType = "Performa Invoice";
                string sessionPathKey = "Path_" + documentType;
                string sessionBase64Key = "base64_" + documentType;
                string sessionFileNameKey = "FileName_" + documentType;
                Session[sessionPathKey] = null;
                Session[sessionBase64Key] = null;
                Session[sessionFileNameKey] = null;
            }
        }
        private void ShowPopup(string message, bool isSuccess)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup",
                "showPopup();", true);
        }
        protected void SendOTPBtn_Click(object sender, EventArgs e)
        {
            try { dBConnection.writeLog("SendOTPBtn_Click", "Send OTP clicked for mobile: " + MobileNo1.Text, "Debug"); } catch {}
            if (string.IsNullOrEmpty(MobileNo1.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Please Enter Mobile Number before proceeding.');", true);
            }
            else
            {
                string mobno = MobileNo1.Text;
                if (mobno.Length > 10 || mobno.Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Please Enter valide Mobile Number before proceeding.');", true);
                    return;
                }
                var otp = GenerateOTP();
                string gstNumber = GSTNumber.Text.Trim();


                Session["GeneratedOTP"] = otp;
                DateTime creation = DateTime.Now;
                string format = "yyyy-MM-dd HH:mm:ss";
                string formattedCreation = creation.ToString(format);
                DateTime validuntil = DateTime.Now.AddMinutes(15);
                string formattedvaliduntil = validuntil.ToString(format);


                string query = "INSERT INTO " + "\"TEC_BPRegistrationOTP\"" + "(\"gstNumber\", \"Mobileno\", \"OTPMobileno\",\"OTP\", \"Creation\", \"ValidUntil\", \"Verified\", \"ValidateOTP\") " +
                    "VALUES ('" + gstNumber + "', '" + mobno + "','" + mobno + "','" + otp + "',  '" + formattedCreation + "','" + formattedvaliduntil + "', 0, '" + otp + "')";
                DbConnection db = new DbConnection();
                db.ExecuteNonQuery(query);

                SendSMS(mobno, otp);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showOtpModal", "var otpModal = new bootstrap.Modal(document.getElementById('otpModal')); otpModal.show();", true);

            }
        }
        public static string GenerateOTP()
        {
            string otp = string.Empty;
            try
            {
                string numbers = "123456789";
                string characters = numbers;
                int length = 6;

                for (int i = 0; i <= length - 1; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                        string ss = otp.IndexOf(character).ToString();
                    }
                    while (otp.IndexOf(character) != -1);
                    otp += character;
                }
                return otp;
            }
            catch (Exception ex)
            { return ""; }
            finally
            { GC.Collect(); }
        }
        private void SendSMS(string mobileNumber, string otp)
        {
            try
            {

                string OTPTemplateId = ConfigurationManager.AppSettings["RedeemTempleId"];
                string OTPApiKey = ConfigurationManager.AppSettings["OTPApiKey"];
                string OTPClientId = ConfigurationManager.AppSettings["OTPClientId"];
                string response = SendRedeemOTP(mobileNumber, otp, OTPTemplateId, OTPApiKey, OTPClientId);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error sending SMS: {ex.Message}");
            }
        }









        public static string SendRedeemOTP(string mobileNumber, string otp, string templateId, string apiKey, string clientId)
        {
            string responseContent = string.Empty;
            try
            {
                string baseUrl = ConfigurationManager.AppSettings["RedeemOTPUrl"];

                var client = new RestClient(baseUrl);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.GET);

                request.AddParameter("SenderId", "NAIDUH");
                request.AddParameter("Message", $"Your OTP for completing VNH NAIDUHALL'S vendor registration form is {otp}. It is valid for 15 minutes. Thank you.");
                request.AddParameter("MobileNumbers", mobileNumber);
                request.AddParameter("TemplateId", templateId);
                request.AddParameter("ApiKey", apiKey);
                request.AddParameter("ClientId", clientId);

                IRestResponse response = client.Execute(request);
                responseContent = response.Content;

                dynamic value = JsonConvert.DeserializeObject(responseContent);
                responseContent = (value == null) ? null : value.ToString();

            }
            catch (WebException ex)
            {
                responseContent = ex.ToString();
            }

            return responseContent;
        }







        private void HandleFileSelection(string documentType)
        {
            foreach (GridViewRow row in gvKYCDocuments.Rows)
            {
                var fu = row.FindControl("fileUpload1") as FileUpload;
                var lblDocType = row.FindControl("DocumentName") as Label;

                if (fu != null && fu.HasFile)
                {
                    string fileName = Path.GetFileName(fu.FileName);
                    string savePath = Server.MapPath("~/Uploads/" + fileName);

                    fu.SaveAs(savePath);
                    APIPosting(documentType, savePath);

                }
            }
        }
        #region APIPosting
        public void APIPosting(string DocType, string FilePath)
        {
            try { dBConnection.writeLog("APIPosting", "APIPosting started for DocType: " + DocType + ", File: " + FilePath, "Debug"); } catch {}
            string AccountNumber = "";

            string AccountNameHolder = "";
            string IfscCode = "";
            string LegalName = "";
            string TradeName = "";
            string GstNumber = "";

            string Building = "";
            string Street = "";
            string Locality = "";
            string City = "";
            string District = "";
            string State = "";
            string Pincode = "";
            string sDocUrl = ConfigurationManager.AppSettings["DocUrl"];
            string sDocKey = ConfigurationManager.AppSettings["DocKey"];
            string DocNo = "1";
            if (DocType == "PAN Card") DocNo = "4";
            if (DocType == "GST Certificate") DocNo = "2";
            if (DocType == "Bank Account") DocNo = "1";
            if (DocType == "MSME Certificate") DocNo = "3";


            string str_Response = string.Empty;
            try
            {

                var client = new RestClient(sDocUrl + DocNo + "&key=" + sDocKey);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                request.AlwaysMultipartFormData = true;
                request.AddHeader("accept", "application/json");

                request.AddFile("file", FilePath);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var json = response.Content;

                    dynamic data = JsonConvert.DeserializeObject(json);
                    if (DocNo == "1")
                    {

                        if (data.ifsc_code != "" && data.ifsc_code != null)
                        {
                            AccountNumber = data.account_number;
                            AccountNameHolder = data.account_nameholder;
                            IfscCode = data.ifsc_code;
                            accountNumber.Text = AccountNumber;
                            ifscCode.Text = IfscCode;
                            accountName.Text = AccountNameHolder;
                            string bankCodeFromIfsc = new string(IfscCode
                .TakeWhile(c => !char.IsDigit(c))
                .ToArray());

                            bankName.ClearSelection();
                            System.Web.UI.WebControls.ListItem item = bankName.Items.FindByValue(bankCodeFromIfsc);
                            if (item != null)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    if (DocNo == "2")
                    {
                        GstDetails data1 = JsonConvert.DeserializeObject<GstDetails>(json);


                        if (data1.Legal_name != "" && data1.Legal_name != null)
                        {
                            LegalName = data1.Legal_name;
                            TradeName = data1.Trade_name;
                            tradeName.Text = TradeName;
                            GstNumber = data1.Gst_number;
                            GSTNumber.Text = GstNumber;
                            Building = data1.address_in_7_separate_feilds.Building;
                            registeredOfficeAddress1.Text = Building;
                            businessBillingAddress1.Text = Building;
                            Street = data1.address_in_7_separate_feilds.Street;
                            registeredOfficeAddress2.Text = Street;
                            businessBillingAddress2.Text = Street;
                            Locality = data1.address_in_7_separate_feilds.Locality;
                            registeredOfficeAddress3.Text = Locality;
                            businessBillingAddress3.Text = Locality;
                            City = data1.address_in_7_separate_feilds.City;
                            registeredOfficeCity.Text = City;
                            businessBillingCity.Text = City;
                            District = data1.address_in_7_separate_feilds.District;
                            State = data1.address_in_7_separate_feilds.State;
                            System.Web.UI.WebControls.ListItem stateItem = registeredOfficeState.Items
        .FindByText(State);

                            if (stateItem != null)
                            {
                                registeredOfficeState.ClearSelection();
                                stateItem.Selected = true;
                            }
                            System.Web.UI.WebControls.ListItem stateItem1 = businessBillingState.Items
       .FindByText(State);

                            if (stateItem1 != null)
                            {
                                businessBillingState.ClearSelection();
                                stateItem1.Selected = true;
                            }
                            Pincode = data1.address_in_7_separate_feilds.Pincode;
                            registeredOfficeZipCode.Text = Pincode;
                            businessBillingZipCode.Text = Pincode;
                        }
                    }
                    if (DocNo == "3")
                    {
                        UdyamDetails data3 = JsonConvert.DeserializeObject<UdyamDetails>(json);



                        if (data3.register_number != "" && data3.register_number != null)
                        {
                            string registerNumber = data3.register_number;
                            string enterpriseType = data3.enterprice_type;
                            ddlEnterpriseType.SelectedValue = enterpriseType;
                            MSMENO.Text = registerNumber;
                            string majorActivity = data3.major_activity;
                            natureOfBusinessActivity.Text = majorActivity;
                        }



                    }
                    if (DocNo == "4")
                    {
                        PanDetails data4 = JsonConvert.DeserializeObject<PanDetails>(json);
                        if (data4.pan_no != "" && data4.pan_no != null)
                        {
                            string panNo = data4.pan_no;
                            PANNumber.Text = panNo;
                            Session["PAN"] = panNo;

                        }

                    }


                }

            }
            catch (WebException ex)
            {
            }

        }
        #endregion

        private void ShowPage(int pageIndex)
        {
            pnlPage1.Visible = pageIndex == 1;
            pnlPage2.Visible = pageIndex == 2;
            pnlPage3.Visible = pageIndex == 3;
            pnlPage4.Visible = pageIndex == 4;
            pnlPage5.Visible = pageIndex == 5;
            pnlPage6.Visible = pageIndex == 6;
            pnlPage7.Visible = pageIndex == 7;

            btnPrevious.Visible = pageIndex > 1;
            btnNext.Visible = pageIndex < 6;

            hfPageIndex.Value = pageIndex.ToString();
        }








        private void LoadContactPersonDropdown()
        {
            try
            {
                string Sp = "CALL \"GetContactTypes\"";

                DataTable dt = dBConnection.ExecuteQueryForDataTable(Sp);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ContactPerson.DataSource = dt;
                    ContactPerson.DataTextField = "Name";
                    ContactPerson.DataValueField = "Name";
                    ContactPerson.DataBind();
                }
                else
                {
                    ContactPerson.Items.Clear();
                }

                ContactPerson.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Type--", ""));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Dropdown Load Error: " + ex.Message);
            }
        }
        protected void BusinessType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BusinessType1.SelectedValue == "Agency")
            {
                Agen1.Style["visibility"] = "visible";
                Agen2.Style["visibility"] = "visible";
                AgencyEmail.Style["visibility"] = "visible";
                AgencyName.Style["visibility"] = "visible";
            }
            else
            {
                Agen1.Style["visibility"] = "hidden";
                Agen2.Style["visibility"] = "hidden";
                AgencyEmail.Style["visibility"] = "hidden";
                AgencyName.Style["visibility"] = "hidden";
            }
        }
        private List<GoodItem> CollectGoodsFromGrid()
        {
            List<GoodItem> list = new List<GoodItem>();
            int i = 1;
            foreach (GridViewRow row in gvMajorGoods.Rows)
            {
                list.Add(new GoodItem
                {
                    SerialNo = i++,
                    Product = ((TextBox)row.FindControl("txtProduct")).Text,
                    Brand = ((TextBox)row.FindControl("txtBrand")).Text,
                    Size = ((TextBox)row.FindControl("txtSize")).Text,
                    MaterialDescription = ((TextBox)row.FindControl("txtMaterialDescription")).Text,
                    HSNCode = ((TextBox)row.FindControl("txtHSNCode")).Text,
                    TaxPercentage = ((TextBox)row.FindControl("txtTaxPercentage")).Text
                });
            }
            return list;
        }


        protected void btnPreview_Click(object sender, EventArgs e)
        {
            List<GoodItem> goods = new List<GoodItem>();
            int i = 1;
            foreach (GridViewRow row in gvMajorGoods.Rows)
            {
                goods.Add(new GoodItem
                {
                    SerialNo = i++,
                    Product = ((TextBox)row.FindControl("txtProduct")).Text,
                    Brand = ((TextBox)row.FindControl("txtBrand")).Text,
                    Size = ((TextBox)row.FindControl("txtSize")).Text,
                    MaterialDescription = ((TextBox)row.FindControl("txtMaterialDescription")).Text,
                    HSNCode = ((TextBox)row.FindControl("txtHSNCode")).Text,
                    TaxPercentage = ((TextBox)row.FindControl("txtTaxPercentage")).Text
                });
            }

            Session["MajorGoods"] = goods;

            Dictionary<string, object> data = new Dictionary<string, object>();

            data["GST Number"] = GSTNumber.Text;
            data["PAN Number"] = PANNumber.Text;
            data["Trade Name"] = tradeName.Text;
            data["Nature of Business"] = natureOfBusinessActivity.Text;
            data["Date of Establishment"] = dateOfEstablishment.Text;
            data["Contact Person"] = contactPersonName.Text;
            data["Designation"] = designation.Text;
            data["Email ID"] = emailId.Text;
            data["Mobile Number"] = mobileNo.Text;
            data["Office Telephone"] = officeTelephoneNo.Text;
            data["TAN Number"] = tanNo.Text;

            data["Registered Address"] = $"{registeredOfficeAddress1.Text}, {registeredOfficeAddress2.Text}, {registeredOfficeAddress3.Text}, {registeredOfficeCity.Text}, {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {registeredOfficeZipCode.Text}";
            if (sameAsRegisteredOffice.Checked)
                data["Billing Address"] = $"{businessBillingAddress1.Text}, {businessBillingAddress2.Text}, {businessBillingAddress3.Text}, {businessBillingCity.Text}, {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {businessBillingZipCode.Text}";
            else
                data["Billing Address"] = $"{businessBillingAddress1.Text}, {businessBillingAddress2.Text}, {businessBillingAddress3.Text}, {businessBillingCity.Text}, {businessBillingState.SelectedItem.Text}, {businessBillingCountry.SelectedItem.Text} - {businessBillingZipCode.Text}";
            if (sameAsRegisteredOffice1.Checked)
                data["Shipping Address"] = $"{shippingAddress1.Text}, {shippingAddress2.Text}, {shippingAddress3.Text}, {shippingCity.Text},  {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {shippingZipCode.Text}";
            else
                data["Shipping Address"] = $"{shippingAddress1.Text}, {shippingAddress2.Text}, {shippingAddress3.Text}, {shippingCity.Text}, {shippingState.SelectedItem.Text}, {shippingCountry.SelectedItem.Text} - {shippingZipCode.Text}";
            if (sameAsRegisteredOffice2.Checked)
                data["Goods Return Address"] = $"{goodsReturnAddress1.Text}, {goodsReturnAddress2.Text}, {goodsReturnAddress3.Text}, {goodsReturnCity.Text},  {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {goodsReturnZipcode.Text}";
            else
                data["Goods Return Address"] = $"{goodsReturnAddress1.Text}, {goodsReturnAddress2.Text}, {goodsReturnAddress3.Text}, {goodsReturnCity.Text}, {goodsReturnState.SelectedItem.Text}, {goodsReturnCountry.SelectedItem.Text} - {goodsReturnZipcode.Text}";

            data["Credit Days"] = CreditDays.Text;
            data["Discount"] = DisCount.Text;
            data["Bank Name"] = bankName.SelectedItem.Text;
            data["Account Name"] = accountName.Text;
            data["Account Number"] = accountNumber.Text;
            data["IFSC Code"] = ifscCode.Text;
            data["Branch Code"] = branchCode.Text;
            data["Bank Address"] = bankAddress.Text;

            data["MSME Status"] = msmeRegistrationStatus.SelectedItem.Text;
            data["MSME Number"] = MSMENO.Text;
            data["Enterprise Type"] = ddlEnterpriseType.SelectedItem.Text;

            data["Mark Down % on MRP (with Tax @0%)"] = Payment1.Text;
            data["Mark Down % on MRP (without Tax @0%)"] = Payment2.Text;
            data["Mark Down % on MRP (with Tax @3%)"] = Payment3.Text;
            data["Mark Down % on MRP (without Tax @3%)"] = Payment4.Text;
            data["Mark Down % on MRP (with Tax @5%)"] = Payment5.Text;
            data["Mark Down % on MRP (without Tax @5%)"] = Payment6.Text;
            data["Mark Down % on MRP (with Tax @18%)"] = Payment9.Text;
            data["Mark Down % on MRP (without Tax @18%)"] = Payment10.Text;

            data["Business Type"] = BusinessType1.SelectedItem.Text;
            data["Agency Email"] = AgencyEmail.Text;
            data["Agency Name"] = AgencyName.Text;

            AddGridToData(data, gvProjectDetails, "Business Location");
            AddGridToData(data, gvPartners, "Partners/Proprietor/Director's / Business Head Detail (Provide at Least One Person Details)");
            AddGridToData(data, gvOperationalContacts, "Primary Operational Contacts");
            AddGridToData(data, gvMajorGoods, "Major goods and services Details With");

            AddGridToData(data, gvMajorCustomers, "List of Major Customers");
            AddGridToData(data, gvOtherInformation, "Other Information");

            data["Name"] = declarationName.Text;
            data["Designation"] = declarationDesignation.Text;
            data["Mobile No"] = MobileNo1.Text;



            string jsonData = JsonConvert.SerializeObject(data);

            string safeJson = HttpUtility.JavaScriptStringEncode(jsonData);

            string script = $@"
        sessionStorage.setItem('PreviewData', '{safeJson}');
        window.open('VendorPreview.aspx', '_blank');
    ";

            ClientScript.RegisterStartupScript(this.GetType(), "OpenPreview", script, true);
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

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int page = int.Parse(hfPageIndex.Value);





            ShowPage(page + 1);

        }


        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            int i = 1;
            foreach (GridViewRow row in gvKYCDocuments.Rows)
            {
                string DocumentType = string.Empty;
                if (i == 1) DocumentType = "PAN Card";
                if (i == 2) DocumentType = "GST Certificate";
                if (i == 3) DocumentType = "Bank Account";
                if (i == 4) DocumentType = "MSME Certificate";
                if (row.RowType == DataControlRowType.DataRow)
                {
                    FileUpload fileUpload = row.FindControl("fileUpload1") as FileUpload;
                    Label lblDocName = row.FindControl("DocumentName") as Label;

                    if (fileUpload != null && fileUpload.HasFile)
                    {
                        string tempPath = Server.MapPath("~/TempFiles/");
                        if (!Directory.Exists(tempPath))
                            Directory.CreateDirectory(tempPath);

                        string fileName = Path.GetFileName(fileUpload.FileName);
                        string savedPath = Path.Combine(tempPath, fileName);
                        fileUpload.SaveAs(savedPath);

                        Session["SelectedFilePath"] = savedPath;
                        Session["SelectedFileName"] = fileName;

                        if (Session["SelectedFileName"] != null && lblDocName != null)
                        {
                            lblDocName.Text = Session["SelectedFileName"].ToString();
                            lblDocName.ForeColor = System.Drawing.Color.Green;
                        }

                        byte[] fileBytes = File.ReadAllBytes(savedPath);
                        string base64String = Convert.ToBase64String(fileBytes);
                        string sessionkey = "Path_" + DocumentType;
                        Session[sessionkey] = base64String;
                    }
                }
                i++;
            }

            foreach (GridViewRow row in GridView1.Rows)
            {
                string DocumentType = string.Empty;
                DocumentType = "Performa Invoice";
                if (row.RowType == DataControlRowType.DataRow)
                {
                    FileUpload fileUpload = row.FindControl("fileUpload1") as FileUpload;
                    Label lblDocName = row.FindControl("DocumentName") as Label;

                    if (fileUpload != null && fileUpload.HasFile)
                    {
                        string tempPath = Server.MapPath("~/TempFiles/");
                        if (!Directory.Exists(tempPath))
                            Directory.CreateDirectory(tempPath);

                        string fileName = Path.GetFileName(fileUpload.FileName);
                        string savedPath = Path.Combine(tempPath, fileName);
                        fileUpload.SaveAs(savedPath);

                        Session["SelectedFilePath"] = savedPath;
                        Session["SelectedFileName"] = fileName;

                        if (Session["SelectedFileName"] != null && lblDocName != null)
                        {
                            lblDocName.Text = Session["SelectedFileName"].ToString();
                            lblDocName.ForeColor = System.Drawing.Color.Green;
                        }

                        byte[] fileBytes = File.ReadAllBytes(savedPath);
                        string base64String = Convert.ToBase64String(fileBytes);
                        string sessionkey = "Path_" + DocumentType;
                        Session[sessionkey] = base64String;
                    }
                }
            }

            int page = int.Parse(hfPageIndex.Value);
            if (page - 1 == 0)
            {
                Response.Redirect("~/Pages/EditableVendorDetails.aspx");
            }
            else
            {
                ShowPage(page - 1);
            }
        }


        [Serializable]
        public class MajorCustomers
        {
            public string CustomerName { get; set; }

        }

        public class OperationalContact
        {
            public string Department { get; set; }
            public string Name { get; set; }
            public string Designation { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
        }
        [Serializable]
        public class MajorGoodsService
        {
            public int SI_No { get; set; }
            public string MaterialDescription { get; set; }
            public string HSNCode { get; set; }
            public string Brand { get; set; }
            public string Size { get; set; }
            public string Product { get; set; }
            public string TaxPercentage { get; set; }
        }
        private void BindKYCGrid()
        {
            DataTable dtKYC = new DataTable();
            dtKYC.Columns.Add("DocumentName");

            dtKYC.Rows.Add("PAN Card (***)");
            dtKYC.Rows.Add("GST Certificate (***)");
            dtKYC.Rows.Add("Bank Account");
            dtKYC.Rows.Add("MSME Certificate");
            gvKYCDocuments.DataSource = dtKYC;
            gvKYCDocuments.DataBind();
        }

        private void BindOtherInformation()
        {
            DataTable dtOtherInfo = new DataTable();
            dtOtherInfo.Columns.Add("Description");
            dtOtherInfo.Columns.Add("TextMode");

            dtOtherInfo.Rows.Add("Total Count of Employees / Labours", "Number");
            dtOtherInfo.Rows.Add("Area of Office / Factory", "Text");
            dtOtherInfo.Rows.Add("Max. Production Capacity", "Number");
            dtOtherInfo.Rows.Add("Yearly Turnover", "Number");

            gvOtherInformation.DataSource = dtOtherInfo;
            gvOtherInformation.DataBind();
        }
        private void BindMajorCustomers()
        {
            List<MajorCustomers> majorCustomers = new List<MajorCustomers>
            {
        new MajorCustomers { CustomerName=""},

    };

            gvMajorCustomers.DataSource = majorCustomers;
            gvMajorCustomers.DataBind();


        }
        private void BindMajorGoodsServices()
        {

            List<MajorGoodsService> majorGoodsService = ViewState["MajorGoodsService"] as List<MajorGoodsService> ?? new List<MajorGoodsService>();

            if (majorGoodsService.Count == 0)
            {
                majorGoodsService.Add(new MajorGoodsService { SI_No = 1, Product = "", Brand = "", Size = "", MaterialDescription = "", HSNCode = "", TaxPercentage = "" });
            }

            gvMajorGoods.DataSource = majorGoodsService;
            gvMajorGoods.DataBind();
        }

        private void BindOperationalContacts()
        {
            List<OperationalContact> contacts = new List<OperationalContact>
    {
        new OperationalContact { Department = "Sales", Name = "", Designation = "", ContactNo = "", Email = "" },
        new OperationalContact { Department = "Purchase", Name = "", Designation = "", ContactNo = "", Email = "" },
        new OperationalContact { Department = "Manager", Name = "", Designation = "", ContactNo = "", Email = "" },
        new OperationalContact { Department = "Accounts", Name = "", Designation = "", ContactNo = "", Email = "" },
        new OperationalContact { Department = "Business Head", Name = "", Designation = "", ContactNo = "", Email = "" }
    };

            gvOperationalContacts.DataSource = contacts;
            gvOperationalContacts.DataBind();
        }
        [Serializable]
        public class BusinessDetails
        {
            public string BusinessState { get; set; }
            public string GSTNumber { get; set; }
            public string AddressOfPlace { get; set; }
            public string GSTVendorClassification { get; set; }

        }
        private void BindbusinessDetails()
        {

            List<BusinessDetails> businessDetails = ViewState["BusinessDetails"] as List<BusinessDetails> ?? new List<BusinessDetails>();

            if (businessDetails.Count == 0)
            {
                businessDetails.Add(new BusinessDetails { BusinessState = "", GSTNumber = "", AddressOfPlace = "", GSTVendorClassification = "" });
            }

            gvProjectDetails.DataSource = businessDetails;
            gvProjectDetails.DataBind();
        }

        protected void BindPartners()
        {
            var partnerDetails = ViewState["PartnerDetails"] as List<PartnerDetails>;

            if (partnerDetails != null)
            {
                for (int i = 0; i < partnerDetails.Count; i++)
                {
                    partnerDetails[i].RowID = i;
                }

                gvPartners.DataSource = partnerDetails;
                gvPartners.DataBind();
            }
        }




        protected void gvProjectDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as BusinessDetails;
                DropDownList ddlBusinessState = (DropDownList)e.Row.FindControl("businessState");
                if (ddlBusinessState != null)
                {
                    LoadStates(ddlBusinessState);

                    if (dataItem != null && !string.IsNullOrEmpty(dataItem.BusinessState))
                    {
                        System.Web.UI.WebControls.ListItem item = ddlBusinessState.Items.FindByValue(dataItem.BusinessState);
                        if (item != null)
                        {
                            ddlBusinessState.SelectedValue = dataItem.BusinessState;
                        }
                    }
                }
                ((TextBox)e.Row.FindControl("gstNumber")).Text = dataItem?.GSTNumber ?? string.Empty;
                ((TextBox)e.Row.FindControl("addressOfPlace")).Text = dataItem?.AddressOfPlace ?? string.Empty;
                ((DropDownList)e.Row.FindControl("gstVendorClassification")).SelectedValue = dataItem?.GSTVendorClassification ?? string.Empty;

            }
        }
        private void LoadStates(DropDownList businessState)
        {


            String Query = "Call \"" + DbConnection.sDBName + "\".\"GetStates\"('" + registeredOfficeCountry.Text + "')";
            DataTable reader = dBConnection.ExecuteQueryForDataTable(Query);
            businessState.DataSource = reader;
            businessState.DataTextField = "StateName";
            businessState.DataValueField = "StateCode";
            businessState.DataBind();
            businessState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State", ""));
        }
        protected void gvPartners_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as PartnerDetails;
                ((TextBox)e.Row.FindControl("partnerName")).Text = dataItem?.Name ?? string.Empty;
                ((TextBox)e.Row.FindControl("partnerDesignation")).Text = dataItem?.Designation ?? string.Empty;
                ((TextBox)e.Row.FindControl("partnerContactNo")).Text = dataItem?.Contact_No ?? string.Empty;
                ((TextBox)e.Row.FindControl("partnerEmail")).Text = dataItem?.Email_ID ?? string.Empty;
            }
        }

        protected void gvMajorGoods_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as MajorGoodsService;
                ((TextBox)e.Row.FindControl("txtProduct")).Text = dataItem?.Product ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtBrand")).Text = dataItem?.Brand ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtSize")).Text = dataItem?.Size ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtMaterialDescription")).Text = dataItem?.MaterialDescription ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtHSNCode")).Text = dataItem?.HSNCode ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtTaxPercentage")).Text = dataItem?.TaxPercentage ?? string.Empty;
            }
        }

        protected void gvMajorCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as MajorCustomers;
                ((TextBox)e.Row.FindControl("customerName")).Text = dataItem?.CustomerName ?? string.Empty;
            }
        }




        protected void lnknewrowadd_Click(object sender, EventArgs e)
        {
            try
            {
                List<BusinessDetails> OBusinessDetails = (List<BusinessDetails>)ViewState["BusinessDetails"];

                if (OBusinessDetails == null)
                {
                    OBusinessDetails = new List<BusinessDetails>();
                }

                foreach (GridViewRow row in gvProjectDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string businessState = (row.FindControl("businessState") as DropDownList).SelectedValue;
                        string gstNumber = (row.FindControl("gstNumber") as TextBox).Text;
                        string addressOfPlace = (row.FindControl("addressOfPlace") as TextBox).Text;
                        string gstVendorClassification = (row.FindControl("gstVendorClassification") as DropDownList).SelectedValue;

                        int i = row.DataItemIndex;
                        if (i >= 0 && i < OBusinessDetails.Count)
                        {
                            OBusinessDetails[i].BusinessState = businessState;
                            OBusinessDetails[i].GSTNumber = gstNumber;
                            OBusinessDetails[i].AddressOfPlace = addressOfPlace;
                            OBusinessDetails[i].GSTVendorClassification = gstVendorClassification;
                        }
                    }
                }

                OBusinessDetails.Add(new BusinessDetails
                {
                    BusinessState = "",
                    GSTNumber = "",
                    AddressOfPlace = "",
                    GSTVendorClassification = ""
                });

                ViewState["BusinessDetails"] = OBusinessDetails;
                BindGrid();
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }
        }

        private void BindGrid()
        {
            List<BusinessDetails> OBusinessDetails = (List<BusinessDetails>)ViewState["BusinessDetails"];
            gvProjectDetails.DataSource = OBusinessDetails;
            gvProjectDetails.DataBind();

        }



        protected void lnknewrowadd_Click1(object sender, EventArgs e)
        {
            try
            {
                List<PartnerDetails> OPartnerDetails = (List<PartnerDetails>)ViewState["PartnerDetails"];
                int i = 0;
                foreach (GridViewRow row in gvPartners.Rows)
                {

                    string partnerName = (row.FindControl("partnerName") as System.Web.UI.WebControls.TextBox).Text;
                    string gstNumber = (row.FindControl("partnerDesignation") as System.Web.UI.WebControls.TextBox).Text;
                    string addressOfPlace = (row.FindControl("partnerContactNo") as System.Web.UI.WebControls.TextBox).Text;
                    string gstVendorClassification = (row.FindControl("partnerEmail") as System.Web.UI.WebControls.TextBox).Text;

                    OPartnerDetails[i].Name = partnerName;
                    OPartnerDetails[i].Designation = gstNumber;
                    OPartnerDetails[i].Contact_No = addressOfPlace;
                    OPartnerDetails[i].Email_ID = gstVendorClassification;

                    i++;
                }
                OPartnerDetails.Add(new PartnerDetails
                {
                    Name = "",
                    Designation = "",
                    Contact_No = "",
                    Email_ID = ""
                });
                ViewState["PartnerDetails"] = OPartnerDetails;
                gvPartners.DataSource = OPartnerDetails;
                gvPartners.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnknewrowadd_Click2(object sender, EventArgs e)
        {
            try
            {

                List<MajorGoodsService> OMajorGoodsService = (List<MajorGoodsService>)ViewState["MajorGoodsService"];
                int i = 0;
                foreach (GridViewRow row in gvMajorGoods.Rows)
                {
                    string product = (row.FindControl("txtProduct") as System.Web.UI.WebControls.TextBox).Text;
                    string brand = (row.FindControl("txtBrand") as System.Web.UI.WebControls.TextBox).Text;
                    string size = (row.FindControl("txtSize") as System.Web.UI.WebControls.TextBox).Text;
                    string materialDescription = (row.FindControl("txtMaterialDescription") as System.Web.UI.WebControls.TextBox).Text;
                    string hSNCode = (row.FindControl("txtHSNCode") as System.Web.UI.WebControls.TextBox).Text;

                    string taxPercentage = (row.FindControl("txtTaxPercentage") as System.Web.UI.WebControls.TextBox).Text;

                    OMajorGoodsService[i].Product = product;
                    OMajorGoodsService[i].Brand = brand;
                    OMajorGoodsService[i].Size = size;
                    OMajorGoodsService[i].MaterialDescription = materialDescription;
                    OMajorGoodsService[i].HSNCode = hSNCode;

                    OMajorGoodsService[i].TaxPercentage = taxPercentage;


                    i++;
                }
                OMajorGoodsService.Add(new MajorGoodsService
                {
                    Product = "",
                    Brand = "",
                    Size = "",
                    MaterialDescription = "",
                    HSNCode = "",
                    TaxPercentage = ""
                });

                ViewState["MajorGoodsService"] = OMajorGoodsService;
                gvMajorGoods.DataSource = OMajorGoodsService;
                gvMajorGoods.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnknewrowadd_Click3(object sender, EventArgs e)
        {
            try
            {
                List<MajorCustomers> OMajorCustomers = (List<MajorCustomers>)ViewState["MajorCustomer"];
                int i = 0;
                foreach (GridViewRow row in gvMajorCustomers.Rows)
                {

                    string customerName = (row.FindControl("customerName") as System.Web.UI.WebControls.TextBox).Text;


                    OMajorCustomers[i].CustomerName = customerName;


                    i++;
                }
                OMajorCustomers.Add(new MajorCustomers
                {
                    CustomerName = ""
                });

                ViewState["MajorCustomer"] = OMajorCustomers;
                gvMajorCustomers.DataSource = OMajorCustomers;
                gvMajorCustomers.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lnkDelete_Click3(object sender, EventArgs e)
        {
            try
            {
                List<MajorCustomers> OMajorCustomers = ViewState["MajorCustomer"] as List<MajorCustomers>;

                if (OMajorCustomers == null)
                    return;

                for (int i = 0; i < gvMajorCustomers.Rows.Count; i++)
                {
                    GridViewRow row = gvMajorCustomers.Rows[i];
                    string customerName = (row.FindControl("customerName") as TextBox)?.Text;

                    if (i < OMajorCustomers.Count)
                    {
                        OMajorCustomers[i].CustomerName = customerName;
                    }
                }

                LinkButton btn = (LinkButton)sender;
                GridViewRow currentRow = (GridViewRow)btn.NamingContainer;
                int indexToDelete = currentRow.RowIndex;

                if (OMajorCustomers.Count == 1)
                {
                    OMajorCustomers[0].CustomerName = string.Empty;
                }
                else
                {
                    if (indexToDelete >= 0 && indexToDelete < OMajorCustomers.Count)
                    {
                        OMajorCustomers.RemoveAt(indexToDelete);
                    }
                }

                ViewState["MajorCustomer"] = OMajorCustomers;
                gvMajorCustomers.DataSource = OMajorCustomers;
                gvMajorCustomers.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<BusinessDetails> OBusinessDetails = ViewState["BusinessDetails"] as List<BusinessDetails>;

                if (OBusinessDetails == null)
                    return;

                foreach (GridViewRow row in gvProjectDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string businessState = (row.FindControl("businessState") as DropDownList)?.SelectedValue;
                        string gstNumber = (row.FindControl("gstNumber") as TextBox)?.Text;
                        string addressOfPlace = (row.FindControl("addressOfPlace") as TextBox)?.Text;
                        string gstVendorClassification = (row.FindControl("gstVendorClassification") as DropDownList)?.SelectedValue;

                        int i = row.DataItemIndex;
                        if (i >= 0 && i < OBusinessDetails.Count)
                        {
                            OBusinessDetails[i].BusinessState = businessState;
                            OBusinessDetails[i].GSTNumber = gstNumber;
                            OBusinessDetails[i].AddressOfPlace = addressOfPlace;
                            OBusinessDetails[i].GSTVendorClassification = gstVendorClassification;
                        }
                    }
                }

                LinkButton btn = (LinkButton)sender;
                GridViewRow currentRow = (GridViewRow)btn.NamingContainer;
                int indexToDelete = currentRow.DataItemIndex;

                if (OBusinessDetails.Count == 1)
                {
                    OBusinessDetails[0].BusinessState = string.Empty;
                    OBusinessDetails[0].GSTNumber = string.Empty;
                    OBusinessDetails[0].AddressOfPlace = string.Empty;
                    OBusinessDetails[0].GSTVendorClassification = string.Empty;
                }
                else
                {
                    if (indexToDelete >= 0 && indexToDelete < OBusinessDetails.Count)
                    {
                        OBusinessDetails.RemoveAt(indexToDelete);
                    }
                }

                ViewState["BusinessDetails"] = OBusinessDetails;
                BindGrid();
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }
        }


        protected void lnkDelete_Click1(object sender, EventArgs e)
        {
            try
            {
                List<PartnerDetails> partnerDetails = ViewState["PartnerDetails"] as List<PartnerDetails>;
                if (partnerDetails == null) return;

                for (int i = 0; i < gvPartners.Rows.Count; i++)
                {
                    GridViewRow row = gvPartners.Rows[i];

                    string partnerName = (row.FindControl("partnerName") as TextBox)?.Text;
                    string designation = (row.FindControl("partnerDesignation") as TextBox)?.Text;
                    string contactNo = (row.FindControl("partnerContactNo") as TextBox)?.Text;
                    string email = (row.FindControl("partnerEmail") as TextBox)?.Text;

                    if (i < partnerDetails.Count)
                    {
                        partnerDetails[i].Name = partnerName;
                        partnerDetails[i].Designation = designation;
                        partnerDetails[i].Contact_No = contactNo;
                        partnerDetails[i].Email_ID = email;
                    }
                }

                LinkButton btn = (LinkButton)sender;
                GridViewRow currentRow = (GridViewRow)btn.NamingContainer;
                int rowIndex = currentRow.RowIndex;

                if (partnerDetails.Count == 1)
                {
                    partnerDetails[0].Name = string.Empty;
                    partnerDetails[0].Designation = string.Empty;
                    partnerDetails[0].Contact_No = string.Empty;
                    partnerDetails[0].Email_ID = string.Empty;
                }
                else
                {
                    if (rowIndex >= 0 && rowIndex < partnerDetails.Count)
                    {
                        partnerDetails.RemoveAt(rowIndex);
                    }
                }

                ViewState["PartnerDetails"] = partnerDetails;
                gvPartners.DataSource = partnerDetails;
                gvPartners.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void lnkDelete_Click2(object sender, EventArgs e)
        {
            try
            {
                List<MajorGoodsService> OMajorGoodsService = ViewState["MajorGoodsService"] as List<MajorGoodsService>;
                if (OMajorGoodsService == null) return;

                for (int i = 0; i < gvMajorGoods.Rows.Count; i++)
                {
                    GridViewRow row = gvMajorGoods.Rows[i];
                    string product = (row.FindControl("txtProduct") as TextBox)?.Text;
                    string brand = (row.FindControl("txtBrand") as TextBox)?.Text;
                    string size = (row.FindControl("txtSize") as TextBox)?.Text;
                    string materialDescription = (row.FindControl("txtMaterialDescription") as TextBox)?.Text;
                    string hSNCode = (row.FindControl("txtHSNCode") as TextBox)?.Text;
                    string taxPercentage = (row.FindControl("txtTaxPercentage") as TextBox)?.Text;

                    if (i < OMajorGoodsService.Count)
                    {
                        OMajorGoodsService[i].Product = product;
                        OMajorGoodsService[i].Brand = brand;
                        OMajorGoodsService[i].Size = size;
                        OMajorGoodsService[i].MaterialDescription = materialDescription;
                        OMajorGoodsService[i].HSNCode = hSNCode;
                        OMajorGoodsService[i].TaxPercentage = taxPercentage;
                    }
                }

                LinkButton btn = (LinkButton)sender;
                GridViewRow currentRow = (GridViewRow)btn.NamingContainer;
                int indexToDelete = currentRow.RowIndex;

                if (OMajorGoodsService.Count == 1)
                {
                    OMajorGoodsService[0].Product = string.Empty;
                    OMajorGoodsService[0].Brand = string.Empty;
                    OMajorGoodsService[0].Size = string.Empty;
                    OMajorGoodsService[0].MaterialDescription = string.Empty;
                    OMajorGoodsService[0].HSNCode = string.Empty;
                    OMajorGoodsService[0].TaxPercentage = string.Empty;
                }
                else
                {
                    if (indexToDelete >= 0 && indexToDelete < OMajorGoodsService.Count)
                    {
                        OMajorGoodsService.RemoveAt(indexToDelete);
                    }
                }

                ViewState["MajorGoodsService"] = OMajorGoodsService;
                gvMajorGoods.DataSource = OMajorGoodsService;
                gvMajorGoods.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void BindGridViews()
        {
            gvProjectDetails.DataSource = ViewState["BusinessDetails"] as List<BusinessDetails>;
            gvProjectDetails.DataBind();

            gvPartners.DataSource = ViewState["PartnerDetails"] as List<PartnerDetails>;
            gvPartners.DataBind();

            gvMajorGoods.DataSource = ViewState["MajorGoodsService"] as List<MajorGoodsService>;
            gvMajorGoods.DataBind();

            gvMajorCustomers.DataSource = ViewState["MajorCustomer"] as List<MajorCustomers>;
            gvMajorCustomers.DataBind();


        }
        [Serializable]
        public class PartnerDetails
        {
            public int SI_No { get; set; }
            public string Name { get; set; }
            public string Designation { get; set; }
            public string Contact_No { get; set; }
            public string Email_ID { get; set; }
            public int RowID { get; internal set; }
        }
        private void BindPartnerDetails()
        {

            List<PartnerDetails> partnerDetails = ViewState["PartnerDetails"] as List<PartnerDetails> ?? new List<PartnerDetails>();

            if (partnerDetails.Count == 0)
            {
                partnerDetails.Add(new PartnerDetails { SI_No = 1, Designation = "", Contact_No = "", Email_ID = "" });

            }
            gvPartners.DataSource = partnerDetails;
            gvPartners.DataBind();
        }

        protected void HomeLogo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/HomePage.aspx");
        }
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/HomePage.aspx");
        }












        private void BuildPreviewData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            data["GST Number"] = GSTNumber.Text;
            data["PAN Number"] = PANNumber.Text;
            data["Trade Name"] = tradeName.Text;
            data["Nature of Business"] = natureOfBusinessActivity.Text;
            data["Date of Establishment"] = dateOfEstablishment.Text;
            data["Contact Person"] = contactPersonName.Text;
            data["Designation"] = designation.Text;
            data["Email ID"] = emailId.Text;
            data["Mobile Number"] = mobileNo.Text;
            data["Office Telephone"] = officeTelephoneNo.Text;
            data["TAN Number"] = tanNo.Text;

            data["Registered Address"] = $"{registeredOfficeAddress1.Text}, {registeredOfficeAddress2.Text}, {registeredOfficeAddress3.Text}, {registeredOfficeCity.Text}, {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {registeredOfficeZipCode.Text}";

            if (sameAsRegisteredOffice.Checked)
                data["Billing Address"] = $"{businessBillingAddress1.Text}, {businessBillingAddress2.Text}, {businessBillingAddress3.Text}, {businessBillingCity.Text}, {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {businessBillingZipCode.Text}";
            else
                data["Billing Address"] = $"{businessBillingAddress1.Text}, {businessBillingAddress2.Text}, {businessBillingAddress3.Text}, {businessBillingCity.Text}, {businessBillingState.SelectedItem.Text}, {businessBillingCountry.SelectedItem.Text} - {businessBillingZipCode.Text}";

            if (sameAsRegisteredOffice1.Checked)
                data["Shipping Address"] = $"{shippingAddress1.Text}, {shippingAddress2.Text}, {shippingAddress3.Text}, {shippingCity.Text}, {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {shippingZipCode.Text}";
            else
                data["Shipping Address"] = $"{shippingAddress1.Text}, {shippingAddress2.Text}, {shippingAddress3.Text}, {shippingCity.Text}, {shippingState.SelectedItem.Text}, {shippingCountry.SelectedItem.Text} - {shippingZipCode.Text}";

            if (sameAsRegisteredOffice2.Checked)
                data["Goods Return Address"] = $"{goodsReturnAddress1.Text}, {goodsReturnAddress2.Text}, {goodsReturnAddress3.Text}, {goodsReturnCity.Text}, {registeredOfficeState.SelectedItem.Text}, {registeredOfficeCountry.SelectedItem.Text} - {goodsReturnZipcode.Text}";
            else
                data["Goods Return Address"] = $"{goodsReturnAddress1.Text}, {goodsReturnAddress2.Text}, {goodsReturnAddress3.Text}, {goodsReturnCity.Text}, {goodsReturnState.SelectedItem.Text}, {goodsReturnCountry.SelectedItem.Text} - {goodsReturnZipcode.Text}";

            data["Credit Days"] = CreditDays.Text;
            data["Discount"] = DisCount.Text;
            data["Bank Name"] = bankName.SelectedItem.Text;
            data["Account Name"] = accountName.Text;
            data["Account Number"] = accountNumber.Text;
            data["IFSC Code"] = ifscCode.Text;
            data["Branch Code"] = branchCode.Text;
            data["Bank Address"] = bankAddress.Text;

            data["MSME Status"] = msmeRegistrationStatus.SelectedItem.Text;
            data["MSME Number"] = MSMENO.Text;
            data["Enterprise Type"] = ddlEnterpriseType.SelectedItem.Text;

            data["Mark Down % on MRP (with Tax @0%)"] = Payment1.Text;
            data["Mark Down % on MRP (without Tax @0%)"] = Payment2.Text;
            data["Mark Down % on MRP (with Tax @3%)"] = Payment3.Text;
            data["Mark Down % on MRP (without Tax @3%)"] = Payment4.Text;
            data["Mark Down % on MRP (with Tax @5%)"] = Payment5.Text;
            data["Mark Down % on MRP (without Tax @5%)"] = Payment6.Text;
            data["Mark Down % on MRP (with Tax @18%)"] = Payment9.Text;
            data["Mark Down % on MRP (without Tax @18%)"] = Payment10.Text;

            data["Business Type"] = BusinessType1.SelectedItem.Text;
            data["Agency Email"] = AgencyEmail.Text;
            data["Agency Name"] = AgencyName.Text;

            AddGridToData(data, gvProjectDetails, "Business Location");
            AddGridToData(data, gvPartners, "Partners / Proprietor / Director's / Business Head Detail");
            AddGridToData(data, gvOperationalContacts, "Primary Operational Contacts");
            AddGridToData(data, gvMajorGoods, "Major Goods and Services");
            AddGridToData(data, gvMajorCustomers, "List of Major Customers");
            AddGridToData(data, gvOtherInformation, "Other Information");

            data["Name"] = declarationName.Text;
            data["Designation"] = declarationDesignation.Text;
            data["Mobile No"] = MobileNo1.Text;

            Session["PreviewData"] = data;
        }


        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try { dBConnection.writeLog("SubmitButton_Click", "SubmitButton clicked for GST: " + GSTNumber.Text, "Debug"); } catch {}

            {
                string TName = tradeName.Text;
                string contactperson = ContactPerson.SelectedValue;
                string Raddress1 = registeredOfficeAddress1.Text;
                string Raddress2 = registeredOfficeAddress2.Text;
                string Raddress3 = registeredOfficeAddress3.Text;
                string Rcountry = registeredOfficeCountry.SelectedValue;
                string Rstate = registeredOfficeState.SelectedValue;
                string Rzipcode = registeredOfficeZipCode.Text;
                string Gaddress1 = goodsReturnAddress1.Text;
                string Gaddress2 = goodsReturnAddress2.Text;
                string Gaddress3 = goodsReturnAddress3.Text;
                string Gcountry = goodsReturnCountry.SelectedValue;
                string Gstate = goodsReturnState.SelectedValue;
                string Gzipcode = goodsReturnZipcode.Text;
                string GoodsReturnCity = goodsReturnCity.Text;

                string Saddress1 = shippingAddress1.Text;
                string Saddress2 = shippingAddress2.Text;
                string Saddress3 = shippingAddress2.Text;
                string Scountry = shippingCountry.SelectedValue;
                string Sstate = shippingState.SelectedValue;
                string Szipcode = shippingZipCode.Text;
                string Scity = shippingCity.Text;

                string Baddress1 = businessBillingAddress1.Text;
                string Baddress2 = businessBillingAddress2.Text;
                string Baddress3 = businessBillingAddress3.Text;
                string Bcountry = businessBillingCountry.SelectedValue;
                string Bstate = businessBillingState.SelectedValue;
                string Bzipcode = businessBillingZipCode.Text;
                string NatureOfBusinessActivity = natureOfBusinessActivity.Text;
                string DateOfEstablishment = dateOfEstablishment.Text;
                string BusinessBillingCity = businessBillingCity.Text;
                string RegisteredOfficeCity = registeredOfficeCity.Text;
                string ContactPersonName = contactPersonName.Text;
                string Designation = designation.Text;
                string EmailId = emailId.Text;
                string ToMail = emailId.Text.Trim();
                string MobileNo = mobileNo.Text;
                string OfficeTelephoneNo = officeTelephoneNo.Text;
                string TanNo = tanNo.Text;
                string MsmeRegistrationStatus = msmeRegistrationStatus.SelectedValue;
                string BankName = bankName.SelectedValue;
                string AccountName = accountName.Text;
                string AccountNumber = accountNumber.Text;
                string IfscCode = ifscCode.Text;
                string BranchCode = branchCode.Text;
                string BankAddress = bankAddress.Text;
                string DeclarationName = declarationName.Text;
                string DeclarationDesignation = declarationDesignation.Text;
                string MSMENo = MSMENO.Text;
                string GSTNo = GSTNumber.Text;
                string PartnerType = ddpartnertype.SelectedValue;
                string VerificationNo = MobileNo1.Text;
                string getid = dBConnection.GetSingleValue("SELECT IfNULL(MAX(\"Id\"), 0) + 1 FROM \"TEC_OLED\"");
                int Id = Convert.ToInt32(getid);
                string getdeleteid = dBConnection.GetSingleValue("select \"Id\" from tec_oled where \"GstNo\" = '" + GSTNo + "'");
                if (getdeleteid != null && getdeleteid != "")
                {
                    int dId = Convert.ToInt32(getdeleteid);
                }

                if (
                    string.IsNullOrEmpty(Raddress1) ||
                    string.IsNullOrEmpty(Rcountry) ||
                    string.IsNullOrEmpty(Baddress1) ||
                    string.IsNullOrEmpty(Bcountry) ||
                    string.IsNullOrEmpty(TName) ||
                    string.IsNullOrEmpty(EmailId) ||
                    string.IsNullOrEmpty(MobileNo) ||
                    string.IsNullOrEmpty(BankName) ||
                    string.IsNullOrEmpty(AccountName) ||
                    string.IsNullOrEmpty(AccountNumber) ||
                    string.IsNullOrEmpty(IfscCode) ||
                    string.IsNullOrEmpty(GSTNo))
                {
                    string script = "alert('Please fill the required fields.');";
                    ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);


                }
                else
                {
                    if (GSTNo.Length == 15)
                    {



                        string sServer = ConfigurationManager.AppSettings["Server"];
                        string sDBUser = DbConnection.DecryptFun(ConfigurationManager.AppSettings["DBUser"]);
                        string sDBPwd = DbConnection.DecryptFun(ConfigurationManager.AppSettings["DBPwd"]);
                        string sDBName = ConfigurationManager.AppSettings["DBName"];

                        string draftcheck = dBConnection.GetSingleValue("select \"Draft\" from tec_oled where \"Id\"='" + Id + "' or \"GstNo\" = '" + GSTNumber.Text.Trim() + "' ");
                        string sConstr = "DRIVER={HDBODBC};UID=" + sDBUser + "PWD=" + sDBPwd + "DATABASENAME=NDB;SERVERNODE=" + sServer + "CS=" + sDBName + ";";
                        using (HanaConnection conn = new HanaConnection(sConstr))
                        {
                            conn.Open();
                            using (HanaTransaction transaction = conn.BeginTransaction())
                            {
                                try
                                {
                                    string IdCheck = dBConnection.GetSingleValue("select \"Id\" from TEC_OLED where \"GstNo\"='" + GSTNo + "'  ");
                                    int IdCheck1 = 0;
                                    if (!string.IsNullOrEmpty(IdCheck))
                                    {
                                        IdCheck1 = Convert.ToInt32(IdCheck);
                                    }
                                    if (IdCheck == null || IdCheck == "")
                                    {
                                        string query = "INSERT INTO tec_oled (" +
        "\"Id\", \"TName\", \"Raddress1\", \"Raddress2\", \"Raddress3\", \"Rcountry\", \"Rstate\", \"registeredOfficeCity\", \"Rzipcode\", " +
        "\"Baddress1\", \"Baddress2\", \"Baddress3\", \"Bcountry\", \"Bstate\", \"businessBillingCity\", \"Bzipcode\", " +
        "\"NatureOfBusinessActivity\", \"DateOfEstablishment\", \"ContactPersonName\", \"Designation\", \"EmailId\", " +
        "\"MobileNo\", \"OfficeTelephoneNo\", \"TANNo\", \"MsmeRegistrationStatus\",\"BankName\", \"AccountName\", " +
        "\"AccountNumber\", \"IfscCode\", \"BranchCode\", \"BankAddress\", \"GstNo\", \"DeclarationName\", \"DeclarationDesignation\"," +
        "\"AppliedDate\",\"PartnerType\", \"MSMENo\",\"PanNo\",\"EnterpriseType\",\"BusinessType\",\"AgencyEmail\",\"AgencyName\"," +
        "\"VerificationNo\",\"Gaddress1\", \"Gaddress2\", \"Gaddress3\", \"Gcountry\", \"Gstate\", \"Gcity\", \"Gzipcode\",\"Saddress1\", \"Saddress2\", \"Saddress3\", \"Scountry\", \"Sstate\", \"Scity\", \"Szipcode\",\"Draft\",\"ContactPerson\") " +
        "VALUES ('" + Id + "', '" + TName + "', '" + Raddress1 + "', '" + Raddress2 + "', '" + Raddress3 + "', '" + Rcountry + "', '" + Rstate + "', '" + RegisteredOfficeCity + "', '" + Rzipcode + "', " +
        "'" + Baddress1 + "', '" + Baddress2 + "', '" + Baddress3 + "', '" + Bcountry + "', '" + Bstate + "', '" + BusinessBillingCity + "', '" + Bzipcode + "', " +
        "'" + NatureOfBusinessActivity + "', '" + DateOfEstablishment + "', '" + ContactPersonName + "', '" + Designation + "', '" + EmailId + "', " +
        "'" + MobileNo + "', '" + OfficeTelephoneNo + "', '" + TanNo + "', '" + MsmeRegistrationStatus + "', '" + BankName + "', '" + AccountName + "', " +
        "'" + AccountNumber + "', '" + IfscCode + "', '" + BranchCode + "', '" + BankAddress + "', '" + GSTNo + "', '" + DeclarationName + "', " +
        "'" + DeclarationDesignation + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + PartnerType + "','" + MSMENo + "'," +
        "'" + PANNumber.Text.Trim() + "','" + ddlEnterpriseType.SelectedValue + "','" + BusinessType1.SelectedValue.Trim() + "'," +
        "'" + AgencyEmail.Text.Trim() + "','" + AgencyName.Text.Trim() + "','" + VerificationNo + "','" + Gaddress1 + "', '" + Gaddress2 + "', '" + Gaddress3 + "', '" + Gcountry + "', '" + Gstate + "', '" + GoodsReturnCity + "', '" + Gzipcode + "','" + Saddress1 + "', '" + Saddress2 + "', '" + Saddress3 + "', '" + Scountry + "', '" + Sstate + "', '" + Scity + "', '" + Szipcode + "','N','" + contactperson + "')";
                                        insertHeaderDetails(conn, transaction, Id, true, draftcheck, query);
                                        InsertPaymentDetails(conn, transaction, Id, false, draftcheck);
                                        InsertBusinessDetails(conn, transaction, Id, false, draftcheck);


                                        InsertPartnerDetails(conn, transaction, Id, false, draftcheck);
                                        InsertOperationalContacts(conn, transaction, Id, false, draftcheck);
                                        InsertMajorGoodsServices(conn, transaction, Id, false, draftcheck);

                                        InsertMajorCustomers(conn, transaction, Id, false, draftcheck);
                                        InsertOtherInformation(conn, transaction, Id, false, draftcheck);
                                        InsertDocuments(conn, transaction, Id, false, draftcheck, "N");
                                        InsertDocuments1(conn, transaction, Id, false, draftcheck, "N");
                                        transaction.Commit();


                                    }

                                    else if (IdCheck1 > 0)
                                    {
                                        string query = "UPDATE tec_oled SET " +
                                        "\"TName\" = '" + TName + "', " +
                                        "\"PartnerType\" = '" + PartnerType + "'," +
                                        "\"Raddress1\" = '" + Raddress1 + "', " +
                                        "\"Raddress2\" = '" + Raddress2 + "', " +
                                        "\"Raddress3\" = '" + Raddress3 + "', " +
                                        "\"Rcountry\" = '" + Rcountry + "', " +
                                        "\"Rstate\" = '" + Rstate + "', " +
                                        "\"Rzipcode\" = '" + Rzipcode + "', " +
                                        "\"registeredOfficeCity\" = '" + RegisteredOfficeCity + "', " +
                                        "\"Baddress1\" = '" + Baddress1 + "', " +
                                        "\"Baddress2\" = '" + Baddress2 + "', " +
                                        "\"Baddress3\" = '" + Baddress3 + "', " +
                                        "\"Bcountry\" = '" + Bcountry + "', " +
                                        "\"Bstate\" = '" + Bstate + "', " +
                                        "\"businessBillingCity\" = '" + BusinessBillingCity + "', " +
                                        "\"Bzipcode\" = '" + Bzipcode + "', " +
                                        "\"NatureOfBusinessActivity\" = '" + NatureOfBusinessActivity + "', " +
                                        "\"DateOfEstablishment\" = '" + DateOfEstablishment + "', " +
                                        "\"ContactPersonName\" = '" + ContactPersonName + "', " +
                                        "\"Designation\" = '" + Designation + "', " +
                                        "\"EmailId\" = '" + EmailId + "', " +
                                        "\"MobileNo\" = '" + MobileNo + "', " +
                                        "\"OfficeTelephoneNo\" = '" + OfficeTelephoneNo + "', " +
                                        "\"TANNo\" = '" + TanNo + "', " +
                                        "\"MsmeRegistrationStatus\" = '" + MsmeRegistrationStatus + "', " +
                                        "\"BankName\" = '" + BankName + "', " +
                                        "\"AccountName\" = '" + AccountName + "', " +
                                        "\"AccountNumber\" = '" + AccountNumber + "', " +
                                        "\"IfscCode\" = '" + IfscCode + "', " +
                                        "\"BranchCode\" = '" + BranchCode + "', " +
                                        "\"BankAddress\" = '" + BankAddress + "', " +
                                        "\"GstNo\" = '" + GSTNo + "', " +
                                        "\"DeclarationName\" = '" + DeclarationName + "', " +
                                        "\"DeclarationDesignation\" = '" + DeclarationDesignation + "', " +
                                        "\"Draft\" = 'N' ," +
                                        "\"AppliedDate\" = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                                        "\"MSMENo\" = '" + MSMENo + "'," +
                                        "\"PanNo\" = '" + PANNumber.Text.Trim() + "'," +
                                         "\"EnterpriseType\" = '" + ddlEnterpriseType.SelectedValue.Trim() + "'," +
                                          "\"BusinessType\" = '" + BusinessType1.Text + "'," +
                                           "\"AgencyEmail\" = '" + AgencyEmail.Text + "'," +
                                            "\"AgencyName\" = '" + AgencyName.Text + "'," +
                                           "\"VerificationNo\" = '" + VerificationNo + "'," +
                                           "\"Gaddress1\" = '" + Gaddress1 + "'," +
                                           "\"Gaddress2\" = '" + Gaddress2 + "'," +
                                           "\"Gaddress3\" = '" + Gaddress3 + "'," +
                                           "\"Gcountry\" = '" + Gcountry + "'," +
                                           "\"Gstate\" = '" + Gstate + "'," +
                                           "\"Gzipcode\" = '" + Gzipcode + "'," +
                                           "\"Gcity\" = '" + GoodsReturnCity + "'," +
                                            "\"Saddress1\" = '" + Saddress1 + "'," +
                                           "\"Saddress2\" = '" + Saddress2 + "'," +
                                           "\"Saddress3\" = '" + Saddress3 + "'," +
                                           "\"Scountry\" = '" + Scountry + "'," +
                                           "\"Sstate\" = '" + Sstate + "'," +
                                           "\"Szipcode\" = '" + Szipcode + "'," +
                                           "\"Scity\" = '" + Scity + "'," +
                                           "\"ContactPerson\"='" + contactperson + "'" +
                                        "WHERE \"Id\" = '" + IdCheck1 + "'";

                                        insertHeaderDetails(conn, transaction, IdCheck1, false, draftcheck, query);

                                        InsertPaymentDetails(conn, transaction, IdCheck1, false, draftcheck);
                                        InsertBusinessDetails(conn, transaction, IdCheck1, false, draftcheck);


                                        InsertPartnerDetails(conn, transaction, IdCheck1, false, draftcheck);
                                        InsertOperationalContacts(conn, transaction, IdCheck1, false, draftcheck);
                                        InsertMajorGoodsServices(conn, transaction, IdCheck1, false, draftcheck);
                                        InsertMajorCustomers(conn, transaction, IdCheck1, false, draftcheck);
                                        InsertOtherInformation(conn, transaction, IdCheck1, false, draftcheck);
                                        InsertDocuments(conn, transaction, IdCheck1, false, draftcheck, "Y");
                                        InsertDocuments1(conn, transaction, IdCheck1, false, draftcheck, "Y");
                                        string que = "Delete from \"ApprovalTrace\" where \"ApproveStatus\"='N' and \"GstNo\"='" + GSTNo + "'";
                                        using (var cmd = new HanaCommand(que, conn, transaction))
                                        {
                                            cmd.ExecuteNonQuery();
                                        }
                                        string query11 = "UPDATE TEC_OLED SET \"Approval\"='',\"Draft\"='N',\"RejectionStatus\"='N', \"RejectionReason\"='', \"RejectedUser\"='',\"ApprovedDepartment\"='' WHERE  \"Id\" = '" + IdCheck1 + "'";
                                        using (var cmd = new HanaCommand(query11, conn, transaction))
                                        {
                                            cmd.ExecuteNonQuery();
                                        }
                                        transaction.Commit();
                                        Session["Path_PANCard"] = null;
                                        Session["Path_GSTCertificate"] = null;
                                        Session["Path_BankAccount"] = null;
                                        Session["Path_MSMECertificate"] = null;



                                    }

                                    string script = @"
alert('You have successfully submitted the form.');
window.location.href = 'EditableVendorDetails.aspx';
";
                                    string script1 = @"
                                    for (let i = localStorage.length - 1; i >= 0; i--) {
                                        let key = localStorage.key(i);
                                        if (key.startsWith('image')) {
                                            localStorage.removeItem(key);
                                            console.log('Removed localStorage key: ' + key);
                                        }
                                    }
                                ";
                                    ClientScript.RegisterStartupScript(this.GetType(), "clearImageStorage", script1, true);
                                    ClientScript.RegisterStartupScript(this.GetType(), "SubmissionSuccess", script, true);


                                }

                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    Response.Write($"Error: {ex.Message}");
                                }
                            }
                        }
                    }
                    else
                    {
                        string script = "alert('GSTNO must be 15 character'); window.location.href = window.location.href;";
                        ClientScript.RegisterStartupScript(this.GetType(), "GSTNO must be 15 character", script, true);
                    }

                }
            }
        }
        private List<GoodItem> CollectGoodsFromGrid(GridView gvMajorGoods)
        {
            var list = new List<GoodItem>();
            for (int i = 0; i < gvMajorGoods.Rows.Count; i++)
            {
                GridViewRow row = gvMajorGoods.Rows[i];

                var txtProduct = row.FindControl("txtProduct") as TextBox;
                var txtBrand = row.FindControl("txtBrand") as TextBox;
                var txtSize = row.FindControl("txtSize") as TextBox;
                var txtMaterialDescription = row.FindControl("txtMaterialDescription") as TextBox;
                var txtHSNCode = row.FindControl("txtHSNCode") as TextBox;
                var txtTaxPercentage = row.FindControl("txtTaxPercentage") as TextBox;

                var item = new GoodItem
                {
                    SerialNo = i + 1,
                    Product = txtProduct?.Text?.Trim() ?? "",
                    Brand = txtBrand?.Text?.Trim() ?? "",
                    Size = txtSize?.Text?.Trim() ?? "",
                    MaterialDescription = txtMaterialDescription?.Text?.Trim() ?? "",
                    HSNCode = txtHSNCode?.Text?.Trim() ?? "",
                    TaxPercentage = txtTaxPercentage?.Text?.Trim() ?? ""
                };

                bool allEmpty = string.IsNullOrWhiteSpace(item.Product)
                                && string.IsNullOrWhiteSpace(item.Brand)
                                && string.IsNullOrWhiteSpace(item.Size)
                                && string.IsNullOrWhiteSpace(item.MaterialDescription)
                                && string.IsNullOrWhiteSpace(item.HSNCode)
                                && string.IsNullOrWhiteSpace(item.TaxPercentage);

                if (!allEmpty) list.Add(item);
            }
            return list;
        }

















































        private void InsertGridData(HanaConnection conn, string gstNo, GridView grid, string tableName)
        {
            foreach (GridViewRow row in grid.Rows)
            {
                StringBuilder columns = new StringBuilder("\"GSTNo\"");
                StringBuilder values = new StringBuilder("@GSTNo");

                using (HanaCommand cmd = new HanaCommand())
                {
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@GSTNo", gstNo);

                    for (int i = 0; i < grid.HeaderRow.Cells.Count; i++)
                    {
                        string header = grid.HeaderRow.Cells[i].Text.Trim();
                        if (header == "Action") continue;

                        string val = "";
                        foreach (Control ctrl in row.Cells[i].Controls)
                        {
                            if (ctrl is TextBox txt) val = txt.Text.Trim();
                            else if (ctrl is DropDownList ddl) val = ddl.SelectedItem.Text.Trim();
                        }

                        if (!string.IsNullOrEmpty(header))
                        {
                            string col = "\"" + header.Replace(" ", "_") + "\"";
                            string p = "@p" + i;
                            columns.Append($", {col}");
                            values.Append($", {p}");
                            cmd.Parameters.AddWithValue(p, val);
                        }
                    }

                    cmd.CommandText = $@"UPSERT ""{tableName}"" ({columns}) VALUES ({values})";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void insertHeaderDetails(HanaConnection conn, HanaTransaction transaction, int Id, bool isDraft, string draftcheck, string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                using (var cmd = new HanaCommand(query, conn, transaction))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InsertBusinessDetails(HanaConnection conn, HanaTransaction transaction, int Id, bool isDraft, string draftcheck)
        {
            int i = 0;
            foreach (GridViewRow row in gvProjectDetails.Rows)
            {
                i++;
                var txtbisnusstsextbox = row.FindControl("businessState") as System.Web.UI.WebControls.DropDownList;
                var gstnumber = row.FindControl("gstNumber") as System.Web.UI.WebControls.TextBox;
                var address = row.FindControl("addressOfPlace") as System.Web.UI.WebControls.TextBox;
                var gstvendorclassification = row.FindControl("gstVendorClassification") as System.Web.UI.WebControls.DropDownList;
                if (isDraft)
                {
                    string query = "INSERT INTO tec_led1 (\"Id\", \"LineId\", \"BusinessState\", \"GSTNumber\", \"AddressOfPlace\", \"GSTVendorClassification\") VALUES ('" +
               Id + "', '" + i + "', '" + txtbisnusstsextbox.Text.Trim() + "', '" +
               gstnumber.Text.Trim() + "', '" + address.Text.Trim() + "', '" +
               gstvendorclassification.Text.Trim() + "')";

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (var cmd = new HanaCommand(query, conn, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(txtbisnusstsextbox.Text) &&
                        !string.IsNullOrEmpty(gstnumber.Text) &&
                        !string.IsNullOrEmpty(address.Text))
                    {
                        if (draftcheck.Trim() == "N" || draftcheck.Trim() == string.Empty)
                        {
                            string query = "INSERT INTO tec_led1 (\"Id\", \"LineId\", \"BusinessState\", \"GSTNumber\", \"AddressOfPlace\", \"GSTVendorClassification\") VALUES ('" +
                Id + "', '" + i + "', '" + txtbisnusstsextbox.Text.Trim() + "', '" +
                gstnumber.Text.Trim() + "', '" + address.Text.Trim() + "', '" +
                gstvendorclassification.Text.Trim() + "')";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, conn, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                        else
                        {
                            string query = "UPDATE tec_led1 SET " +
                "\"BusinessState\" = '" + txtbisnusstsextbox.Text.Trim() + "', " +
                "\"GSTNumber\" = '" + gstnumber.Text.Trim() + "', " +
                "\"AddressOfPlace\" = '" + address.Text.Trim() + "', " +
                "\"GSTVendorClassification\" = '" + gstvendorclassification.Text.Trim() + "' " +
                "WHERE \"Id\" = '" + Id + "' AND \"LineId\" = '" + i + "'";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, conn, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                    }
                }
            }
        }

        private void InsertPaymentDetails(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck)
        {
            var creditDays = CreditDays.Text.Trim();
            var discount = DisCount.Text.Trim();
            var markDownTax0 = Payment1.Text.Trim();
            var markDownWithoutTax0 = Payment2.Text.Trim();
            var markDownTax3 = Payment3.Text.Trim();
            var markDownWithoutTax3 = Payment4.Text.Trim();
            var markDownTax5 = Payment5.Text.Trim();
            var markDownWithoutTax5 = Payment6.Text.Trim();
            var markDownTax18 = Payment9.Text.Trim();
            var markDownWithoutTax18 = Payment10.Text.Trim();
            var businessType = BusinessType1.SelectedValue.Trim();
            var agencyEmail = AgencyEmail.Text.Trim();
            var agencyName = AgencyName.Text.Trim();
            var priceType = ddlPriceType.SelectedValue.Trim();


            if (isDraft)
            {
                string query = "INSERT INTO \"PaymentDetails\" (\"Id\", \"CreditDays\",\"DisCount\", \"MarkDownTax0\", \"MarkDownWithoutTax0\", " +
               "\"MarkDownTax3\", \"MarkDownWithoutTax3\", \"MarkDownTax5\", \"MarkDownWithoutTax5\", " +
               " \"MarkDownTax18\", \"MarkDownWithoutTax18\", " +
               "\"BusinessType\", \"AgencyEmail\", \"AgencyName\") VALUES ('" +
               Id + "', '" + creditDays.Trim() + "','" + discount + "', '" +
               markDownTax0.Trim() + "', '" + markDownWithoutTax0.Trim() + "', '" +
               markDownTax3.Trim() + "', '" + markDownWithoutTax3.Trim() + "', '" +
               markDownTax5.Trim() + "', '" + markDownWithoutTax5.Trim() + "', '" +
               markDownTax18.Trim() + "', '" + markDownWithoutTax18.Trim() + "', '" +
               businessType.Trim() + "', '" + agencyEmail.Trim() + "', '" + agencyName.Trim() + "')";
                using (var cmd = new HanaCommand(query, connection, transaction))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                if (draftcheck.Trim() == "N" || draftcheck.Trim() == string.Empty)
                {
                    if (!string.IsNullOrEmpty(creditDays) &&
                        !string.IsNullOrEmpty(markDownTax0) &&
                        !string.IsNullOrEmpty(markDownWithoutTax0) &&
                        !string.IsNullOrEmpty(businessType) &&
                        !string.IsNullOrEmpty(agencyEmail) &&
                         !string.IsNullOrEmpty(agencyName))
                    {
                        string query = "INSERT INTO \"PaymentDetails\" (\"Id\", \"CreditDays\",\"DisCount\", \"MarkDownTax0\", \"MarkDownWithoutTax0\", " +
                            "\"MarkDownTax3\", \"MarkDownWithoutTax3\", \"MarkDownTax5\", \"MarkDownWithoutTax5\", " +
                            " \"MarkDownTax18\", \"MarkDownWithoutTax18\", " +
                            "\"BusinessType\", \"AgencyEmail\", \"AgencyName\") VALUES ('" +
                            Id + "', '" + creditDays + "','" + discount + "', '" +
                            markDownTax0 + "', '" + markDownWithoutTax0 + "', '" +
                            markDownTax3 + "', '" + markDownWithoutTax3 + "', '" +
                            markDownTax5 + "', '" + markDownWithoutTax5 + "', '" +
                            markDownTax18 + "', '" + markDownWithoutTax18 + "', '" +
                            businessType + "', '" + agencyEmail + "', '" + agencyName + "')";

                        using (var cmd = new HanaCommand(query, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    string query = "UPDATE \"PaymentDetails\" SET " +
                        "\"CreditDays\" = '" + creditDays + "', " +
                        "\"DisCount\" = '" + discount + "', " +
                        "\"MarkDownTax0\" = '" + markDownTax0 + "', " +
                        "\"MarkDownWithoutTax0\" = '" + markDownWithoutTax0 + "', " +
                        "\"MarkDownTax3\" = '" + markDownTax3 + "', " +
                        "\"MarkDownWithoutTax3\" = '" + markDownWithoutTax3 + "', " +
                        "\"MarkDownTax5\" = '" + markDownTax5 + "', " +
                        "\"MarkDownWithoutTax5\" = '" + markDownWithoutTax5 + "', " +
                        "\"MarkDownTax18\" = '" + markDownTax18 + "', " +
                        "\"MarkDownWithoutTax18\" = '" + markDownWithoutTax18 + "', " +
                        "\"BusinessType\" = '" + businessType + "', " +
                        "\"AgencyEmail\" = '" + agencyEmail + "', " +
                         "\"AgencyName\" = '" + agencyName + "', " +
                         "\"PriceType\" = '" + priceType.Trim() + "'" +
                        "WHERE \"Id\" = '" + Id + "'";

                    using (var cmd = new HanaCommand(query, connection, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }


            }
        }
        protected void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            string filePath = ConfigurationManager.AppSettings["ExcelFilePath"];
            string fullPath = filePath;

            if (System.IO.File.Exists(fullPath))
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("Content-Disposition", $"attachment; filename={System.IO.Path.GetFileName(fullPath)}");
                Response.TransmitFile(fullPath);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Excel file not found.');", true);
            }
        }
        protected void ddlPriceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPriceType.SelectedValue == "Markdown")
            {
                pnlMarkdownFields.Visible = true;
                btnDownloadExcel.Style["visibility"] = "visible";
                lblMRPExcel.Style["visibility"] = "visible";
            }
            else
            {
                pnlMarkdownFields.Visible = false;
                btnDownloadExcel.Style["visibility"] = "hidden";
                lblMRPExcel.Style["visibility"] = "hidden";
            }
        }
        private void InsertPartnerDetails(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck)
        {
            int i = 0;
            foreach (GridViewRow row in gvPartners.Rows)
            {
                i++;
                var name = row.FindControl("partnerName") as System.Web.UI.WebControls.TextBox;
                var Designation = row.FindControl("partnerDesignation") as System.Web.UI.WebControls.TextBox;
                var Contact_No = row.FindControl("partnerContactNo") as System.Web.UI.WebControls.TextBox;
                var Email_ID = row.FindControl("partnerEmail") as System.Web.UI.WebControls.TextBox;

                if (isDraft)
                {
                    string query = "INSERT INTO tec_led2 (\"Id\", \"LineId\", \"Name\", \"Designation\", \"Contact_No\", \"Email_ID\") VALUES ('" +
                Id + "', '" + i + "', '" + name.Text.Trim() + "', '" +
                Designation.Text.Trim() + "', '" + Contact_No.Text.Trim() + "', '" +
                Email_ID.Text.Trim() + "')";

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (var cmd = new HanaCommand(query, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                else
                {
                    if (draftcheck.Trim() == "N" || draftcheck.Trim() == string.Empty)
                    {
                        if (!string.IsNullOrEmpty(name?.Text) &&
                            !string.IsNullOrEmpty(Designation?.Text) &&
                            !string.IsNullOrEmpty(Contact_No?.Text) &&
                            !string.IsNullOrEmpty(Email_ID?.Text))
                        {
                            string query = "INSERT INTO tec_led2 (\"Id\", \"LineId\", \"Name\", \"Designation\", \"Contact_No\", \"Email_ID\") VALUES ('" +
                 Id + "', '" + i + "', '" + name.Text.Trim() + "', '" +
                 Designation.Text.Trim() + "', '" + Contact_No.Text.Trim() + "', '" +
                 Email_ID.Text.Trim() + "')";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                        else
                        {
                            string query = "UPDATE tec_led2 SET " +
                     "\"Name\" = '" + name.Text.Trim() +
                     "', \"Designation\" = '" + Designation.Text.Trim() +
                     "', \"Contact_No\" = '" + Contact_No.Text.Trim() +
                     "', \"Email_ID\" = '" + Email_ID.Text.Trim() +
                     "' WHERE \"Id\" = '" + Id + "' AND \"LineId\" = '" + i + "'";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                    }
                }
            }
        }




        public void InsertOperationalContacts(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck)
        {
            int i = 0;
            foreach (GridViewRow row in gvOperationalContacts.Rows)
            {
                i++;
                var name = row.FindControl("pocName") as System.Web.UI.WebControls.TextBox;
                var designation = row.FindControl("pocDesignation") as System.Web.UI.WebControls.TextBox;
                var contactNo = row.FindControl("pocContactNo") as System.Web.UI.WebControls.TextBox;
                var email = row.FindControl("pocEmail") as System.Web.UI.WebControls.TextBox;
                var department = row.FindControl("lblDepartment") as System.Web.UI.WebControls.Label;


                if (isDraft)
                {

                    string insertQuery = "INSERT INTO tec_led3 (\"ID\", \"LineId\", \"Department\", \"Name\", \"Designation\", \"ContactNo\", \"Email\") VALUES ('" +
                       Id + "', '" + i + "', '" + department?.Text.Trim() + "', '" +
                       name.Text.Trim() + "', '" + designation.Text.Trim() + "', '" +
                       contactNo.Text.Trim() + "', '" + email.Text.Trim() + "')";

                    if (!string.IsNullOrEmpty(insertQuery))
                    {
                        using (var cmd = new HanaCommand(insertQuery, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }



                }
                else
                {
                    if (draftcheck.Trim() == "N" || draftcheck.Trim() == string.Empty)
                    {
                        if (!string.IsNullOrEmpty(name?.Text) &&
                            !string.IsNullOrEmpty(designation?.Text) &&
                            !string.IsNullOrEmpty(contactNo?.Text) &&
                            !string.IsNullOrEmpty(email?.Text))
                        {
                            string insertQuery = "INSERT INTO tec_led3 (\"ID\", \"LineId\", \"Department\", \"Name\", \"Designation\", \"ContactNo\", \"Email\") VALUES ('" +
                      Id + "', '" + i + "', '" + department?.Text.Trim() + "', '" +
                      name.Text.Trim() + "', '" + designation.Text.Trim() + "', '" +
                      contactNo.Text.Trim() + "', '" + email.Text.Trim() + "')";

                            if (!string.IsNullOrEmpty(insertQuery))
                            {
                                using (var cmd = new HanaCommand(insertQuery, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(name?.Text) &&
                            !string.IsNullOrEmpty(designation?.Text) &&
                            !string.IsNullOrEmpty(contactNo?.Text) &&
                            !string.IsNullOrEmpty(email?.Text))
                        {
                            string query = "UPDATE tec_led3 SET " +
                "\"Department\" = '" + department?.Text.Trim() +
                "', \"Name\" = '" + name.Text.Trim() +
                "', \"Designation\" = '" + designation.Text.Trim() +
                "', \"ContactNo\" = '" + contactNo.Text.Trim() +
                "', \"Email\" = '" + email.Text.Trim() +
                "' WHERE \"ID\" = '" + Id + "' AND \"LineId\" = '" + i + "'";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                    }
                }
            }
        }



        private void InsertMajorGoodsServices(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck)
        {
            int i = 0;
            foreach (GridViewRow row in gvMajorGoods.Rows)
            {
                i++;
                var product = row.FindControl("txtProduct") as System.Web.UI.WebControls.TextBox;
                var brand = row.FindControl("txtBrand") as System.Web.UI.WebControls.TextBox;
                var size = row.FindControl("txtSize") as System.Web.UI.WebControls.TextBox;
                var materialDescription = row.FindControl("txtMaterialDescription") as System.Web.UI.WebControls.TextBox;
                var hsnCode = row.FindControl("txtHSNCode") as System.Web.UI.WebControls.TextBox;
                var taxPercentage = row.FindControl("txtTaxPercentage") as System.Web.UI.WebControls.TextBox;

                if (isDraft)
                {
                    string query = "INSERT INTO tec_led4 (\"Id\", \"LineId\", \"MaterialDescription\", \"HSNCode\", \"Brand\", \"Size\",\"Product\" ,\"TaxPercentage\") VALUES ('" +
                Id + "', '" + i + "', '" + materialDescription.Text.Trim() + "', '" +
                hsnCode.Text.Trim() + "', '" + brand.Text.Trim() + "','" + size.Text.Trim() + "','" + product.Text.Trim() + "','" + taxPercentage.Text.Trim() + "')";

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (var cmd = new HanaCommand(query, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                else
                {
                    if (draftcheck.Trim() == "N" || draftcheck.Trim() == string.Empty)
                    {
                        if (!string.IsNullOrEmpty(materialDescription?.Text) &&
                            !string.IsNullOrEmpty(hsnCode?.Text) &&
                            !string.IsNullOrEmpty(brand?.Text) &&
                            !string.IsNullOrEmpty(size?.Text) &&
                            !string.IsNullOrEmpty(taxPercentage?.Text) &&
                             !string.IsNullOrEmpty(product?.Text))
                        {
                            string query = "INSERT INTO tec_led4 (\"Id\", \"LineId\", \"MaterialDescription\", \"HSNCode\",  \"Brand\", \"Size\",\"Product\", \"TaxPercentage\") VALUES ('" +
               Id + "', '" + i + "', '" + materialDescription.Text.Trim() + "', '" +
               hsnCode.Text.Trim() + "','" + brand.Text.Trim() + "','" + size.Text.Trim() + "','" + product.Text.Trim() + "', '" + taxPercentage.Text.Trim() + "')";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                    }
                    else
                    {
                        string query = "UPDATE tec_led4 SET \"MaterialDescription\" = '" + materialDescription.Text.Trim() +
                 "', \"HSNCode\" = '" + hsnCode.Text.Trim() +
                 "', \"Brand\" = '" + brand.Text.Trim() +
                 "', \"Size\" = '" + size.Text.Trim() +
                  "', \"Product\" = '" + product.Text.Trim() +
                 "', \"TaxPercentage\" = '" + taxPercentage.Text.Trim() +
                 "' WHERE \"Id\" = '" + Id + "' AND \"LineId\" = '" + i + "'";

                        if (!string.IsNullOrEmpty(query))
                        {
                            using (var cmd = new HanaCommand(query, connection, transaction))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }

                    }
                }
            }
        }
        public class ImageFile
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }











        private void InsertMajorCustomers(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck)
        {
            int i = 0;
            foreach (GridViewRow row in gvMajorCustomers.Rows)
            {
                i++;
                var txtCustomerName = row.FindControl("CustomerName") as System.Web.UI.WebControls.TextBox;

                if (isDraft)
                {
                    string query = "INSERT INTO tec_led5 (\"ID\", \"LineId\", \"CustomerName\") VALUES ('" +
                Id + "', '" + i + "', '" + txtCustomerName.Text.Trim() + "')";

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (var cmd = new HanaCommand(query, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    if (draftcheck.Trim() == "N" || draftcheck.Trim() == string.Empty)
                    {

                        if (!string.IsNullOrEmpty(txtCustomerName?.Text))
                        {
                            string query = "INSERT INTO tec_led5 (\"ID\", \"LineId\", \"CustomerName\") VALUES ('" +
                Id + "', '" + i + "', '" + txtCustomerName.Text.Trim() + "')";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else
                    {
                        string query = "UPDATE tec_led5 SET \"CustomerName\" = '" + txtCustomerName.Text.Trim() +
                "' WHERE \"ID\" = '" + Id + "' AND \"LineId\" = '" + i + "'";

                        if (!string.IsNullOrEmpty(query))
                        {
                            using (var cmd = new HanaCommand(query, connection, transaction))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }

                    }
                }
            }
        }


        private void InsertOtherInformation(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck)
        {
            int i = 0;
            foreach (GridViewRow row in gvOtherInformation.Rows)
            {
                i++;
                var textModeTextBox = row.FindControl("txtValue") as System.Web.UI.WebControls.TextBox;
                var description = row.FindControl("lblDescription") as System.Web.UI.WebControls.Label;
                if (isDraft)
                {
                    string query = "INSERT INTO tec_led6 (\"ID\", \"LineId\", \"Description\", \"TextMode\") VALUES ('" +
              Id + "', '" +
              i + "', '" +
              description.Text.Trim() + "', '" +
              textModeTextBox.Text.Trim() + "')";

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (var cmd = new HanaCommand(query, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                else
                {
                    if (draftcheck.Trim() == "N" || draftcheck.Trim() == string.Empty)
                    {
                        if (!string.IsNullOrEmpty(textModeTextBox?.Text))
                        {
                            string query = "INSERT INTO tec_led6 (\"ID\", \"LineId\", \"Description\", \"TextMode\") VALUES ('" +
               Id + "', '" +
               i + "', '" + description.Text.Trim() + "','" + textModeTextBox.Text.Trim() + "')";

                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(textModeTextBox?.Text))
                        {
                            string query = "UPDATE tec_led6 SET \"Description\" = '" + description.Text.Trim() +
               "', \"TextMode\" = '" + textModeTextBox.Text.Trim() +
               "' WHERE \"ID\" = '" + Id + "' AND \"LineId\" = '" + i + "'";
                            if (!string.IsNullOrEmpty(query))
                            {
                                using (var cmd = new HanaCommand(query, connection, transaction))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoadStates()
        {



            String Query = "Call \"" + DbConnection.sDBName + "\".\"GetStates\"('" + registeredOfficeCountry.Text + "')";
            DataTable stateData = dBConnection.ExecuteQueryForDataTable(Query);
            registeredOfficeState.DataSource = stateData;
            registeredOfficeState.DataTextField = "StateName";
            registeredOfficeState.DataValueField = "StateCode";
            registeredOfficeState.DataSource = stateData;
            registeredOfficeState.DataTextField = "StateName";
            registeredOfficeState.DataValueField = "StateCode";
            registeredOfficeState.DataBind();

            goodsReturnState.DataSource = stateData;
            goodsReturnState.DataTextField = "StateName";
            goodsReturnState.DataValueField = "StateCode";
            goodsReturnState.DataBind();

            shippingState.DataSource = stateData;
            shippingState.DataTextField = "StateName";
            shippingState.DataValueField = "StateCode";
            shippingState.DataBind();

            businessBillingState.DataSource = stateData;
            businessBillingState.DataTextField = "StateName";
            businessBillingState.DataValueField = "StateCode";
            businessBillingState.DataBind();

            registeredOfficeState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State", ""));
            goodsReturnState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State", ""));
            shippingState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State", ""));
            businessBillingState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State", ""));

            System.Web.UI.WebControls.ListItem defaultItem1 = registeredOfficeState.Items.FindByText("Tamil Nadu");
            if (defaultItem1 != null)
                registeredOfficeState.SelectedValue = defaultItem1.Value;

            System.Web.UI.WebControls.ListItem defaultItem3 = goodsReturnState.Items.FindByText("Tamil Nadu");
            if (defaultItem3 != null)
                goodsReturnState.SelectedValue = defaultItem3.Value;

            System.Web.UI.WebControls.ListItem defaultItem4 = shippingState.Items.FindByText("Tamil Nadu");
            if (defaultItem4 != null)
                shippingState.SelectedValue = defaultItem4.Value;


            System.Web.UI.WebControls.ListItem defaultItem2 = businessBillingState.Items.FindByText("Tamil Nadu");
            if (defaultItem2 != null)
                businessBillingState.SelectedValue = defaultItem2.Value;
        }

        private void LoadBanks()
        {



            String Query = "Call \"" + DbConnection.sDBName + "\".\"GetBanks\"()";
            DataTable reader = dBConnection.ExecuteQueryForDataTable(Query);
            bankName.DataSource = reader;
            bankName.DataTextField = "BankName";
            bankName.DataValueField = "BankCode";
            bankName.DataBind();
            bankName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Bank", ""));
        }

        private void LoadCountries()
        {


            String Query = "Call \"" + DbConnection.sDBName + "\".\"GetCountries\"()";
            DataTable countryData = dBConnection.ExecuteQueryForDataTable(Query);
            registeredOfficeCountry.DataSource = countryData;
            registeredOfficeCountry.DataTextField = "CountryName";
            registeredOfficeCountry.DataValueField = "CountryCode";
            registeredOfficeCountry.DataBind();

            goodsReturnCountry.DataSource = countryData;
            goodsReturnCountry.DataTextField = "CountryName";
            goodsReturnCountry.DataValueField = "CountryCode";
            goodsReturnCountry.DataBind();


            shippingCountry.DataSource = countryData;
            shippingCountry.DataTextField = "CountryName";
            shippingCountry.DataValueField = "CountryCode";
            shippingCountry.DataBind();

            businessBillingCountry.DataSource = countryData;
            businessBillingCountry.DataTextField = "CountryName";
            businessBillingCountry.DataValueField = "CountryCode";
            businessBillingCountry.DataBind();

            registeredOfficeCountry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Country", ""));
            goodsReturnCountry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Country", ""));
            shippingCountry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Country", ""));
            businessBillingCountry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Country", ""));

            System.Web.UI.WebControls.ListItem indiaItem1 = registeredOfficeCountry.Items.FindByText("India");
            if (indiaItem1 != null)
                registeredOfficeCountry.SelectedValue = indiaItem1.Value;
            System.Web.UI.WebControls.ListItem indiaItem3 = goodsReturnCountry.Items.FindByText("India");
            if (indiaItem3 != null)
                goodsReturnCountry.SelectedValue = indiaItem3.Value;

            System.Web.UI.WebControls.ListItem indiaItem4 = shippingCountry.Items.FindByText("India");
            if (indiaItem4 != null)
                shippingCountry.SelectedValue = indiaItem4.Value;

            System.Web.UI.WebControls.ListItem indiaItem2 = businessBillingCountry.Items.FindByText("India");
            if (indiaItem2 != null)
                businessBillingCountry.SelectedValue = indiaItem2.Value;
        }





        protected void GSTNumber_TextChanged(object sender, EventArgs e)
        {
            string gstNumber = GSTNumber.Text.Trim();
            string gst = dBConnection.GetSingleValue("select \"GstNo\" from tec_oled where \"GstNo\"='" + gstNumber + "'");
            string getid1 = dBConnection.GetSingleValue("select \"Id\" from tec_oled where \"GstNo\"='" + gstNumber.Trim() + "' ");

            string gstNumber1 = GSTNumber.Text.Trim();
            if (gstNumber1 != null && gstNumber1.Length == 15)
            {
                string PanNumber = gstNumber1.Substring(2, 10);
                PANNumber.Text = PanNumber;
            }
            else GSTNumber.Focus();
            if (gstNumber1.Length != 15)
            {

                GSTNumber.Focus();
            }
            if (gst != null && gst != "")
            {
                if (!string.IsNullOrEmpty(gstNumber))
                {
                    string getid = dBConnection.GetSingleValue("select \"Id\" from tec_oled where \"GstNo\"='" + gstNumber.Trim() + "' ");

                    if (!string.IsNullOrEmpty(getid))
                    {
                        int FindId = Convert.ToInt32(getid);

                        string query = "SELECT \"TName\",\"PartnerType\", \"Raddress1\", \"Raddress2\", \"Raddress3\", " +
                   "\"Rcountry\", \"Rstate\", \"businessBillingCity\",\"registeredOfficeCity\",\"Gcity\",\"Scity\",\"Rzipcode\", \"Gaddress1\", \"Gaddress2\", \"Gaddress3\", \"Gcountry\", \"Gstate\", \"Gzipcode\", \"Saddress1\", \"Saddress2\", \"Saddress3\", \"Scountry\", \"Sstate\", \"Szipcode\",\"Baddress1\", \"Baddress2\", \"Baddress3\", \"Bcountry\", \"Bstate\", \"Bzipcode\", " +
                   "\"NatureOfBusinessActivity\",\"DateOfEstablishment\",\"ContactPersonName\",\"Designation\",\"EmailId\",\"MobileNo\",\"OfficeTelephoneNo\"," +
                   "\"TANNo\",\"MsmeRegistrationStatus\",\"MSMENo\",\"BankName\",\"AccountName\",\"AccountNumber\",\"IfscCode\",\"BranchCode\",\"BankAddress\",\"DeclarationName\",\"DeclarationDesignation\",\"EnterpriseType\",\"BusinessType\",\"AgencyEmail\",\"AgencyName\",\"VerificationNo\",\"ContactPerson\" " +
                   "FROM tec_oled WHERE \"Id\" = '" + getid + "'";

                        DataTable data = dBConnection.ExecuteQueryForDataTable(query);

                        if (data.Rows.Count > 0)
                        {
                            DataRow row = data.Rows[0];

                            ddpartnertype.SelectedValue = row["PartnerType"].ToString();
                            ddlEnterpriseType.SelectedValue = row["EnterpriseType"].ToString();
                            MSMENO.Text = row["MSMENO"].ToString();
                            tradeName.Text = row["TName"].ToString();
                            MobileNo1.Text = row["VerificationNo"].ToString();
                            BusinessType1.SelectedValue = row["BusinessType"].ToString();
                            AgencyEmail.Text = row["AgencyEmail"].ToString();
                            AgencyName.Text = row["AgencyName"].ToString();
                            registeredOfficeAddress1.Text = row["Raddress1"].ToString();
                            registeredOfficeAddress2.Text = row["Raddress2"].ToString();
                            registeredOfficeAddress3.Text = row["Raddress3"].ToString();


                            string dbValue = row["Rcountry"].ToString();
                            if (!string.IsNullOrEmpty(dbValue) && registeredOfficeCountry.Items.FindByValue(dbValue) != null)
                            {
                                registeredOfficeCountry.SelectedValue = dbValue;
                            }
                            else
                            {
                                registeredOfficeCountry.ClearSelection();
                            }
                            string dbValue22 = row["Gcountry"].ToString();
                            if (!string.IsNullOrEmpty(dbValue22) && goodsReturnCountry.Items.FindByValue(dbValue22) != null)
                            {
                                goodsReturnCountry.SelectedValue = dbValue22;
                            }
                            else
                            {
                                goodsReturnCountry.ClearSelection();
                            }

                            string dbValue111 = row["Scountry"].ToString();
                            if (!string.IsNullOrEmpty(dbValue111) && shippingCountry.Items.FindByValue(dbValue111) != null)
                            {
                                shippingCountry.SelectedValue = dbValue111;
                            }
                            else
                            {
                                shippingCountry.ClearSelection();
                            }

                            string dbValue2 = row["Bcountry"].ToString();
                            if (!string.IsNullOrEmpty(dbValue2) && businessBillingCountry.Items.FindByValue(dbValue2) != null)
                            {
                                businessBillingCountry.SelectedValue = dbValue2;
                            }
                            else
                            {
                                businessBillingCountry.ClearSelection();
                            }
                            string dbValue1 = row["Rstate"].ToString();
                            if (!string.IsNullOrEmpty(dbValue) && registeredOfficeState.Items.FindByValue(dbValue) != null)
                            {
                                registeredOfficeState.SelectedValue = dbValue1;
                            }
                            else
                            {
                                registeredOfficeCountry.ClearSelection();
                            }
                            string dbValue11 = row["Gstate"].ToString();
                            if (!string.IsNullOrEmpty(dbValue11) && registeredOfficeState.Items.FindByValue(dbValue) != null)
                            {
                                registeredOfficeState.SelectedValue = dbValue11;
                            }
                            else
                            {
                                registeredOfficeCountry.ClearSelection();
                            }
                            string dbValue12 = row["Bstate"].ToString();
                            if (!string.IsNullOrEmpty(dbValue12) && businessBillingState.Items.FindByValue(dbValue12) != null)
                            {
                                businessBillingState.SelectedValue = dbValue12;
                            }
                            else
                            {
                                businessBillingState.ClearSelection();
                            }
                            string dbValue112 = row["Sstate"].ToString();
                            if (!string.IsNullOrEmpty(dbValue112) && shippingState.Items.FindByValue(dbValue112) != null)
                            {
                                shippingState.SelectedValue = dbValue112;
                            }
                            else
                            {
                                shippingState.ClearSelection();
                            }

                            registeredOfficeZipCode.Text = row["Rzipcode"].ToString();

                            businessBillingAddress1.Text = row["Baddress1"].ToString();
                            businessBillingAddress2.Text = row["Baddress2"].ToString();
                            businessBillingAddress3.Text = row["Baddress3"].ToString();
                            businessBillingCountry.Text = row["Bcountry"].ToString();
                            businessBillingState.SelectedValue = row["Bstate"].ToString();
                            registeredOfficeCountry.SelectedValue = row["Rcountry"].ToString();
                            businessBillingZipCode.Text = row["Bzipcode"].ToString();

                            goodsReturnAddress1.Text = row["Gaddress1"].ToString();
                            goodsReturnAddress2.Text = row["Gaddress2"].ToString();
                            goodsReturnAddress3.Text = row["Gaddress3"].ToString();

                            goodsReturnCountry.Text = row["Gcountry"].ToString();
                            goodsReturnState.SelectedValue = row["Gstate"].ToString();
                            goodsReturnZipcode.Text = row["Gzipcode"].ToString();
                            goodsReturnCity.Text = row["Gcity"].ToString();

                            shippingAddress1.Text = row["Saddress1"].ToString();
                            shippingAddress2.Text = row["Saddress2"].ToString();
                            shippingAddress3.Text = row["Saddress3"].ToString();
                            shippingCountry.Text = row["Scountry"].ToString();
                            shippingState.SelectedValue = row["Sstate"].ToString();
                            shippingZipCode.Text = row["Szipcode"].ToString();
                            shippingCity.Text = row["Scity"].ToString();

                            natureOfBusinessActivity.Text = row["NatureOfBusinessActivity"].ToString();
                            dateOfEstablishment.Text = row["DateOfEstablishment"].ToString();
                            registeredOfficeCity.Text = row["registeredOfficeCity"].ToString();
                            businessBillingCity.Text = row["businessBillingCity"].ToString();
                            contactPersonName.Text = row["ContactPersonName"].ToString();
                            designation.Text = row["Designation"].ToString();

                            declarationName.Text = row["declarationName"].ToString();
                            declarationDesignation.Text = row["DeclarationDesignation"].ToString();
                            emailId.Text = row["EmailId"].ToString();
                            mobileNo.Text = row["MobileNo"].ToString();
                            officeTelephoneNo.Text = row["OfficeTelephoneNo"].ToString();
                            tanNo.Text = row["TANNo"].ToString();
                            msmeRegistrationStatus.SelectedValue = row["MSMERegistrationStatus"].ToString();
                            ContactPerson.SelectedValue = row["ContactPerson"].ToString();
                            string query12 = "Select * from \"PaymentDetails\" where \"Id\"='" + getid + "'";
                            DataTable data12 = dBConnection.ExecuteQueryForDataTable(query12);
                            if (data12.Rows.Count > 0)
                            {
                                DataRow row1 = data12.Rows[0];
                                CreditDays.Text = row1["CreditDays"].ToString();
                                DisCount.Text = row1["DisCount"].ToString();
                                ddlPriceType.SelectedValue = row1["PriceType"].ToString();
                                Payment1.Text = row1["MarkDownTax0"].ToString();
                                Payment2.Text = row1["MarkDownWithoutTax0"].ToString();
                                Payment3.Text = row1["MarkDownTax3"].ToString();
                                Payment4.Text = row1["MarkDownWithoutTax3"].ToString();
                                Payment5.Text = row1["MarkDownTax5"].ToString();
                                Payment6.Text = row1["MarkDownWithoutTax5"].ToString();
                                Payment9.Text = row1["MarkDownTax18"].ToString();
                                Payment10.Text = row1["MarkDownWithoutTax18"].ToString();
                                BusinessType1.SelectedValue = row1["BusinessType"].ToString();
                                AgencyEmail.Text = row1["AgencyEmail"].ToString();
                                AgencyName.Text = row1["AgencyName"].ToString();
                            }

                            string dbValue21 = row["BankName"].ToString();
                            if (!string.IsNullOrEmpty(dbValue) && registeredOfficeState.Items.FindByValue(dbValue) != null)
                            {

                                bankName.SelectedValue = dbValue21;
                            }
                            else
                            {
                                bankName.ClearSelection();
                            }
                            string bankCodeFromIfsc = new string(row["IFSCCode"].ToString()
           .TakeWhile(c => !char.IsDigit(c))
           .ToArray());

                            bankName.ClearSelection();
                            System.Web.UI.WebControls.ListItem item = bankName.Items.FindByValue(bankCodeFromIfsc);
                            if (item != null)
                            {
                                item.Selected = true;
                            }
                            accountName.Text = row["AccountName"].ToString();
                            accountNumber.Text = row["AccountNumber"].ToString();
                            ifscCode.Text = row["IFSCCode"].ToString();
                            branchCode.Text = row["BranchCode"].ToString();
                            bankAddress.Text = row["BankAddress"].ToString();
                        }

                        DataTable dt = dBConnection.ExecuteQueryForDataTable("SELECT \"BusinessState\", \"GSTNumber\", \"AddressOfPlace\", \"GSTVendorClassification\" FROM tec_led1 WHERE \"Id\" = '" + getid + "' ");
                        if (dt.Rows.Count > 0)
                        {
                            List<BusinessDetails> businessDetailsList = dt.AsEnumerable()
            .Select(row => new BusinessDetails
            {
                BusinessState = row["BusinessState"].ToString(),
                GSTNumber = row["GSTNumber"].ToString(),
                AddressOfPlace = row["AddressOfPlace"].ToString(),
                GSTVendorClassification = row["GSTVendorClassification"].ToString()
            }).ToList();

                            ViewState["BusinessDetails"] = businessDetailsList;
                            gvProjectDetails.DataSource = businessDetailsList;
                            gvProjectDetails.DataBind();
                        }
                        DataTable dt1 = dBConnection.ExecuteQueryForDataTable("SELECT \"Name\", \"Designation\", \"Contact_No\", \"Email_ID\" FROM tec_led2 WHERE \"Id\" = '" + getid + "'");
                        if (dt1.Rows.Count > 0)
                        {
                            List<PartnerDetails> partnerDetailsList = dt1.AsEnumerable()
            .Select(row => new PartnerDetails
            {
                Name = row["Name"].ToString(),
                Designation = row["Designation"].ToString(),
                Contact_No = row["Contact_No"].ToString(),
                Email_ID = row["Email_ID"].ToString()
            }).ToList();

                            ViewState["PartnerDetails"] = partnerDetailsList;
                            gvPartners.DataSource = partnerDetailsList;
                            gvPartners.DataBind();
                        }
                        DataTable dt2 = dBConnection.ExecuteQueryForDataTable("SELECT \"Department\", \"Name\", \"Designation\", \"ContactNo\", \"Email\" FROM tec_led3 WHERE \"ID\" = '" + getid + "' ");
                        if (dt2.Rows.Count > 0)
                        {
                            List<OperationalContact> operationalContactsList = dt2.AsEnumerable()
             .Select(row => new OperationalContact
             {
                 Department = row["Department"].ToString(),
                 Name = row["Name"].ToString(),
                 Designation = row["Designation"].ToString(),
                 ContactNo = row["ContactNo"].ToString(),
                 Email = row["Email"].ToString()
             }).ToList();

                            gvOperationalContacts.DataSource = operationalContactsList;
                            gvOperationalContacts.DataBind();
                        }
                        DataTable dt3 = dBConnection.ExecuteQueryForDataTable("SELECT \"MaterialDescription\", \"HSNCode\", \"Brand\", \"Size\", \"Product\",\"TaxPercentage\" FROM tec_led4 WHERE \"Id\" = '" + getid + "' ");
                        if (dt3.Rows.Count > 0)
                        {
                            List<MajorGoodsService> majorGoodsList = dt3.AsEnumerable()
            .Select(row => new MajorGoodsService
            {
                Product = row["Product"].ToString(),
                Brand = row["Brand"].ToString(),
                Size = row["Size"].ToString(),
                MaterialDescription = row["MaterialDescription"].ToString(),
                HSNCode = row["HSNCode"].ToString(),

                TaxPercentage = row["TaxPercentage"].ToString()
            }).ToList();

                            ViewState["MajorGoodsService"] = majorGoodsList;
                            gvMajorGoods.DataSource = majorGoodsList;
                            gvMajorGoods.DataBind();
                        }
                        DataTable dt4 = dBConnection.ExecuteQueryForDataTable("SELECT \"CustomerName\" FROM tec_led5 WHERE \"ID\" = '" + getid + "' ");
                        if (dt4.Rows.Count > 0)
                        {
                            List<MajorCustomers> majorCustomersList = dt4.AsEnumerable()
            .Select(row => new MajorCustomers
            {
                CustomerName = row["CustomerName"].ToString()
            }).ToList();

                            ViewState["MajorCustomer"] = majorCustomersList;
                            gvMajorCustomers.DataSource = majorCustomersList;
                            gvMajorCustomers.DataBind();
                        }
                        DataTable dt5 = dBConnection.ExecuteQueryForDataTable("SELECT  \"Description\",\"TextMode\" FROM tec_led6 WHERE \"ID\" = '" + getid + "' ");
                        if (dt5.Rows.Count > 0)
                        {
                            List<OtherInformation> otherInformationList = dt5.AsEnumerable()
            .Select(row => new OtherInformation
            {
                Description = row["Description"].ToString(),
                TextMode = row["TextMode"].ToString()
            }).ToList();

                            gvOtherInformation.DataSource = otherInformationList;
                            gvOtherInformation.DataBind();
                        }
                    }
                    else
                    {
                        Response.Write("No records found for the provided GSTNo.");
                    }
                    int j = 1;

                    for (int i = 0; i < gvKYCDocuments.Rows.Count; i++)
                    {
                        string DocumentType = string.Empty;
                        if (i == 0) DocumentType = "PAN Card";
                        if (i == 1) DocumentType = "GST Certificate";
                        if (i == 2) DocumentType = "Bank Account";
                        if (i == 3) DocumentType = "MSME Certificate";

                        GridViewRow row = gvKYCDocuments.Rows[i];

                        Label lblDocumentName = (Label)row.FindControl("DocumentName");

                        if (lblDocumentName != null)
                        {
                            string documentName = dBConnection.GetSingleValue("SELECT \"DocumentName\" FROM tec_led7 WHERE \"Id\"='" + getid + "' AND \"LineId\"='" + j + "'");
                            string base64File = dBConnection.GetSingleValue("SELECT \"FileData\" FROM tec_led7 WHERE \"Id\"='" + getid + "' AND \"LineId\"='" + j + "'");

                            if (!string.IsNullOrEmpty(documentName))
                            {
                                lblDocumentName.Text = documentName;
                                string sessionKeyPath = "Path_" + DocumentType;
                                if (!string.IsNullOrEmpty(base64File))
                                    Session[sessionKeyPath] = base64File;
                            }
                            else
                            {
                                lblDocumentName.Text = "No Document Found";
                            }
                        }

                        j++;
                    }






                    string query1 = "SELECT \"LineId\", \"DocumentType\", \"DocumentName\", \"FileData\" " +
                "FROM tec_led7 WHERE \"Id\" = '" + getid + "' AND \"LineId\" >= 5 ORDER BY \"LineId\"";
                    DataTable dtDocs = dBConnection.ExecuteQueryForDataTable(query1);

                    if (dtDocs.Rows.Count == 0)
                    {
                        DataRow newRow = dtDocs.NewRow();
                        newRow["LineId"] = DBNull.Value;
                        newRow["DocumentType"] = "Performa Invoice";
                        newRow["DocumentName"] = "";
                        newRow["FileData"] = DBNull.Value;
                        dtDocs.Rows.Add(newRow);
                    }

                    GridView1.DataSource = dtDocs;
                    GridView1.DataBind();





                    List<DocumentDetail> documentList = new List<DocumentDetail>();

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        GridViewRow row = GridView1.Rows[i];
                        Label lblDocumentName = (Label)row.FindControl("DocumentName");

                        if (dtDocs.Rows.Count > i)
                        {
                            string documentName = dtDocs.Rows[i]["DocumentName"].ToString();
                            string base64File = dtDocs.Rows[i]["FileData"].ToString();
                            string documentType = dtDocs.Rows[i]["DocumentType"].ToString();

                            lblDocumentName.Text = string.IsNullOrEmpty(documentName) ? "DocName" : documentName;
                            lblDocumentName.ForeColor = System.Drawing.Color.Black;

                            string sessionKeyPath = "Path_" + documentType + "_" + i;
                            Session[sessionKeyPath] = base64File;

                            documentList.Add(new DocumentDetail
                            {
                                DocumentType = documentType,
                                DocumentName = lblDocumentName.Text
                            });
                        }
                        else
                        {
                            lblDocumentName.Text = "DocName";
                            lblDocumentName.ForeColor = System.Drawing.Color.Black;

                            documentList.Add(new DocumentDetail
                            {
                                DocumentType = "Performa Invoice",
                                DocumentName = lblDocumentName.Text
                            });
                        }
                    }

                    ViewState["DocumentDetails"] = documentList;


                }
            }
        }
        protected void ValidateMobileNumber()
        {
            string mobileNumber = mobileNo.Text.Trim();

            if (mobileNumber.Length != 10 || !mobileNumber.All(char.IsDigit) && mobileNumber.Length != 0)
            {

                return;
            }


        }


        private void BindKYCGrid1()
        {
            DataTable dtKYC = new DataTable();
            dtKYC.Columns.Add("DocumentType");
            dtKYC.Columns.Add("FileData");

            dtKYC.Rows.Add("PAN Card", "<Base64EncodedFileDataForPanCard>");
            dtKYC.Rows.Add("GST Certificate", "<Base64EncodedFileDataForGST>");
            dtKYC.Rows.Add("Bank Account", "<Base64EncodedFileDataForBankAccount>");
            dtKYC.Rows.Add("MSME Certificate", "<Base64EncodedFileDataForMSMECertificate>");
            gvKYCDocuments.DataSource = dtKYC;
            gvKYCDocuments.DataBind();
        }

        private void BindKYCGrid11()
        {
            DataTable dtKYC = new DataTable();
            dtKYC.Columns.Add("DocumentType");
            dtKYC.Columns.Add("FileData");
            dtKYC.Rows.Add("Performa Invoice", "<Base64EncodedFileDataForPerformaInvoice>");
            GridView1.DataSource = dtKYC;
            GridView1.DataBind();
        }



        protected void btnView_Click(object sender, EventArgs e)
        {

            string getid2 = dBConnection.GetSingleValue("select ifnull(\"Id\",0) from tec_oled where \"GstNo\"='" + GSTNumber.Text.Trim() + "' ");
            if (getid2 != "")
            {
                string CheckDraft = dBConnection.GetSingleValue("Select \"Draft\" from tec_oled where \"Id\"='" + getid2 + "'");
                if (CheckDraft == "Y")
                {
                    LinkButton btn1 = (LinkButton)sender;
                    string documentType1 = btn1.CommandArgument;
                    string filePathFromDb = dBConnection.GetSingleValue("select \"FileData\" from tec_led7 where \"Id\"='" + getid2 + "' and \"DocumentType\"='" + documentType1 + "' ");

                    if (!string.IsNullOrEmpty(filePathFromDb) && File.Exists(filePathFromDb))
                    {
                        byte[] fileBytes = File.ReadAllBytes(filePathFromDb);
                        string fileType = GetFileTypeFromExtension(filePathFromDb);
                        string fileName = Path.GetFileName(filePathFromDb);
                        string tempFilePath = Server.MapPath("~/TempFiles/" + fileName);

                        File.WriteAllBytes(tempFilePath, fileBytes);
                        string sessionPathKey1 = "";
                        string sessionBase64Key1 = "";
                        string sessionFileNameKey1 = "";
                        Session[sessionPathKey1] = tempFilePath;
                        Session[sessionBase64Key1] = Convert.ToBase64String(fileBytes);
                        Session[sessionFileNameKey1] = fileName;
                        Session[sessionPathKey1] = tempFilePath;
                        Session[sessionBase64Key1] = Convert.ToBase64String(fileBytes);
                        Session[sessionFileNameKey1] = fileName;



                        OpenFileInNewTab(tempFilePath);

                    }
                    return;
                }
            }
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            string documentType = btn.CommandArgument;
            FileUpload fileUploadControl = row.FindControl("fileUpload1") as FileUpload;
            Label lblDocName = row.FindControl("DocumentName") as Label;

            string sessionPathKey = "Path_" + documentType;
            string sessionBase64Key = "base64_" + documentType;
            string sessionFileNameKey = "FileName_" + documentType;

            if (fileUploadControl != null && fileUploadControl.HasFile)
            {
                string fileExt = Path.GetExtension(fileUploadControl.FileName).ToLower();
                string tempFolder = Server.MapPath("~/TempFiles/");
                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                string tempPath = Path.Combine(tempFolder, "temp_" + Guid.NewGuid().ToString("N") + fileExt);
                fileUploadControl.SaveAs(tempPath);

                byte[] fileBytes = File.ReadAllBytes(tempPath);
                string base64File = Convert.ToBase64String(fileBytes);

                Session[sessionPathKey] = tempPath;
                Session[sessionBase64Key] = base64File;
                Session[sessionFileNameKey] = fileUploadControl.FileName;

                if (lblDocName != null)
                {
                    lblDocName.Text = fileUploadControl.FileName;
                    lblDocName.ForeColor = System.Drawing.Color.Green;
                }

                OpenFileInNewTab(tempPath);
                return;
            }

            if (Session[sessionPathKey] != null && File.Exists(Session[sessionPathKey].ToString()))
            {
                string tempPath = Session[sessionPathKey].ToString();


                byte[] fileBytes;
                try
                {
                    if (System.IO.File.Exists(tempPath))
                    {
                        fileBytes = System.IO.File.ReadAllBytes(tempPath);
                    }
                    else
                    {
                        fileBytes = Convert.FromBase64String(tempPath);
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "alert('Invalid Base64 file data.');", true);
                    return;
                }

                string fileExtension = DetectFileType(fileBytes);
                if (string.IsNullOrEmpty(fileExtension))
                    fileExtension = "pdf";

                string tempFolder = Server.MapPath("~/TempFiles/");
                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);

                string uniqueFileName = $"{documentType}_{Guid.NewGuid():N}.{fileExtension}";
                string tempFilePath = Path.Combine(tempFolder, uniqueFileName);
                File.WriteAllBytes(tempFilePath, fileBytes);

                string fileUrl = ResolveUrl("~/TempFiles/" + uniqueFileName);
                string script1 = $"window.open('{fileUrl}', '_blank');";
                Page.ClientScript.RegisterStartupScript(GetType(), "OpenFile", script1, true);

                return;
            }

            string getid1 = dBConnection.GetSingleValue("select \"Id\" from tec_oled where \"GstNo\"='" + GSTNumber.Text.Trim() + "' ");
            if (getid1 != "")
            {
                string fileDataBase64 = dBConnection.GetSingleValue("select \"FileData\" from tec_led7 where \"Id\"='" + getid1 + "' and \"DocumentType\"='" + documentType + "' ");

                if (!string.IsNullOrEmpty(fileDataBase64))
                {
                    byte[] fileBytes = Convert.FromBase64String(fileDataBase64);
                    string fileType = GetFileType(fileDataBase64);
                    string fileName = fileType == "pdf" ? "tempDocument.pdf" : "tempImage.jpg";
                    string filePath = Server.MapPath("~/TempFiles/" + fileName);

                    File.WriteAllBytes(filePath, fileBytes);

                    Session[sessionPathKey] = filePath;
                    Session[sessionBase64Key] = fileDataBase64;
                    Session[sessionFileNameKey] = fileName;

                    if (lblDocName != null)
                    {
                        lblDocName.Text = fileName;
                        lblDocName.ForeColor = System.Drawing.Color.Green;
                    }

                    OpenFileInNewTab(filePath);
                }
                return;
            }
            string script = "alert('Please Upload the File First.');";
            ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);
            return;
        }































        private string DetectFileType(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length < 4)
                return string.Empty;

            if (fileBytes[0] == 0x25 && fileBytes[1] == 0x50 && fileBytes[2] == 0x44 && fileBytes[3] == 0x46)
                return "pdf";

            if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8)
                return "jpg";

            if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
                return "png";

            if (fileBytes[0] == 0x47 && fileBytes[1] == 0x49 && fileBytes[2] == 0x46)
                return "gif";

            return string.Empty;
        }

        protected void btnView_Click1(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                int rowIndex = row.RowIndex;

                string documentType = btn.CommandArgument;
                string rowKeySuffix = "_" + rowIndex;

                string sessionPathKey = "Path_" + documentType + rowKeySuffix;
                string sessionBase64Key = "base64_" + documentType + rowKeySuffix;
                string sessionFileNameKey = "FileName_" + documentType + rowKeySuffix;

                FileUpload fileUploadControl = row.FindControl("fileUpload1") as FileUpload;
                Label lblDocName = row.FindControl("DocumentName") as Label;

                if (fileUploadControl != null && fileUploadControl.HasFile)
                {
                    string fileExt = Path.GetExtension(fileUploadControl.FileName).ToLower();
                    string tempFolder = Server.MapPath("~/TempFiles/");
                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }

                    string tempPath = Path.Combine(tempFolder, "temp_" + Guid.NewGuid().ToString("N") + fileExt);
                    fileUploadControl.SaveAs(tempPath);

                    byte[] fileBytes = File.ReadAllBytes(tempPath);
                    string base64File = Convert.ToBase64String(fileBytes);

                    Session[sessionPathKey] = tempPath;
                    Session[sessionBase64Key] = base64File;
                    Session[sessionFileNameKey] = fileUploadControl.FileName;

                    if (lblDocName != null)
                    {
                        lblDocName.Text = fileUploadControl.FileName;
                        lblDocName.ForeColor = System.Drawing.Color.Green;
                    }

                    OpenFileInNewTab(tempPath);
                    return;
                }

                if (Session[sessionPathKey] != null && File.Exists(Session[sessionPathKey].ToString()))
                {
                    string tempPath = Session[sessionPathKey].ToString();



                    byte[] fileBytes;
                    try
                    {
                        if (System.IO.File.Exists(tempPath))
                        {
                            fileBytes = System.IO.File.ReadAllBytes(tempPath);
                        }
                        else
                        {
                            fileBytes = Convert.FromBase64String(tempPath);
                        }
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", "alert('Invalid Base64 file data.');", true);
                        return;
                    }

                    string fileExtension = DetectFileType(fileBytes);
                    if (string.IsNullOrEmpty(fileExtension))
                        fileExtension = "pdf";

                    string tempFolder = Server.MapPath("~/TempFiles/");
                    if (!Directory.Exists(tempFolder))
                        Directory.CreateDirectory(tempFolder);

                    string uniqueFileName = $"{documentType}_{Guid.NewGuid():N}.{fileExtension}";
                    string tempFilePath = Path.Combine(tempFolder, uniqueFileName);
                    File.WriteAllBytes(tempFilePath, fileBytes);

                    string fileUrl = ResolveUrl("~/TempFiles/" + uniqueFileName);
                    string script1 = $"window.open('{fileUrl}', '_blank');";
                    Page.ClientScript.RegisterStartupScript(GetType(), "OpenFile", script1, true);
                    return;
                }

                string getid = dBConnection.GetSingleValue(
                    "select \"Id\" from tec_oled where \"GstNo\"='" + GSTNumber.Text.Trim() + "' ");

                if (!string.IsNullOrEmpty(getid))
                {
                    string fileDataBase64 = dBConnection.GetSingleValue(
                        "select \"FileData\" from tec_led7 where \"Id\"='" + getid + "' and \"DocumentType\"='" + documentType + "' ");

                    if (!string.IsNullOrEmpty(fileDataBase64))
                    {
                        fileDataBase64 = fileDataBase64.Trim();
                        if (fileDataBase64.Contains(","))
                            fileDataBase64 = fileDataBase64.Substring(fileDataBase64.IndexOf(",") + 1);

                        fileDataBase64 = fileDataBase64.Replace(" ", "")
                                                       .Replace("\r", "")
                                                       .Replace("\n", "");

                        if (!IsBase64String(fileDataBase64))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "InvalidFileAlert",
                                "alert('The file data is invalid or not in base64 format. Please upload again.');", true);
                            return;
                        }

                        byte[] fileBytes = Convert.FromBase64String(fileDataBase64);
                        string fileType = GetFileType(fileDataBase64);
                        string fileName = fileType == "pdf" ? "tempDocument.pdf" : "tempImage.jpg";
                        string filePath = Server.MapPath("~/TempFiles/" + fileName);

                        File.WriteAllBytes(filePath, fileBytes);

                        Session[sessionPathKey] = filePath;
                        Session[sessionBase64Key] = fileDataBase64;
                        Session[sessionFileNameKey] = fileName;

                        if (lblDocName != null)
                        {
                            lblDocName.Text = fileName;
                            lblDocName.ForeColor = System.Drawing.Color.Green;
                        }

                        OpenFileInNewTab(filePath);
                        return;
                    }
                }

                ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert",
                    "alert('Please upload the file first.');", true);
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }
        }

        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return false;

            base64 = base64.Trim()
                           .Replace(" ", "")
                           .Replace("\r", "")
                           .Replace("\n", "");

            if (base64.Length % 4 != 0)
                return false;

            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetFileTypeFromExtension(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".pdf":
                    return "pdf";
                case ".jpg":
                case ".jpeg":
                    return "jpg";
                case ".png":
                    return "png";
                default:
                    return "unknown";
            }
        }

        private void OpenFileInNewTab(string filePath)
        {
            string fileName = Path.GetFileName(filePath).Replace("'", "\\'");
            string fileUrl = ResolveClientUrl("~/TempFiles/" + fileName);

            string script = $@"
        var win = window.open('{fileUrl}', '_blank');
        if (!win) {{
            alert('Popup blocked! Please allow popups for this site.');
        }}
    ";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenFile", script, true);
        }




        private string GetFileDataByDocumentType(string documentType)
        {
            string getid1 = dBConnection.GetSingleValue("select ifnull(\"Id\",0) from tec_oled where \"GstNo\"='" + GSTNumber.Text.Trim() + "' ");
            if (getid1 == null || getid1 == string.Empty) getid1 = "0";
            return dBConnection.GetSingleValue("SELECT \"FileData\" FROM tec_led7 WHERE \"DocumentType\" = '" + documentType + "' and \"Id\"='" + getid1 + "'");
        }

        private string GetFileType(string documentType)
        {
            if (documentType.Length >= 4 && documentType.Substring(0, 4).Equals("JVBE", StringComparison.OrdinalIgnoreCase))
            {
                return "pdf";
            }

            return "image";
        }


        protected void btnDownload_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string documentType = btn.CommandArgument;

            FileUpload fileUploadControl = row.FindControl("fileUpload1") as FileUpload;
            string sessionTempPathKey = "TempFilePath_" + documentType;
            string sessionViewPathKey = "Path_" + documentType;
            string sessionFileNameKey = "FileName_" + documentType;

            if (fileUploadControl != null && fileUploadControl.HasFile)
            {
                byte[] fileBytes = fileUploadControl.FileBytes;
                string fileName = fileUploadControl.FileName;
                string contentType = GetContentType(Path.GetExtension(fileName));

                string tempFolder = Server.MapPath("~/TempFiles/");
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);
                string tempPath = Path.Combine(tempFolder, "temp_" + Guid.NewGuid().ToString("N") + Path.GetExtension(fileName));
                File.WriteAllBytes(tempPath, fileBytes);

                Session[sessionTempPathKey] = tempPath;
                Session[sessionFileNameKey] = fileName;

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.BinaryWrite(fileBytes);
                Response.End();
                return;
            }

            string viewPath = Session[sessionViewPathKey] as string;
            if (!string.IsNullOrEmpty(viewPath) && File.Exists(viewPath))
            {
                string fileName = Session[sessionFileNameKey] != null
                                    ? Session[sessionFileNameKey].ToString()
                                    : Path.GetFileName(viewPath);
                string contentType = GetContentType(Path.GetExtension(viewPath));
                byte[] fileBytes = File.ReadAllBytes(viewPath);

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.BinaryWrite(fileBytes);
                Response.End();
                return;
            }

            string tempPathSaved = Session[sessionTempPathKey] as string;
            if (!string.IsNullOrEmpty(tempPathSaved) && File.Exists(tempPathSaved))
            {
                string fileName = Path.GetFileName(tempPathSaved);
                string contentType = GetContentType(Path.GetExtension(fileName));
                byte[] fileBytes = File.ReadAllBytes(tempPathSaved);

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.BinaryWrite(fileBytes);
                Response.End();
                return;
            }

            string fileDataBase64 = GetFileDataByDocumentType(documentType);
            if (!string.IsNullOrEmpty(fileDataBase64))
            {
                byte[] fileBytes = Convert.FromBase64String(fileDataBase64);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + documentType + ".pdf");
                Response.BinaryWrite(fileBytes);
                Response.End();
                return;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "nofile", "alert('No file available to download.');", true);
        }


        protected void btnDownload_Click1(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int rowIndex = row.RowIndex;

            string documentType = btn.CommandArgument;
            string rowKeySuffix = "_" + rowIndex;

            string sessionPathKey = "Path_" + documentType + rowKeySuffix;
            string sessionBase64Key = "base64_" + documentType + rowKeySuffix;
            string sessionFileNameKey = "FileName_" + documentType + rowKeySuffix;

            FileUpload fileUploadControl = row.FindControl("fileUpload1") as FileUpload;

            if (fileUploadControl != null && fileUploadControl.HasFile)
            {
                byte[] fileBytes = fileUploadControl.FileBytes;
                string fileName = fileUploadControl.FileName;
                string contentType = GetContentType(Path.GetExtension(fileName));

                string tempFolder = Server.MapPath("~/TempFiles/");
                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);

                string tempPath = Path.Combine(tempFolder, "temp_" + Guid.NewGuid().ToString("N") + Path.GetExtension(fileName));
                File.WriteAllBytes(tempPath, fileBytes);

                Session[sessionPathKey] = tempPath;
                Session[sessionBase64Key] = Convert.ToBase64String(fileBytes);
                Session[sessionFileNameKey] = fileName;

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.BinaryWrite(fileBytes);
                Response.End();
                return;
            }

            if (Session[sessionPathKey] != null && File.Exists(Session[sessionPathKey].ToString()))
            {
                string filePath = Session[sessionPathKey].ToString();
                string fileName = Session[sessionFileNameKey]?.ToString() ?? Path.GetFileName(filePath);
                string contentType = GetContentType(Path.GetExtension(fileName));
                byte[] fileBytes = File.ReadAllBytes(filePath);

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.BinaryWrite(fileBytes);
                Response.End();
                return;
            }

            if (Session[sessionBase64Key] != null)
            {
                byte[] fileBytes = Convert.FromBase64String(Session[sessionBase64Key].ToString());
                string fileName = Session[sessionFileNameKey]?.ToString() ?? (documentType + ".pdf");

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.BinaryWrite(fileBytes);
                Response.End();
                return;
            }

            string getid = dBConnection.GetSingleValue(
                "select \"Id\" from tec_oled where \"GstNo\"='" + GSTNumber.Text.Trim() + "' ");

            if (!string.IsNullOrEmpty(getid))
            {
                string fileDataBase64 = dBConnection.GetSingleValue(
                    "select \"FileData\" from tec_led7 where \"Id\"='" + getid + "' and \"DocumentType\"='" + documentType + "' ");

                if (!string.IsNullOrEmpty(fileDataBase64))
                {
                    byte[] fileBytes = Convert.FromBase64String(fileDataBase64);
                    string fileName = documentType + ".pdf";

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                    Response.BinaryWrite(fileBytes);
                    Response.End();
                    return;
                }
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "nofile", "alert('No file available to download.');", true);
        }

        private string GetContentType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf": return "application/pdf";
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                default: return "application/octet-stream";
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://www.google.com");
        }
        protected void gvOperationalContacts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as OperationalContact;

                if (dataItem != null)
                {
                    var txtName = (TextBox)e.Row.FindControl("pocName");
                    if (txtName != null)
                    {
                        txtName.Text = dataItem.Name;
                    }

                    var txtDesignation = (TextBox)e.Row.FindControl("pocDesignation");
                    if (txtDesignation != null)
                    {
                        txtDesignation.Text = dataItem.Designation;
                    }

                    var txtContactNo = (TextBox)e.Row.FindControl("pocContactNo");
                    if (txtContactNo != null)
                    {
                        txtContactNo.Text = dataItem.ContactNo;
                    }

                    var txtEmail = (TextBox)e.Row.FindControl("pocEmail");
                    if (txtEmail != null)
                    {
                        txtEmail.Text = dataItem.Email;
                    }
                }
            }
        }


        public class OtherInformation
        {
            public string Description { get; set; }
            public string TextMode { get; set; }
        }

        protected void gvOtherInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as OtherInformation;

                if (dataItem != null)
                {
                    var lblDescription = (Label)e.Row.FindControl("lblDescription");
                    if (lblDescription != null)
                    {
                        lblDescription.Text = dataItem.Description;
                    }

                    var txtValue = (TextBox)e.Row.FindControl("txtValue");
                    if (txtValue != null)
                    {
                        txtValue.Text = dataItem.TextMode;
                    }
                }
            }
        }
        private void InsertDocuments(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck, string IsUpdate)
        {
            string tradeNameText = tradeName.Text;
            string sServer = ConfigurationManager.AppSettings["Server"];
            string sDBUser = ConfigurationManager.AppSettings["DBUser"];
            string sDBPwd = ConfigurationManager.AppSettings["DBPwd"];
            string sDBName = ConfigurationManager.AppSettings["DBName"];
            string sConstr = "DRIVER={HDBODBC};UID=" + sDBUser + "PWD=" + sDBPwd + "DATABASENAME=NDB;SERVERNODE=" + sServer + "CS=" + sDBName + ";";
            string TName = tradeName.Text;
            DataTable Dtbl = (DataTable)ViewState["gvKYCDocuments"];

            int i = 1;
            foreach (GridViewRow row in gvKYCDocuments.Rows)
            {




                if (IsUpdate != "Y")
                {
                    string DocumentType = string.Empty;
                    if (i == 1) DocumentType = "PAN Card";
                    if (i == 2) DocumentType = "GST Certificate";
                    if (i == 3) DocumentType = "Bank Account";
                    if (i == 4) DocumentType = "MSME Certificate";
                    Label lblDocName = row.FindControl("DocumentName") as Label;
                    string sessionKey = lblDocName.Text;
                    string sessionKeyPath = "Path_" + DocumentType;
                    if (Session[sessionKeyPath].ToString() != null)
                    {

                        string base64File = Session[sessionKeyPath].ToString();

                        string query = "INSERT INTO tec_led7 (\"Id\",\"LineId\", \"TradeName\", \"DocumentName\", \"DocumentType\", \"FileData\") VALUES " +
                                       "('" + Id + "','" + i + "','" + tradeNameText.Trim() + "','" + sessionKey + "','" + DocumentType + "','" + base64File + "')";
                        dBConnection.ExecuteNonQuery(query);
                    }



                }
                else
                {
                    string DocumentType = string.Empty;
                    if (i == 1) DocumentType = "PAN Card";
                    if (i == 2) DocumentType = "GST Certificate";
                    if (i == 3) DocumentType = "Bank Account";
                    if (i == 4) DocumentType = "MSME Certificate";
                    Label lblDocName = row.FindControl("DocumentName") as Label;
                    string sessionKey = lblDocName.Text;
                    string sessionKeyPath = "Path_" + DocumentType;






                    if (Session[sessionKeyPath] != null && File.Exists(Session[sessionKeyPath].ToString()))
                    {
                        string filePath = Session[sessionKeyPath].ToString();
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        string base64File = Convert.ToBase64String(fileBytes);

                        lblDocName = row.FindControl("DocumentName") as Label;
                        string existingFileName = lblDocName != null ? lblDocName.Text.Trim() : "";

                        if (string.IsNullOrEmpty(existingFileName) && Session["FileName_" + DocumentType] != null)
                        {
                            existingFileName = Session["FileName_" + DocumentType].ToString();
                        }

                        string query = "UPDATE tec_led7 SET \"TradeName\" = '" + tradeNameText.Trim() +
                                       "', \"DocumentName\" = '" + existingFileName +
                                       "', \"DocumentType\" = '" + DocumentType +
                                       "', \"FileData\" = '" + filePath +
                                       "' WHERE \"Id\" = '" + Id + "' and \"LineId\"='" + i + "'";

                        dBConnection.ExecuteNonQuery(query);
                    }

                }

            }

            i++;

        }


        private void InsertDocuments1(HanaConnection connection, HanaTransaction transaction, int Id, bool isDraft, string draftcheck, string IsUpdate)
        {
            string tradeNameText = tradeName.Text.Trim();
            DataTable Dtbl = (DataTable)ViewState["gvKYCDocuments"];
            int i = 5;

            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow)
                    continue;

                FileUpload fileUpload = row.FindControl("fileUpload1") as FileUpload;
                Label lblDocName = row.FindControl("DocumentName") as Label;

                string DocumentType = "Performa Invoice";
                string DocumentName = lblDocName != null ? lblDocName.Text : "";

                string base64File = null;

                if (fileUpload != null && fileUpload.HasFile)
                {
                    using (var binaryReader = new System.IO.BinaryReader(fileUpload.PostedFile.InputStream))
                    {
                        base64File = Convert.ToBase64String(binaryReader.ReadBytes(fileUpload.PostedFile.ContentLength));
                    }
                    DocumentName = fileUpload.FileName;
                }
                else
                {
                    string sessionKeyPath = "Path_" + DocumentType + "_" + row.RowIndex;
                    if (Session[sessionKeyPath] != null)
                    {
                        base64File = Session[sessionKeyPath].ToString();
                    }
                }

                if (string.IsNullOrEmpty(base64File))
                {
                    i++;
                    continue;
                }



                string query;
                if (IsUpdate == "Y")
                {
                    query = $"UPDATE tec_led7 SET \"TradeName\"='{tradeNameText}', \"DocumentName\"='{DocumentName}', \"DocumentType\"='{DocumentType}', \"FileData\"='{base64File}' WHERE \"Id\"='{Id}' AND \"LineId\"='{i}'";
                }
                else
                {
                    query = $"INSERT INTO tec_led7 (\"Id\",\"LineId\",\"TradeName\",\"DocumentName\",\"DocumentType\",\"FileData\") VALUES ('{Id}','{i}','{tradeNameText}','{DocumentName}','{DocumentType}','{base64File}')";
                }

                using (var cmd = new HanaCommand(query, connection, transaction))
                {
                    cmd.ExecuteNonQuery();
                }



                i++;
            }
        }

        public class AddressFields
        {
            public string Building { get; set; }
            public string Street { get; set; }
            public string Locality { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string State { get; set; }
            public string Pincode { get; set; }
        }

        public class GstDetails
        {
            public string Legal_name { get; set; }
            public string Trade_name { get; set; }
            public string Gst_number { get; set; }
            public AddressFields address_in_7_separate_feilds { get; set; }
        }
        public class UdyamDetails
        {
            public string enterprice_type { get; set; }
            public string register_number { get; set; }
            public string major_activity { get; set; }
        }
        public class TechOTP
        {
            public string gstNumber { get; set; }

            public string Mobileno { get; set; }
            public string OTPMobileno { get; set; }
            public string OTP { get; set; }
            public string Creation { get; set; }
            public string ValidUntil { get; set; }
            public bool Verified { get; set; }
            public int ValidateOTP { get; set; }
            public string MESSAGE { get; set; }
        }

        public class ImageUploadModel
        {
            public string SerialNo { get; set; }
            public List<string> Images { get; set; }
        }
        public class PanDetails
        {
            public string pan_no { get; set; }
        }
        [Serializable]
        public class DocumentDetail
        {
            public string DocumentType { get; set; }
            public string DocumentName { get; set; }
        }

        private void InitializeGrid()
        {
            List<DocumentDetail> documentList = new List<DocumentDetail>
    {
        new DocumentDetail { DocumentType = "Performa Invoice", DocumentName = "" }
    };

            ViewState["DocumentDetails"] = documentList;
            BindGridView1();
        }

        private void BindGridView1()
        {
            List<DocumentDetail> documentList = ViewState["DocumentDetails"] as List<DocumentDetail>;
            if (documentList == null)
                documentList = new List<DocumentDetail>();

            GridView1.DataSource = documentList;
            GridView1.DataBind();
        }

        protected void lnknewrowadd_Click4(object sender, EventArgs e)
        {
            try
            {
                List<DocumentDetail> documentList = ViewState["DocumentDetails"] as List<DocumentDetail>;
                if (documentList == null)
                    documentList = new List<DocumentDetail>();

                foreach (GridViewRow row in GridView1.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        int index = row.DataItemIndex;

                        Label lblDocName = row.FindControl("DocumentName") as Label;
                        if (lblDocName != null)
                        {
                            lblDocName.Text = documentList[index].DocumentName;

                        }

                    }
                }

                documentList.Add(new DocumentDetail
                {
                    DocumentType = "Performa Invoice",
                    DocumentName = ""
                });

                ViewState["DocumentDetails"] = documentList;

                BindGridView1();
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }
        }










        protected void lnkDelete_Click4(object sender, EventArgs e)
        {
            try
            {
                List<DocumentDetail> documentList = ViewState["DocumentDetails"] as List<DocumentDetail>;
                if (documentList == null || documentList.Count == 0)
                    return;

                LinkButton btn = (LinkButton)sender;
                GridViewRow currentRow = (GridViewRow)btn.NamingContainer;
                int indexToDelete = currentRow.RowIndex;

                if (documentList.Count > 1)
                {
                    documentList.RemoveAt(indexToDelete);

                    string documentType = "Performa Invoice";
                    string rowKeySuffix = "_" + indexToDelete;

                    Session.Remove("Path_" + documentType + rowKeySuffix);
                    Session.Remove("FileName_" + documentType + rowKeySuffix);
                    Session.Remove("base64_" + documentType + rowKeySuffix);

                    ShiftSessionKeysAfterDelete(documentType, indexToDelete, documentList.Count);
                }
                else
                {
                    documentList[0].DocumentType = "Performa Invoice";
                    documentList[0].DocumentName = "";

                    string documentType = "Performa Invoice";
                    string rowKeySuffix = "_0";

                    Session.Remove("Path_" + documentType + rowKeySuffix);
                    Session.Remove("FileName_" + documentType + rowKeySuffix);
                    Session.Remove("base64_" + documentType + rowKeySuffix);
                }

                ViewState["DocumentDetails"] = documentList;
                BindGridView1();
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }
        }

        private void ShiftSessionKeysAfterDelete(string documentType, int deletedIndex, int totalRows)
        {
            for (int i = deletedIndex + 1; i <= totalRows; i++)
            {
                string oldSuffix = "_" + i;
                string newSuffix = "_" + (i - 1);

                if (Session["Path_" + documentType + oldSuffix] != null)
                {
                    Session["Path_" + documentType + newSuffix] = Session["Path_" + documentType + oldSuffix];
                    Session.Remove("Path_" + documentType + oldSuffix);
                }

                if (Session["FileName_" + documentType + oldSuffix] != null)
                {
                    Session["FileName_" + documentType + newSuffix] = Session["FileName_" + documentType + oldSuffix];
                    Session.Remove("FileName_" + documentType + oldSuffix);
                }

                if (Session["base64_" + documentType + oldSuffix] != null)
                {
                    Session["base64_" + documentType + newSuffix] = Session["base64_" + documentType + oldSuffix];
                    Session.Remove("base64_" + documentType + oldSuffix);
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string documentType = "Performa Invoice";
                string rowKeySuffix = "_" + e.Row.RowIndex;

                string sessionFileNameKey = "FileName_" + documentType + rowKeySuffix;

                var lbl = (Label)e.Row.FindControl("DocumentName");

                if (Session[sessionFileNameKey] != null && lbl != null)
                {
                    lbl.Text = Session[sessionFileNameKey].ToString();
                    lbl.ForeColor = System.Drawing.Color.Green;
                }
            }
        }


    }
}

