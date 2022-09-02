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
            List<Player> player_list = new List<Player>();
            player_list = game.createPlayer(game.CurGameMode);
            
            // WriteLine($"You choose {game.CurGameType} and the game mode is {game.CurGameMode}");
            //board.drawBoard(game);
            // game.drawTest();
            game.startGame(game, board, player_list);
        }
    }

    class Game {
        // maximum number of each type of game
        readonly int[] NUM_MAXMOVE = new int[3] {0, 9, 64};

        // record the status of this game
        // odd number is player1, even number is player2
        private int num_ChessMove = 0;

        private Move move = new Move();
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

        public List<Player> createPlayer(string gm){
            // list {empty, player1, player2}
            List<Player> player_list = new List<Player>();
            // just for making the index number sync with player number
            Player p0 = new Player();
            Player p1 = new Human();
            player_list.Add(p0);
            player_list.Add(p1);
            if ( gm == (GameMode.cvh).ToString() ){
                Player p2 = new Computer();
                player_list.Add(p2);
            }else{
                Player p2 = new Human();
                player_list.Add(p2);
            }
            return player_list;
        }

        // return number of current player
        private int getPlayerNum(int val){
            // return number of the current player
            return val%2==0?1:2;
        }

        // start a new game
        public bool startGame( Game game, Board board, List<Player> player_list){
            //gm: current game mode
            //gt: current game type

            bool res = false;
            //assume that player1 always be the first player
            player_list[0].reversi_rowinput(1);
            
            if ( game.CurGameType == (GameType.tictactoe).ToString() ){
                while(game.num_ChessMove < NUM_MAXMOVE[(int)GameType.tictactoe] ){
                    int row = player_list[this.getPlayerNum(game.num_ChessMove)].tic_rowinput(this.getPlayerNum(game.num_ChessMove));
                    int col = player_list[this.getPlayerNum(game.num_ChessMove)].tic_colinput(this.getPlayerNum(game.num_ChessMove));
                    board.transferrowcoltobox(
                        row,
                        col,
                        this.getPlayerNum(game.num_ChessMove)
                        );
                    board.ticboard1();
                    int num_GameRes = move.checkresult(row, col, this.getPlayerNum(game.num_ChessMove), game.num_ChessMove);
                    if ( num_GameRes != 0 ) break;
                    game.changeStatus(game, 1);
                }
                
            }else{

            }
            return res;
        }

        // adjust calculator of number of moves
        public void changeStatus(Game game, int offset){
            // change num_ChessStatus everytime when any players move the chess or user back to previous move
            // offset: 1 (next move
            //         -1 (previous move
            if (offset == -1 || offset == 1){
                game.num_ChessMove += offset;
            }else{
                // use this function by giving the wrong value!!!!
            }
            
        }
    }

    class Move
    {
        // check valid move


        public int checkresult(int t_row, int t_col, int chessstatus, int moves)
        {
            // moves: current number of moves

            // return game result
            // 0: can be continue
            // 1: player1 win
            // 2: player2 win
            // 3: tie game

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
            { Console.WriteLine("Player 2 Win "); return 2; }
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
            else if (moves == 9) {
                Console.WriteLine("drawn game");
                return 3;
            }

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
        
        public int transferrowcoltobox(int row, int col, int status)
        {
            //Tic Tac Toe
            if (this.CurGameType == (GameType.tictactoe).ToString()){
                if (row == 2 && col == 2)
                {
                    if (status == 2)
                    { tic_coordiante[0] = "O"; }
                    else if (status == 1)
                    { tic_coordiante[0] = "X"; }
                    else
                    { tic_coordiante[0] = " "; }
                }
                else if (row == 2 && col == 4)
                {
                    if (status == 2)
                    { tic_coordiante[1] = "O"; }
                    else if (status == 1)
                    { tic_coordiante[1] = "X"; }
                    else
                    { tic_coordiante[1] = " "; }
                }
                else if (row == 2 && col == 6)
                {
                    if (status == 2)
                    { tic_coordiante[2] = "O"; }
                    else if (status == 1)
                    { tic_coordiante[2] = "X"; }
                    else
                    { tic_coordiante[2] = " "; }
                }
                else if (row == 4 && col == 2)
                {
                    if (status == 2)
                    { tic_coordiante[3] = "O"; }
                    else if (status == 1)
                    { tic_coordiante[3] = "X"; }
                    else
                    { tic_coordiante[3] = " "; }
                }
                else if (row == 4 && col == 4)
                {
                    if (status == 2)
                    { tic_coordiante[4] = "O"; }
                    else if (status == 1)
                    { tic_coordiante[4] = "X"; }
                    else
                    { tic_coordiante[4] = " "; }
                }
                else if (row == 4 && col == 6)
                {
                    if (status == 2)
                    { tic_coordiante[5] = "O"; }
                    else if (status == 1)
                    { tic_coordiante[5] = "X"; }
                    else
                    { tic_coordiante[5] = " "; }
                }
                else if (row == 6 && col == 2)
                {
                    if (status == 2)
                    { tic_coordiante[6] = "O"; }
                    else if ((status == 1))
                    { tic_coordiante[6] = "X"; }
                    else
                    { tic_coordiante[6] = " "; }
                }
                else if (row == 6 && col == 4)
                {
                    if (status == 2)
                    { tic_coordiante[7] = "O"; }
                    else if ((status == 1))
                    { tic_coordiante[7] = "X"; }
                    else
                    { tic_coordiante[7] = " "; }
                }
                else if (row == 6 && col == 6)
                {
                    if (status == 2)
                    { tic_coordiante[8] = "O"; }
                    else if ((status == 1))
                    { tic_coordiante[8] = "X"; }
                    else
                    { tic_coordiante[8] = " "; }
                }
            }else{

            }
            


            return 0;
        }
        public void ticboard1()
        {

            Console.WriteLine("\u250c\u2500\u2500\u252c\u2500\u2500\u252c\u2500\u2500\u2510");
            Console.WriteLine("\u2502\u2502\u2502 {2} \u2502", tic_coordiante[0], tic_coordiante[1], tic_coordiante[2]);
            Console.WriteLine("\u251c\u2500\u2500\u253c\u2500\u2500\u253c\u2500\u2500\u2524");
            Console.WriteLine("\u2502 {0} \u2502 {1} \u2502 {2} \u2502", tic_coordiante[3], tic_coordiante[4], tic_coordiante[5]);
            Console.WriteLine("\u251c\u2500\u2500\u253c\u2500\u2500\u253c\u2500\u2500\u2524");
            Console.WriteLine("\u2502 {0} \u2502 {1} \u2502 {2} \u2502", tic_coordiante[6], tic_coordiante[7], tic_coordiante[8]);
            Console.WriteLine("\u2514\u2500\u2500\u2534\u2500\u2500\u2534\u2500\u2500\u2518");

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
        public void initialreversi()
        {

            reversi_c[27] = "O";
            reversi_c[28] = "O";
            reversi_c[35] = "X";
            reversi_c[36] = "X";

        }

        public void place(int row, int col, int status)
        {
            if (row == 2 && col == 2)
            {
                if (status == 2)
                { reversi_c[0] = "O"; }
                else if (status == 1)
                { reversi_c[0] = "X"; }
                else
                { reversi_c[0] = " "; }
            }
            else if (row == 2 && col == 4)
            {
                if (status == 2)
                { reversi_c[1] = "O"; }
                else if (status == 1)
                { reversi_c[1] = "X"; }
                else
                { reversi_c[1] = " "; }
            }
            else if (row == 2 && col == 6)
            {
                if (status == 2)
                { reversi_c[2] = "O"; }
                else if (status == 1)
                { reversi_c[2] = "X"; }
                else
                { reversi_c[2] = " "; }
            }
            else if (row == 2 && col == 8)
            {
                if (status == 2)
                { reversi_c[3] = "O"; }
                else if (status == 1)
                { reversi_c[3] = "X"; }
                else
                { reversi_c[3] = " "; }
            }
            else if (row == 2 && col == 10)
            {
                if (status == 2)
                { reversi_c[4] = "O"; }
                else if (status == 1)
                { reversi_c[4] = "X"; }
                else
                { reversi_c[4] = " "; }
            }
            else if (row == 2 && col == 12)
            {
                if (status == 2)
                { reversi_c[5] = "O"; }
                else if (status == 1)
                { reversi_c[5] = "X"; }
                else
                { reversi_c[5] = " "; }
            }
            else if (row == 2 && col == 14)
            {
                if (status == 2)
                { reversi_c[6] = "O"; }
                else if (status == 1)
                { reversi_c[6] = "X"; }
                else
                { reversi_c[6] = " "; }
            }
            else if (row == 2 && col == 16)
            {
                if (status == 2)
                { reversi_c[7] = "O"; }
                else if (status == 1)
                { reversi_c[7] = "X"; }
                else
                { reversi_c[7] = " "; }
            }
            else if (row == 4 && col == 2)
            {
                if (status == 2)
                { reversi_c[8] = "O"; }
                else if (status == 1)
                { reversi_c[8] = "X"; }
                else
                { reversi_c[8] = " "; }
            }
            else if (row == 4 && col == 4)
            {
                if (status == 2)
                { reversi_c[9] = "O"; }
                else if (status == 1)
                { reversi_c[9] = "X"; }
                else
                { reversi_c[9] = " "; }
            }
            else if (row == 4 && col == 6)
            {
                if (status == 2)
                { reversi_c[10] = "O"; }
                else if (status == 1)
                { reversi_c[10] = "X"; }
                else
                { reversi_c[10] = " "; }
            }
            else if (row == 4 && col == 8)
            {
                if (status == 2)
                { reversi_c[11] = "O"; }
                else if (status == 1)
                { reversi_c[11] = "X"; }
                else
                { reversi_c[11] = " "; }
            }
            else if (row == 4 && col == 10)
            {
                if (status == 2)
                { reversi_c[12] = "O"; }
                else if (status == 1)
                { reversi_c[12] = "X"; }
                else
                { reversi_c[12] = " "; }
            }
            else if (row == 4 && col == 12)
            {
                if (status == 2)
                { reversi_c[13] = "O"; }
                else if (status == 1)
                { reversi_c[13] = "X"; }
                else
                { reversi_c[13] = " "; }
            }
            else if (row == 4 && col == 14)
            {
                if (status == 2)
                { reversi_c[14] = "O"; }
                else if (status == 1)
                { reversi_c[14] = "X"; }
                else
                { reversi_c[14] = " "; }
            }
            else if (row == 4 && col == 16)
            {
                if (status == 2)
                { reversi_c[16] = "O"; }
                else if (status == 1)
                { reversi_c[16] = "X"; }
                else
                { reversi_c[16] = " "; }
            }
            else if (row == 6 && col == 2)
            {
                if (status == 2)
                { reversi_c[17] = "O"; }
                else if ((status == 1))
                { reversi_c[17] = "X"; }
                else
                { reversi_c[17] = " "; }
            }
            else if (row == 6 && col == 4)
            {
                if (status == 2)
                { reversi_c[17] = "O"; }
                else if ((status == 1))
                { reversi_c[17] = "X"; }
                else
                { reversi_c[17] = " "; }
            }
            else if (row == 6 && col == 6)
            {
                if (status == 2)
                { reversi_c[18] = "O"; }
                else if ((status == 1))
                { reversi_c[18] = "X"; }
                else
                { reversi_c[18] = " "; }
            }
            else if (row == 6 && col == 8)
            {
                if (status == 2)
                { reversi_c[19] = "O"; }
                else if ((status == 1))
                { reversi_c[19] = "X"; }
                else
                { reversi_c[19] = " "; }
            }
            else if (row == 6 && col == 10)
            {
                if (status == 2)
                { reversi_c[20] = "O"; }
                else if ((status == 1))
                { reversi_c[20] = "X"; }
                else
                { reversi_c[20] = " "; }
            }
            else if (row == 6 && col == 12)
            {
                if (status == 2)
                { reversi_c[21] = "O"; }
                else if ((status == 1))
                { reversi_c[21] = "X"; }
                else
                { reversi_c[21] = " "; }
            }
            else if (row == 6 && col == 14)
            {
                if (status == 2)
                { reversi_c[22] = "O"; }
                else if ((status == 1))
                { reversi_c[22] = "X"; }
                else
                { reversi_c[22] = " "; }
            }
            else if (row == 6 && col == 16)
            {
                if (status == 2)
                { reversi_c[23] = "O"; }
                else if ((status == 1))
                { reversi_c[23] = "X"; }
                else
                { reversi_c[23] = " "; }
            }
            else if (row == 8 && col == 2)
            {
                if (status == 2)
                { reversi_c[24] = "O"; }
                else if ((status == 1))
                { reversi_c[24] = "X"; }
                else
                { reversi_c[24] = " "; }
            }
            else if (row == 8 && col == 4)
            {
                if (status == 2)
                { reversi_c[25] = "O"; }
                else if ((status == 1))
                { reversi_c[25] = "X"; }
                else
                { reversi_c[25] = " "; }
            }
            else if (row == 8 && col == 6)
            {
                if (status == 2)
                { reversi_c[26] = "O"; }
                else if ((status == 1))
                { reversi_c[26] = "X"; }
                else
                { reversi_c[26] = " "; }
            }
            else if (row == 8 && col == 8)
            {
                if (status == 2)
                { reversi_c[27] = "O"; }
                else if ((status == 1))
                { reversi_c[27] = "X"; }
                else
                { reversi_c[27] = " "; }
            }
            else if (row == 8 && col == 10)
            {
                if (status == 2)
                { reversi_c[28] = "O"; }
                else if ((status == 1))
                { reversi_c[28] = "X"; }
                else
                { reversi_c[28] = " "; }
            }
            else if (row == 8 && col == 12)
            {
                if (status == 2)
                { reversi_c[29] = "O"; }
                else if ((status == 1))
                { reversi_c[29] = "X"; }
                else
                { reversi_c[29] = " "; }
            }
            else if (row == 8 && col == 14)
            {
                if (status == 2)
                { reversi_c[30] = "O"; }
                else if ((status == 1))
                { reversi_c[30] = "X"; }
                else
                { reversi_c[30] = " "; }
            }
            else if (row == 8 && col == 16)
            {
                if (status == 2)
                { reversi_c[31] = "O"; }
                else if ((status == 1))
                { reversi_c[31] = "X"; }
                else
                { reversi_c[31] = " "; }
            }
            else if (row == 10 && col == 2)
            {
                if (status == 2)
                { reversi_c[32] = "O"; }
                else if ((status == 1))
                { reversi_c[32] = "X"; }
                else
                { reversi_c[32] = " "; }
            }
            else if (row == 10 && col == 4)
            {
                if (status == 2)
                { reversi_c[33] = "O"; }
                else if ((status == 1))
                { reversi_c[33] = "X"; }
                else
                { reversi_c[33] = " "; }
            }
            else if (row == 10 && col == 6)
            {
                if (status == 2)
                { reversi_c[34] = "O"; }
                else if ((status == 1))
                { reversi_c[34] = "X"; }
                else
                { reversi_c[34] = " "; }
            }
            else if (row == 10 && col == 8)
            {
                if (status == 2)
                { reversi_c[35] = "O"; }
                else if ((status == 1))
                { reversi_c[35] = "X"; }
                else
                { reversi_c[35] = " "; }
            }
            else if (row == 10 && col == 10)
            {
                if (status == 2)
                { reversi_c[36] = "O"; }
                else if ((status == 1))
                { reversi_c[36] = "X"; }
                else
                { reversi_c[36] = " "; }
            }
            else if (row == 10 && col == 12)
            {
                if (status == 2)
                { reversi_c[37] = "O"; }
                else if ((status == 1))
                { reversi_c[37] = "X"; }
                else
                { reversi_c[37] = " "; }
            }
            else if (row == 10 && col == 14)
            {
                if (status == 2)
                { reversi_c[38] = "O"; }
                else if ((status == 1))
                { reversi_c[38] = "X"; }
                else
                { reversi_c[38] = " "; }
            }
            else if (row == 10 && col == 16)
            {
                if (status == 2)
                { reversi_c[39] = "O"; }
                else if ((status == 1))
                { reversi_c[39] = "X"; }
                else
                { reversi_c[39] = " "; }
            }
            else if (row == 12 && col == 2)
            {
                if (status == 2)
                { reversi_c[40] = "O"; }
                else if ((status == 1))
                { reversi_c[40] = "X"; }
                else
                { reversi_c[40] = " "; }
            }
            else if (row == 12 && col == 4)
            {
                if (status == 2)
                { reversi_c[41] = "O"; }
                else if ((status == 1))
                { reversi_c[41] = "X"; }
                else
                { reversi_c[41] = " "; }
            }
            else if (row == 12 && col == 6)
            {
                if (status == 2)
                { reversi_c[42] = "O"; }
                else if ((status == 1))
                { reversi_c[42] = "X"; }
                else
                { reversi_c[42] = " "; }
            }
            else if (row == 12 && col == 8)
            {
                if (status == 2)
                { reversi_c[43] = "O"; }
                else if ((status == 1))
                { reversi_c[43] = "X"; }
                else
                { reversi_c[43] = " "; }
            }
            else if (row == 12 && col == 10)
            {
                if (status == 2)
                { reversi_c[44] = "O"; }
                else if ((status == 1))
                { reversi_c[44] = "X"; }
                else
                { reversi_c[44] = " "; }
            }
            else if (row == 12 && col == 12)
            {
                if (status == 2)
                { reversi_c[45] = "O"; }
                else if ((status == 1))
                { reversi_c[45] = "X"; }
                else
                { reversi_c[45] = " "; }
            }
            else if (row == 12 && col == 14)
            {
                if (status == 2)
                { reversi_c[46] = "O"; }
                else if ((status == 1))
                { reversi_c[46] = "X"; }
                else
                { reversi_c[46] = " "; }
            }
            else if (row == 12 && col == 16)
            {
                if (status == 2)
                { reversi_c[47] = "O"; }
                else if ((status == 1))
                { reversi_c[47] = "X"; }
                else
                { reversi_c[47] = " "; }
            }
            else if (row == 14 && col == 2)
            {
                if (status == 2)
                { reversi_c[48] = "O"; }
                else if ((status == 1))
                { reversi_c[48] = "X"; }
                else
                { reversi_c[48] = " "; }
            }
            else if (row == 14 && col == 4)
            {
                if (status == 2)
                { reversi_c[49] = "O"; }
                else if ((status == 1))
                { reversi_c[49] = "X"; }
                else
                { reversi_c[49] = " "; }
            }
            else if (row == 14 && col == 6)
            {
                if (status == 2)
                { reversi_c[50] = "O"; }
                else if ((status == 1))
                { reversi_c[50] = "X"; }
                else
                { reversi_c[50] = " "; }
            }
            else if (row == 14 && col == 8)
            {
                if (status == 2)
                { reversi_c[51] = "O"; }
                else if ((status == 1))
                { reversi_c[51] = "X"; }
                else
                { reversi_c[51] = " "; }
            }
            else if (row == 14 && col == 10)
            {
                if (status == 2)
                { reversi_c[52] = "O"; }
                else if ((status == 1))
                { reversi_c[52] = "X"; }
                else
                { reversi_c[52] = " "; }
            }
            else if (row == 14 && col == 12)
            {
                if (status == 2)
                { reversi_c[53] = "O"; }
                else if ((status == 1))
                { reversi_c[53] = "X"; }
                else
                { reversi_c[53] = " "; }
            }
            else if (row == 14 && col == 14)
            {
                if (status == 2)
                { reversi_c[54] = "O"; }
                else if ((status == 1))
                { reversi_c[54] = "X"; }
                else
                { reversi_c[54] = " "; }
            }
            else if (row == 14 && col == 16)
            {
                if (status == 2)
                { reversi_c[55] = "O"; }
                else if ((status == 1))
                { reversi_c[55] = "X"; }
                else
                { reversi_c[55] = " "; }
            }
            else if (row == 16 && col == 2)
            {
                if (status == 2)
                { reversi_c[56] = "O"; }
                else if ((status == 1))
                { reversi_c[56] = "X"; }
                else
                { reversi_c[56] = " "; }
            }
            else if (row == 16 && col == 4)
            {
                if (status == 2)
                { reversi_c[57] = "O"; }
                else if ((status == 1))
                { reversi_c[57] = "X"; }
                else
                { reversi_c[57] = " "; }
            }
            else if (row == 16 && col == 6)
            {
                if (status == 2)
                { reversi_c[58] = "O"; }
                else if ((status == 1))
                { reversi_c[58] = "X"; }
                else
                { reversi_c[58] = " "; }
            }
            else if (row == 16 && col == 8)
            {
                if (status == 2)
                { reversi_c[59] = "O"; }
                else if ((status == 1))
                { reversi_c[59] = "X"; }
                else
                { reversi_c[59] = " "; }
            }
            else if (row == 16 && col == 10)
            {
                if (status == 2)
                { reversi_c[60] = "O"; }
                else if ((status == 1))
                { reversi_c[60] = "X"; }
                else
                { reversi_c[60] = " "; }
            }
            else if (row == 16 && col == 12)
            {
                if (status == 2)
                { reversi_c[61] = "O"; }
                else if ((status == 1))
                { reversi_c[61] = "X"; }
                else
                { reversi_c[61] = " "; }
            }
            else if (row == 16 && col == 14)
            {
                if (status == 2)
                { reversi_c[62] = "O"; }
                else if ((status == 1))
                { reversi_c[62] = "X"; }
                else
                { reversi_c[62] = " "; }
            }
            else if (row == 16 && col == 16)
            {
                if (status == 2)
                { reversi_c[63] = "O"; }
                else if ((status == 1))
                { reversi_c[63] = "X"; }
                else
                { reversi_c[63] = " "; }
            }
        }

        public void reversiboard1()
        {
            Console.WriteLine("Row: 2, 4, 6, 8, 10, 12, 14, 16");
            Console.WriteLine("Col: 2, 4, 6, 8, 10, 12, 14, 16");
            Console.WriteLine("   --------------------------");
            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[0], reversi_c[1], reversi_c[2], reversi_c[3], reversi_c[4], reversi_c[5], reversi_c[6], reversi_c[7]);
            Console.WriteLine("   --------------------------");

            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[8], reversi_c[9], reversi_c[10], reversi_c[11], reversi_c[12], reversi_c[13], reversi_c[14], reversi_c[15]);
            Console.WriteLine("   --------------------------");

            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[16], reversi_c[17], reversi_c[18], reversi_c[19], reversi_c[20], reversi_c[21], reversi_c[22], reversi_c[23]);
            Console.WriteLine("   --------------------------");

            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[24], reversi_c[25], reversi_c[26], reversi_c[27], reversi_c[28], reversi_c[29], reversi_c[30], reversi_c[31]);
            Console.WriteLine("   --------------------------");

            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[32], reversi_c[33], reversi_c[34], reversi_c[35], reversi_c[36], reversi_c[37], reversi_c[38], reversi_c[39]);
            Console.WriteLine("   --------------------------");

            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[40], reversi_c[41], reversi_c[42], reversi_c[43], reversi_c[44], reversi_c[45], reversi_c[46], reversi_c[47]);
            Console.WriteLine("   --------------------------");

            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[48], reversi_c[49], reversi_c[50], reversi_c[51], reversi_c[52], reversi_c[53], reversi_c[54], reversi_c[55]);
            Console.WriteLine("   --------------------------");

            Console.WriteLine("   | {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |",
                reversi_c[56], reversi_c[57], reversi_c[58], reversi_c[59], reversi_c[60], reversi_c[61], reversi_c[62], reversi_c[63]);
            Console.WriteLine("   --------------------------"); ;




        }



        public int counter = 1;
        public int k = 0;
        public static string[] tic_coordiante = new string[9];
        public static string[] reversi_c = new string[64];

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

        virtual public int tic_rowinput(int chessstatus){ return 0; }
        virtual public int tic_colinput(int chessstatus){ return 0; }
        virtual public int reversi_rowinput(int chessstatus){ return 0; }
        virtual public int reversi_colinput(int chessstatus){ return 0; }
    }

    class Human : Player{
        override public int tic_rowinput(int chessstatus)
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
        override public int tic_colinput(int chessstatus)
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

        override public int reversi_rowinput(int chessstatus)
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
        override public int reversi_colinput(int chessstatus)
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
        public void tictactoerandom()
        {
            Random rand = new Random();

            int cooridint = rand.Next(1, 9);

            int t_row = 0;
            int t_col = 0;


            if (cooridint == 1)
            {
                t_row = 2;
                t_col = 2;
            }
            else if (cooridint == 2)
            {
                t_row = 2;
                t_col = 4;
            }
            else if (cooridint == 3)
            {
                t_row = 2;
                t_col = 6;
            }
            else if (cooridint == 4)
            {
                t_row = 4;
                t_col = 2;
            }
            else if (cooridint == 5)
            {
                t_row = 4;
                t_col = 4;
            }
            else if (cooridint == 6)
            {
                t_row = 4;
                t_col = 6;
            }
            else if (cooridint == 7)
            {
                t_row = 6;
                t_col = 2;
            }
            else if (cooridint == 8)
            {
                t_row = 6;
                t_col = 4;
            }
            else if (cooridint == 9)
            {
                t_row = 6;
                t_col = 6;
            }
        }

        public void reversirandom()
        {
            Random rand = new Random();

            int cooridint = rand.Next(1, 64);

            int t_row = 0;
            int t_col = 0;


            if (cooridint == 1)
            {
                t_row = 2;
                t_col = 2;
            }
            else if (cooridint == 2)
            {
                t_row = 2;
                t_col = 4;
            }
            else if (cooridint == 3)
            {
                t_row = 2;
                t_col = 6;
            }
            else if (cooridint == 4)
            {
                t_row = 2;
                t_col = 8;
            }
            else if (cooridint == 5)
            {
                t_row = 2;
                t_col = 10;
            }
            else if (cooridint == 6)
            {
                t_row = 2;
                t_col = 12;
            }
            else if (cooridint == 7)
            {
                t_row = 2;
                t_col = 14;
            }
            else if (cooridint == 8)
            {
                t_row = 2;
                t_col = 16;
            }
            else if (cooridint == 9)
            {
                t_row = 4;
                t_col = 2;
            }
            else if (cooridint == 9)
            {
                t_row = 4;
                t_col = 4;
            }
            else if (cooridint == 10)
            {
                t_row = 4;
                t_col = 6;
            }
            else if (cooridint == 11)
            {
                t_row = 4;
                t_col = 8;
            }
            else if (cooridint == 12)
            {
                t_row = 4;
                t_col = 10;
            }
            else if (cooridint == 13)
            {
                t_row = 4;
                t_col = 12;
            }
            else if (cooridint == 14)
            {
                t_row = 4;
                t_col = 14;
            }
            else if (cooridint == 15)
            {
                t_row = 4;
                t_col = 16;
            }
            else if (cooridint == 16)
            {
                t_row = 6;
                t_col = 2;
            }
            else if (cooridint == 17)
            {
                t_row = 6;
                t_col = 4;
            }
            else if (cooridint == 18)
            {
                t_row = 6;
                t_col = 6;
            }
            else if (cooridint == 19)
            {
                t_row = 6;
                t_col = 8;
            }
            else if (cooridint == 20)
            {
                t_row = 6;
                t_col = 10;
            }
            else if (cooridint == 21)
            {
                t_row = 6;
                t_col = 12;
            }
            else if (cooridint == 22)
            {
                t_row = 6;
                t_col = 14;
            }
            else if (cooridint == 23)
            {
                t_row = 6;
                t_col = 16;
            }
            else if (cooridint == 24)
            {
                t_row = 8;
                t_col = 2;
            }
            else if (cooridint == 25)
            {
                t_row = 8;
                t_col = 4;
            }
            else if (cooridint == 26)
            {
                t_row = 8;
                t_col = 6;
            }
            else if (cooridint == 27)
            {
                t_row = 8;
                t_col = 8;
            }
            else if (cooridint == 28)
            {
                t_row = 8;
                t_col = 10;
            }
            else if (cooridint == 29)
            {
                t_row = 8;
                t_col = 12;
            }
            else if (cooridint == 30)
            {
                t_row = 8;
                t_col = 14;
            }
            else if (cooridint == 31)
            {
                t_row = 8;
                t_col = 16;
            }
            else if (cooridint == 32)
            {
                t_row = 10;
                t_col = 2;
            }
            else if (cooridint == 33)
            {
                t_row = 10;
                t_col = 4;
            }
            else if (cooridint == 34)
            {
                t_row = 10;
                t_col = 6;
            }
            else if (cooridint == 35)
            {
                t_row = 10;
                t_col = 8;
            }
            else if (cooridint == 36)
            {
                t_row = 10;
                t_col = 10;
            }
            else if (cooridint == 37)
            {
                t_row = 10;
                t_col = 12;
            }
            else if (cooridint == 38)
            {
                t_row = 10;
                t_col = 14;
            }
            else if (cooridint == 39)
            {
                t_row = 10;
                t_col = 16;
            }
            else if (cooridint == 40)
            {
                t_row = 12;
                t_col = 2;
            }
            else if (cooridint == 41)
            {
                t_row = 12;
                t_col = 4;
            }
            else if (cooridint == 42)
            {
                t_row = 12;
                t_col = 6;
            }
            else if (cooridint == 43)
            {
                t_row = 12;
                t_col = 8;
            }
            else if (cooridint == 44)
            {
                t_row = 12;
                t_col = 10;
            }
            else if (cooridint == 45)
            {
                t_row = 12;
                t_col = 12;
            }
            else if (cooridint == 46)
            {
                t_row = 12;
                t_col = 14;
            }
            else if (cooridint == 47)
            {
                t_row = 12;
                t_col = 16;
            }
            else if (cooridint == 48)
            {
                t_row = 14;
                t_col = 2;
            }
            else if (cooridint == 49)
            {
                t_row = 14;
                t_col = 4;
            }
            else if (cooridint == 50)
            {
                t_row = 14;
                t_col = 6;
            }
            else if (cooridint == 51)
            {
                t_row = 14;
                t_col = 8;
            }
            else if (cooridint == 52)
            {
                t_row = 14;
                t_col = 10;
            }
            else if (cooridint == 53)
            {
                t_row = 14;
                t_col = 12;
            }
            else if (cooridint == 54)
            {
                t_row = 14;
                t_col = 14;
            }
            else if (cooridint == 55)
            {
                t_row = 14;
                t_col = 16;
            }
            else if (cooridint == 56)
            {
                t_row = 16;
                t_col = 2;
            }
            else if (cooridint == 57)
            {
                t_row = 16;
                t_col = 4;
            }
            else if (cooridint == 58)
            {
                t_row = 16;
                t_col = 6;
            }
            else if (cooridint == 59)
            {
                t_row = 16;
                t_col = 8;
            }
            else if (cooridint == 60)
            {
                t_row = 16;
                t_col = 10;
            }
            else if (cooridint == 61)
            {
                t_row = 16;
                t_col = 12;
            }
            else if (cooridint == 62)
            {
                t_row = 16;
                t_col = 14;
            }
            else if (cooridint == 63)
            {
                t_row = 16;
                t_col = 16;
            }

        }
    }

    class Hint : Game {

    }
}