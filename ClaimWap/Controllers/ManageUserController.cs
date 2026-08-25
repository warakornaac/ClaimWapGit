using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;

namespace ClaimWap.Controllers
{
    public class ManageUserController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;

        // GET: /ManageUser/
        public ActionResult Index()
        {
            List<UsrGrp> userList = new List<UsrGrp>();

            try
            {
                SqlConnection Connection = new SqlConnection(connectionString);
                using (Connection)
                {
                    string query = @"SELECT [ID], [company], [UsrID], [Department], [Email], [SLMCOD], 
                                            [UsrTyp], [ISApprover], [UsrClmStaff], [Usermail]
                                    FROM [dbo].[UsrGrp]";

                    using (SqlCommand cmd = new SqlCommand(query, Connection))
                    {
                        Connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UsrGrp user = new UsrGrp
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    company = reader.IsDBNull(reader.GetOrdinal("company")) ? null : reader.GetString(reader.GetOrdinal("company")),
                                    UsrID = reader.IsDBNull(reader.GetOrdinal("UsrID")) ? null : reader.GetString(reader.GetOrdinal("UsrID")),
                                    Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString(reader.GetOrdinal("Department")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                    SLMCOD = reader.IsDBNull(reader.GetOrdinal("SLMCOD")) ? null : reader.GetString(reader.GetOrdinal("SLMCOD")),
                                    UsrTyp = reader.IsDBNull(reader.GetOrdinal("UsrTyp")) ? 0 : reader.GetInt32(reader.GetOrdinal("UsrTyp")),
                                    ISApprover = reader.IsDBNull(reader.GetOrdinal("ISApprover")) ? 0 : reader.GetInt32(reader.GetOrdinal("ISApprover")),
                                    UsrClmStaff = reader.IsDBNull(reader.GetOrdinal("UsrClmStaff")) ? 0 : reader.GetInt32(reader.GetOrdinal("UsrClmStaff")),
                                    Usermail = reader.IsDBNull(reader.GetOrdinal("Usermail")) ? 0 : reader.GetInt32(reader.GetOrdinal("Usermail"))
                                };

                                userList.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong: " + ex.Message;
            }

            return View(userList);
        }

        // GET: GetUserList for DataTables/jsGrid
        public JsonResult GetUserList()
        {
            List<UsrGrp> userList = new List<UsrGrp>();

            try
            {
                SqlConnection Connection = new SqlConnection(connectionString);
                using (Connection)
                {
                    string query = @"SELECT [ID], [company], [UsrID], [Department], [Email], [SLMCOD], 
                                            [UsrTyp], [ISApprover], [UsrClmStaff], [Usermail]
                                    FROM [dbo].[UsrGrp]";

                    using (SqlCommand cmd = new SqlCommand(query, Connection))
                    {
                        Connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UsrGrp user = new UsrGrp
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    company = reader.IsDBNull(reader.GetOrdinal("company")) ? null : reader.GetString(reader.GetOrdinal("company")),
                                    UsrID = reader.IsDBNull(reader.GetOrdinal("UsrID")) ? null : reader.GetString(reader.GetOrdinal("UsrID")),
                                    Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString(reader.GetOrdinal("Department")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                    SLMCOD = reader.IsDBNull(reader.GetOrdinal("SLMCOD")) ? null : reader.GetString(reader.GetOrdinal("SLMCOD")),
                                    UsrTyp = reader.IsDBNull(reader.GetOrdinal("UsrTyp")) ? 0 : reader.GetInt32(reader.GetOrdinal("UsrTyp")),
                                    ISApprover = reader.IsDBNull(reader.GetOrdinal("ISApprover")) ? 0 : reader.GetInt32(reader.GetOrdinal("ISApprover")),
                                    UsrClmStaff = reader.IsDBNull(reader.GetOrdinal("UsrClmStaff")) ? 0 : reader.GetInt32(reader.GetOrdinal("UsrClmStaff")),
                                    Usermail = reader.IsDBNull(reader.GetOrdinal("Usermail")) ? 0 : reader.GetInt32(reader.GetOrdinal("Usermail"))
                                };

                                userList.Add(user);
                            }
                        }
                    }
                }

                return Json(new { success = true, data = userList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(UsrGrp user)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("P_AddNewUserAD", Connection))
                    {
                        // กำหนดประเภทคำสั่งให้เป็น Stored Procedure
                        cmd.CommandType = CommandType.StoredProcedure;

                        // กำหนด Input Parameters ที่ SP ต้องการ
                        cmd.Parameters.AddWithValue("@inUser", (object)user.UsrID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@instatus", (object)user.UsrTyp ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ISApprover", (object)user.ISApprover ?? DBNull.Value);

                        // กำหนด Output Parameter สำหรับรับค่ากลับจาก SP
                        SqlParameter outGenStatusParam = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outGenStatusParam);

                        Connection.Open();
                        cmd.ExecuteNonQuery();

                        // ดึงค่าที่ได้จาก Output Parameter
                        string statusMessage = outGenStatusParam.Value != DBNull.Value
                        ? outGenStatusParam.Value.ToString()
                        : "";

                        // ตรวจสอบผลลัพธ์ด้วยคำว่า "Success" แทน
                        if (statusMessage == "Success")
                        {
                            return Json(new { success = true, message = "Added successfully." });
                        }
                        else
                        {
                            // กรณีนี้จะครอบคลุมทั้ง 'Already exists', 'Not UserAD' หรือ Error Message จาก Catch
                            return Json(new { success = false, message = statusMessage });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // GET: Get user by ID
        [HttpGet]
        public JsonResult GetUserById(int id)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT [ID], [company], [UsrID], [Department], [Email], [SLMCOD], 
                                    [UsrTyp], [ISApprover], [UsrClmStaff], [Usermail]
                            FROM [dbo].[UsrGrp]
                            WHERE [ID] = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, Connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        Connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UsrGrp user = new UsrGrp
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    company = reader["company"] as string,
                                    UsrID = reader["UsrID"] as string,
                                    Department = reader["Department"] as string,
                                    Email = reader["Email"] as string,
                                    SLMCOD = reader["SLMCOD"] as string,
                                    UsrTyp = reader["UsrTyp"] != DBNull.Value ? Convert.ToInt32(reader["UsrTyp"]) : 0,
                                    ISApprover = reader["ISApprover"] != DBNull.Value ? Convert.ToInt32(reader["ISApprover"]) : 0,
                                    UsrClmStaff = reader["UsrClmStaff"] != DBNull.Value ? Convert.ToInt32(reader["UsrClmStaff"]) : 0,
                                    Usermail = reader["Usermail"] != DBNull.Value ? Convert.ToInt32(reader["Usermail"]) : 0
                                };

                                return Json(new { success = true, data = user }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }

                return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Update user
        [HttpPost]
        public JsonResult Update(UsrGrp user)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("P_UpdateUserAD", Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // ส่ง UsrID เป็นตัวอ้างอิง และ UsrTyp เป็นค่าที่จะเปลี่ยน
                        cmd.Parameters.AddWithValue("@inUser", user.UsrID);
                        cmd.Parameters.AddWithValue("@instatus", user.UsrTyp);
                        cmd.Parameters.AddWithValue("@SLMCOD", (object)user.SLMCOD ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ISApprover", user.ISApprover);
                        cmd.Parameters.AddWithValue("@UsrClmStaff", user.UsrClmStaff);
                        //cmd.Parameters.AddWithValue("@EmpID", user.EmpID);
                        //cmd.Parameters.AddWithValue("@company", user.company);
                        //cmd.Parameters.AddWithValue("@Department", user.Department);
                        //cmd.Parameters.AddWithValue("@EMail", user.EMail);

                        SqlParameter outParam = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100)
                        { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(outParam);

                        Connection.Open();
                        cmd.ExecuteNonQuery();

                        string result = outParam.Value.ToString();
                        if (result == "Update Success")
                        {
                            return Json(new { success = true, message = "Updated successfully." });
                        }
                        return Json(new { success = false, message = result });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Delete user
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                SqlConnection Connection = new SqlConnection(connectionString);
                using (Connection)
                {
                    string query = @"DELETE FROM [dbo].[UsrGrp] WHERE [ID] = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, Connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        Connection.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            return Json(new { success = true, message = "Deleted successfully." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Deleted failed." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
