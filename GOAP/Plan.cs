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
		int bestdepth = int.MaxValue;
		Stack<string> planStack = new Stack<string>();
		public void Search(State state, Goal goal, int depth)
		{
			if (goal.Fulfillment(state) > bestscore)
			{
				bestscore = goal.Fulfillment(state);
				bestdepth = depth;
				bestPath = new Stack<String>(planStack.Reverse());
			}
			if (goal.Fulfillment(state) == bestscore && depth < bestdepth)
			{
				bestdepth = depth;
				bestPath = new Stack<String>(planStack.Reverse());
			}


			if (state.Relations.Exists(rel => rel.Item1 == "Object Relation" && rel.Item2 == "Disc Large" && rel.Item3 == "Peg C"))
			{
				if (state.Relations.Exists(rel => rel.Item1 == "Object Relation" && rel.Item2 == "Disc Medium" && rel.Item3 == "Disc Large"))
				{
					if (state.Relations.Exists(rel => rel.Item1 == "Object Relation" && rel.Item2 == "Disc Small" && rel.Item3 == "Disc Medium"))
					{
					}
				}
			}


			if (depth < 7)
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
