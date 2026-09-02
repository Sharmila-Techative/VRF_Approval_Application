<%@ WebHandler Language="C#" Class="UploadHandler" %>
using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Configuration;

public class UploadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string panNumber = context.Session["PAN"] != null ? context.Session["PAN"].ToString() : null;
        context.Response.ContentType = "application/json";
        string folder = ConfigurationManager.AppSettings["ImageUploadFolder"];

        // Physical save path
        string rootPath = Path.IsPathRooted(folder) ? folder : context.Server.MapPath("~/" + folder);
        if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

        string action = context.Request.QueryString["action"];

        // 🔹 Fetch all images for product
        if (action == "get")
        {
            string serialNo = context.Request.QueryString["serialNo"];
            string product = context.Request.QueryString["product"];
            string key = $"images_{serialNo}_{product}";

            var files = new List<object>();
            foreach (var f in Directory.GetFiles(rootPath))
            {
                string fileName = Path.GetFileName(f);

                // Only include files that match your key and have the right extension
                if (!fileName.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                    continue;

                string ext = Path.GetExtension(f).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    continue;
                byte[] imgBytes = File.ReadAllBytes(f);
                string base64 = Convert.ToBase64String(imgBytes);
                string dataUrl = "data:image/jpeg;base64," + base64;

                files.Add(new { fileName = Path.GetFileName(f), base64 = dataUrl });
            }

            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { files }));
            return;
        }

        // 🔹 Delete image
        if (action == "delete")
        {
            string fileName = context.Request.QueryString["file"];
            if (!string.IsNullOrEmpty(fileName))
            {
                string rootFile = Path.Combine(rootPath, fileName);
                if (File.Exists(rootFile))
                {
                    File.Delete(rootFile);
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { success = true }));
                    return;
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { success = false, message = "File not found" }));
            return;
        }

        // 🔹 Upload JPG images
        string serial = context.Request.Form["serialNo"];
        string productKey = context.Request.Form["product"];
        string fileKey = $"images_{serial}_{productKey}";

        var savedFiles = new List<object>();
        int counter = 1;

        foreach (string keyName in context.Request.Files)
        {
            HttpPostedFile file = context.Request.Files[keyName];
            string ext = Path.GetExtension(file.FileName).ToLower();

            if (ext != ".jpg" && ext != ".jpeg") continue;

            string fileName;
            do
            {
                fileName = $"{fileKey}({counter}).jpg";
                counter++;
            } while (File.Exists(Path.Combine(rootPath, fileName)));

            string savePath = Path.Combine(rootPath, fileName);
            file.SaveAs(savePath);

            // Return Base64 for preview
            byte[] imgBytes = File.ReadAllBytes(savePath);
            string base64 = Convert.ToBase64String(imgBytes);
            string dataUrl = "data:image/jpeg;base64," + base64;

            savedFiles.Add(new { fileName = fileName, base64 = dataUrl });
        }

        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { success = true, files = savedFiles }));
    }

    public bool IsReusable { get { return false; } }
}