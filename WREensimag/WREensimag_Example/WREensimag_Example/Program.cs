using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WREensimag_Example
{
    class Program
    {

        const string pathToDll = @"Path\To\x64\Or\X86\wre-ensimag-c-4.1.dll";


        [DllImport(pathToDll, EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]
        public static extern int NORMmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );

        public static double[,] computeCovarianceMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int returnFromNorm = NORMmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (returnFromNorm != 0)
            {

                throw new Exception(); // Check out what went wrong here
            }
            return covMatrix;
        }

        static void Main(string[] args)
        {

            double[,] returns = { {0.05, -0.1, 0.6}, {-0.001, -0.4, 0.56}, {0.7, 0.001, 0.12}, {-0.3, 0.2, -0.1},
                                {0.1, 0.2, 0.3}};

            double[,] myCovMatrix = computeCovarianceMatrix(returns);
            Console.WriteLine("Covariance matrix:");
            for (int i = 0; i < myCovMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < myCovMatrix.GetLength(0); j++)
                {
                    Console.WriteLine("Cov(" + i + "," + j + ")=" + myCovMatrix[i, j]);

                }
            }
            Console.WriteLine("Type enter to exit");
            Console.Read();
        }
    }
}
