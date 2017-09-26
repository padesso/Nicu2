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
        private int sizeX;
        private int sizeY;

        private Stack<int[,]> myColorStack;
        private Stack<int[,]> myOwnerStack;

        protected int mySizeX;
        protected int mySizeY;

        protected int[,] myColor;
        protected int[,] myOwner;

        protected List<Stack<Point2>> playerTerritories; //Can't believe this shit works...

        public FillGridModel()
        {
            mySizeX = 0;
            mySizeY = 0;
            myColor = null;

            //TODO: pass number of players and instantiate in a loop
            playerTerritories = new List<Stack<Point2>>(2);
            playerTerritories.Add(new Stack<Point2>(mySizeX * mySizeY));    //Player
            playerTerritories.Add(new Stack<Point2>(mySizeX * mySizeY));    //Enemy
        }

        public string DebugPrintColors()
        {
            string board = string.Empty;

            for(int col = 0; col < mySizeX; col++)
            {
                for(int row = 0; row < mySizeY; row++)
                {
                    board += Color(col,row).ToString() + " ";
                }
                board += "\n";
            }

            return board;
        }
        
        private void Cleanup()
        {
            if(myColor != null)
            {
                myColor = null;  //TODO: verify this trashes all elements properly
            }

            if(myOwner != null)
            {
                myOwner = null;  //TODO: verify this trashes all elements properly
            }
        }

        private void CreateColorOwnerStack(int depth)
        {
            myColorStack = new Stack<int[,]>(depth);
            myOwnerStack = new Stack<int[,]>(depth);
            for(int i = 0; i < depth; i++)
            {
                myColorStack.Push(CreateArray());
                myOwnerStack.Push(CreateArray());
            }
        }

        private void DeleteColorOwnerStack(int depth)
        {
            myColorStack.Clear();
            myOwnerStack.Clear();
        }

        private float Negamax(int depth, float alpha, float beta, int owner, int? bestMove )
        {
            if (depth == 0 || (Score(0) + Score(1) == mySizeX * mySizeY))
            {
                // Evaluation of the game at this point:
                float evaluation = Score(0) - Score(1) - depth * 0.001f;
                float locScore = 0;

                for (int col = 0; col < mySizeX; col++)
                {
                    for (int row = 0; row < mySizeY; row++)
                    {
                        if (myOwner[col, row] == -1)
                            continue;

                        locScore += col > mySizeX / 2 ? mySizeX - col : col;
                        locScore += row > mySizeY / 2 ? mySizeY - row : row;

                        if (myOwner[col, row] == 0)
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
            int[,] origColor = myColorStack.ElementAt(depth - 1);
            int[,] origOwner = myOwnerStack.ElementAt(depth - 1);
            AssignArray(ref origColor, ref myColor);
            AssignArray(ref origOwner, ref myOwner);

            List<Stack<Point2>> origTerritory = new List<Stack<Point2>>(2) { playerTerritories[0], playerTerritories[1] };

            float currBestValue = -mySizeX * mySizeY;
            int? currBestMove = null; //TODO: this seems extraneous...

            for (int testColor = 0; testColor <= 5; testColor++)
            {
                if (LastSelectedColor(owner) == testColor)
                    continue;

                // Apply move
                FloodFill(owner, testColor);

                // Determine its value
                float val = -Negamax(depth - 1, -beta, -alpha, 1 - owner, currBestMove);

                // Undo move
                AssignArray(ref myColor, ref origColor);
                AssignArray(ref myOwner, ref origOwner);
                playerTerritories[0] = origTerritory[0];
                playerTerritories[1] = origTerritory[1];

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

        protected void AssignArray(ref int[,] into, ref int[,] from)
        {
            for (int col = 0; col < mySizeX; ++col)
            {
                for (int row = 0; row < mySizeY; row++)
                {
                    into[col, row] = from[col, row];
                }
            }
        }

        public void Initialize(int x, int y)
        {
            // In case we were already initialized, clean up the old stuff.
            Cleanup();

            mySizeX = x;
            mySizeY = y;

            // Create an array for colors.
            myColor = CreateArray();

            Random rand = new Random(DateTime.Now.Millisecond);

            // Assign random colors to the array.
            for (int col = 0; col < mySizeX; ++col)
                for (int row = 0; row < mySizeY; ++row)
                    myColor[col, row] =  rand.Next(0, 5);

            // Create an array for owners.
            myOwner = CreateArray();

            // Assign "-1" to all owners to show a lack of ownership.
            myOwner = InitArray(-1);

            // Give the player's their starting ownership
            //TODO: handle multiplayer
            myOwner[0, 0] = 0;
            myOwner[mySizeX - 1, mySizeY - 1] = 1;
            playerTerritories[0].Push(new Point2(0, 0));
            playerTerritories[1].Push(new Point2(mySizeX - 1, mySizeY - 1));

            // An easy way to expand the initial ownership so that
            // we get all of the matching adjacent colors.
            FloodFill(0, myColor[0, 0]);
            FloodFill(1, myColor[mySizeX - 1, mySizeY - 1]);
        }

        protected int[,] CreateArray()
        {
            return new int[mySizeX, mySizeY];
        }

        protected int[,] InitArray(int value)
        {
            int[,] a = new int[mySizeX,mySizeY];

            for(int col = 0; col < mySizeX; ++col )
                for (int row = 0; row < mySizeY; ++row)
                    a[col,row] = value;

            return a;
        }

        public int Color(int x, int y)
        {
            if (x < 0 || x > mySizeX || y < 0 || y > mySizeY)
                return -1;

            return myColor[x, y];
        }

        public int Owner(int x, int y)
        {
            if (x < 0 || x > mySizeX || y < 0 || y > mySizeY)
                return -1;

            return myOwner[x, y];
        }

        public int LastSelectedColor(int owner)
        {
            if (owner == 0)
                return myColor[0, 0];

            return myColor[mySizeX - 1, mySizeY - 1];
        }

        public int Score(int owner)
        {
            return playerTerritories[owner].Count();
        }

        public void FloodFill(int owner, int color)
        {
            // Change all of our current territory to the new color.
            int cTerritory = playerTerritories[owner].Count();
            for (int i = 0; i < cTerritory; i++)
            {
                Point2 p = playerTerritories[owner].ElementAt(i);
                myColor[p.X, p.Y] = color;
            }

            // Create the position stack
            //Vector<Point2I> posStack(30);
            Stack<Point2> posStack = new Stack<Point2>(30);

            // For every tile we own, we'll look at the adjacent tiles.
            // If those tiles are unowned,
            //    we'll take ownership and put them on a stack
            //    so that we can search beyond them.
            for (int i = 0; i < cTerritory; ++i)
            {
                Point2 p = playerTerritories[owner].ElementAt(i);

                if (p.X > 0 && myOwner[p.X - 1, p.Y] == -1)
                    posStack.Push(new Point2(p.X - 1, p.Y));

                if (p.X < mySizeX - 1 && myOwner[p.X + 1, p.Y] == -1)
                    posStack.Push(new Point2(p.X + 1, p.Y));

                if (p.Y > 0 && myOwner[p.X, p.Y - 1] == -1)
                    posStack.Push(new Point2(p.X, p.Y - 1));

                if (p.Y < mySizeY - 1 && myOwner[p.X, p.Y + 1] == -1)
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
                if (myColor[p.X, p.Y] == color && myOwner[p.X, p.Y] == -1)
                {
                    playerTerritories[owner].Push(p);
                    myOwner[p.X, p.Y] = owner;

                    // Push adjacent tiles onto the stack
                    if (p.X > 0 && myOwner[p.X - 1, p.Y] == -1)
                        posStack.Push(new Point2(p.X - 1, p.Y));

                    if (p.X < mySizeX - 1 && myOwner[p.X + 1, p.Y] == -1)
                        posStack.Push(new Point2(p.X + 1, p.Y));

                    if (p.Y > 0 && myOwner[p.X, p.Y - 1] == -1)
                        posStack.Push(new Point2(p.X, p.Y - 1));

                    if (p.Y < mySizeY - 1 && myOwner[p.X, p.Y + 1] == -1)
                        posStack.Push(new Point2(p.X, p.Y + 1));
                }
            }

            FillTrapped(owner, color);
        }

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
            for (int col = 0; col < mySizeX; ++col)
            {
                for (int row = 0; row < mySizeY; ++row)
                {
                    // Skip the tile if it's owned by somebody.
                    if (myOwner[col,row] != -1)
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
                        if (p.X > 0 && myOwner[p.X - 1, p.Y] == -1)
                            searchStack.Push(new Point2(p.X - 1, p.Y));

                        if (p.X < mySizeX - 1 && myOwner[p.X + 1, p.Y] == -1)
                            searchStack.Push(new Point2(p.X + 1, p.Y));

                        if (p.Y > 0 && myOwner[p.X, p.Y - 1] == -1)
                            searchStack.Push(new Point2(p.X, p.Y - 1));

                        if (p.Y < mySizeY - 1 && myOwner[p.X, p.Y + 1] == -1)
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
                        if (p.X > 0 && myOwner[p.X - 1, p.Y] != -1 && myOwner[p.X - 1, p.Y] != owner)
                        {
                            isATrappedRegion = false;
                        }
                        if (p.X < mySizeX - 1 && myOwner[p.X + 1, p.Y] != -1 && myOwner[p.X + 1, p.Y] != owner)
                        {
                            isATrappedRegion = false;
                        }
                        if (p.Y > 0 && myOwner[p.X, p.Y - 1] != -1 && myOwner[p.X, p.Y - 1] != owner)
                        {
                            isATrappedRegion = false;
                        }
                        if (p.Y < mySizeY - 1 && myOwner[p.X, p.Y + 1] != -1 && myOwner[p.X, p.Y + 1] != owner)
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
                            myOwner[p.X, p.Y] = owner;
                            myColor[p.X, p.Y] = color;
                            playerTerritories[owner].Push(p);
                        }
                    }
                } // Next row
            } // Next col
        }

        public int? BestMove(int owner, int plies)
        {
            CreateColorOwnerStack(plies);

            // Init to an invaild move.
            int? bestMove = -1;
            Negamax(plies, -mySizeX * mySizeY, mySizeX * mySizeY, owner, bestMove);

            DeleteColorOwnerStack(plies);

            return bestMove;
        }

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
