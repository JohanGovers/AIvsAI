﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GOAP;
using GOAP.Planners;

namespace ConsoleTestProgram
{
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

            IPlan p = new DFSPlan();
            p.Search(s0, g0);


            List<string> bpath = new List<string>(); 
            bpath.Add("Initial score: " + g0.Fulfillment(s0));
			//foreach (var bs in p.GetPath().Reverse())
			//{
			//    var pa = s0.PlanningActions.Where(l => l.Name == bs.Name).First();
			//    pa.Execute(s0);
			//    bpath.Add(bs +" score: "+ g0.Fulfillment(s0));
			//}
        }
    }
}