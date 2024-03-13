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

                image = new Pixel[hauteur, largeur];

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

        public void From_Image_To_File(string filePath)
        {
            int paddingPerRow = (4 - (largeur * 3 % 4)) % 4;
            int fileSize = 54 + ((largeur * 3 + paddingPerRow) * hauteur);

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.WriteByte((byte)'B');
                fs.WriteByte((byte)'M');
                WriteInt32ToFileStream(fs, fileSize);
                WriteInt32ToFileStream(fs, 0);
                WriteInt32ToFileStream(fs, 54);
                WriteInt32ToFileStream(fs, 40);
                WriteInt32ToFileStream(fs, largeur);
                WriteInt32ToFileStream(fs, hauteur);
                fs.WriteByte((byte)1);
                fs.WriteByte((byte)0);
                fs.WriteByte((byte)24);
                fs.WriteByte((byte)0);
                WriteInt32ToFileStream(fs, 0);
                WriteInt32ToFileStream(fs, fileSize - 54);
                WriteInt32ToFileStream(fs, 0);
                WriteInt32ToFileStream(fs, 0);
                WriteInt32ToFileStream(fs, 0);
                WriteInt32ToFileStream(fs, 0);

                for (int y = 0; y <hauteur; y++)
                {
                    for (int x = 0; x < largeur; x++)
                    {
                        Pixel pixel = image[y, x];
                        fs.WriteByte((byte)pixel.B);
                        fs.WriteByte((byte)pixel.G);
                        fs.WriteByte((byte)pixel.R);
                    }
                    for (int p = 0; p < paddingPerRow; p++)
                    {
                        fs.WriteByte(0);
                    }
                }
            }
        }

        private void WriteInt32ToFileStream(FileStream fs, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            fs.Write(bytes, 0, bytes.Length);
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
