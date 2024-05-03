using projet_psi;
using static System.Net.Mime.MediaTypeNames;

//images qu'on veut traiter
MyImage image = new MyImage("images/Test.bmp");
MyImage image2 = new MyImage("images/Test.bmp");

string choix = "";
string choix2;

Console.WriteLine("Bienvenue dans le projet de traitement d'images");
Console.WriteLine("Voici les images initialisées :");
Console.WriteLine("image 1 : Test ");
Console.WriteLine("image 2 : Test");
Console.WriteLine("Voulez vous les changer (O/N) ?");
choix2 = Console.ReadLine();
if (choix2 == "O" || choix2 == "o")
{
    Console.WriteLine("Quelle image voulez vous ? \n1. images/Test.bmp \n2. images/coco.bmp \n3. images/lac.bmp ?");
    int c = Convert.ToInt32(Console.ReadLine());
    switch (c)
    {
        case 1:
            image = new MyImage("images/Test.bmp");
            break;
        case 2:
            image = new MyImage("images/coco.bmp");
            break;
        case 3:
            image = new MyImage("images/lac.bmp");
            break;
    }
    Console.WriteLine("Quelle deuxième image voulez vous ? \n1. images/Test.bmp \n2. images/coco.bmp \n3. images/lac.bmp ?");
    int d = Convert.ToInt32(Console.ReadLine());
    switch (d)
    {
        case 1:
            image2 = new MyImage("images/Test.bmp");
            break;
        case 2:
            image2 = new MyImage("images/coco.bmp");
            break;
        case 3:
            image2 = new MyImage("images/lac.bmp");
            break;
    }
}

do
{
    Console.Clear();
    Console.WriteLine("Voici les différentes fonctions du programme");
    Console.WriteLine("1. Grayscale \n2. Noir et blanc (ne choisis pas mtn je trouve pas) \n3. Rotation \n4. Agrandissement \n5. Matrice de convolution \n6. Créer une image décrivant une fractale \n7. Coder et Décoder une image \n8. Innovation");
    Console.WriteLine("Entrez le numéro de la fonction que vous voulez tester");
    int choix1 = Convert.ToInt32(Console.ReadLine());
    switch(choix1)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("Test de la fonction Grayscale");
            Test_Grayscale(image);
            break;
        case 2:
            //image2.NoirEtBlanc().From_Image_To_File("images/Sortie.bmp");
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("Test de la fonction Rotation");
            Test_Rotation(image2);
            break;
        case 4:
            Console.Clear();
            Console.WriteLine("Entrez le facteur d'agrandissement");
            double facteur = Convert.ToDouble(Console.ReadLine());
            TestAgrandirImage(image2, facteur);
            break;
        case 5:
            Console.Clear();
            Test_Convolution(image2);
            break;
        case 6:
            Console.Clear();
            Console.WriteLine("1. Mandelbrot \n2. Julia");
            int choix3 = Convert.ToInt32(Console.ReadLine());
            if(choix3 == 1)
            {
                TestMandelbrot();
            }
            else
            {
                TestJulia();
            }
            break;
        case 7:
            Console.Clear();
            Console.WriteLine("1. Coder \n2. Décoder");
            int choix4 = Convert.ToInt32(Console.ReadLine());
            if(choix4 == 1)
            {
                Test_Codage(image2, image);
            }
            else
            {
                Test_Decodage(image, image);
            }
            break;
        case 8 :
            Console.Clear();
            TestRotationnel(image2);
            break;
    }
    Console.WriteLine("Voulez vous continuer (O/N) ?");
    choix = Console.ReadLine();
} while (choix == "O" || choix == "o");


static void Test_Grayscale(MyImage image_originale)
{
    MyImage image = image_originale.Grayscale();
    image.From_Image_To_File("images/Sortie.bmp");
    Console.WriteLine("regardez l'image en noir et blanc");
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
    double[,] sobelHorizontal = new double[,]
    {
        { -1, -2, -1 },
        { 0, 0, 0 },
        { 1, 2, 1 }
    };
    Console.WriteLine("Voici les matrices de convolution disponibles :");
    Console.WriteLine("1. Sobel Horizontal \n2. Sobel Vertical \n3. Matrice Flou \n4. Détection de contours");
    Console.WriteLine("Entrez le numéro de la matrice de convolution que vous voulez utiliser");
    int choix = Convert.ToInt32(Console.ReadLine());
    switch (choix)
    {
        case 1:
            image.AppliquerMatriceConvolution(sobelVertical).From_Image_To_File("images/Sortie.bmp");
            Console.WriteLine("regardez l'image convoluée");
            break;
        case 2:
            image.AppliquerMatriceConvolution(sobelHorizontal).From_Image_To_File("images/Sortie.bmp");
            Console.WriteLine("regardez l'image convoluée");
            break;
        case 3:
            image.AppliquerMatriceConvolution(MatriceFlou).From_Image_To_File("images/Sortie.bmp");
            Console.WriteLine("regardez l'image convoluée");
            break;
        case 4:
            image.AppliquerMatriceConvolution(detectioncontours).From_Image_To_File("images/Sortie.bmp");
            Console.WriteLine("regardez l'image convoluée");
            break;
    }
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
    Console.WriteLine("entrez Re(c)");
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
