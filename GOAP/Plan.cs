using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP
{
    public class Plan
    {
        public Stack<string> bestPath = new Stack<string>();
        double bestscore = 0;
        Stack<string> planStack = new Stack<string>();
        public void Search(State state, Goal goal, int depth)
        {
            if (goal.Fulfillment(state) > bestscore)
            {
                bestscore = goal.Fulfillment(state);
                bestPath = new Stack<String>(planStack.Reverse());
            }
            if (depth < 8)
            {
                foreach (var a in state.PlanningActions.Where(l => l.CanExecute(state)))
                {
                    planStack.Push(a.Name);
                    Search(a.Migrate(state), goal, depth + 1);
                    planStack.Pop();
                }
            }
        }
    }
}
