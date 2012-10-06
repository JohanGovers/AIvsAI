using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOAP.Planners;

namespace GOAP.Planners
{
    class AStarPlan : IPlan
    {
        //public void Pathfind(int source, int goal, HexMap SearchHexMap, List<PathNode> Route, 
        //                     List<PathNode> Considered,ref int iterations)
        /*
        OPEN = priority queue containing START
        CLOSED = empty set
        while lowest rank in OPEN is not the GOAL:
          current = remove lowest rank item from OPEN
          add current to CLOSED
          for neighbors of current:
            cost = g(current) + movementcost(current, neighbor)
            if neighbor in OPEN and cost less than g(neighbor):
              remove neighbor from OPEN, because new path is better
            if neighbor in CLOSED and cost less than g(neighbor): **
              remove neighbor from CLOSED
            if neighbor not in OPEN and neighbor not in CLOSED:
              set g(neighbor) to cost
              add neighbor to OPEN
              set priority queue rank to g(neighbor) + h(neighbor)
              set neighbor's parent to current

        reconstruct reverse path from goal to start
        by following parent pointers
*/
        public void Astar(State state, Goal goal)
        {
            List<State> Open = new List<State>();
            List<State> Closed = new List<State>();
            Open.Add(state);
            while (goal.Fulfillment(Open[0]) < 1.0)
            {
                State Current = Open[0];
                Closed.Add(Current);
                Open.RemoveAt(0);
                foreach (var a in state.PlanningActions.Where(l => l.CanExecute(state)))
                {
                }
            }
        }

        public void Search(State state, Goal goal)
        {
            Astar(state, goal);
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
