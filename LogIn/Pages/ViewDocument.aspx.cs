using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LogIn.Pages
{
    public partial class ViewDocument : System.Web.UI.Page
    {
        DbConnection db = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                db.writeLog("ViewDocument_Page_Load", "Loading ViewDocument page.", "Debug");
                if (Session["FileData"] != null && Session["FileMimeType"] != null)
                {
                    string base64Data = Session["FileData"].ToString();
                    string mimeType = Session["FileMimeType"].ToString();
                    db.writeLog("ViewDocument_Page_Load", "Document MIME Type: " + mimeType, "Debug");

                    if (mimeType == "image/jpeg")
                    {
                        imgDocument.ImageUrl = $"data:{mimeType};base64,{base64Data}";
                        imgDocument.Visible = true;
                    }
                    else if (mimeType == "application/pdf")
                    {
                        ltPDFViewer.Text = $"<iframe src='data:{mimeType};base64,{base64Data}' width='100%' height='600px'></iframe>";
                        ltPDFViewer.Visible = true;
                    }
                }
                else
                {
                    db.writeLog("ViewDocument_Page_Load", "No document data in session.", "Debug");
                    Response.Write("No document data available.");
                }
            }
            catch (Exception ex)
            {
                db.LogError(ex, "ViewDocument_Page_Load");
            }
        }
    }
}