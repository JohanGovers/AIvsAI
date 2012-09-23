using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP
{
    public class Goal
    {
        public string Name;
        List<Tuple<string, int>> _targets = new List<Tuple<string, int>>();
        
        public Goal(string Name)
        {
            this.Name = Name;
        }

        public Goal Target(string item, int quantity)
        {
            _targets.Add(new Tuple<string, int>(item, quantity));
            return this;
        }

        public double Fulfillment(State s)
        {
            double score = 0.0;
            foreach (var tgt in _targets)
            {
                double actual = s.ItemQuantity(tgt.Item1);
                if (actual > tgt.Item2) actual = tgt.Item2;
                score += actual / tgt.Item2;
            }
            return score / _targets.Count();
        }
    }

    public class PlanningAction : ICloneable
    {
        public string Name;
        bool multiProducer = false;

        List<string> _requires=new List<string>();
        List<Tuple<string, int>> _consumes = new List<Tuple<string, int>>();
        List<Tuple<string, int>> _produces = new List<Tuple<string, int>>();
        List<Action<State>> _postActions = new List<Action<State>>();

        public PlanningAction(string Name)
        {
            this.Name = Name;
        }

        public PlanningAction AssignPostAction(Action<State> PostAction)
        {
            _postActions.Add(PostAction);
            return this;
        }

        public bool CanExecute(State s)
        {
            if (_requires.Exists(l => !s.Has(l))) return false;
            if (_consumes.Exists(l => !s.Sufficient(l.Item1,l.Item2))) return false;
            return true;
        }

        public State Migrate(State s)
        {
            var newState = s.Clone() as State;
            Execute(newState);
            return newState;
        }

        public void Execute(State s)
        {
            _consumes.ForEach(c => s.ReduceItem(c.Item1, c.Item2));
            if (multiProducer)
            {
                int quantity = s.ItemQuantity(_requires.First());
                _produces.ForEach(p => s.AddItem(p.Item1, p.Item2 * quantity));
           
            }
            else
            {
                _produces.ForEach(p => s.AddItem(p.Item1, p.Item2));
            }
            _postActions.ForEach(pa => pa(s));
        }

        public PlanningAction Requires(string item)
        {
            _requires.Add(item);
            return this;
        }
        public PlanningAction Consumes(string item,int quantity)
        {
            _consumes.Add(new Tuple<string,int>(item,quantity));
            return this;
        }
        public PlanningAction Consumes(string item)
        {
            return Consumes(item, 1);
        }
        public PlanningAction Produces(string item, int quantity)
        {
            _produces.Add(new Tuple<string, int>(item, quantity));
            return this;
        }
        public PlanningAction Produces(string item)
        {
            return Produces(item, 1);
        }

        public PlanningAction MultiProducer()
        {
            multiProducer = true;
            return this;
        }



        public object Clone()
        {
            var cloned = new PlanningAction(this.Name);
            cloned.multiProducer = this.multiProducer;
            foreach (var req in _requires)
            {
                cloned._requires.Add(req);
            }
            foreach (var c in _consumes)
            {
                cloned._consumes.Add(new Tuple<string, int>(c.Item1, c.Item2));
            }
            foreach (var p in _produces)
            {
                cloned._produces.Add(new Tuple<string, int>(p.Item1, p.Item2));
            }
            return cloned;
        }
    }

    static class PlanningActionHandler
    {
        /*public static IPerishingAction Perishes()
        {
            return new PerishingAction();
        }*/

    }

}