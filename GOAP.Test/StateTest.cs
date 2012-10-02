using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GOAP.Test
{
    [TestFixture]
    public class StateTest
    {
        [Test]
        public void StatesCanBeCompared()
        {
            State s0 = new State();
            s0.AddItem("Item 1");
            s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Disc Medium"));
            s0.PlanningActions.Add(new PlanningAction("Use hammer"));
            
            State s1 = new State();
            s1.AddItem("Item 1");
            s1.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Disc Medium"));
            s1.PlanningActions.Add(new PlanningAction("Use hammer"));

            Assert.IsTrue(s0.Equals(s1));
            Assert.IsTrue(s1.Equals(s0));
            s0.AddItem("Item 2");
            s0.Relations.Add(new Tuple<string, string, string>("InValid Configuration", "Disc Small", "Disc Medium"));
            s0.PlanningActions.Add(new PlanningAction("Use sledgehammer"));
            s1.AddItem("Item 2");
            s1.Relations.Add(new Tuple<string, string, string>("InValid Configuration", "Disc Small", "Disc Medium"));
            s1.PlanningActions.Add(new PlanningAction("Use sledgehammer"));
            
            Assert.IsTrue(s0.Equals(s1));
            Assert.IsTrue(s1.Equals(s0));

            s0.RemoveItem("Item 1");
            Assert.IsFalse(s0.Equals(s1));
            Assert.IsFalse(s1.Equals(s0));
            s1.RemoveItem("Item 1");

            Assert.IsTrue(s0.Equals(s1));
            Assert.IsTrue(s1.Equals(s0));

            s0.Items["Item 2"] += 4;

            Assert.IsFalse(s0.Equals(s1));
            Assert.IsFalse(s1.Equals(s0));

            s1.Items["Item 2"] += 4;
            Assert.IsTrue(s0.Equals(s1));
            Assert.IsTrue(s1.Equals(s0));

            s0.Relations.RemoveAt(0);
            Assert.IsFalse(s0.Equals(s1));
            Assert.IsFalse(s1.Equals(s0));

            s1.Relations.RemoveAt(0);
            Assert.IsTrue(s0.Equals(s1));
            Assert.IsTrue(s1.Equals(s0));

        }
    }
}
