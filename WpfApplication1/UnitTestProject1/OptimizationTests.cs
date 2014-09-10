using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApplication1.Services;

namespace ServicesTests
{
    [TestClass]
    public class OptimizationTests
    {
        [TestMethod]
        public void TestOptimize()
        {

        }

        [TestMethod]
        public void TestcomputeCovarianceMatrix()
        {

        }

        static int iset = 0;
        static double gset;
        public double GaussSimu() {

            double u1, u2, r, theta;
            Random rand;
            if (iset == 0){
                rand = new Random();
                u1 = rand.NextDouble();
                u2 = rand.NextDouble();
                r = Math.Sqrt(-2 * Math.Log(u1));
                theta = 2 * Math.PI * u2;
                gset = r * Math.Cos(theta);
                iset = 1;
                return r * Math.Sin(theta);
            }else{
                iset = 0;
                return gset;
            }
        }

        public double SimuBS(double S, double T, double r, double v){
            return S * Math.Exp((0.25) * T + v * Math.Sqrt(T) * GaussSimu());
        }

        public double[,] GenerateReturns(double init, int nbAssets, int nbDates)
        {
            double r = 0.001; 
            double T = (double)nbDates/365.0;
            double v = 0.2;
            double[,] returns = new double[nbDates,nbAssets];
            for (int j = 0; j < nbAssets; j++)
            {
                returns[0, j] = init/(double)nbAssets;
            }
            for (int i = 1; i < nbDates; i++)
            {
                for (int j = 0; j < nbAssets; j++)
                {
                    returns[i, j] = SimuBS(returns[i - 1, j], T, r, v);
                }
            }
            return returns;
        }

        [TestMethod]
        public void TestcomputeOptWeights()
        {
            Optimization opt = new Optimization();
            double[,] returns = new double[2,2];
            double[,] covMatrix = opt.ComputeCovarianceMatrix(returns);
            double[] expectedReturns = {0};
            double mu = 0;
            double[] OptW = opt.ComputeOptWeights(covMatrix, expectedReturns, mu);
        }
    }
}
