// Map an index to an image map string
$IM[0] = "aquaSquareImageMap";
$IM[1] = "blueSquareImageMap";
$IM[2] = "greenSquareImageMap";
$IM[3] = "purpleSquareImageMap";
$IM[4] = "redSquareImageMap";         
$IM[5] = "yellowSquareImageMap";  

function fillGrid::onLevelLoaded(%this, %scenegraph)
{
   startTimer();
   
   // Clone the tile map for a border map
   %this.borders = %scenegraph.getGlobalTileMap().createTileLayer(
       %this.getTileCount(), %this.getTileSize() );
   %this.borders.setArea( %this.getArea() );
   %this.borders.setBlendColor( 0.63, 0.19, 0.01 );
   
   %maxX = %this.getTileCountX();
   %maxY = %this.getTileCountY();
   
   %this.model = new FillGridModel();
   %this.model.initialize( %maxX, %maxY );
   %this.displayModel();
   
   //set clicks
   %this.playerClicks[0] = 0;
   %this.playerClicks[1] = 0;
}

function fillGrid::onLevelEnded( %this, %scenegraph )
{
  %this.model.delete();
}

//called when user clicks a color
function fillGrid::setCurrentColor( %this, %player, %newColor )
{  
  %this.playerClicks[%player]++;
  
  %this.model.floodFill( %player, %newColor );
  %this.displayModel();
}

function fillGrid::displayModel( %this )
{
  %maxX = %this.getTileCountX();
  %maxY = %this.getTileCountY();
  
  for( %x = 0; %x < %maxX; %x++ )
  {
    for( %y = 0; %y < %maxY; %y++ )
    {
      %color = %this.model.color( %x, %y );
      %this.setStaticTile( %x SPC %y, $IM[%color] );
    }
  }
  
  %this.updateBorders();
  %this.setScores();
}

function fillGrid::updateBorders( %this )
{
  %maxX = %this.getTileCountX();
  %maxY = %this.getTileCountY();

  for( %x = 0; %x < %maxX; %x++ )
  {
    for( %y = 0; %y < %maxY; %y++ )
    {
      %owner = %this.model.owner( %x, %y );

      // If the tile isn't owned, we don't care about its borders.      
      if( %owner == -1 )
      {
        %this.borders.clearTile( %x, %y );
        continue;
      }
      
      %bits = 0;
      
      %bits |= (%owner != %this.model.owner(%x, %y - 1)) << 0;
      %bits |= (%owner != %this.model.owner(%x + 1, %y)) << 1;
      %bits |= (%owner != %this.model.owner(%x, %y + 1)) << 2;
      %bits |= (%owner != %this.model.owner(%x - 1, %y)) << 3;
      
      %this.borders.setStaticTile( %x, %y, SquareBordersImageMap, %bits );
    }
  }
}

function fillGrid::setScores(%this)
{
  player1ClicksLabel.text = %this.playerClicks[0];
  player2ClicksLabel.text = %this.playerClicks[1];
  player1OwnedLabel.text = %this.model.score(0);
  player2OwnedLabel.text = %this.model.score(1);
}