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

    /// <summary>
    /// ajoute (en fin de liste) un cours pour l'action
    /// </summary>
    /// <param name="date"></param>
    /// <param name="rate"></param>
    public void addRate(DateTime date, double rate)
    {
        this._rates.Add(new Rate(date, rate));
    }

    public float getRate(DateTime date)
    {
        //return this.rates.Find(   //(x => x.date = date, x => x.action = this.name);
        return 0;
    }

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
