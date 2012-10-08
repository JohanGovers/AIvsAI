using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GOAP.Planners;

namespace GOAP.Test
{
    [TestFixture]
    class Puzzle15Test
    {
        Domain puzzle;

        [Test]
        public void Solve()
        {
            puzzle = new Domain().BuildLogic((domain, state) =>
            {
                // 
                //    01 02 03 04
                //    05 06 07 08
                //    09 10 11 12
                //    13 14 15 
                //
                //
                string[] invalid_north = new string[] { "1", "2", "3", "4" };
                string[] invalid_east = new string[] { "4", "8", "12", "16" };
                string[] invalid_south = new string[] { "13", "14", "15", "16" };
                string[] invalid_west = new string[] { "1", "5", "9", "13" };

                Func<State, string> BlankPosition = (st) => { return st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item2 == "Blank").Item3; };
                Func<State, bool> CanFlipNorth = (st) => { return !invalid_north.Contains(BlankPosition(st)); };
                Func<State, bool> CanFlipEast = (st) => { return !invalid_east.Contains(BlankPosition(st)); };
                Func<State, bool> CanFlipSouth = (st) => { return !invalid_south.Contains(BlankPosition(st)); };
                Func<State, bool> CanFlipWest = (st) => { return !invalid_west.Contains(BlankPosition(st)); };

                Action<State> FlipNorth = (State st) =>
                {
                    var slot = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item2 == "Blank").Item3;
                    int pos = int.Parse(slot);
                    string brick = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos - 4).ToString()).Item2;
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos - 4).ToString()));
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos).ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", brick, pos.ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", "Blank", (pos - 4).ToString()));
                };
                Action<State> FlipEast = (State st) =>
                {
                    var slot = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item2 == "Blank").Item3;
                    int pos = int.Parse(slot);
                    string brick = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos + 1).ToString()).Item2;
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos + 1).ToString()));
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos).ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", brick, pos.ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", "Blank", (pos + 1).ToString()));
                };
                Action<State> FlipSouth = (State st) =>
                {
                    var slot = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item2 == "Blank").Item3;
                    int pos = int.Parse(slot);
                    string brick = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos + 4).ToString()).Item2;
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos + 4).ToString()));
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos).ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", brick, pos.ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", "Blank", (pos + 4).ToString()));
                };
                Action<State> FlipWest = (State st) =>
                {
                    var slot = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item2 == "Blank").Item3;
                    int pos = int.Parse(slot);
                    string brick = st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos - 1).ToString()).Item2;
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos - 1).ToString()));
                    st.Relations.Remove(st.Relations.Find(rel => rel.Item1 == "Board" && rel.Item3 == (pos).ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", brick, pos.ToString()));
                    st.Relations.Add(new Tuple<string, string, string>("Board", "Blank", (pos - 1).ToString()));
                };

                state.PlanningActions.Add(new PlanningAction("Flip North")
                                       .AssignPrejudicate(st => CanFlipNorth(st))
                                       .AssignPostAction(st => FlipNorth(st))
                                      );
                state.PlanningActions.Add(new PlanningAction("Flip East")
                                         .AssignPrejudicate(st => CanFlipEast(st))
                                         .AssignPostAction(st => FlipEast(st))
                                        );
                state.PlanningActions.Add(new PlanningAction("Flip South")
                                         .AssignPrejudicate(st => CanFlipSouth(st))
                                         .AssignPostAction(st => FlipSouth(st))
                                        );
                state.PlanningActions.Add(new PlanningAction("Flip West")
                                         .AssignPrejudicate(st => CanFlipWest(st))
                                         .AssignPostAction(st => FlipWest(st))
                                        );


                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 1", "1"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 2", "2"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 3", "13"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 4", "12"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 5", "11"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 6", "10"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 7", "9"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 8", "8"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 9", "7"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 10", "6"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 11", "5"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 12", "4"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 13", "3"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 14", "14"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Brick 15", "15"));
                state.Relations.Add(new Tuple<string, string, string>("Board", "Blank", "16"));
            }
            ).AssignGoal(
                     new Goal("Solve Puzzle").RelationalTarget("Board", "Brick 15", "16"));

            //puzzle.State.PlanningActions[0].Execute(puzzle.State);

            IPlan p = new DFSPlan().SetMaxSearchDepth(10);
            p.Search(puzzle.State, puzzle.Goal);
         
            Assert.GreaterOrEqual(puzzle.Goal.Fulfillment(p.GetFinalState()), 1.0);
            
        }
    }
}
