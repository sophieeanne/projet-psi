using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using System.ComponentModel;

namespace projet_psi
{
    internal class MyImage
    {
        int taille;
        int tailleOffset;
        int largeur;
        int hauteur;
        int nbBits;
        Pixel[,] image;
        public MyImage(string myfile)
        {
            byte[] file = File.ReadAllBytes(myfile);
            if (file[0]=='B' && file[1]=='M')
            {
                taille = BitConverter.ToInt32(file, 2);

                tailleOffset = BitConverter.ToInt32(file, 10);

                largeur = BitConverter.ToInt32(file, 18);
                hauteur = BitConverter.ToInt32(file, 22);

                nbBits = BitConverter.ToInt32(file, 28);



                //remplissage de la matrice de pixels
                for (int y = 0; y < hauteur; y++)
                {
                    for (int x = 0; x < largeur; x++)
                    {
                        int pixelIndex = tailleOffset + (y * largeur + x) * 3; //trouve l'index du pixel dans les bytes
                        byte blue = file[pixelIndex];  
                        byte green = file[pixelIndex + 1];
                        byte red = file[pixelIndex + 2];

                        image[y, x] = new Pixel(red, green, blue);
                    }
                }
            }
            else
            {
                Console.WriteLine("Fichier erroné");
            }
        }

        public void From_Image_To_File(string file)
        {
            
        }

        public byte[] Convertir_Int_To_Endian(int val)
        {
            byte[] octets = BitConverter.GetBytes(val); //convertit l'entier en tableau de bytes
            if (BitConverter.IsLittleEndian) //si le système est en little endian
            {
                int l = octets.Length;
                for(int i = 0; i < l/2; i++) //inverse les octets
                {
                    byte temp = octets[i];
                    octets[i] = octets[l - i - 1];
                    octets[l - i - 1] = temp;
                }   
            }
            return octets;

        }
    }
}
