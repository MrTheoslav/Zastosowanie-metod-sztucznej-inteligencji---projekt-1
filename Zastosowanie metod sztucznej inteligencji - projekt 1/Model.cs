using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastosowanie_metod_sztucznej_inteligencji___projekt_1
{
    class Model
    {

        public int MinArrayIndex(double[] array)
        {
            double minValue = 99999999999;
            int indexOfMin = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < minValue)
                {
                    minValue = array[i];
                    indexOfMin = i;
                }
            }
            return indexOfMin;
        }


        public static double[] AveragePosition(double[,] array1)
        {
            int kol = array1.GetLength(1);
            int rows = array1.GetLength(0);
            double[] avgX = new double[kol];

            for (int ind = 0; ind < kol; ind++)
            {
                for (int ind2 = 0; ind2 < rows; ind2++)
                {
                    avgX[ind] += array1[ind2, ind];
                }
            }
            for (int i = 0; i < kol; i++)
            {
                avgX[i] /= rows;
            }
            return avgX;
        }
    }
}
