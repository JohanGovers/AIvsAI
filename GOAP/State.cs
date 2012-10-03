using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP
{
	public class State : ICloneable, IEquatable<State>
    {
        //public static List<Func<State>> _funcs = new List<Func<State>>();
        //public static Dictionary<string, Predicate<State,string>> prejudicates = new Dictionary<string, Predicate<State,string>>();
        public static Dictionary<string, Func<State, string, string, bool>> prejudicates = new Dictionary<string, Func<State, string, string, bool>>();
        public static Dictionary<string, Func<State, string, string, string>> itemfuncs = new Dictionary<string, Func<State, string, string, string>>();

        public List<Tuple<string, string, string>> Relations = new List<Tuple<string, string, string>>();
        public Dictionary<string, int> Items;
        public List<PlanningAction> PlanningActions;
        
        /// <summary>
        /// A State is a snapshot of the current world, its items and inter-object relations
        /// </summary>
        public State()
        {
            Items = new Dictionary<string, int>();
            PlanningActions = new List<PlanningAction>();
        }
        public void AddItem(string item)  {
            AddItem(item, 1);
        }
        public void AddItem(string item, int quantity)
        {
            if (!Items.ContainsKey(item)) Items.Add(item, quantity);
            else Items[item] += quantity;
        }
        public void ReduceItem(string item, int quantity)
        {
            if (Items.ContainsKey(item))
            {
                Items[item] -= quantity;
                if (Items[item] == 0) RemoveItem(item);
            }
        }
        
        public void RemoveItem(string item)
        {
            Items.Remove(item);
        }
        public int ItemQuantity(string item)
        {
            if (!Items.ContainsKey(item))
            {
                return 0;
            }
            return Items[item];
        }
        public bool Sufficient(string item, int quantity)
        {
            if (!Items.ContainsKey(item)) return false;
            if (Items[item] < quantity) return false;
            return true;
        }
        public bool Has(string item)
        {
            return Sufficient(item, 1);
        }

        public object Clone()
        {
            var cloned = new State();
            foreach (var item in Items)
            {
                cloned.AddItem(item.Key, item.Value);
            }
            foreach (var pa in this.PlanningActions)
            {
                cloned.PlanningActions.Add((PlanningAction)pa.Clone());
            }
            foreach (var rel in Relations)
            {
                cloned.Relations.Add(rel);
            }
            return cloned;
        }

        public bool Equals(State other)
        {
            if (Items.Any(l => !other.Items.Contains(l))) return false;
            if (Items.Count() != other.Items.Count()) return false;
            if (Relations.Any(l => !other.Relations.Contains(l))) return false;
            if (Relations.Count() != other.Relations.Count()) return false;
            if (PlanningActions.Select(n => n.Name).Any(l => !other.PlanningActions.Select(n => n.Name).Contains(l))) return false;
            if (PlanningActions.Count() != other.PlanningActions.Count()) return false;
            return true;
        }
    }
}
