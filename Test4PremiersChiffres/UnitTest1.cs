using projet_psi;

namespace Test4PremiersChiffres
{
    public class Tests
    {
        private MyImage _monImage;

        [SetUp]
        public void Initialisation()
        {
            _monImage = new MyImage(); 
        }

        [Test]
        public void TestQuatrePremiersChiffres()
        {
            // Test pour la valeur 255, binaire 11111111, attendu : 15
            Assert.That(_monImage.quatre_premiers_chiffres(255), Is.EqualTo(15));
        }
    }
}
