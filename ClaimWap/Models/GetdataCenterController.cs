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
namespace ClaimWap.Models
{
    public class GetdataCenterController : Controller
    {
        //
        // GET: /Default1/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Getdateslm(string UserId)
        {
            string usre = Session["UserID"].ToString();
            string Password = Session["UserPassword"].ToString();
            string spuser = Session["UsrGrpspecial"].ToString();
            List<SLM> SlmList = new List<SLM>();


            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            if (spuser != "1")
            {
                var command = new SqlCommand("P_Chk_user", Connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UsrID", UserId);


                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    SlmList.Add(new SLM()
                    {
                        SLMCOD = dr.GetValue(1).ToString(),
                        SLMNAM = dr.GetValue(2).ToString()
                    });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
            }
            else
            {
                var command = new SqlCommand("P_Chk_user_special", Connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UsrID", UserId);


                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    SlmList.Add(new SLM()
                    {
                        SLMCOD = dr.GetValue(1).ToString(),
                        SLMNAM = dr.GetValue(2).ToString()
                    });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();

            }

            Connection.Dispose();
            Connection.Close();

            return Json(SlmList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateslmapproveSalesreturn(string UserId)
        {
            string usre = Session["UserID"].ToString();
            string Password = Session["UserPassword"].ToString();
            string spuser = Session["UsrGrpspecial"].ToString();
            string UserType = Session["UserType"].ToString();
            List<SLM> SlmList = new List<SLM>();


            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            if (UserType == "5")
            {
                var command = new SqlCommand("P_Chk_user", Connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UsrID", UserId);


                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    SlmList.Add(new SLM()
                    {
                        SLMCOD = dr.GetValue(1).ToString(),
                        SLMNAM = dr.GetValue(2).ToString()
                    });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
            }
            else if (UserType == "4")
            {
                var command = new SqlCommand("P_Chk_user_Appbysupsales_special", Connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UsrID", UserId);


                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    SlmList.Add(new SLM()
                    {
                        SLMCOD = dr.GetValue(1).ToString(),
                        SLMNAM = dr.GetValue(2).ToString()
                    });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();

            }

            Connection.Dispose();
            Connection.Close();

            return Json(SlmList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateSaleslevel(string UserId)
        {
            string usre = Session["UserID"].ToString();
            string Password = Session["UserPassword"].ToString();
            string spuser = Session["UsrGrpspecial"].ToString();
            string Slmsrt = string.Empty;
            List<SLM> SlmList = new List<SLM>();
            string query = string.Empty;

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Saleslevel", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@User", UserId);
            //SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
            //returnValuedoc.Direction = System.Data.ParameterDirection.Output;
            //command.Parameters.Add(returnValuedoc);

            //command.ExecuteNonQuery();
            //Slmsrt = returnValuedoc.Value.ToString();
            //List<string> stringList = Slmsrt.Split(';').ToList();


            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                SlmList.Add(new SLM()
                {
                    SLMCOD = dr.GetValue(1).ToString(),
                    SLMNAM = dr.GetValue(2).ToString()
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();

            Connection.Dispose();
            Connection.Close();

            return Json(SlmList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateCusCode(string Name, string Slm, string Company)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string SLMCOD = string.Empty;
            string query = string.Empty;
            //string Company = string.Empty;
            string tableCus = string.Empty;
            tableCus = "v_CUSPROV";
            if (Company != "")
            {
                if (Company == "TAM" || Company == "VELOX")
                {
                    tableCus = "Customer_TAM";
                }
            }
            if (Slm == "ALL")
            {

                SLMCOD = "";
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM  from " + tableCus + " pc    where  pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%'", Name);
            }
            else
            {
                SLMCOD = Slm;
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM from " + tableCus + " pc    where  pc.SLMCOD ='" + SLMCOD + "' and (pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%')", Name);

            }
            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Code.Add(reader.GetString(1));
                }
                // reader.Dispose();
                //S20161016
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                //E20161016

            }
            Connection.Close();
            return Json(Code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getdatevendor(string Name, string com)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string Com = string.Empty;
            string query = string.Empty;
            if (com == "TAC")
            {

                //SLMCOD = "";
                query = string.Format("select distinct pc.[No_],pc.[No_] + ' | ' + pc.[Name] from NNVVendor_TAC pc    where  pc.[No_] LIKE '%{0}%'or pc.[Name]  LIKE '%{0}%'", Name);
            }
            else
            {
                //SLMCOD = Slm;
                query = string.Format("select distinct pc.[No_],pc.[No_] + ' | ' + pc.[Name] from NNVVendor_AAC pc   where  pc.[No_] LIKE '%{0}%'or pc.[Name]  LIKE '%{0}%'", Name);
            }
            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Code.Add(reader.GetString(1));
                }
                // reader.Dispose();
                //S20161016
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                //E20161016

            }
            Connection.Close();
            return Json(Code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateCusAdd(string Name, string Slm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string SLMCOD = string.Empty;
            string query = string.Empty;

            if (Slm == "ALL")
            {
                SLMCOD = "";
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM  + ' | ' + pc.ADDR_01 +' '+pc.ADDR_02 from v_CUSPROV pc  where  pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%' UNION ALL select distinct ct.CUSCOD,ct.CUSCOD + ' | ' + ct.CUSNAM  + ' | ' + ct.ADDR_01 +' '+ct.ADDR_02 from Customer_TAM ct where ct.CUSCOD LIKE '%{0}%' or ct.CUSNAM  LIKE '%{0}%'", Name);
            }
            else
            {
                SLMCOD = Slm;
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM  + ' | ' + pc.ADDR_01 +' '+pc.ADDR_02 from v_CUSPROV pc    where  pc.SLMCOD ='" + SLMCOD + "' and (pc.CUSCOD LIKE '%{0}%' or pc.CUSNAM  LIKE '%{0}%') UNION ALL select distinct ct.CUSCOD,ct.CUSCOD + ' | ' + ct.CUSNAM  + ' | ' + ct.ADDR_01 +' '+ct.ADDR_02 from Customer_TAM ct where ct.SLMCOD = '" + SLMCOD + "' and (ct.CUSCOD LIKE '%{0}%' or ct.CUSNAM  LIKE '%{0}%')", Name);

            }
            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Code.Add(reader.GetString(1));
                }
                // reader.Dispose();
                //S20161016
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                //E20161016

            }
            Connection.Close();
            return Json(Code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateByCusCode(string CusCode, string Slm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string SLMCOD = string.Empty;
            string CusSlmcode = string.Empty;
            string Cusname = string.Empty;
            string query = string.Empty;

            if (Slm == "ALL")
            {

                SLMCOD = "";
                query = string.Format("select  pc.CUSCOD, pc.CUSNAM,pc.SLMCOD  from v_CUSPROV pc    where  pc.CUSCOD ='" + CusCode + "'");
            }
            else
            {
                SLMCOD = Slm;
                query = string.Format("select  pc.CUSCOD, pc.CUSNAM,pc.SLMCOD  from v_CUSPROV pc  where  pc.CUSCOD ='" + CusCode + "   and pc.SLMCOD ='" + SLMCOD + "'");

            }
            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Cusname = reader["CUSNAM"].ToString();
                    CusSlmcode = reader["SLMCOD"].ToString();
                }
                // reader.Dispose();
                //S20161016
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                //E20161016

            }
            Connection.Close();
            return Json(new { Cusname, CusSlmcode }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getdateclmno(string Name, string Slm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string SLMCOD = string.Empty;
            string query = string.Empty;
            if (Slm == "ALL")
            {

                SLMCOD = "";
                query = string.Format("select distinct pc.CLM_ID,pc.CLM_ID + ' | ' + pc.Customer  from v_ClmNo pc    where pc.CLM_ID LIKE '%{0}%'or pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%'", Name);
            }
            else
            {
                SLMCOD = Slm;
                query = string.Format("select distinct pc.CLM_ID,pc.CLM_ID + ' | ' + pc.Customer  from v_ClmNo pc    where  pc.SLMCOD ='" + SLMCOD + "' and ( pc.CLM_ID LIKE '%{0}%'or pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%')", Name);

            }
            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Code.Add(reader.GetString(1));
                }
                // reader.Dispose();
                //S20161016
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                //E20161016

            }
            Connection.Close();
            return Json(Code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataUsetype(string User)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string Usetpye = string.Empty;
            string Department = string.Empty;
            SqlCommand cmd = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User + "'", Connection);
            SqlDataReader rev = cmd.ExecuteReader();
            while (rev.Read())
            {


                Usetpye = rev["UsrTyp"].ToString();

            }
            rev.Close();
            rev.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(new { Usetpye }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserTech2()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            List<Grptech2> List = new List<Grptech2>();
            SqlCommand cmd = new SqlCommand("select * From v_TECH2 order by TECH2_NAME asc", Connection);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new Grptech2()
                {
                    UserId = dr["TECH2_NAME"].ToString(),
                    UserName = dr["TECH2_NAME"].ToString()
                });

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            List<Section> List = new List<Section>();
            SqlCommand cmd = new SqlCommand("select * From v_MST_SECTION order by SEC ", Connection);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new Section()
                {
                    Id = dr["SEC"].ToString(),
                    Name = dr["SECNAM"].ToString()
                });

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataDepartment(string User)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string Usetpye = string.Empty;
            string Department = string.Empty;
            SqlCommand cmd = new SqlCommand("select * From UsrGrp  where UsrID =N'" + User + "'", Connection);
            SqlDataReader rev = cmd.ExecuteReader();
            while (rev.Read())
            {


                Usetpye = rev["UsrTyp"].ToString();
                Department = rev["Department"].ToString();
            }
            rev.Close();
            rev.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(new { Department, Usetpye }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataSlmbycus(string Cus, string Company)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string Slm = string.Empty;
            string Slmcodname = string.Empty;
            string Telcus = string.Empty;
            string tableCus = string.Empty;
            tableCus = "v_CUSPROV";
            if (Company != "")
            {
                if (Company == "TAM")
                {
                    tableCus = "customer_tam";
                }
            }

            SqlCommand cmd = new SqlCommand("select Pm.SLMCOD ,Pm.SLMNAM from " + tableCus + " Pc INNER JOIN v_SLMTAB_SM Pm ON Pc.SLMCOD = Pm.SLMCOD where Pc.CUSCOD =N'" + Cus + "'", Connection);
            SqlDataReader rev_CUSPROV = cmd.ExecuteReader();
            while (rev_CUSPROV.Read())
            {
                Slm = rev_CUSPROV["SLMCOD"].ToString();
                Slmcodname = rev_CUSPROV["SLMNAM"].ToString();
            }

            rev_CUSPROV.Close();
            rev_CUSPROV.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(new { Slm, Slmcodname }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getdatastkgrp(string prodId)
        {
            string chkCompany = string.Empty;
            //string prodIdTemp = string.Empty;
            chkCompany = Session["company"].ToString();
            List<Stkgrp> List = new List<Stkgrp>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            if (prodId == "" || prodId == null)
            {
                prodId = "";
            }
            var command = new SqlCommand("P_Get_Stkgrp", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@intCompany", chkCompany);
            command.Parameters.AddWithValue("@intProd", prodId);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();


            while (dr.Read())
            {
                List.Add(new Stkgrp()
                {
                    STKGRP = dr["STKGRP"].ToString(),
                    GRPNAM = dr["GRPNAM"].ToString()
                });

            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataUsesp()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<UseCustomer> List = new List<UseCustomer>();


            SqlCommand cmd = new SqlCommand("select ID,UsrID from  UsrGrp_special order by UsrID", Connection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new UseCustomer()
                {
                    ID = dr["ID"].ToString(),
                    USRID = dr["UsrID"].ToString()

                });

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataNVAttribute()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<Attribute> List = new List<Attribute>();


            SqlCommand cmd = new SqlCommand("select [Attribute Code],[Description] from  v_NVAttribute", Connection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new Attribute()
                {
                    AttributeCode = dr["Attribute Code"].ToString(),
                    Description = dr["Description"].ToString()
                });

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateStatusClm(string Name)
        {
            List<DefineCode> List = new List<DefineCode>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_DefineCode", Connection);
            command.CommandType = CommandType.StoredProcedure;
            //ดึง ช่างที่รับผิดชอบ/TECH1 Name
            if (Name == "12" && Session["company"].ToString() == "TAM")
            {
                Name = "41"; //ดึงเฉพาะ Tec TAM
            }
            command.Parameters.AddWithValue("@DefineID", Convert.ToInt32(Name));
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new DefineCode()
                {
                    ID = dr["ID"].ToString(),
                    CODE = dr["CODE"].ToString(),
                    FIELD_RELATION = dr["FIELD_RELATION"].ToString(),
                    DESCRIPTION_TH = dr["DESCRIPTION_TH"].ToString(),
                    DESCRIPTION_EN = dr["DESCRIPTION_EN"].ToString(),
                    INACTIVE = dr["INACTIVE"].ToString()
                });
                //ถ้าไม่ใช่ TAM ลบ Status ที่เกี่ยว Saleman, Yap/GM
                if (Name == "6" && Session["company"].ToString() != "TAM")
                {
                    var nameToRemove = new[] { "9", "10", "11", "12", "13", "14" };
                    List.RemoveAll(t => nameToRemove.Contains(Name));
                }
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductdata(string UserId)
        {
            List<Pm> List = new List<Pm>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Product_Data", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUsrID", UserId);
            command.Parameters.AddWithValue("@inType", 4);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new Pm()
                {
                    PROD = dr["PROD"].ToString(),
                    PRODNAM = dr["PRODNAM"].ToString()

                });

            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPmName(string UserId, String Comp)
        {
            List<Pm> List = new List<Pm>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Product_Data", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUsrID", UserId);
            command.Parameters.AddWithValue("@inType", 4);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new Pm()
                {
                    PROD = dr["PROD"].ToString(),
                    PRODNAM = dr["PRODNAM"].ToString()

                });

            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetdateStockCode(string Name, string XvalCompany)
        {
            string CUSCOD = string.Empty;
            List<string> StockCode = new List<string>();
            //if (Session["CUSID"] == null)
            //{

            //    return Json(new
            //    {
            //        redirectUrl = Url.Action("LogIn", "Account"),
            //        isRedirect = true
            //    });

            //}
            //else
            //{
            //CUSCOD = Session["CUSID"].ToString();

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Item", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inSearch", Name);
            command.Parameters.AddWithValue("@inSLMCOD", "");
            command.Parameters.AddWithValue("@inCUSCOD", "");
            command.Parameters.AddWithValue("@inProd", "");
            command.Parameters.AddWithValue("@inSTKGRP", "");
            command.Parameters.AddWithValue("@inFix", 1);
            command.Parameters.AddWithValue("@Company", XvalCompany);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                StockCode.Add(dr.GetString(1));

            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            //}
            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateStockdetail(string Name, string XvalCompany)
        {
            string CUSCOD = string.Empty;
            List<Item_DetailGetdata> Getdata = new List<Item_DetailGetdata>();
            Item_Detail model = null;

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Item_Detail", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inSearch", Name);
            command.Parameters.AddWithValue("@Company", XvalCompany);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Item_Detail();
                model.Company = dr["Company"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.UOM = dr["UOM"].ToString();
                model.SEC = dr["SEC"].ToString();
                model.DEPNAM = dr["DEPNAM"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.GRPNAM = dr["GRPNAM"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                Getdata.Add(new Item_DetailGetdata { val = model });

            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            //}
            return Json(Getdata, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateitemCrossRef(string Name, string XvalCompany, string XvalCus, string fix)
        {
            string CUSCOD = string.Empty;
            List<string> StockCode = new List<string>();


            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_itemCrossRef", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inSearch", Name);
            command.Parameters.AddWithValue("@inSLMCOD", "");
            command.Parameters.AddWithValue("@inCUSCOD", XvalCus);
            command.Parameters.AddWithValue("@inProd", "");
            command.Parameters.AddWithValue("@inSTKGRP", "");
            command.Parameters.AddWithValue("@inFix", 1);
            command.Parameters.AddWithValue("@Company", XvalCompany);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                StockCode.Add(dr.GetString(1));

            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            //}
            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathImage(string inCLM_ID, string CLM_NO)
        {
            string flagClaimBat = string.Empty;
            string getClaimBat = string.Empty;
            if (!string.IsNullOrEmpty(inCLM_ID)) { 
                getClaimBat = inCLM_ID.Substring(0, 1);
                if (!string.IsNullOrEmpty(getClaimBat)) {
                    if (getClaimBat.ToUpper() == "E") {
                        flagClaimBat = "Y";
                    }
                }
            }
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<ImageFilesListDetail> Getdata = new List<ImageFilesListDetail>();
            ImageFiles model = null;
            //var root = @"..\..\e-Warranty_test\UploadedImage\";
            //path claim std
            var root = @"..\ImgUpload\";
            //var root = @"C:\Claim\ImgUpload\";
            var command = new SqlCommand("P_GetPathImage", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inCim_No", inCLM_ID);
            command.Parameters.AddWithValue("@inCim_NoSub", CLM_NO);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ImageFiles();
                model.IMAGE_ID = dr["IMAGE_ID"].ToString();
                model.REQ_NO = dr["REQ_NO"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.IMAGE_NO = dr["IMAGE_NO"].ToString();
                model.IMAGE_NAME = dr["IMAGE_NAME"].ToString();
                //check path std, ewarranty
                if (string.IsNullOrEmpty(flagClaimBat)) {
                    model.PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString());
                    //model.PATH = "C:\Claim\ImgUpload\" + dr["IMAGE_NAME"].ToString();
                    //model.PATH = Path.Combine("C:\\Claim\\ImgUpload\\", dr["IMAGE_NAME"].ToString());
                    //model.PATH = @"C:\Claim\ImgUpload\" + dr["IMAGE_NAME"].ToString();
                    //model.PATH = "https://car-images.bauersecure.com/pagefiles/90541/1040x0/best_supercar_052.jpg?scale=down";

                }
                else { 
                    model.PATH = dr["IMAGE_NAME"].ToString();
                }
                //model.PATH = "D:\\Projects\\work spaces\\ClaimWap\\ClaimWap\\ImgUpload\\CM18110012-GDB7224YO-CM18110012-01-01.png";
                //ex. combile path server
                //string serverPath = @"D:\eWarranty_Test\";
                //string filePath = "\\UploadedImage\\155-0.png";
                //Uri serverUri = new Uri(serverPath + filePath);
                //string finalPath = serverUri.LocalPath;
                //model.PATH = finalPath;
                //model.PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString());
                Getdata.Add(new ImageFilesListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetfileVideo(string inCLM_ID, string CLM_NO, string Im_No)
        {
            List<VideoFiles> videolist = new List<VideoFiles>();
            string Docnocm = string.Empty;
            var root = @"..\VideoFileUpload\";
            var CS = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(CS);
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand command = new SqlCommand("spGetAllVideoFile", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@inCim_No", inCLM_ID);
                command.Parameters.AddWithValue("@inCim_NoSub", CLM_NO);
                command.Parameters.AddWithValue("@inImg_ID", Im_No);
                Connection.Open();
                con.Open();
                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    VideoFiles video = new VideoFiles();
                    video.ID = Convert.ToInt32(rdr["ID"]);
                    video.Name = rdr["Name"].ToString();
                    video.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    // video.FilePath = rdr["FilePath"].ToString();
                    video.FilePath = Path.Combine(root, rdr["Name"].ToString());
                    videolist.Add(video);
                }
                rdr.Close();
                rdr.Dispose();
                command.Dispose();
            }

            Connection.Close();
            return Json(new { videolist }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathPdf_sup(string clm_no_supplier, string clm_sub_no, string clm_type)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<ImageFilesListDetail> Getdata = new List<ImageFilesListDetail>();
            ImageFiles model = null;
            var root = "";
            if (clm_type == "In")
            {
                root = @"..\PdfUpload\Invoice";
            }
            else
            {
                root = @"..\PdfUpload\CN";
            }
            var command = new SqlCommand("P_GetPathPdf", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inclm_no_supplier", clm_no_supplier);
            command.Parameters.AddWithValue("@inCim_NoSub", clm_sub_no);
            command.Parameters.AddWithValue("@intype", clm_type);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ImageFiles();
                model.IMAGE_ID = dr["PDF_ID"].ToString();
                model.REQ_NO = dr["REQ_NO"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.IMAGE_NO = dr["PDF_NO"].ToString();
                model.IMAGE_NAME = dr["PDF_NAME"].ToString();
                //model.PATH = dr["PATH"].ToString();
                // model.PATH = Server.MapPath(@"~\ImgUpload\" + dr["IMAGE_NAME"].ToString());
                model.PATH = Path.Combine(root, dr["PDF_NAME"].ToString());
                //model.PATH = "D:\\Projects\\work spaces\\ClaimWap\\ClaimWap\\ImgUpload\\CM18110012-GDB7224YO-CM18110012-01-01.png";
                Getdata.Add(new ImageFilesListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataInvoicestatud(string _flength, string _foc, string _cusno, string _invoiceno, string _itemno, string _com, string _psdate, string _statdate, string _enddate, string _slmcod)
        {
            string message = string.Empty;
            string PSTDAT = string.Empty;
            string formattedLInvdate = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<InvoiceStatusListDetailGetdata> Getdata = new List<InvoiceStatusListDetailGetdata>();
            InvoiceStatus model = null;
            try
            {

                var command = new SqlCommand("P_InvoiceCheck_ByCusItm_Date", Connection);
                command.CommandTimeout = 300;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUS", _cusno);
                command.Parameters.AddWithValue("@PN", _itemno);
                command.Parameters.AddWithValue("@DOC", _invoiceno);
                command.Parameters.AddWithValue("@GRP", "");
                command.Parameters.AddWithValue("@PSTDATEStat", _statdate);
                command.Parameters.AddWithValue("@PSTDATEEnd", _enddate);
                command.Parameters.AddWithValue("@Com", _com);
                command.Parameters.AddWithValue("@SLM", _slmcod);
                command.Parameters.AddWithValue("@FOC", _foc);
                command.Parameters.AddWithValue("@flength", _flength);
                //command.Parameters.AddWithValue("@PN", "JTC215");
                //command.Parameters.AddWithValue("@DOC", "");
                //command.Parameters.AddWithValue("@GRP", "");
                //command.Parameters.AddWithValue("@PSTDATE", ""); 
                Connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    model = new InvoiceStatus();

                    model.COMP = dr["COMP"].ToString();
                    model.DOCNUM = dr["DOCNUM"].ToString();
                    //model.PSTDAT = dr["PSTDAT"].ToString();
                    PSTDAT = dr["PSTDAT"].ToString();
                    if (PSTDAT != "")
                    {
                        DateTime dateLastInvdate = Convert.ToDateTime(PSTDAT);
                        formattedLInvdate = dateLastInvdate.ToString("dd/MM/yyyy");
                        model.PSTDAT = formattedLInvdate;
                    }
                    else
                    {
                        model.PSTDAT = "-";

                    }
                    model.PEOPLE = dr["PEOPLE"].ToString();
                    model.CUSNAM = dr["CUSNAM"].ToString();
                    model.SLMCOD = dr["SLMCOD"].ToString();
                    model.SEC = dr["SEC"].ToString();
                    model.STKGRP = dr["STKGRP"].ToString();
                    model.STKCOD = dr["STKCOD"].ToString();
                    model.STKDES = dr["STKDES"].ToString();
                    model.Qty = dr["Qty"].ToString();
                    model.Price = dr["@Price"].ToString();
                    model.Amt = dr["Amt"].ToString();
                    model.DiscountPer = dr["Discount%"].ToString();
                    model.DiscountAmt = dr["DiscountAmt"].ToString();
                    model.Net_Price = dr["Net_Price"].ToString();
                    model.NetAmt = dr["NetAmt"].ToString();
                    model.FOC = dr["FOC"].ToString();
                    model.AsOf = dr["AsOf"].ToString();
                    model.Uom = dr["UOM"].ToString();
                    model.Prod = dr["PROD"].ToString();
                    model.ProdName = dr["PRODNAM"].ToString();
                    model.GrpName = dr["GRPNAM"].ToString();
                    model.QTY_Remaining = dr["QTY_Remaining"].ToString();
                    //model.LastClaimNo = dr["LastClaimNo"].ToString();
                    //model.LastClaimdate = dr["LastClaimdate"].ToString();
                    //model.ClaimStatus = dr["ClaimStatus"].ToString();
                    model.LastClaimNo = "";
                    model.LastClaimdate = "";
                    model.ClaimStatus = "";
                    model.EXTDOC = dr["EXTDOC"].ToString();
                    model.CrRefNo = dr["CrRefNo"].ToString();
                    Getdata.Add(new InvoiceStatusListDetailGetdata { val = model });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { Getdata, message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Getdatahis_Date_2year(string _flength, string _foc, string _cusno, string _invoiceno, string _itemno, string _com, string _psdate, string _statdate, string _enddate, string _slmcod)
        {
            string PSTDAT = string.Empty;
            string message = string.Empty;
            string formattedLInvdate = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<InvoiceStatusListDetailGetdata> Getdata = new List<InvoiceStatusListDetailGetdata>();
            InvoiceStatus model = null;
            try
            {
                var command = new SqlCommand("P_Getdatehis_Date_2year", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUS", _cusno);
                command.Parameters.AddWithValue("@PN", _itemno);
                command.Parameters.AddWithValue("@DOC", _invoiceno);
                command.Parameters.AddWithValue("@GRP", "");
                command.Parameters.AddWithValue("@PSTDATEStat", _statdate);
                command.Parameters.AddWithValue("@PSTDATEEnd", _enddate);
                command.Parameters.AddWithValue("@Com", _com);
                command.Parameters.AddWithValue("@SLM", _slmcod);
                command.Parameters.AddWithValue("@FOC", _foc);
                command.Parameters.AddWithValue("@flength", _flength);

                Connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    model = new InvoiceStatus();

                    model.COMP = dr["COMP"].ToString();
                    model.DOCNUM = dr["DOCNUM"].ToString();
                    //model.PSTDAT = dr["PSTDAT"].ToString();
                    PSTDAT = dr["PSTDAT"].ToString();
                    if (PSTDAT != "")
                    {
                        DateTime dateLastInvdate = Convert.ToDateTime(PSTDAT);
                        formattedLInvdate = dateLastInvdate.ToString("dd/MM/yyyy");
                        model.PSTDAT = formattedLInvdate;
                    }
                    else
                    {
                        model.PSTDAT = "-";

                    }
                    model.PEOPLE = dr["PEOPLE"].ToString();
                    model.CUSNAM = dr["CUSNAM"].ToString();
                    model.SLMCOD = dr["SLMCOD"].ToString();
                    model.SEC = dr["SEC"].ToString();
                    model.STKGRP = dr["STKGRP"].ToString();
                    model.STKCOD = dr["STKCOD"].ToString();
                    model.STKDES = dr["STKDES"].ToString();
                    model.Qty = dr["Qty"].ToString();
                    model.Price = dr["@Price"].ToString();
                    model.Amt = dr["Amt"].ToString();
                    model.DiscountPer = dr["Discount%"].ToString();
                    model.DiscountAmt = dr["DiscountAmt"].ToString();
                    model.Net_Price = dr["Net_Price"].ToString();
                    model.NetAmt = dr["NetAmt"].ToString();
                    model.FOC = dr["FOC"].ToString();
                    model.AsOf = dr["AsOf"].ToString();
                    model.Uom = dr["UOM"].ToString();
                    model.Prod = dr["PROD"].ToString();
                    model.ProdName = dr["PRODNAM"].ToString();
                    model.GrpName = dr["GRPNAM"].ToString();
                    model.QTY_Remaining = dr["QTY_Remaining"].ToString();
                    //model.LastClaimNo = dr["LastClaimNo"].ToString();
                    //model.LastClaimdate = dr["LastClaimdate"].ToString();
                    //model.ClaimStatus = dr["ClaimStatus"].ToString();
                    model.LastClaimNo = "";
                    model.LastClaimdate = "";
                    model.ClaimStatus = "";
                    model.EXTDOC = dr["EXTDOC"].ToString();
                    model.CrRefNo = dr["CrRefNo"].ToString();
                    Getdata.Add(new InvoiceStatusListDetailGetdata { val = model });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                Connection.Close();
                message = "true";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { Getdata, message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataInvoicestatudRT(string _flength, string _foc, string _cusno, string _invoiceno, string _itemno, string _com, string _psdate, string _statdate, string _enddate, string _slmcod)
        {
            string PSTDAT = string.Empty;
            string message = string.Empty;
            string formattedLInvdate = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<InvoiceStatusListDetailGetdata> Getdata = new List<InvoiceStatusListDetailGetdata>();
            InvoiceStatus model = null;
            try
            {
                var command = new SqlCommand("P_InvoiceCheck_ByCusItm_Date_2year", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUS", _cusno);
                command.Parameters.AddWithValue("@PN", _itemno);
                command.Parameters.AddWithValue("@DOC", _invoiceno);
                command.Parameters.AddWithValue("@GRP", "");
                command.Parameters.AddWithValue("@PSTDATEStat", _statdate);
                command.Parameters.AddWithValue("@PSTDATEEnd", _enddate);
                command.Parameters.AddWithValue("@Com", _com);
                command.Parameters.AddWithValue("@SLM", _slmcod);
                command.Parameters.AddWithValue("@FOC", _foc);
                command.Parameters.AddWithValue("@flength", _flength);

                Connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    model = new InvoiceStatus();

                    model.COMP = dr["COMP"].ToString();
                    model.DOCNUM = dr["DOCNUM"].ToString();
                    //model.PSTDAT = dr["PSTDAT"].ToString();
                    PSTDAT = dr["PSTDAT"].ToString();
                    if (PSTDAT != "")
                    {
                        DateTime dateLastInvdate = Convert.ToDateTime(PSTDAT);
                        formattedLInvdate = dateLastInvdate.ToString("dd/MM/yyyy");
                        model.PSTDAT = formattedLInvdate;
                    }
                    else
                    {
                        model.PSTDAT = "-";

                    }
                    model.PEOPLE = dr["PEOPLE"].ToString();
                    model.CUSNAM = dr["CUSNAM"].ToString();
                    model.SLMCOD = dr["SLMCOD"].ToString();
                    model.SEC = dr["SEC"].ToString();
                    model.STKGRP = dr["STKGRP"].ToString();
                    model.STKCOD = dr["STKCOD"].ToString();
                    model.STKDES = dr["STKDES"].ToString();
                    model.Qty = dr["Qty"].ToString();
                    model.Price = dr["@Price"].ToString();
                    model.Amt = dr["Amt"].ToString();
                    model.DiscountPer = dr["Discount%"].ToString();
                    model.DiscountAmt = dr["DiscountAmt"].ToString();
                    model.Net_Price = dr["Net_Price"].ToString();
                    model.NetAmt = dr["NetAmt"].ToString();
                    model.FOC = dr["FOC"].ToString();
                    model.AsOf = dr["AsOf"].ToString();
                    model.Uom = dr["UOM"].ToString();
                    model.Prod = dr["PROD"].ToString();
                    model.ProdName = dr["PRODNAM"].ToString();
                    model.GrpName = dr["GRPNAM"].ToString();
                    model.QTY_Remaining = dr["QTY_Remaining"].ToString();
                    //model.LastClaimNo = dr["LastClaimNo"].ToString();
                    //model.LastClaimdate = dr["LastClaimdate"].ToString();
                    //model.ClaimStatus = dr["ClaimStatus"].ToString();
                    model.LastClaimNo = "";
                    model.LastClaimdate = "";
                    model.ClaimStatus = "";
                    model.EXTDOC = dr["EXTDOC"].ToString();
                    model.CrRefNo = dr["CrRefNo"].ToString();
                    Getdata.Add(new InvoiceStatusListDetailGetdata { val = model });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                Connection.Close();
                message = "true";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { Getdata, message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataInvoiceByTech(string _qty, string _flength, string _foc, string _cusno, string _invoiceno, string _itemno, string _com, string _psdate, string _statdate, string _enddate, string _slmcod)
        {
            string message = string.Empty;
            string PSTDAT = string.Empty;
            string formattedLInvdate = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<InvoiceStatusListDetailGetdata> Getdata = new List<InvoiceStatusListDetailGetdata>();
            InvoiceStatus model = null;
            try
            {

                var command = new SqlCommand("P_InvoiceCheck_ByTech", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUS", _cusno);
                command.Parameters.AddWithValue("@PN", _itemno);
                command.Parameters.AddWithValue("@DOC", _invoiceno);
                command.Parameters.AddWithValue("@Qty", _qty);
                command.Parameters.AddWithValue("@PSTDATEStat", _statdate);
                command.Parameters.AddWithValue("@PSTDATEEnd", _enddate);
                command.Parameters.AddWithValue("@Com", _com);
                command.Parameters.AddWithValue("@SLM", _slmcod);
                command.Parameters.AddWithValue("@FOC", _foc);
                command.Parameters.AddWithValue("@flength", _flength);
                //command.Parameters.AddWithValue("@PN", "JTC215");
                //command.Parameters.AddWithValue("@DOC", "");
                //command.Parameters.AddWithValue("@GRP", "");
                //command.Parameters.AddWithValue("@PSTDATE", ""); 
                Connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    model = new InvoiceStatus();

                    model.COMP = dr["COMP"].ToString();
                    model.DOCNUM = dr["DOCNUM"].ToString();
                    //model.PSTDAT = dr["PSTDAT"].ToString();
                    PSTDAT = dr["PSTDAT"].ToString();
                    if (PSTDAT != "")
                    {
                        DateTime dateLastInvdate = Convert.ToDateTime(PSTDAT);
                        formattedLInvdate = dateLastInvdate.ToString("dd/MM/yyyy");
                        model.PSTDAT = formattedLInvdate;
                    }
                    else
                    {
                        model.PSTDAT = "-";

                    }
                    model.PEOPLE = dr["PEOPLE"].ToString();
                    model.CUSNAM = dr["CUSNAM"].ToString();
                    model.SLMCOD = dr["SLMCOD"].ToString();
                    model.SEC = dr["SEC"].ToString();
                    model.STKGRP = dr["STKGRP"].ToString();
                    model.STKCOD = dr["STKCOD"].ToString();
                    model.STKDES = dr["STKDES"].ToString();
                    model.Qty = dr["Qty"].ToString();
                    model.Price = dr["@Price"].ToString();
                    model.Amt = dr["Amt"].ToString();
                    model.DiscountPer = dr["Discount%"].ToString();
                    model.DiscountAmt = dr["DiscountAmt"].ToString();
                    model.Net_Price = dr["Net_Price"].ToString();
                    model.NetAmt = dr["NetAmt"].ToString();
                    model.FOC = dr["FOC"].ToString();
                    model.AsOf = dr["AsOf"].ToString();
                    model.Uom = dr["UOM"].ToString();
                    model.Prod = dr["PROD"].ToString();
                    model.ProdName = dr["PRODNAM"].ToString();
                    model.GrpName = dr["GRPNAM"].ToString();
                    model.QTY_Remaining = dr["QTY_Remaining"].ToString();
                    //model.LastClaimNo = dr["LastClaimNo"].ToString();
                    //model.LastClaimdate = dr["LastClaimdate"].ToString();
                    //model.ClaimStatus = dr["ClaimStatus"].ToString();
                    model.LastClaimNo = "";
                    model.LastClaimdate = "";
                    model.ClaimStatus = "";
                    model.EXTDOC = dr["EXTDOC"].ToString();
                    model.CrRefNo = dr["CrRefNo"].ToString();
                    Getdata.Add(new InvoiceStatusListDetailGetdata { val = model });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { Getdata, message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetTechLocation(string Name)
        {

            List<TechLo> List = new List<TechLo>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string company = Session["company"].ToString();
            string whCompany = string.Empty;
            whCompany = " AND company <> 'TAM'";
            if (company == "TAM" || company == "VELOX")
            {
                whCompany = " AND company = 'TAM'";
            }

            SqlCommand cmd = new SqlCommand("select * From TechLocation where DefineCode = '" + Name + "'" + whCompany, Connection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new TechLo()
                {
                    DefineCode = dr["DefineCode"].ToString(),
                    Location = dr["Location"].ToString()
                });


            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetdataCountProcess(string Usertype, string Userlogin)
        //{
        //    var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
        //    SqlConnection Connection = new SqlConnection(connectionString);
        //    List<ListCountProcess> Getdata = new List<ListCountProcess>();
        //    CountProcess model = null;
        //    var command = new SqlCommand("P_Count_Process", Connection);
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.AddWithValue("@Usertyp", Usertype);
        //    command.Parameters.AddWithValue("@Userlogin", Userlogin);

        //    Connection.Open();
        //    SqlDataReader dr = command.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        model = new CountProcess();
        //        model.CountAdmin = dr["CountAdmin"].ToString();
        //        model.Status = dr["Status"].ToString();
        //        model.company = dr["company"].ToString();
        //        Getdata.Add(new ListCountProcess { val = model });
        //    }

        //    dr.Close();
        //    dr.Dispose();
        //    command.Dispose();
        //    Connection.Close();
        //    return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetdataCountProcess(string Usertype, string Userlogin)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;

            List<ListCountProcess> Getdata = new List<ListCountProcess>();

            try
            {
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        Connection.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Connection Error\nConnStr: " + connectionString + "\n" + ex.ToString());
                    }

                    using (SqlCommand command = new SqlCommand("P_Count_Process", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Usertyp", Usertype);
                        command.Parameters.AddWithValue("@Userlogin", Userlogin);

                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var model = new CountProcess
                                {
                                    CountAdmin = dr["CountAdmin"].ToString(),
                                    Status = dr["Status"].ToString(),
                                    company = dr["company"].ToString()
                                };

                                Getdata.Add(new ListCountProcess { val = model });
                            }
                        }
                    }
                }

                return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // return error ออกไปดูใน browser / postman
                return Json(new
                {
                    error = true,
                    message = ex.Message,
                    detail = ex.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductall()
        {
            string company = Session["company"].ToString();
            string tbProd = string.Empty;
            tbProd = " v_SLMTAB_PD";
            if (company == "TAM")
            {
                tbProd = " v_SLMTAB_PD_TAM";
            }
            List<Pm> List = new List<Pm>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            SqlCommand command = new SqlCommand("select * From " + tbProd, Connection);

            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new Pm()
                {
                    PROD = dr["SLMCOD"].ToString(),
                    PRODNAM = dr["SLMNAM"].ToString()
                });

            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReasonCodes()
        {
            List<Pm> List = new List<Pm>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            SqlCommand command = new SqlCommand("select * From Reason_Codes where Code like'CN%' ", Connection);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new Pm()
                {
                    PROD = dr["Code"].ToString(),
                    PRODNAM = dr["Description"].ToString()
                });

            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataClamline(string CLM_No, string CLM_SUB)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<ClimeListDetail> Getdata = new List<ClimeListDetail>();
            ClimeDetail model = null;
            var command = new SqlCommand("P_Search_ProcessClaim_Detail", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", CLM_No);
            command.Parameters.AddWithValue("@DOCSUB", CLM_SUB);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ClimeDetail();
                model.CLM_ADMIN = dr["CLM_ADMIN"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.CLM_REMARK = dr["CLM_REMARK"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_QTY_ORG = dr["CLM_QTY_ORG"].ToString();
                model.CLM_QTY_PASS = dr["CLM_QTY_PASS"].ToString();
                model.CLM_QTY_REJECT = dr["CLM_QTY_REJECT"].ToString();
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
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
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_ANLYS_RESULT = dr["TECH1_ANLYS_RESULT"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.TECH2_REMARK = dr["TECH2_REMARK"].ToString();

                model.TECH2_ProblemCategory = dr["TECH2_ProblemCategory"].ToString();
                model.TECH2_ProblemCategoryDetail = dr["TECH2_ProblemCategoryDetail"].ToString();

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
                model.SM_ANLYS_STATUS = dr["SM_ANLYS_STATUS"].ToString();

                model.GM_NAME = dr["GM_NAME"].ToString();
                model.GM_APPRV_DATE = dr["GM_APPRV_DATE"].ToString();
                model.GM_PROCESS_STATUS = dr["GM_PROCESS_STATUS"].ToString();
                model.GM_REMARK = dr["GM_REMARK"].ToString();

                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.CLM_UPDATE_DATE = dr["CLM_UPDATE_DATE"].ToString();
                model.CLM_UPDATE_BY = dr["CLM_UPDATE_BY"].ToString();
                //model.CLM_UPDATE_DATE = "";
                //model.CLM_UPDATE_BY = "";
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
                model.Customer = dr["CUSCOD"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.TechnicianName = dr["TechnicianName"].ToString();
                // model.ClmtypeTech = dr["ClmtypeTech"].ToString();
                model.Clmtype = dr["Clmtype"].ToString();
                model.Process_Tech = dr["Process_Tech"].ToString();
                model.Process_PM = dr["Process_PM"].ToString();
                model.TEC_ANLYS_STATUS = dr["TEC_ANLYS_STATUS"].ToString();
                model.TEC_PROCESS_STATUS = dr["TEC_PROCESS_STATUS"].ToString();
                model.TEC_ANLYS_AFTERPROCESS = dr["TEC_ANLYS_AFTERPROCESS"].ToString();
                model.PM_ANLYS_STATUS = dr["PM_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS_ANLYS = dr["PM_PROCESS_STATUS_ANLYS"].ToString();
                model.PM_ANLYS_AFTERPROCESS = dr["PM_ANLYS_AFTERPROCESS"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_Installdate = dr["CLM_Installdate"].ToString();
                model.CLM_Contact = dr["CLM_Contact"].ToString();
                model.CLM_ContactTel = dr["CLM_ContactTel"].ToString();
                model.CLM_COMMENT = dr["CLM_COMMENT"].ToString();
                model.CLM_CLAIMNOTE = dr["CLM_ClaimNote"].ToString();
                Getdata.Add(new ClimeListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetClimdata(string inCLM_ID, string inCLM_SUB)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaim", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", inCLM_ID);
            // command.Parameters.AddWithValue("@SubDOC", inCLM_SUB);
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
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_QTY_ORG = dr["CLM_QTY_ORG"].ToString();
                model.CLM_QTY_PASS = dr["CLM_QTY_PASS"].ToString();
                model.CLM_QTY_REJECT = dr["CLM_QTY_REJECT"].ToString();
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
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.CLM_FOC = dr["CLM_Foc"].ToString();
                model.SM_NAME = dr["SM_NAME"].ToString();
                model.SM_APPRV_DATE = dr["SM_APPRV_DATE"].ToString();
                model.SM_PROCESS_STATUS = dr["SM_PROCESS_STATUS"].ToString();
                model.SM_REMARK = dr["SM_REMARK"].ToString();

                model.GM_NAME = dr["GM_NAME"].ToString();
                model.GM_APPRV_DATE = dr["GM_APPRV_DATE"].ToString();
                model.GM_PROCESS_STATUS = dr["GM_PROCESS_STATUS"].ToString();
                model.GM_REMARK = dr["GM_REMARK"].ToString();
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimwhdata(string inCLM_ID, string inCLM_SUB)
        {
            string message = string.Empty;
            ClimeWhdata model = null;
            List<ClimewhListDetail> Getdata = new List<ClimewhListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaim_WH", Connection);
            //var command = new SqlCommand("P_Search_ProcessClaim_WH_Test2021", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", inCLM_ID);
            command.Parameters.AddWithValue("@SubDOC", inCLM_SUB);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ClimeWhdata();
                model.REQ_NO = dr["REQ_NO"].ToString();
                model.COMP = dr["COMP"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.REQ_Update_BY = dr["REQ_Update_BY"].ToString();
                model.REQ_Update_DATE = dr["REQ_Update_DATE"].ToString();
                model.REQ_Dep_BY = dr["REQ_Dep_BY"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_STKGRP = dr["CLM_STKGRP"].ToString();
                model.CLM_Location = dr["CLM_Location"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.INV_QTY = dr["INV_QTY"].ToString();
                model.CLM_INVDATE = dr["CLM_INVDATE"].ToString();
                model.CLM_REQ_QTY = dr["CLM_REQ_QTY"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.CLM_DUEDATE = dr["CLM_DUEDATE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_PERFORM_DES = dr["CLM_PERFORM_DES"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.CLM_ADMIN = dr["CLM_ADMIN"].ToString();
                model.CLM_ADMIN_REMARK = dr["CLM_ADMIN_REMARK"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.TECH1_NAME = dr["TECH1_NAME"].ToString();
                model.TECH1_ANLYS_DATE = dr["TECH1_ANLYS_DATE"].ToString();
                model.TECH1_ANLYS_RESULT = dr["TECH1_ANLYS_RESULT"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.ANLYS_AFTERPROCESS = dr["ANLYS_AFTERPROCESS"].ToString();
                model.SCRAP_DATE = dr["SCRAP_DATE"].ToString();
                model.CLM_TECH1_QTY = dr["CLM_TECH1_QTY"].ToString();
                model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                model.TECH2_ANLYS_DATE = dr["TECH2_ANLYS_DATE"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_REMARK = dr["TECH2_REMARK"].ToString();
                model.CLM_TECH2_QTY = dr["CLM_TECH2_QTY"].ToString();
                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.PM_Replacement = dr["PM_Replacement"].ToString();
                model.PM_REMARK = dr["PM_REMARK"].ToString();
                model.CLM_PM_QTY = dr["CLM_PM_QTY"].ToString();
                model.User_Print = dr["User_Print"].ToString();
                model.CLM_UPDATE_BY = dr["CLM_UPDATE_BY"].ToString();
                model.CLM_UPDATE_DATE = dr["CLM_UPDATE_DATE"].ToString();
                model.SEC = dr["SEC"].ToString();
                model.DEPNAM = dr["DEPNAM"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                model.GRPNAM = dr["GRPNAM"].ToString();
                model.Technician = dr["Technician"].ToString();
                model.Clmtype = dr["Clmtype"].ToString();
                model.Process_Tech = dr["Process_Tech"].ToString();
                model.Process_PM = dr["Process_PM"].ToString();
                model.TEC_ANLYS_STATUS = dr["TEC_ANLYS_STATUS"].ToString();
                model.TEC_PROCESS_STATUS = dr["TEC_PROCESS_STATUS"].ToString();
                model.TEC_ANLYS_AFTERPROCESS = dr["TEC_ANLYS_AFTERPROCESS"].ToString();
                model.PM_ANLYS_STATUS = dr["PM_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS_ANLYS = dr["PM_PROCESS_STATUS_ANLYS"].ToString();
                model.PM_ANLYS_AFTERPROCESS = dr["PM_ANLYS_AFTERPROCESS"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.TECH1NAME = dr["TECH1NAME"].ToString();
                model.CLM_TECH2_QTY = dr["CLM_TECH2_QTY"].ToString();
                model.CLM_PM_QTY = dr["CLM_PM_QTY"].ToString();
                model.PM_AFTER_APPRV = dr["PM_AFTER_APPRV"].ToString();
                model.CLM_COMMENT = dr["CLM_COMMENT"].ToString();
                model.CLM_TECH2_WASTE_QTY = dr["CLM_TECH2_WASTE_QTY"].ToString();
                model.CLM_TECH2_GOOD_QTY = dr["CLM_TECH2_GOOD_QTY"].ToString();
                model.CLM_BatchCode = dr["CLM_BatchCode"].ToString();
                model.PM_Option1 = dr["PM_Option1"].ToString();
                model.PM_Optiontext1 = dr["PM_Optiontext1"].ToString();
                model.PM_Option2 = dr["PM_Option2"].ToString();
                model.PM_Optiontext2 = dr["PM_Optiontext2"].ToString();
                model.PM_Option3 = dr["PM_Option3"].ToString();
                model.PM_Optiontext3 = dr["PM_Optiontext3"].ToString();
                model.PM_Option4 = dr["PM_Option4"].ToString();
                model.PM_Optiontext4 = dr["PM_Optiontext4"].ToString();
                model.PM_Option5 = dr["PM_Option5"].ToString();
                model.PM_Optiontext5 = dr["PM_Optiontext5"].ToString();
                model.ADMINWH_STATUS = dr["ADMINWH_STATUS"].ToString();
                model.ADMINWH_BY = dr["ADMINWH_BY"].ToString();
                model.ADMINWH_UPDATE = dr["ADMINWH_UPDATE"].ToString();
                model.ADMINWH_REMARK = dr["ADMINWH_REMARK"].ToString();
                model.UNITCOST = dr["UNITCOST"].ToString();
                model.TOTALCOST = dr["TOTALCOST"].ToString();

                model.BU_Head = dr["BU_Head"].ToString();
                model.BU_APPRV_DATE = dr["BU_APPRV_DATE"].ToString();
                model.BU_APPRV_STATUS = dr["BU_APPRV_STATUS"].ToString();
                model.BU_REMARK = dr["BU_REMARK"].ToString();
                model.CLM_BU_QTY = dr["CLM_BU_QTY"].ToString();
                model.CLM_BU_Ref = dr["CLM_BU_Ref"].ToString();
                model.BU_HeadNAM = dr["BU_HeadNAM"].ToString();

                model.MD_Head = dr["MD_Head"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_REMARK = dr["MD_REMARK"].ToString();
                model.CLM_MD_QTY = dr["CLM_MD_QTY"].ToString();
                model.CLM_MD_Ref = dr["CLM_MD_Ref"].ToString();
                Getdata.Add(new ClimewhListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetdataCusShipping(string XXcus, string XXslm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<ItemListshipto> Getdata = new List<ItemListshipto>();
            shipto model = null;

            //  SqlCommand cmd = new SqlCommand("select *    from dbo.v_NVcust_ShiptoAddr  where [customer No_] ='" + XXcus + "' order by [Code] ", Connection);
            // SqlDataReader rev_Mod = cmd.ExecuteReader();
            var command = new SqlCommand("P_Search_cust_ShiptoAddr_Customer", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inCUS", XXcus);
            command.Parameters.AddWithValue("@inSLMCOD", XXslm);
            SqlDataReader rev_Mod = command.ExecuteReader();

            while (rev_Mod.Read())
            {
                model = new shipto();
                model.customer = rev_Mod["Customer No_"].ToString();
                model.code = rev_Mod["Code"].ToString();
                model.name = rev_Mod["Name"].ToString();
                model.name2 = rev_Mod["Name 2"].ToString();
                model.address = rev_Mod["Address"].ToString();
                model.address2 = rev_Mod["Address 2"].ToString();
                model.city = rev_Mod["City"].ToString();
                Getdata.Add(new ItemListshipto { val = model });
            }

            rev_Mod.Close();
            rev_Mod.Dispose();
            command.Dispose();

            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetdataCusMasterAdd(string XXcus)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<ItemListshipto> Getdata = new List<ItemListshipto>();
            shipto model = null;

            //  SqlCommand cmd = new SqlCommand("select *    from dbo.v_NVcust_ShiptoAddr  where [customer No_] ='" + XXcus + "' order by [Code] ", Connection);
            // SqlDataReader rev_Mod = cmd.ExecuteReader();
            var command = new SqlCommand("P_Search_cust_Addr_Master", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inCUS", XXcus);

            SqlDataReader rev_Mod = command.ExecuteReader();

            while (rev_Mod.Read())
            {
                model = new shipto();
                model.customer = rev_Mod["Customer No_"].ToString();
                model.code = rev_Mod["Code"].ToString();
                model.name = rev_Mod["Name"].ToString();
                model.name2 = rev_Mod["Name 2"].ToString();
                model.address = rev_Mod["Address"].ToString();
                model.address2 = rev_Mod["Address 2"].ToString();
                model.city = rev_Mod["City"].ToString();
                Getdata.Add(new ItemListshipto { val = model });
            }

            rev_Mod.Close();
            rev_Mod.Dispose();
            command.Dispose();

            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetdataRcvStstus(string CM_NO)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string countStr = string.Empty;

            SqlCommand cmd = new SqlCommand("select * from dbo.v_COUNT_RCVSTATUS  where [CLM_ID] ='" + CM_NO + "'", Connection);
            SqlDataReader rev_Mod = cmd.ExecuteReader();

            while (rev_Mod.Read())
            {
                countStr = rev_Mod["No"].ToString();
            }
            //S20161016
            rev_Mod.Close();
            rev_Mod.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(countStr, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataRcvStstusRT(string CM_NO)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string countStr = string.Empty;

            SqlCommand cmd = new SqlCommand("select * from dbo.v_COUNT_RCVSTATUS_RT  where [STMP_ID] ='" + CM_NO + "'", Connection);
            SqlDataReader rev_Mod = cmd.ExecuteReader();

            while (rev_Mod.Read())
            {
                countStr = rev_Mod["No"].ToString();
            }
            //S20161016
            rev_Mod.Close();
            rev_Mod.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(countStr, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetClimdataPm(string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaim_Pm", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
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
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_QTY_ORG = dr["CLM_QTY_ORG"].ToString();
                model.CLM_QTY_PASS = dr["CLM_QTY_PASS"].ToString();
                model.CLM_QTY_REJECT = dr["CLM_QTY_REJECT"].ToString();
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
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_Ref = dr["CLM_Ref"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                model.CLM_FOC = dr["CLM_FOC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.Requestdate = dr["Requestdate"].ToString();
                model.Requestdatecus = dr["CusRequestdate"].ToString();

                model.SM_NAME = dr["SM_NAME"].ToString();
                model.SM_APPRV_DATE = dr["SM_APPRV_DATE"].ToString();
                model.SM_PROCESS_STATUS = dr["SM_PROCESS_STATUS"].ToString();
                model.SM_REMARK = dr["SM_REMARK"].ToString();

                model.GM_NAME = dr["GM_NAME"].ToString();
                model.GM_APPRV_DATE = dr["GM_APPRV_DATE"].ToString();
                model.GM_PROCESS_STATUS = dr["GM_PROCESS_STATUS"].ToString();
                model.GM_REMARK = dr["GM_REMARK"].ToString();
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            //return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

            return new JsonResult() { Data = Getdata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            //var jsonResult = Json(Getdata, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            // return jsonResult;


        }
        //ค้นหาข้อมูลหน้า Approve Claim Customer
        public JsonResult GetClimdataSm(string inUser, string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaim_Sm", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUser", inUser);
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
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
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_QTY_ORG = dr["CLM_QTY_ORG"].ToString();
                model.CLM_QTY_PASS = dr["CLM_QTY_PASS"].ToString();
                model.CLM_QTY_REJECT = dr["CLM_QTY_REJECT"].ToString();
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
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_Ref = dr["CLM_Ref"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                model.CLM_FOC = dr["CLM_FOC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.Requestdate = dr["Requestdate"].ToString();
                model.Requestdatecus = dr["CusRequestdate"].ToString();

                model.SM_NAME = dr["SM_NAME"].ToString();
                model.SM_APPRV_DATE = dr["SM_APPRV_DATE"].ToString();
                model.SM_PROCESS_STATUS = dr["SM_PROCESS_STATUS"].ToString();
                model.SM_REMARK = dr["SM_REMARK"].ToString();

                model.GM_NAME = dr["GM_NAME"].ToString();
                model.GM_APPRV_DATE = dr["GM_APPRV_DATE"].ToString();
                model.GM_PROCESS_STATUS = dr["GM_PROCESS_STATUS"].ToString();
                model.GM_REMARK = dr["GM_REMARK"].ToString();

                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClimdataGm(string inUser, string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaim_Gm", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUser", inUser);
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
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
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_QTY_ORG = dr["CLM_QTY_ORG"].ToString();
                model.CLM_QTY_PASS = dr["CLM_QTY_PASS"].ToString();
                model.CLM_QTY_REJECT = dr["CLM_QTY_REJECT"].ToString();
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
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_Ref = dr["CLM_Ref"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                model.CLM_FOC = dr["CLM_FOC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.Requestdate = dr["Requestdate"].ToString();
                model.Requestdatecus = dr["CusRequestdate"].ToString();

                model.SM_NAME = dr["SM_NAME"].ToString();
                model.SM_APPRV_DATE = dr["SM_APPRV_DATE"].ToString();
                model.SM_PROCESS_STATUS = dr["SM_PROCESS_STATUS"].ToString();
                model.SM_REMARK = dr["SM_REMARK"].ToString();

                model.GM_NAME = dr["GM_NAME"].ToString();
                model.GM_APPRV_DATE = dr["GM_APPRV_DATE"].ToString();
                model.GM_PROCESS_STATUS = dr["GM_PROCESS_STATUS"].ToString();
                model.GM_REMARK = dr["GM_REMARK"].ToString();
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            //return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
            return new JsonResult() { Data = Getdata, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetClimWHdataPm(string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaimWH_Pm", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Climedata();
                model.CLM_ID = dr["CLM_ID"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.CLM_INVDATE = dr["CLM_INVDATE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.CLM_PERFORM_DES = dr["CLM_PERFORM_DES"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_Ref = dr["CLM_Ref"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                //model.CLM_FOC = dr["CLM_FOC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.Requestdate = dr["Requestdate"].ToString();
                //model.Requestdatecus = dr["CusRequestdate"].ToString();
                model.CLM_TECH1_QTY = dr["CLM_TECH1_QTY"].ToString();
                model.CLM_TECH2_QTY = dr["CLM_TECH2_QTY"].ToString();
                model.CLM_PM_QTY = dr["CLM_PM_QTY"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_Dep_BY = dr["REQ_Dep_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.PM_Option1 = dr["PM_Option1"].ToString();
                model.PM_Optiontext1 = dr["PM_Optiontext1"].ToString();
                model.PM_Option2 = dr["PM_Option2"].ToString();
                model.PM_Optiontext2 = dr["PM_Optiontext2"].ToString();
                model.PM_Option3 = dr["PM_Option3"].ToString();
                model.PM_Optiontext3 = dr["PM_Optiontext3"].ToString();
                model.PM_Option4 = dr["PM_Option4"].ToString();
                model.PM_Optiontext4 = dr["PM_Optiontext4"].ToString();
                model.PM_Option5 = dr["PM_Option5"].ToString();
                model.PM_Optiontext5 = dr["PM_Optiontext5"].ToString();
                model.PM_Optiontext5 = dr["PM_Optiontext5"].ToString();
                model.PM_Optiontext5 = dr["PM_Optiontext5"].ToString();

                model.BU_Head = dr["BU_Head"].ToString();
                model.BU_APPRV_DATE = dr["BU_APPRV_DATE"].ToString();
                model.BU_APPRV_STATUS = dr["BU_APPRV_STATUS"].ToString();
                model.BU_REMARK = dr["BU_REMARK"].ToString();
                model.CLM_BU_QTY = dr["CLM_BU_QTY"].ToString();
                model.CLM_BU_Ref = dr["CLM_BU_Ref"].ToString();

                model.UNITCOST = dr["UNITCOST"].ToString();
                model.TOTCOST = dr["totcost"].ToString();

                model.MD_Head = dr["MD_Head"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_REMARK = dr["MD_REMARK"].ToString();
                model.CLM_MD_QTY = dr["CLM_MD_QTY"].ToString();
                model.CLM_MD_Ref = dr["CLM_MD_Ref"].ToString();


                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimWHdataApproveDM(string inBUCODE, string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaimWH_Approve_DM", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inBU", inBUCODE);
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Climedata();
                model.CLM_ID = dr["CLM_ID"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.CLM_INVDATE = dr["CLM_INVDATE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.CLM_PERFORM_DES = dr["CLM_PERFORM_DES"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_Ref = dr["CLM_Ref"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                //model.CLM_FOC = dr["CLM_FOC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.Requestdate = dr["Requestdate"].ToString();
                //model.Requestdatecus = dr["CusRequestdate"].ToString();
                model.CLM_TECH1_QTY = dr["CLM_TECH1_QTY"].ToString();
                model.CLM_TECH2_QTY = dr["CLM_TECH2_QTY"].ToString();
                model.CLM_PM_QTY = dr["CLM_PM_QTY"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_Dep_BY = dr["REQ_Dep_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.PM_Option1 = dr["PM_Option1"].ToString();
                model.PM_Optiontext1 = dr["PM_Optiontext1"].ToString();
                model.PM_Option2 = dr["PM_Option2"].ToString();
                model.PM_Optiontext2 = dr["PM_Optiontext2"].ToString();
                model.PM_Option3 = dr["PM_Option3"].ToString();
                model.PM_Optiontext3 = dr["PM_Optiontext3"].ToString();
                model.PM_Option4 = dr["PM_Option4"].ToString();
                model.PM_Optiontext4 = dr["PM_Optiontext4"].ToString();
                model.PM_Option5 = dr["PM_Option5"].ToString();
                model.PM_Optiontext5 = dr["PM_Optiontext5"].ToString();
                model.BU_Head = dr["BU_Head"].ToString();
                model.BU_APPRV_DATE = dr["BU_APPRV_DATE"].ToString();
                model.BU_APPRV_STATUS = dr["BU_APPRV_STATUS"].ToString();
                model.BU_REMARK = dr["BU_REMARK"].ToString();
                model.CLM_BU_QTY = dr["CLM_BU_QTY"].ToString();
                model.CLM_BU_Ref = dr["CLM_BU_Ref"].ToString();

                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.UNITCOST = dr["UNITCOST"].ToString();
                model.totcost = dr["totcost"].ToString();
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimWHdataApproveDM_MD(string inBUCODE, string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaimWH_Approve_DM_MD_20052022", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inBU", inBUCODE);
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Climedata();
                model.CLM_ID = dr["CLM_ID"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.CLM_INVDATE = dr["CLM_INVDATE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.CLM_PERFORM_DES = dr["CLM_PERFORM_DES"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_Ref = dr["CLM_Ref"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                //model.CLM_FOC = dr["CLM_FOC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.Requestdate = dr["Requestdate"].ToString();
                //model.Requestdatecus = dr["CusRequestdate"].ToString();
                model.CLM_TECH1_QTY = dr["CLM_TECH1_QTY"].ToString();
                model.CLM_TECH2_QTY = dr["CLM_TECH2_QTY"].ToString();
                model.CLM_PM_QTY = dr["CLM_PM_QTY"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_Dep_BY = dr["REQ_Dep_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.PM_Option1 = dr["PM_Option1"].ToString();
                model.PM_Optiontext1 = dr["PM_Optiontext1"].ToString();
                model.PM_Option2 = dr["PM_Option2"].ToString();
                model.PM_Optiontext2 = dr["PM_Optiontext2"].ToString();
                model.PM_Option3 = dr["PM_Option3"].ToString();
                model.PM_Optiontext3 = dr["PM_Optiontext3"].ToString();
                model.PM_Option4 = dr["PM_Option4"].ToString();
                model.PM_Optiontext4 = dr["PM_Optiontext4"].ToString();
                model.PM_Option5 = dr["PM_Option5"].ToString();
                model.PM_Optiontext5 = dr["PM_Optiontext5"].ToString();
                model.BU_Head = dr["BU_Head"].ToString();
                model.BU_APPRV_DATE = dr["BU_APPRV_DATE"].ToString();
                model.BU_APPRV_STATUS = dr["BU_APPRV_STATUS"].ToString();
                model.BU_REMARK = dr["BU_REMARK"].ToString();
                model.CLM_BU_QTY = dr["CLM_BU_QTY"].ToString();
                model.CLM_BU_Ref = dr["CLM_BU_Ref"].ToString();

                model.MD_Head = dr["MD_Head"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_REMARK = dr["MD_REMARK"].ToString();


                model.PMCODE = dr["PMCODE"].ToString();
                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.UNITCOST = dr["UNITCOST"].ToString();
                model.totcost = dr["totcost"].ToString();
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult CheckUsrClmStaff(string Name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string User = Name;
            string UsrClmStaff = string.Empty;
            SqlCommand cmd = new SqlCommand("select * From UsrGrp where UsrID =N'" + User + "'", Connection);
            SqlDataReader rev = cmd.ExecuteReader();


            while (rev.Read())
            {

                UsrClmStaff = rev["UsrClmStaff"].ToString();
            }
            rev.Close();
            rev.Dispose();
            cmd.Dispose();
            Connection.Close();

            return Json(new { UsrClmStaff }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckUsrClmStaffTechnical(string Name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string User = Name;
            string company = Session["company"].ToString();
            string whCompany = string.Empty;
            if (company == "TAM")
            {
                whCompany = " AND company = '" + company + "'";
            }
            List<Pm> List = new List<Pm>();
            SqlCommand cmd = new SqlCommand("select * From UsrGrp where Department =N'" + User + "'" + whCompany, Connection);
            SqlDataReader rev = cmd.ExecuteReader();

            while (rev.Read())
            {

                List.Add(new Pm()
                {
                    PROD = rev["UsrID"].ToString(),
                    PRODNAM = rev["UsrID"].ToString()
                });
            }
            rev.Close();
            rev.Dispose();
            cmd.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathImageRT(string inCLM_ID, string CLM_NO)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<ImageFilesListDetail> Getdata = new List<ImageFilesListDetail>();
            ImageFiles model = null;
            // var root = @"\Warranty\ImgUpload\";
            var root = @"..\ImgUploadRT\";
            var command = new SqlCommand("P_GetPathImage_RT", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inCim_No", inCLM_ID);
            command.Parameters.AddWithValue("@inCim_NoSub", CLM_NO);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ImageFiles();
                model.IMAGE_ID = dr["IMAGE_ID"].ToString();
                model.REQ_NO = dr["STMP_ID"].ToString();
                model.CLM_NO_SUB = dr["STMP_ID_SUB"].ToString();
                model.IMAGE_NO = dr["IMAGE_NO"].ToString();
                model.IMAGE_NAME = dr["IMAGE_NAME"].ToString();
                //model.PATH = dr["PATH"].ToString();
                //  model.PATH = Server.MapPath(@"~\ImgUpload\" + dr["IMAGE_NAME"].ToString());
                model.PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString());
                //model.PATH = "D:\\Projects\\work spaces\\ClaimWap\\ClaimWap\\ImgUpload\\CM18110012-GDB7224YO-CM18110012-01-01.png";
                Getdata.Add(new ImageFilesListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetfileVideoRT(string inCLM_ID, string CLM_NO, string Im_No)
        {
            List<VideoFiles> videolist = new List<VideoFiles>();
            string Docnocm = string.Empty;
            var root = @"..\VideoFileUploadRT\";
            var CS = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(CS);
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand command = new SqlCommand("spGetAllVideoFile_RT", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@inCim_No", inCLM_ID);
                command.Parameters.AddWithValue("@inCim_NoSub", CLM_NO);
                command.Parameters.AddWithValue("@inImg_ID", Im_No);
                Connection.Open();
                con.Open();
                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    VideoFiles video = new VideoFiles();
                    video.ID = Convert.ToInt32(rdr["ID"]);
                    video.Name = rdr["Name"].ToString();
                    video.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    // video.FilePath = rdr["FilePath"].ToString();
                    video.FilePath = Path.Combine(root, rdr["Name"].ToString());
                    videolist.Add(video);
                }
                rdr.Close();
                rdr.Dispose();
                command.Dispose();
            }

            Connection.Close();
            return Json(new { videolist }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getsalesreturndata(string inslm, string inCOM, string inSTATS, string instatdate, string inenddate, string incusno, string initem, string indoc, string inuderlogin)
        {

            string message = string.Empty;
            Salesreturnsupper model = null;
            List<SalesreturnsupperListDetail> Getdata = new List<SalesreturnsupperListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessSalesReturn_Sup", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inslm", inslm);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@incusno", incusno);
            command.Parameters.AddWithValue("@initem", initem);
            command.Parameters.AddWithValue("@indoc", indoc);
            command.Parameters.AddWithValue("@inuserlogin", inuderlogin);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Salesreturnsupper();

                model.REQ_ID = dr["REQ_ID"].ToString();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.STMP_COMPANY = dr["STMP_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.STMP_REQQTY = dr["STMP_REQQTY"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_CAUSE = dr["STMP_CAUSE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.STMP_RCVDATE = dr["STMP_REQUESTDATE"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.STMP_LineAMT = dr["STMP_LineAMT"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.SMSUP_APPRV_STATUS = dr["SMSUP_APPRV_STATUS"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.PM_REMARK = dr["PM_REMARK"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_PROCESS_STATUS = dr["MD_PROCESS_STATUS"].ToString();
                model.MD_REMARK = dr["MD_REMARK"].ToString();
                model.STMP_STATUS = dr["STMP_STATUS"].ToString();
                Getdata.Add(new SalesreturnsupperListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }

        public JsonResult Getsalesreturndetail(string indoc, string indocsup)
        {

            string message = string.Empty;
            SalesreturnDetail model = null;
            List<SalesreturnDetailList> Getdata = new List<SalesreturnDetailList>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessSalesReturn_Detail", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", indoc);
            command.Parameters.AddWithValue("@DOCSUB", indocsup);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new SalesreturnDetail();

                model.CUSCOD = dr["CUSCOD"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.STMP_LastDocNo = dr["STMP_LastDocNo"].ToString();
                model.STMP_LastDocDate = dr["STMP_LastDocDate"].ToString();
                model.STMP_LineAMT = dr["STMP_LineAMT"].ToString();
                model.STMP_COMPANY = dr["STMP_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.GRPNAM = dr["GRPNAM"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                model.STMP_REQQTY = dr["STMP_REQQTY"].ToString();
                model.STMP_QTY = dr["STMP_QTY"].ToString();
                model.STMP_UOM = dr["STMP_UOM"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_CAUSE = dr["STMP_CAUSE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.STMP_REQUESTDATE = dr["STMP_REQUESTDATE"].ToString();
                model.STMP_DATE = dr["STMP_DATE"].ToString();
                model.STMP_ADMIN = dr["STMP_ADMIN"].ToString();
                model.STMP_ADMIN_REQQTY = dr["STMP_ADMIN_REQQTY"].ToString();
                model.STMP_ADMIN_REQ_DATE = dr["STMP_ADMIN_REQ_DATE"].ToString();
                model.SMSUP_CODE = dr["SMSUP_CODE"].ToString();
                model.SMSUP_APPRV_DATE = dr["SMSUP_APPRV_DATE"].ToString();
                model.SMSUP_APPRV_STATUS = dr["SMSUP_APPRV_STATUS"].ToString();
                model.SMSUP_REMARK = dr["SMSUP_REMARK"].ToString();
                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.PM_REMARK = dr["PM_REMARK"].ToString();
                model.MD_NAME = dr["MD_NAME"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_PROCESS_STATUS = dr["MD_PROCESS_STATUS"].ToString();
                model.MD_REMARK = dr["MD_REMARK"].ToString();
                model.STMP_STATUS = dr["STMP_STATUS"].ToString();

                Getdata.Add(new SalesreturnDetailList { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimdataPmRT(string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Salesreturnsupper model = null;
            List<SalesreturnsupperListDetail> Getdata = new List<SalesreturnsupperListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessReturn_Pm", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Salesreturnsupper();

                model.REQ_ID = dr["REQ_ID"].ToString();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.STMP_COMPANY = dr["STMP_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.STMP_REQQTY = dr["STMP_REQQTY"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_CAUSE = dr["STMP_CAUSE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();;
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.STMP_RCVDATE = dr["STMP_REQUESTDATE"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.STMP_LineAMT = dr["STMP_LineAMT"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.SMSUP_APPRV_STATUS = dr["SMSUP_APPRV_STATUS"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_PROCESS_STATUS = dr["MD_PROCESS_STATUS"].ToString();
                model.SMSUP_APPRV_DATE = dr["SMSUP_APPRV_DATE"].ToString();
                model.STMP_STATUS = dr["STMP_STATUS"].ToString();
                model.Statisallbill = dr["Statusallbill"].ToString();
                Getdata.Add(new SalesreturnsupperListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClimdataMdRT(string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem, string inUserLogin)
        {
            string message = string.Empty;
            SalesreturnMD model = null;
            List<SalesreturnsupperListDetailMD> Getdata = new List<SalesreturnsupperListDetailMD>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessReturn_Md", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
            command.Parameters.AddWithValue("@inUserLogin", inUserLogin);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new SalesreturnMD();
                model.REQ_ID = dr["REQ_ID"].ToString();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.STMP_COMPANY = dr["STMP_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.STMP_REQQTY = dr["STMP_REQQTY"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_CAUSE = dr["STMP_CAUSE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.STMP_RCVDATE = dr["STMP_REQUESTDATE"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.STMP_LineAMT = dr["STMP_LineAMT"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.SMSUP_APPRV_STATUS = dr["SMSUP_APPRV_STATUS"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_PROCESS_STATUS = dr["MD_PROCESS_STATUS"].ToString();
                model.MD_REMARK = dr["MD_REMARK"].ToString();
                model.SMSUP_APPRV_DATE = dr["SMSUP_APPRV_DATE"].ToString();
                model.STMP_STATUS = dr["STMP_STATUS"].ToString();
                model.Statisallbill = dr["Statusallbill"].ToString();
                Getdata.Add(new SalesreturnsupperListDetailMD { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetfileVideoWH(string inCLM_ID, string CLM_NO, string Im_No)
        {
            List<VideoFiles> videolist = new List<VideoFiles>();
            string Docnocm = string.Empty;
            var root = @"..\VideoFileUploadWH\";
            var CS = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(CS);
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand command = new SqlCommand("spGetAllVideoFile_WH", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@inCim_No", inCLM_ID);
                command.Parameters.AddWithValue("@inCim_NoSub", CLM_NO);
                command.Parameters.AddWithValue("@inImg_ID", Im_No);
                Connection.Open();
                con.Open();
                SqlDataReader rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    VideoFiles video = new VideoFiles();
                    video.ID = Convert.ToInt32(rdr["ID"]);
                    video.Name = rdr["Name"].ToString();
                    video.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    // video.FilePath = rdr["FilePath"].ToString();
                    video.FilePath = Path.Combine(root, rdr["Name"].ToString());
                    videolist.Add(video);
                }
                rdr.Close();
                rdr.Dispose();
                command.Dispose();
            }

            Connection.Close();
            return Json(new { videolist }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathImageWH(string inCLM_ID, string CLM_NO)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<ImageFilesListDetail> Getdata = new List<ImageFilesListDetail>();
            ImageFiles model = null;
            // var root = @"\Warranty\ImgUpload\";
            var root = @"..\ImgUploadWH\";
            var command = new SqlCommand("P_GetPathImageWH", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inCim_No", inCLM_ID);
            command.Parameters.AddWithValue("@inCim_NoSub", CLM_NO);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ImageFiles();
                model.IMAGE_ID = dr["IMAGE_ID"].ToString();
                model.REQ_NO = dr["REQ_NO"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.IMAGE_NO = dr["IMAGE_NO"].ToString();
                model.IMAGE_NAME = dr["IMAGE_NAME"].ToString();
                //model.PATH = dr["PATH"].ToString();
                //  model.PATH = Server.MapPath(@"~\ImgUpload\" + dr["IMAGE_NAME"].ToString());
                model.PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString());
                //model.PATH = "D:\\Projects\\work spaces\\ClaimWap\\ClaimWap\\ImgUpload\\CM18110012-GDB7224YO-CM18110012-01-01.png";
                Getdata.Add(new ImageFilesListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Getdatastock(string stck, string com)
        {
            Stock model = null;
            List<ListStock> Getdata = new List<ListStock>();

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;

            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            SqlCommand command = new SqlCommand("select * From v_WMS_Stock where client='" + com + "' and item_no=N'" + stck + "' ", Connection);



            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Stock();
                model.client = dr["client"].ToString();
                model.nav_bin = dr["nav_bin"].ToString();
                model.item_no = dr["item_no"].ToString();
                model.Qty_avail = dr["Qty_avail"].ToString();
                model.RevQty = dr["RevQty"].ToString();
                model.WaitingPackQty = dr["Waiting Pack Qty"].ToString();

                Getdata.Add(new ListStock { val = model });
            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }
    }
}
