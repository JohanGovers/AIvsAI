using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP.Planners
{
    public class BFSPlan : IPlan
    {
        private Queue<State> stateProcessQueue = new Queue<State>();

        public void Search(State state, Goal goal)
        {
            stateProcessQueue.Enqueue(state);

            InnerSearch(state, goal);
        }

        private void InnerSearch(State state, Goal goal)
        {
            // Evaluate exit criteria

            // Put each state in a queue

            // Recurse
        }

        public Stack<PlanningAction> GetPath()
        {
            throw new NotImplementedException();
        }

        public State GetFinalState()
        {
            throw new NotImplementedException();
        }
    }
}
