using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP.Goals
{
    public class CustomGoal : IGoal
    {
        Func<State, double> _goalFunction;
        public CustomGoal AssignGoal(Func<State, double> goalFunction)
        {
            this._goalFunction = goalFunction;
            return this;
        }
        public double Fulfillment(State state)
        {
            return _goalFunction(state);
        }
    }
}
