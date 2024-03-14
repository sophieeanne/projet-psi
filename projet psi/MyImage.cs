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
            if (file.Length >0 && file[0]=='B' && file[1]=='M')
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


        // Constructeur pour créer une instance MyImage à partir d'une matrice de pixels
        public MyImage(Pixel[,] image, int width, int height)
        {
            this.image = image;
            this.largeur = width;
            this.hauteur = height;

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

                for (int y = 0; y <hauteur; y++ )
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



        public MyImage AgrandirImage(double factor)
        {
            // Calcul des nouvelles dimensions arrondies au plus proche entier
            int newLargeur = (int)Math.Round(this.largeur * factor);
            int newHauteur = (int)Math.Round(this.hauteur * factor);

            // Nouvelle image agrandie
            Pixel[,] newImage = new Pixel[newHauteur, newLargeur];

            for (int y = 0; y < newHauteur; y++)
            {
                for (int x = 0; x < newLargeur; x++)
                {
                    // Trouver le pixel correspondant dans l'image originale
                    int oldX = (int)(x / factor);
                    int oldY = (int)(y / factor);

                    // S'assurer que les coordonnées calculées ne dépassent pas les dimensions originales
                    oldX = Math.Min(oldX, this.largeur - 1);
                    oldY = Math.Min(oldY, this.hauteur - 1);

                    // Répéter le pixel correspondant dans la nouvelle image
                    newImage[y, x] = this.image[oldY, oldX];
                }
            }

            // Créer une nouvelle instance de MyImage avec la nouvelle image agrandie
            return new MyImage(newImage, newLargeur, newHauteur);
        }







        public MyImage AppliquerMatriceConvolution(double[,] matriceConvolution)
        {
            // Assumer que matriceConvolution est une matrice 3x3
            int width = this.largeur;
            int height = this.hauteur;

            Pixel[,] newImage = new Pixel[height, width];


            // Définir les bordures en noir
            Pixel blackPixel = new Pixel(0, 0, 0);

            // Bordure supérieure et inférieure
            for (int x = 0; x < width; x++)
            {
                newImage[0, x] = blackPixel; // Bordure supérieure
                newImage[height - 1, x] = blackPixel; // Bordure inférieure
            }

            // Bordure gauche et droite
            for (int y = 0; y < height; y++)
            {
                newImage[y, 0] = blackPixel; // Bordure gauche
                newImage[y, width - 1] = blackPixel; // Bordure droite
            }




            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterY = 0; filterY < 3; filterY++)
                    {
                        for (int filterX = 0; filterX < 3; filterX++)
                        {
                            int imageX = (x - 1 + filterX + width) % width;
                            int imageY = (y - 1 + filterY + height) % height;

                            Pixel pixel = this.image[imageY, imageX];

                            red += pixel.R * matriceConvolution[filterY, filterX];
                            green += pixel.G * matriceConvolution[filterY, filterX];
                            blue += pixel.B * matriceConvolution[filterY, filterX];
                        }
                    }

                    // Clamp les valeurs pour qu'elles soient dans l'intervalle [0, 255]
                    int r = Math.Max(0, Math.Min(255, (int)Math.Round(red)));
                    int g = Math.Max(0, Math.Min(255, (int)Math.Round(green)));
                    int b = Math.Max(0, Math.Min(255, (int)Math.Round(blue)));

                    newImage[y, x] = new Pixel((byte)r, (byte)g, (byte)b);
                }
            }

            return new MyImage(newImage, width, height);
        }
        public MyImage Rotation(int angle, MyImage img)
        {
            if (img.image == null)
            {
                return null;
            }

            //transformer les angles en radian
            double rad = angle * Math.PI / 180.0;
            double cosT = Math.Cos(rad);
            double sinT = Math.Sin(rad);

            //calcule les nouvelles dimensions de l'image
            int nv_larg = (int)Math.Ceiling(Math.Abs(img.largeur * cosT) + Math.Abs(img.hauteur * sinT));
            int nv_haut = (int)Math.Ceiling(Math.Abs(img.largeur * sinT) + Math.Abs(img.hauteur * cosT));
            
            double ox = img.largeur / 2.0;
            double oy = img.hauteur / 2.0;

            
            Pixel[,] imrot = new Pixel[nv_haut, nv_larg];
            
            //remplir la matrice en blanc (utile si l'angle n'est pas 90°)
            for (int i = 0; i < nv_haut; i++)
            {
                for (int j = 0; j < nv_larg; j++)
                {
                    imrot[i, j] = new Pixel(255, 255, 255);
                }
            }

            for (int i = 0; i < img.hauteur; i++)
            {
                for (int j = 0; j < img.largeur; j++)
                {
                    //rotation par rapport au centre de la matrice : on mutliplie coordonnée par la matrice de rotation
                    int x = (int)Math.Round(cosT * j - sinT * i);
                    int y = (int)Math.Round(sinT * j + cosT * i);
                    
                    //on vérifie si c'est dans les dimensions de l'image
                    if (x >= 0 && y >= 0 && x < nv_larg && y < nv_haut)
                    {
                        imrot[y, x] = img.image[i, j]; 
                    }
                }
            }
            return Enregistrer_Image_Rotation(imrot, nv_larg, nv_haut, "images/Sortie.bmp");
           
        }

        //retourne une nouvelle instance de myimage (l'image avec rotation) et l'enregistre dans le dossier image
        public MyImage Enregistrer_Image_Rotation(Pixel[,] image, int largeur, int hauteur,string chemin)
        {
            MyImage nvimage = new MyImage(chemin);
            nvimage.image = image;
            nvimage.largeur = largeur;
            nvimage.hauteur = hauteur;
            nvimage.From_Image_To_File("images/image_avec_rotation.bmp");
            return nvimage;
        }
















    }
}
