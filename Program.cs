// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// Console.WriteLine(" superjohn")
// Console.WriteLine(" Ellen")
// Console.WriteLine("Jordan")

using System;

namespace ChessGame // Note: actual namespace depends on the project name.
{
    class Programs
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Board {
         /* 
           # 1 -------
           # 2 | | | |
           # 3 -------
           # 4 | | | |
           # 5 -------
           # 6 | | | |
           # 7 -------
        */

        /*
         * column > 9
         * row > 7
         * - >> 1,3,5,7
         * | >> 
         * " " >> 2,4,6
         */

        public void ticboard1() 
        {
            for (int i = 1; i<=7;i++)
            {
                for (int j = 1; j<=7;j++)
                {
                    /* - */
                    if ((i%2)==1 )
                    {
                        Console.Write("-");
                    }
                    else if ((j % 2) == 0) /*j == 2 || j == 4 || j == 6 */
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                       
                        Console.Write("|");

                    }

                }
                WriteLine();
            }
        }
        /* 
           # 1  -----------------
           # 2  | | | | | | | | |
           # 3  -----------------
           # 4  | | | | | | | | |
           # 5  -----------------
           # 6  | | | | | | | | |
           # 7  -----------------
           # 8  | | | | | | | | |
           # 9  -----------------
           # 10 | | | | | | | | |
           # 11 -----------------
           # 12 | | | | | | | | |
           # 13 -----------------
           # 14 | | | | | | | | |
           # 15 -----------------
           # 16 | | | | | | | | |
           # 17 -----------------
        
        */

        /*
         * column > 17
         * row > 
         * - >> 1,3,5,7,9,11,13,15,17
         * | >> 2,4,6,8,10,12,14,16
         */
        public void reversiboard1()
        {
            for (int i = 1; i <= 17; i++)
            {
                for (int j = 1; j <= 17; j++)
                {
                    /* - */
                    if ((i % 2) == 1)
                    {
                        Console.Write("-");
                    }
                    else if ((j%2) == 0) /*2,4,6,8,10,12,14,16*/
                    {
                        Console.Write(" ");
                    }
                    else
                    {

                        Console.Write("|");

                    }

                }
                WriteLine();
            }
        }
    }

    class History : Board{
        //Save all moves
        protected Stack<string> forwardTrack = new Stack<string>();
        //Save the moves which are undo by user
        protected Queue<string> backTrack = new Queue<string>();
    }

    class Chess : Board {

    }


    class Player{

    }

    class Human : Player{

    }

    class Computer : Player{
        
    }
}