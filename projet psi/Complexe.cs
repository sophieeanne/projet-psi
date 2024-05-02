using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projet_psi
{
    public class Complex
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }

        /// <summary>
        /// Multiplie ce complexe par lui-même et ajoute un autre complexe.
        /// </summary>
        public Complex CarréPlus(Complex c)
        {
            double newRe = Re * Re - Im * Im + c.Re;
            double newIm = 2 * Re * Im + c.Im;
            return new Complex(newRe, newIm);
        }

        /// <summary>
        /// Calcule le modul au carré du complexe pour optimiser les calculs.
        /// </summary>
        public double ModuleCarré()
        {
            return Re * Re + Im * Im;
        }

    }
}
