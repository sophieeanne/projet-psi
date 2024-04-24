using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projet_psi
{
    internal class JpegEncoder
    {
        // Constantes pour la taille des blocs et d'autres configurations
        private const int BlockSize = 8;

        // Tables de quantification, définies globalement dans la classe
        private readonly int[,] luminanceQuantTable; // À initialiser avec les valeurs standard ou personnalisées
        private readonly int[,] chrominanceQuantTable; // Idem

        public JpegEncoder()
        {
            // Initialisation de la table de quantification pour la luminance
            luminanceQuantTable = new int[,]
            {
            { 16, 11, 10, 16, 24, 40, 51, 61 },
            { 12, 12, 14, 19, 26, 58, 60, 55 },
            { 14, 13, 16, 24, 40, 57, 69, 56 },
            { 14, 17, 22, 29, 51, 87, 80, 62 },
            { 18, 22, 37, 56, 68, 109, 103, 77 },
            { 24, 35, 55, 64, 81, 104, 113, 92 },
            { 49, 64, 78, 87, 103, 121, 120, 101 },
            { 72, 92, 95, 98, 112, 100, 103, 99 }
            };

            // Initialisation de la table de quantification pour la chrominance
            chrominanceQuantTable = new int[,]
            {
            { 17, 18, 24, 47, 99, 99, 99, 99 },
            { 18, 21, 26, 66, 99, 99, 99, 99 },
            { 24, 26, 56, 99, 99, 99, 99, 99 },
            { 47, 66, 99, 99, 99, 99, 99, 99 },
            { 99, 99, 99, 99, 99, 99, 99, 99 },
            { 99, 99, 99, 99, 99, 99, 99, 99 },
            { 99, 99, 99, 99, 99, 99, 99, 99 },
            { 99, 99, 99, 99, 99, 99, 99, 99 }
            };
        }

        // Méthode pour appliquer la DCT à un bloc 8x8
        public double[,] ApplyDCT(double[,] block)
        {
            double[,] dctTransform = new double[BlockSize, BlockSize];
            double c1 = Math.Sqrt(2.0 / BlockSize);
            double c2 = 1.0 / Math.Sqrt(2.0);
            double temp;

            for (int u = 0; u < BlockSize; u++)
            {
                for (int v = 0; v < BlockSize; v++)
                {
                    temp = 0.0;
                    for (int x = 0; x < BlockSize; x++)
                    {
                        for (int y = 0; y < BlockSize; y++)
                        {
                            temp += block[x, y] *
                                    Math.Cos((2 * x + 1) * u * Math.PI / (2.0 * BlockSize)) *
                                    Math.Cos((2 * y + 1) * v * Math.PI / (2.0 * BlockSize));
                        }
                    }

                    temp *= ((u == 0) ? c2 : c1) * ((v == 0) ? c2 : c1);
                    dctTransform[u, v] = temp;
                }
            }
            return dctTransform;
        }

        // Méthode pour quantifier un bloc DCT
        public int[,] QuantizeBlock(double[,] dctCoefficients, int[,] quantizationTable)
        {
            int[,] quantizedCoefficients = new int[BlockSize, BlockSize];

            for (int i = 0; i < BlockSize; i++)
            {
                for (int j = 0; j < BlockSize; j++)
                {
                    quantizedCoefficients[i, j] = (int)(dctCoefficients[i, j] / quantizationTable[i, j]);
                }
            }
            return quantizedCoefficients;
        }
    }
}
