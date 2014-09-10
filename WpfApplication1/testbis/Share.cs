using System;
using System.Collections.Generic;

public class Share
{
    private string _name;
    private string _ticker;
    private List<Rate> _rates;

    public Share(string name, string ticker)
	{
        this._name = name;
        this._ticker = ticker;
        this._rates = new List<Rate>();
	}

    public string _Name
    {
        get { return _name; }
        private set { _name = value; }
    }

    public string _Ticker
    {
        get { return _ticker; }
        private set { _ticker = value; }
    }

    public List<Rate> _Rates
    {
        get { return _rates; }
        private set { _rates = value; }
    }


    /// <summary>
    /// ajoute (en fin de liste) un cours pour l'action
    /// </summary>
    /// <param name="date">date du cours</param>
    /// <param name="rate">cours</param>
    public void addRate(DateTime date, double rate)
    {
        this._rates.Add(new Rate(date, rate));
    }

    /// <summary>
    /// donne le cours de laction pour une date donnée
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public double getRate(DateTime date)
    {
        double res = 0;
        foreach (Rate r in this._rates)
        {
            if (r._Date == date)
            {
                res = r._Rate;
                return res;
            }
        }
        return res;
    }

    /// <summary>
    /// donne tous les taux concernant cette action par ordre chronologique
    /// </summary>
    /// <returns></returns>
    public List<double> GetAllRates()
    {
        List<double> res = new List<double>();
        foreach (Rate r in this._rates)
        {
            res.Add(r._Rate);
        }

        return res;
    }
}
