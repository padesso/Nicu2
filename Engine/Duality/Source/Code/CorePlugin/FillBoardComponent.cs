using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality_.Model;
using System.Diagnostics;

namespace FloodFill
{
    public class FillBoardComponent : Component, ICmpInitializable, ICmpUpdatable
    {
        FillGridModel fillGrid;

        int currentPlayer = 0;

        public void OnInit(InitContext context)
        {
            if (context == InitContext.Activate)
            {
                fillGrid = new FillGridModel();
                fillGrid.Initialize(5, 5);

                //TestNegamaxSpeed(4);
                //TestRandomPlay();
            }
        }

        private void TestNegamaxSpeed(int plies)
        {
            Stopwatch clock = new Stopwatch();
            clock.Start();

            int bestMove = fillGrid.BestMove(0, plies);

            clock.Stop();
            long runTime = clock.ElapsedMilliseconds;
            Debug.WriteLine("Negamax with " + plies + " plies: " + runTime.ToString() + " ms.");
        }

        private void TestRandomPlay()
        {
            int numGamesEvenlyScored = 0;
            int numGamesUnEvenlyScored = 0;
            Random rand = new Random();

            for (int i = 0; i < 50000; i++)
            {
                int currentPlayer = 0;

                while (fillGrid.Score(0) + fillGrid.Score(1) < fillGrid.SizeX * fillGrid.SizeY)
                {
                    fillGrid.FloodFill(currentPlayer, rand.Next(0, 5));
                    currentPlayer = currentPlayer == 1 ? 0 : 1;
                }

                if (fillGrid.Score(0) + fillGrid.Score(1) == fillGrid.SizeX * fillGrid.SizeY)
                    numGamesEvenlyScored++;

                if (fillGrid.Score(0) + fillGrid.Score(1) > fillGrid.SizeX * fillGrid.SizeY)
                    numGamesUnEvenlyScored++;

                fillGrid.Initialize(5, 5);
            }

            Debug.WriteLine("Evenly Scored Games: " + numGamesEvenlyScored);
            Debug.WriteLine("Unevenly Scored Games: " + numGamesUnEvenlyScored);
        }

        public void OnShutdown(ShutdownContext context)
        {
            fillGrid = null;
        }

        int debugTickDelay = 50;
        int debugTickCount = 50;
        bool playing = true;
        Random rand = new Random();

        public void OnUpdate()
        {
            if (playing)
            {
                if (debugTickCount >= debugTickDelay)
                {
                    debugTickCount = 0;

                    fillGrid.FloodFill(currentPlayer, fillGrid.BestMove(currentPlayer, 3));

                    Debug.WriteLine(fillGrid.DebugPrintColors());

                    currentPlayer = currentPlayer == 1 ? 0 : 1;

                    //TODO: Why are there duplicates in the playerterritory lists?

                    if (fillGrid.Score(0) + fillGrid.Score(1) == fillGrid.SizeX * fillGrid.SizeY)
                        playing = false;
                }
                else
                {
                    debugTickCount++;
                }
            }
        }
    }
}
