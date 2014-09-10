using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Services
{
    internal class Optimization
    {

        //Dll import from wre-ensimag
        const string pathToDll = @"x64\wre-ensimag-c-4.1.dll";
        
        //Calculte the co-variance matrice
        [DllImport(pathToDll, EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]
        public static extern int NORMmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );

        
        //Use to solve portfolio optimization
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

        //Private enum use to swtich the measure use
        //By default we use a quadratique measure
        private Measure _measure;

        public Optimization()
        {
            _measure = Measure.MinStdDev;

        }
       
        //TODO MOVE INTO THE RIGHT DIRECTORY
        /// <summary>
        /// Launch our tracker and return all the weight value 
        /// </summary>
        /// <param name="returns"></param>
        /// <param name="durationTime"></param>
        /// <param name="waitingTime"></param>
        /// <returns></returns>
        public double[,] Tracking(double[,] returns,int estimationTime,int waitingTime)
        {
            int dataSize= returns.GetLength(0);
            int nbAssets= returns.GetLength(1)-1;
            double [] optimizedWeights= new double[nbAssets];
            // The final result: will contain the numbers of actions
            double[,] finalResult= new double[dataSize,nbAssets];
            // Matrix use to copy the current estimation data
            double[,] estimationValues= new double[estimationTime,nbAssets+1];
            // 
            for (int i = estimationTime; i < dataSize; i += waitingTime)
            {
                //Copy the estimation period
                for (int j = 0; j < estimationTime; j++)
                {
                    //Copy all assets
                    //TODO create functions to copy lines
                    for(int indexAsset= 0; indexAsset< nbAssets+1; indexAsset++){
                        estimationValues[indexAsset, j] = returns[indexAsset, i - j];
                    }   

                }
                //Calculate the optimized weight
                optimizedWeights= Optimize(estimationValues, 0);

                for (int j = i; j < i + waitingTime; j++ )
                {
                    for(int indexAsset= 0; indexAsset< nbAssets; indexAsset++){
                        finalResult[indexAsset,j]= optimizedWeights[indexAsset];
                    }
                }
                

            }
            return finalResult;
        }
        

        /// <summary>
        ///  Main function            
        ///  Call all the computing funcions and link he results     
        /// </summary>
        /// <param name="returns">matrix containings all the assets on the row (including the portfolio itself) and all the date on the columns</param>
        /// <param name="mu"> desired profit made with the portfolio</param>
        /// <returns>double array containing optimized weights</returns>
        public double[] Optimize(double[,] returns,double mu)
        {          
            double[,] CovarianceMatrix= ComputeCovarianceMatrix(returns);
            double[] expectedReturns= new double[returns.GetLength(1)];
            //We will calculate the rate mean of all rates and store in expectedReturns
            for (int i = 0; i < returns.GetLength(0); i++ )
            {
                for (int j = 0; j < returns.GetLength(1); j++ )
                {
                    expectedReturns[i]+=  returns[i, j];
                }
                expectedReturns[i] = expectedReturns[i] / returns.GetLength(1); 
            }
                  
            ComputeOptWeights(CovarianceMatrix, expectedReturns, mu);
            return null;
        }



        /// <summary>
        /// Compute the covariance matrix
        /// </summary>
        /// <param name="returns">matrix containings all the assets on the row (including the portfolio itself) and all the date on the columns</param>
        /// <returns>covariance matrix for the assets and the covariance vector of the portfolio will all the assets</returns> 
        public double[,] ComputeCovarianceMatrix(double[,] returns)

        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int returnFromNorm = NORMmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (returnFromNorm != 0)
            {

                // Check out what went wrong here
                throw new Exception(); 
            }
            return covMatrix;
        }

        /// <summary>
        /// Compute the optimals weight
        /// </summary>
        /// <param name="covMatrix"> the covaiance matrix (all assets + portfolio) </param>
        /// <param name="expectedReturns">vector of mean rate (all assets + portfolio)</param>
        /// <param name="mu">estimate profit</param>
        /// <returns>optimal weight</returns>
        public double[] ComputeOptWeights(double[,] covMatrix, double[] expectedReturns, double mu)

        {
            //Initialize all the parameters
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
            //Call the function from wre
            int returnFromOpt = OptWeights(ref nbAssets, cov, shareExpectedReturns, benchmarkCov, ref benchmarkExpectedReturn, ref nbEqConst, ref nbIneqConst, C, b, minWeights, maxWeights, ref mu, optimalWeights, ref info);

             if (returnFromOpt != 0)
            {

                throw new Exception(); // Check out what went wrong here
            }
            return optimalWeights;
        }



    }
}
