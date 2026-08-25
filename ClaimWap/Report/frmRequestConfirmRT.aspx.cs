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
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ClaimWap
{
    public partial class frmReqconfirmRT : System.Web.UI.Page
    {
        protected ReportViewer ReportViewercomReq = new ReportViewer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fnLoadReportConReq();
            }    
        }
        //private void fnLoadReportConReq()
        //{

        //    string Doc = string.Empty;
        //    string Docwords = string.Empty;
        //    string Docdisplay = string.Empty;


        //    Docdisplay = Request.QueryString["ClmNUM"];
        //    byte[] data = System.Convert.FromBase64String(Docdisplay);
        //    Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);


        //    //Doc = "CM18110019,CM18120036,CM18120037";
        //    string Cus = string.Empty;
        //    string slm = string.Empty;
        //    string item = string.Empty;
        //    string cusre = string.Empty;
        //    string cmsib = string.Empty;
        //   // Doc = "RTA20010002,RTT20010009,RTT20010011";
        //    DataSet ds1 = new DataSet();
        //    string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(conString))
        //    {
        //        ReportViewercomReq.ProcessingMode = ProcessingMode.Local;
        //        ReportViewercomReq.LocalReport.ReportPath = Server.MapPath("~/Report/rptRequestConfirmRT.rdlc");
        //        con.Open();
        //        SqlDataAdapter sda1 = new SqlDataAdapter();


        //        SqlCommand cmd = new SqlCommand("P_GetReqRT_ByDoc", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@DOC", Doc.ToString());
        //        sda1.SelectCommand = cmd;
        //        sda1.Fill(ds1, "DataSet1");
        //        SqlDataReader dr = cmd.ExecuteReader();
        //         while (dr.Read())
        //         {
        //             cusre = dr["CUSNAM"].ToString();
        //             Cus = dr["CUSCOD"].ToString() + '-' + Regex.Replace(cusre, "\"[^\"]*\"", string.Empty);
        //             slm = dr["SLMCOD"].ToString() + '-' + dr["SLMNAM"].ToString();
        //             item = dr["STKCOD"].ToString() + '-' + dr["STKDES"].ToString();
        //             cmsib = dr["STMP_ID_SUB"].ToString();
        //         }
        //         dr.Close();
        //         dr.Dispose();
        //         cmd.Dispose();
        //         con.Close();
        //    }
        //    ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
        //    //ReportDataSource datasource3 = new ReportDataSource("DataSet2", ds1.Tables[1]);
        //    ReportViewercomReq.LocalReport.DataSources.Clear();
        //    ReportViewercomReq.LocalReport.DataSources.Add(datasource2);
        //    ReportViewer rpt = new ReportViewer();
        //    //rpt.SetPageSettings(new System.Drawing.Printing.PageSettings() { Landscape = true });
        //    string reportType = "PDF";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    //The DeviceInfo settings should be changed based on the reportType

        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    " <OutputFormat>PDF</OutputFormat>" +
        //    " <PageWidth>8.5in</PageWidth>" +
        //    "<PageHeight>11.7in</PageHeight>" +
        //    "<MarginTop>0.5in</MarginTop>" +
        //    " <MarginLeft>0.1in</MarginLeft>" +
        //    " <MarginRight>0.1in</MarginRight>" +
        //    " <MarginBottom>0in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    //Render the report
        //    renderedBytes = ReportViewercomReq.LocalReport.Render(
        //    reportType,
        //    deviceInfo,
        //    out mimeType,
        //    out encoding,
        //    out fileNameExtension,
        //    out streams,
        //    out warnings);

        //    Response.Buffer = true;
        //    Response.Clear();
        //    Response.ContentType = mimeType;
        //    Response.AddHeader("content-disposition", "attachment; filename=RequestRT-" + cmsib +"-"+ Cus + "-" + slm +"." + fileNameExtension);

        //    Response.BinaryWrite(renderedBytes);

        //    string path = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) + @"\Downloads\RequestConfirmRT-" + cmsib + "-" + Cus + "-" + slm + ".pdf";
        //    System.IO.File.Delete(path);


        //    Response.End();
        //}
        private void fnLoadReportConReq()
        {
            string Doc = string.Empty;
            string Docdisplay = Request.QueryString["ClmNUM"];
            byte[] data = System.Convert.FromBase64String(Docdisplay);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

            string Cus = string.Empty;
            string slm = string.Empty;
            string cmsib = string.Empty;
            string rtno = string.Empty;

            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conString))
            {
                ReportViewercomReq.ProcessingMode = ProcessingMode.Local;
                ReportViewercomReq.LocalReport.ReportPath = Server.MapPath("~/Report/rptRequestConfirmRT.rdlc");
                con.Open();
                SqlDataAdapter sda1 = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("P_GetReqRT_ByDoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DOC", Doc.ToString());
                sda1.SelectCommand = cmd;
                sda1.Fill(ds1, "DataSet1");

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string cusre = dr["CUSNAM"].ToString();
                    Cus = dr["CUSCOD"].ToString() + '-' + Regex.Replace(cusre, "\"[^\"]*\"", string.Empty);
                    slm = dr["SLMCOD"].ToString() + '-' + dr["SLMNAM"].ToString();
                    cmsib = dr["STMP_ID_SUB"].ToString();
                    rtno = dr["STMP_ID"].ToString();
                }
                dr.Close();
                cmd.Dispose();
                con.Close();
            }

            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            ReportViewercomReq.LocalReport.DataSources.Clear();
            ReportViewercomReq.LocalReport.DataSources.Add(datasource2);

            string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>8.5in</PageWidth>" +
                "<PageHeight>11.7in</PageHeight>" +
                "<MarginTop>0.5in</MarginTop>" +
                "<MarginLeft>0.1in</MarginLeft>" +
                "<MarginRight>0.1in</MarginRight>" +
                "<MarginBottom>0in</MarginBottom>" +
                "</DeviceInfo>";

            // Render ครั้งเดียว
            renderedBytes = ReportViewercomReq.LocalReport.Render(
                "PDF",
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            // โหลดฟอนต์จากไฟล์
            string fontPath = Server.MapPath("~/Fonts/C39P60DlTt.ttf");
            BaseFont barcodeFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            // อ่าน PDF ที่สร้างจาก RDLC แล้วฝังฟอนต์ใหม่
            using (MemoryStream ms = new MemoryStream())
            {
                PdfReader reader = new PdfReader(renderedBytes);
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    int n = reader.NumberOfPages;
                    for (int i = 1; i <= n; i++)
                    {
                        PdfContentByte cb = stamper.GetOverContent(i);
                        cb.BeginText();
                        cb.SetFontAndSize(barcodeFont, 28);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "*" + rtno + "*", 471, 745, 0);
                        cb.EndText();
                    }
                }

                renderedBytes = ms.ToArray();
                reader.Close();
            }

            // สร้าง MemoryStream สำหรับรวม PDF
            using (MemoryStream outputPdf = new MemoryStream())
            {
                Document document = new Document();
                PdfCopy writer = new PdfCopy(document, outputPdf);
                document.Open();

                // อ่าน PDF ต้นฉบับ
                PdfReader reader = new PdfReader(renderedBytes);
                int totalPages = reader.NumberOfPages;

                // วนตามจำนวนหน้าใน report
                for (int page = 1; page <= totalPages; page++)
                {
                    // ทำซ้ำแต่ละหน้า 4 ครั้ง
                    for (int copyIndex = 1; copyIndex <= 4; copyIndex++)
                    {
                        writer.AddPage(writer.GetImportedPage(reader, page));
                    }
                }

                reader.Close();
                document.Close();


                // ส่งออกไฟล์ PDF รวม
                byte[] finalPdf = outputPdf.ToArray();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition","attachment; filename=RequestRT-" + cmsib + "-" + Cus + "-" + slm + ".pdf");
                Response.BinaryWrite(finalPdf);
                Response.End();
            }
        }
    }
}