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
        }

        public Noeud(Noeud noeud1, Noeud noeud2)
        {
            if(noeud1.frequence < noeud2.frequence)
            {
                gauche = noeud1;
                droit = noeud2;
            }
            else
            {
                gauche = noeud2;
                droit = noeud1;
            }
            frequence = noeud1.frequence + noeud2.frequence;
        }



    }
}
