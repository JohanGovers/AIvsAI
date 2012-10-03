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
		[Test]
		public void SolveTowerOfHanoiWith3Discs()
		{
			// S ==> S
			// M ==> M
			// L ==> L
			// A  B  C
			// Type Item
			// Peg : Item
			// Disc : moveable

			var s0 = new State();
            var domain = new Domain();
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Disc Medium"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Disc Large"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Peg A"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Peg B"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Peg C"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Disc Large"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Peg A"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Peg B"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Peg C"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Large", "Peg A"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Large", "Peg B"));
			domain.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Large", "Peg C"));

			s0.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Small", "Disc Medium"));
			s0.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Medium", "Disc Large"));
			s0.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Large", "Peg A"));

			domain.prejudicates.Add("Has Discs", (State s, string str, string str2) => { return s.Items.Any(l => l.Key.Contains(str)); });
			domain.prejudicates.Add("Valid Configuration", (State s, string item, string ontop) =>
			{
				return domain.Relations.Exists(rel => rel.Item1 == "Valid Configuration"
										&& rel.Item2 == item
										&& rel.Item3 == ontop
								  );
			});
			domain.prejudicates.Add("Valid Move", (State s, string fromPeg, string toPeg) =>
			{
				var itemToBeMoved = domain.itemfuncs["Peek Stack?"](s, fromPeg, null);
				// Only discs can be moved.
				if (!itemToBeMoved.Contains("Disc")) return false;
				var itemOnToPeg = domain.itemfuncs["Peek Stack?"](s, toPeg, null);
				return domain.prejudicates["Valid Configuration"](s, itemToBeMoved, itemOnToPeg);
			});

			domain.itemfuncs.Add("Peek Stack?", (State s, string peg, string ontop) =>
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
			});

			domain.itemfuncs.Add("Pop Stack?", (State s, string peg, string ontop) =>
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
				var popitem = s.Relations.Find(rel => rel.Item1 == "Object Relation" && rel.Item2 == item);
				s.Relations.Remove(popitem);
				return item;
			});

			domain.itemfuncs.Add("Push Stack", (State s, string disc, string toPeg) =>
			{
				string ontop = domain.itemfuncs["Peek Stack?"](s, toPeg, null);
				s.Relations.Add(new Tuple<string, string, string>("Object Relation", disc, ontop));
				return "";
			});

			domain.itemfuncs.Add("Move Disc", (State s, string fromPeg, string toPeg) =>
			{
				string pegToMove = domain.itemfuncs["Pop Stack?"](s, fromPeg, null);
				domain.itemfuncs["Push Stack"](s, pegToMove, toPeg);
				return "";
			});

			/*List<string> abc = new List<string> { "A", "B", "C" };
			foreach (var fr in abc)
			{
				foreach (var to in abc.Where(l => l != fr))
				{
					var x0 = "Move " + fr + " to " + to;
					var x1 = "Peg " + fr;
					var x2 = "Peg " + to;
					s0.PlanningActions.Add(new PlanningAction(x0)
					.AssignPrejudicate(st => State.prejudicates["Valid Move"](st, x1, x2))
					.AssignPostAction(st => { State.itemfuncs["Move Disc"](st, x1, x2); })
					);
				}
			}*/

			s0.PlanningActions.Add(new PlanningAction("Move A to B")
									.AssignPrejudicate(st => domain.prejudicates["Valid Move"](st, "Peg A", "Peg B"))
									.AssignPostAction(st => { domain.itemfuncs["Move Disc"](st, "Peg A", "Peg B"); })
					   );
			s0.PlanningActions.Add(new PlanningAction("Move A to C")
                                   .AssignPrejudicate(st => domain.prejudicates["Valid Move"](st, "Peg A", "Peg C"))
                                   .AssignPostAction(st => { domain.itemfuncs["Move Disc"](st, "Peg A", "Peg C"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move B to A")
                                   .AssignPrejudicate(st => domain.prejudicates["Valid Move"](st, "Peg B", "Peg A"))
                                   .AssignPostAction(st => { domain.itemfuncs["Move Disc"](st, "Peg B", "Peg A"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move B to C")
                                   .AssignPrejudicate(st => domain.prejudicates["Valid Move"](st, "Peg B", "Peg C"))
                                   .AssignPostAction(st => { domain.itemfuncs["Move Disc"](st, "Peg B", "Peg C"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move C to A")
                                   .AssignPrejudicate(st => domain.prejudicates["Valid Move"](st, "Peg C", "Peg A"))
                                   .AssignPostAction(st => { domain.itemfuncs["Move Disc"](st, "Peg C", "Peg A"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move C to B")
                                   .AssignPrejudicate(st => domain.prejudicates["Valid Move"](st, "Peg C", "Peg B"))
                                   .AssignPostAction(st => { domain.itemfuncs["Move Disc"](st, "Peg C", "Peg B"); })
								   );
            
			var g0 = new Goal("Solve Hanoi").RelationalTarget("Object Relation", "Disc Large", "Peg C")
											.RelationalTarget("Object Relation", "Disc Medium", "Disc Large")
											.RelationalTarget("Object Relation", "Disc Small", "Disc Medium");

			var s1 = s0.Clone();

            var xf = domain.prejudicates["Valid Move"](s0, "Peg A", "Peg B"); 

			List<string> bpath = new List<string>();
			bpath.Add("Initial score: " + g0.Fulfillment(s0));


			Plan p = new Plan().SetMaxSearchDepth(8);
			p.Search(s0, g0);

            Assert.AreEqual(p.GetPath().Count, 7,"{0}",p.GetPath().Count);
       }
	}
}
