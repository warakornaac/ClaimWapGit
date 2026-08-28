using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Controllers;
using ClaimWap.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ClaimWap.Controllers
{
    public class Createsalesreturn_salesController : Controller
    {
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
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;

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

                }
                ViewBag.Srtno = Doc;



            }
            return View();
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
        public JsonResult SaveClimtemp(string infoc, string incasebec, string innet_price, string inCLM_ID, string CLM_NO, string CUSCOD, string inSTKCOD, string inSTKDES, string inCLM_UOM, string inCLM_INVNO, string inINV_QTY, string inCLM_INVDATE, string inCLM_QTY, string inCLM_CAUSE, string inCLM_CAUSE2, string inCLM_PERFORM, string inCLM_RCVSTATUS, string inCLM_RCVBY, string inCLM_RCVDATE, string InCom)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_Salesreturn_temp", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inCOM", InCom);
                command.Parameters.AddWithValue("@inCLM_NO", CLM_NO);
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                command.Parameters.AddWithValue("@inSTKCOD", inSTKCOD);
                command.Parameters.AddWithValue("@inSTKDES", inSTKDES);
                command.Parameters.AddWithValue("@inCLM_UOM", inCLM_UOM);
                command.Parameters.AddWithValue("@inCLM_INVNO ", inCLM_INVNO);
                command.Parameters.AddWithValue("@inCLM_INVDATE ", inCLM_INVDATE);
                command.Parameters.AddWithValue("@inCLM_QTY", inCLM_QTY);
                command.Parameters.AddWithValue("@inCLM_CAUSE  ", inCLM_CAUSE);
                command.Parameters.AddWithValue("@inCLM_CAUSE2", inCLM_CAUSE2);
                command.Parameters.AddWithValue("@inCLM_PERFORM   ", inCLM_PERFORM);
                command.Parameters.AddWithValue("@inCLM_RCVSTATUS ", inCLM_RCVSTATUS);
                command.Parameters.AddWithValue("@inCLM_RCVBY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCLM_RCVDATE ", inCLM_RCVDATE);
                command.Parameters.AddWithValue("@inINV_QTY", inINV_QTY);
                command.Parameters.AddWithValue("@inFoc", infoc);
                command.Parameters.AddWithValue("@innet_price", innet_price);
                command.Parameters.AddWithValue("@incasebec", incasebec);
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
        public JsonResult SaveClim(string imgsignatureman, string imgsignature, string inqtybox, string inCLM_ID, string inCLM_RCVBY, string inCusShipping, string incodeShipping, string inremakeclaim)
        {
            string message = string.Empty;
            string cmid = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_SalesReturn", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inREQ_BY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCusShipping", inCusShipping);
                command.Parameters.AddWithValue("@incodeShipping", incodeShipping);
                command.Parameters.AddWithValue("@inremakeclaim", inremakeclaim);
                command.Parameters.AddWithValue("@inqtybox", inqtybox);
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
        public JsonResult GetClimtemp(string inCLM_ID)
        {
            string message = string.Empty;
            ClimedataRt model = null;
            List<ClimeRttempListDetail> Getdata = new List<ClimeRttempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_SalesReturn_temp", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", inCLM_ID);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ClimedataRt();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.UOM = dr["UOM"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_QTY = dr["STMP_QTY"].ToString();
                model.STMP_CASE = dr["STMP_CASE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.InsertDate = dr["InsertDate"].ToString();
                model.COMPANY = dr["COMPANY"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.Status = dr["Status"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                model.STMP_ReasonCodes =  dr["STMP_ReasonCodes"].ToString();
                model.Resoncodedes =   dr["Resoncodedes"].ToString();
                model.FOC = dr["FOC"].ToString();
                model.Price = dr["Price"].ToString();
                Getdata.Add(new ClimeRttempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult DeleteClaimtemp(string comid, string clmnoup)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_DelClaim_temp_RT", Connection);
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

    }
}
