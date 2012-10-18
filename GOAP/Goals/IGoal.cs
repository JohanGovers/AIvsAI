using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP.Goals
{
    /// <summary>
    /// Implement the Fulfillment method which basically inspects the current State and gives it a score between 0 and 1
    /// </summary>
    public interface IGoal
    {
        double Fulfillment(State s);
    }
}
