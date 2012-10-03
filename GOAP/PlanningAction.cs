using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP
{
    

    public class PlanningAction : ICloneable
    {
        public string Name;
        bool multiProducer = false;
        List<Func<State,bool>> _prejudicates = new List<Func<State,bool>>();
        Dictionary<string, int> _requires = new Dictionary<string, int>();
        Dictionary<string, int> _consumes = new Dictionary<string, int>();
        Dictionary<string, int> _produces = new Dictionary<string, int>();
        List<Action<State>> _postActions = new List<Action<State>>();

        /// <summary>
        /// A PlanningAction transforms a State into a new State.
        /// </summary>
        /// <param name="Name"></param>
        public PlanningAction(string Name)
        {
            this.Name = Name;
        }

        public PlanningAction AssignPrejudicate(Func<State,bool> prejudicate)
        {
            _prejudicates.Add(prejudicate);
            return this;
        }


        public PlanningAction AssignPostAction(Action<State> PostAction)
        {
            _postActions.Add(PostAction);
            return this;
        }

        public bool CanExecute(State s)
        {
            if (_prejudicates.Any(l => !l(s))) return false;
            if (_requires.Any(l => !s.Has(l.Key))) return false;
            if (_consumes.Any(l => !s.Sufficient(l.Key, l.Value))) return false;
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
            foreach (var c in _consumes)
            {
                s.ReduceItem(c.Key, c.Value);
            }
            if (multiProducer)
            {
                int quantity = s.ItemQuantity(_requires.Keys.First());
                foreach (var p in _produces)
                {
                    s.AddItem(p.Key, p.Value * quantity);
                }
           
            }
            else
            {
                foreach (var p in _produces)
                {
                    s.AddItem(p.Key, p.Value);
                }
            }
            _postActions.ForEach(pa => pa(s));
        }

        public PlanningAction Requires(string item)
        {
            _requires.Add(item, 0);
            return this;
        }
        public PlanningAction Consumes(string item,int quantity)
        {
            _consumes.Add(item, quantity);
            return this;
        }
        public PlanningAction Consumes(string item)
        {
            return Consumes(item, 1);
        }
        public PlanningAction Produces(string item, int quantity)
        {
            _produces.Add(item, quantity);
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
                cloned._requires.Add(req.Key,req.Value);
            }
            foreach (var c in _consumes)
            {
                cloned._consumes.Add(c.Key, c.Value);
            }
            foreach (var p in _produces)
            {
                cloned._produces.Add(p.Key,p.Value);
            }
            foreach (var pr in _prejudicates)
            {
                cloned._prejudicates.Add(pr);
            }
            foreach (var pa in _postActions)
            {
                cloned._postActions.Add(pa);
            }
            return cloned;
        }
    }
}