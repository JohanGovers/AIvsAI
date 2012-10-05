using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP.Planners
{
    public class Domain
    {
        public List<Tuple<string, string, string>> Relations = new List<Tuple<string, string, string>>();
  
        /// <summary>
        /// A Domain holds prejudicate logic and functions that represents a specific AI planning domain, or problem.
        /// Put Relations that are static during the planning search here to save State memory.
        /// </summary>
        public Domain()
        {
            
        }
        public Goal Goal { get; private set; }
        public Domain AssignGoal(Goal goal)
        {
            Goal = goal;
            return this;
        }

        public State State { get; private set; }
        
        /// <summary>
        /// Write the Domain specific code here. Use Lambda Expressions to declare Domain specific helping functions or actions.
        /// state is used to model dynamic objects and object relations whereas domain can hold static relations
        /// state is being instantiated
        /// </summary>
        /// <param name="Logic"></param>
        public Domain BuildLogic(Action<Domain, State> Logic)
        {
            Logic(this, State = new State());
            return this;
        }

    }
}
