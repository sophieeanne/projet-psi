// See https://aka.ms/new-console-template for more information
using projet_psi;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, World!");
//MyImage image = new MyImage("images/Test.bmp");
////double[,] sobelVertical = new double[,]
////{
////    { -1, 0, 1 },
////    { -2, 0, 2 },
////    { -1, 0, 1 }
////};
////image.AppliquerMatriceConvolution(sobelVertical).From_Image_To_File("images/Sortie.bmp");

//MyImage rota = image.Rotation(90, image);
//rota.From_Image_To_File("images/testrotation.bmp");
MyImage image = new MyImage("images/lac.bmp");
MyImage rota = image.Rotation(90, image); // Utilisation d'une instance différente de MyImage pour l'image de sortie
Console.WriteLine("y'a une sortie");
