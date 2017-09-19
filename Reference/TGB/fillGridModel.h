#ifndef _FILLGRIDMODEL_H_
#define _FILLGRIDMODEL_H_

#ifndef _SCRIPTOBJECTS_H_
#include "console/scriptObjects.h"
#endif

#ifndef _TVECTOR_H_
#include "core/tVector.h"
#endif

class FillGridModel : public ScriptObject
{
  typedef ScriptObject Parent;

public:
  FillGridModel();
  ~FillGridModel();
  void cleanup();
  bool onAdd();
  void onRemove();

  void initialize( S32 x, S32 y );
  S32 sizeX();
  S32 sizeY();
  S32 color( S32 x, S32 y );
  S32 owner( S32 x, S32 y );
  S32 lastSelectedColor( S32 owner );
  S32 score( S32 owner );
  void floodFill( S32 owner, S32 color );
  S32 bestMove( S32 owner, S32 plies );

  DECLARE_CONOBJECT(FillGridModel);

protected:
  S32 **createArray();
  void initArray( S32 **a, S32 value );
  void assignArray( S32 **into, S32 **from );
  void deleteArray( S32 **a );

private:
  void createColorOwnerStack( S32 depth );
  void deleteColorOwnerStack( S32 depth );
  // http://en.wikipedia.org/wiki/Negamax
  F32 negamax( S32 depth, F32 alpha, F32 beta, S32 maximizer, 
               S32 &bestMove );

protected:
  S32 mySizeX;
  S32 mySizeY;
  S32 **myColor;
  S32 **myOwner;
  Vector<Point2I> myTerritory[2];

private:
  S32 ***myColorStack;
  S32 ***myOwnerStack;
};

inline S32 FillGridModel::score( S32 owner )
{
  return myTerritory[owner].size();
}

#endif