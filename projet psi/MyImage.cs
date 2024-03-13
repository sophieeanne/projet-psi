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
            using (var fs = new FileStream(file, FileMode.Create))
            using (var writer = new BinaryWriter(fs))
            {
                // En-tête de fichier BMP
                writer.Write(new char[] { 'B', 'M' }); // Signature
                int fileSize = 54 + (largeur * hauteur * 3); // Taille du fichier avec en-tête
                writer.Write(fileSize); // Taille du fichier
                writer.Write(0); // Champ réservé
                writer.Write(54); // Offset des données de l'image (en-tête inclus)

                // En-tête DIB (BITMAPINFOHEADER)
                writer.Write(40); // Taille de l'en-tête DIB
                writer.Write(largeur); // Largeur
                writer.Write(hauteur); // Hauteur
                writer.Write((short)1); // Nombre de plans de couleur
                writer.Write((short)24); // Bits par pixel
                writer.Write(0); // Pas de compression
                writer.Write(largeur * hauteur * 3); // Taille de l'image
                writer.Write(0); // Résolution horizontale (pixels/mètre)
                writer.Write(0); // Résolution verticale (pixels/mètre)
                writer.Write(0); // Nombre de couleurs dans la palette
                writer.Write(0); // Toutes les couleurs sont importantes

                // Écriture des données de l'image (pixel par pixel)
                for (int y = hauteur - 1; y >= 0; y--) // BMP stocke les pixels de bas en haut
                {
                    for (int x = 0; x < largeur; x++)
                    {
                        Pixel pixel = image[y, x];
                        writer.Write(pixel.B);
                        writer.Write(pixel.G);
                        writer.Write(pixel.R);
                    }

                    // Padding pour aligner chaque ligne sur un multiple de 4 octets
                    for (int padding = (largeur * 3) % 4; padding > 0 && padding < 4; padding--)
                    {
                        writer.Write((byte)0);
                    }
                }
            }
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
