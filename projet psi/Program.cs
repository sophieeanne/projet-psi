using projet_psi;
using static System.Net.Mime.MediaTypeNames;

//images qu'on veut traiter
MyImage image = new MyImage("images/Test.bmp");
MyImage image2 = new MyImage("images/coco.bmp");

Test_Rotation(image);
//Test_Codage(image2, image); 
//Test_Decodage(image, image2);
//Test_Convolution(image);

//TestMandelbrot();

static void Test_Convolution(MyImage image)
{
    double[,] sobelVertical = new double[,]
    {
        { -1, 0, 1 },
        { -2, 0, 2 },
        { -1, 0, 1 }
    };
    image.AppliquerMatriceConvolution(sobelVertical).From_Image_To_File("images/Sortie.bmp");
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
