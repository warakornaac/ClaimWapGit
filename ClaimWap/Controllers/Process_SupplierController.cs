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
using System.Web.Script.Serialization;
namespace ClaimWap.Models
{
    public class Process_SupplierController : Controller
    {
        //
        // GET: /Process_Supplier/

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
                    ViewBag.Comapny = Company;
                }

                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                ViewBag.Comapny = Company;


            }
            return View();
        }
        public JsonResult GetSupplierNo()
        {
           
             List<Supno> Listsup = new List<Supno>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_SupplierNo", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Listsup.Add(new Supno()
                {
                    No = dr["CLM_NO_Supplier"].ToString(),
                    Detail = dr["Detail"].ToString()
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(Listsup, JsonRequestBehavior.AllowGet);
           
        }
        public JsonResult GetSupplierReceiveNo()
        {

            List<Supno> Listsup = new List<Supno>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_SupplierNo_Receive", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Listsup.Add(new Supno()
                {
                    No = dr["CLM_NO_Supplier"].ToString(),
                    Detail = dr["Detail"].ToString()
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(Listsup, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetClimcheckdataSupplier(string statuswad,string supplino,string sec,string tec2 ,string inCLM_ID, string incompany, string instatus, string incusno, string initemno, string instatdate, string inenddate, string insales, string instatusrec, string instatusclaimafter, string instkgrp, string statusprint, string proceedtofter)
        {
            string message = string.Empty;
            ClimeDetail model = null;
            List<ClimeListDetail> Getdata = new List<ClimeListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessSend_Supplier", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 300;

            command.Parameters.AddWithValue("@inDOC", inCLM_ID);
            command.Parameters.AddWithValue("@inDOCSUB", "");
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
            command.Parameters.AddWithValue("@inproceed", '0');
            command.Parameters.AddWithValue("@inprint", '2');
            command.Parameters.AddWithValue("@intec2", tec2);
            command.Parameters.AddWithValue("@insec", sec);
            command.Parameters.AddWithValue("@insupplino", supplino);
            command.Parameters.AddWithValue("@instrw", statuswad);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
               model = new ClimeDetail();
                model.CLM_ADMIN = dr["CLM_ADMIN"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.CLM_REMARK = dr["CLM_REMARK"].ToString();
                model.CLM_QTY = dr["CLM_REQQTY"].ToString();
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
                model.FCLM_QTY = dr["FCLM_QTY"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.CLM_FOC = dr["CLM_Foc"].ToString();
                model.CS_No = dr["CS_No"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                model.Print_statusCN = dr["Print_statusCN"].ToString();
                model.VENDOR = dr["VENDOR"].ToString();
                model.VENDORNAME = dr["VENDORNAME"].ToString();
                model.UNIT_COST = dr["UNIT_COST"].ToString();
                model.DEPCOD = dr["DEPCOD"].ToString();
                model.AMOUNT = dr["AMOUNT"].ToString();
                model.ClaimType=dr["ClaimType"].ToString();
                model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                model.Endprocessdate = dr["PM_APPRV_DATE"].ToString();
                model.Sec = dr["Sec"].ToString();
                model.InvoiceSupplier = dr["InvoiceSupplier"].ToString();
                model.InvoiceDateSupplier = dr["InvoiceDateSupplier"].ToString();
                model.Lastqtyinv = dr["Lastqtyinv"].ToString();
                model.Lastcurinv = dr["Lastcurinv"].ToString();
                model.Lastunitcostinv = dr["Lastunitcostinv"].ToString();
                model.LastAmountinv = dr["LastAmountinv"].ToString();
                model.LastRATEinv = dr["LastRATEinv"].ToString();
                model.Cmstyp = dr["Claim_Type"].ToString();
                Getdata.Add(new ClimeListDetail { val = model });
               
            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
           
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimcheckdataSupplierWH(string statuswad,string supplino,string sec,string tec2 ,string inCLM_ID, string incompany, string instatus, string incusno, string initemno, string instatdate, string inenddate, string insales, string instatusrec, string instatusclaimafter, string instkgrp, string statusprint, string proceedtofter)
        {
            string message = string.Empty;
            ClimeDetail model = null;
            List<ClimeListDetail> Getdata = new List<ClimeListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            
            var commandWH = new SqlCommand("P_Search_ProcessSend_Supplier_WH", Connection);
            commandWH.CommandType = CommandType.StoredProcedure;

            commandWH.Parameters.AddWithValue("@inDOC", inCLM_ID);
            commandWH.Parameters.AddWithValue("@inDOCSUB", "");
            commandWH.Parameters.AddWithValue("@inCOM", incompany);
            commandWH.Parameters.AddWithValue("@inCUS", incusno);
            commandWH.Parameters.AddWithValue("@inSLMCOD", insales);
            commandWH.Parameters.AddWithValue("@inSTKCOD", initemno);
            commandWH.Parameters.AddWithValue("@inSTATS", instatus);
            commandWH.Parameters.AddWithValue("@inSTATSREC", instatusrec);
            commandWH.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            commandWH.Parameters.AddWithValue("@inRequestenddate", inenddate);
            commandWH.Parameters.AddWithValue("@instatusclaimafter", "0");
            commandWH.Parameters.AddWithValue("@instkgrp", instkgrp);
            commandWH.Parameters.AddWithValue("@inproceed", '0');
            commandWH.Parameters.AddWithValue("@inprint", '2');
            commandWH.Parameters.AddWithValue("@intec2", tec2);
            commandWH.Parameters.AddWithValue("@insec", sec);
            commandWH.Parameters.AddWithValue("@insupplino", supplino);
            commandWH.Parameters.AddWithValue("@instrw", statuswad);
            Connection.Open();
            SqlDataReader drWH = commandWH.ExecuteReader();
            while (drWH.Read())
            {
                model = new ClimeDetail();
                model.CLM_ADMIN = drWH["CLM_ADMIN"].ToString();
                model.CLM_SHELF_LOCATION = drWH["CLM_SHELF_LOCATION"].ToString();
                model.CLM_REMARK = drWH["CLM_REMARK"].ToString();
                model.CLM_QTY = drWH["CLM_REQQTY"].ToString();
                model.CLM_RCVSTATUS = drWH["CLM_RCVSTATUS"].ToString();
                model.CLM_RCVBY = drWH["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = drWH["CLM_RCVDATE"].ToString();
                model.CLM_UOM = drWH["CLM_UOM"].ToString();
                model.CLM_DUEDATE = drWH["CLM_DUEDATE"].ToString();
                model.CLM_DATE = drWH["CLM_DATE"].ToString();
                model.Technician = drWH["Technician"].ToString();
                model.STKGRP = drWH["STKGRP"].ToString();
                model.TECH1_NAME = drWH["TECH1_NAME"].ToString();
                model.TECH1_ANLYS_DATE = drWH["TECH1_ANLYS_DATE"].ToString();
                model.CLM_PERFORM = drWH["CLM_PERFORM"].ToString();
                model.TECH1_ANLYS_STATUS = drWH["TECH1_ANLYS_STATUS"].ToString();
                model.ANLYS_AFTERPROCESS = drWH["ANLYS_AFTERPROCESS"].ToString();
                model.SCRAP_DATE = drWH["SCRAP_DATE"].ToString();
                model.TECH1_ANLYS_RESULT = drWH["TECH1_ANLYS_RESULT"].ToString();
                model.TECH1_PROCESS_STATUS = drWH["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_NAME = drWH["TECH2_NAME"].ToString();
                model.TECH2_ANLYS_STATUS = drWH["TECH2_ANLYS_STATUS"].ToString();
                model.TECH2_REMARK = drWH["TECH2_REMARK"].ToString();
                model.TECH2_ANLYS_DATE = drWH["TECH2_ANLYS_DATE"].ToString();
                model.TECH2_PROCESS_STATUS = drWH["TECH2_PROCESS_STATUS"].ToString();
                model.PM_NAME = drWH["PM_NAME"].ToString();
                model.PM_APPRV_DATE = drWH["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = drWH["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = drWH["PM_PROCESS_STATUS"].ToString();
                model.PM_Replacement = drWH["PM_Replacement"].ToString();
                model.PM_REMARK = drWH["PM_REMARK"].ToString();
                model.ADMIN_ANLYS_STATUS = drWH["ADMIN_ANLYS_STATUS"].ToString();
                model.CLM_STATUS = drWH["CLM_STATUS"].ToString();
                model.CLM_UPDATE_DATE = drWH["CLM_UPDATE_DATE"].ToString();
                model.CLM_UPDATE_BY = drWH["CLM_UPDATE_BY"].ToString();
                //model.CLM_UPDATE_DATE = "";
                // model.CLM_UPDATE_BY = "";
                model.Customer = drWH["CUSCOD"].ToString();
                model.REQ_NO = drWH["REQ_NO"].ToString();
                model.CLM_NO_SUB = drWH["CLM_NO_SUB"].ToString();
                model.REQ_DATE = drWH["REQ_DATE"].ToString();
                model.CLM_FRMSUP_STATUS = drWH["CLM_FRMSUP_STATUS"].ToString();
                model.STKCOD = drWH["STKCOD"].ToString();
                model.STKDES = drWH["STKDES"].ToString();
                model.CLM_INVNO = drWH["CLM_INVNO"].ToString();
                model.STATUSTEXT = drWH["STATUSTEXT"].ToString();
                model.CLM_PERFORMTEXT = drWH["CLM_PERFORMTEXT"].ToString();
                model.ANLYS_AFTERPROCESSTEXT = drWH["ANLYS_AFTERPROCESSTEXT"].ToString();
                model.CLM_FRMSUP_DATE = drWH["CLM_FRMSUP_DATE"].ToString();
                model.CLM_FRMSUP_NO = drWH["CLM_FRMSUP_NO"].ToString();
                model.SLMCOD = drWH["SLMCOD"].ToString();
                model.FCLM_QTY = drWH["FCLM_QTY"].ToString();
                model.CLM_COMPANY = drWH["CLM_COMPANY"].ToString();
                model.PERFORMDESCRIPTION = drWH["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = drWH["RCVSTATUSDESCRIPTION"].ToString();
                model.CUSNAM = drWH["CUSNAM"].ToString();
                model.SLMNAM = drWH["SLMNAM"].ToString();
                model.CLM_FOC = drWH["CLM_Foc"].ToString();
                model.CS_No = drWH["CS_No"].ToString();
                model.Log_No = drWH["Log_No"].ToString();
                model.Print_statusCN = drWH["Print_statusCN"].ToString();
                model.VENDOR = drWH["VENDOR"].ToString();
                model.VENDORNAME = drWH["VENDORNAME"].ToString();
                model.UNIT_COST = drWH["UNIT_COST"].ToString();
                model.DEPCOD = drWH["DEPCOD"].ToString();
                model.AMOUNT = drWH["AMOUNT"].ToString();
                model.ClaimType = drWH["ClaimType"].ToString();
                model.TECH2_NAME = drWH["TECH2_NAME"].ToString();
                model.Endprocessdate = drWH["PM_APPRV_DATE"].ToString();
                model.Sec = drWH["Sec"].ToString();
                model.InvoiceSupplier = drWH["InvoiceSupplier"].ToString();
                model.InvoiceDateSupplier = drWH["InvoiceDateSupplier"].ToString();
                model.Lastqtyinv = drWH["Lastqtyinv"].ToString();
                model.Lastcurinv = drWH["Lastcurinv"].ToString();
                model.Lastunitcostinv = drWH["Lastunitcostinv"].ToString();
                model.LastAmountinv = drWH["LastAmountinv"].ToString();
                model.LastRATEinv = drWH["LastRATEinv"].ToString();
                model.Cmstyp = drWH["Claim_Type"].ToString();
                Getdata.Add(new ClimeListDetail { val = model });
            }
            drWH.Close();
            drWH.Dispose();
            commandWH.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult ReopenClaimSupplier(string DataSend, string usesend)
        {
            string message = string.Empty;
            string gendocno = string.Empty;

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<itemsup> _ItemList = new JavaScriptSerializer().Deserialize<List<itemsup>>(DataSend);
            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {

                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_Reopen_Claim_Supplier", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = tran;
                    command.Parameters.AddWithValue("@inCLM_NO_Supplier", _ItemList[i].MyIndexValuekeysupplier);
                    command.Parameters.AddWithValue("@inCLM_NO_SUB", _ItemList[i].MyIndexValuecsubno);
                    command.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].MyIndexValuecstkcod);
                    command.Parameters.AddWithValue("@inREQ_BY", usesend);
                    SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                    returnValue.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValue);
                    command.ExecuteNonQuery();
                    message = returnValue.Value.ToString();
                    command.Dispose();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetReceive_Supplier(string inNO)
        {
            List<ListGetdataReceive_Supplier> Getdata = new List<ListGetdataReceive_Supplier>();
            Receive_SupplierGetdata model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessReceive_Supplier", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inDOC",inNO);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                 model = new Receive_SupplierGetdata();
                 model.CLM_NO_Supplier=dr["CLM_NO_Supplier"].ToString();
                 model.REQ_TotalQty=dr["REQ_TotalQty"].ToString();
                 model.REQ_TotalAMT=dr["REQ_TotalAMT"].ToString();
                 model.REQ_TotalItem=dr["REQ_TotalItem"].ToString();
                 model.REQ_BY=dr["REQ_BY"].ToString();
                 model.REQ_DATE=dr["REQ_DATE"].ToString();
                 model.Remake=dr["Remake"].ToString();
                 model.CLM_SUB_No=dr["CLM_NO_SUB"].ToString();
                 model.STKCOD=dr["STKCOD"].ToString();
                 model.Send_Qty=dr["Send_Qty"].ToString();
                 model.Unit_Price=dr["Unit_Price"].ToString();
                 model.AMT=dr["AMT"].ToString();
                 model.Supplier=dr["Supplier"].ToString();
                 model.STKDES=dr["STKDES"].ToString();
                 model.UOM=dr["UOM"].ToString();
                 model.VENDORNAME=dr["VENDORNAME"].ToString();
                 model.INV_Sup_No=dr["INV_Sup_No"].ToString();
                 model.INV_Qty_Sup=dr["INV_Qty_Sup"].ToString();
                 model.CN_Sup_No=dr["CN_Sup_No"].ToString();
                 model.CN_Qty_Sup=dr["CN_Qty_Sup"].ToString();
                 model.CN_Amt = dr["CN_Amt"].ToString();
                 model.INV_Qty_Sup_Cancel=dr["INV_Qty_Sup_Cancel"].ToString();
                 model.CN_Qty_Sup_Cancel=dr["CN_Qty_Sup_Cancel"].ToString();
                 model.STATUS = dr["STATUS"].ToString();
                 model.REQ_DATE_Clim = dr["REQ_DATE_Clim"].ToString();
				 model.Endprocessdate= dr["STATUS"].ToString();
                 model.ClaimType = dr["ClaimType"].ToString();
                 model.CS_No = dr["CS_No"].ToString();
                 model.Log_No = dr["Log_No"].ToString();
                 model.ANLYS_AFTERPROCESSTEXT = dr["ANLYS_AFTERPROCESSTEXT"].ToString();
                 model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                 model.CUSNAM = dr["CUSNAM"].ToString();
                 model.CUSCOD = dr["CUSCOD"].ToString();
                 model.SLMCOD = dr["SLMCOD"].ToString();
                 model.SLMNAM = dr["SLMNAM"].ToString();
                 model.CLM_UOM = dr["CLM_UOM"].ToString();
                 model.Endprocessdate = dr["Endprocessdate"].ToString();
                 model.STATUS = dr["STATUS"].ToString();
                 model.InvoiceSupplier = dr["InvoiceSupplier"].ToString();
                 model.InvoiceDateSupplier = dr["InvoiceDateSupplier"].ToString();
                 model.STATUSName = dr["StatusName"].ToString();
                 model.Lastqtyinv = dr["Lastqtyinv"].ToString();
                 model.Lastcurinv = dr["Lastcurinv"].ToString();
                 model.Lastunitcostinv = dr["Lastunitcostinv"].ToString();
                 model.LastAmountinv = dr["LastAmountinv"].ToString();
                 model.LastRATEinv = dr["LastRATEinv"].ToString();
                 model.Cur_Sup = dr["Cur_Sup"].ToString();
                 model.ClaimType = dr["Claim_Type"].ToString();
                 Getdata.Add(new ListGetdataReceive_Supplier { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddClaimDisposal(string ingenno, string DataSend, string usesend)
        {
            string message = string.Empty;
            string gendocno = string.Empty;
            List<ItemSupplier> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemSupplier>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {

                gendocno = ingenno;


                for (int i = 0; i < _ItemList.Count; i++)
                        {
                            var commandLine = new SqlCommand("P_Add_Claim_Supplier", Connection);
                            commandLine.CommandType = CommandType.StoredProcedure;
                            commandLine.Transaction = tran;
                            commandLine.Parameters.AddWithValue("@inCOMP", _ItemList[i].company);
                            commandLine.Parameters.AddWithValue("@inCLM_NO_Supplier", gendocno);
                            commandLine.Parameters.AddWithValue("@inCLM_SUB_No", _ItemList[i].Clmsupno);
                            commandLine.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].VSTKCOD);
                            commandLine.Parameters.AddWithValue("@inSend_Qty", _ItemList[i].VQty);
                            commandLine.Parameters.AddWithValue("@inUnit_Price", _ItemList[i].VUNITPRICE);
                            commandLine.Parameters.AddWithValue("@inAMT", _ItemList[i].VAMT);
                            commandLine.Parameters.AddWithValue("@inINV_Sup_No", "");
                            commandLine.Parameters.AddWithValue("@inINV_Qty_Sup", "0");
                            commandLine.Parameters.AddWithValue("@inCN_Sup_No", "");
                            commandLine.Parameters.AddWithValue("@inCN_Qty_Sup", "0");
                            commandLine.Parameters.AddWithValue("@inCN_Amt_Sup", "0");
                            commandLine.Parameters.AddWithValue("@inINV_Qty_Sup_Cancel", "0");
                            commandLine.Parameters.AddWithValue("@inCN_Qty_Sup_Cancel", "0");
                            commandLine.Parameters.AddWithValue("@inREQ_BY", usesend);
                            //commandLine.Parameters.AddWithValue("@inRemake", commentsend);
                            commandLine.Parameters.AddWithValue("@inSupplier", _ItemList[i].Vsupplier);
                            commandLine.Parameters.AddWithValue("@inInvoiceSupplier", _ItemList[i].VInvoiceSupplier);
                            commandLine.Parameters.AddWithValue("@inInvoiceDateSupplier", _ItemList[i].VInvoiceDateSupplier);
                          
                            SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);

                            returnValue.Direction = System.Data.ParameterDirection.Output;
                            commandLine.Parameters.Add(returnValue);
                            commandLine.ExecuteNonQuery();
                            
                            message = returnValue.Value.ToString();
                        }

                


                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteClaimSupplier(string DataSend, string usesend)
        {
            string message = string.Empty;
            List<itemedit> _ItemList = new JavaScriptSerializer().Deserialize<List<itemedit>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();
            try
            {

                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_Del_Supplier", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = tran;
                    command.Parameters.AddWithValue("@inCLM_NO_Supplier", _ItemList[i].clmnosupplier);
                    command.Parameters.AddWithValue("@inCLM_NO_SUB", _ItemList[i].clmnosub);
                    command.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].stkcod);
                    command.Parameters.AddWithValue("@infix", _ItemList[i].fix);
                    command.Parameters.AddWithValue("@inInsertBy",_ItemList[i].InsertBy);
                    SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                    returnValue.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValue);
                    command.ExecuteNonQuery();
                    message = returnValue.Value.ToString();
                    command.Dispose();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }
            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SaveClaimSupplier(string claimtype ,string commentsend, string com, string Doc, string CLM_NO_Supplier, string DataSend, string ressupplierdatesend, string usesend, string countstksend, string countnumbersend, string countamt)
        {
            string message = string.Empty;
            string gendocno = string.Empty;
            List<ItemSupplier> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemSupplier>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
          
            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();
           
                try
                {

                    var command = new SqlCommand("P_Save_Claim_Supplier_H", Connection);
                    command.Transaction = tran;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inCOMP ", com);
                    command.Parameters.AddWithValue("@inCLM_NO_Supplier", CLM_NO_Supplier);
                    command.Parameters.AddWithValue("@inREQ_TotalQty", countstksend);
                    command.Parameters.AddWithValue("@inREQ_TotalAMT", countamt);
                    command.Parameters.AddWithValue("@inREQ_TotalItem", countnumbersend);
                    command.Parameters.AddWithValue("@inSupplier", "");
                    command.Parameters.AddWithValue("@inREQ_BY", usesend);
                    command.Parameters.AddWithValue("@inRemake", commentsend);
                    command.Parameters.AddWithValue("@inDocNam ", Doc);
                    SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                    returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValuedoc);
                    command.ExecuteNonQuery();
                  
                    gendocno = returnValuedoc.Value.ToString();
                    if (gendocno != "")
                    {
                        
                        for (int i = 0; i < _ItemList.Count; i++)
                        {
                            var commandLine = new SqlCommand("P_Save_Claim_Supplier_L", Connection);
                            commandLine.CommandType = CommandType.StoredProcedure;
                            commandLine.Transaction = tran;
                            commandLine.Parameters.AddWithValue("@inCOMP ", com);
                            commandLine.Parameters.AddWithValue("@inCLM_NO_Supplier", gendocno);
                            commandLine.Parameters.AddWithValue("@inCLM_SUB_No", _ItemList[i].Clmsupno);
                            commandLine.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].VSTKCOD);
                            commandLine.Parameters.AddWithValue("@inSend_Qty", _ItemList[i].VQty);
                            commandLine.Parameters.AddWithValue("@inUnit_Price", _ItemList[i].VUNITPRICE);
                            commandLine.Parameters.AddWithValue("@inAMT ", _ItemList[i].VAMT);
                            commandLine.Parameters.AddWithValue("@inINV_Sup_No", "");
                            commandLine.Parameters.AddWithValue("@inINV_Qty_Sup", "0");
                            commandLine.Parameters.AddWithValue("@inCN_Sup_No", "");
                            commandLine.Parameters.AddWithValue("@inCN_Qty_Sup", "0");
                            commandLine.Parameters.AddWithValue("@inCN_Amt_Sup", "0");
                            commandLine.Parameters.AddWithValue("@inCN_Cur_Sup", "");
                            commandLine.Parameters.AddWithValue("@inINV_Qty_Sup_Cancel", "0");
                            commandLine.Parameters.AddWithValue("@inCN_Qty_Sup_Cancel", "0");
                            commandLine.Parameters.AddWithValue("@inREQ_BY", usesend);
                            commandLine.Parameters.AddWithValue("@inDateReceive", ressupplierdatesend);
                            commandLine.Parameters.AddWithValue("@inSupplier", _ItemList[i].Vsupplier);
                            commandLine.Parameters.AddWithValue("@inInvoiceSupplier", _ItemList[i].VInvoiceSupplier);
                            commandLine.Parameters.AddWithValue("@inInvoiceDateSupplier", _ItemList[i].VInvoiceDateSupplier);
                            commandLine.Parameters.AddWithValue("@inClaimtype", claimtype);
                            SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                            returnValue.Direction = System.Data.ParameterDirection.Output;
                            commandLine.Parameters.Add(returnValue);
                            commandLine.ExecuteNonQuery();
                            
                            message = returnValue.Value.ToString();
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                  
                    //throw;
                    message = ex.Message;
                }


                Connection.Close();

                return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);
          
        }
        public JsonResult UpdateClaimSupplier(string commentsend, string com, string Doc, string CLM_NO_Supplier, string DataSend, string ressupplierdatesend, string usesend, string countstksend, string countnumbersend, string countamt)
        {
            string message = string.Empty;
            string gendocno = string.Empty;
            List<ItemSupplier> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemSupplier>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {
                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_update_processsend_supplier", Connection);
                    command.Transaction = tran;
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@inUser", usesend);
                    command.Parameters.AddWithValue("@inCLM_NO_Supplier", CLM_NO_Supplier);
                    command.Parameters.AddWithValue("@inCLM_SUB_No", _ItemList[i].VSTCLMNO);
                    command.Parameters.AddWithValue("@inINV_Sup_Cut", _ItemList[i].VInvoiceSupplier);
                    command.Parameters.AddWithValue("@inINV_Date_Sup", _ItemList[i].VInvoiceDateSupplier);

                    SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                    returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValuedoc);
                    command.ExecuteNonQuery();

                    gendocno = returnValuedoc.Value.ToString();
                    message = "Y";
                }
               
                
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SaveClaimSupplierreceive(string com,string supplierdatered, string supplierusersendre, string suppliercommentred, string keysupplier, string DataSend)
        {
            string message = string.Empty;
            string gendocno = string.Empty;
            List<Suppliereceive> _ItemList = new JavaScriptSerializer().Deserialize<List<Suppliereceive>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {

                var command = new SqlCommand("P_Save_Claim_Supplier_H", Connection);
                command.Transaction = tran;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCOMP ", com);
                command.Parameters.AddWithValue("@inCLM_NO_Supplier", keysupplier);
                command.Parameters.AddWithValue("@inREQ_TotalQty", "0");
                command.Parameters.AddWithValue("@inREQ_TotalAMT", "0");
                command.Parameters.AddWithValue("@inREQ_TotalItem", "0");
                command.Parameters.AddWithValue("@inSupplier", "");
                command.Parameters.AddWithValue("@inREQ_BY", supplierusersendre);
                command.Parameters.AddWithValue("@inRemake", suppliercommentred);
                command.Parameters.AddWithValue("@inDocNam ", "");
                SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();

                gendocno = returnValuedoc.Value.ToString();
                if (gendocno != "")
                {

                    for (int i = 0; i < _ItemList.Count; i++)
                    {
                        var commandLine = new SqlCommand("P_Save_Claim_Supplier_L", Connection);
                        commandLine.CommandType = CommandType.StoredProcedure;
                        commandLine.Transaction = tran;
                        commandLine.Parameters.AddWithValue("@inCOMP ", com);
                        commandLine.Parameters.AddWithValue("@inCLM_NO_Supplier", keysupplier);
                        commandLine.Parameters.AddWithValue("@inCLM_SUB_No", _ItemList[i].MyIndexValuecsubno);
                        commandLine.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].MyIndexValuecstkcod);
                        commandLine.Parameters.AddWithValue("@inSend_Qty", "0");
                        commandLine.Parameters.AddWithValue("@inUnit_Price","0");
                        commandLine.Parameters.AddWithValue("@inAMT ","0");
                        commandLine.Parameters.AddWithValue("@inINV_Sup_No", _ItemList[i].MyIndexValueinvno);
                        commandLine.Parameters.AddWithValue("@inINV_Qty_Sup", _ItemList[i].MyIndexValueQtyinv);
                        commandLine.Parameters.AddWithValue("@inCN_Sup_No", _ItemList[i].MyIndexValuecnno);
                        commandLine.Parameters.AddWithValue("@inCN_Qty_Sup", _ItemList[i].MyIndexValueQtycn);
                        commandLine.Parameters.AddWithValue("@inCN_Amt_Sup", _ItemList[i].MyIndexValueAmtcn);
                        commandLine.Parameters.AddWithValue("@inCN_Cur_Sup", _ItemList[i].MyIndexValuesCurcn);
                        commandLine.Parameters.AddWithValue("@inINV_Qty_Sup_Cancel", _ItemList[i].MyIndexValueQtyinvcance);
                        commandLine.Parameters.AddWithValue("@inCN_Qty_Sup_Cancel", _ItemList[i].MyIndexValueQtycncance);
                        commandLine.Parameters.AddWithValue("@inREQ_BY", supplierusersendre);
                        commandLine.Parameters.AddWithValue("@inDateReceive", supplierdatered);
                        commandLine.Parameters.AddWithValue("@inSupplier","");
                        commandLine.Parameters.AddWithValue("@inInvoiceSupplier", _ItemList[i].MyIndexValueInvoiceSupplier);
                        commandLine.Parameters.AddWithValue("@inInvoiceDateSupplier", _ItemList[i].MyIndexValueInvoiceDateSupplier);
                        commandLine.Parameters.AddWithValue("@inClaimtype", "");
                        SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                        returnValue.Direction = System.Data.ParameterDirection.Output;
                        commandLine.Parameters.Add(returnValue);
                        commandLine.ExecuteNonQuery();

                        message = returnValue.Value.ToString();
                    }
                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateClaimSupplierreceive(string com,string  im_name ,string  DataSend, string supplierusersendre,string keysupplier)
        {
            string message = string.Empty;
            List<ItemSupplier> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemSupplier>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_update_Afterprocess_supplier", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inUser", im_name);
                    command.Parameters.AddWithValue("@inCOMP", com);
                    command.Parameters.AddWithValue("@inCLM_NO_Supplier", _ItemList[i].keysupplier);
                    command.Parameters.AddWithValue("@inCLM_SUB_No", _ItemList[i].VSTCLMNO);
                    command.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].VSTKCOD);
                    command.ExecuteNonQuery();

                    command.Dispose();
                    message = "true";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UploadInvoicePdf(HttpPostedFileBase filex, string UserID)
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
                    file.SaveAs(Server.MapPath(@"~\PdfUpload\Invoice\" + fileName + ".pdf")); //File will be saved in application root

                    imgname = fileName + ".pdf";
                    message = "true";
                 


                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message, imgname, path }, JsonRequestBehavior.AllowGet);
            //return Json("Uploaded " + Request.Files.Count + " files");
        }
        public JsonResult UploadCnPdf(HttpPostedFileBase filex, string UserID)
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
                    file.SaveAs(Server.MapPath(@"~\PdfUpload\CN\" + fileName + ".pdf")); //File will be saved in application root

                    imgname = fileName + ".pdf";
                    message = "true";



                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message, imgname, path }, JsonRequestBehavior.AllowGet);
            //return Json("Uploaded " + Request.Files.Count + " files");
        }
        public JsonResult SavePathPdf(string im_name, string Cim_No, string Im_No, string inCim_NoSub, string type)
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
        public class ItemListGetdata
        {
            public ItemSupplier val { get; set; }

        }
        public class ItemSupplier
        {
            public string CartID { get; set; }
            public string VSTCLMNO { get; set; }
            public string VSTKCOD { get; set; }
            public string VAMT { get; set; }
            public string VUNITPRICE { get; set; }
            public string Vendor { get; set; }
            public string Clmsupno { get; set; }
            public string VQty { get; set; }
            public string Vsupplier { get; set; }
            public string keysupplier { get; set; }
            public string company { get; set; }
            public string VInvoiceSupplier{ get; set; }
            public string VInvoiceDateSupplier { get; set; }
        }

        public class Suppliereceive
        {
             public string MyIndexValueinvno { get; set; }
             public string MyIndexValueQtyinv { get; set; }
             public string MyIndexValuecnno { get; set; }
             public string MyIndexValueQtycn { get; set; }
             public string MyIndexValueAmtcn { get; set; }
             public string MyIndexValueQtyinvcance { get; set; }
             public string MyIndexValueQtycncance { get; set; }
             public string MyIndexValuecstkcod { get; set; }
             public string MyIndexValuecsubno { get; set; }
             public string MyIndexValueInvoiceSupplier { get; set; }
             public string MyIndexValueInvoiceDateSupplier { get; set; }
             public string MyIndexValuesCurcn { get; set; }
             public string MyIndexValuestype { get; set; }
        }
        public class itemsup
        {
            public string MyIndexValuecstkcod{ get; set; }
            public string MyIndexValuecsubno{ get; set; }
            public string  MyIndexValueusersendre{ get; set; }
            public string MyIndexValuekeysupplier { get; set; }
        }

        public class itemedit
        {
          public string clmnosupplier{ get; set; }
          public string clmnosub{ get; set; }
          public string stkcod{ get; set; }
          public string InsertBy{ get; set; }
          public string fix { get; set; }
        }
    }
}
