using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.Statistics;

namespace Zastosowanie_metod_sztucznej_inteligencji___projekt_1
{

    public class main
    {
        delegate double Funkcja1(params double[] X);
        Funkcja1 f;


        public static double rastriginFunction(params double[] X)
        {

            double sum = 0;
            int D = X.Length;
            for (int i = 0; i < D; i++)
            {
                double xi = X[i];
                sum += xi * xi - 10 * Math.Cos(2 * Math.PI * xi);
            }

            return 10 * D + sum;

        }

        public static double rosenbrockFunction(params double[] X)
        {

            double sum = 0;
            int D = X.Length;
            for (int i = 0; i < D; i++)
            {

                double xi = X[i];
                double xOneMore = X[i + 1];
                sum += 100 * Math.Pow(xOneMore - xi * xi, 2) + Math.Pow(1 - xi, 2);
            }

            return sum;

        }

        public static double sphereFunction(params double[] X)
        {

            double sum = 0;
            int D = X.Length;
            for (int i = 0; i < D; i++)
            {
                double xi = X[i];
                sum += xi * xi;
            }

            return sum;
        }

        public static double bealeFunction(params double[] X)
        {

            double x = X[0];
            double y = X[1];

            return Math.Pow(1.5 - x + x * y, 2) + Math.Pow(2.25 - x + x * y * y, 2) + Math.Pow(2.625 - x + x * y * y * y, 2);

        }

        public static double bukinFunctionN6(params double[] X)
        {

            double x = X[0];
            double y = X[1];

            return 100 * Math.Sqrt(Math.Abs(y - 0.01 * x * x)) + 0.01 * Math.Abs(x + 10);

        }


        public static double himmelblauFunctionN6(params double[] X)
        {

            double x = X[0];
            double y = X[1];


            return Math.Pow(x * x + y - 11, 2) + Math.Pow(x + y * y - 7, 2);

        }
        static void Main(string[] args)
        {
            Double[] X = { -2.3, 4.3 };
            Double[] Y = { 2.3, -4.3 };
                                                     //funkcja, wielkość populacji. zakres x, zakres y, ilość iteracji, wymiar     
            HarrisHawks harrisHawks = new HarrisHawks(sphereFunction, 10, X, Y, 20, 2);

            Double[] dataX = new double[10];
            Double[] dataY = new double[10];
            Double[] dataF = new double[10];

            for (int y = 0; y < 10; y++)
            {
                double[] result = harrisHawks.Solve();

                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine(result[i]);
                    

                }

                dataX[y] = result[0];
                dataY[y] = result[1];
                dataF[y] = result[2];
            }

            double standardDeviationX = Statistics.StandardDeviation(dataX);
            double meanX = Statistics.Mean(dataX);
            double standardDeviationY = Statistics.StandardDeviation(dataY);
            double meanY = Statistics.Mean(dataY);
            double standardDeviationF = Statistics.StandardDeviation(dataF);
            double meanF = Statistics.Mean(dataF);



            double minValue = double.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < dataF.Length; i++)
            {
                if (dataF[i] < minValue)
                {
                    minValue = dataF[i];
                    minIndex = i;
                }
            }

           


            try
            {
                
                StreamWriter sw = new StreamWriter("C:\\Users\\Pumpel\\Desktop\\wyniki.txt");

                sw.WriteLine("Funkcja testowa: Sphere");
                sw.WriteLine("Paramter beta: "+ 1.5);
                sw.WriteLine("Rozmiar populacji: "+10);
                sw.WriteLine("Liczba iteracji: " +20);

                sw.WriteLine("Odchylenie standardowe poszukiwanych parametrów: " + standardDeviationX+", "+ standardDeviationY);
                sw.WriteLine("Odchylenie standardowe wartości funkcji celu: " + standardDeviationF);

                sw.WriteLine("Współczynnik zmienności poszukiwanych parametrów: " + (standardDeviationX / meanX) + ", " + (standardDeviationY / meanY));
                sw.WriteLine("Współczynnik zmienności wartości funkcji celu: " + (standardDeviationF / meanF));


                sw.WriteLine("Wartość funkcji celu: " + dataF[minIndex]);
                sw.WriteLine("Znalezione minimum: " + dataX[minIndex] + ", " + dataY[minIndex]);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

        }

    }
}