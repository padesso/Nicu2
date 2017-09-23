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

                int i = 1;
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

                Random rand = new Random();
                fillGrid.FloodFill(0, rand.Next(0, 5));
            }
            else
            {
                debugTickCount++;
            }
        }
    }
}
