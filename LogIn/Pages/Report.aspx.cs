using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;

namespace LogIn.Pages
{
    public partial class Report : System.Web.UI.Page
    {
        DbConnection conn = new DbConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                conn.writeLog("Report_Page_Load", "Report page loaded. IsPostBack: " + IsPostBack, "Debug");
                if (!IsPostBack)
                {
                    LoadReportTypes();
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "Report_Page_Load");
            }
        }

        private void LoadReportTypes()
        {
            try
            {
                conn.writeLog("LoadReportTypes", "Loading report types.", "Debug");
                string Query = "Call \"SP_GetReportTypes\"()";
                DataTable Dt = new DataTable();
                Dt = conn.ExecuteQueryForDataTable(Query);
                ddlReportType.DataSource = Dt;
                ddlReportType.DataTextField = "ReportName";
                ddlReportType.DataValueField = "ReportID";
                ddlReportType.DataBind();

                ddlReportType.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "LoadReportTypes");
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"] != null ? Session["username"].ToString() : "";
                conn.writeLog("btnLoad_Click", "Loading report for Type: " + ddlReportType.SelectedValue + ", From: " + txtFromDate.Text + ", To: " + txtToDate.Text, "Debug");
                string Query = "Call \"SP_GetReportData\" ('" + username + "','" + ddlReportType.SelectedValue + "','" + txtFromDate.Text + "','" + txtToDate.Text + "')";
                DataTable dt = conn.ExecuteQueryForDataTable(Query);
                gvReport.DataSource = dt;
                gvReport.DataBind();

                ViewState["ReportData"] = dt;
                conn.writeLog("btnLoad_Click", "Report rows loaded: " + dt.Rows.Count, "Debug");
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnLoad_Click");
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["ReportData"] != null)
                {
                    DataTable dt = (DataTable)ViewState["ReportData"];
                    conn.writeLog("btnDownload_Click", "Exporting report to Excel. Rows: " + dt.Rows.Count, "Debug");
                    ExportToExcel(dt);
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "btnDownload_Click");
            }
        }

        private void ExportToExcel(DataTable dt)
        {
            try
            {
                string fileName = $"Report_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Report");

                    ws.Cell(1, 1).InsertTable(dt, "ReportData", true);

                    var headerRange = ws.Range(1, 1, 1, dt.Columns.Count);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 123, 255);
                    headerRange.Style.Font.FontColor = XLColor.White;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    ws.Columns().AdjustToContents();

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName.ToLower();
                        if (colName.Contains("date"))
                        {
                            ws.Column(i + 1).Style.DateFormat.Format = "dd-MM-yyyy";
                        }
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
                        Response.BinaryWrite(fileBytes);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                conn.LogError(ex, "ExportToExcel");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}
