﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Reporting.WebForms;


namespace ClaimWap.Report
{
    public partial class frmViewLabel : System.Web.UI.Page
    {
        protected ReportViewer ReportViewer2 = new ReportViewer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fnLoadReportBoc();
            }    
          
        }
        private void fnLoadReportBoc()
        {
            string Doc = string.Empty;
            string Docwords = string.Empty;
            string Docdisplay = string.Empty;
            string SubDocwords = string.Empty;
            string SubDoc = string.Empty;
            string subClmCompany = string.Empty;
            string clmCompany = string.Empty;
            string fileReport = string.Empty;

            //string Doc_subdisplay = string.Empty;
            Docdisplay = Request.QueryString["ClmNUM"];
            string[] words = Docdisplay.Split('/');
            // Doc_subdisplay = Request.QueryString["ClmsubNUM"];
            Docwords = words[0];
            byte[] data = System.Convert.FromBase64String(Docwords);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

            SubDocwords = words[1];
            byte[] datasub = System.Convert.FromBase64String(SubDocwords);
            SubDoc = System.Text.ASCIIEncoding.ASCII.GetString(datasub);

            //รายการนี้มาจาก com ไหน
            subClmCompany = words[2];
            byte[] dataCom = System.Convert.FromBase64String(subClmCompany);
            clmCompany = System.Text.ASCIIEncoding.ASCII.GetString(dataCom);

            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                fileReport = "~/Report/rptLabel.rdlc";
                if (clmCompany != "")
                {
                    if (clmCompany == "TAM")
                    {
                        fileReport = "~/Report/rptLabelTam.rdlc";
                    }
                }
                ReportViewer2.ProcessingMode = ProcessingMode.Local;
                ReportViewer2.LocalReport.ReportPath = Server.MapPath(fileReport);
                con.Open();
                SqlDataAdapter sda1 = new SqlDataAdapter();


                SqlCommand cmd = new SqlCommand("P_GetClaim_ByDocSub", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DOC", Doc.ToString());
                cmd.Parameters.AddWithValue("@DOCSUB", SubDoc.ToString());
                sda1.SelectCommand = cmd;
                sda1.Fill(ds1, "DataSet1");
            }
            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            //ReportDataSource datasource3 = new ReportDataSource("DataSet2", ds1.Tables[1]);
            ReportViewer2.LocalReport.DataSources.Clear();
            ReportViewer2.LocalReport.DataSources.Add(datasource2);
            ReportViewer rpt = new ReportViewer();
            //rpt.SetPageSettings(new System.Drawing.Printing.PageSettings() { Landscape = true });
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType

            string deviceInfo =
            "<DeviceInfo>" +
            " <OutputFormat>PDF</OutputFormat>" +
            " <PageWidth>3in</PageWidth>" +
            "<PageHeight>4in</PageHeight>" +
            "<MarginTop>0in</MarginTop>" +
            " <MarginLeft>0in</MarginLeft>" +
            " <MarginRight>0in</MarginRight>" +
            " <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = ReportViewer2.LocalReport.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

            ////clear the response stream and write the bytes to the outputstream
            //set content-disposition to “attachment” so that user is prompted to take an action
            //on the file (open or save)
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=rptClaim " + SubDoc + "-" +DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + fileNameExtension);

            Response.BinaryWrite(renderedBytes);


            string path = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) + @"\Downloads\rptClaim" + ".pdf";
            //WebClient client = new WebClient();
            // Byte[] buffer = client.DownloadData(path);
            System.IO.File.Delete(path);


            Response.End();


        }
    }
}