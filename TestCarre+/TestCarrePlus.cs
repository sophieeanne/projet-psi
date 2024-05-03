using projet_psi;


namespace TestCarre_
{
    public class TestsComplex
    {
        [Test]
        public void CarrePlus_RetourneNombreComplexeCorrect()
        {
            // Initialisation
            var baseComplexe = new Complex(1, 1);  // (1 + i)
            var ajouterComplexe = new Complex(2, 3);   // (2 + 3i)

            // Résultats attendus de (1 + i)² + (2 + 3i)
            // (1 + i)² = 1² - 1² + 2*i*1 = 0 + 2i
            // Résultat = (0 + 2 + 2i + 3i) = 2 + 5i
            var resultatAttendu = new Complex(2, 5);

            // Action
            var resultat = baseComplexe.CarréPlus(ajouterComplexe);

            // Assertion
            Assert.That(resultat.Re, Is.EqualTo(resultatAttendu.Re), "Les parties réelles doivent être égales.");
            Assert.That(resultat.Im, Is.EqualTo(resultatAttendu.Im), "Les parties imaginaires doivent être égales.");
        }
    }
}