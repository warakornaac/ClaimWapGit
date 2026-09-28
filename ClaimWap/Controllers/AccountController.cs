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
using System.Security.Principal;
using System.Web.Security;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Globalization;

namespace ClaimWap.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LogIn()
        {

            return View();

        }
        [HttpGet]
        public ActionResult ChangePassword()
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



            }

            return View();

        }

        [HttpPost]
        public ActionResult LogIn(ClaimWap.Models.LoginUserViewModel User)
        {
            string Userlog = string.Empty;
            string Usertype = string.Empty;
            string dateexpire = string.Empty;
            int intdateexpire = 0;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                //ADSRV01
                DirectoryEntry entry = new DirectoryEntry("LDAP://ADSRV2016-01/dc=Automotive,dc=com", User.Usre, User.Password);
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + User.Usre + ")";
                search.PropertiesToLoad.Add("cn");

                SearchResult result = search.FindOne();
                Connection.Open();

                if (null == result)
                {
                    if (IsValid(User.Usre, User.Password))
                    {

                        this.Session["UsrGrpspecial"] = 0;
                        //login ad pass
                        InsertLoginLog(Connection, User.Usre, usrTyp: null, flag: "internal", loginStatus: "SUCCESS");
                        FormsAuthentication.SetAuthCookie(User.Usre, false);
                        return RedirectToAction("LogIn", "Account");
                    }
                    else
                    {
                        //login ad fail
                        ModelState.AddModelError("", "Login details are wrong.");
                        InsertLoginLog(Connection, User.Usre, usrTyp: null, flag: "internal", loginStatus: "FAIL", failReason: "IsValid check failed");
                    }
                }
                else
                {
                    this.Session["UserID"] = User.Usre;
                    this.Session["UserPassword"] = User.Password;
                    this.Session["UsrGrpspecial"] = 0;
                    this.Session["company"] = null;

                    SqlCommand cmd = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User.Usre + "'", Connection);
                    SqlDataReader rev = cmd.ExecuteReader();
                    while (rev.Read())
                    {

                        dateexpire = rev["Date to Expire"].ToString();
                        this.Session["UserType"] = rev["UsrTyp"].ToString();
                        this.Session["ISApprover"] = rev["ISApprover"].ToString();
                        this.Session["Department"] = rev["Department"].ToString();
                        this.Session["company"] = rev["company"].ToString();
                    }
                    rev.Close();
                    rev.Dispose();
                    cmd.Dispose();
                    //login ad pass
                    InsertLoginLog(Connection, User.Usre, this.Session["UserType"]?.ToString(), flag: "internal", loginStatus: "SUCCESS");
                    if (!string.IsNullOrEmpty(User.Usre))
                    {
                        using (SqlCommand cmdAD = new SqlCommand("SELECT Department FROM v_ADUser WHERE LogInName = @uid", Connection))
                        {
                            cmdAD.Parameters.AddWithValue("@uid", User.Usre);
                            object adDept = cmdAD.ExecuteScalar();
                            if (adDept != null && adDept != DBNull.Value)
                            {
                                this.Session["Department"] = adDept.ToString();
                            }
                        }
                    }
                    // Safely parse the database value. Avoid Convert.ToInt32 which throws on bad format.
                    int parsedDays;
                    if (int.TryParse(dateexpire, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedDays))
                    {
                        intdateexpire = parsedDays;

                        // Handle the special case where password must be changed (0) first
                        if (intdateexpire == 0)
                        {
                            this.Session["DatetoExpire"] = "The user's password must be changed password  Changed password on Citrix";
                        }
                        else if (intdateexpire <= 15)
                        {
                            this.Session["DatetoExpire"] = "Passwords expire '" + intdateexpire + "' days";
                        }
                        else
                        {
                            this.Session["DatetoExpire"] = "..";
                        }
                    }
                    else
                    {
                        // Non-numeric or null/empty value — set a safe default display
                        this.Session["DatetoExpire"] = "..";
                    }

                    FormsAuthentication.SetAuthCookie(User.Usre, false);


                    return RedirectToAction("Index", "SelectDisplay");

                }
                Connection.Close();
            }
            catch (COMException ex)
            {
                string type = string.Empty;
                int contyp = 0;
                Connection.Open();
                this.Session["UserID"] = User.Usre;
                this.Session["UserPassword"] = User.Password;
                SqlCommand cmd = new SqlCommand("select * From UsrGrp_special where UsrID = @inUser and [dbo].F_decrypt([Password])= @inPassword and  [LoginFail] <> 3", Connection);
                cmd.Parameters.AddWithValue("@inUser", User.Usre);
                cmd.Parameters.AddWithValue("@inPassword", User.Password);
                SqlDataReader rev = cmd.ExecuteReader();
                while (rev.Read())
                {
                    this.Session["UsrCode"] = rev["SLMCOD"].ToString();
                    this.Session["UsrGrpspecial"] = 1;
                    this.Session["UserType"] = rev["UsrTyp"].ToString();
                    this.Session["Contact"] = rev["Contact"].ToString();
                    this.Session["ContactPhone"] = rev["ContactPhone"].ToString();
                    this.Session["ISApprover"] = rev["ISApprover"].ToString();
                    type = rev["UsrTyp"].ToString();
                    this.Session["Department"] = rev["Department"].ToString();
                    this.Session["company"] = rev["company"].ToString();

                }
                rev.Close();
                rev.Dispose();
                cmd.Dispose();
                if (type != "")
                {
                    contyp = Convert.ToInt32(type);
                    //user special pass
                    InsertLoginLog(Connection, User.Usre, type, flag: "special", loginStatus: "SUCCESS");

                    if (contyp != 7)
                    {
                        return RedirectToAction("Index", "SelectDisplay");
                    }
                    else
                    {

                        string message = string.Empty;
                        var cmdup = new SqlCommand("P_logSingin", Connection);
                        cmdup.CommandType = CommandType.StoredProcedure;
                        cmdup.Parameters.AddWithValue("@UsrID", User.Usre);
                        cmdup.Parameters.AddWithValue("@Password", User.Password);
                        SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                        returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                        cmdup.Parameters.Add(returnValuedoc);
                        cmdup.ExecuteNonQuery();
                        message = returnValuedoc.Value.ToString();

                        cmdup.Dispose();

                        return RedirectToAction("Index", "CheckstatusCustomer");
                    }
                }
                else
                {
                    string message = string.Empty;
                    var cmdup = new SqlCommand("P_CountLoginFail", Connection);
                    cmdup.CommandType = CommandType.StoredProcedure;
                    cmdup.Parameters.AddWithValue("@UsrID", User.Usre);
                    SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                    returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                    cmdup.Parameters.Add(returnValuedoc);
                    cmdup.ExecuteNonQuery();
                    message = returnValuedoc.Value.ToString();
                    //user special fail
                    string failReason = (message == "true") ? "Incorrect password" : "Password locked";
                    InsertLoginLog(Connection, User.Usre, usrTyp: null, flag: "special", loginStatus: "FAIL", failReason: failReason);

                    if (message == "true")
                    {
                        ModelState.AddModelError("", "Login details are wrong.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password lock, please contact Admin Technical");
                    }
                }
                Connection.Close();
            }


            return View();

        }
        //fn keep loglogin
        private void InsertLoginLog(SqlConnection connection, string usrId, string usrTyp, string flag, string loginStatus, string failReason = null) {
            try {
                using (SqlCommand cmdLog = new SqlCommand("P_InsertUsrGrpLoginLog", connection)) {
                    cmdLog.CommandType = CommandType.StoredProcedure;
                    cmdLog.Parameters.AddWithValue("@UsrID", (object)usrId ?? DBNull.Value);
                    cmdLog.Parameters.AddWithValue("@UsrTyp", (object)usrTyp ?? DBNull.Value);
                    cmdLog.Parameters.AddWithValue("@Flag", (object)flag ?? DBNull.Value);
                    cmdLog.Parameters.AddWithValue("@IPAddress", (object)Request.UserHostAddress ?? DBNull.Value);

                    string userAgent = Request.UserAgent;
                    if (!string.IsNullOrEmpty(userAgent) && userAgent.Length > 500)
                        userAgent = userAgent.Substring(0, 500);
                    cmdLog.Parameters.AddWithValue("@UserAgent", (object)userAgent ?? DBNull.Value);

                    cmdLog.Parameters.AddWithValue("@LoginStatus", (object)loginStatus ?? "FAIL");
                    cmdLog.Parameters.AddWithValue("@FailReason", (object)failReason ?? DBNull.Value);

                    string systemName = "Warranty";
                    cmdLog.Parameters.AddWithValue("@SystemName", systemName);

                    cmdLog.ExecuteNonQuery();
                }
            }
            catch {
                // silent-fail: ไม่ให้ error จากการ log กระทบ flow login หลัก
            }
        }

        public JsonResult Checkpassword(string textold, string UsrID)
        {
            string message = string.Empty;
            string no = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_checkpassword", Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@oldpassword", textold);
                command.Parameters.AddWithValue("@UsrID", UsrID);
                SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);



                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
                // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Forgotpassword(string mlie)
        {
            string message = string.Empty;
            string no = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_requestpassword", Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@mail", mlie);
                SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);



                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
                // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Changepassword(string textold, string UsrID)
        {
            string message = string.Empty;
            string no = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_Changepassword", Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@oldpassword", textold);
                command.Parameters.AddWithValue("@UsrID", UsrID);
                SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);



                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
                // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        private bool IsValid(string user, string Password)
        {

            bool IsValid = false;
            if (user == null || Password == null) { IsValid = false; }
            else
            {


            }
            return IsValid;
        }



    }
}
