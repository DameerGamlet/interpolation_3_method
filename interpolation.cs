using System;
using System.Collections.Generic;

namespace Metodu_V_task1
{
    class Program
    {
        static double Finc(double x)
        {
            return (Math.Pow(x, 2) + 2 * x - 2);
        }
        struct CubeSpline
        {
            public double a, b, c, d, x;
        }
        static double Cube_SplineS(int n, double Xv, double[] MasX, double[] MasY, int t)
        {
            CubeSpline[] splinew = new CubeSpline[n];

            double ai, bi, ci, F, hi_minus, hi_plus, hy1, hy2;

            for (int i = 0; i < n; i++)
            {
                splinew[i].x = MasX[i];
                splinew[i].a = MasY[i];
            }
            splinew[0].c = splinew[n - 1].c = 0;
            double[] Si1 = new double[n];
            double[] Si2 = new double[n];
            Si1[0] = Si2[0] = 0;

            for (int i = 1; i < n - 1; ++i)
            {
                ai = hi_minus = MasX[i] - MasX[i - 1];
                bi = hi_plus = MasX[i + 1] - MasX[i];
                hy1 = (MasY[i] - MasY[i - 1]);
                hy2 = (MasY[i + 1] - MasY[i]);
                ci = (2 * (hi_minus + hi_plus));
                F = ((hy2 / (hi_plus - hy1)) / hi_minus * 6);
                Si1[i] = -bi / (ai * Si1[i - 1] + ci);
                Si2[i] = (F - ai * Si2[i - 1]) / (ai * Si1[i - 1] + ci);
               // if (t == 0)
               // {
               //     Console.Write("\nSI[{0}] = {1} + {2} * {3} + {4} * {3} * {3} + {5} * {3} * {3} * {3} =\t{6}", i, ai, bi, hi_minus, ci, F, ai + bi * hi_minus + ci * Math.Pow(hi_minus, 2) + F * Math.Pow(hi_minus, 3));
               //
               // }
            }
            // Console.WriteLine();
            for (int i = n - 2; i > 0; --i)
            {
                splinew[i].c = Si1[i] * splinew[i + 1].c + Si2[i];
            }
            for (int i = n - 1; i > 0; --i)
            {
                hi_minus = MasX[i] - MasX[i - 1];
                splinew[i].d = (splinew[i].c - splinew[i - 1].c) / hi_minus;
                splinew[i].b = ((hi_minus * (2.0 * splinew[i].c + splinew[i - 1].c)) / 6.0 + (MasY[i] - MasY[i - 1])) / hi_minus;
            }

            CubeSpline Spl;

            if (Xv <= splinew[0].x)
            {
                Spl = splinew[0];
            }
            else if (Xv >= splinew[n - 1].x)
            {
                Spl = splinew[n - 1];
            }

            else
            {
                int i = 0;
                int j = n - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (Xv <= splinew[k].x) { j = k; }
                    else { i = k; }
                }
                Spl = splinew[j];
            }

            return Spl.a + (Spl.b + (Spl.c / 2.0 + Spl.d * (Xv - Spl.x) / 6.0) * (Xv - Spl.x)) * (Xv - Spl.x);
        }

        static double Lagrange_Interpolation(int n, double Xv, double[] x, double[] y)
        {
            double resultL = 0;

            for (int i = 0; i < n; i++)
            {
                double SecResult = 1;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        SecResult *= (Xv - x[j]) / (x[i] - x[j]);
                    }
                }
                resultL += SecResult * y[i];
            }
            return resultL;

        }

        static double Newton_Interpolation(int n, double Xp, double[] MasX, double[] MasY)
        {
            double result = MasX[0];
            double Factorial;
            double Polinom;
            for (int i = 1; i < n; i++)
            {
                Factorial = 0;
                for (int j1 = 0; j1 <= i; j1++)
                {//следующее слагаемое полинома
                    Polinom = 1;
                    //считаем знаменатель разделенной разности
                    for (int j2 = 0; j2 <= i; j2++)
                    {
                        if (j2 != j1)
                        {
                            Polinom = Polinom * (MasX[j1] - MasX[j2]);
                        }
                    }
                    //считаем разделенную разность
                    Factorial = (Factorial + (MasY[j1] / Polinom));
                }
                //домножаем разделенную разность на скобки (x-x[0])...(x-x[i-1])
                for (int m = 0; m < i; m++)
                {
                    Factorial = Factorial * (Xp - MasX[m]);
                }
                result = result + Factorial;//полином
            }
            return result;
        }

        static void Main(string[] args)
        {
            Console.Write(" Введите значение n. \n n = ");
            int n = (int.Parse(Console.ReadLine()));
            Console.WriteLine(" Введите значения x[i] {0} раз:", n);
            int timer = 0;
            double[] masX = new double[n];
            double[] masY = new double[n];
            double[] masP = new double[n - 1];
            while (timer != n)
            {
                Console.Write(" x[{0}] = ", timer);
                masX[timer] = (double.Parse(Console.ReadLine()));
                timer++;
            }

            Console.WriteLine(" Значения y[i]:", n);
            timer = 0;
            while (timer != n)
            {
                Console.Write(" x[{0}] = {1}; ", timer, masX[timer]);
                Console.Write("      y[{0}] = ", timer);
                masY[timer] = Finc((double)masX[timer]);
                Console.Write(masY[timer] + "\n");
                timer++;
            }

            Console.WriteLine("\n Необходимо найти промежуточные значение, а именно ");
            timer = 0;
            int timer2 = 1;
            while (timer < n && timer2 != n)
            {
                Console.Write(" P[{0}, {1}] = ", timer, timer2);
                masP[timer] = ((masX[timer] + masX[timer2]) / 2);
                Console.WriteLine(masP[timer]);
                ++timer; ++timer2;
            }

            Console.WriteLine("\n Интерполяция ЛАНГРАНЖА ");
            
            timer = 0;
            timer2 = 1;
            while (timer < n && timer2 != n)
            {
                Console.Write(" F в точке P[{0}, {1}] = ", timer, timer2);
                masP[timer] = ((masX[timer] + masX[timer2]) / 2);
                Console.WriteLine(Lagrange_Interpolation(n, masP[timer], masX, masY));
                ++timer; ++timer2;
            }

            timer = 0;
            Console.WriteLine(" Интерполяция НЬЮТОНА ");
            
            timer = 0;
            timer2 = timer + 1;
            double step = 1;
            while (timer < n && timer2 != n)
            {
                Console.Write(" F в точке P[{0}, {1}] = ", timer, timer2);
                Console.WriteLine(Newton_Interpolation(n, masP[timer], masX, masY));
                ++timer; ++timer2;
            }

            timer = 0;
            Console.WriteLine(" Интерполяция КУБИЧЕСКИМИ СПЛАЙНАМИ ");
            
            timer = 0;
            timer2 = timer + 1;
            while (timer < n && timer2 != n)
            {
                Console.Write(" F в точке P[{0}, {1}] = ", timer, timer2);
                masP[timer] = ((masX[timer] + masX[timer2]) / 2);
                Console.WriteLine(Cube_SplineS(n, masP[timer], masX, masY, timer));
                ++timer; ++timer2;
            }
        }
    }
}
