using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOAP;

namespace GOAP
{
    class Plan
    {
        public Stack<string> bestPath = new Stack<string>();
        double bestscore = 0;
        Stack<string> planStack = new Stack<string>();
        public void Search(State state, Goal goal,int depth)
        {
            if (goal.Fulfillment(state)> bestscore)
            {
                bestscore = goal.Fulfillment(state);
                bestPath = new Stack<String>(planStack.Reverse());
            }
            if (depth < 8)
            {
                foreach (var a in state.PlanningActions.Where(l => l.CanExecute(state)))
                {
                    planStack.Push(a.Name);
                    Search(a.Migrate(state), goal, depth+1);
                    planStack.Pop();
                }
            }
        }
    }

    class Program
    {
        
        
        static void Main(string[] args)
        {
            var s0 = new State();
            s0.AddItem("Cash", 500);
            s0.AddItem("Factory Blueprint", 1);
            s0.PlanningActions.Add(new PlanningAction("Build Harvester").Requires("Factory").Consumes("Cash", 100).Produces("Harvester"));
            s0.PlanningActions.Add(new PlanningAction("Build Factory").Consumes("Cash", 400).Produces("Factory").Requires("Factory Blueprint"));
            s0.PlanningActions.Add(new PlanningAction("Harvest").Requires("Harvester").Produces("Cash", 100).MultiProducer());
            s0.PlanningActions.Add(new PlanningAction("Upgrade Harvester(s)").Consumes("Cash", 0).AssignPostAction(ls => ls.PlanningActions.Find(l => l.Name == "Harvest").Produces("Cash", 1259)));
            s0.PlanningActions.Add(new PlanningAction("Sell Factory Blueprint").Consumes("Factory Blueprint").Produces("Cash", 5000));
           
            var g0 = new Goal("Build Fortune").Target("Factory", 2).Target("Cash",10000);
            //var g0 = new Goal("Build Fortune").Target("Cash", 20000);
            double score = g0.Fulfillment(s0);
            //a1.Execute(s0);
            double score2 = g0.Fulfillment(s0);

            var s1 = s0.Clone();

            Plan p = new Plan();
            p.Search(s0, g0, 0);


            /*var pa = new PlanningAction("Build Harvester").Requires("Factory").Consumes("Cash", 50).Produces("Harvester");
            pa.AssignPostAction(ls => { ls.PlanningActions.Add(new PlanningAction("Spawner")); });
            */
            List<string> bpath = new List<string>(); 
            bpath.Add("Initial score: " + g0.Fulfillment(s0));
            foreach (var bs in p.bestPath.Reverse())
            {
                var pa = s0.PlanningActions.Where(l => l.Name == bs).First();
                pa.Execute(s0);
                bpath.Add(bs +" score: "+ g0.Fulfillment(s0));
            }
        }
    }
}