using System;

public class Program
{

    // public static int[,] GetMatrix()
    // {
    //     int x, y;
    //     System.Console.Write("Enter the no.of rows: ");
    //     x = Convert.ToInt32(Console.ReadLine());
    //     System.Console.Write("Enter the no.of columns: ");
    //     y = Convert.ToInt32(Console.ReadLine());
    //     int[,] matrix = new int[x, y];

    //     for (int i = 0; i < x; i++)
    //     {
    //         for (int j = 0; j < y; j++)
    //         {
    //             matrix[i, j] = Convert.ToInt32(Console.ReadLine());
    //         }
    //     }

    //     return matrix;

    // }

    static void Main()
    {

        // int[,] matrix = GetMatrix();

        int[,] matrix = new int[,]{
               { 0, 1, 0, 0, 0, 0, 1 },
                { 0, 1, 0, 0, 1, 1, 1 },
                { 1, 1, 1, 0, 1, 0, 1 },
                { 0, 1, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 1, 0, 1 },
                { 0, 1, 0, 0, 0, 0, 1 },
                { 0, 0, 1, 0, 1, 0, 1 }
        };

        Console.WriteLine("\nMatrix Before rules");
        printMatrix(matrix);
        updateMatrix(matrix);
        System.Console.WriteLine();
        System.Console.WriteLine();
        Console.WriteLine("Matrix After rules");
        printMatrix(matrix);
        Console.WriteLine();
    }

    private static int[] GetRow(int[,] matrix, int row)
    {
        int[] cellrow = new int[matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            cellrow[i] = matrix[row, i];
        }
        return cellrow;
    }



    private static void updateMatrix(int[,] matrix)
    {

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {

                if (matrix[i, j] == 1)
                {
                    updateLivingCell(i, j, matrix);
                }
                else
                {
                    updateDeadCell(i, j, matrix);
                }
            }
        }
    }




    private static void updateLivingCell(int x, int y, int[,] matrix)
    {

        if (willSurvive(x, y, matrix))
        {
            matrix[x, y] = 1;
        }
        else
        {
            matrix[x, y] = 0;
        }

    }

    private static void updateDeadCell(int x, int y, int[,] matrix)
    {

        if (willRevive(x, y, matrix))
        {
            matrix[x, y] = 1;
        }
        else
        {
            matrix[x, y] = 0;
        }
    }

    private static bool willSurvive(int x, int y, int[,] matrix)
    {

        int livingCells = sumNeighborsPreviousLine(x, y, matrix);
        livingCells += sumNeighborsSameLine(x, y, matrix);
        livingCells += sumNeighborsNextLine(x, y, matrix);

        return (livingCells == 2 || livingCells == 3);

    }


    private static int sumNeighborsNextLine(int x, int y, int[,] matrix)
    {
        int livingCellsSum = 0;

        if (x < matrix.GetLength(0) - 1)
        {
            int previous = (isValidIndex(y - 1, GetRow(matrix, x + 1))) ? matrix[x + 1, y - 1] : 0;
            int same = (isValidIndex(y, GetRow(matrix, x + 1))) ? matrix[x + 1, y] : 0;
            int next = (isValidIndex(y + 1, GetRow(matrix, x + 1))) ? matrix[x + 1, y + 1] : 0;
            livingCellsSum = previous + same + next;
        }
        return livingCellsSum;
    }


    private static int sumNeighborsSameLine(int x, int y, int[,] matrix)
    {

        int previous = (isValidIndex(y - 1, GetRow(matrix, x))) ? matrix[x, y - 1] : 0;
        // int same = (isValidIndex(y, GetRow(matrix, x))) ? matrix[x, y] : 0;
        int next = (isValidIndex(y + 1, GetRow(matrix, x))) ? matrix[x, y + 1] : 0;
        return previous + next;
    }


    private static int sumNeighborsPreviousLine(int x, int y, int[,] matrix)
    {
        int livingCellsSum = 0;

        if (x > 0)
        {
            int previous = (isValidIndex(y - 1, GetRow(matrix, x - 1))) ? matrix[x - 1, y - 1] : 0;
            int same = (isValidIndex(y, GetRow(matrix, x - 1))) ? matrix[x - 1, y] : 0;
            int next = (isValidIndex(y + 1, GetRow(matrix, x - 1))) ? matrix[x - 1, y + 1] : 0;
            livingCellsSum = previous + same + next;
        }
        return livingCellsSum;
    }


    private static bool willRevive(int x, int y, int[,] matrix)
    {

        int livingCells = sumNeighborsPreviousLine(x, y, matrix);
        livingCells += sumNeighborsSameLine(x, y, matrix);
        livingCells += sumNeighborsNextLine(x, y, matrix);
        return livingCells == 3;
    }



    private static bool isValidIndex(int index, int[] row)
    {
        return index > 0 && index < row.Length;
    }




    private static void printMatrix(int[,] matrix)
    {

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }

    }
}
