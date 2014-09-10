using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testbis
{
    public struct correspondance
    {
       public string _name;
       public string _ticker;

        public correspondance(string name, string ticker)
        {
            _name = name;
            _ticker = ticker;
        }
    }

    class Requete
    {
        /// <summary>
        /// Initialise l'application
        /// </summary>
        /// <returns>Le portefeuille du client initialisé avec les 29 actions de poids 1/29, avec historique de leur taux</returns>
        public Portfolio initialisation()
        {
            using (DataDataContext dc = new DataDataContext())
            {

                Boolean b = false;
                List<correspondance> donnees = new List<correspondance>();
                //correspondances ticker/name
                if (File.Exists("Z:/Documents/Cours-3A/NET/Correspondances.txt"))
                {

                    StreamReader monStreamReader = new StreamReader("Z:/Documents/Cours-3A/NET/Correspondances.txt");
                    string ligne = monStreamReader.ReadLine();
                    char[] sep = { '\t' };
                    

                    while ((ligne = monStreamReader.ReadLine()) != null)
                    {
                        //nom
                        string[] tab;
                        tab = ligne.Split(sep);


                        donnees.Add(new correspondance(tab[0], tab[1]));
                    }

                    monStreamReader.Close();
                    b = true;
                }
                //récupération de l'indice reference cac40
                var cac = (from c in dc.HistoIndices
                           select c.name).Distinct().Single();

                //création de l'action cac40
                Share cac40 = new Share("CAC-40", cac);

                var valeur = from v in dc.HistoIndices
                             where v.name == cac
                             select new { v.value, v.date };

                //assignation de ses taux au cours du temps
                foreach (var v in valeur)
                    {
                        cac40.addRate(v.date, v.value);

                    }
   
                //création du pf clien référencé sur l'indice du cac40
                Portfolio P = new Portfolio("Porte-feuille client", cac40);

                //ajout des 29 actions du cac40 dans le portefeuille au poids 0
                var ticker = (from t in dc.HistoComponents
                             select t.name).Distinct();

                foreach (string t in ticker) //pour toutes les actions
                {
                    var valeur2 = from v in dc.HistoComponents 
                                 where v.name == t 
                                 select new {v.value, v.date};
                    Share action;
                    if (b)
                    {
                        string name = (from d in donnees
                                       where d._ticker == t
                                       select d._name).Single().ToString();

                        action = new Share(name, t);
                    }
                    else
                    {
                        action = new Share(null, t);
                    }
                    foreach (var v in valeur2)
                    {
                        action.addRate(v.date, v.value);

                    }
                    P.addAction(action,(double)1/(double)29);
                }

                











                return P;
            }
        }

    }
}
