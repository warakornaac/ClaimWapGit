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
    public partial class frmReqRT : System.Web.UI.Page
    {
        protected ReportViewer ReportViewerReq = new ReportViewer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fnLoadReportReq();
            }    
        }
        private void fnLoadReportReq()
        {
            
            string Doc = string.Empty;
            string Docwords = string.Empty;
            string Docdisplay = string.Empty;


            Docdisplay = Request.QueryString["ClmNUM"];
            byte[] data = System.Convert.FromBase64String(Docdisplay);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

           
            //Doc = "CM18110019,CM18120036,CM18120037";
            string Cus = string.Empty;
            string slm = string.Empty;
            string item = string.Empty;
            string cusre = string.Empty;
            string cmsib = string.Empty;
            string rtno = string.Empty;
           // Doc = "RTA20010002,RTT20010009,RTT20010011";
            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                ReportViewerReq.ProcessingMode = ProcessingMode.Local;
                ReportViewerReq.LocalReport.ReportPath = Server.MapPath("~/Report/rptRequestRT.rdlc");
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
                     cusre = dr["CUSNAM"].ToString();
                     Cus = dr["CUSCOD"].ToString() + '-' + Regex.Replace(cusre, "\"[^\"]*\"", string.Empty);
                     slm = dr["SLMCOD"].ToString() + '-' + dr["SLMNAM"].ToString();
                     item = dr["STKCOD"].ToString() + '-' + dr["STKDES"].ToString();
                     cmsib = dr["STMP_ID_SUB"].ToString();
                    rtno = dr["STMP_ID"].ToString();
                }
                dr.Close();
                 dr.Dispose();
                 cmd.Dispose();
                 con.Close();
            }
            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            //ReportDataSource datasource3 = new ReportDataSource("DataSet2", ds1.Tables[1]);
            ReportViewerReq.LocalReport.DataSources.Clear();
            ReportViewerReq.LocalReport.DataSources.Add(datasource2);
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
            " <PageWidth>8.5in</PageWidth>" +
            "<PageHeight>11.7in</PageHeight>" +
            "<MarginTop>0.5in</MarginTop>" +
            " <MarginLeft>0.1in</MarginLeft>" +
            " <MarginRight>0.1in</MarginRight>" +
            " <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = ReportViewerReq.LocalReport.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

            //
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

                PdfReader readerCheck = new PdfReader(renderedBytes);
                int totalPages = readerCheck.NumberOfPages;
                readerCheck.Close();

                for (int page = 1; page <= totalPages; page++)
                {
                    for (int copy = 1; copy <= 4; copy++)
                    {
                        using (PdfReader reader = new PdfReader(renderedBytes))
                        {
                            writer.AddPage(writer.GetImportedPage(reader, page));
                        }
                    }
                }

                document.Close();
                byte[] finalPdf = outputPdf.ToArray();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition",
                    "attachment; filename=RequestRT-" + cmsib + "-" + Cus + "-" + slm + "." + fileNameExtension);
                Response.BinaryWrite(finalPdf);
                Response.End();
            }
        }
    }
}