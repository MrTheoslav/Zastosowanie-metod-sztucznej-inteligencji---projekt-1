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

        public static double rosenbrockFunction(params double[] X)
        {

            double sum = 0;
            int D = X.Length;
            for (int i = 0; i < D - 1; i++)
            {

                double xi = X[i];
                double xOneMore = X[i + 1];
                sum += 100 * Math.Pow(xOneMore - xi * xi, 2) + Math.Pow(1 - xi, 2);
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
            Double[,] FunctionsX = { { -2.3, 4.3 }, { -10, 10 }, { -4.5, 4.5 }, { -15, 5 }, { -5, 5 } };
            Double[,] FunctionsY = { { 2.3, -4.3 }, { -10, 10 }, { -4.5, 4.5 }, { -3, 3 }, { -5, 5 } };

            List<Funkcja1> functions = new List<Funkcja1> { sphereFunction, rosenbrockFunction, bealeFunction, bukinFunctionN6, himmelblauFunctionN6 };
            List<string> nameOfFunction = new List<string>
            {   "sphereFunction","rosenbrockFunction", "bealeFunction", "bukinFunctionN6", "himmelblauFunctionN6"

            };
            Double[] Beta = { 0.3, 0.6, 1.0, 1.2, 1.4, 1.5 };
            int[] SizeN = { 10, 20, 40, 80, 100 };
            int[] iterationT = { 5, 10, 20, 40, 60, 80, 100 };

            int f = 0;

            foreach (Funkcja1 f1 in functions)
            {
                Double[] X = { FunctionsX[f, 0], FunctionsX[f, 1] };
                Double[] Y = { FunctionsY[f, 0], FunctionsY[f, 1] };
                Console.WriteLine(f);
                string name = nameOfFunction[f];
                f++;


                foreach (int N in SizeN)
                {

                    foreach (int T in iterationT)
                    {

                        foreach (Double B in Beta)
                        {



                            //funkcja, wielkość populacji. zakres x, zakres y, ilość iteracji, wymiar , beta    
                            HarrisHawks harrisHawks = new HarrisHawks(f1, N, X, Y, T, 2, B);

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

                            //obliczanie odchylenia standardowego, wspólczynika odchylenia standardowego
                            double standardDeviationX = Statistics.StandardDeviation(dataX);
                            double meanX = Statistics.Mean(dataX);
                            double standardDeviationY = Statistics.StandardDeviation(dataY);
                            double meanY = Statistics.Mean(dataY);
                            double standardDeviationF = Statistics.StandardDeviation(dataF);
                            double meanF = Statistics.Mean(dataF);


                            //wybranie najmniejszej funkcji celu wraz jej X i Y
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

                            if (meanF == 0)
                            {
                                meanF = 1;
                            }
                            if (meanX == 0)
                            {
                                meanX = 1;
                            }
                            if (meanY == 0)
                            {
                                meanY = 1;
                            }
                            table.Add(new TableOfResults
                            {
                                Algorithm = "HHO",
                                TestFunction = name,
                                NumberOfParameters = 1,
                                Parameters = B,
                                Iterator = T,
                                Size = N,
                                Minimum = dataX[minIndex] + ", " + dataY[minIndex],
                                StandartDeviationForParameters = standardDeviationX + ", " + standardDeviationY,
                                VariationCoefficientForParameter = (standardDeviationX / meanX) + ", " + (standardDeviationY / meanY),
                                ObjectiveFunction = dataF[minIndex].ToString(),
                                StandartDeviationForFunction = standardDeviationF.ToString(),
                                VariationCoefficientForFunction = (standardDeviationF / meanF).ToString()
                            });

                        }
                    }
                }
            }

            //zapis do pliku  
            try
            {
                var save = new SaveToCSV();
                save.Save(filePath, table);
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