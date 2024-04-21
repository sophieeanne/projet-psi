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


        /// <summary>
        /// Constructeur pour créer une instance MyImage à partir d'une matrice de pixels
        /// </summary>
        /// <param name="image">matrice de pixel</param>
        /// <param name="width">largeur</param>
        /// <param name="height">hauteur</param>
        public MyImage(Pixel[,] image, int width, int height)
        {
            this.image = image;
            this.largeur = width;
            this.hauteur = height;

        }
        /// <summary>
        /// enregistre l'image dans un fichier
        /// </summary>
        /// <param name="filePath">nom du fichier</param>
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
        /// <summary>
        /// écrit un entier 32 bits dans un fichier
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="value"></param>
        private void WriteInt32ToFileStream(FileStream fs, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            fs.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// convertit un nombre de endian à int
        /// </summary>
        /// <param name="octets">tableau d'octets</param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] octets)
        {
            if (BitConverter.IsLittleEndian) //si le système est en little endian
            {
                int l = octets.Length;
                for (int i = 0; i < l / 2; i++) //inverse les octets
                {
                    byte temp = octets[i];
                    octets[i] = octets[l - i - 1];
                    octets[l - i - 1] = temp;
                }
            }
            return BitConverter.ToInt32(octets, 0);
        }

        /// <summary>
        /// convertit un entier en endian
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
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

        /// <summary>
        /// méthode pour agrandir une image
        /// </summary>
        /// <param name="factor">facteur par lequelle on veut agrandir l'image</param>
        /// <returns></returns>
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



        // pour Julia: fonction récursive qui fait z^2+c
        //pour l'instant complexe = tableau de double, si le temps je ferai une classe
        private int JulRecursif(double[] z,double[] c, int n)
        {
            if ((Math.Sqrt(z[0] * z[0] + z[1] * z[1])>2 ) || n>16)
            {
                return (n);
            }
            double a = z[0];
            double b = z[1];

            double d = a*a-b*b;
            double e = -2 * a * b;
            double[] r = { d + c[0], e + c[1] }; //^2+c
            return JulRecursif(r,c,n+1);

        }
        
        public MyImage Julia(double echelle, double[] c, int hauteur, int largeur, double contraste, int seuil)
        {
            Pixel[,] Jul = new Pixel[hauteur, largeur];
            for(int i = 0; i < largeur; i++)
            {
                for(int j = 0; j<hauteur; j++)
                {
                    double coeffcouleur = (double)5;
                    //on crée le nombre complexe associé a chaque pixel
                    double reel = echelle * (i - largeur / 2);
                    double imaginaire = echelle * (j - hauteur / 2);
                    //Console.WriteLine(reel +""+ imaginaire);
                    double[] z = { reel, imaginaire  };
                    int iter = (JulRecursif(z, c, 0));
                    Console.WriteLine(iter);
                    //double intensite1 = (double)255 / Math.Pow((iter+1),contraste);
                    //double intensite2 = (double)255 / Math.Pow((iter+2), contraste);
                    //double intensite3 = (double)255 / Math.Pow(iter, contraste); //du bricolage pour avoir des couleurs
                    byte lum = (byte)(255 * (Convert.ToInt32(iter > seuil)));

                    Jul[j, i] = new Pixel(    lum, lum, lum); 

                }
            }
            return new MyImage(Jul, largeur, hauteur);
        }


        public MyImage AppliquerMatriceConvolution(double[,] matriceConvolution)
        {
            // matriceConvolution est une matrice 3x3
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
        /// <summary>
        /// fait une rotation d'image à l'angle demandé
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="img"></param>
        /// <returns></returns>
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

            //calcule les nouvelles dimensions de l'image (Math.Ceiling permet d'arrondir à l'entier supérieur)
            int nv_larg = (int)Math.Ceiling(Math.Abs(img.largeur * cosT) + Math.Abs(img.hauteur * sinT));
            int nv_haut = (int)Math.Ceiling(Math.Abs(img.largeur * sinT) + Math.Abs(img.hauteur * cosT));
            
            //calcule du centre l'image
            double ox = img.largeur / 2.0;
            double oy = img.hauteur / 2.0;

            //initialisation de la matrice de pixels
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
                    int x = (int)Math.Round(cosT * (j - ox) - sinT * (i - oy) + nv_larg / 2.0);
                    int y = (int)Math.Round(sinT * (j - ox) + cosT * (i - oy) + nv_haut / 2.0);
                    //on vérifie si c'est dans les dimensions de l'image
                    if (x >= 0 && y >= 0 && x < nv_larg && y < nv_haut)
                    {
                        imrot[y, x] = img.image[i, j]; 
                    }
                }
            }
            return Enregistrer_Image(imrot, nv_larg, nv_haut, "images/Sortie.bmp","image_avec_rotation");
           
        }

       /// <summary>
       /// enregistre une image dans un fichier
       /// </summary>
       /// <param name="image"></param>
       /// <param name="largeur"></param>
       /// <param name="hauteur"></param>
       /// <param name="chemin"></param>
       /// <param name="nomfichier"></param>
       /// <returns></returns>
        public MyImage Enregistrer_Image(Pixel[,] image, int largeur, int hauteur,string chemin, string nomfichier)
        {
            MyImage nvimage = new MyImage(chemin);
            nvimage.image = image;
            nvimage.largeur = largeur;
            nvimage.hauteur = hauteur;
            nvimage.From_Image_To_File("images/image_"+nomfichier+".bmp");
            return nvimage;
        }

        /// <summary>
        /// méthode pour coder une image
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public MyImage Coder_Image(MyImage image1, MyImage image2)
        {
            int h = Math.Min(image1.hauteur, image2.hauteur);
            int l = Math.Min(image1.largeur, image2.largeur);
            Pixel[,] image = new Pixel[h, l];
            for(int i=0; i<h; i++)
            {
                for(int j=0; j<l; j++)
                {
                    if (i < image1.hauteur && j < image1.largeur)
                    {
                        Pixel p1 = image1.image[i, j];
                        Pixel p2 = image2.image[i, j];
                        int r = concatener(quatre_premiers_chiffres(p1.R), quatre_premiers_chiffres(p2.R));
                        int g = concatener(quatre_premiers_chiffres(p1.G), quatre_premiers_chiffres(p2.G));
                        int b = concatener(quatre_premiers_chiffres(p1.B), quatre_premiers_chiffres(p2.B));
                        image[i, j] = new Pixel((byte)r, (byte)g, (byte)b);
                    }
                    else
                    {
                        image[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
            return Enregistrer_Image(image, l, h, "images/Sortie.bmp", "image_codée");
        }
        /// <summary>
        /// méthode pour décoder une image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public MyImage Decoder_Image1(MyImage image)
        {
            Pixel[,] image1 = new Pixel[image.hauteur, image.largeur];
            Pixel[,] image2 = new Pixel[image.hauteur, image.largeur];
            //première image
            for (int i = 0; i < image.hauteur; i++)
            {
                for (int j = 0; j < image.largeur; j++)
                {
                    Pixel p = image.image[i, j];
                    int r = concatener(quatre_premiers_chiffres(p.R), 0000);
                    int g = concatener(quatre_premiers_chiffres(p.G), 0000);
                    int b = concatener(quatre_premiers_chiffres(p.B), 0000);
                    image1[i, j] = new Pixel((byte)r, (byte)g, (byte)b);
                }
            }
            return Enregistrer_Image(image1, image.largeur, image.hauteur, "images/Sortie.bmp","image_décodée_1");
        }
        
        /// <summary>
        /// méthode pour décoder une autre image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public MyImage Decoder_Image2(MyImage image)
        {
            Pixel[,] image2 = new Pixel[image.hauteur, image.largeur];
            for (int i = 0; i < image.hauteur; i++)
            {
                for (int j = 0; j < image.largeur; j++)
                {
                    Pixel p = image.image[i, j];
                    int r = concatener(quatre_derniers_chiffres(p.R), 0000);
                    int g = concatener(quatre_derniers_chiffres(p.G), 0000);
                    int b = concatener(quatre_derniers_chiffres(p.B), 0000);
                    image2[i, j] = new Pixel((byte)r, (byte)g, (byte)b);
                }
            }
            return Enregistrer_Image(image2, image.largeur, image.hauteur, "images/Sortie.bmp","image_décodée_2");
        }

        /// <summary>
        /// méthode pour convertir un nombre décimal en binaire
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int Decimal_Vers_Binaire(int n)
        {
            int b = 0;
            int i = 1;
            int pos = 0;
            while (n != 0)
            {
                b += (n % 2) * i;
                n /= 2;
                i *= 10;
                pos++;
            }
            while (pos < 8)
            {
                b += (int)Math.Pow(10, pos); 
                pos++;
            }
            return b;
        }
        /// <summary>
        /// méthode pour avoir les 4 premiers chiffres d'un nombre
        /// </summary>
        /// <param name="n"></param>
        /// <returns> les 4 premiers chiffres d'un nombre</returns>
        public int quatre_premiers_chiffres(int n)
        {
            n = Decimal_Vers_Binaire(n);
            string a = n.ToString();
            string b = a.Substring(0, 4);
            return int.Parse(b);
      
        }
        /// <summary>
        /// méthode pour avoir les 4 derniers chiffres d'un nombre
        /// </summary>
        /// <param name="n"></param>
        /// <returns> les 4 derniers chiffres d'un nombre</returns>
        public int quatre_derniers_chiffres(int n)
        {
            n = Decimal_Vers_Binaire(n);
            string a = n.ToString();
            string b = a.Substring(a.Length-4);
            return int.Parse(b);
        }
        /// <summary>
        /// méthode pour concaténer deux nombres
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>le nombre concaténé</returns>
        public int concatener(int a, int b)
        {
            string stra = a.ToString();
            string strb = b.ToString();
            string str = stra + strb;
            return int.Parse(str);
        }















    }
}
