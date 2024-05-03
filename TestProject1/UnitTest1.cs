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
            var image = new MyImage(new Pixel[,] { { new Pixel(255, 255, 255) } }, 1, 1);
            string filePath = "test.bmp";

            // Act
            image.From_Image_To_File(filePath);

            // Assert
            Assert.That(File.Exists(filePath), "Le fichier doit exister après l'exécution de From_Image_To_File.");
            File.Delete(filePath); // Cleanup
        }

    }
}