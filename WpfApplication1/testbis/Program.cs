using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testbis
{

    class Program
    {
        static void Main(string[] args)
        {

          /*  Share action = new Share("CAC40", "CAC4");
            System.Console.WriteLine("hello");
            Portfolio P = new Portfolio("pf", action);

           Share action1 = new Share("oreal", "O");
            Share action2 = new Share("renault", "R");
            Share action3 = new Share("danone", "D");

            P.addAction(action1, 0.5);
            P.addAction(action2, 0.52);

            foreach (composition c in P._Actions)
            {
                System.Console.WriteLine(c._action._Name);
                System.Console.WriteLine(c._weight);
            }

            P.setWeight(action2, 0.3);
            P.addAction(action3, 1);

            foreach (composition c in P._Actions)
            {
                System.Console.WriteLine(c._action._Name);
                System.Console.WriteLine(c._weight);
            }
            

            System.Console.WriteLine("hello");*/
             Requete r = new Requete();
             Portfolio P = r.initialisation();


             List<Share> liste = P.GetAllShares();
            foreach (Share a in liste)
            {
                System.Console.WriteLine(a._Name);
                /*List<double> rates = a.GetAllRates();

                foreach (double d in rates)
                {
                    System.Console.WriteLine(d);
                }*/

            }
            
            DateTime di = new DateTime(2006,01,02);
            double Res = P.PortfolioRate(di);
            System.Console.WriteLine("valeur pf :");
            System.Console.WriteLine(Res);
            System.Console.WriteLine(Res);
        }
    }
}
