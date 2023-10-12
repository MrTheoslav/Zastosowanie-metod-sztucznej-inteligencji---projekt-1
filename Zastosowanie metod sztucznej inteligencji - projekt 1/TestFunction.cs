using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastosowanie_metod_sztucznej_inteligencji___projekt_1
{
    
    public class TestFunction
    {

        public static double rastriginFunction()
        {
            int dimension = 2;
            double[] x = new double[dimension];

            Random random = new Random();
            for(int i =0; i < dimension; i++)
            {
                x[i] = (random.NextDouble()*10.24)-5.12;
            }

            double sum = 0;

            for (int i=0; i < dimension;i++)
            {
                double xi = x[i];
                sum += xi * xi - 10 * Math.Cos(2 * Math.PI * xi);
            }

            return 10 * dimension + sum;

        }


        
        public static double rosenbrockFunction()
        {

            int dimension = 2;
            double[] x = new double[dimension];

            Random random = new Random();
            for (int i = 0; i < dimension; i++)
            {
                x[i] = random.NextDouble() * 10; //can be changed
            }

            double sum = 0;

            for (int i =0; i<dimension; i++)
            {

                double xi = x[i];
                double xOneMore = x[i + 1];
                sum += 100 * Math.Pow(xOneMore - xi * xi, 2) + Math.Pow(1 - xi, 2);
            }

            return sum;

        }

        public static double sphereFunction()
        {
            int dimension = 2;
            double[] x = new double[dimension];

            Random random = new Random();
            for (int i = 0; i < dimension; i++)
            {
                x[i] = random.NextDouble() * 7; //can be changed
            }

            double sum = 0;

            for (int i = 0; i < dimension; i++)
            {
                double xi = x[i];
                sum += xi * xi;
            }

            return sum;
        }


        public static double bealeFunction()
        {
            Random random = new Random();
            double x = (random.NextDouble() * 9) - 4.5;
            double y = (random.NextDouble() * 9) - 4.5;

            return Math.Pow(1.5 - x + x * y, 2) + Math.Pow(2.25 - x + x * y * y, 2) + Math.Pow(2.625 - x + x * y * y * y, 2);

        }

        public static double bukinFunctionN6()
        {
            Random random = new Random();
            double x = (random.NextDouble() *3 ) -9;
            double y = (random.NextDouble() * 6) - 3;

            return 100 * Math.Sqrt(Math.Abs(y - 0.01 * x * x)) + 0.01 * Math.Abs(x + 10);

        }


        public static double himmelblauFunctionN6()
        {
            Random random = new Random();
            double x = (random.NextDouble() * 10) - 5;
            double y = (random.NextDouble() * 10) - 5;


            return Math.Pow(x * x + y - 11, 2) + Math.Pow(x + y * y - 7, 2);

        }





    }
}
