using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality_.Model
{
    public class FillGridModel
    {
        private List<int[,]> boardColorList;
        private List<int[,]> boardOwnerList;

        protected int sizeX;
        protected int sizeY;

        protected int[,] boardColor;
        protected int[,] boardOwner;

        protected List<Stack<Point2>> playerTerritories;

        /// <summary>
        /// Create an instance of the game board.
        /// </summary>
        public FillGridModel()
        {
            sizeX = 0;
            sizeY = 0;
            boardColor = null;

            playerTerritories = new List<Stack<Point2>>(2);
            playerTerritories.Add(new Stack<Point2>(sizeX * sizeY));    //Player
            playerTerritories.Add(new Stack<Point2>(sizeX * sizeY));    //Enemy
        }

        /// <summary>
        /// Setup initial values for the game board.  Random colors and no ownership outside starting tiles.
        /// </summary>
        /// <param name="x">Number of columns.</param>
        /// <param name="y">Number of rows.</param>
        public void Initialize(int x, int y)
        {
            // In case we were already initialized, clean up the old stuff.
            Cleanup();

            sizeX = x;
            sizeY = y;

            // Create an array for colors.
            boardColor = CreateArray();

            Random rand = new Random(DateTime.Now.Millisecond);

            // Assign random colors to the array.
            for (int col = 0; col < sizeX; ++col)
                for (int row = 0; row < sizeY; ++row)
                    boardColor[col, row] = rand.Next(0, 5);

            // Create an array for owners.
            //boardOwner = CreateArray();

            // Assign "-1" to all owners to show a lack of ownership.
            boardOwner = InitArray(-1);

            // Give the player's their starting ownership
            boardOwner[0, 0] = 0;
            boardOwner[sizeX - 1, sizeY - 1] = 1;

            playerTerritories = new List<Stack<Point2>>(2);
            playerTerritories.Add(new Stack<Point2>(sizeX * sizeY));    //Player
            playerTerritories.Add(new Stack<Point2>(sizeX * sizeY));    //Enemy

            playerTerritories[0].Push(new Point2(0, 0));
            playerTerritories[1].Push(new Point2(sizeX - 1, sizeY - 1));

            // An easy way to expand the initial ownership so that
            // we get all of the matching adjacent colors.
            FloodFill(0, boardColor[0, 0]);
            FloodFill(1, boardColor[sizeX - 1, sizeY - 1]);
        }

        /// <summary>
        /// Returns the colors of the board as a string.
        /// </summary>
        /// <returns>The game board colors.</returns>
        public string DebugPrintColors()
        {
            string board = string.Empty;

            for(int col = 0; col < sizeX; col++)
            {
                for(int row = 0; row < sizeY; row++)
                {
                    board += Color(col,row).ToString() + " ";
                }
                board += "\n";
            }

            return board;
        }
        
        /// <summary>
        /// Release the memory of the board data structures.
        /// </summary>
        private void Cleanup()
        {
            if(boardColor != null)
            {
                boardColor = null; 
            }

            if(boardOwner != null)
            {
                boardOwner = null;
            }
        }
        
        /// <summary>
        /// Game AI routine.  Calls itself recursively to evaluate best move for given number of iterations (plies).
        /// </summary>
        /// <param name="depth">Number of plies to evaluate.</param>
        /// <param name="alpha">Alpha pruning parameter.</param>
        /// <param name="beta">Beta pruning parameter.</param>
        /// <param name="owner">Which player to evaluate turn.</param>
        /// <param name="bestMove">Reference to best color to select.</param>
        /// <returns>A float that is evaluated to determine best move.</returns>
        private float Negamax(int depth, float alpha, float beta, int owner, ref int bestMove )
        {
            //This checks for end game or end of AI evaluation
            if (depth == 0 || (Score(0) + Score(1) == sizeX * sizeY))
            {
                // Evaluation of the game at this point:
                float evaluation = Score(0) - Score(1) - depth * 0.001f;
                float locScore = 0;

                //Loop through all tiles and score all owned tiles.  
                for (int col = 0; col < sizeX; col++)
                {
                    for (int row = 0; row < sizeY; row++)
                    {
                        if (boardOwner[col, row] == -1)
                            continue;

                        locScore += col > sizeX / 2 ? sizeX - col : col;
                        locScore += row > sizeY / 2 ? sizeY - row : row;

                        //Bad score (positive) for owner 0, good for AI
                        if (boardOwner[col, row] == 0)
                        {
                            locScore *= 0.00001f;
                        }
                        else
                        {
                            locScore *= -0.00001f;
                        }
                    }
                }
                evaluation += locScore;

                //Flip score for AI so we can run this for both players
                if (owner == 0)
                {
                    return evaluation;
                }
                else
                {
                    return -evaluation;
                }
            }

            // Make copies of the current arrays and territories so we can restore them.
            boardColorList[depth - 1] = (int[,])boardColor.Clone();
            boardOwnerList[depth - 1] = (int[,])boardOwner.Clone();

            int[,] origColor = boardColorList[depth - 1];
            int[,] origOwner = boardOwnerList[depth - 1];

            List<Stack<Point2>> origTerritory = new List<Stack<Point2>>(2) { playerTerritories[0], playerTerritories[1] };

            float currBestValue = -sizeX * sizeY;
            int currBestMove = -1; 

            //Test score for all colors (depth first since it calls recursively)
            for (int testColor = 0; testColor <= 5; testColor++)
            {
                if (LastSelectedColor(owner) == testColor)
                    continue;

                // Apply move
                FloodFill(owner, testColor);

                // Determine its value for this color
                float val = -Negamax(depth - 1, -beta, -alpha, 1 - owner, ref currBestMove);

                // Undo move
                boardColor = (int[,])origColor.Clone();
                boardOwner = (int[,])origOwner.Clone();
                playerTerritories[0] = new Stack<Point2>(origTerritory[0]);
                playerTerritories[1] = new Stack<Point2>(origTerritory[1]);

                // If the move beats our current best option, set it.
                if (val > currBestValue)
                {
                    currBestValue = val;
                    bestMove = testColor;
                }

                // Alpha-Beta Pruning (check Wikipedia)
                if (val > alpha)
                    alpha = val;
                if (alpha >= beta)
                    break;
            }

            return currBestValue;
        }
       
        /// <summary>
        /// Create an empty two dimensional array.
        /// </summary>
        /// <returns>An empty two dimensional integer array.</returns>
        protected int[,] CreateArray()
        {
            return new int[sizeX, sizeY];
        }

        /// <summary>
        /// Seeds an array with an initial value.
        /// </summary>
        /// <param name="value">Initial value to seed array with.</param>
        /// <returns>Seeded two dimensional integer array.</returns>
        protected int[,] InitArray(int value)
        {
            int[,] a = new int[sizeX,sizeY];

            for(int col = 0; col < sizeX; ++col )
                for (int row = 0; row < sizeY; ++row)
                    a[col,row] = value;

            return a;
        }

        /// <summary>
        /// Get the color assigned to a tile of given coordinates.
        /// </summary>
        /// <param name="x">Column to evaluate.</param>
        /// <param name="y">Row to evaluate.</param>
        /// <returns>Color of tile for given coordinates.  -1 for invalid tile.</returns>
        public int Color(int x, int y)
        {
            if (x < 0 || x > sizeX || y < 0 || y > sizeY)
                return -1;

            return boardColor[x, y];
        }

        /// <summary>
        /// Get the owner assigned to a tile of given coordinates.
        /// </summary>
        /// <param name="x">Column to evaluate.</param>
        /// <param name="y">Row to evaluate.</param>
        /// <returns>Player ID for tile at given coordinates.  -1 for invalid tile. </returns>
        public int Owner(int x, int y)
        {
            if (x < 0 || x > sizeX || y < 0 || y > sizeY)
                return -1;

            return boardOwner[x, y];
        }

        /// <summary>
        /// Get the last selected color by looking at the color of the origin tile.
        /// </summary>
        /// <param name="owner">Which player we are fetching the last selected color for.</param>
        /// <returns></returns>
        public int LastSelectedColor(int owner)
        {
            if (owner == 0)
                return boardColor[0, 0];

            return boardColor[sizeX - 1, sizeY - 1];
        }

        /// <summary>
        /// Score is the number of tiles a player owns.
        /// </summary>
        /// <param name="owner">Which player we want to score.</param>
        /// <returns></returns>
        public int Score(int owner)
        {
            return playerTerritories[owner].Count();
        }

        /// <summary>
        /// Exapnad the player territoriy into all adjacent tiles that match the selected color.
        /// </summary>
        /// <param name="owner">Which player to flood fill.</param>
        /// <param name="color">Which color to expand the player territory to.</param>
        public void FloodFill(int owner, int color)
        {
            // Change all of our current territory to the new color.
            int territoryCount = playerTerritories[owner].Count();
            for (int i = territoryCount - 1; i >= 0; i--)
            {
                Point2 p = playerTerritories[owner].ElementAt(i);
                boardColor[p.X, p.Y] = color;
            }

            // Create the position stack
            //Vector<Point2I> posStack(30);
            Stack<Point2> posStack = new Stack<Point2>(30);

            // For every tile we own, we'll look at the adjacent tiles.
            // If those tiles are unowned,
            //    we'll take ownership and put them on a stack
            //    so that we can search beyond them.
            for (int i = territoryCount - 1; i >= 0; i--)
            {
                Point2 p = playerTerritories[owner].ElementAt(i);

                if (p.X > 0 && boardOwner[p.X - 1, p.Y] == -1)
                    posStack.Push(new Point2(p.X - 1, p.Y));

                if (p.X < sizeX - 1 && boardOwner[p.X + 1, p.Y] == -1)
                    posStack.Push(new Point2(p.X + 1, p.Y));

                if (p.Y > 0 && boardOwner[p.X, p.Y - 1] == -1)
                    posStack.Push(new Point2(p.X, p.Y - 1));

                if (p.Y < sizeY - 1 && boardOwner[p.X, p.Y + 1] == -1)
                    posStack.Push(new Point2(p.X, p.Y + 1));
            }

            // Switch owners
            while (posStack.Count() > 0)
            {
                // Pop off the top item.
                //Point2 p = posStack.First();
                Point2 p = posStack.Pop();

                // A tile can only be on the stack at this point if it wasn't owned
                // by anybody.  We can safely claim it if matches our current color.
                // Note: This algorithm may have already switched the owner, so we
                // make sure that it's still unowned.
                if (boardColor[p.X, p.Y] == color && boardOwner[p.X, p.Y] == -1)
                {
                    playerTerritories[owner].Push(p);
                    boardOwner[p.X, p.Y] = owner;

                    // Push adjacent tiles onto the stack
                    if (p.X > 0 && boardOwner[p.X - 1, p.Y] == -1)
                        posStack.Push(new Point2(p.X - 1, p.Y));

                    if (p.X < sizeX - 1 && boardOwner[p.X + 1, p.Y] == -1)
                        posStack.Push(new Point2(p.X + 1, p.Y));

                    if (p.Y > 0 && boardOwner[p.X, p.Y - 1] == -1)
                        posStack.Push(new Point2(p.X, p.Y - 1));

                    if (p.Y < sizeY - 1 && boardOwner[p.X, p.Y + 1] == -1)
                        posStack.Push(new Point2(p.X, p.Y + 1));
                }
            }

            FillTrapped(owner, color);
        }

        /// <summary>
        /// Determine if tiles are completely surrounded by a player territory and fill them in, if so.
        /// </summary>
        /// <param name="owner">Which player is evaluating trapped tiles.</param>
        /// <param name="color">What color the trapped tiles should be set to.</param>
        private void FillTrapped(int owner, int color)
        {
            // I'm pre-sizing these vectors so that they don't have to
            // grow while the algorithm is running.  Note: they still
            // are size = 0, it's just that they have space for 200
            // objects before any memory management has to occur.
            Stack<Point2> trappedRegion = new Stack<Point2>(200);
            Stack<Point2> searchStack = new Stack<Point2>(200);

            // While we iterate over the array of tiles, we want to skip
            // any un-owned tile that we already visited earlier in the
            // algorithm.
            //int[,] visited = CreateArray();
            int[,] visited = InitArray(0); //init to 0 (false)

            // Iterate over the entire array
            for (int col = 0; col < sizeX; ++col)
            {
                for (int row = 0; row < sizeY; ++row)
                {
                    // Skip the tile if it's owned by somebody.
                    if (boardOwner[col,row] != -1)
                        continue;

                    // Skip the tile if we've already looked at it.
                    if (visited[col,row] == 1)
                        continue;

                    // Push the unowned tile onto the stack, then
                    // set the list of trapped tiles to be empty.
                    searchStack.Push(new Point2(col, row));
                    trappedRegion.Clear();

                    // The following code will get a group of un-owned tiles
                    // that are together (trappedRegion), and will mark 
                    // all of those tiles as visited.
                    while (searchStack.Count() > 0)
                    {
                        // Pop off the top of the stack.
                        Point2 p = searchStack.Pop();
                        //searchStack.Pop();

                        if (visited[p.X, p.Y] == 1)
                            continue;

                        // Set as visited.
                        visited[p.X, p.Y] = 1;

                        // Add the tile to trappedRegion.
                        trappedRegion.Push(p);

                        // Push adjacent tiles onto the stack.
                        // Note: we never push a tile onto the stack if it
                        // belong to somebody, so we don't need to check
                        // for that in this depth-first search.
                        if (p.X > 0 && boardOwner[p.X - 1, p.Y] == -1)
                            searchStack.Push(new Point2(p.X - 1, p.Y));

                        if (p.X < sizeX - 1 && boardOwner[p.X + 1, p.Y] == -1)
                            searchStack.Push(new Point2(p.X + 1, p.Y));

                        if (p.Y > 0 && boardOwner[p.X, p.Y - 1] == -1)
                            searchStack.Push(new Point2(p.X, p.Y - 1));

                        if (p.Y < sizeY - 1 && boardOwner[p.X, p.Y + 1] == -1)
                            searchStack.Push(new Point2(p.X, p.Y + 1));
                    }

                    // We've now found a contiguous group of un-owned tiles
                    // (trappedRegion).  If any of those tiles are adjacent to
                    // the opponent, then we're going to ignore the list.
                    //
                    // There's 3 cases here:
                    // * Un-owned region is adjacent to only the current player
                    // * Un-owned region is adjacent to both players
                    // * Un-owned region is adjacent to only the opponent
                    //
                    // The third case shouldn't happen because those tiles should
                    // have been filled in on his turn.  Therefore, we only will
                    // have the first 2 cases.  And in those 2, the only time
                    // that the tiles aren't trapped is if they happen to touch
                    // the opponent.

                    // We're going to assume it's a trapped region to start.  The
                    // for-loop will bail when we get to the end of the list *OR*
                    // whenever we find that the region isn't trapped.
                    bool isATrappedRegion = true;
                    int regionSize = trappedRegion.Count();
                    for (int i = 0; i < regionSize && isATrappedRegion; ++i)
                    {
                        Point2 p = trappedRegion.ElementAt(i);
                        if (p.X > 0 && boardOwner[p.X - 1, p.Y] != -1 && boardOwner[p.X - 1, p.Y] != owner)
                        {
                            isATrappedRegion = false;
                        }
                        if (p.X < sizeX - 1 && boardOwner[p.X + 1, p.Y] != -1 && boardOwner[p.X + 1, p.Y] != owner)
                        {
                            isATrappedRegion = false;
                        }
                        if (p.Y > 0 && boardOwner[p.X, p.Y - 1] != -1 && boardOwner[p.X, p.Y - 1] != owner)
                        {
                            isATrappedRegion = false;
                        }
                        if (p.Y < sizeY - 1 && boardOwner[p.X, p.Y + 1] != -1 && boardOwner[p.X, p.Y + 1] != owner)
                        {
                            isATrappedRegion = false;
                        }
                    }

                    // We got through the loop!  Are we *still* a trapped region?
                    if (isATrappedRegion)
                    {
                        for (int i = 0; i < regionSize; ++i)
                        {
                            Point2 p = trappedRegion.ElementAt(i);
                            boardOwner[p.X, p.Y] = owner;
                            boardColor[p.X, p.Y] = color;
                            playerTerritories[owner].Push(p);
                        }
                    }
                } // Next row
            } // Next col
        }

        /// <summary>
        /// Get the best move for the selected player for a given number of AI iterations.
        /// </summary>
        /// <param name="owner">Which player to find the best move for.</param>
        /// <param name="plies">Number of iterations the AI will evaluate.</param>
        /// <returns></returns>
        public int BestMove(int owner, int plies)
        {
            CreateBoardColorOwnerLists(plies);

            // Init to an invaild move.
            int bestMove = -1;
            Negamax(plies, -sizeX * sizeY, sizeX * sizeY, owner, ref bestMove);

            DeleteBoardColorOwnerLists(plies);

            return bestMove;
        }

        /// <summary>
        /// Create temporary boards to evaluate by AI.
        /// </summary>
        /// <param name="depth">Number of temporary boards to be created.</param>
        private void CreateBoardColorOwnerLists(int depth)
        {
            boardColorList = new List<int[,]>(depth);
            boardOwnerList = new List<int[,]>(depth);
            for (int i = 0; i < depth; i++)
            {
                boardColorList.Add(InitArray(-1));
                boardOwnerList.Add(InitArray(-1));
            }
        }

        private void DeleteBoardColorOwnerLists(int depth)
        {
            boardColorList.Clear();
            boardOwnerList.Clear();
        }

        /// <summary>
        /// Number of columns in the grid.
        /// </summary>
        public int SizeX
        {
            get
            {
                return sizeX;
            }

            set
            {
                sizeX = value;
            }
        }

        /// <summary>
        /// Number of rows in the grid.
        /// </summary>
        public int SizeY
        {
            get
            {
                return sizeY;
            }

            set
            {
                sizeY = value;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~FillGridModel()
        {
            Cleanup();
        }
    }
}
