#include "platform/platform.h"
#include "console/simBase.h"
#include "console/consoleTypes.h"
#include "console/simBase.h"
#include "math/mRandom.h"
#include "math/mPoint.h"

#include "BobbyPointClick/fillGridModel.h"

IMPLEMENT_CONOBJECT(FillGridModel);

FillGridModel::FillGridModel() : ScriptObject(),
  mySizeX( 0 ),
  mySizeY( 0 ),
  myColor( NULL ),
  myOwner( NULL )
{
  myTerritory[0].reserve( mySizeX * mySizeY );
  myTerritory[1].reserve( mySizeX * mySizeY );
}

FillGridModel::~FillGridModel()
{
  cleanup();
}

void FillGridModel::cleanup()
{
  if( myColor != NULL )
  {
    deleteArray( myColor );
    myColor = NULL;
  }

  if( myOwner != NULL )
  {
    deleteArray( myOwner );
    myOwner = NULL;
  }
}

bool FillGridModel::onAdd()
{
  if (!Parent::onAdd())
    return false;

  return true;
}

void FillGridModel::onRemove()
{
  Parent::onRemove();
}

void FillGridModel::initialize( S32 x, S32 y )
{
  // In case we were already initialized, clean up the old stuff.
  cleanup();

  mySizeX = x;
  mySizeY = y;

  // Create an array for colors.
  myColor = createArray();
  
  // Assign random colors to the array.
  for( S32 col = 0; col < mySizeX; ++col )
    for( S32 row = 0; row < mySizeY; ++row )
      myColor[col][row] = gRandGen.randI( 0, 5 );

  // Create an array for owners.
  myOwner = createArray();
  
  // Assign "-1" to all owners to show a lack of ownership.
  initArray( myOwner, -1 );

  // Give the player's their starting ownership
  myOwner[0][0] = 0;
  myOwner[mySizeX-1][mySizeY-1] = 1;
  myTerritory[0].push_back( Point2I( 0, 0 ) );
  myTerritory[1].push_back( Point2I( mySizeX-1, mySizeY-1 ) );

  // An easy way to expand the initial ownership so that
  // we get all of the matching adjacent colors.
  floodFill( 0, myColor[0][0] );
  floodFill( 1, myColor[mySizeX-1][mySizeY-1] );
}

S32 FillGridModel::sizeX()
{
  return mySizeX;
}

S32 FillGridModel::sizeY()
{
  return mySizeY;
}

S32 FillGridModel::color( S32 x, S32 y )
{
  if( x < 0 || x >= mySizeX || y < 0 || y >= mySizeY )
    return -1;
  return myColor[x][y];
}

S32 FillGridModel::owner( S32 x, S32 y )
{
  if( x < 0 || x >= mySizeX || y < 0 || y >= mySizeY )
    return -1;
  return myOwner[x][y];
}

S32 FillGridModel::lastSelectedColor( S32 owner )
{
  if( owner == 0 ) return myColor[0][0];
  return myColor[mySizeX-1][mySizeY-1];
}

void FillGridModel::floodFill( S32 owner, S32 color )
{
  // Change all of our current territory to the new color.
  S32 cTerritory = myTerritory[owner].size();
  for( S32 i = 0; i < cTerritory; ++i )
  {
    const Point2I& p = myTerritory[owner][i];
    myColor[p.x][p.y] = color;
  }
  
  // Create the position stack
  Vector<Point2I> posStack( 30 );

  // For every tile we own, we'll look at the adjacent tiles.
  // If those tiles are unowned,
  //    we'll take ownership and put them on a stack
  //    so that we can search beyond them.
  for( S32 i = 0; i < cTerritory; ++i )
  {
    const Point2I& p = myTerritory[owner][i];

    if( p.x > 0 && myOwner[p.x - 1][p.y] == -1 )
      posStack.push_back( Point2I( p.x - 1, p.y ) );
    if( p.x < mySizeX - 1 && myOwner[p.x + 1][p.y] == -1 )
      posStack.push_back( Point2I( p.x + 1, p.y ) );
    if( p.y > 0 && myOwner[p.x][p.y - 1] == -1 )
      posStack.push_back( Point2I( p.x, p.y - 1 ) );
    if( p.y < mySizeY - 1 && myOwner[p.x][p.y + 1] == -1 )
      posStack.push_back( Point2I( p.x, p.y + 1 ) );
  }

  // Switch owners
  while( posStack.size() > 0 )
  {
    // Pop off the last item.
    Point2I p = posStack.last();
    posStack.pop_back();

    // A tile can only be on the stack at this point if it wasn't owned
    // by anybody.  We can safely claim it if matches our current color.
    // Note: This algorithm may have already switched the owner, so we
    // make sure that it's still unowned.
    if( myColor[p.x][p.y] == color && myOwner[p.x][p.y] == -1 )
    {
      myTerritory[owner].push_back( p );
      myOwner[p.x][p.y] = owner;
      
      // Push adjacent tiles onto the stack
      if( p.x > 0 && myOwner[p.x - 1][p.y] == -1 )
        posStack.push_back( Point2I( p.x - 1, p.y ) );
      if( p.x < mySizeX - 1 && myOwner[p.x + 1][p.y] == -1 )
        posStack.push_back( Point2I( p.x + 1, p.y ) );
      if( p.y > 0 && myOwner[p.x][p.y - 1] == -1 )
        posStack.push_back( Point2I( p.x, p.y - 1 ) );
      if( p.y < mySizeY - 1 && myOwner[p.x][p.y + 1] == -1 )
        posStack.push_back( Point2I( p.x, p.y + 1 ) );
    }
  }
}

S32 FillGridModel::bestMove( S32 owner, S32 plies )
{
  createColorOwnerStack( plies );

  // Init to an invaild move.
  S32 bestMove = -1;
  negamax( plies, -mySizeX * mySizeY, mySizeX * mySizeY, owner, bestMove );

  deleteColorOwnerStack( plies );

  return bestMove;
}

void FillGridModel::createColorOwnerStack( S32 depth )
{
  myColorStack = new S32**[depth];
  myOwnerStack = new S32**[depth];
  for( int i = 0; i < depth; ++i )
  {
    myColorStack[i] = createArray();
    myOwnerStack[i] = createArray();
  }
}

void FillGridModel::deleteColorOwnerStack( S32 depth )
{
  for( int i = 0; i < depth; ++i )
  {
    deleteArray( myColorStack[i] );
    deleteArray( myOwnerStack[i] );
  }
  delete[] myColorStack;
  delete[] myOwnerStack;
}

F32 FillGridModel::negamax( S32 depth, F32 alpha, F32 beta, S32 owner, 
                           S32 &bestMove )
{
  if( depth == 0 || (score(0) + score(1) == mySizeX * mySizeY) )
  {
    // Evaluation of the game at this point:
    F32 evaluation = score(0) - score(1) - depth * 0.001;
    F32 locScore = 0;
    for( int col = 0; col < mySizeX; ++col )
    {
      for( int row = 0; row < mySizeY; ++row )
      {
        if( myOwner[col][row] == -1 ) continue;
        locScore += col > mySizeX/2 ? mySizeX-col : col;
        locScore += row > mySizeY/2 ? mySizeY-row : row;
        if( myOwner[col][row] == 0 ) locScore *= 0.00001;
        else locScore *= -0.00001;
      }
    }
    evaluation += locScore;
    if( owner == 0 ) return evaluation;
    else return -evaluation;
  }

  // Make copies of the current arrays and territories so we can restore them.
  S32 **origColor = myColorStack[depth-1];
  S32 **origOwner = myOwnerStack[depth-1];
  assignArray( origColor, myColor );
  assignArray( origOwner, myOwner );
  Vector<Point2I> origTerritory[2] = { myTerritory[0], myTerritory[1] };

  F32 currBestValue = -mySizeX * mySizeY;
  S32 currBestMove;

  for( int testColor = 0; testColor <= 5; testColor++ )
  {
    if( lastSelectedColor( owner ) == testColor ) continue;

    // Apply move
    floodFill( owner, testColor );

    // Determine its value
    F32 value = -negamax( depth - 1, -beta, -alpha, 1 - owner, currBestMove );
    
    // Undo move
    assignArray( myColor, origColor );
    assignArray( myOwner, origOwner );
    myTerritory[0] = origTerritory[0];
    myTerritory[1] = origTerritory[1];

    // If the move beats our current best option, set it.
    if( value > currBestValue )
    {
      currBestValue = value;
      bestMove = testColor;
    }

    // Alpha-Beta Pruning (check Wikipedia)
    if( value > alpha )
      alpha = value;
    if( alpha >= beta )
      break;
  }

  return currBestValue;
}

S32 **FillGridModel::createArray()
{
  S32 **a;
  a = new S32*[mySizeX];
  for( S32 col = 0; col < mySizeX; ++col )
    a[col] = new S32[mySizeY];
  return a;
}

void FillGridModel::initArray( S32 **a, S32 value )
{
  for( S32 col = 0; col < mySizeX; ++col )
    for( S32 row = 0; row < mySizeY; ++row )
      a[col][row] = value;
}

void FillGridModel::assignArray( S32 **into, S32 **from )
{
  for( S32 col = 0; col < mySizeX; ++col )
    dMemcpy( into[col], from[col], mySizeY * sizeof(S32) );
}

void FillGridModel::deleteArray( S32 **a )
{
  for( S32 col = 0; col < mySizeX; ++col )
    delete[] a[col];
  delete[] a;
}

ConsoleMethod( FillGridModel, initialize, void, 3, 4, "( <x y> )" )
{
  S32 x, y;

  if( argc == 3 ) // Space separated vector
    dSscanf( argv[2], "%d %d", &x, &y );
  else if( argc == 4 ) // Two arguments passed in
  {
    x = dAtoi( argv[2] );
    y = dAtoi( argv[3] );
  }

  object->initialize( x, y );
}

ConsoleMethod( FillGridModel, color, S32, 3, 4, "( <x y> )" )
{
  S32 x, y;

  if( argc == 3 )
    dSscanf( argv[2], "%d %d", &x, &y );
  else if( argc == 4 )
  {
    x = dAtoi( argv[2] );
    y = dAtoi( argv[3] );
  }

  return object->color( x, y );
}

ConsoleMethod( FillGridModel, owner, S32, 3, 4, "( <x y> )" )
{
  S32 x, y;

  if( argc == 3 )
    dSscanf( argv[2], "%d %d", &x, &y );
  else if( argc == 4 )
  {
    x = dAtoi( argv[2] );
    y = dAtoi( argv[3] );
  }

  return object->owner( x, y );
}

ConsoleMethod( FillGridModel, lastSelectedColor, S32, 3, 3, "( <owner> )" )
{
  return object->lastSelectedColor( dAtoi( argv[2] ) );
}

ConsoleMethod( FillGridModel, score, S32, 3, 3, "( <owner> )" )
{
  return object->score( dAtoi( argv[2] ) );
}

ConsoleMethod( FillGridModel, floodFill, void, 4, 4, "( <owner>, <color> )" )
{
  object->floodFill( dAtoi( argv[2] ), dAtoi( argv[3] ) );
}

ConsoleMethod( FillGridModel, bestMove, S32, 4, 4, "( <owner>, <plies> )" )
{
  return object->bestMove( dAtoi( argv[2] ), dAtoi( argv[3] ) );
}