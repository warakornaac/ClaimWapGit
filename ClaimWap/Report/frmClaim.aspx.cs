using System;
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
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;
namespace ClaimWap.Report
{
    public partial class frmReqClaim : System.Web.UI.Page
    {
       
        //protected DataSet ds = new DataSet();
        protected ReportViewer ReportViewer = new ReportViewer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fnLoadReportBoc();
            }    
        }
        private void fnLoadReportBoc() {
            string Doc = string.Empty;
            string Docwords = string.Empty;
            string Docdisplay = string.Empty;
            string SubDocwords = string.Empty;
            string SubDoc = string.Empty;
            string SubUsrtype = string.Empty;
            string Usrtype = string.Empty;

            Docdisplay = Request.QueryString["ClmNUM"];
            string[] words = Docdisplay.Split('/');
            Docwords = words[0];
            byte[] data = System.Convert.FromBase64String(Docwords);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

            SubDocwords = words[1];
            byte[] datasub = System.Convert.FromBase64String(SubDocwords);
            SubDoc = System.Text.ASCIIEncoding.ASCII.GetString(datasub);

            SubUsrtype = words[2];
            byte[] datasrtype = System.Convert.FromBase64String(SubUsrtype);
            Usrtype = System.Text.ASCIIEncoding.ASCII.GetString(datasrtype);

            string Cus = string.Empty;
            string slm = string.Empty;
            string item = string.Empty;
            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;

            ReportViewer.ProcessingMode = ProcessingMode.Local;
            ReportViewer.LocalReport.ReportPath = Server.MapPath("~/Report/rptClaimTech.rdlc");

            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand("P_GetClaim_ByDocSub", con)) {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;
                cmd.Parameters.AddWithValue("@DOC", Doc);
                cmd.Parameters.AddWithValue("@DOCSUB", SubDoc);

                using (SqlDataAdapter sda1 = new SqlDataAdapter(cmd)) {
                    sda1.Fill(ds1);   // ✅ ดึงข้อมูลจริงเข้า DataSet สำหรับ Report
                }
            } // con ปิด/dispose อัตโนมัติแม้เกิด exception

            // ตรวจสอบก่อนใช้ ป้องกัน IndexOutOfRange ถ้า SP ไม่คืนข้อมูล
            if (ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) {
                // TODO: แสดงข้อความแจ้งผู้ใช้ว่าไม่พบข้อมูล แทนที่จะปล่อยให้ error
                Response.Write("ไม่พบข้อมูลใบเคลม");
                Response.End();
                return;
            }

            DataRow row = ds1.Tables[0].Rows[0];
            Cus = row["CUSCOD"].ToString() + '-' + row["CUSNAM"].ToString();
            slm = row["SLMCOD"].ToString() + '-' + row["SLMNAM"].ToString();
            item = row["STKCOD"].ToString();

            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            ReportViewer.LocalReport.DataSources.Clear();
            ReportViewer.LocalReport.DataSources.Add(datasource2);

            string GrpUserPrint = Usrtype;
            ReportParameter rp = new ReportParameter("GrpUserPrint", GrpUserPrint, false);
            this.ReportViewer.LocalReport.SetParameters(new ReportParameter[] { rp });

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>8.5in</PageWidth>" +
                " <PageHeight>11.7in</PageHeight>" +
                " <MarginTop>0.5in</MarginTop>" +
                " <MarginLeft>0.1in</MarginLeft>" +
                " <MarginRight>0.1in</MarginRight>" +
                " <MarginBottom>0in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = ReportViewer.LocalReport.Render(
                reportType, deviceInfo, out mimeType, out encoding,
                out fileNameExtension, out streams, out warnings);

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition",
                "attachment; filename=rptClaim" + SubDoc + "-" + Cus + "-" + slm + "." + fileNameExtension);
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }
    }
}