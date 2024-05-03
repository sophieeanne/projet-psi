using projet_psi;

namespace TestProject1
{
    [TestFixture]
    public class MyImageTests
    {
        [Test]
        public void TestFromImageToFile()
        {
            // Arrange
            MyImage image = new MyImage(new Pixel[,] { { new Pixel(255, 255, 255) } }, 1, 1);
            string filePath = "test.bmp";

            // Act
            image.From_Image_To_File(filePath);

            // Assert
            Assert.That(File.Exists(filePath), "Le fichier doit exister après l'exécution de From_Image_To_File.");
            File.Delete(filePath); // Cleanup
        }
        [Test]

        public void TestDecoderImage2()
        {
            // Arrange
            MyImage image = new MyImage(new Pixel[,] { { new Pixel(255, 255, 255) } }, 1, 1);
            string filePath = "test.bmp";
            image.From_Image_To_File(filePath);
            MyImage image2 = new MyImage(new Pixel[,] { { new Pixel(255, 255, 255) } }, 1, 1);
            // Act
            image2.Decoder_Image2(image);

            // Assert
            Assert.That(image2, Is.EqualTo(image), "Les deux images doivent être égales.");
            File.Delete(filePath); // Cleanup
        }   
        public void TestDecimalVersBinaire()
        {
            // Arrange
            int decimalNumber = 255;
            MyImage imageTest = new MyImage(new Pixel[,] { { new Pixel(255, 255, 255) } }, 1, 1);
            // Act
            int binaryNumber = imageTest.Decimal_Vers_Binaire(decimalNumber);

            // Assert
            Assert.That(binaryNumber, Is.EqualTo("11111111"), "La conversion de 255 en binaire doit donner 11111111.");
        }
        

    }
}