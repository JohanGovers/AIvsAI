using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GOAP.Planners;

namespace GOAP.Test
{
	[TestFixture]
	class TowerOfHanoiTest
	{
        Domain hanoiDomain;
        public TowerOfHanoiTest()
        {
            // S ==> S
            // M ==> M
            // L ==> L
            // A  B  C
            //
            // Peg  = {A,B,C}
            // Disc = {S,M,L}

            hanoiDomain = new Domain().BuildLogic((domain, state) =>
            {
                state.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Small", "Disc Medium"));
                state.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Medium", "Disc Large"));
                state.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Large", "Peg A"));

                Dictionary<string, int> ObjectRelativeOrder = new Dictionary<string, int>()
	            {
	                {"Peg A", 0},
	                {"Peg B", 0},
	                {"Peg C", 0},
	                {"Disc Large", 1},
	                {"Disc Medium", 2},
	                {"Disc Small", 3}
	            };
                Func<string, string, bool> IsValidConfiguration = (above, below) =>
                {
                    return (ObjectRelativeOrder[above] > ObjectRelativeOrder[below]);
                };

                Func<State, string, string> PeekStack = (State s, string peg) =>
                {
                    string item = null;
                    bool abort = false;
                    while (!abort)
                    {
                        try
                        {
                            peg = s.Relations.Find(rel => rel.Item1 == "Object Relation" && rel.Item3 == peg).Item2;
                            item = peg;
                        }
                        catch
                        {
                            abort = true;
                        }
                    }
                    if (item == null) return peg;
                    return item;
                };

                Func<State, string, string, bool> IsValidMove = (State st, string fromPeg, string toPeg) =>
                {
                    var itemToBeMoved = PeekStack(st, fromPeg);
                    // Only discs can be moved.
                    if (!itemToBeMoved.Contains("Disc")) return false;
                    return IsValidConfiguration(itemToBeMoved, PeekStack(st, toPeg));
                };

                Func<State, string, string, string> PopStack = (State s, string peg, string ontop) =>
                {
                    string item = null;
                    while (peg != null)
                    {
                        try
                        {
                            peg = s.Relations.Find(rel => rel.Item1 == "Object Relation" && rel.Item3 == peg).Item2;
                            item = peg;
                        }
                        catch
                        {
                            peg = null;
                        }
                    }
                    s.Relations.Remove(s.Relations.Find(rel => rel.Item1 == "Object Relation" && rel.Item2 == item));
                    return item;
                };

                Action<State, string, string> PushStack = (State st, string disc, string toPeg) =>
                {
                    st.Relations.Add(new Tuple<string, string, string>("Object Relation", disc, PeekStack(st, toPeg)));
                };

                Action<State, string, string> MoveItem = (State s, string fromPeg, string toPeg) =>
                {
                    PushStack(s, PopStack(s, fromPeg, null), toPeg);
                };

                state.PlanningActions.Add(new PlanningAction("Move A to B")
                                        .AssignPrejudicate(st => IsValidMove(st, "Peg A", "Peg B"))
                                        .AssignPostAction(st => MoveItem(st, "Peg A", "Peg B"))
                                       );
                state.PlanningActions.Add(new PlanningAction("Move A to C")
                                       .AssignPrejudicate(st => IsValidMove(st, "Peg A", "Peg C"))
                                       .AssignPostAction(st => MoveItem(st, "Peg A", "Peg C"))
                                       );
                state.PlanningActions.Add(new PlanningAction("Move B to A")
                                       .AssignPrejudicate(st => IsValidMove(st, "Peg B", "Peg A"))
                                       .AssignPostAction(st => MoveItem(st, "Peg B", "Peg A"))
                                       );
                state.PlanningActions.Add(new PlanningAction("Move B to C")
                                       .AssignPrejudicate(st => IsValidMove(st, "Peg B", "Peg C"))
                                       .AssignPostAction(st => MoveItem(st, "Peg B", "Peg C"))
                                       );
                state.PlanningActions.Add(new PlanningAction("Move C to A")
                                       .AssignPrejudicate(st => IsValidMove(st, "Peg C", "Peg A"))
                                       .AssignPostAction(st => MoveItem(st, "Peg C", "Peg A"))
                                       );
                state.PlanningActions.Add(new PlanningAction("Move C to B")
                                       .AssignPrejudicate(st => IsValidMove(st, "Peg C", "Peg B"))
                                       .AssignPostAction(st => MoveItem(st, "Peg C", "Peg B"))
                                       );
            }).AssignGoal(
                     new Goal("Solve Hanoi").RelationalTarget("Object Relation", "Disc Large", "Peg C")
                                            .RelationalTarget("Object Relation", "Disc Medium", "Disc Large")
                                            .RelationalTarget("Object Relation", "Disc Small", "Disc Medium")
            );
        }
		[Test]
		public void SolveTowerOfHanoiWith3Discs()
		{
			
			List<string> bpath = new List<string>();
			bpath.Add("Initial score: " + hanoiDomain.Goal.Fulfillment(hanoiDomain.State));


			IPlan p = new DFSPlan().SetMaxSearchDepth(7);
			p.Search(hanoiDomain.State, hanoiDomain.Goal);

            Assert.GreaterOrEqual(hanoiDomain.Goal.Fulfillment(p.GetFinalState()), 1.0);
       }

        [Test]
        public void IncrementalPlanningSearch()
        {
            List<string> bpath = new List<string>();
            bpath.Add("Initial score: " + hanoiDomain.Goal.Fulfillment(hanoiDomain.State));

            IPlan p = new DFSPlan().SetMaxSearchDepth(4);
            p.Search(hanoiDomain.State, hanoiDomain.Goal);
            p.Search(p.GetFinalState(), hanoiDomain.Goal);

            Assert.GreaterOrEqual(hanoiDomain.Goal.Fulfillment(p.GetFinalState()), 1.0);
        }

	}

}
