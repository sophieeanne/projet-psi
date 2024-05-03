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
        /// <summary>
        /// Méthode qui implémente l'algorithme de Huffman
        /// </summary>
        public void Algo_Huffman()
        {
            //on prend la valeur de chaque pixel et on l'ajoute à la liste des fréquences
            foreach (KeyValuePair<Pixel, int> symbole in fréquences)
            {

                feuilles.Add(new Noeud(symbole.Key, symbole.Value));
            }
            //si la liste des feuilles n'est pas vide
            while (feuilles.Count > 1)
            {
                //on trie les feuilles par fréquence
                List<Noeud> feuillestriees = feuilles.OrderBy(noeud => noeud.frequence).ToList<Noeud>();
                //si la liste des feuilles triées contient au moins 2 éléments
                if (feuillestriees.Count >= 2)
                {
                    //on prend les deux premiers éléments de la liste et on les ajoute à un parent
                    List<Noeud> deuxpremiers = feuillestriees.Take(2).ToList<Noeud>();
                    Noeud parent = new Noeud(deuxpremiers[0], deuxpremiers[1]);
                    feuilles.Remove(deuxpremiers[0]);
                    feuilles.Remove(deuxpremiers[1]);
                    feuilles.Add(parent); //on ajoute le parent à la liste des feuilles
                }
                
                this.root = feuilles.FirstOrDefault(); //first or default renvoie le premier élément de la liste ou null si la liste est vide
            }

        }



    }
}
