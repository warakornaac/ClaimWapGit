using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;
using System.Data;
using ClaimWap.Controllers;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace ClaimWap.Controllers
{
    public class Checkstatus_ScController : Controller
    {
        //
        // GET: /Checkstatus_Sc/

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
                if (UserType == "3")
                {
                    return RedirectToAction("Index", "ProcessApprove");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
                else if (UserType == "8" || UserType == "9")
                {
                    return RedirectToAction("Index", "Disposal");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
                else if (UserType == "10")
                {
                    return RedirectToAction("Index", "CheckstatusRt");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
               // else
               // {
                   // return RedirectToAction("Index", "Checkstatus_Sc");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                    ViewBag.Company = Company; 
                //}
             
            }
            return View();
        }
       
       
        public JsonResult UploadPdf(HttpPostedFileBase filex, string UserID)
        {
            string message = string.Empty;
            string imgname = string.Empty;
            string path = string.Empty;
            try
            {
                string ImageName = Guid.NewGuid().ToString();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file

                    //Use the following properties to get file's name, size and MIMEType
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;

                    System.IO.Stream fileContent = file.InputStream;
                    //To save file, use SaveAs method
                   // file.SaveAs(Server.MapPath(@"~/PdfUpload") + fileName+ ".pdf"); //File will be saved in application root
                    file.SaveAs(Server.MapPath(@"~\PdfUpload\" + fileName + ".pdf")); //File will be saved in application root
                   
                    imgname = fileName + ".pdf";
                    message = "true";
                    //if (file != null && file.ContentLength > 0)
                    //{

                    //    path = Path.Combine(Server.MapPath("~/PdfUpload"),
                    //                               Path.GetFileName(imgname));
                    //    file.SaveAs(path);
                    //    message = "true";


                    //}
                    

                }
            }
            catch (Exception ex)
            {
                message = ex.Message ;
            }

            return Json(new { message, imgname, path }, JsonRequestBehavior.AllowGet);
            //return Json("Uploaded " + Request.Files.Count + " files");
        }
        public JsonResult SavePathPdf(string im_name, string Cim_No, string Im_No, string inCim_NoSub,string type)
        {
            string message = string.Empty;

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_PathPdf", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inim_name", im_name);
                command.Parameters.AddWithValue("@inCim_NoSub", inCim_NoSub);
                command.Parameters.AddWithValue("@inCim_No", Cim_No);
                command.Parameters.AddWithValue("@inIm_No", Im_No);
                command.Parameters.AddWithValue("@intype", type);
                command.ExecuteNonQuery();

                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Updateafterdateprocess(string User,string DataSend)
        {
            string message = string.Empty;
            List<Datasave> _ItemList = new JavaScriptSerializer().Deserialize<List<Datasave>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_update_Afterprocess", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inUser", _ItemList[i].user);
                    command.Parameters.AddWithValue("@inCim_No", _ItemList[i].req_no);
                    command.Parameters.AddWithValue("@inCim_NoSub", _ItemList[i].clm_no_sub);
                    command.Parameters.AddWithValue("@inScrapdate", _ItemList[i].scrapdatetec1);

                    command.ExecuteNonQuery();

                    command.Dispose();
                }
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Updatercvstatusprocess(string User, string DataSend)
        {
            string message = string.Empty;
            List<Datasave> _ItemList = new JavaScriptSerializer().Deserialize<List<Datasave>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_updatePrintordercar", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inUser", _ItemList[i].user);
                    command.Parameters.AddWithValue("@inCim_No", _ItemList[i].req_no);
                    command.Parameters.AddWithValue("@inCim_NoSub", _ItemList[i].clm_no_sub);
                   

                    command.ExecuteNonQuery();

                    command.Dispose();
                }
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClimcheckdata(string inCLM_ID, string incompany, string instatus, string incusno, string initemno, string instatdate, string inenddate, string insales, string instatusrec, string instatusclaimafter, string instkgrp,string statusprint,string proceedtofter)  
        {
            string message = string.Empty;
            ClimeDetail model = null;
            List<ClimeListDetail> Getdata = new List<ClimeListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessCheckStstusClaim", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 150; // 2.30 นาที    
            command.Parameters.AddWithValue("@inDOC", inCLM_ID);
	        command.Parameters.AddWithValue("@inDOCSUB" ,"");
            command.Parameters.AddWithValue("@inCOM", incompany);
            command.Parameters.AddWithValue("@inCUS", incusno);
            command.Parameters.AddWithValue("@inSLMCOD", insales);
            command.Parameters.AddWithValue("@inSTKCOD", initemno);
            command.Parameters.AddWithValue("@inSTATS", instatus);
            command.Parameters.AddWithValue("@inSTATSREC", instatusrec);
            command.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            command.Parameters.AddWithValue("@inRequestenddate", inenddate);
            command.Parameters.AddWithValue("@instatusclaimafter", "0");
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@inproceed", proceedtofter);
            command.Parameters.AddWithValue("@inprint", statusprint);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ClimeDetail();
                model.CLM_ADMIN = dr["CLM_ADMIN"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.CLM_REMARK = dr["CLM_REMARK"].ToString();
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString(); 
                model.FCLM_QTY = dr["FCLM_QTY"].ToString();
                model.CLM_QTY_ORG = dr["CLM_QTY_ORG"].ToString();
                model.CLM_QTY_PASS = dr["CLM_QTY_PASS"].ToString();
                model.CLM_QTY_REJECT = dr["CLM_QTY_REJECT"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_DUEDATE = dr["CLM_DUEDATE"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.Technician = dr["Technician"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.TECH1_NAME = dr["TECH1_NAME"].ToString();
                model.TECH1_ANLYS_DATE = dr["TECH1_ANLYS_DATE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                model.ANLYS_AFTERPROCESS = dr["ANLYS_AFTERPROCESS"].ToString();
                model.SCRAP_DATE = dr["SCRAP_DATE"].ToString();
                model.TECH1_ANLYS_RESULT = dr["TECH1_ANLYS_RESULT"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.TECH2_REMARK = dr["TECH2_REMARK"].ToString();
                model.TECH2_ANLYS_DATE = dr["TECH2_ANLYS_DATE"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.PM_Replacement = dr["PM_Replacement"].ToString();
                model.PM_REMARK = dr["PM_REMARK"].ToString();

                model.SM_NAME = dr["SM_NAME"].ToString();
                model.SM_APPRV_DATE = dr["SM_APPRV_DATE"].ToString();
                model.SM_PROCESS_STATUS = dr["SM_PROCESS_STATUS"].ToString();
                model.SM_REMARK = dr["SM_REMARK"].ToString();

                model.GM_NAME = dr["GM_NAME"].ToString();
                model.GM_APPRV_DATE = dr["GM_APPRV_DATE"].ToString();
                model.GM_PROCESS_STATUS = dr["GM_PROCESS_STATUS"].ToString();
                model.GM_REMARK = dr["GM_REMARK"].ToString();

                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.CLM_UPDATE_DATE = dr["CLM_UPDATE_DATE"].ToString();
                model.CLM_UPDATE_BY = dr["CLM_UPDATE_BY"].ToString();
                //model.CLM_UPDATE_DATE = "";
               // model.CLM_UPDATE_BY = "";
                model.Customer = dr["CUSCOD"].ToString();
                model.REQ_NO = dr["REQ_NO"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.CLM_FRMSUP_STATUS = dr["CLM_FRMSUP_STATUS"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.STATUSTEXT = dr["STATUSTEXT"].ToString();
                model.CLM_PERFORMTEXT = dr["CLM_PERFORMTEXT"].ToString();
                model.ANLYS_AFTERPROCESSTEXT = dr["ANLYS_AFTERPROCESSTEXT"].ToString();
                model.CLM_FRMSUP_DATE = dr["CLM_FRMSUP_DATE"].ToString();
                model.CLM_FRMSUP_NO = dr["CLM_FRMSUP_NO"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.CLM_FOC = dr["CLM_Foc"].ToString();
                model.CS_No = dr["CS_No"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                model.Print_statusCN = dr["Print_statusCN"].ToString();
                model.CLM_CLAIMNOTE = dr["CLM_ClaimNote"].ToString();
                Getdata.Add(new ClimeListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public JsonResult Getafterdateprocess(string req_no, string clm_no_sub)
        {
            string message = string.Empty;
            string afterprocess =  string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                SqlCommand command = new SqlCommand("select convert(char(10),SCRAP_DATE,126) as SCRAP_DATE from Claim_Line where [REQ_NO] =N'" + req_no + "' and  [CLM_NO_SUB] =N'" + clm_no_sub + "'", Connection);

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    afterprocess = dr["SCRAP_DATE"].ToString();


                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
           
            Connection.Close();
            return Json(new { afterprocess }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveClaimsupplier(string inCLM_ID, string inCLM_SUB, string inCLM_FRMSUP_NO, string inCLM_FRMSUP_STATUS,string inusrlogin)
        {


            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_supplier", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inCLM_SUB", inCLM_SUB);
                command.Parameters.AddWithValue("@inCLM_FRMSUP_NO", inCLM_FRMSUP_NO);
                command.Parameters.AddWithValue("@inCLM_FRMSUP_STATUS", inCLM_FRMSUP_STATUS);
                command.Parameters.AddWithValue("@inusrlogin",inusrlogin);
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
        public JsonResult CountPrint(string User,  string clm_no_sub)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Count_Print", Connection);
                command.CommandType = CommandType.StoredProcedure;
         
                command.Parameters.AddWithValue("@inCLM_SUB", clm_no_sub);
                command.Parameters.AddWithValue("@inusrlogin", User);



                command.ExecuteNonQuery();

                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Cancelclaim(string User,string req_no,string clm_no_sub){
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Cancel_Claim", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", req_no);
                command.Parameters.AddWithValue("@inCLM_SUB", clm_no_sub);
                command.Parameters.AddWithValue("@inusrlogin", User);
              


                command.ExecuteNonQuery();
               
                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message}, JsonRequestBehavior.AllowGet);
        }
    }

}
