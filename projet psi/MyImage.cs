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
    public class MyImage
    {
        int taille;
        int tailleOffset;
        int largeur;
        int hauteur;
        int nbBits;
        Pixel[,] image;

        //constructeur sans paramètre
        public MyImage()
        {
            taille = 0;
            tailleOffset = 0;
            largeur = 0;
            hauteur = 0;
            nbBits = 0;
            image = null;
        }
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

        public MyImage Grayscale()
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Pixel p = image[i, j];
                    int n = ((int)p.R + (int)p.B + (int)p.G) / 3;
                    image[i, j] = new Pixel((byte)n, (byte)n, (byte)n);
                }
            }
            MyImage im = new MyImage(image, largeur,hauteur);
            return im; 

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



        /// <summary>
        /// Calcule récursivement le nombre d'itérations de Julia pour un point donné avec la formule z carré + c avec les fonctions d'instance complexe
        /// </summary>
        private int JulRecursif(Complex z, Complex c, int n)
        {
            if (z.ModuleCarré() > 4 || n > 32)
            {
                return n;
            }
            return JulRecursif(z.CarréPlus(c), c, n + 1);
        }


        /// <summary>
        /// Génère une image des fractales julia
        /// </summary>
        public MyImage Julia(double echelle, Complex c, int hauteur, int largeur, int seuil)
        {
            Pixel[,] Jul = new Pixel[hauteur, largeur];
            double offsetX = largeur / 2.0;
            double offsetY = hauteur / 2.0;

            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    double reel = echelle * (i - offsetX);
                    double imaginaire = echelle * (j - offsetY);
                    Complex z = new Complex(reel, imaginaire);  //nombre complexe associé a chaque pixel
                    int iter = JulRecursif(z, c, 0);  //iterations de quand ca diverge ou atteint le max 
                    byte lum = (byte)(255 * (Convert.ToInt32(iter > seuil)));
                    Jul[j, i] = new Pixel(lum, lum, lum);
                }
            }
            return new MyImage(Jul, largeur, hauteur);
        }
        /// <summary>
        /// Calcule le nombre d'itérations nécessaires pour la divergence de c
        /// </summary>
        /// <param name="c">Le point complexe à tester.</param>
        /// <param name="maxIter">Nombre maximal d'itérations.</param>
        /// <returns>Le nombre d'itérations réalisées avant que la suite ne dépasse 2 en module.</returns>
        private int mandel(Complex c, int maxIter)
        {
            Complex z = new Complex(0, 0);
            int n = 0;
            // boucle pour voir si ça diverge
            while (z.ModuleCarré() <= 4 && n < maxIter)
            {
                z = z.CarréPlus(c); // z devient z^2 + c
                n++;
            }
            return n; // retourne le nb d'itérations
        }

        /// <summary>
        /// Génère une image de Mandelbrot
        /// </summary>
        /// <param name="echelle">Facteur d'échelle pour le zoom </param>
        /// <param name="hauteur">Hauteur de l'image en pixels.</param>
        /// <param name="largeur">Largeur </param>
        /// <param name="maxIter">Nombre maximal d itérations pour chaque point.</param>
        /// <returns>Une image de lensemble de Mandelbrot</returns>
        public MyImage mandelbrot(double echelle, int hauteur, int largeur, int maxIter)
        {
            Pixel[,] image = new Pixel[hauteur, largeur];
            double offsetX = largeur / 2.0;
            double offsetY = hauteur / 2.0;

            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    double reel = (i - offsetX) * echelle;
                    double imaginaire = (j - offsetY) * echelle;
                    Complex c = new Complex(reel, imaginaire);
                    int iter = mandel(c, maxIter);
                    byte lum = (byte)(255 * iter / maxIter);//pour avoir la nuance de gris selon le nombre d'iterations ppour diverger
                    image[j, i] = new Pixel(lum, lum, lum); // on met la couleur
                }
            }
            return new MyImage(image, largeur, hauteur);
        }


        /// <summary>
        /// Calcule le rotationnel de l'image et crée une nouvelle image avec le résultat
        /// </summary>
        public MyImage CalculerRotationnel()
        {
            Pixel[,] resultat = new Pixel[hauteur, largeur];

            //boucle dans l'image en evitant les bords
            for (int j = 1; j < hauteur - 1; j++)
            {
                for (int i = 1; i < largeur - 1; i++)
                {
                    // Equivalent des derivees partielles
                    int dBdx = image[j, i + 1].B - image[j, i].B;
                    int dBdy = image[j + 1, i].B - image[j, i].B;
                    int dGdx = image[j, i + 1].G - image[j, i].G;
                    int dRdy = image[j + 1, i].R - image[j, i].R;

                    // Calcule le rotationnel
                    byte composanteX = (byte)Math.Max(0, Math.Min(255, 128 + dBdy)); // Pour eviter les valeurs negatives ou superieures a 255
                    byte composanteY = (byte)Math.Max(0, Math.Min(255, 128 - dBdx)); 
                    byte composanteZ = (byte)Math.Max(0, Math.Min(255, 128 + dGdx - dRdy)); // Z est un melange des deux autres

                    resultat[j, i] = new Pixel(composanteX, composanteY, composanteZ);
                }
            }

            //Bords  en noir
            for (int i = 0; i < largeur; i++)
            {
                resultat[0, i] = new Pixel(0, 0, 0); 
                resultat[hauteur - 1, i] = new Pixel(0, 0, 0); 
            }
            for (int j = 0; j < hauteur; j++)
            {
                resultat[j, 0] = new Pixel(0, 0, 0); 
                resultat[j, largeur - 1] = new Pixel(0, 0, 0); 
            }

            return new MyImage(resultat, largeur, hauteur);
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
            if(img.image == null)
            {
                return null;
            }
            //transformer les angles en radian
            double rad = angle * Math.PI / 180;
            double cosT = Math.Cos(rad);
            double sinT = Math.Sin(rad);

            //calcule les nouvelles dimensions de l'image 
            int nv_larg = (int)(Math.Abs(img.largeur * cosT) + Math.Abs(img.hauteur * sinT));
            int nv_haut= (int)(Math.Abs(img.largeur * sinT) + Math.Abs(img.hauteur * cosT));

            //initialisation de la matrice de pixels
            Pixel[,] imrot = new Pixel[nv_haut, nv_larg];

            //on calcule le centre de l'image originelle
            int x0 = img.largeur / 2;
            int y0 = img.hauteur / 2;

            //on calcule le centre de l'image avec rotation
            int x1 = nv_larg / 2;
            int y1 = nv_haut / 2;

            for (int i = 0; i < nv_haut; i++)
            {
                for (int j = 0; j < nv_larg; j++)
                {
                    //on mutliplie par la matrice de rotation, on fait une rotation autour du centre de l'image
                    int x = (int)((j - x1) * cosT + (i - y1) * sinT + x0);
                    int y = (int)((i - y1) * cosT - (j - x1) * sinT + y0);

                    if (x >= 0 && x < img.largeur && y >= 0 && y < img.hauteur)
                    {
                        imrot[i, j] = img.image[y, x];
                    }
                    else
                    {
                        imrot[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
            return Enregistrer_Image(imrot, nv_larg, nv_haut, "images/Sortie.bmp", "image avec rotation");


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
        /// <returns> </returns> 
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
            return Enregistrer_Image(image2, image.largeur, image.hauteur, "images/Sortie.bmp","image_décodée_2"); //enregistrer l'image dans un fichier 
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
        /// <summary>
        /// méthode pour convertir une image en niveaux de gris
        /// </summary>
        public void Convertir_YCbCR()
        {
            Pixel[,] pixels = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Pixel p = image[i, j];
                    int Y = (int)(0.299 * p.R + 0.587 * p.G + 0.114 * p.B);
                    int Cb = (int)(-0.1687 * p.R - 0.3313 * p.G + 0.5 * p.B + 128);
                    int Cr = (int)(0.5 * p.R - 0.4187 * p.G - 0.0813 * p.B + 128);
                    pixels[i, j] = new Pixel((byte)Y, (byte)Cr, (byte)Cb);

                }
            }
            Enregistrer_Image(pixels, largeur, hauteur, "images/Sortie.bmp", "YCbCr");
        }
        
        public void sous_echantillonage_422(MyImage im)
        {
            im.Convertir_YCbCR();
            Pixel[,] pixels = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Pixel p = im.image[i, j];
                    int Y = p.R;
                        if (j % 2 == 0)
                        {
                            pixels[i, j] = im.image[i, j];
                        }
                        else
                        {
                            Pixel p1 = im.image[i, j - 1];
                            pixels[i, j] = new Pixel((byte)Y, (byte)p1.G, (byte)p1.B);
                        }
                }
            }   
            Enregistrer_Image(pixels, largeur, hauteur, "images/Sortie.bmp", "sous_echantillonage_422");
        }

        public void sous_echantillonage_420(MyImage im)
        {
            im.Convertir_YCbCR();
            Pixel[,] pixels = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Pixel p = im.image[i, j];
                    int Y = p.R;
                    if (i%2 ==0 && j%2 == 0)
                    {
                        pixels[i, j] = im.image[i, j];
                    }
                    else if (i%2 != 0 && j%2 == 0)
                    {
                        Pixel p1 = im.image[i - 1, j];
                        pixels[i, j] = new Pixel((byte)Y, (byte)p1.G, (byte)p1.B);
                        
                    }
                    else if (i % 2 == 0 && j % 2 != 0)
                    {
                        Pixel p1 = im.image[i, j - 1];
                        pixels[i, j] = new Pixel((byte)Y, (byte)p1.G, (byte)p1.B);
                       
                    }
                    else
                    {
                        Pixel p1 = im.image[i - 1, j];
                        Pixel p2 = im.image[i, j - 1];
                        pixels[i, j] = new Pixel((byte)Y, (byte)((p1.G + p2.G) / 2), (byte)((p1.B + p2.B) / 2));     
                    }   
                }
            }
            Enregistrer_Image(pixels, largeur, hauteur, "images/Sortie.bmp", "sous_echantillonage_420");
        }

        public List<Pixel[,]> division_matrices_8x8(MyImage im)
        {
            List<Pixel[,]> matrices = new List<Pixel[,]>();
            for (int i = 0; i < (im.hauteur+7)/8; i += 8)
            {
                for (int j = 0; j < (im.largeur+7)/8; j += 8)
                {
                    Pixel[,] matrice = new Pixel[8, 8];
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            matrice[x, y] = im.image[i + x, j + y];
                        }
                    }
                    matrices.Add(matrice);
                }
            }
            return matrices;
        }

        public void DCT(List<Pixel[,]> matrices, MyImage im)
        {
            foreach (Pixel[,] matrice in matrices)
            {
                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        Pixel p = matrice[i, j];    
                        byte red = (byte)(p.R - 128);
                        byte green = (byte)(p.G - 128);
                        byte blue = (byte)(p.B - 128);
                        matrice[i, j] = new Pixel(red, green, blue);
                    }
                }
                double[,] dct = new double[8, 8];
                double ci, cj, dct1, somme;
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        if (u == 0)
                        {
                            ci = 1 / Math.Sqrt(8);
                        }
                        else
                        {
                            ci = Math.Sqrt(2) / Math.Sqrt(8);
                        }
                        if (v == 0)
                        {
                            cj = 1 / Math.Sqrt(8);
                        }
                        else
                        {
                            cj = Math.Sqrt(2) / Math.Sqrt(8);
                        }
                        somme = 0;
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {
                                dct1 = matrice[x, y].R * Math.Cos((2 * x + 1) * u * Math.PI / 16) * Math.Cos((2 * y + 1) * v * Math.PI / 16);
                                somme += dct1;
                            }
                        }
                        dct[u, v] = ci * cj * somme; 
                    }
                }

                //for(int i =0; i < 8; i++)
                //{
                //    for(int j= 0; j < 8; j++)
                //    {
                //        Console.Write(dct[i, j] + " ");
                //    }
                //    Console.WriteLine();
                //}


            }
        }   
        public void quantification(MyImage im)
        {
            List<Pixel[,]> matrices = division_matrices_8x8(im);
            DCT(matrices, im);
            int[,] q = {
                            { 16, 11, 10, 16, 24, 40, 51, 61 },
                            { 12, 12, 14, 19, 26, 58, 60, 55 },
                            { 14, 13, 16, 24, 40, 57, 69, 56 },
                            { 14, 17, 22, 29, 51, 87, 80, 62 },
                            { 18, 22, 37, 56, 68, 109, 103, 77 },
                            { 24, 35, 55, 64, 81, 104, 113, 92 },
                            { 49, 64, 78, 87, 103, 121, 120, 101 },
                            { 72, 92, 95, 98, 112, 100, 103, 99 } };
            foreach (Pixel[,] matrice in matrices)
            {
                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        Pixel p = matrice[i, j];    
                        byte red = (byte)(p.R / q[i, j]);
                        byte green = (byte)(p.G / q[i, j]);
                        byte blue = (byte)(p.B / q[i, j]);
                        matrice[i, j] = new Pixel(red, green, blue);
                    }
                }
            }       
        }


    }
}
