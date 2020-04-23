using Module06_BO;
using System.Collections.Generic;
using System.ComponentModel;

namespace BO
{
    public class Samourai : AbstractClasse
    {
        public int Force { get; set; }
        public string Nom { get; set; }
        public virtual Arme Arme { get; set; }

        [DisplayName("Arts martiaux maitrisés")]
        public virtual List<ArtMartial> ArtMartials { get; set; } = new List<ArtMartial>();

        private int potentiel;

        public int Potentiel
        {
            get
            {
                int degats = 0;

                if (Arme != null)
                {
                    degats = Arme.Degats;
                }

                potentiel = (Force + degats) * (ArtMartials.Count + 1);
                return potentiel;
            }
        }
    }
}
