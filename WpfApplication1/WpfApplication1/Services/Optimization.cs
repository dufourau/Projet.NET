﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Services
{
    internal class Optimization
    {
<<<<<<< HEAD
        const string pathToDll = @"Path\To\x64\Or\X86\wre-ensimag-c-4.1.dll";

=======
        //Dll import from wre-ensimag
        const string pathToDll = @"Path\To\x64\Or\X86\wre-ensimag-c-4.1.dll";
        
        //Calculte the co-variance matrice
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
        [DllImport(pathToDll, EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]
        public static extern int NORMmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );
<<<<<<< HEAD

=======
        
        //Use to solve portfolio optimization
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
        [DllImport(pathToDll, EntryPoint = "WREallocIT", CallingConvention = CallingConvention.Cdecl)]
        public static extern int OptWeights(
            ref int nbAssets,
            double[,] cov,
            double[] expectedReturns,
            double[] benchmarkCov,
            ref double benchmarkExpectedReturn,
            ref int nbEqConst,
            ref int nbIneqConst,
            double[,] C,
            double[] b,
            double[] minWeights,
            double[] maxWeights,
            ref double relativeTargetReturn,
            double[] optimalWeights,
            ref int info
        );

<<<<<<< HEAD
=======
        //Private enum use to swtich the measure use
        //By default we use a quadratique measure
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
        private Measure _measure;

        public Optimization()
        {
            _measure = Measure.MinStdDev;

        }

<<<<<<< HEAD
=======
        /// <summary>
        ///  Main function            
        ///  Call all the computing funcions and link he results
        ///  INPUT: >> double [,] returns: matrix containings all the assets on the row (including the portfolio itself) and all the date on the columns
        ///         >> double mu : desired profit made with the portfolio       
        ///  OUTPUT: double array containing optimized weights
        /// </summary>
        public double[] Optimize(double[,] returns,double mu)
        {          
            double[,] CovarianceMatrix= computeCovarianceMatrix(returns);
            double[] expectedReturns= new double[returns.GetLength(1)];
            computeOptWeights(CovarianceMatrix, expectedReturns, mu);
            return null;
        }



        /*
         * Compute the covariance matrix
         * INPUT: 
         * OUTPUT: 
         */
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
        public double[,] computeCovarianceMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int returnFromNorm = NORMmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (returnFromNorm != 0)
            {
<<<<<<< HEAD

                throw new Exception(); // Check out what went wrong here
=======
                // Check out what went wrong here
                throw new Exception(); 
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
            }
            return covMatrix;
        }

<<<<<<< HEAD
        public double[] computeOptWeights(double[,] covMatrix, double[] expectedReturns, double mu)
        {
=======
        /*
         * Compute the optimals weight
         * INPUT: 
         * OUTPUT: 
         */
        public double[] computeOptWeights(double[,] covMatrix, double[] expectedReturns, double mu)
        {
            //Initialize all the parameters
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
            int nbAssets = covMatrix.GetLength(0)-1;
            double[] benchmarkCov= new double[nbAssets];
            double benchmarkExpectedReturn;
            int nbEqConst= 1;
            int nbIneqConst= 0;
            double[,] C= new double[nbAssets,1];
            double[] b= new double[nbAssets];
            double[] minWeights= new double[nbAssets];
            double[] maxWeights= new double[nbAssets];
            double[] shareExpectedReturns= new double[nbAssets];
            double relativeTargetReturn;
            double[,] cov= new double[nbAssets,nbAssets];
            
            for (int i = 0; i < nbAssets; i++ )
            {
                maxWeights[i]= 1;
                minWeights[i]= 0;
                C[i, 1]= 1;
                b[i]= 1;
                benchmarkCov[i] = covMatrix[nbAssets, i];       
                shareExpectedReturns[i]= expectedReturns[i];
                for(int j = 0; j < nbAssets; j++ ){
                    cov[i,j]= covMatrix[i, j];
                }
            }
           

            benchmarkExpectedReturn = expectedReturns.Last();
            relativeTargetReturn= mu;
            int info = 0;

            double[] optimalWeights = new double[nbAssets];
<<<<<<< HEAD
=======

            //Call the function from wre
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
            int returnFromOpt = OptWeights(ref nbAssets, cov, shareExpectedReturns, benchmarkCov, ref benchmarkExpectedReturn, ref nbEqConst, ref nbIneqConst, C, b, minWeights, maxWeights, ref mu, optimalWeights, ref info);

             if (returnFromOpt != 0)
            {
<<<<<<< HEAD

                throw new Exception(); // Check out what went wrong here
=======
                // Check out what went wrong here
                throw new Exception(); 
>>>>>>> 1a7cabe2d6d985c584c9d916840ad289fafbaa40
            }
            return optimalWeights;
        }



    }
}
