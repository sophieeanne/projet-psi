using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projet_psi
{
    internal class Noeud
    {
        public Pixel pixel;
        public int frequence;
        public Noeud gauche;
        public Noeud droit;

        public Noeud(Pixel pixel, int frequence)
        {
            this.pixel = pixel;
            this.frequence = frequence;
            this.gauche = null;
            this.droit = null;
        }



    }
}
