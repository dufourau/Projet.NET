using System;
using System.Collections.Generic;
using System.Linq;

public struct composition
{
    public Share _action;
    public double _weight;

    public void  setWeight(double weight)
    {
        _weight = weight;
    }
}

public class Portfolio
{


    private string _name; //nom du pf
    private List<composition> _actions; //composition d'actions du pf
    private List<Rate> _rates; //evolution de la valeur du pf
    private Share _action_ref; //taux du cac40 (indice référence)

	public Portfolio(string name, Share action_ref)
	{
        this._name = name;
        this._actions = new List<composition>();
        //remplissage du portefeuille avec toutes les actions de poids 0
        this._rates = new List<Rate>();
        this._action_ref = action_ref;
	}

    public string _Name
    {
        get { return this._name; }
        private set { this._name = value; }
    }

    public List<composition> _Actions
    {
        get { return this._actions; }
        set {this._actions = value; }

    }
    public void setWeight(Share action, double weight)
    {
        var req = (from c in this._actions where c._action == action select c).SingleOrDefault();
       // (from c in this._actions where c._action == action select c).SingleOrDefault().setWeight(weight);

        System.Console.WriteLine("ancien poids");
       System.Console.WriteLine(req._weight);
        req._weight = weight;
        System.Console.WriteLine("new poids");
        System.Console.WriteLine(req._weight);
    }

    public void addRate(DateTime date, double rate)
    {
        this._rates.Add(new Rate(date, rate));
    }

    public double getRate(DateTime date)
    {
        return this._rates.Find(x => x._Date == date)._Rate;
    }

    public double getWeight(Share action)
    {
        var req = from c in this._actions
                  where c._action == action
                  select c._weight;

        foreach (double r in req)
        {

            return r;
        }

        return 0;

    }

    public Share _Action_ref
    {
        get { return this._action_ref; }
        private set { this._action_ref = value; }
    }

    public void addAction(Share action, double weight)
    {
        if (this._actions.Exists(x => x._action._Name == action._Name) )
        {
            System.Console.WriteLine("action existe");
            this.setWeight(action, weight);
        }
        else 
        {
            composition comp;
            comp._action = action;
            comp._weight = weight;
            this._actions.Add(comp);
        }
    }

    public List<Share> GetAllShares()
    {
        List<Share> res = new List<Share>();
        foreach (composition c in this._actions)
        {
            res.Add(c._action);
        }

        return res;
    }
}
