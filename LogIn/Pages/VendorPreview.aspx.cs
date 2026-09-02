using LogIn.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogIn.Pages
{
    public partial class VendorPreview : System.Web.UI.Page
    {
        DbConnection dbConnection = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dbConnection.writeLog("VendorPreview_Page_Load", "VendorPreview page load initiated.", "Debug");
                RenderDynamicPreview();
            }
            catch (Exception ex)
            {
                dbConnection.LogError(ex, "VendorPreview_Page_Load");
            }
        }

        private void RenderDynamicPreview()
        {
            try
            {
                string json = Session["PreviewData"] as string;
                var data1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                string gstNo = data1["GST Number"].ToString();
                dbConnection.writeLog("RenderDynamicPreview", "Rendering preview for GST: " + gstNo, "Debug");

                string Query = "Select ifnull(\"Approval\",'N') from \"TEC_OLED\" where \"GstNo\"='" + gstNo + "'";
                string Approval = dbConnection.GetSingleValue(Query);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json ?? "{}")
                          ?? new Dictionary<string, object>();
                var goods = Session["MajorGoods"] as List<GoodItem> ?? new List<GoodItem>();

                string htmlTemplate = System.IO.File.ReadAllText(Server.MapPath("~/Design/VendorForm.html"));
                if (Approval == "Y")
                {
                    htmlTemplate = Regex.Replace(htmlTemplate, @"<div class=""watermark""[^>]*>.*?</div>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }
                htmlTemplate = PopulateFields(htmlTemplate, data);

                htmlTemplate = htmlTemplate.Replace(
                    "<div class=\"items-section\" id=\"items_supplied\"></div>",
                    GenerateItemsHtml(goods)
                );

                litHtml.Text = htmlTemplate;
                dbConnection.writeLog("RenderDynamicPreview", "Preview HTML generated successfully.", "Debug");
            }
            catch (Exception ex)
            {
                dbConnection.LogError(ex, "RenderDynamicPreview");
                litHtml.Text = $"<div style='color:red; padding:20px;'><h2>ERROR:</h2><p>{ex.Message}</p><p>{ex.StackTrace}</p></div>";
            }
        }

        private string PopulateFields(string html, Dictionary<string, object> data)
        {
            try
            {
                var fieldMappings = new Dictionary<string, string>
                {
                    ["ref_no"] = "",
                    ["code_no"] = "",
                    ["form_date"] = "",
                    ["location"] = "",
                    ["vendor_name"] = "Trade Name",
                    ["address"] = "Billing Address",
                    ["address1"] = "",
                    ["registered_office"] = "Registered Address",
                    ["registered_office1"] = "",
                    ["business"] = "Nature of Business",
                    ["contact1"] = "Mobile Number",
                    ["contact2"] = "Office Telephone",
                    ["email1"] = "Email ID",
                    ["email2"] = "Agency Email",
                    ["proprietor"] = "Contact Person",
                    ["prop_phone"] = "Mobile No",
                    ["bank_branch"] = "Bank Name",
                    ["account_no"] = "Account Number",
                    ["ifsc"] = "IFSC Code",
                    ["return_address"] = "Goods Return Address",
                    ["days"] = "Credit Days",
                    ["md0_with"] = "md0_with",
                    ["md0_without"] = "md0_without",
                    ["md3_with"] = "md3_with",
                    ["md3_without"] = "md3_without",
                    ["md5_with"] = "md5_with",
                    ["md5_without"] = "md5_without",
                    ["md18_with"] = "md18_with",
                    ["md18_without"] = "md18_without",
                    ["discount"] = "Discount",
                    ["gst"] = "GST Number",
                    ["pan"] = "PAN Number",
                    ["msme"] = "MSME Number",
                    ["msme_date"] = "Date of Establishment",
                    ["activity"] = "Nature of Business",
                    ["enterprise"] = "Enterprise Type",
                    ["legal_name"] = "Trade Name",
                    ["trade_name"] = "Trade Name",
                    ["agency_direct"] = "Business Type",
                    ["agency_email"] = "Agency Email",
                    ["contact_person"] = "NHFS Contact Person",
                    ["Remarks"] = "Remarks",
                    ["location"] = "location",
                    ["form_date"] = "date"
                };

                foreach (var mapping in fieldMappings)
                {
                    string htmlId = mapping.Key;
                    string dataKey = mapping.Value;

                    string value = string.IsNullOrEmpty(dataKey) ? "" : GetSafeValue(data, dataKey);

                    value = HttpUtility.HtmlEncode(value);

                    value = value.Replace("\r\n", "<br/>").Replace("\n", "<br/>");

                    string pattern = $@"<span class=""readonly-field"" id=""{htmlId}""></span>";
                    string replacement = $@"<span class=""readonly-field"" id=""{htmlId}"">{value}</span>";

                    html = html.Replace(pattern, replacement);
                }

                return html;
            }
            catch (Exception ex)
            {
                dbConnection.LogError(ex, "PopulateFields");
                return html;
            }
        }

        private string GenerateItemsHtml(List<GoodItem> goods)
        {
            try
            {
                StringBuilder items = new StringBuilder();
                string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };

                for (int i = 0; i < goods.Count && i < letters.Length; i++)
                {
                    var item = goods[i];
                    items.Append($@"
                <div style=""margin-bottom: 12px; line-height: 1.4;"">
                    <div style=""margin-bottom: 6px;"">
                        <strong>{letters[i]}) Product:</strong> <span style=""font-weight: normal;"">{HttpUtility.HtmlEncode(item.Product ?? "")}</span>
                    </div>
                    <div style=""margin-bottom: 6px;"">
                        <strong>Brand:</strong> <span style=""font-weight: normal;"">{HttpUtility.HtmlEncode(item.Brand ?? "")}</span>
                    </div>
                    <div style=""margin-bottom: 0;"">
                        <strong>Size:</strong> <span style=""font-weight: normal;"">{HttpUtility.HtmlEncode(item.Size ?? "")}</span>
                    </div>
                </div>");
                }

                return $"<div class=\"items-section\" id=\"items_supplied\">{items}</div>";
            }
            catch (Exception ex)
            {
                dbConnection.LogError(ex, "GenerateItemsHtml");
                return "<div class=\"items-section\" id=\"items_supplied\"></div>";
            }
        }

        private string GetSafeValue(Dictionary<string, object> data, string key)
        {
            data.TryGetValue(key, out object value);
            return value?.ToString() ?? "";
        }
    }
}