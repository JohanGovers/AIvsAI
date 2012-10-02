using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GOAP.Test
{
	[TestFixture]
	class TowerOfHanoiTest
	{
		[Test]
		public void SolveTowerOfHanoiWith3Tiles()
		{
			// S ==> S
			// M ==> M
			// L ==> L
			// A  B  C
			// Type Item
			// Peg : Item
			// Disc : moveable

			var s0 = new State();
			s0.AddItem("Peg A");
			s0.AddItem("Peg B");
			s0.AddItem("Peg C");
			s0.AddItem("Disc Small");
			s0.AddItem("Disc Medium");
			s0.AddItem("Disc Large");
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Disc Medium"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Disc Large"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Peg A"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Peg B"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Small", "Peg C"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Disc Large"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Peg A"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Peg B"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Medium", "Peg C"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Large", "Peg A"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Large", "Peg B"));
			s0.Relations.Add(new Tuple<string, string, string>("Valid Configuration", "Disc Large", "Peg C"));

			s0.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Small", "Disc Medium"));
			s0.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Medium", "Disc Large"));
			s0.Relations.Add(new Tuple<string, string, string>("Object Relation", "Disc Large", "Peg A"));

			State.prejudicates.Add("Has Discs", (State s, string str, string str2) => { return s.Items.Any(l => l.Key.Contains(str)); });
			State.prejudicates.Add("Valid Configuration", (State s, string item, string ontop) =>
			{
				return s.Relations.Exists(rel => rel.Item1 == "Valid Configuration"
										&& rel.Item2 == item
										&& rel.Item3 == ontop
								  );
			});
			State.prejudicates.Add("Valid Move", (State s, string fromPeg, string toPeg) =>
			{
				var itemToBeMoved = State.itemfuncs["Peek Stack?"](s, fromPeg, null);
				// Only discs can be moved.
				if (!itemToBeMoved.Contains("Disc")) return false;
				var itemOnToPeg = State.itemfuncs["Peek Stack?"](s, toPeg, null);
				return State.prejudicates["Valid Configuration"](s, itemToBeMoved, itemOnToPeg);
			});

			State.itemfuncs.Add("Peek Stack?", (State s, string peg, string ontop) =>
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

			State.itemfuncs.Add("Pop Stack?", (State s, string peg, string ontop) =>
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

			State.itemfuncs.Add("Push Stack", (State s, string disc, string toPeg) =>
			{
				string ontop = State.itemfuncs["Peek Stack?"](s, toPeg, null);
				s.Relations.Add(new Tuple<string, string, string>("Object Relation", disc, ontop));
				return "";
			});

			State.itemfuncs.Add("Move Disc", (State s, string fromPeg, string toPeg) =>
			{
				string pegToMove = State.itemfuncs["Pop Stack?"](s, fromPeg, null);
				State.itemfuncs["Push Stack"](s, pegToMove, toPeg);
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
									.AssignPrejudicate(st => State.prejudicates["Valid Move"](st, "Peg A", "Peg B"))
									.AssignPostAction(st => { State.itemfuncs["Move Disc"](st, "Peg A", "Peg B"); })
					   );
			s0.PlanningActions.Add(new PlanningAction("Move A to C")
								   .AssignPrejudicate(st => State.prejudicates["Valid Move"](st, "Peg A", "Peg C"))
								   .AssignPostAction(st => { State.itemfuncs["Move Disc"](st, "Peg A", "Peg C"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move B to A")
								   .AssignPrejudicate(st => State.prejudicates["Valid Move"](st, "Peg B", "Peg A"))
								   .AssignPostAction(st => { State.itemfuncs["Move Disc"](st, "Peg B", "Peg A"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move B to C")
								   .AssignPrejudicate(st => State.prejudicates["Valid Move"](st, "Peg B", "Peg C"))
								   .AssignPostAction(st => { State.itemfuncs["Move Disc"](st, "Peg B", "Peg C"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move C to A")
								   .AssignPrejudicate(st => State.prejudicates["Valid Move"](st, "Peg C", "Peg A"))
								   .AssignPostAction(st => { State.itemfuncs["Move Disc"](st, "Peg C", "Peg A"); })
								   );
			s0.PlanningActions.Add(new PlanningAction("Move C to B")
								   .AssignPrejudicate(st => State.prejudicates["Valid Move"](st, "Peg C", "Peg B"))
								   .AssignPostAction(st => { State.itemfuncs["Move Disc"](st, "Peg C", "Peg B"); })
								   );
            
			var g0 = new Goal("Solve Hanoi").RelationalTarget("Object Relation", "Disc Large", "Peg C")
											.RelationalTarget("Object Relation", "Disc Medium", "Disc Large")
											.RelationalTarget("Object Relation", "Disc Small", "Disc Medium");

			var s1 = s0.Clone();

			var xf = State.prejudicates["Valid Move"](s0, "Peg A", "Peg B");  //(s0, "Peg A", "Peg B");

			List<string> bpath = new List<string>();
			bpath.Add("Initial score: " + g0.Fulfillment(s0));


			Plan p = new Plan();
			p.Search(s0, g0, 0);
			//p.Search(s0, g0, 0);

            Assert.Greater(p.bestPath.Count, 6);

			foreach (var bs in p.bestPath.Reverse())
			{
				var pa = s0.PlanningActions.Where(l => l.Name == bs).First();
				pa.Execute(s0);
				bpath.Add(bs + " score: " + g0.Fulfillment(s0));
			}
		}
	}
}
