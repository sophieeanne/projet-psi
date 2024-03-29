﻿// See https://aka.ms/new-console-template for more information
using projet_psi;
using static System.Net.Mime.MediaTypeNames;

//image qu'on veut traiter
MyImage image = new MyImage("images/lac.bmp");
MyImage image2 = new MyImage("images/coco.bmp");

//Test rotation
//MyImage rota = image.Rotation(90, image);

//Test convolution
//double[,] sobelVertical = new double[,]
//{
//    { -1, 0, 1 },
//    { -2, 0, 2 },
//    { -1, 0, 1 }
//};
//image.AppliquerMatriceConvolution(sobelVertical).From_Image_To_File("images/Sortie.bmp");

//Test coder 
MyImage image_codee = image2.Coder_Image(image2, image);

//Test decoder
MyImage image_decodee1 = image_codee.Decoder_Image1(image_codee);
MyImage image_decodee2 = image_codee.Decoder_Image2(image_codee);

//Debug
Console.WriteLine("y'a une sortie");
//rota.From_Image_To_File("images/testrotation.bmp");

//TestMandelbrot();

//MyImage image = new MyImage("images/lac.bmp");
//MyImage rota = image.Rotation(90, image); // Utilisation d'une instance différente de MyImage pour l'image de sortie
//Console.WriteLine("y'a une sortie");


static void TestMandelbrot()
{
    Console.WriteLine("entrez Re(c)");
    double a = Convert.ToDouble(Console.ReadLine());

    Console.WriteLine("entrez Im(c)");
    double b = Convert.ToDouble(Console.ReadLine());

    double[] c = {a,b};

    Console.WriteLine("entrez la largeur");
    int largeur = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("entrez la hauteur");
    int hauteur = Convert.ToInt32(Console.ReadLine());

    //Console.WriteLine("entrez le contraste");
    double contraste = 0.2; //Convert.ToDouble(Console.ReadLine());

    //Console.WriteLine("entrez l'échelle");
    //double echelle = Convert.ToDouble(Console.ReadLine());

    double echelle = (double)2 / largeur;

    Console.WriteLine(echelle);
    Pixel[,] blank = new Pixel[largeur,hauteur];

    MyImage mandel = new MyImage(blank, largeur, hauteur);
    mandel.Mandelbrot(echelle, c, hauteur, largeur, contraste).From_Image_To_File("images/Mandel.bmp");
    Console.WriteLine("rergarder le farctale de mandlebort");
}
