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


        public double[] AveragePosition(double[,] array1)
        {
            int len = array1.GetLength(1);
            double[] avgX = new double[len];

            for (int ind = 0; ind < array1.Length; ind++)
            {
                for (int ind2 = 0; ind2 < len; ind2++)
                {
                    avgX[ind2] += array1[ind, ind2];
                }
            }
            double ar = array1.Length;
            for (int i = 0; i < array1.Length; i++)
            {
                avgX[i] /= array1.Length;
            }
            return avgX;
        }
    }
}
