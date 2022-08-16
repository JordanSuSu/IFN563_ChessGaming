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