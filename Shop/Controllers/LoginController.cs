using Shop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Shop.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(User user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ShoesDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM T_Users", conn);
                SqlDataReader sqlData;
                sqlData = cmd.ExecuteReader();
              

                while (sqlData.Read())
                {
                    if (user.Username == sqlData["UserName"].ToString() && user.Password == sqlData["Password"].ToString()) 
                    {
                        FormsAuthentication.SetAuthCookie(user.Username, false);
                        return RedirectToAction("Index", "Shop");

                    }
                    else
                    {
                        return View();
                    }
                }
                return View();

            }
            catch (Exception ex)
            {

                Response.Write(ex.ToString());
                return View();

            }finally { conn.Close(); }
            

         
        }
    }
}