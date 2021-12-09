using System;

public class Program
{
    static void Main()
    {

        int[,] cellsData = new int[,]{
                {1, 1, 1, 0, 1, 0, 1},	// expected {1, 0, 1, 1, 1, 1, 1} // because of updates
				{0, 1, 1, 0, 0, 0, 1},
                {0, 1, 0, 0, 1, 1, 1},
                {0, 1, 0, 0, 0, 0, 1}, 	// expected {1, 1, 1, 0, 0, 1, 1}
				{0, 1, 0, 0, 0, 0, 1},
                {0, 0, 1, 0, 1, 0, 1},
                {0, 0, 0, 0, 1, 0, 1}	// expected {0, 0, 0, 1, 0, 1, 0}			
		};

        Console.WriteLine("Before");
        printData(cellsData);
        processData(cellsData);
        Console.WriteLine("\n\nAfter");
        printData(cellsData);
        Console.WriteLine();
    }

    private static int[] GetRow(int[,] celldata, int row)
    {
        int[] cellrow = new int[7];
        for (int i = 0; i < 7; i++)
        {
            cellrow[i] = celldata[row, i];
            Console.WriteLine(" here " + cellrow[i]);
        }
        return cellrow;
    }



    private static void processData(int[,] cellsData)
    {

        for (int i = 0; i < cellsData.Length; i++)
        {
            for (int j = 0; j < GetRow(cellsData, i).Length; j++)
            {

                if (cellsData[i, j] == 1)
                {
                    updateLivingCell(i, j, cellsData);
                }
                else
                {
                    updateDeadCell(i, j, cellsData);
                }
            }
        }
    }




    private static void updateLivingCell(int x, int y, int[,] cellsData)
    {

        if (willSurvive(x, y, cellsData))
        {
            cellsData[x, y] = 1;
        }
        else
        {
            cellsData[x, y] = 0;
        }

    }

    private static void updateDeadCell(int x, int y, int[,] cellsData)
    {

        if (willResurrect(x, y, cellsData))
        {
            cellsData[x, y] = 1;
        }
        else
        {
            cellsData[x, y] = 0;
        }
    }

    private static bool willSurvive(int x, int y, int[,] cellsData)
    {

        int livingCells = sumNeighborsPreviousLine(x, y, cellsData);
        livingCells += sumNeighborsSameLine(x, y, cellsData);
        livingCells += sumNeighborsNextLine(x, y, cellsData);

        return (livingCells == 2 || livingCells == 3);

    }


    private static int sumNeighborsNextLine(int x, int y, int[,] cellsData)
    {
        int livingCellsSum = 0;

        if (x < cellsData.Length - 1)
        {
            int previous = (isValidIndex(y - 1, GetRow(cellsData, x + 1))) ? cellsData[x + 1, y - 1] : 0;
            int same = (isValidIndex(y, GetRow(cellsData, x + 1))) ? cellsData[x + 1, y] : 0;
            int next = (isValidIndex(y + 1, GetRow(cellsData, x + 1))) ? cellsData[x + 1, y + 1] : 0;
            livingCellsSum = previous + same + next;
        }
        return livingCellsSum;
    }


    private static int sumNeighborsSameLine(int x, int y, int[,] cellsData)
    {

        int previous = (isValidIndex(y - 1, GetRow(cellsData, x))) ? cellsData[x, y - 1] : 0;
        int same = (isValidIndex(y, GetRow(cellsData, x))) ? cellsData[x, y] : 0;
        int next = (isValidIndex(y + 1, GetRow(cellsData, x))) ? cellsData[x, y + 1] : 0;
        return previous + same + next;
    }


    private static int sumNeighborsPreviousLine(int x, int y, int[,] cellsData)
    {
        int livingCellsSum = 0;

        if (x > 0)
        {
            int previous = (isValidIndex(y - 1, GetRow(cellsData, x - 1))) ? cellsData[x - 1, y - 1] : 0;
            int same = (isValidIndex(y, GetRow(cellsData, x - 1))) ? cellsData[x - 1, y] : 0;
            int next = (isValidIndex(y + 1, GetRow(cellsData, x - 1))) ? cellsData[x - 1, y + 1] : 0;
            livingCellsSum = previous + same + next;
        }
        return livingCellsSum;
    }


    private static bool willResurrect(int x, int y, int[,] cellsData)
    {

        int livingCells = sumNeighborsPreviousLine(x, y, cellsData);
        livingCells += sumNeighborsSameLine(x, y, cellsData);
        livingCells += sumNeighborsNextLine(x, y, cellsData);
        return livingCells == 3;
    }



    private static bool isValidIndex(int index, int[] line)
    {
        return index > 0 && index < line.Length;
    }




    private static void printData(int[,] matrix)
    {

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }

    }
}

