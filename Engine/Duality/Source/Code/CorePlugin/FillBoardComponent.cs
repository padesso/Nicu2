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
        Tilemap tilesGameBoard;

        int currentPlayer = 0;

        public void OnInit(InitContext context)
        {
            if (context == InitContext.Activate)
            {
                fillGrid = new FillGridModel();
                fillGrid.Initialize(20, 20);

                //Get a reference the to the game board
                tilesGameBoard = Scene.Current.FindComponent<Tilemap>(true);

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

        bool playing = true;
        Random rand = new Random();
        int plies = 4;

        public void OnUpdate()
        {
            if (playing)
            {
                // Player vs CPU
                if (currentPlayer == 0)
                {
                    if (DualityApp.Keyboard.KeyPressed(Duality.Input.Key.Number0))
                    {
                        fillGrid.FloodFill(currentPlayer, 0);
                        currentPlayer = 1;
                    }

                    if (DualityApp.Keyboard.KeyPressed(Duality.Input.Key.Number1))
                    {
                        fillGrid.FloodFill(currentPlayer, 1);
                        currentPlayer = 1;
                    }

                    if (DualityApp.Keyboard.KeyPressed(Duality.Input.Key.Number2))
                    {
                        fillGrid.FloodFill(currentPlayer, 2);
                        currentPlayer = 1;
                    }

                    if (DualityApp.Keyboard.KeyPressed(Duality.Input.Key.Number3))
                    {
                        fillGrid.FloodFill(currentPlayer, 3);
                        currentPlayer = 1;
                    }

                    if (DualityApp.Keyboard.KeyPressed(Duality.Input.Key.Number4))
                    {
                        fillGrid.FloodFill(currentPlayer, 4);
                        currentPlayer = 1;
                    }

                    if (DualityApp.Keyboard.KeyPressed(Duality.Input.Key.Number5))
                    {
                        fillGrid.FloodFill(currentPlayer, 5);
                        currentPlayer = 1;
                    }
                }
                else
                {
                    fillGrid.FloodFill(currentPlayer, fillGrid.BestMove(currentPlayer, plies));
                    currentPlayer = 0;
                }

                // CPU vs CPU
                //fillGrid.FloodFill(currentPlayer, fillGrid.BestMove(currentPlayer, plies));
                //currentPlayer = currentPlayer == 0 ? 1 : 0;

                UpdateGameBoardColors();

                if (fillGrid.Score(0) + fillGrid.Score(1) == fillGrid.SizeX * fillGrid.SizeY)
                    playing = false;
            }
        }

        private void UpdateGameBoardColors()
        {
            //populate the initial state of the game board
            for (int col = 0; col < fillGrid.SizeX; col++)
            {
                for (int row = 0; row < fillGrid.SizeY; row++)
                {
                    tilesGameBoard.SetTile(col, row, new Tile(fillGrid.Color(col, row)));
                }
            }
        }
    }
}
