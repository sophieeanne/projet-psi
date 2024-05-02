﻿using projet_psi;
using static System.Net.Mime.MediaTypeNames;

//images qu'on veut traiter
MyImage image = new MyImage("images/lac.bmp");
MyImage image2 = new MyImage("images/Test.bmp");

Test_Rotation(image2);
//Test_Codage(image2, image); 
//Test_Decodage(image, image);
//Test_Convolution(image2);
//TestMandelbrot();
//TestJulia();
//TestRotationnel(image2);
//image2.sous_echantillonage_420(image2);
//Test_Grayscale(image2);
//Console.WriteLine("sortie");

static void Test_Grayscale(MyImage image_originale)
{
    MyImage image = image_originale.Grayscale();
    image.From_Image_To_File("images/Sortie.bmp");
}
static void Test_Convolution(MyImage image)
{
    double[,] sobelVertical = new double[,]
    {
        { -1, 0, 1 },
        { -2, 0, 2 },
        { -1, 0, 1 }
    };
    double[,] MatriceFlou = new double[,]
    {
        { 1.0 / 9.0, 1.0 / 9.0, 1.0 / 9.0 },
        { 1.0 / 9.0, 1.0 / 9.0, 1.0 / 9.0 },
        { 1.0 / 9.0, 1.0 / 9.0, 1.0 / 9.0 }
    };
    double[,] detectioncontours = new double[,]
    {
        {1,0,-1 },
        {0,0,0 },
        {-1,0,1 }
    };
    image.AppliquerMatriceConvolution(MatriceFlou).From_Image_To_File("images/Sortie.bmp");
    Console.WriteLine("regardez l'image convoluée");
}       
static void Test_Rotation(MyImage image)
{
    Console.WriteLine("entrez l'angle de rotation");
    int angle = Convert.ToInt32(Console.ReadLine());
    MyImage rota = image.Rotation(angle, image);
    Console.WriteLine("regardez l'image de rotation");
}
static void Test_Codage(MyImage image, MyImage imagebis)
{
    image.Coder_Image( image,  imagebis);
    Console.WriteLine("regardez l'image codée");
}

static void Test_Decodage(MyImage image, MyImage imagebis)
{
    Console.WriteLine("on code d'abord une image");
    MyImage image_codee = image.Coder_Image(image, imagebis);
    Console.WriteLine("regardez l'image codée");
    image.Decoder_Image1(image_codee);
    image.Decoder_Image2(image_codee);
    Console.WriteLine("regardez les images décodées");
}

static void TestJulia()
{
    //Console.WriteLine("entrez Re(c)");
    //double a = Convert.ToDouble(Console.ReadLine());
    double a = (0-0.70176);
    //Console.WriteLine("entrez Im(c)");
    //double b = Convert.ToDouble(Console.ReadLine());
    double b = 0 - 0.3842;
    Complex c = new Complex(a,b);

    //Console.WriteLine("entrez la largeur");
    //int largeur = Convert.ToInt32(Console.ReadLine());
    int largeur = 2000;
    //Console.WriteLine("entrez la hauteur");
    //int hauteur = Convert.ToInt32(Console.ReadLine());
    int hauteur = largeur;
    //Console.WriteLine("entrez le seuil");
    int seuil = 16;
    //int seuil = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("entrez l'echelle par exemple 0.05");
    
    double echelle = Convert.ToDouble(Console.ReadLine());


    Console.WriteLine(echelle);
    Pixel[,] blank = new Pixel[largeur,hauteur];

    MyImage Jul = new MyImage(blank, largeur, hauteur);
    Jul.Julia(echelle, c, hauteur, largeur, seuil).From_Image_To_File("images/Jul.bmp");
    Console.WriteLine("Regardez le fractale de Julia ");
}
static void TestMandelbrot()
{
    //Console.WriteLine("entrez Re(c)");
    double a = (0 - 0.70176);
    //Console.WriteLine("entrez Im(c)");
    double b = 0 - 0.3842;
    Complex c = new Complex(a, b);

    //Console.WriteLine("entrez la largeur");
    int largeur = 500;
    //Console.WriteLine("entrez la hauteur");
    int hauteur = largeur;
    //Console.WriteLine("entrez le seuil");
    //int seuil = Convert.ToInt32(Console.ReadLine());
    int seuil = 16;
    Console.WriteLine("entrez l'echelle par exempl 0.001");
    double echelle = Convert.ToDouble(Console.ReadLine());

    Console.WriteLine(echelle);
    Pixel[,] blank = new Pixel[largeur, hauteur];

    MyImage mandelImg = new MyImage(blank, largeur, hauteur);
    mandelImg.mandelbrot(echelle, hauteur, largeur, seuil).From_Image_To_File("images/Mandel.bmp");
    Console.WriteLine("Regardez le fractale de Mandelbrot ");
}
static void TestRotationnel(MyImage image)
{
    // Calcule le rotationnel de l'image
    MyImage imageRotationnel = image.CalculerRotationnel();
    // Enregistre l'image du rotationnel dans un fichier
    imageRotationnel.From_Image_To_File("images/Rotationnel.bmp");
    // Indique à l'utilisateur où trouver l'image
    Console.WriteLine("L'image rotationnelle a été enregistrée dans images/Rotationnel.bmp");
}

static void TestAgrandirImage(MyImage image, double facteur)
{
    // Agrandit l'image
    MyImage imageAgrandie = image.AgrandirImage(facteur);
    // Enregistre l'image agrandie dans un fichier
    imageAgrandie.From_Image_To_File("images/Agrandie.bmp");
    // Indique à l'utilisateur où trouver l'image
    Console.WriteLine("L'image agrandie a été enregistrée dans images/Agrandie.bmp");
}
