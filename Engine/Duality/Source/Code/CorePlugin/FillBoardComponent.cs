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

        public void OnInit(InitContext context)
        {
            if (context == InitContext.Activate)
            {
                fillGrid = new FillGridModel();
                fillGrid.Initialize(20, 20);

                
            }
        }

        public void OnShutdown(ShutdownContext context)
        {
            fillGrid = null;
        }

        int debugTickDelay = 1000;
        int debugTickCount = 1000;

        public void OnUpdate()
        {
            if (debugTickCount >= debugTickDelay)
            {
                Debug.WriteLine(fillGrid.DebugPrintColors());
                debugTickCount = 0;

                //Random first player
                //Random rand = new Random();
                //fillGrid.FloodFill(0, rand.Next(0, 5));
                //fillGrid.FloodFill(1, rand.Next(0, 5));

                int bestMove = fillGrid.BestMove(1, 3);
                Debug.WriteLine("Best move for player 1: " + bestMove.ToString());
                fillGrid.FloodFill(1, bestMove);
            }
            else
            {
                debugTickCount++;
            }
        }
    }
}
