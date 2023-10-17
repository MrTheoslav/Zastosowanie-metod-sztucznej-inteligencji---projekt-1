using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastosowanie_metod_sztucznej_inteligencji___projekt_1
{

    delegate double Funkcja1(params double[] x);
    
    class HarrisHawks 
    {
        Model _model = new Model();
        private Funkcja1 funkcja2 { get; set; }
        private int N { get; set; }

        private double [] limitX1 { get; set; }
        private double [] limitX2 { get; set; }
        private int T { get; set; }
        private int D { get; set; }


        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double[] XBest { get; set; }
        public double FBest { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int NumberOfEvaluationFitnessFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public HarrisHawks(Funkcja1 _funkcja2, int _N, double[] _limitX1, double[] _limitX2, int _T, int _D)
        {
            this.funkcja2 = _funkcja2;
            this.N = _N;
            this.limitX1 = _limitX1;
            this.limitX2 = _limitX2;
            this.T = _T;
            this.D = _D;
            // funkcja1 jest funkcja dla ktorej nalezy znalezc wartosc minimalna
            // N jest liczba jastrzebi
            // limitX1/2 i limitY1/2 oznaczają dziedzine z zakresu ktorej losowane sa pozycje jastrzebi
            // T oznacza maksymalna liczbe iteracji



        }
        public double LF()
        {
            Random rnd = new Random();
            double beta = 1.5;
            double delta = Math.Pow((((alglib.gammafunction(1+beta))*Math.Sin(Math.PI*beta/2.0))) / (alglib.gammafunction((1+beta)/2)*beta*Math.Pow(2, ((beta-1)/2))), (1/beta));

            double u = rnd.NextDouble();
            double v = rnd.NextDouble();

            double result = 0.01 * u * delta / Math.Pow(Math.Abs(v), (1 / beta));
            return result;
        }
        public double Solve()
        {
            Random rnd = new Random();
            double[] xRabbit = new double[D];

            // populacja jastrzebi [liczba jastrzebi, liczba wspolrzednych]
            // hawkFitness zawiera wartosci funkcji w punktach, w ktorych znajduja sie jastrzebie
            double[,] populationX = new double[N, D];
            double[] hawkFitness = new double[N];
            
            // uzupelniamy losowymi wspolrzednymi znajdujacymi sie w dziedzinie
            for (int i = 0; i < N; i++)
            {
                for (int i2 = 0; i2 < D; i2++)
                {
                    populationX[i, i2] = (rnd.NextDouble() * (limitX2[i2] - limitX1[i2])) + limitX1[i2];
                }
            }

            
            // kroki iteracji ------------------
            for (int t = 0; t < T; t++)
            {
                // tablica do przechowywania tymczasowych nowych wartosci
                // nie mozna bezposrednio nadpisac, poniewaz przy obliczeniach np sredniej caly czas potrzeba X(t)
                double[,] populationX1_tmp = new double[N, D];

                // uzupelnienie tablicy hawkFitness na podstawie wynikow funkcji po podstawieniu wspolrzednych jastrzebi
                for (int p1 = 0; p1 < N; p1++)
                {
                    double[] currentHawk = new double[D];
                    for (int i = 0; i < D; i++)
                    {
                        currentHawk[i] = populationX[p1, i];
                    }

                    hawkFitness[p1] = funkcja2(currentHawk);
                }

                // przypisanie najlepszej pozycji jako pozycja krolika
                int rabIndex = _model.MinArrayIndex(hawkFitness);
                for (int i2 = 0; i2 < D; i2++)
                {
                    xRabbit[i2] = populationX[rabIndex, i2];
                }


                // wykonanie ponizszych instrukcji nastepuje dla kazdego jastrzebia z populacji
                for (int hawk = 0; hawk < N; hawk++)
                {


                    double e_0 = 2 * rnd.NextDouble() - 1;
                    double J = 2 * (1 - rnd.NextDouble());

                    double E = 2 * e_0 * (1 - t / T);

                    // nowe wspolrzedne dla danego jastrzebia
                    double[] newX = new double[D];

                    // q - chance for perching strategy
                    double q = rnd.NextDouble();

                    double r1 = rnd.NextDouble();
                    double r2 = rnd.NextDouble();
                    double r3 = rnd.NextDouble();
                    double r4 = rnd.NextDouble();

                    // losujemy jastrzebia
                    int randHawk = rnd.Next(0, N);

                    if (Math.Abs(E) >= 1)
                    {
                        // faza -- Exploration

                        if (q >= 0.5)
                        {
                            for (int i3 = 0; i3 < D; i3++)
                            {
                                newX[i3] = populationX[randHawk, i3] - r1 * Math.Abs(populationX[randHawk, i3] - 2 * r2 * populationX[hawk, i3]);
                            }
                        }
                        else
                        {
                            for (int i3 = 0; i3 < D; i3++)
                            {
                                newX[i3] = (xRabbit[i3] - _model.AveragePosition(populationX)[i3]) - r3 * (limitX1[i3] + r4 * (limitX2[i3] - limitX1[i3]));
                            }

                        }

                        // przypisanie nowych wartosci -- tablica tmp czyli X(t + 1) poniewaz 
                        // obliczenia dla kolejnych iteracji caly czas kozystaja z poprzednich wartosci czyli X(t)
                        for (int j1 = 0; j1 < D; j1++)
                        {
                            if (newX[j1] < limitX1[j1])
                            {
                                // zabezpiecza przed wybieraniem pozycji spoza dziedziny (mniejsze)
                                newX[j1] = limitX1[j1];
                            }
                            if (newX[j1] > limitX2[j1])
                            {
                                // zabezpiecza przed wybieraniem pozycji spoza dziedziny (wieksze)
                                newX[j1] = limitX2[j1];
                            }

                            populationX1_tmp[hawk, j1] = newX[j1];
                        }
                    }


                    else
                    {
                        // faza -- Exploitation

                        double r = rnd.NextDouble();

                        double[] Y = new double[D];

                        if (r >= 0.5 && Math.Abs(E) >= 0.5)
                        {
                            // soft besiege
                            for (int i3 = 0; i3 < D; i3++)
                            {
                                newX[i3] = xRabbit[i3] - populationX[hawk, i3] - E * Math.Abs(J * xRabbit[i3] - populationX[hawk, i3]);
                            }
                            for (int j1 = 0; j1 < D; j1++)
                            {
                                if (newX[j1] < limitX1[j1])
                                {
                                    // zabezpiecza przed wybieraniem pozycji spoza dziedziny (mniejsze)
                                    newX[j1] = limitX1[j1];
                                }
                                if (newX[j1] > limitX2[j1])
                                {
                                    // zabezpiecza przed wybieraniem pozycji spoza dziedziny (wieksze)
                                    newX[j1] = limitX2[j1];
                                }

                                populationX1_tmp[hawk, j1] = newX[j1];
                            }

                        }
                        else if (r >= 0.5 && Math.Abs(E) < 0.5)
                        {
                            // hard besiege
                            for (int i3 = 0; i3 < D; i3++)
                            {
                                newX[i3] = xRabbit[i3] - E * Math.Abs(xRabbit[i3] - populationX[hawk, i3]);
                            }
                            for (int j1 = 0; j1 < D; j1++)
                            {
                                if (newX[j1] < limitX1[j1])
                                {
                                    // zabezpiecza przed wybieraniem pozycji spoza dziedziny (mniejsze)
                                    newX[j1] = limitX1[j1];
                                }
                                if (newX[j1] > limitX2[j1])
                                {
                                    // zabezpiecza przed wybieraniem pozycji spoza dziedziny (wieksze)
                                    newX[j1] = limitX2[j1];
                                }

                                populationX1_tmp[hawk, j1] = newX[j1];
                            }
                        }
                        else if (r < 0.5 && Math.Abs(E) >= 0.5)
                        {
                            //  Soft besiege with progressive rapid dives

                            for (int i3 = 0; i3 < D; i3++)
                            {
                                Y[i3] = xRabbit[i3] - E * Math.Abs(J * xRabbit[i3] - populationX[hawk, i3]);
                            }
                            double[] S = new double[D];
                            for (int i2 = 0; i2 < D; i2++)
                            {
                                S[i2] = (rnd.NextDouble() * (limitX2[i2] - limitX1[i2])) + limitX1[i2];
                            }

                            double[] Z = new double[D];
                            for (int i3 = 0; i3 < D; i3++)
                            {
                                Z[i3] = Y[i3] + S[i3] * LF();
                            }

                            // wartosci obecnego jastrzebia
                            double[] currentHawk = new double[D];
                            for (int i4 = 0; i4 < D; i4++)
                            {
                                currentHawk[i4] = populationX[hawk, i4];
                            }

                            if (funkcja2(Y) < funkcja2(currentHawk))
                            {
                                for (int j1 = 0; j1 < D; j1++)
                                {
                                    if (Y[j1] < limitX1[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (mniejsze)
                                        Y[j1] = limitX1[j1];
                                    }
                                    if (Y[j1] > limitX2[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (wieksze)
                                        Y[j1] = limitX2[j1];
                                    }
                                    populationX1_tmp[hawk, j1] = Y[j1];
                                }
                            }
                            else if(funkcja2(Y) < funkcja2(currentHawk))
                            {
                                for (int j1 = 0; j1 < D; j1++)
                                {
                                    if (Z[j1] < limitX1[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (mniejsze)
                                        Z[j1] = limitX1[j1];
                                    }
                                    if (Z[j1] > limitX2[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (wieksze)
                                        Z[j1] = limitX2[j1];
                                    }

                                    populationX1_tmp[hawk, j1] = Z[j1];
                                }
                            }
                        }



                        else if (r < 0.5 && Math.Abs(E) < 0.5)
                        {
                            //  Hard besiege with progressive rapid dives
                            for (int i3 = 0; i3 < D; i3++)
                            {
                                Y[i3] = xRabbit[i3] - E * Math.Abs(J * xRabbit[i3] - _model.AveragePosition(populationX)[i3]);
                            }

                            double[] S = new double[D];
                            for (int i2 = 0; i2 < D; i2++)
                            {
                                S[i2] = (rnd.NextDouble() * (limitX2[i2] - limitX1[i2])) + limitX1[i2];
                            }
                            double[] Z = new double[D];
                            for (int i3 = 0; i3 < D; i3++)
                            {
                                Z[i3] = Y[i3] + S[i3] * LF();
                            }


                            // wartosci obecnego jastrzebia
                            double[] currentHawk = new double[D];
                            for (int i4 = 0; i4 < D; i4++)
                            {
                                currentHawk[i4] = populationX[hawk, i4];
                            }

                            if (funkcja2(Y) < funkcja2(currentHawk))
                            {
                                for (int j1 = 0; j1 < D; j1++)
                                {

                                    if (Y[j1] < limitX1[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (mniejsze)
                                        Y[j1] = limitX1[j1];
                                    }
                                    if (Y[j1] > limitX2[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (wieksze)
                                        Y[j1] = limitX2[j1];
                                    }

                                    populationX1_tmp[hawk, j1] = Y[j1];
                                }
                            }
                            else if (funkcja2(Y) < funkcja2(currentHawk))
                            {
                                for (int j1 = 0; j1 < D; j1++)
                                {

                                    if (Z[j1] < limitX1[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (mniejsze)
                                        Z[j1] = limitX1[j1];
                                    }
                                    if (Z[j1] > limitX2[j1])
                                    {
                                        // zabezpiecza przed wybieraniem pozycji spoza dziedziny (wieksze)
                                        Z[j1] = limitX2[j1];
                                    }

                                    populationX1_tmp[hawk, j1] = Z[j1];
                                }
                            }
                        }


                    }

                }

                for (int hawkNum = 0; hawkNum < N; hawkNum++) {
                    for (int ind = 0; ind < D; ind++)
                    {
                        populationX[hawkNum, ind] = populationX1_tmp[hawkNum, ind];
                    }
                }
            }

            for (int a = 0; a < D; a++)
            {
                XBest[a] = xRabbit[a];
            }

            return funkcja2(xRabbit);
        }








        

        


    }
}
