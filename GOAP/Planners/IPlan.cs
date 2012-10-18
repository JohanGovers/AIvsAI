using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOAP.Goals;

namespace GOAP.Planners
{
    public interface IPlan
    {
        void Search(State state, IGoal goal);
        IList<PlanningAction> GetPath();
        State GetFinalState();
    }
}
