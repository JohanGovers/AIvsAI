using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOAP.Planners
{
    public class Domain
    {
        public List<Tuple<string, string, string>> Relations = new List<Tuple<string, string, string>>();
        public Dictionary<string, Func<State, string, string, bool>> prejudicates = new Dictionary<string, Func<State, string, string, bool>>();
        public Dictionary<string, Func<State, string, string, string>> itemfuncs = new Dictionary<string, Func<State, string, string, string>>();

        /// <summary>
        /// A Domain holds prejudicate logic and functions that represents a specific AI planning domain, or problem.
        /// Put Relations that are static during the planning search here to save State memory.
        /// </summary>
        public Domain()
        {

        }


    }
}
