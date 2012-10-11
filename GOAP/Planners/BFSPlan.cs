using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP.Planners
{
    public class BFSPlan : IPlan
    {
        private int _maxSearchDepth = 5;
        
        private Queue<State> stateProcessQueue = new Queue<State>();

        // TODO: Remove the Tuple. Use something with better naming
        private List<Tuple<State, PlanningAction>> takenPath = new List<Tuple<State, PlanningAction>>();

        public BFSPlan SetMaxSearchDepth(int maxsearchdepth)
        {
            _maxSearchDepth = maxsearchdepth;
            return this;
        }

        public void Search(State state, Goal goal)
        {
            int searchDepth = 0;

            stateProcessQueue.Enqueue(state);

            // Evaluate exit criteria
            while (searchDepth < _maxSearchDepth && stateProcessQueue.Count > 0)
            {
                searchDepth++;

                var currentState = stateProcessQueue.Dequeue();
                
                // Put each action in a queue
                foreach (var action in state.PlanningActions.Where(l => l.CanExecute(state)))
                {
                    var neighbourState = action.Migrate(currentState); // Note: Feels backwards. I want to type currentState.Execute(action);
                    // TODO: Don't add already visited states...
                    takenPath.Add(new Tuple<State, PlanningAction>(currentState, action));
                    stateProcessQueue.Enqueue(neighbourState);
                }
            }
        }

        public Stack<PlanningAction> GetPath()
        {
            return new Stack<PlanningAction>(takenPath.Select(tuple => tuple.Item2).Reverse());
        }

        public State GetFinalState()
        {
            return takenPath.Last().Item1;
        }
    }
}
