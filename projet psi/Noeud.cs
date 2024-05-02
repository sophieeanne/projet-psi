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
        //constructeur de la classe Noeud
        public Noeud(int frequence, Noeud gauche, Noeud droit)
        {
            this.pixel = null;
            this.frequence = frequence;
            this.gauche = gauche;
            this.droit = droit;
        }
        //acces aux attributs   
        public Pixel Pixel
        {
            get { return pixel; }
            set { pixel = value; }
        }
        public int Frequence
        {
            get { return frequence; }
            set { frequence = value; }
        }
        public Noeud Gauche
        {
            get { return gauche; }
            set { gauche = value; }
        }
        public Noeud Droit
        {
            get { return droit; }
            set { droit = value; }
        }



    }
}
