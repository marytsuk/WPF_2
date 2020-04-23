# WPF_2
Additional features in the binding mechanism data in Windows Presentation Foundation. Interaction between WPF and WindowsForms.
1. Checking the correctness of data in the binding. System.IDataErrorInfo Interface.
2. Routed events and commands.
3. The Chart control from WindowsForms.

Requirements for the program
Create a user interface for the app,
which allows you to view graphically the data specified on the
uniform grid and parameter-dependent.

Displays graphics in the Chart control.
In the Chart control data from ObservableModelData is output in two columns
output areas (Chart Area).
A single output area Chart Area displays graphs of function values in nodes
meshes for all ModelData elements that have the p parameter value less than or
equal to the value from the ModelData element that the user selected in the element
ListBox.
In this output area
1.  graphs are output without markers;
2.  the chart type is determined by the user's choice;
3.  the number of digits after the decimal point when digitizing is determined by the user's choice;
4.  the values of the p parameter for ModelData elements are output in Legend.

In the other output area, the Chart Area displays graphs of three functions. For each
a grid node from the ModelData element that the user selected in the ListBox element,
calculates the minimum, maximum, and average value of the function among all
modeldata elements of the collection. For those collection items in which the grid node
selected by the user in ListBox
