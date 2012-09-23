using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP
{
    public class State : ICloneable
    {
        public Dictionary<string, int> Items;
        public List<PlanningAction> PlanningActions;
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
            if (!Items.ContainsKey(item))
            {
                Items.Add(item, quantity);
            }
            else
            {
                Items[item] += quantity;
            }
        }
        public void ReduceItem(string item, int quantity)
        {
            if (Items.ContainsKey(item))
            {
                Items[item] -= quantity;
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
            if (!Items.ContainsKey(item))
            {
                return false;
            }
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
            return cloned;
        }
    }
}
