<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3083630029">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="676808703">
        <_items dataType="Array" type="Duality.Component[]" id="408852270" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="1148977665">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">3083630029</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-250</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-250</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="3620905836">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3083630029</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="2895703608">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="4147566188" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="2945495908">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="PerspectiveWorld" value="0" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                </item>
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="3292989974">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="None" value="0" />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="OrthoScreen" value="1" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </passes>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
            <priority dataType="Int">0</priority>
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="3737111400">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3083630029</gameobj>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2161028448" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="257854517">
            <item dataType="Type" id="3427459574" value="Duality.Components.Transform" />
            <item dataType="Type" id="3053280282" value="Duality.Components.Camera" />
            <item dataType="Type" id="3662226198" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="855591496">
            <item dataType="ObjectRef">1148977665</item>
            <item dataType="ObjectRef">3620905836</item>
            <item dataType="ObjectRef">3737111400</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1148977665</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4123293055">+jPDHA9nLEa/hsyLtTQNGw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3301912879">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3138277837">
        <_items dataType="Array" type="Duality.GameObject[]" id="1401643558" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="4230585930">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="167197406">
              <_items dataType="Array" type="Duality.Component[]" id="4225504016" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2295933566">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4230585930</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1367260515">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <angleVel dataType="Float">0</angleVel>
                    <angleVelAbs dataType="Float">0</angleVelAbs>
                    <deriveAngle dataType="Bool">true</deriveAngle>
                    <gameobj dataType="ObjectRef">3301912879</gameobj>
                    <ignoreParent dataType="Bool">false</ignoreParent>
                    <parentTransform />
                    <pos dataType="Struct" type="Duality.Vector3" />
                    <posAbs dataType="Struct" type="Duality.Vector3" />
                    <scale dataType="Float">1</scale>
                    <scaleAbs dataType="Float">1</scaleAbs>
                    <vel dataType="Struct" type="Duality.Vector3" />
                    <velAbs dataType="Struct" type="Duality.Vector3" />
                  </parentTransform>
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="1937470651">
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">4230585930</gameobj>
                  <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="3980648495" custom="true">
                    <body>
                      <version dataType="Int">3</version>
                      <data dataType="Array" type="System.Byte[]" id="88321774">H4sIAAAAAAAEAJXWUQ7DIAwD0JRyhN3/rJum/lh68lilCQFJMCbx8pqZ1+d3zfe7Y1gxG80ek/17lqFzcWnvWbxi2JollgSYJnlNzuhAgAk+/UhBnkCc3Bst5unJ0grLNGmEcI8xt0zuYknwyXzC3TIhpHTPWcbMgcHyHRoypkaSzHdg0iZ4uh9cmpBGx9IhBx6b9KxicsmhveYoSt6BFLTKIUAuTnEfLVLW2kEjuBwOHFhx7d0powd6Tc3ipanQpIAyOjJpCd20nCz9fVDT6/zSr9U0k50Xa7XJSm0SxBIdmbTk49/EuSpyjy1HEytiYXUwCw40kvlCrnl3uh+0HGyGWu4mS60hGMFlFHY6mUtkojWJucg+hACpbnxi1l9ynSDedbpTOPgKAAA=</data>
                    </body>
                  </tileData>
                  <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                    <contentPath dataType="String">Data\Dev\Images\PiecesTiles.Tileset.res</contentPath>
                  </tileset>
                </item>
                <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="2928694082">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <externalTilemap />
                  <gameobj dataType="ObjectRef">4230585930</gameobj>
                  <offset dataType="Float">-0</offset>
                  <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="Flat" value="0" />
                  <tileDepthOffset dataType="Int">0</tileDepthOffset>
                  <tileDepthScale dataType="Float">0</tileDepthScale>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3870598922" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="746300668">
                  <item dataType="ObjectRef">3427459574</item>
                  <item dataType="Type" id="3965049156" value="Duality.Plugins.Tilemaps.Tilemap" />
                  <item dataType="Type" id="1921663638" value="Duality.Plugins.Tilemaps.TilemapRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="1486614934">
                  <item dataType="ObjectRef">2295933566</item>
                  <item dataType="ObjectRef">1937470651</item>
                  <item dataType="ObjectRef">2928694082</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2295933566</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="37714344">49G1t5wMV0+fAkex1Pli/g==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TilesLayer</name>
            <parent dataType="ObjectRef">3301912879</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1327884472">
        <_items dataType="Array" type="Duality.Component[]" id="926452647" length="4">
          <item dataType="ObjectRef">1367260515</item>
          <item dataType="Struct" type="FloodFill.FillBoardComponent" id="410982742">
            <active dataType="Bool">true</active>
            <currentPlayer dataType="Int">0</currentPlayer>
            <debugTickCount dataType="Int">50</debugTickCount>
            <debugTickDelay dataType="Int">50</debugTickDelay>
            <fillGrid />
            <gameobj dataType="ObjectRef">3301912879</gameobj>
            <playing dataType="Bool">true</playing>
            <plies dataType="Int">4</plies>
            <rand dataType="Struct" type="System.Random" id="2492263688">
              <inext dataType="Int">0</inext>
              <inextp dataType="Int">21</inextp>
              <SeedArray dataType="Array" type="System.Int32[]" id="1934177132">0, 2033522323, 1245746842, 1382373958, 117037906, 1584432807, 1197521606, 304543702, 1484103670, 1037942712, 1728037146, 2010360994, 1427645445, 618774034, 2075178207, 1715443397, 568570654, 933923115, 1207204382, 12175955, 1768613907, 2021903324, 997478271, 1632601717, 2047854979, 955318404, 1901332932, 1013035623, 802676792, 525537661, 226572908, 1877822318, 3099085, 1415387464, 825110547, 877603394, 1980791850, 1870430982, 2075665589, 1140114361, 2063427616, 39396173, 1131450489, 1998742797, 796045686, 1423948353, 215520912, 582561606, 690261256, 28241824, 1092923067, 516057406, 1087962561, 1089933811, 560657657, 169728114</SeedArray>
            </rand>
            <tilesGameBoard dataType="ObjectRef">1937470651</tilesGameBoard>
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="648251303" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1982342804">
            <item dataType="ObjectRef">3427459574</item>
            <item dataType="Type" id="529083236" value="FloodFill.FillBoardComponent" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2872621110">
            <item dataType="ObjectRef">1367260515</item>
            <item dataType="ObjectRef">410982742</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1367260515</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2999808816">z3+gpm9YW061Ozq74mIvuw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Map</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">4230585930</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
