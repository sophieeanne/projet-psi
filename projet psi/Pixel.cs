using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projet_psi
{
    using System;

    public class Pixel
    {
        byte red;
        byte green;
        byte blue;

        public Pixel(byte r, byte g, byte b)
        {
            this.red = r;
            this.green = g;
            this.blue = b;
        }
        public byte R
        {
            get { return red; }
        }
        public byte G
        {
            get { return green; }
        }
        public byte B
        {
            get { return blue; }
        }
        public string toString()
        {
            return "red : " + red + "\ngreen : " + green + "\nblue : " + blue;
        }
        public static bool operator ==(Pixel p, Pixel p1)
        {
            return p.Equals(p1);
        }
        public static bool operator !=(Pixel p, Pixel p1)
        {
            return !p.Equals(p1);
        }

        public bool Equals(Pixel p)
        {
            return this.red == p.red && this.green == p.green && this.blue == p.blue;
        }
        public static bool Equals(Pixel p1, Pixel p2)
        {
            return p1.red == p2.red && p1.green == p2.green && p1.blue == p2.blue;
        }
    }
}
