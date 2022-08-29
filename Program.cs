// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// Console.WriteLine(" superjohn")
// Console.WriteLine(" Ellen")
// Console.WriteLine("Jordan")

using System;
using System.Diagnostics.Metrics;
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

    class Move
    {
        // check valid move


        public int checkresult(int t_row, int t_col, int chessstatus)
        {
            //diagoanl 2,2 / 4,4 / 6,6
            if (t_row == 2 && t_col == 2)
            {
                if (chessstatus == 1)
                { diagonalcounter1++; Console.WriteLine("diagonalcounter1 " + diagonalcounter1); }
                else
                { diagonalcounter2++; Console.WriteLine("diagonalcounter2 " + diagonalcounter2); }
            }
            else if (t_row == 4 && t_col == 4)
            {
                if (chessstatus == 1)
                { diagonalcounter1++; Console.WriteLine("diagonalcounter1 " + diagonalcounter1); }
                else
                { diagonalcounter2++; Console.WriteLine("diagonalcounter2 " + diagonalcounter2); }
            }
            else if (t_row == 6 && t_col == 6)
            {
                if (chessstatus == 1)
                { diagonalcounter1++; Console.WriteLine("diagonalcounter1 " + diagonalcounter1); }
                else
                { diagonalcounter2++; Console.WriteLine("diagonalcounter2 " + diagonalcounter2); }
            }
            //diagoanl 2,6 / 4,4 / 6,2
            if (t_row == 2 && t_col == 6)
            {
                if (chessstatus == 1)
                { diagonalcounter3++; Console.WriteLine("diagonalcounter3 " + diagonalcounter3); }
                else
                { diagonalcounter4++; Console.WriteLine("diagonalcounter4 " + diagonalcounter4); }
            }
            else if (t_row == 6 && t_col == 2)
            {
                if (chessstatus == 1)
                { diagonalcounter3++; Console.WriteLine("diagonalcounter3 " + diagonalcounter3); }
                else
                { diagonalcounter4++; Console.WriteLine("diagonalcounter4 " + diagonalcounter4); }
            }
            else if (t_row == 4 && t_col == 4)
            {
                if (chessstatus == 1)
                { diagonalcounter3++; Console.WriteLine("diagonalcounter3 " + diagonalcounter3); }
                else
                { diagonalcounter4++; Console.WriteLine("diagonalcounter4 " + diagonalcounter4); }
            }

            //row
            if (t_row == 2)
            {
                if (chessstatus == 1)
                { row2counter1++; Console.WriteLine("row2counter1 " + row2counter1); }
                else
                { row2counter2++; Console.WriteLine("row2counter2 " + row2counter2); }
            }
            else if (t_row == 4)
            {
                if (chessstatus == 1)
                { row4counter1++; Console.WriteLine("row4counter1 " + row4counter1); }
                else
                { row4counter2++; Console.WriteLine("row4counter2 " + row4counter2); }
            }
            else if (t_row == 6)
            {
                if (chessstatus == 1)
                { row6counter1++; Console.WriteLine("row6counter1 " + row6counter1); }
                else
                { row6counter2++; Console.WriteLine("row6counter2 " + row6counter2); }
            }

            // col
            if (t_col == 2)
            {
                if (chessstatus == 1)
                { col2counter1++; Console.WriteLine("col2counter1 " + col2counter1); }
                else
                { col2counter2++; Console.WriteLine("col2counter2 " + col2counter2); }
            }
            else if (t_col == 4)
            {
                if (chessstatus == 1)
                { col4counter1++; Console.WriteLine("col4counter1 " + col4counter1); }
                else
                { col4counter2++; Console.WriteLine("col4counter2 " + col4counter2); }
            }
            else if (t_col == 6)
            {
                if (chessstatus == 1)
                { col6counter1++; Console.WriteLine("col6counter1 " + col6counter1); }
                else
                { col6counter2++; Console.WriteLine("col6counter2 " + col6counter2); }
            }

            if (diagonalcounter1 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (diagonalcounter2 == 3)
            { Console.WriteLine("Player 2 Win "); return 2; }
            else if (diagonalcounter3 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (diagonalcounter4 == 3)
                return 4;
            else if (row2counter1 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (row4counter1 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (row6counter1 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (row2counter2 == 3)
            { Console.WriteLine("Player 2 Win "); return 2; }
            else if (row4counter2 == 3)
            { Console.WriteLine("Player 2 Win "); return 2; }
            else if (row6counter2 == 3)
            { Console.WriteLine("Player 2 Win "); return 2; }
            else if (col2counter1 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (col4counter1 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (col6counter1 == 3)
            { Console.WriteLine("Player 1 Win "); return 1; }
            else if (col2counter2 == 3)
            { Console.WriteLine("Player 2 Win "); return 2; }
            else if (col4counter2 == 3)
            { Console.WriteLine("Player 2 Win "); return 2; }
            else if (col6counter2 == 3)
            { Console.WriteLine("Player 2 Win "); return 2; }
            else
            { Console.WriteLine("drawn game"); }

            return 0;
        }



        //Array.Sort(Sportathon.row);
        //Array.Sort(Sportathon.column);
        // row --, column ||



        public static int[] tic_row = { 2, 4, 6 };
        public static int[] tic_col = { 2, 4, 6 };

        public static int[] reversi_row = { 2, 4, 6, 8, 10, 12, 14, 16 };
        public static int[] reversi_col = { 2, 4, 6, 8, 10, 12, 14, 16 };

        public static int tic_rowcoordiantes;
        public static int tic_colcoordiantes;
        public static int reversi_rowcoordiantes;
        public static int reversi_colcoordiantes;

        public static int diagonalcounter1 = 0;
        public static int diagonalcounter2 = 0;
        public static int diagonalcounter3 = 0;
        public static int diagonalcounter4 = 0;


        public static int row2counter1 = 0;
        public static int row4counter1 = 0;
        public static int row6counter1 = 0;
        public static int row2counter2 = 0;
        public static int row4counter2 = 0;
        public static int row6counter2 = 0;

        public static int col2counter1 = 0;
        public static int col4counter1 = 0;
        public static int col6counter1 = 0;
        public static int col2counter2 = 0;
        public static int col4counter2 = 0;
        public static int col6counter2 = 0;

    }

    class Board : Game
    {
        
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

        public int transferrowcoltobox(int row, int col, int status)
        {
            if (row == 2 && col == 2)
            {
                if (status == 2)
                { coordiante[0] = "O"; }
                else if (status == 1)
                { coordiante[0] = "X"; }
                else
                { coordiante[0] = " "; }
            }
            else if (row == 2 && col == 4)
            {
                if (status == 2)
                { coordiante[1] = "O"; }
                else if (status == 1)
                { coordiante[1] = "X"; }
                else
                { coordiante[1] = " "; }
            }
            else if (row == 2 && col == 6)
            {
                if (status == 2)
                { coordiante[2] = "O"; }
                else if (status == 1)
                { coordiante[2] = "X"; }
                else
                { coordiante[2] = " "; }
            }
            else if (row == 4 && col == 2)
            {
                if (status == 2)
                { coordiante[3] = "O"; }
                else if (status == 1)
                { coordiante[3] = "X"; }
                else
                { coordiante[3] = " "; }
            }
            else if (row == 4 && col == 4)
            {
                if (status == 2)
                { coordiante[4] = "O"; }
                else if (status == 1)
                { coordiante[4] = "X"; }
                else
                { coordiante[4] = " "; }
            }
            else if (row == 4 && col == 6)
            {
                if (status == 2)
                { coordiante[5] = "O"; }
                else if (status == 1)
                { coordiante[5] = "X"; }
                else
                { coordiante[5] = " "; }
            }
            else if (row == 6 && col == 2)
            {
                if (status == 2)
                { coordiante[6] = "O"; }
                else if ((status == 1))
                { coordiante[6] = "X"; }
                else
                { coordiante[6] = " "; }
            }
            else if (row == 6 && col == 4)
            {
                if (status == 2)
                { coordiante[7] = "O"; }
                else if ((status == 1))
                { coordiante[7] = "X"; }
                else
                { coordiante[7] = " "; }
            }
            else if (row == 6 && col == 6)
            {
                if (status == 2)
                { coordiante[8] = "O"; }
                else if ((status == 1))
                { coordiante[8] = "X"; }
                else
                { coordiante[8] = " "; }
            }


            return 0;
        }
        public void ticboard1()
        {

            Console.WriteLine("-----------");
            Console.WriteLine("| {0} | {1} | {2} |", coordiante[0], coordiante[1], coordiante[2]);
            Console.WriteLine("-----------");
            Console.WriteLine("| {0} | {1} | {2} |", coordiante[3], coordiante[4], coordiante[5]);
            Console.WriteLine("-----------");
            Console.WriteLine("| {0} | {1} | {2} |", coordiante[6], coordiante[7], coordiante[8]);
            Console.WriteLine("-----------");


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

        public int counter = 1;
        public int k = 0;
        public static string[] coordiante = new string[9];
        private List<int> rowcolstatuslist = new List<int>();
        public List<int> Rowcolstatuslist
        {
            get { return rowcolstatuslist; }
            set { rowcolstatuslist.Add(Convert.ToInt32(value)); }
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
        public static int[] tic_row = { 2, 4, 6 };
        public static int[] tic_col = { 2, 4, 6 };

        public static int[] reversi_row = { 2, 4, 6, 8, 10, 12, 14, 16 };
        public static int[] reversi_col = { 2, 4, 6, 8, 10, 12, 14, 16 };

        public static int tic_rowcoordiantes;
        public static int tic_colcoordiantes;
        public static int reversi_rowcoordiantes;
        public static int reversi_colcoordiantes;
    }

    class Human : Player{
        public int tic_rowinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter row coordiantes: ");
            string rowinput = Console.ReadLine();

            bool rowresult = Int32.TryParse(rowinput, out tic_rowcoordiantes);
            int i = Array.BinarySearch(tic_row, tic_rowcoordiantes);


            while (rowresult == false || i < 0)
            {
                Console.Write("please enter a valid row coordiantes: ");
                rowresult = Int32.TryParse(ReadLine(), out tic_rowcoordiantes);
                i = Array.BinarySearch(tic_row, tic_rowcoordiantes);
            }

            return tic_rowcoordiantes;
        }
        public int tic_colinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter col coordiantes: ");
            string colinput = Console.ReadLine();

            bool colresult = Int32.TryParse(colinput, out tic_colcoordiantes);
            int j = Array.BinarySearch(tic_col, tic_colcoordiantes);

            while (colresult == false || j < 0)
            {
                Console.Write("please enter a valid column coordiantes: ");
                colresult = Int32.TryParse(ReadLine(), out tic_colcoordiantes);
                j = Array.BinarySearch(tic_col, tic_colcoordiantes);
            }

            return tic_colcoordiantes;
        }

        public int reversi_rowinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter row coordiantes: ");
            string rowinput = Console.ReadLine();
            bool rowresult = Int32.TryParse(rowinput, out reversi_rowcoordiantes);
            int i = Array.BinarySearch(reversi_row, reversi_rowcoordiantes);


            while (rowresult == false || i < 0)
            {
                Console.Write("please enter a valid row coordiantes: ");
                rowresult = Int32.TryParse(ReadLine(), out reversi_rowcoordiantes);
                i = Array.BinarySearch(reversi_row, reversi_rowcoordiantes);
            }

            return reversi_rowcoordiantes;
        }
        public int reversi_colinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter col coordiantes: ");
            string colinput = Console.ReadLine();
            bool colresult = Int32.TryParse(colinput, out reversi_colcoordiantes);
            int j = Array.BinarySearch(reversi_col, reversi_colcoordiantes);

            while (colresult == false || j < 0)
            {

                Console.Write("please enter a valid column coordiantes: ");
                colresult = Int32.TryParse(ReadLine(), out reversi_colcoordiantes);
                j = Array.BinarySearch(reversi_col, reversi_colcoordiantes);
            }

            return reversi_colcoordiantes;
        }
        

    }

    class Computer : Player{
        
    }

    class Hint : Game {

    }
}