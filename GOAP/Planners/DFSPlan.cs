using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOAP.Planners;

namespace GOAP
{
 	public class DFSPlan : IPlan
	{
        Stack<PlanningAction> planStack = new Stack<PlanningAction>();
        Stack<State> stateStack = new Stack<State>();
        Stack<PlanningAction> bestActionStack = new Stack<PlanningAction>();
        Stack<State> bestStateStack = new Stack<State>();
        double bestscore = 0;
		int bestdepth = int.MaxValue;
        
        /// <summary>
        /// Simple uninformed Depth First Search
        /// </summary>
        public DFSPlan()
        {

        }

        int _maxsearchdepth = 7;
        /// <summary>
        /// Search termination criteria.
        /// </summary>
        /// <param name="maxsearchdepth"></param>
        /// <returns></returns>
        public DFSPlan SetMaxSearchDepth(int maxsearchdepth)
        {
            _maxsearchdepth = maxsearchdepth;
            return this;
        }

		private void Search(State state, Goal goal, int depth)
		{
        	if (goal.Fulfillment(state) > bestscore)
			{
				bestscore = goal.Fulfillment(state);
				bestdepth = depth;
                bestActionStack = new Stack<PlanningAction>(planStack.Reverse());
                bestStateStack = new Stack<State>(stateStack.Reverse());
			}
			if (goal.Fulfillment(state) == bestscore && depth < bestdepth)
			{
				bestdepth = depth;
                bestActionStack = new Stack<PlanningAction>(planStack.Reverse());
                bestStateStack = new Stack<State>(stateStack.Reverse());
		    }

            if (depth < _maxsearchdepth)
			{
				foreach (var a in state.PlanningActions.Where(l => l.CanExecute(state)))
				{
                    var migratedState = a.Migrate(state);
					if(!stateStack.Contains(migratedState)) 
                    {
                        planStack.Push(a);
                        stateStack.Push(migratedState);
                        Search(migratedState, goal, depth + 1);
                        stateStack.Pop();
					    planStack.Pop();
                    }
				}
			}
		}

        public void Search(State state, Goal goal)
        {
            Search(state, goal, 0);
        }

        public IList<PlanningAction> GetPath()
        {
			return this.bestActionStack.ToList();
        }


        public State GetFinalState()
        {
            return this.bestStateStack.First();
        }
    }
}
