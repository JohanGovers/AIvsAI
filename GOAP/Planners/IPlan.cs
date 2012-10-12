using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP.Planners
{
    public interface IPlan
    {
        void Search(State state, Goal goal);
        IList<PlanningAction> GetPath();
        State GetFinalState();
    }
}
