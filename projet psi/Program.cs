// See https://aka.ms/new-console-template for more information
using projet_psi;

Console.WriteLine("Hello, World!");
MyImage image = new MyImage("images/lac.bmp");

image.AgrandirImage(0.4).From_Image_To_File("images/copie.bmp");

