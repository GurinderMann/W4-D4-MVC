using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Prodotti
    {
        public int Id { get; set; } 
        public string Nome { get; set; }    
        public decimal Prezzo   { get; set; }
        public string Descrizione { get; set; }
        public string ImgCopertina { get; set; }
    
        public string ImgExtra { get; set; }
        public string ImgAggiuntiva { get; set; }

       public bool Disponibile { get; set; }
        
        public static List <Prodotti> prodottiList { get; set; } = new List <Prodotti> ();

    }
}