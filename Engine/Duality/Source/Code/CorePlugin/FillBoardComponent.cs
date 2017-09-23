using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality_.Model;

namespace FloodFill
{
    public class FillBoardComponent : Component, ICmpInitializable
    {
        FillGridModel fillGrid;

        public void OnInit(InitContext context)
        {
            if (context == InitContext.Activate)
            {
                fillGrid = new FillGridModel();
                fillGrid.Initialize(20, 20);

                fillGrid.FloodFill(0, 1);
                fillGrid.FloodFill(1, 2);
                fillGrid.FloodFill(0, 2);
                fillGrid.FloodFill(1, 4);

                int i = 1;
            }
        }

        public void OnShutdown(ShutdownContext context)
        {
            fillGrid = null;
        }
    }
}
