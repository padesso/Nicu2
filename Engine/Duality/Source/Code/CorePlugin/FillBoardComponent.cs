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
                fillGrid.Initialize(20, 20);

                int numPlies = 3;
                Stopwatch clock = new Stopwatch();
                clock.Start();
                int bestMove = fillGrid.BestMove(currentPlayer, numPlies);
                clock.Stop();
                long runTime = clock.ElapsedMilliseconds;
                Debug.WriteLine("Negamax with " + numPlies + " plies: " + runTime.ToString() + " ms.");
            }
        }

        public void OnShutdown(ShutdownContext context)
        {
            fillGrid = null;
        }

        int debugTickDelay = 50;
        int debugTickCount = 50;
        bool playing = true;

        public void OnUpdate()
        {
            if(playing)
            {
                if (debugTickCount >= debugTickDelay)
                {                    
                    debugTickCount = 0;

                    //Random first player
                    Random rand = new Random();

                    //CPU play
                    int bestMove = fillGrid.BestMove(currentPlayer, 3);
                    fillGrid.FloodFill(currentPlayer, bestMove);

                    //Random play
                    //fillGrid.FloodFill(currentPlayer, rand.Next(0, 5));

                    Debug.WriteLine(fillGrid.DebugPrintColors());

                    currentPlayer = currentPlayer == 1 ? 0 : 1;

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
