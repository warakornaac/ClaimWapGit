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
    public partial class frmReqClaim : System.Web.UI.Page
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
            string subClmCompany = string.Empty;
            string clmCompany = string.Empty;
            string fileReport = string.Empty;

            Docdisplay = Request.QueryString["ClmNUM"];
            string[] words = Docdisplay.Split('/');
            Docwords = words[0];
            byte[] dataDoc = System.Convert.FromBase64String(Docwords);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(dataDoc);

            subClmCompany = words[1];
            byte[] dataCom = System.Convert.FromBase64String(subClmCompany);
            clmCompany = System.Text.ASCIIEncoding.ASCII.GetString(dataCom);

            string Cus = string.Empty;
            string slm = string.Empty;
            string item = string.Empty;
            string cmsib = string.Empty;
            string cmno = string.Empty;

            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conString))
            {
                fileReport = "~/Report/rptRequestClaim.rdlc";
                if (!string.IsNullOrEmpty(clmCompany))
                {
                    if (clmCompany == "TAM")
                        fileReport = "~/Report/rptRequestClaimTam.rdlc";
                    else if (clmCompany == "VELOX")
                        fileReport = "~/Report/rptRequestClaimVelox.rdlc";
                }

                ReportViewer2.ProcessingMode = ProcessingMode.Local;
                ReportViewer2.LocalReport.ReportPath = Server.MapPath(fileReport);
                con.Open();
                SqlDataAdapter sda1 = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand("P_GetReqClaim_ByDoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DOC", Doc.ToString());
                sda1.SelectCommand = cmd;
                sda1.Fill(ds1, "DataSet1");

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Cus = dr["CUSCOD"].ToString() + '-' + Regex.Replace(dr["CUSNAM"].ToString(), "\"[^\"]*\"", string.Empty);
                    slm = dr["SLMCOD"].ToString() + '-' + dr["SLMNAM"].ToString();
                    item = dr["STKCOD"].ToString() + '-' + dr["STKDES"].ToString();
                    cmsib = dr["CLM_NO_SUB"].ToString();
                    cmno = dr["REQ_NO"].ToString();
                }
                dr.Close();
                cmd.Dispose();
                con.Close();
            }

            Cus = Cus.Replace('.', '-').Replace(',', '-');

            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            ReportViewer2.LocalReport.DataSources.Clear();
            ReportViewer2.LocalReport.DataSources.Add(datasource2);

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

            // Render PDF ต้นฉบับ
            renderedBytes = ReportViewer2.LocalReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
            );
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
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "*" + cmno + "*", 461, 742, 0);
                        //cb.ShowTextAligned(Element.ALIGN_LEFT, "*"+ cmsib + "*", 448, 742, 0); 
                        cb.EndText();
                    }
                }

                renderedBytes = ms.ToArray();
                reader.Close();
            }

            // ใช้ iTextSharp รวม PDF ซ้ำ 4 ชุด
            byte[] finalPdf;
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (Document doc = new Document())
                {
                    PdfCopy copy = new PdfCopy(doc, outputStream);
                    doc.Open();

                    PdfReader reader = new PdfReader(renderedBytes);
                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        for (int i = 0; i < 4; i++) // จำนวน copy = 4
                        {
                            copy.AddPage(copy.GetImportedPage(reader, page));
                        }
                    }
                    reader.Close();
                    doc.Close();
                }
                finalPdf = outputStream.ToArray();
            }

            // ส่งออกเป็น Response PDF
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=RequestClaim-" + cmsib + "-" + Cus + "-" + slm + ".pdf");
            Response.BinaryWrite(finalPdf);
            Response.End();
        }

    }
}