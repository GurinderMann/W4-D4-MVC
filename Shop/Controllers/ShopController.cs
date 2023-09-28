using Shop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ShoesDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Prodotti> prodottiList = new List<Prodotti>();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM T_Prodotti WHERE Disponibile = 1   ", conn);
                SqlDataReader sqlData;
                sqlData = cmd.ExecuteReader();

                while (sqlData.Read())
                {
                    int id = Convert.ToInt16(sqlData["IDProdotto"].ToString());
                    string nome = sqlData["Nome"].ToString();
                    decimal prezzo = Convert.ToDecimal(sqlData["Prezzo"]);
                    string descrizione = sqlData["Descrizione"].ToString();
                    bool dispoinibile = Convert.ToBoolean(sqlData["Disponibile"].ToString());
                    string copertina = sqlData["ImgCopertina"].ToString();

                    // Crea un nuovo oggetto Prodotti e aggiungilo alla lista
                    Prodotti prodotto = new Prodotti
                    {
                        Id = id,
                        Nome = nome,
                        Prezzo = prezzo,
                        Descrizione = descrizione,
                        ImgCopertina = copertina,
                        Disponibile = dispoinibile


                    };

                    prodottiList.Add(prodotto);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            finally { conn.Close(); }

            Prodotti.prodottiList = prodottiList;

            return View(Prodotti.prodottiList);
        }

        public ActionResult Details(int id)
        {
            Prodotti prodotto = null;
         

            string connectionString = ConfigurationManager.ConnectionStrings["ShoesDB"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
              
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM T_Prodotti WHERE IDProdotto = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader sqlData = cmd.ExecuteReader();

                if (sqlData.Read())
                {
                 
                    string nome = sqlData["Nome"].ToString();
                    decimal prezzo = Convert.ToDecimal(sqlData["Prezzo"]);
                    string descrizione = sqlData["Descrizione"].ToString();
                    string copertina = sqlData["ImgCopertina"].ToString();
                    string extra = sqlData["ImgExtra"].ToString();
                    string aggiuntiva = sqlData["ImgAggiuntiva"].ToString();

                    prodotto = new Prodotti
                    {
                    
                        Nome = nome,
                        Prezzo = prezzo,
                        Descrizione = descrizione,
                        ImgCopertina = copertina,
                        ImgExtra = extra,
                        ImgAggiuntiva = aggiuntiva
                       
                    };
                }

                sqlData.Close();
            }

          
            return View(prodotto);
        }

    }
}