using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projet_psi
{
    internal class Huffman
    {
        public Noeud root { get; set;}
        public Dictionary<Pixel, int> fréquences = new Dictionary<Pixel, int>();
        public List<Noeud> feuilles = new List<Noeud>();

        public void Algo_Huffman()
        {
            foreach (KeyValuePair<Pixel, int> symbole in fréquences)
            {
                feuilles.Add(new Noeud(symbole.Key, symbole.Value));
            }
            while (feuilles.Count > 1)
            {
                List<Noeud> feuillestriees = feuilles.OrderBy(noeud => noeud.frequence).ToList<Noeud>();
                if (feuillestriees.Count >= 2)
                {
                    List<Noeud> deuxpremiers = feuillestriees.Take(2).ToList<Noeud>();
                    Noeud parent = new Noeud(deuxpremiers[0], deuxpremiers[1]);
                    feuilles.Remove(deuxpremiers[0]);
                    feuilles.Remove(deuxpremiers[1]);
                    feuilles.Add(parent);
                }
                this.root = feuilles.FirstOrDefault();
            }

        }



    }
}
