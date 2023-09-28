using Shop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    [Authorize(Users = "Admin")]
    public class AdminController : Controller
    {
       
        List<Prodotti> prodottiList = new List<Prodotti>();
        // GET: Admin
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ShoesDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM T_Prodotti", conn);
                SqlDataReader sqlData;
                sqlData = cmd.ExecuteReader();

                while (sqlData.Read())
                {
                    int id = Convert.ToInt16(sqlData["IDProdotto"].ToString());
                    string nome = sqlData["Nome"].ToString();
                    decimal prezzo = Convert.ToDecimal(sqlData["Prezzo"]);
                    string descrizione = sqlData["Descrizione"].ToString();


                    // Crea un nuovo oggetto Prodotti e aggiungilo alla lista
                    Prodotti prodotto = new Prodotti
                    {
                        Id = id,
                        Nome = nome,
                        Prezzo = prezzo,
                        Descrizione = descrizione,


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



        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Prodotti p, HttpPostedFileBase ImgCopertina, HttpPostedFileBase ImgExtra, HttpPostedFileBase ImgAggiuntiva)
        {
            if (ModelState.IsValid)
            {
                if (ImgCopertina.ContentLength > 0 && ImgExtra.ContentLength > 0 && ImgCopertina.ContentLength > 0)
                {
                    string NameFile = Path.GetFileName(ImgCopertina.FileName);
                    string SavePath = Path.Combine(Server.MapPath("~/Content/img"), NameFile);
                    ImgCopertina.SaveAs(SavePath);
                    p.ImgCopertina = ImgCopertina.FileName;

                    string ExtraFile = Path.GetFileName(ImgExtra.FileName);
                    string ExtraPath = Path.Combine(Server.MapPath("~/Content/img"), ExtraFile);
                    ImgExtra.SaveAs(ExtraPath);
                    p.ImgExtra = ImgExtra.FileName;

                    string Aggiuntiva = Path.GetFileName(ImgAggiuntiva.FileName);
                    string AggiuntivaPath = Path.Combine(Server.MapPath("~/Content/img"), Aggiuntiva);
                    ImgAggiuntiva.SaveAs(AggiuntivaPath);
                    p.ImgAggiuntiva = ImgAggiuntiva.FileName;


                    string connectionString = ConfigurationManager.ConnectionStrings["ShoesDB"].ConnectionString.ToString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    try
                    {
                        conn.Open();


                        string insertQuery = "INSERT INTO T_Prodotti (Nome, Prezzo, Descrizione, Disponibile, ImgCopertina, ImgExtra, ImgAggiuntiva) VALUES (@Nome, @Prezzo, @Descrizione, @Disponibile, @ImgCopertina, @ImgExtra, @ImgAggiuntiva )";

                        SqlCommand cmd = new SqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@Nome", p.Nome);
                        cmd.Parameters.AddWithValue("@Prezzo", p.Prezzo);
                        cmd.Parameters.AddWithValue("@Descrizione", p.Descrizione);
                        cmd.Parameters.AddWithValue("@Disponibile", p.Disponibile);
                        cmd.Parameters.AddWithValue("@ImgCopertina", p.ImgCopertina);
                        cmd.Parameters.AddWithValue("@ImgExtra", p.ImgExtra);
                        cmd.Parameters.AddWithValue("@ImgAggiuntiva", p.ImgAggiuntiva);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ViewBag.MessaggioConferma = "Prodotto creato con successo";
                        }
                        else
                        {
                            ViewBag.MessaggioConferma = "Nessun prodotto creato";
                        }
                    }
                    catch (Exception ex)
                    {

                        ViewBag.Errore = "Si è verificato un errore durante la creazione del prodotto: " + ex.Message;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }



        public ActionResult Edit(int id)
        {
            Prodotti prodotto = new Prodotti();
            foreach (Prodotti p in Prodotti.prodottiList)
            {
                if (p.Id == id)
                {
                    prodotto = p;
                    break;
                }
            }
            return View(prodotto);
        }

        [HttpPost]
        public ActionResult Edit(Prodotti p, HttpPostedFileBase ImgCopertina, HttpPostedFileBase ImgExtra, HttpPostedFileBase ImgAggiuntiva)
        {
            if (ModelState.IsValid)
            {

                if (ImgCopertina.ContentLength > 0 && ImgExtra.ContentLength > 0 && ImgCopertina.ContentLength > 0)
                {
                    string NameFile = Path.GetFileName(ImgCopertina.FileName);
                    string SavePath = Path.Combine(Server.MapPath("~/Content/img"), NameFile);
                    ImgCopertina.SaveAs(SavePath);
                    p.ImgCopertina = ImgCopertina.FileName;

                    string ExtraFile = Path.GetFileName(ImgExtra.FileName);
                    string ExtraPath = Path.Combine(Server.MapPath("~/Content/img"), ExtraFile);
                    ImgExtra.SaveAs(ExtraPath);
                    p.ImgExtra = ImgExtra.FileName;

                    string Aggiuntiva = Path.GetFileName(ImgAggiuntiva.FileName);
                    string AggiuntivaPath = Path.Combine(Server.MapPath("~/Content/img"), Aggiuntiva);
                    ImgAggiuntiva.SaveAs(AggiuntivaPath);
                    p.ImgAggiuntiva = ImgAggiuntiva.FileName;


                    string connectionString = ConfigurationManager.ConnectionStrings["ShoesDB"].ConnectionString.ToString();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string updateQuery = "UPDATE T_Prodotti SET Nome = @Nome, Prezzo = @Prezzo, ImgCopertina = @ImgCopertina, ImgExtra = @ImgExtra, ImgAggiuntiva = @ImgAggiuntiva, Disponibile = @Disponibile WHERE IDProdotto = @ID";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Nome", p.Nome); // Aggiungi il parametro @Nome
                            cmd.Parameters.AddWithValue("@Prezzo", p.Prezzo);
                            cmd.Parameters.AddWithValue("@ID", p.Id);
                            cmd.Parameters.AddWithValue("@Disponibile", p.Disponibile);
                            cmd.Parameters.AddWithValue("@ImgCopertina", p.ImgCopertina);
                            cmd.Parameters.AddWithValue("@ImgExtra", p.ImgExtra);
                            cmd.Parameters.AddWithValue("@ImgAggiuntiva", p.ImgAggiuntiva);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                ViewBag.MessaggioConferma = "Salvataggio effettuato";
                            }
                            else
                            {
                                ViewBag.MessaggioConferma = "Nessuna modifica effettuata";
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            Prodotti prodotto = new Prodotti();
            foreach (Prodotti p in Prodotti.prodottiList)
            {
                if (p.Id == id)
                {
                    prodotto = p;
                    break;
                }
            }
            return View(prodotto);
        }

        [HttpPost]
        public ActionResult Delete(Prodotti p)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ShoesDB"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string updateQuery = "DELETE FROM T_Prodotti WHERE IDProdotto = @ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {

                    cmd.Parameters.AddWithValue("@ID", p.Id);

                    cmd.ExecuteNonQuery();

                }
            }

            return RedirectToAction("Index");
        }
    }
}