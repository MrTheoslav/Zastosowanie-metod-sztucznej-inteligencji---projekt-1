using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.Statistics;
using System.Data;

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
                sum += (xi-0.5) * (xi-0.5);
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
            //dla paramteru 10
         

            List<Funkcja1> functions = new List<Funkcja1> { rastriginFunction };
            List<string> nameOfFunction = new List<string>{ "rastriginFunction" };
            Double[] Beta = {0.1,0.4,0.8,1.2,1.5};
            int[] SizeN = {10,30,60,100,150,200};
            int[] iterationT = {5,10,20,40,60,80,100,150,200};
            int[] Dimension = {2,3,5,8};
            int f = 0;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = "wyniki.csv";
            string filePath = Path.Combine(desktopPath, fileName);
            var table = new List<TableOfResults>();

            foreach (int D in Dimension)
            {
                Double[] Min = new Double[D];
                Double[] Max = new Double[D];
                for (int i = 0; i < D; i++)
                {
                    Min[i] = -5.12;
                    Max[i] = 5.12;
                }
               
                foreach (Funkcja1 f1 in functions)
                {


                    
                    
                    string name = nameOfFunction[0];
                    


                    foreach (int N in SizeN)
                    {

                        foreach (int T in iterationT)
                        {

                            foreach (Double B in Beta)
                            {



                                //funkcja, wielkość populacji. zakres x, zakres y, ilość iteracji, wymiar , beta    
                                HarrisHawks harrisHawks = new HarrisHawks(f1, N, Min, Max, T,D, B);

                                Double[,] data = new double[D+1,10];


                                for (int y = 0; y < 10; y++)
                                {
                                    double[] result = harrisHawks.Solve();

                                  

                                    for (int i = 0; i < result.Length; i++)
                                    {
                                        data[i,y] = result[i];
                                     
                                    }
                                }

                                Double[] toDeviation = new Double[10];
                                string StandartDeviationForParameters="";
                                string VariationCoefficientForParameter= "";
                                string StandartDeviationForFunction= "";
                                string VariationCoefficientForFunction= "";

                                for (int i = 0;i<=D; i++)
                                {
                                    for(int j = 0; j < 10; j++)
                                    {
                                        toDeviation[j] =data[i, j];

                                    }
                                    double standardDeviation = Statistics.StandardDeviation(toDeviation);
                                    double mean = Statistics.Mean(toDeviation);
                                    if(mean==0)
                                    {
                                        mean = 1;
                                    }
                                    double variationCoefficient = (standardDeviation / mean);

                                    if(i==D)
                                    {
                                        StandartDeviationForFunction =standardDeviation.ToString();
                                       VariationCoefficientForFunction=variationCoefficient.ToString();

                                    }
                                    else
                                    {
                                        if (i == D - 1)
                                        {
                                            StandartDeviationForParameters += standardDeviation.ToString() ;
                                            VariationCoefficientForParameter += variationCoefficient.ToString() ;

                                        }
                                        else
                                        {
                                            StandartDeviationForParameters += standardDeviation.ToString() + ", ";
                                            VariationCoefficientForParameter += variationCoefficient.ToString() + ", ";
                                        }
                                    }


                                }




                                Double[] toSeekTheBestMinimum = new Double[10];

                                for(int i = 0;i<10;i++)
                                {
                                    toSeekTheBestMinimum[i] = data[D,i];
                                }


                                //wybranie najmniejszej funkcji celu wraz jej X i Y
                                double minValue = double.MaxValue;
                                int minIndex = -1;

                                for (int i = 0; i < toSeekTheBestMinimum.Length; i++)
                                {
                                    if (toSeekTheBestMinimum[i] < minValue)
                                    {
                                        minValue = toSeekTheBestMinimum[i];
                                        minIndex = i;
                                    }
                                }

                                string minimumParametres = "";
                                for(int i = 0;i<D; i++)
                                {
                                    if (i == D - 1)
                                    {
                                        minimumParametres += data[i, minIndex] ;
                                    }
                                    else
                                    {
                                        minimumParametres += data[i, minIndex] + ", ";
                                    }
                                }

                                table.Add(new TableOfResults
                                {
                                    Algorithm = "HHO",
                                    TestFunction = name,
                                    NumberOfParameters = 1,
                                    Parameters = B,
                                    Iterator = T,
                                    Size = N,
                                    Minimum_X_Y = minimumParametres,
                                    StandartDeviationForParameters = StandartDeviationForParameters,
                                    VariationCoefficientForParameter = VariationCoefficientForParameter,
                                    ObjectiveFunction = data[D, minIndex].ToString(),
                                    StandartDeviationForFunction = StandartDeviationForFunction,
                                    VariationCoefficientForFunction = StandartDeviationForFunction,
                                    Dimension = D,
                                    ParameterForRastrigiFunction = 0,
                                }) ;

                            }
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