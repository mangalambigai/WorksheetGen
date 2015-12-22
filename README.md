The project generates arithmetic worksheets for elementary school children.
Some generated worksheets are in https://sites.google.com/site/k2mathworksheet/

## Setup:
1. Open the solution in Microsoft Visual Studio
2. In Settings.settings, edit the xmlfile path. This xml has the parameters for the worksheet:
```
<sheets>
 <group name="Mystery Animal Addition- Zoo Animals" type="arithmetic" operation="addition" columns ="4" rows="3" digits="1">
    <sheet name = "Zoo Animal 1" answer ="MONKEY"/>
    <sheet name = "Zoo Animal 2" answer ="ZEBRA"/>
    <sheet name = "Zoo Animal 3" answer ="GIRAFFE"/>
    <sheet name = "Zoo Animal 4" answer ="LION"/>
    <sheet name = "Zoo Animal 5" answer ="TIGER"/>
    <sheet name = "Zoo Animal 6" answer ="HIPPOPOTAMUS"/>
    <sheet name = "Zoo Animal 7" answer ="RHINOCEROS"/>
    <sheet name = "Zoo Animal 8" answer ="ELEPHANT"/>
    <sheet name = "Zoo Animal 9" answer ="CHEETAH"/>
  </group>
</sheets>
```
3. In Settings.settings, edit the pdffolder path. This is the folder that will have the output pdf files.
4. Build the project, and run it.

This project uses iTextSharp, which is required for building this project. Nuget package manager should download iTextSharp for you. If not, please install it and then build and run.