// See https://aka.ms/new-console-template for more information
using projet_psi;
using static System.Net.Mime.MediaTypeNames;

//image qu'on veut traiter
MyImage image = new MyImage("images/lac.bmp");

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

//Debug
Console.WriteLine("y'a une sortie");
