// See https://aka.ms/new-console-template for more information
using projet_psi;

Console.WriteLine("Hello, World!");
MyImage image = new MyImage("images/lac.bmp");

image.AgrandirImage(3,2).From_Image_To_File("images/copie.bmp");