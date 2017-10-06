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

                int numPlies = 6;
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

        int debugTickDelay = 100;
        int debugTickCount = 100;

        public void OnUpdate()
        {
            if (debugTickCount >= debugTickDelay)
            {
                Debug.WriteLine(fillGrid.DebugPrintColors());
                debugTickCount = 0;

                //Random first player
                Random rand = new Random();
                
                int bestMove = fillGrid.BestMove(currentPlayer, 3);               
                fillGrid.FloodFill(currentPlayer, bestMove);

                currentPlayer = currentPlayer == 1 ? 0 : 1;
            }
            else
            {
                debugTickCount++;
            }
        }
    }
}
