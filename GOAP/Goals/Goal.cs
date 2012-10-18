using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOAP.Goals;

namespace GOAP
{
    public class Goal : IGoal
    {
        public string Name;
        Dictionary<string, int> _targets = new Dictionary<string, int>();
        List<Tuple<string, string, string>> _relationalTargets = new List<Tuple<string, string, string>>();
        

        public Goal(string Name)
        {
            this.Name = Name;
        }

        public Goal Target(string item, int quantity)
        {
            _targets.Add(item, quantity);
            return this;
        }

        public Goal RelationalTarget(string relation, string objectA, string objectB )
        {
            _relationalTargets.Add(new Tuple<string,string,string>(relation, objectA, objectB));
            return this;
        }

        public double Fulfillment(State s)
        {
            double score = 0.0;
            foreach (var tgt in _targets)
            {
                double actual = s.ItemQuantity(tgt.Key);
                if (actual > tgt.Value) actual = tgt.Value;
                score += actual / tgt.Value;
            }
            foreach (var reltgt in _relationalTargets)
            {
				if (s.Relations.Contains(reltgt)) { score += 1.0; }
				else { break; }
            }
            return score / (_targets.Count + _relationalTargets.Count);
        }
    }
}
