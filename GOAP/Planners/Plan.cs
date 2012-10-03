using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOAP.Planners;

namespace GOAP
{
 	public class Plan : IPlan
	{
        private Stack<PlanningAction> bestPath = new Stack<PlanningAction>();
		double bestscore = 0;
		int bestdepth = int.MaxValue;
        Stack<PlanningAction> planStack = new Stack<PlanningAction>();

        /// <summary>
        /// Simple uninformed Depth First Search
        /// </summary>
        public Plan()
        {

        }

        int _maxsearchdepth = 7;
        /// <summary>
        /// Search termination criteria.
        /// </summary>
        /// <param name="maxsearchdepth"></param>
        /// <returns></returns>
        public Plan SetMaxSearchDepth(int maxsearchdepth)
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
                bestPath = new Stack<PlanningAction>(planStack.Reverse());
			}
			if (goal.Fulfillment(state) == bestscore && depth < bestdepth)
			{
				bestdepth = depth;
                bestPath = new Stack<PlanningAction>(planStack.Reverse());
			}

            if (depth < _maxsearchdepth)
			{
				foreach (var a in state.PlanningActions.Where(l => l.CanExecute(state)))
				{
					planStack.Push(a);
					Search(a.Migrate(state), goal, depth + 1);
					planStack.Pop();
				}
			}
		}

        public void Search(State state, Goal goal)
        {
            Search(state, goal, 0);
        }

        public Stack<PlanningAction> GetPath()
        {
            return this.bestPath;
        }
    }
}
