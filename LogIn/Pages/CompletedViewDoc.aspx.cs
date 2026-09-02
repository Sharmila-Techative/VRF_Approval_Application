using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogIn.Pages
{
    public partial class CompletedViewDoc : System.Web.UI.Page
    {
        DbConnection dBConnection = new DbConnection();
        SAPbobsCOM.Documents oBP = null;
        SAPbobsCOM.Items oItem = null;
        public SAPbobsCOM.Company p_oCompany;
        public string Type = WebConfigurationManager.AppSettings["ServerType"];


        protected void Page_Load(object sender, EventArgs e)
        {
            try { dBConnection.writeLog("CompletedViewDoc_Page_Load", "CompletedViewDoc loaded. IsPostBack: " + IsPostBack, "Debug"); } catch {}
            if (!IsPostBack)
            {
                popuptext.ReadOnly = false;
                popuptext.Enabled = true;
                popuptext.Attributes.Remove("readonly");
                ShowPage(1);
                BindOperationalContacts();
                BindOtherInformation();
                BindKYCGrid1();
                BindKYCGrid11();

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
            new MajorGoodsService { MaterialDescription = "", HSNCode = "", TaxPercentage = "" }

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


                if (Request.QueryString["GSTNumber"] != null)
                {
                    string gstNumber = Request.QueryString["GSTNumber"];
                    Session["GSTNumber"] = gstNumber;
                    if (Session["draftValue"] != null && Session["draftValue"].ToString() == "Draft")
                    {
                        if (Session["draftValue"].ToString() == "Draft")
                        {
                            string username = Session["username"].ToString();
                            DbConnection conn = new DbConnection();
                            string Department = conn.GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
                            string IsApprovalReq = conn.GetSingleValue("Call \"IsApprovalReq\"('" + username + "','" + Department + "') ");
                            if (IsApprovalReq == "Y")
                            {
                                btnDraftApproved.Visible = true;
                                btnCancel.Visible = true;
                            }
                            Approve.Style["visibility"] = "hidden";
                            btnReject.Style["visibility"] = "hidden";
                            Session["draftValue"] = "";
                        }

                    }
                    GSTNumber_TextChanged(gstNumber, EventArgs.Empty);
                }
            }
            else
            {

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
        protected void gvMajorCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as MajorCustomers;
                ((TextBox)e.Row.FindControl("customerName")).Text = dataItem?.CustomerName ?? string.Empty;
            }
        }
        protected void gvMajorGoods_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as MajorGoodsService;
                ((TextBox)e.Row.FindControl("txtProduct")).Text = dataItem?.Product ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtMaterialDescription")).Text = dataItem?.MaterialDescription ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtHSNCode")).Text = dataItem?.HSNCode ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtBrand")).Text = dataItem?.Brand ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtSize")).Text = dataItem?.Size ?? string.Empty;
                ((TextBox)e.Row.FindControl("txtTaxPercentage")).Text = dataItem?.TaxPercentage ?? string.Empty;
            }
        }
        [Serializable]
        public class DocumentDetail
        {
            public string DocumentType { get; set; }
            public string DocumentName { get; set; }
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

                    i++;

                }
            }

            int page = int.Parse(hfPageIndex.Value);
            ShowPage(page - 1);
            if (page - 1 == 0)
            {
                Response.Redirect("~/Pages/VendorForm.aspx");
            }
            else
            {
                ShowPage(page - 1);
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            int page = int.Parse(hfPageIndex.Value);
            page = int.Parse(hfPageIndex.Value);
            ShowPage(page + 1);

            if (page + 1 == 8)
            {
                Response.Redirect("~/Pages/VendorForm.aspx");
            }



        }
        private void UpdatePage(int pageIndex)
        {
            int currentPage = int.Parse(hfPageIndex.Value); ;


            pnlApprovalButtons.Visible = (currentPage == 3);


        }
        private void UpdatePage1()
        {
            int currentPage = int.Parse(hfPageIndex.Value); ;


            pnlApprovalButtons.Visible = (currentPage == 3);


        }
        private void ShowPage(int pageIndex)
        {
            pnlPage1.Visible = pageIndex == 1;
            pnlPage2.Visible = pageIndex == 2;
            pnlPage3.Visible = pageIndex == 3;
            pnlPage4.Visible = pageIndex == 4;
            pnlPage5.Visible = pageIndex == 5;
            pnlPage6.Visible = pageIndex == 6;
            pnlPage7.Visible = pageIndex == 7;
            pnlApprovalButtons.Visible = (pageIndex == 7);

            btnPrevious.Visible = pageIndex > 1;
            btnNext.Visible = pageIndex < 7;

            hfPageIndex.Value = pageIndex.ToString();
            if (pageIndex == 1)
            {
                btnPrevious.Visible = false;

            }
            if (pageIndex == 7)
            {
                btnNext.Visible = false;
            }
        }
        protected void GSTNumber_TextChanged(String gstNumber, EventArgs e)
        {
            GSTNumber.Text = gstNumber;
            if (!string.IsNullOrEmpty(gstNumber))
            {
                if (gstNumber != null && gstNumber.Length == 15)
                {
                    string PanNumber = gstNumber.Substring(2, 10);
                    PANNumber.Text = PanNumber;
                    Session["PAN"] = PanNumber;
                }
                string getid;
                if (Type == "HANA")
                    getid = dBConnection.GetSingleValue("select \"Id\" from tec_oled where \"GstNo\"='" + gstNumber.Trim() + "' ");
                else
                    getid = dBConnection.SQL_GetSingleValue("select \"Id\" from tec_oled where \"GstNo\"='" + gstNumber.Trim() + "' ");

                if (!string.IsNullOrEmpty(getid))
                {
                    int FindId = Convert.ToInt32(getid);

                    string query = "SELECT \"ContactPerson\",\"TName\", \"Raddress1\", \"Raddress2\", \"Raddress3\",\"Gaddress1\", \"Gaddress2\", \"Gaddress3\",\"Gcountry\", \"Gstate\",\"Gzipcode\",\"Gcity\",\"registeredOfficeCity\",\"businessBillingCity\", " +
                   "\"Rcountry\", \"Rstate\", \"Rzipcode\", \"Baddress1\", \"Baddress2\", \"Baddress3\", \"Bcountry\", \"Bstate\", \"Bzipcode\", " +
                   "\"NatureOfBusinessActivity\",\"DateOfEstablishment\",\"ContactPersonName\",\"Designation\",\"EmailId\",\"MobileNo\",\"OfficeTelephoneNo\"," +
                   "\"TANNo\",\"MsmeRegistrationStatus\",\"BankName\",\"AccountName\",\"AccountNumber\",\"IfscCode\",\"BranchCode\",\"BankAddress\",\"DeclarationName\",\"DeclarationDesignation\",\"EnterpriseType\",\"BusinessType\",\"AgencyEmail\",\"AgencyName\",\"MSMENo\" " +
                   "FROM tec_oled WHERE \"Id\" = '" + getid + "'";

                    string Query1 = "SELECT \"CreditDays\",\"DisCount\",\"MarkDownTax0\",\"MarkDownWithoutTax0\",\"MarkDownTax3\",\"MarkDownWithoutTax3\",\"MarkDownTax5\",\"MarkDownWithoutTax5\",\"MarkDownTax18\",\"MarkDownWithoutTax18\" FROM \"PaymentDetails\" WHERE \"Id\" = '" + getid + "'";

                    DataTable data;
                    if (Type == "HANA")
                        data = dBConnection.ExecuteQueryForDataTable(query);
                    else
                        data = dBConnection.SQL_ExecuteQueryForDataTable(query);

                    DataTable data1;
                    if (Type == "HANA")
                        data1 = dBConnection.ExecuteQueryForDataTable(Query1);
                    else
                        data1 = dBConnection.SQL_ExecuteQueryForDataTable(Query1);

                    if (data.Rows.Count > 0)
                    {
                        DataRow row = data.Rows[0];

                        ddpartnertype.Text = "Vendor";

                        tradeName.Text = row["TName"].ToString();
                        ContactPerson.Text = row["ContactPerson"].ToString();
                        ddlEnterpriseType.Text = row["EnterpriseType"].ToString();
                        BusinessType.Text = row["BusinessType"].ToString();
                        if (row["BusinessType"].ToString() == "Agency")
                        {

                            AgencyEmail.Style["visibility"] = "visible";
                            AgencyName.Style["visibility"] = "visible";
                        }
                        AgencyEmail.Text = row["AgencyEmail"].ToString();
                        AgencyName.Text = row["AgencyName"].ToString();
                        MSMENO.Text = row["MSMENo"].ToString();

                        string country = dBConnection.GetSingleValue("call \"TEC_GetCountryByID\"('" + row["Rcountry"].ToString() + "')");
                        string State = dBConnection.GetSingleValue("call \"TEC_GetStateByID\"('" + row["Rstate"].ToString() + "')");

                        registeredOfficeAddress1.Text = row["Raddress1"].ToString();
                        registeredOfficeAddress2.Text = row["Raddress2"].ToString();
                        registeredOfficeAddress3.Text = row["Raddress3"].ToString();
                        registeredOfficeCountry.Text = country;
                        registeredOfficeState.Text = State;
                        registeredOfficeZipCode.Text = row["Rzipcode"].ToString();
                        country = dBConnection.GetSingleValue("call \"TEC_GetCountryByID\"('" + row["Gcountry"].ToString() + "')");
                        State = dBConnection.GetSingleValue("call \"TEC_GetStateByID\"('" + row["Gstate"].ToString() + "')");

                        goodsReturnAddress1.Text = row["Gaddress1"].ToString();
                        goodsReturnAddress2.Text = row["Gaddress2"].ToString();
                        goodsReturnAddress3.Text = row["Gaddress3"].ToString();
                        goodsReturnCountry.Text = country;
                        goodsReturnState.Text = State;
                        goodsReturnZipcode.Text = row["Gzipcode"].ToString();
                        if (data1.Rows.Count > 0)
                        {
                            DataRow row1 = data1.Rows[0];
                            CreditDays.Text = row1["CreditDays"].ToString();
                            DisCount.Text = row1["DisCount"].ToString();
                            Payment1.Text = row1["MarkDownTax0"].ToString();
                            Payment2.Text = row1["MarkDownWithoutTax0"].ToString();
                            Payment3.Text = row1["MarkDownTax3"].ToString();
                            Payment4.Text = row1["MarkDownWithoutTax3"].ToString();
                            Payment5.Text = row1["MarkDownTax5"].ToString();
                            Payment6.Text = row1["MarkDownWithoutTax5"].ToString();
                            Payment9.Text = row1["MarkDownTax18"].ToString();
                            Payment10.Text = row1["MarkDownWithoutTax18"].ToString();
                        }
                        country = dBConnection.GetSingleValue("call \"TEC_GetCountryByID\"('" + row["BCountry"].ToString() + "')");
                        State = dBConnection.GetSingleValue("call \"TEC_GetStateByID\"('" + row["BState"].ToString() + "')");
                        businessBillingAddress1.Text = row["BAddress1"].ToString();
                        businessBillingAddress2.Text = row["BAddress2"].ToString();
                        businessBillingAddress3.Text = row["BAddress3"].ToString();
                        businessBillingCountry.Text = country;
                        businessBillingState.Text = State;
                        businessBillingZipCode.Text = row["BZipCode"].ToString();
                        natureOfBusinessActivity.Text = row["NatureOfBusinessActivity"].ToString();
                        dateOfEstablishment.Text = row["DateOfEstablishment"].ToString();
                        contactPersonName.Text = row["ContactPersonName"].ToString();
                        designation.Text = row["Designation"].ToString();

                        declarationName.Text = row["declarationName"].ToString();
                        declarationDesignation.Text = row["DeclarationDesignation"].ToString();
                        emailId.Text = row["EmailId"].ToString();
                        mobileNo.Text = row["MobileNo"].ToString();
                        officeTelephoneNo.Text = row["OfficeTelephoneNo"].ToString();
                        tanNo.Text = row["TANNo"].ToString();
                        msmeRegistrationStatus.Text = row["MSMERegistrationStatus"].ToString();
                        string Bank = dBConnection.GetSingleValue("call \"TEC_GetBankByID\"('" + row["BankName"].ToString() + "')");
                        bankName.Text = Bank;
                        accountName.Text = row["AccountName"].ToString();
                        accountNumber.Text = row["AccountNumber"].ToString();
                        ifscCode.Text = row["IFSCCode"].ToString();
                        branchCode.Text = row["BranchCode"].ToString();
                        bankAddress.Text = row["BankAddress"].ToString();
                    }

                    DataTable dt;
                    if (Type == "HANA")
                        dt = dBConnection.ExecuteQueryForDataTable("SELECT \"BusinessState\", \"GSTNumber\", \"AddressOfPlace\", \"GSTVendorClassification\" FROM tec_led1 WHERE \"Id\" = '" + getid + "'");
                    else
                        dt = dBConnection.SQL_ExecuteQueryForDataTable("SELECT \"BusinessState\", \"GSTNumber\", \"AddressOfPlace\", \"GSTVendorClassification\" FROM tec_led1 WHERE \"Id\" = '" + getid + "'");
                    if (dt.Rows.Count > 0)
                    {
                        List<BusinessDetails> businessDetailsList = dt.AsEnumerable()
        .Select(row => new BusinessDetails
        {
            BusinessState = dBConnection.GetSingleValue("call \"TEC_GetStateByID\"('" + row["BusinessState"].ToString() + "')"),
            GSTNumber = row["GSTNumber"].ToString(),
            AddressOfPlace = row["AddressOfPlace"].ToString(),
            GSTVendorClassification = row["GSTVendorClassification"].ToString()
        }).ToList();

                        gvProjectDetails.DataSource = businessDetailsList;
                        gvProjectDetails.DataBind();
                    }
                    DataTable dt1;
                    if (Type == "HANA")
                        dt1 = dBConnection.ExecuteQueryForDataTable("SELECT \"Name\", \"Designation\", \"Contact_No\", \"Email_ID\" FROM tec_led2 WHERE \"Id\" = '" + getid + "' ");
                    else
                        dt1 = dBConnection.SQL_ExecuteQueryForDataTable("SELECT \"Name\", \"Designation\", \"Contact_No\", \"Email_ID\" FROM tec_led2 WHERE \"Id\" = '" + getid + "' ");
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
                        gvPartners.DataSource = partnerDetailsList;
                        gvPartners.DataBind();
                    }
                    DataTable dt2;
                    if (Type == "HANA")
                        dt2 = dBConnection.ExecuteQueryForDataTable("SELECT \"Department\", \"Name\", \"Designation\", \"ContactNo\", \"Email\" FROM tec_led3 WHERE \"ID\" = '" + getid + "'  ");
                    else
                        dt2 = dBConnection.SQL_ExecuteQueryForDataTable("SELECT \"Department\", \"Name\", \"Designation\", \"ContactNo\", \"Email\" FROM tec_led3 WHERE \"ID\" = '" + getid + "'  ");
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
                    DataTable dt3;
                    if (Type == "HANA")
                        dt3 = dBConnection.ExecuteQueryForDataTable("SELECT \"Product\",\"MaterialDescription\", \"HSNCode\",\"Brand\",\"Size\", \"TaxPercentage\" FROM tec_led4 WHERE \"Id\" = '" + getid + "' ");
                    else
                        dt3 = dBConnection.SQL_ExecuteQueryForDataTable("SELECT \"Product\",\"MaterialDescription\", \"HSNCode\", \"Brand\",\"Size\", \"TaxPercentage\" FROM tec_led4 WHERE \"Id\" = '" + getid + "' ");
                    if (dt3.Rows.Count > 0)
                    {
                        List<MajorGoodsService> majorGoodsList = dt3.AsEnumerable()
        .Select(row => new MajorGoodsService
        {
            Product = row["Product"].ToString(),
            MaterialDescription = row["MaterialDescription"].ToString(),
            HSNCode = row["HSNCode"].ToString(),
            Brand = row["Brand"].ToString(),
            Size = row["Size"].ToString(),
            TaxPercentage = row["TaxPercentage"].ToString()
        }).ToList();

                        gvMajorGoods.DataSource = majorGoodsList;
                        gvMajorGoods.DataBind();
                    }
                    DataTable dt4;
                    if (Type == "HANA")
                        dt4 = dBConnection.ExecuteQueryForDataTable("SELECT \"CustomerName\" FROM tec_led5 WHERE \"ID\" = '" + getid + "' ");
                    else
                        dt4 = dBConnection.SQL_ExecuteQueryForDataTable("SELECT \"CustomerName\" FROM tec_led5 WHERE \"ID\" = '" + getid + "' ");
                    if (dt4.Rows.Count > 0)
                    {
                        List<MajorCustomers> majorCustomersList = dt4.AsEnumerable()
        .Select(row => new MajorCustomers
        {
            CustomerName = row["CustomerName"].ToString()
        }).ToList();

                        gvMajorCustomers.DataSource = majorCustomersList;
                        gvMajorCustomers.DataBind();
                    }
                    DataTable dt5;
                    if (Type == "HANA")
                        dt5 = dBConnection.ExecuteQueryForDataTable("SELECT  \"Description\",\"TextMode\" FROM tec_led6 WHERE \"ID\" = '" + getid + "' ");
                    else
                        dt5 = dBConnection.SQL_ExecuteQueryForDataTable("SELECT  \"Description\",\"TextMode\" FROM tec_led6 WHERE \"ID\" = '" + getid + "' ");
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
                    int j = 1;
                    for (int i = 0; i < gvKYCDocuments.Rows.Count; i++)
                    {
                        GridViewRow row = gvKYCDocuments.Rows[i];

                        Label lblDocumentName = (Label)row.FindControl("DocumentName");

                        if (lblDocumentName != null)
                        {
                            string documentName;
                            if (Type == "HANA")
                                documentName = dBConnection.GetSingleValue("SELECT \"DocumentName\" FROM tec_led7 WHERE \"Id\"='" + getid + "' AND \"LineId\"='" + j + "'");
                            else
                                documentName = dBConnection.SQL_GetSingleValue("SELECT \"DocumentName\" FROM tec_led7 WHERE \"Id\"='" + getid + "' AND \"LineId\"='" + j + "'");

                            if (!string.IsNullOrEmpty(documentName))
                            {
                                lblDocumentName.Text = documentName;
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

                else
                {

                    Response.Write("No records found for the provided GSTNo.");
                }


            }
        }
        [System.Web.Services.WebMethod]

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
                    string script = $"window.open('{fileUrl}', '_blank');";
                    Page.ClientScript.RegisterStartupScript(GetType(), "OpenFile", script, true);
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
        public string GetDocumentImageByDocumentName(string documentName)
        {
            string gstNumber = Session["GSTNumber"].ToString();
            DbConnection db = new DbConnection();
            string getid;
            if (Type == "HANA")
                getid = db.GetSingleValue("select \"Id\" from tec_oled where \"GstNo\"='" + gstNumber + "'");
            else
                getid = db.SQL_GetSingleValue("select \"Id\" from tec_oled where \"GstNo\"='" + gstNumber + "'");
            int Id = Convert.ToInt32(getid);
            string base64ImageData = null;
            string query;
            if (Type == "HANA")
                query = "SELECT \"FileData\" FROM tec_led7 WHERE \"DocumentType\" = '" + documentName + "' and \"Id\"=" + Id + "";
            else
                query = "SELECT \"FileData\" FROM tec_led7 WHERE \"DocumentType\" = '" + documentName + "' and \"Id\"=" + Id + "";
            if (Type == "HANA")
                base64ImageData = db.GetSingleValue(query);
            else
                base64ImageData = db.SQL_GetSingleValue(query);

            return base64ImageData != null ? "data:image/png;base64," + base64ImageData : string.Empty;
        }







        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string documentType = btn.CommandArgument;

                string getid1;
                string gstNo = GSTNumber.Text.Trim();

                if (Type == "HANA")
                    getid1 = dBConnection.GetSingleValue($"select \"Id\" from tec_oled where \"GstNo\"='{gstNo}'");
                else
                    getid1 = dBConnection.SQL_GetSingleValue($"select \"Id\" from tec_oled where \"GstNo\"='{gstNo}'");

                if (string.IsNullOrEmpty(getid1))
                    return;

                string fileDataBase64;
                if (Type == "HANA")
                    fileDataBase64 = dBConnection.GetSingleValue($"select \"FileData\" from tec_led7 where \"Id\"='{getid1}' and \"DocumentType\"='{documentType}'");
                else
                    fileDataBase64 = dBConnection.SQL_GetSingleValue($"select \"FileData\" from tec_led7 where \"Id\"='{getid1}' and \"DocumentType\"='{documentType}'");

                if (string.IsNullOrEmpty(fileDataBase64))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "nofile", "alert('No file found for this document type.');", true);
                    return;
                }

                byte[] fileBytes;
                try
                {
                    if (System.IO.File.Exists(fileDataBase64))
                    {
                        fileBytes = System.IO.File.ReadAllBytes(fileDataBase64);
                    }
                    else
                    {
                        fileBytes = Convert.FromBase64String(fileDataBase64);
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
                string script = $"window.open('{fileUrl}', '_blank');";
                Page.ClientScript.RegisterStartupScript(GetType(), "OpenFile", script, true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errorMsg", $"alert('Error: {ex.Message}');", true);
            }
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


        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            DbConnection conn = new DbConnection();
            string username = Session["username"].ToString();
            string reason = popuptext.Text;

            string gstAllowedStatus = ddlGstRecreate.SelectedValue;

            string GSTNo = Session["GSTNo"].ToString();
            string Department = string.Empty;
            string level = conn.GetSingleValue("Select \"Level\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
            if (Type == "HANA")
                Department = conn.GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
            else
                Department = conn.SQL_GetSingleValue("Select \"Department\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");

            string query = "UPDATE TEC_OLED SET \"Approval\"='N',\"RejectionStatus\"='Y',\"DraftApproved\"='N',\"Draft\"='Y', \"RejectionReason\"='" + reason + "', \"RejectedUser\"='" + username + "',\"ApprovedDepartment\"='" + Department + "' WHERE \"GstNo\"='" + GSTNo + "'";

            conn.ExecuteNonQuery("Insert into \"Mail_Log\" (\"GstNo\",\"Type\",\"ActionDate\") values('" + GSTNo + "','Rejected',Current_Date)");
            if (Type == "HANA")
                conn.ExecuteNonQuery(query);
            else
                conn.SQL_ExecuteNonQuery(query);
            conn.ExecuteNonQuery("insert into \"ApprovalTrace\"  (\"User\",\"GstNo\",\"ApproveStatus\",\"RjectedReason\",\"Level\",\"ReApplySts\") values('" + username + "','" + GSTNo + "','N','" + reason + "','" + level + "','" + gstAllowedStatus + "')");
            string script = "alert('Rejected Successfully');";
            ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);
            Response.Redirect("/Pages/VendorForm.aspx");
        }

        protected void ApproveVendor(object sender, EventArgs e)
        {

            string username = Session["username"].ToString();
            string gstNo = GSTNumber.Text;
            DbConnection conn = new DbConnection();
            string Department = string.Empty;
            string remarks = txtRemarksPage.Text.Trim();
            string level = conn.GetSingleValue("Select \"Level\" from \"TEC_OUSR\" where \"User_Name\"='" + username + "' or  \"User_Mail_Id\" = '" + username + "'");
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

            if (Type == "HANA")
                conn.ExecuteNonQuery("call \"IsApproved\"('" + username + "','" + Department + "','" + gstNo + "')");
            else
                conn.SQL_ExecuteNonQuery("Exec  \"IsApproved\" '" + username + "','" + Department + "','" + gstNo + "'");

            conn.ExecuteNonQuery("insert into \"ApprovalTrace\"  (\"User\",\"GstNo\",\"ApproveStatus\",\"Level\") values('" + username + "','" + gstNo + "','Y','" + level + "')");

            string script = "alert('Approval Successfull');";
            ClientScript.RegisterStartupScript(this.GetType(), "RequiredFieldsAlert", script, true);
            Response.Redirect("/Pages/VendorForm.aspx");
        }



        protected void Reject(object sender, EventArgs e)
        {
            popuptext.ReadOnly = false;
            popuptext.Enabled = true;
            popuptext.Attributes.Remove("readonly");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "BankDetailspopup", "BankDetailspopup();", true);

        }
        protected void DraftApproved(object sender, EventArgs e)
        {
            DbConnection connection = new DbConnection();
            connection.ExecuteNonQuery("Update \"TEC_OLED\" set \"MerApproved\"='Y',\"DraftApproved\"='Y' where \"GstNo\"='" + GSTNumber.Text.Trim() + "'");
            Response.Redirect("~/Pages/VendorForm.aspx");
        }
        protected void Cancel(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/VendorForm.aspx");
        }





        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string documentType = btn.CommandArgument;

                string fileDataBase64 = GetFileDataByDocumentType(documentType);

                if (!string.IsNullOrEmpty(fileDataBase64))
                {
                    string fileExtension = GetFileType(fileDataBase64);

                    byte[] fileBytes = Convert.FromBase64String(fileDataBase64);

                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", $"attachment; filename={documentType}.{fileExtension}");
                    Response.BinaryWrite(fileBytes);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('File not found.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Error while downloading file: {ex.Message}');", true);
            }
        }





        private string GetFileDataByDocumentType(string documentType)
        {
            string getid1;
            if (Type == "HANA")
                getid1 = dBConnection.GetSingleValue($"select \"Id\" from tec_oled where \"GstNo\"='{GSTNumber.Text.Trim()}' ");
            else
                getid1 = dBConnection.SQL_GetSingleValue($"select \"Id\" from tec_oled where \"GstNo\"='{GSTNumber.Text.Trim()}' ");

            if (string.IsNullOrEmpty(getid1))
                return null;

            string query = $"SELECT \"FileData\" FROM tec_led7 WHERE \"DocumentType\"='{documentType}' AND \"Id\"='{getid1}'";

            return Type == "HANA"
                ? dBConnection.GetSingleValue(query)
                : dBConnection.SQL_GetSingleValue(query);
        }
        private string GetFileType(string base64Data)
        {
            if (string.IsNullOrEmpty(base64Data) || base64Data.Length < 4)
                return "bin";

            if (base64Data.StartsWith("JVBE", StringComparison.OrdinalIgnoreCase))
                return "pdf";

            if (base64Data.StartsWith("iVBOR", StringComparison.OrdinalIgnoreCase))
                return "png";

            if (base64Data.StartsWith("/9j/", StringComparison.OrdinalIgnoreCase))
                return "jpg";

            if (base64Data.StartsWith("R0lG", StringComparison.OrdinalIgnoreCase))
                return "gif";

            return "bin";
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
        protected void gvProjectDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as BusinessDetails;
                ((TextBox)e.Row.FindControl("businessState")).Text = dataItem?.BusinessState ?? string.Empty;
                ((TextBox)e.Row.FindControl("gstNumber")).Text = dataItem?.GSTNumber ?? string.Empty;
                ((TextBox)e.Row.FindControl("addressOfPlace")).Text = dataItem?.AddressOfPlace ?? string.Empty;
                ((TextBox)e.Row.FindControl("gstVendorClassification")).Text = dataItem?.GSTVendorClassification ?? string.Empty;
            }
        }
        [Serializable]
        public class OtherInformation
        {
            public string Description { get; set; }
            public string TextMode { get; set; }
        }

        [Serializable]
        public class MajorCustomers
        {
            public string CustomerName { get; set; }

        }
        [Serializable]
        public class MajorGoodsService
        {
            public int SI_No { get; set; }
            public string Product { get; set; }
            public string MaterialDescription { get; set; }
            public string HSNCode { get; set; }
            public string Brand { get; set; }
            public string Size { get; set; }
            public string TaxPercentage { get; set; }
        }
        [Serializable]
        public class OperationalContact
        {
            public string Department { get; set; }
            public string Name { get; set; }
            public string Designation { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
        }
        [Serializable]
        public class PartnerDetails
        {
            public int SI_No { get; set; }
            public string Name { get; set; }
            public string Designation { get; set; }
            public string Contact_No { get; set; }
            public string Email_ID { get; set; }
            public string RowID { get; set; }

        }
        [Serializable]
        public class BusinessDetails
        {
            public string BusinessState { get; set; }
            public string GSTNumber { get; set; }
            public string AddressOfPlace { get; set; }
            public string GSTVendorClassification { get; set; }

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
        private void BindKYCGrid1()
        {
            DataTable dtKYC = new DataTable();
            dtKYC.Columns.Add("DocumentType");
            dtKYC.Columns.Add("FileData");

            dtKYC.Rows.Add("PAN Card", "<Base64EncodedFileDataForPanCard>");
            dtKYC.Rows.Add("GST Certificate", "<Base64EncodedFileDataForGST>");
            dtKYC.Rows.Add("Bank Account", "<Base64EncodedFileDataForBankAccount>");
            dtKYC.Rows.Add("MSME Certificate", "<Base64EncodedFileDataForBusinessAddress>");

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

    }
}