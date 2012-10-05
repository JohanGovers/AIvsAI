using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GOAP.Planners;

namespace GOAP.Test
{
    [TestFixture]
    public class PlanTest
    {
        [Test]
        public void PerformOnlyActionInPlan()
        {
            string actionName = "Build Harvester";
            var state = new State();
            state.PlanningActions.Add(new PlanningAction(actionName).Produces("Harvester"));
            var g0 = new Goal("Build Harvester").Target("Harvester", 1);
            
            IPlan p = new Plan();
            p.Search(state, g0);

            Assert.That(p.GetPath().Peek().Name, Is.EqualTo(actionName));
        }

        [Test]
        public void PerformActionLeadingToGoal()
        {
            string actionName = "Build Harvester";
            var state = new State();
            state.PlanningActions.Add(new PlanningAction(actionName).Produces("Harvester"));
            state.PlanningActions.Add(new PlanningAction("Not leading to goal").Produces("retsevraH"));
            var g0 = new Goal("Build Harvester").Target("Harvester", 1);

            IPlan p = new Plan();
            p.Search(state, g0);

            Assert.That(p.GetPath().Peek().Name, Is.EqualTo(actionName));
        }

        [Test]
        public void DontPerformAnyActionWhenNoActionIsAvailable()
        {
            var state = new State();
            state.AddItem("Cash", 99);
            var action = new PlanningAction("Not affordable").Consumes("Cash", 100).Produces("Harvester");
            state.PlanningActions.Add(action);

            var g0 = new Goal("Build Harvester").Target("Harvester", 1);

            IPlan p = new Plan();
            p.Search(state, g0);

            Assert.That(p.GetPath().Count, Is.EqualTo(0));
        }

        [Test]
        public void LimitDepthFirstPlanSearch()
        {
            var state = new State();
            state.AddItem("Cash", 0);
            var action = new PlanningAction("Get Salary").Produces("Cash",100);
            state.PlanningActions.Add(action);

            var g0 = new Goal("Accumulate Cash").Target("Cash", 1000);

            IPlan p = new Plan().SetMaxSearchDepth(3);
            p.Search(state, g0);

            Assert.That(p.GetPath().Count, Is.EqualTo(3));
        }
    }
}
