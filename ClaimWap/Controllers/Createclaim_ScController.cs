using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;
using System.IO;
using System.Web.Script.Serialization;
namespace ClaimWap.Controllers
{
    public class Createclaim_ScController : Controller
    {
        //
        // GET: /Createclaim_Sc/

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
                // string Docsub = string.Empty;
                string Docdisplay = string.Empty;
                string Docwords = string.Empty;
                // string SubDocwords = string.Empty;
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



            }
            return View();
        }

        [HttpPost]
        public JsonResult UploadVideo(HttpPostedFileBase filex, string aj_CLM_NO_SUB, string aj_CLM_NO)
        {
            string fileName = string.Empty;
            string message = string.Empty;
            int Size = 0;
            // if (fileupload != null)
            // {
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    fileName = Path.GetFileName(file.FileName);
                    int fileSize = file.ContentLength;
                    Size = fileSize / 1000;
                    file.SaveAs(Server.MapPath(@"~\VideoFileUpload\" + fileName + ".mp4"));


                }
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { fileName, message, Size }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Savefilevideo(HttpPostedFileBase filex, string aj_CLM_NO_SUB, string aj_CLM_NO, string im_name, string Size, string Im_No)
        {
            string fileName = string.Empty;
            string message = string.Empty;
            // if (fileupload != null)
            // {
            try
            {


                var CS = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
                // string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spAddNewVideoFile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@inCim_No", aj_CLM_NO);
                    cmd.Parameters.AddWithValue("@inCim_NoSub", aj_CLM_NO_SUB);
                    cmd.Parameters.AddWithValue("@Name", im_name);
                    cmd.Parameters.AddWithValue("@FileSize", Size);
                    cmd.Parameters.AddWithValue("@inImg_ID", Im_No);
                    cmd.Parameters.AddWithValue("FilePath", "~/VideoFileUpload/" + im_name + ".mp4");
                    cmd.ExecuteNonQuery();


                    cmd.Dispose();
                    con.Close();

                }
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { fileName, message }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public JsonResult SaveToTemp(HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        string filename = "";
        //        string imgepath = "Null";
        //        if (file != null)
        //        {
        //            filename = file.FileName;
        //            imgepath = filename;
        //            string extension = Path.GetExtension(file.FileName);
        //            filename = DateTime.Now.Ticks + filename;
        //            var path = Path.Combine(Server.MapPath("~/Temp/"), filename);
        //            file.SaveAs(path);
        //        }
        //        return Json(filename, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// This method is used to move files from Temp folder to Destinatin folder.
        /// </summary>
        /// <returns></returns>
        //public JsonResult SaveToMainFolder()
        //{
        //    //This method has been copied from here:https://stackoverflow.com/a/15140431/5202777 
        //    string fileName = "";
        //    string destFile = "";
        //    string sourcePath = Server.MapPath("~/Temp/");
        //    string targetPath = Server.MapPath(@"~\ImgUpload\");

        //    if (System.IO.Directory.Exists(sourcePath))
        //    {
        //        string[] files = System.IO.Directory.GetFiles(sourcePath);
        //        // Copy the files. 
        //        foreach (string file in files)
        //        {
        //            fileName = System.IO.Path.GetFileName(file);
        //            destFile = System.IO.Path.Combine(targetPath, fileName);
        //            System.IO.File.Copy(file, destFile, true);
        //        }
        //        RemoveFiles();
        //    }
        //    return Json("All Files saved Successfully.", JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// Make Temp folder empty once all files are copied to destination folder.
        /// </summary>
        //public void RemoveFiles()
        //{
        //    string sourcePath = Server.MapPath("~/Temp/");
        //    string[] files = System.IO.Directory.GetFiles(sourcePath);
        //    foreach (string file in files)
        //    {
        //        if (System.IO.File.Exists(System.IO.Path.Combine(sourcePath, file)))
        //        {
        //            try
        //            {
        //                System.IO.File.Delete(file);
        //            }
        //            catch (System.IO.IOException e)
        //            {
        //                return;
        //            }
        //        }
        //    }
        //}
        [HttpPost]
        public ActionResult UploadFiles(FormCollection formCollection)
        {
            //int pussend = 0;
            //int pus = 0;
            int countPass = 0;
            int countError = 0;
            string message = string.Empty;
            string alertmessage = string.Empty;
            string uname = string.Empty;
            string Pathimg = string.Empty;
            string path = Server.MapPath(@"~\ImgUpload\");
            HttpFileCollectionBase files = Request.Files;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    bool allowSave = true;
                    string name = formCollection["uploadername"];
                    string inCim_NoSub = formCollection["inCim_NoSub"];
                    string No = formCollection["No"];
                    //Pathimg = name + "-" + pussend + ".png";
                    var command = new SqlCommand("P_Save_PathImage", Connection);
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
                    message = "true";
                    //pus = i;
                    //pussend = (pus + 1);
                    HttpPostedFileBase file = files[i];
                    string fileName = file.FileName;
                    //file.SaveAs(path + file.FileName);
                    string fullPath = Server.MapPath("~/ImgUpload/" + uname);
                    file.SaveAs(fullPath);
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


                                message = ex.Message;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }

            Connection.Close();
            countPass = files.Count - countError;
            alertmessage = countPass + " Files uploaded!" + " \n" + countError + " Unable upload file!";
            return Json(new { message, alertmessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase filex, string UserID)
        {
            string message = string.Empty;
            string imgname = string.Empty;
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
                    // file.SaveAs(Server.MapPath("~/") + fileName); //File will be saved in application root
                    file.SaveAs(Server.MapPath(@"~\ImgUpload\" + fileName + ".png")); //File will be saved in application root

                    imgname = fileName + ".png";
                    message = "true";

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message, imgname }, JsonRequestBehavior.AllowGet);
            //return Json("Uploaded " + Request.Files.Count + " files");
        }
        public JsonResult Gendocno(string Gendoc, string Clno, string Com)
        {
            string Docnocm = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Gen_Doc_Control", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inDocNam", Gendoc);
            command.Parameters.AddWithValue("@inClno", Clno);
            command.Parameters.AddWithValue("@inCom", Com);
            SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
            returnValuedoc.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(returnValuedoc);
            Connection.Open();
            command.ExecuteNonQuery();
            Docnocm = returnValuedoc.Value.ToString();


            command.Dispose();
            Connection.Close();

            return Json(new { Docnocm }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveClimtemp(string incontactcus, string inclaimcontactcus, string installdatecus, string inreminqtynew, string infoc, string inINV_QTY, string inCLM_ID, string CLM_NO, string InCom, string CUSCOD, string inSTKCOD, string inSTKDES, string inCLM_UOM, string inCLM_INVNO, string inCLM_INVDATE, string inCLM_QTY, string inCLM_USEDAY, string inCLM_CAUSE, string inCLM_PERFORM, string inCLM_RCVSTATUS, string inCLM_RCVBY, string inCLM_RCVDATE, string vehicle, string modeltype, string modelyear, string enginecode, string chassisno, string pump, string typeofProduct, string warrantycardno, string milage, string dateofdamage, string BatchCode, string customer_claim_note)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_Claim_temp", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inCOM", InCom);
                command.Parameters.AddWithValue("@inCLM_NO", CLM_NO);
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                command.Parameters.AddWithValue("@inSTKCOD", inSTKCOD);
                command.Parameters.AddWithValue("@inSTKDES", inSTKDES);
                command.Parameters.AddWithValue("@inCLM_UOM", inCLM_UOM);
                command.Parameters.AddWithValue("@inCLM_INVNO", inCLM_INVNO);
                command.Parameters.AddWithValue("@inINV_QTY", inINV_QTY);
                command.Parameters.AddWithValue("@inCLM_INVDATE", inCLM_INVDATE);
                command.Parameters.AddWithValue("@inCLM_QTY", inCLM_QTY);
                command.Parameters.AddWithValue("@inCLM_USEDAY", inCLM_USEDAY);
                command.Parameters.AddWithValue("@inCLM_CAUSE", inCLM_CAUSE);
                command.Parameters.AddWithValue("@inCLM_PERFORM", inCLM_PERFORM);
                command.Parameters.AddWithValue("@inCLM_RCVSTATUS", inCLM_RCVSTATUS);
                command.Parameters.AddWithValue("@inCLM_RCVBY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCLM_RCVDATE", inCLM_RCVDATE);
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
                command.Parameters.AddWithValue("@inCLM_BatchCode", BatchCode);
                command.Parameters.AddWithValue("@inFoc", infoc);
                command.Parameters.AddWithValue("@ininreminqtynew", inreminqtynew);
                command.Parameters.AddWithValue("@installdatecus", installdatecus);
                command.Parameters.AddWithValue("@inclaimcontactcus", inclaimcontactcus);
                command.Parameters.AddWithValue("@incontactcus", incontactcus);
                command.Parameters.AddWithValue("@inClaimNote", customer_claim_note);
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
        public JsonResult SaveClim(string imgsignatureman, string imgsignature, string inqtybox, string inqtyunit, string inCLM_ID, string inCLM_RCVBY, string inCusShipping, string incodeShipping, string inremakeclaim)
        {
            string message = string.Empty;
            string cmid = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_Claim", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inREQ_BY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCusShipping", inCusShipping);
                command.Parameters.AddWithValue("@incodeShipping", incodeShipping);
                command.Parameters.AddWithValue("@inremakeclaim", inremakeclaim);
                command.Parameters.AddWithValue("@inqtybox", inqtybox);
                command.Parameters.AddWithValue("@inqtyunit", inqtyunit);
                command.Parameters.AddWithValue("@inimgsignature", imgsignature);
                command.Parameters.AddWithValue("@inimgsignatureman", imgsignatureman);
                SqlParameter returnValuedoc = new SqlParameter("@outGenDoc", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                cmid = returnValuedoc.Value.ToString();
                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, cmid }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePathImagetemp(string DataSend)
        {
            string message = string.Empty;
            List<itemimg> _ItemList = new JavaScriptSerializer().Deserialize<List<itemimg>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_Save_PathImage", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inim_name", _ItemList[i].Img);
                    command.Parameters.AddWithValue("@inCim_NoSub", _ItemList[i].inCim_NoSub);
                    command.Parameters.AddWithValue("@inCim_No", _ItemList[i].Cim_No);
                    command.Parameters.AddWithValue("@inIm_No", _ItemList[i].Im_No);
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
        public JsonResult DeleteClaimtemp(string comid, string clmnoup)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_DelClaim_temp", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DOC", comid);
                command.Parameters.AddWithValue("@SUBDOC", clmnoup);
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
        public JsonResult GetClimtemp(string inCLM_ID)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Claim_temp", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", inCLM_ID);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Climedata();
                model.CLM_ID = dr["CLM_ID"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.CLM_INVDATE = dr["CLM_INVDATE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.Status = dr["Status"].ToString();
                model.CLM_Machine = dr["CLM_Machine"].ToString();
                model.CLM_Model = dr["CLM_Model"].ToString();
                model.CLM_ModelYear = dr["CLM_ModelYear"].ToString();
                model.CLM_EngineCode = dr["CLM_EngineCode"].ToString();
                model.CLM_ChassisNo = dr["CLM_ChassisNo"].ToString();
                model.CLM_InjecPump = dr["CLM_InjecPump"].ToString();
                model.CLM_TypeProduct = dr["CLM_TypeProduct"].ToString();
                model.CLM_WarrantyNo = dr["CLM_WarrantyNo"].ToString();
                model.CLM_Milage = dr["CLM_Milage"].ToString();
                model.CLM_DateDamage = dr["CLM_DateDamage"].ToString();
                model.CLM_BatchCode = dr["CLM_BatchCode"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                model.CLM_Installdate = dr["CLM_Installdate"].ToString();
                model.CLM_Contact = dr["CLM_Contact"].ToString();
                model.CLM_ContactTel = dr["CLM_ContactTel"].ToString();
                model.CLM_ClaimNote = dr["CLM_ClaimNote"].ToString();
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }

        public JsonResult Deleteimage(string comid, string clmnoup, string clmidimg, string absPath)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_Delimage", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DOC", comid);
                command.Parameters.AddWithValue("@SUBDOC", clmnoup);
                command.Parameters.AddWithValue("@CLMIMAGENO", clmidimg);
                command.ExecuteNonQuery();
                command.Dispose();

                message = "true";
                var root = @"..\ImgUpload\";

                if (message == "true")
                {
                    string tempFilePathWithFileName = Path.GetTempFileName();
                    string filePath = Path.Combine(root, absPath);
                    string filePath1 = Server.MapPath(filePath);
                    if (System.IO.File.Exists(filePath1))
                    {


                        System.IO.File.Delete(filePath1);


                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteFileVideo(string comid, string clmnoup, string clmidimg, string absPath, string Im_No)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_Delvideo", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DOC", comid);
                command.Parameters.AddWithValue("@SUBDOC", clmnoup);
                command.Parameters.AddWithValue("@inImg_ID", Im_No);
                command.ExecuteNonQuery();
                command.Dispose();

                message = "true";
                var root = @"..\VideoFileUpload\";

                if (message == "true")
                {
                    string tempFilePathWithFileName = Path.GetTempFileName();
                    string filePath = Path.Combine(root, absPath);
                    string filePath1 = Server.MapPath(filePath);
                    if (System.IO.File.Exists(filePath1))
                    {


                        System.IO.File.Delete(filePath1);


                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }
    }
}
