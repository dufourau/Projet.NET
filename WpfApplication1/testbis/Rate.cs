using System;

public class Rate
{

    private DateTime _date;
    private double _rate;

    public Rate(DateTime date, double rate)
	{
        this._rate = rate;
        this._date = date;
	}

    public DateTime _Date
    {
        get { return _date; }
        private set { _date = value; }
    }

    public double _Rate
    {
        get { return _rate; }
        private set { _rate = value; }
    }

}
