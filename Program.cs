// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// Console.WriteLine(" superjohn")
// Console.WriteLine(" Ellen")
// Console.WriteLine("Jordan")

using System;
using static System.Console;
namespace ChessGame // Note: actual namespace depends on the project name.
{
    class Programs
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            Board board = new Board();
            // let user to select which type of game he/she want to play
            WriteLine("Please enter which game you want to play:\n1.Wild tic-tac-toe\n2.Reversi aka Othello");
            game = game.setGameType(ReadLine(), game);

            // let user to select which game mode he/she want to play
            WriteLine("Please enter which game mode u want to play:\n1.play with human\n2.play with computer");
            game = game.setGameMode(ReadLine(), game);
            WriteLine($"You choose {game.CurGameType} and the game mode is {game.CurGameMode}");
            board.drawBoard(game);
        }
    }

    class Game {
        private string str_CurGameMode = (GameMode.hvh).ToString();
        private string str_CurGameType = (GameType.tictactoe).ToString();
        // public Board board = new Board();
        protected enum GameMode{
            //human vs human
            hvh = 1,
            //computer vs human
            cvh = 2
        };
        
        protected enum GameType{
            //Wild tic-tac-toe
            tictactoe = 1,
            //Reversi aka Othello
            reversi = 2
        }
        public string CurGameMode{
            get{ return str_CurGameMode; }
            set
            {
                str_CurGameMode = ((GameMode)Convert.ToInt32(value)).ToString();
            }
        }

        public string CurGameType{
            get{ return str_CurGameType; }
            set
            {
                str_CurGameType = ((GameType)Convert.ToInt32(value)).ToString();
            }
        }

        public Game setGameType(string value, Game game){
            int num_type;
            bool res = int.TryParse(value, out num_type);
            // WriteLine(res + "--" + (num_type == (int)GameType.reversi || num_type == (int)GameType.tictactoe));
            while (!(res && (num_type == (int)GameType.reversi || num_type == (int)GameType.tictactoe))){
                WriteLine("Please re-enter a valid number of game you want to play:\n1.Wild tic-tac-toe\n2.Reversi aka Othello");
                res = int.TryParse(ReadLine(), out num_type);
            }

            game.str_CurGameType = ((GameType)num_type).ToString();
            return game;
        }
        public Game setGameMode(string value, Game game){
            //check whether the input value is a valid number of game mode
            int num_mode;
            bool res = int.TryParse(value, out num_mode);
            while ( !(res && (num_mode == (int)GameMode.cvh || num_mode == (int)GameMode.hvh)) ){
                WriteLine("Please re-enter a valid number of game mode u want to play:\n1.play with human\n2.play with computer");
                res = int.TryParse(ReadLine(), out num_mode);
            }
            // WriteLine(num_mode);
            game.str_CurGameMode = ((GameMode)num_mode).ToString();
            return game;
        }
    
    }

    class Board : Game {
        
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
        // display the current state of board
        public void drawBoard(Game game){
            int max = game.CurGameType == (GameType.tictactoe).ToString() ? 7 : 17 ;
            for (int i = 1; i <= max; i++)
            {
                for (int j = 1; j <= max; j++)
                {
                    /* - */
                    if ((i%2)==1 )
                    {
                        Write("-");
                    }
                    else if ((j % 2) == 0) /*j == 2 || j == 4 || j == 6 */
                    {
                        Write(" ");
                    }
                    else
                    {
                       
                        Write("|");

                    }
                }
                WriteLine();
            }
        }
        public void ticboard1() 
        {
            for (int i = 1; i<=7;i++)
            {
                for (int j = 1; j<=7;j++)
                {
                    /* - */
                    if ((i%2)==1 )
                    {
                        Write("-");
                    }
                    else if ((j % 2) == 0) /*j == 2 || j == 4 || j == 6 */
                    {
                        Write(" ");
                    }
                    else
                    {
                       
                        Write("|");

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


    class Player : Game{

    }

    class Human : Player{

    }

    class Computer : Player{
        
    }

    class Hint : Game {

    }
}