using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;

namespace ClaimWap.Controllers
{
    public class Process_ScController : Controller
    {
        //
        // GET: /Process_Sc/

        public ActionResult Index()
        {

            if (Session["UserID"] == null && Session["UserPassword"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            else
            {
                string User = Session["UserID"].ToString();
                string UserType = Session["UserType"].ToString();
                string Company = Session["company"].ToString();
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                ViewBag.Company = Company;
                string Doc = string.Empty;
                string Docsub = string.Empty;
                string Docdisplay = string.Empty;
                string Docwords = string.Empty;
                string SubDocwords = string.Empty;
                Docdisplay = Request.QueryString["ClmNUM"];

                if (Docdisplay != null)
                {
                    string[] words = Docdisplay.Split('/');
                    Docwords = words[0];
                    byte[] data = System.Convert.FromBase64String(Docwords);
                    Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

                   // SubDocwords = words[1];
                   // byte[] datasub = System.Convert.FromBase64String(SubDocwords);
                   // Docsub = System.Text.ASCIIEncoding.ASCII.GetString(datasub);

                   
                   // Doc = Docdisplay;
                   // Docsub = Docdisplay;
                }
                ViewBag.Claimno = Doc;
               // ViewBag.Claimsubno = Docsub;
            }
            return View();
        }
        public JsonResult SaveProcessClaimDetailTec2(string clamtyp, string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_TECH2_NAME, string aj_TECH2_APPRV_STATUS, string aj_TECH2_REMARK, string aj_TECH2_APPRV_DATE, string aj_TECH2_QTY_ORG, string aj_TECH2_QTY_PASS, string aj_TECH2_QTY_REJECT, string aj_TECH2_problemCategory, string aj_TECH2_QTY_problemCategoryDetail, string aj_TECH2_QTY_WR, string aj_TECH2_QTY_WE)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_DetailTec2", Connection);
                 command.CommandType = CommandType.StoredProcedure;
                 command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                 command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                 command.Parameters.AddWithValue("@inTECH2_NAME", aj_TECH2_NAME);
                 command.Parameters.AddWithValue("@inTECH2_APPRV_STATUS", aj_TECH2_APPRV_STATUS);
                 command.Parameters.AddWithValue("@inTECH2_REMARK", aj_TECH2_REMARK);
                 command.Parameters.AddWithValue("@inTECH2_APPRV_DATE", aj_TECH2_APPRV_DATE);
                 command.Parameters.AddWithValue("@inTECH2_QTY_ORG", aj_TECH2_QTY_ORG);
                 command.Parameters.AddWithValue("@inTECH2_QTY_PASS", aj_TECH2_QTY_PASS);
                 command.Parameters.AddWithValue("@inTECH2_QTY_REJECT", aj_TECH2_QTY_REJECT);
                 command.Parameters.AddWithValue("@inTECH2_QTY_problemCategory", aj_TECH2_problemCategory);
                 command.Parameters.AddWithValue("@inTECH2_QTY_problemCategoryDetail", aj_TECH2_QTY_problemCategoryDetail);
                 command.Parameters.AddWithValue("@inTECH2_QTY_WR", aj_TECH2_QTY_WR);
                 command.Parameters.AddWithValue("@inTECH2_QTY_WE", aj_TECH2_QTY_WE);
                 command.Parameters.AddWithValue("@inWarrantyClmType", clamtyp);
               
               
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                subno = returnValuedoc.Value.ToString();
                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult saveProcessAfterRejected(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_ANLYS_AFTERPROCESS,string usrname)
        {
            string message = string.Empty;
            string subno = string.Empty;
            string genStatus = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_AfterReject_Claim", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", aj_REQ_NO);
            command.Parameters.AddWithValue("@inCLM_SUB", aj_CLM_NO_SUB);
            command.Parameters.AddWithValue("@inRemark", aj_ANLYS_AFTERPROCESS);
            command.Parameters.AddWithValue("@inusrlogin", usrname);

                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);

                command.ExecuteNonQuery();
                subno = returnValuedoc.Value.ToString();
                command.Dispose();
                genStatus = returnValuedoc.Value?.ToString() ?? "N";
                message = genStatus == "Y" ? "true" : "false";

            }
            catch (Exception ex)
            {
                genStatus = "N";
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, genStatus }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProcessClaimDetailPM(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_PM_NAME,string aj_PM_APPRV_STATUS,string aj_PM_REMARK,string aj_PM_APPRV_DATE,string aj_PM_Replacement)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_DetailPM", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inPM_NAME", aj_PM_NAME);
                command.Parameters.AddWithValue("@inPM_APPRV_STATUS", aj_PM_APPRV_STATUS);
                command.Parameters.AddWithValue("@inPM_REMARK", aj_PM_REMARK);
                command.Parameters.AddWithValue("@inPM_APPRV_DATE", aj_PM_APPRV_DATE);
                command.Parameters.AddWithValue("@inPM_Replacement", aj_PM_Replacement);
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                subno = returnValuedoc.Value.ToString();
                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProcessClaimDetail(string datereturn ,string namereturn ,string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_TECH1_NAME, string aj_TECH1_ANLYS_DATE, string aj_CLM_PERFORM, string aj_TECH1_ANLYS_STATUS, string aj_ANLYS_AFTERPROCESS, string aj_SCRAP_DATE, string aj_TECH1_ANLYS_RESULT, string vehicle, string modeltype, string modelyear, string enginecode, string chassisno, string pump, string typeofProduct, string warrantycardno, string milage, string dateofdamage, string inCLM_USEDAY, string BatchCode, string aj_CLM_SHELF_LOCATION, string aj_Technician)
        {
            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_Detail", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
            
				command.Parameters.AddWithValue("@inTECH1_NAME", aj_TECH1_NAME);
                command.Parameters.AddWithValue("@inTECH1_ANLYS_DATE", aj_TECH1_ANLYS_DATE);
                command.Parameters.AddWithValue("@inCLM_PERFORM", aj_CLM_PERFORM);
                command.Parameters.AddWithValue("@inTECH1_ANLYS_STATUS", aj_TECH1_ANLYS_STATUS);
                command.Parameters.AddWithValue("@inANLYS_AFTERPROCESS", aj_ANLYS_AFTERPROCESS);
                command.Parameters.AddWithValue("@inSCRAP_DATE",aj_SCRAP_DATE);
                command.Parameters.AddWithValue("@inTECH1_ANLYS_RESULT", aj_TECH1_ANLYS_RESULT);
                //command.Parameters.AddWithValue("@inCLM_ADMIN", aj_CLM_NO_SUB);
                //command.Parameters.AddWithValue("@inCLM_LineAMT", aj_CLM_LineAMT);
                //command.Parameters.AddWithValue("@inCLM_LineDiscountAMT", aj_CLM_LineDiscountAMT);
                //command.Parameters.AddWithValue("@inCLM_LineDiscPercent", aj_CLM_LineDiscPercent);
                //command.Parameters.AddWithValue("@inCLM_RCVSTATUS", aj_CLM_RCVSTATUS);
                //command.Parameters.AddWithValue("@inCLM_RCVBY", aj_CLM_RCVBY);
                //command.Parameters.AddWithValue("@inCLM_REQQTY", aj_CLM_QTY);
                //// command.Parameters.AddWithValue("@inCLM_DTE", aj_CLM_DATE);
                //command.Parameters.AddWithValue("@inCLM_ADMIN", aj_CLM_ADMIN);
                command.Parameters.AddWithValue("@inCLM_SHELF_LOCATION", aj_CLM_SHELF_LOCATION);
                //command.Parameters.AddWithValue("@inCLM_REMARK", aj_CLM_REMARK);
                //command.Parameters.AddWithValue("@inCLM_RCVDATE", aj_CLM_RCVDATE);
                //command.Parameters.AddWithValue("@inCLM_UOM", aj_CLM_UOM);
                //command.Parameters.AddWithValue("@inCLM_DUEDATE", aj_CLM_DUEDATE);
                //command.Parameters.AddWithValue("@inCLM_DATE", aj_CLM_DATE);
                command.Parameters.AddWithValue("@inTechnician", aj_Technician);
                //command.Parameters.AddWithValue("@instkgrp", aj_stkgrp);
                //command.Parameters.AddWithValue("@inTECH1_NAME", aj_TECH1_NAME);
                //command.Parameters.AddWithValue("@inTECH1_ANLYS_DATE", aj_TECH1_ANLYS_DATE);
                //command.Parameters.AddWithValue("@inCLM_PERFORM", aj_CLM_PERFORM);
                //command.Parameters.AddWithValue("@inTECH1_ANLYS_STATUS", aj_TECH1_ANLYS_STATUS);
                //command.Parameters.AddWithValue("@inANLYS_AFTERPROCESS", aj_ANLYS_AFTERPROCESS);
                //command.Parameters.AddWithValue("@inSCRAP_DATE", aj_SCRAP_DATE);
                //command.Parameters.AddWithValue("@inTECH1_ANLYS_RESULT", aj_TECH1_ANLYS_RESULT);

                command.Parameters.AddWithValue("@inCLM_Machine", vehicle);
                command.Parameters.AddWithValue("@inCLM_Model", modeltype);
                command.Parameters.AddWithValue("@inCLM_ModelYear", modelyear);
                command.Parameters.AddWithValue("@inCLM_EngineCode", enginecode);
                command.Parameters.AddWithValue("@inCLM_ChassisNo", chassisno);
                command.Parameters.AddWithValue("@inCLM_InjecPump", pump);
                command.Parameters.AddWithValue("@inCLM_TypeProduct", typeofProduct);
                command.Parameters.AddWithValue("@inCLM_WarrantyNo", warrantycardno);
                command.Parameters.AddWithValue("@inCLM_Milage", milage);
                command.Parameters.AddWithValue("@inCLM_DateDamage", dateofdamage);
                command.Parameters.AddWithValue("@inCLM_USEDAY", inCLM_USEDAY);
                command.Parameters.AddWithValue("@inCLM_BatchCode", BatchCode);
                //command.Parameters.AddWithValue("@innamereturne", namereturn);
                //command.Parameters.AddWithValue("@indatereturn", datereturn);
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                fag  = returnValuedoc.Value.ToString();
                if (fag == "Y")
                {




                    message = "true";
                }
                command.Dispose();
               


            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveProcessClaimUpdateAdmin(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_CLM_COMMENT, string aj_CLM_RCVBY)
        {

            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimCus_UpdateComment_Admin", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inCLM_COMMENT", aj_CLM_COMMENT);
                command.Parameters.AddWithValue("@inCLM_RCVBY", aj_CLM_RCVBY);

                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                fag = returnValuedoc.Value.ToString();
                if (fag == "Y")
                {




                    message = "true";
                }
                command.Dispose();



            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        } 
        [HttpPost]
        public ActionResult UploadFiles(FormCollection formCollection)
        {
            //int pussend = 0;
            //int pus = 0;
            int countPass = 0;
            int countError = 0;
            string uname = string.Empty;
            string Pathimg = string.Empty;
            string path = Server.MapPath(@"~\ImgUpload\");
            HttpFileCollectionBase files = Request.Files;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            for (int i = 0; i < files.Count; i++)
            {
                bool allowSave = true;
                string name = formCollection["uploadername"];
                string inCim_NoSub = formCollection["inCim_NoSub"];
                string No = formCollection["No"];
                //Pathimg = name + "-" + pussend + ".png";
                var command = new SqlCommand("P_Save_PathImage_tec", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inim_name", "");
                command.Parameters.AddWithValue("@inCim_NoSub", inCim_NoSub);
                command.Parameters.AddWithValue("@inCim_No", No);
                command.Parameters.AddWithValue("@inIm_No", "");

                SqlParameter returnValuedoc = new SqlParameter("@outimagename", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();
                //text sql
                uname = returnValuedoc.Value.ToString();
                command.Dispose();
                //pus = i;
                //pussend = (pus + 1);
                HttpPostedFileBase file = files[i];
                string fileName = file.FileName;
                string fullPath = Server.MapPath("~/ImgUpload/" + uname);
                file.SaveAs(fullPath);
                //file.SaveAs(fullPath);
                int byteCount = file.ContentLength;
                if (file.ContentType.Contains("image"))
                {
                    if (byteCount > 1048576)
                    { //เกิน 1 MB
                        try
                        {
                            System.Web.Helpers.WebImage img = new System.Web.Helpers.WebImage(fullPath);
                            if (img.Width > 1000)
                            {
                                img.Resize(1024, 768);
                                FileInfo fileInfo = new FileInfo(fullPath);
                                long sizeAfter = fileInfo.Length;
                                if (sizeAfter > 1048576)
                                { //เกิน 1 MB
                                    img.Resize(800, 600);
                                }
                                img.Save(fullPath, "png", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            allowSave = false;
                            ++countError;
                            FileInfo fileInfo = new FileInfo(fullPath);
                            fileInfo.Delete();
                            command = new SqlCommand("P_Delete_PathImage", Connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@imageName", uname);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                }
            }
            Connection.Close();
            countPass = files.Count - countError;
            return Json(countPass + " Files uploaded!" + " \n" + countError + " Unable upload file!");
        }

        public JsonResult SaveProcessClaimDetailAdmin(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_CLM_LineAMT, string aj_stkgrp, string aj_CLM_LineDiscountAMT, string aj_CLM_LineDiscPercent, string aj_CLM_RCVSTATUS, string aj_CLM_RCVBY,
             string aj_CLM_QTY, string aj_CLM_DATE, string aj_CLM_ADMIN, string aj_CLM_SHELF_LOCATION, string aj_CLM_REMARK, string aj_CLM_RCVDATE, string aj_CLM_UOM, string aj_CLM_DUEDATE, string aj_Technician,
            string vehicle, string modeltype, string modelyear, string enginecode, string chassisno, string pump, string typeofProduct, string warrantycardno, string milage, string dateofdamage, string inCLM_USEDAY, string BatchCode, string aj_ADMIN_ANLYS_STATUS,string aj_TECH1_PROCESS_STATUS,
            string Installdate ,string claimcontac, string contact )
        {
            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_Detail_Admin", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inCLM_LineAMT", aj_CLM_LineAMT);
                command.Parameters.AddWithValue("@inCLM_LineDiscountAMT", aj_CLM_LineDiscountAMT);
                command.Parameters.AddWithValue("@inCLM_LineDiscPercent", aj_CLM_LineDiscPercent);
                command.Parameters.AddWithValue("@inCLM_RCVSTATUS", aj_CLM_RCVSTATUS);
                command.Parameters.AddWithValue("@inCLM_RCVBY", aj_CLM_RCVBY);
                command.Parameters.AddWithValue("@inCLM_QTY", aj_CLM_QTY);
                command.Parameters.AddWithValue("@inADMIN_ANLYS_STATUS", aj_ADMIN_ANLYS_STATUS);
                command.Parameters.AddWithValue("@inTECH1_PROCESS_STATUS", aj_TECH1_PROCESS_STATUS);
                command.Parameters.AddWithValue("@inCLM_ADMIN", aj_CLM_ADMIN);
                //  command.Parameters.AddWithValue("@inCLM_SHELF_LOCATION", aj_CLM_SHELF_LOCATION);
                command.Parameters.AddWithValue("@inCLM_REMARK", aj_CLM_REMARK);
                command.Parameters.AddWithValue("@inCLM_RCVDATE", aj_CLM_RCVDATE);
                command.Parameters.AddWithValue("@inCLM_UOM", aj_CLM_UOM);
                command.Parameters.AddWithValue("@inCLM_DUEDATE", aj_CLM_DUEDATE);
                command.Parameters.AddWithValue("@inCLM_DATE", aj_CLM_DATE);
                // command.Parameters.AddWithValue("@inTechnician", aj_Technician);
                command.Parameters.AddWithValue("@instkgrp", aj_stkgrp);
                

                command.Parameters.AddWithValue("@inCLM_Machine", vehicle);
                command.Parameters.AddWithValue("@inCLM_Model", modeltype);
                command.Parameters.AddWithValue("@inCLM_ModelYear", modelyear);
                command.Parameters.AddWithValue("@inCLM_EngineCode", enginecode);
                command.Parameters.AddWithValue("@inCLM_ChassisNo", chassisno);
                command.Parameters.AddWithValue("@inCLM_InjecPump", pump);
                command.Parameters.AddWithValue("@inCLM_TypeProduct", typeofProduct);
                command.Parameters.AddWithValue("@inCLM_WarrantyNo", warrantycardno);
                command.Parameters.AddWithValue("@inCLM_Milage", milage);
                command.Parameters.AddWithValue("@inCLM_DateDamage", dateofdamage);
                command.Parameters.AddWithValue("@inCLM_USEDAY", inCLM_USEDAY);
                command.Parameters.AddWithValue("@inCLM_BatchCode", BatchCode);

                command.Parameters.AddWithValue("@inCLM_Installdate", Installdate);
                command.Parameters.AddWithValue("@inCLM_Contact", claimcontac);
                command.Parameters.AddWithValue("@inCLM_ContactTel", contact);
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                fag = returnValuedoc.Value.ToString();
                if (fag == "Y")
                {




                    message = "true";
                }
                command.Dispose();



            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }
    }
}
