using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality_.Model;
using System.Diagnostics;
using Duality.Plugins.Tilemaps;
using Duality.Resources;

namespace FloodFill
{
    public class FillBoardComponent : Component, ICmpInitializable, ICmpUpdatable
    {
        FillGridModel fillGrid;
        Tilemap gameBoard;

        int currentPlayer = 0;

        public void OnInit(InitContext context)
        {
            if (context == InitContext.Activate)
            {
                fillGrid = new FillGridModel();
                fillGrid.Initialize(20, 20);

                //Get a reference the to the game board
                gameBoard = Scene.Current.FindComponent<Tilemap>(true);

                UpdateGameBoardColors();

                Debug.WriteLine("Wait...");
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

                fillGrid.Initialize(20, 20);
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
        int plies = 4;

        public void OnUpdate()
        {
            if (playing)
            {
                if (debugTickCount >= debugTickDelay)
                {
                    debugTickCount = 0;

                    fillGrid.FloodFill(currentPlayer, fillGrid.BestMove(currentPlayer, plies));

                    UpdateGameBoardColors();

                    //Debug.WriteLine(fillGrid.DebugPrintColors());

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

        private void UpdateGameBoardColors()
        {
            //populate the initial state of the game board
            for (int col = 0; col < fillGrid.SizeX; col++)
            {
                for (int row = 0; row < fillGrid.SizeY; row++)
                {
                    gameBoard.SetTile(col, row, new Tile(fillGrid.Color(col, row)));
                }
            }
        }
    }
}
