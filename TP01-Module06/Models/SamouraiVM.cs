using BO;
using Module06_BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP01_Module06.Models
{
    public class SamouraiVM
    {
        public Samourai Samourai { get; set; }
        public List<SelectListItem> Armes { get; set; } 
        public int? IdSelectedArme { get; set; }

        [DisplayName("Arts martiaux maitrisés")]
        public List<SelectListItem> ArtMartials { get; set; } = new List<SelectListItem>();

        public List<int> IdsArtMartial { get; set; } = new List<int>();

    }
}