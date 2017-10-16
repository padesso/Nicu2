using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality_.Model;
using System.Diagnostics;
using Duality.Plugins.Tilemaps;
using Duality.Resources;
using Duality.Components;

namespace FloodFill
{
    public class FillBoardComponent : Component, ICmpInitializable, ICmpUpdatable
    {
        FillGridModel fillGrid;
        Tilemap tilesGameBoard;
        Camera camera;

        bool playing = true;
        Random rand = new Random();
        int plies = 5;
        int currentPlayer = 0;

        public void OnInit(InitContext context)
        {
            if (context == InitContext.Activate)
            {
                //Get a reference the to the game board
                tilesGameBoard = Scene.Current.FindComponent<Tilemap>(true);
                camera = Scene.Current.FindComponent<Camera>(true);

                fillGrid = new FillGridModel();
                fillGrid.Initialize(tilesGameBoard.Size.X, tilesGameBoard.Size.Y);

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

        public void OnUpdate()
        {
            if (playing)
            {
                // Player vs CPU
                if (currentPlayer == 0)
                {
                    if(DualityApp.Mouse.ButtonHit(Duality.Input.MouseButton.Left))
                    {              
                        Vector2 tileIndex = GetTilePosFromWorldPos(camera.GetSpaceCoord(DualityApp.Mouse.Pos).Xy);
                        Tile clickedTile = tilesGameBoard.Tiles[(int)tileIndex.X, (int)tileIndex.Y];
                        fillGrid.FloodFill(currentPlayer, clickedTile.Index);
                        currentPlayer = 1;
                    }

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

        private Point2 GetTilePosFromWorldPos(Vector2 worldPos)
        {
            float gridMinX = tilesGameBoard.GameObj.Transform.Pos.X - (tilesGameBoard.Size.X * ((Tileset)tilesGameBoard.Tileset).TileSize.X) / 2;
            float gridMinY = tilesGameBoard.GameObj.Transform.Pos.Y - (tilesGameBoard.Size.Y * ((Tileset)tilesGameBoard.Tileset).TileSize.Y) / 2;

            int xTileIndex = (int)((worldPos.X - gridMinX) / ((Tileset)tilesGameBoard.Tileset).TileSize.X);
            int yTileIndex = (int)((worldPos.Y - gridMinY) / ((Tileset)tilesGameBoard.Tileset).TileSize.Y);

            return new Point2(xTileIndex, yTileIndex);
        }
    }
}
