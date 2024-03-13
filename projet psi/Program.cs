// See https://aka.ms/new-console-template for more information
using projet_psi;

Console.WriteLine("Hello, World!");
MyImage image = new MyImage("images/lac.bmp");
double[,] sobelVertical = new double[,]
{
    { -1, 0, 1 },
    { -2, 0, 2 },
    { -1, 0, 1 }
};
image.AppliquerMatriceConvolution(sobelVertical).From_Image_To_File("images/copie.bmp");

