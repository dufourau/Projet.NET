using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testbis
{
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

                    Share action = new Share(null, t);
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
