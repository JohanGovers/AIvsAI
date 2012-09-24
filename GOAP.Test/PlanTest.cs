using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

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
            
            Plan p = new Plan();
            p.Search(state, g0, 0);

            Assert.That(p.bestPath.Peek(), Is.EqualTo(actionName));
        }

        [Test]
        public void PerformActionLeadingToGoal()
        {
            string actionName = "Build Harvester";
            var state = new State();
            state.PlanningActions.Add(new PlanningAction(actionName).Produces("Harvester"));
            state.PlanningActions.Add(new PlanningAction("Not leading to goal").Produces("retsevraH"));
            var g0 = new Goal("Build Harvester").Target("Harvester", 1);

            Plan p = new Plan();
            p.Search(state, g0, 0);

            Assert.That(p.bestPath.Peek(), Is.EqualTo(actionName));
        }

        [Test]
        public void DontPerformAnyActionWhenNoActionIsAvailable()
        {
            var state = new State();
            state.AddItem("Cash", 99);
            var action = new PlanningAction("Not affordable").Consumes("Cash", 100).Produces("Harvester");
            state.PlanningActions.Add(action);

            var g0 = new Goal("Build Harvester").Target("Harvester", 1);

            Plan p = new Plan();
            p.Search(state, g0, 0);

            Assert.That(p.bestPath.Count, Is.EqualTo(0));
        }
    }
}
