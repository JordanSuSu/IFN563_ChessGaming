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
            Console.OutputEncoding = System.Text.Encoding.Unicode;
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
        private Hint hint = new Hint();
        private History history = new History();
        private string str_CurGameMode = (GameMode.hvh).ToString();
        private string str_CurGameType = (GameType.reversi).ToString();
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

        public void displayHint(){
            hint.demo();
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

            board.initialChessBoard(game);
            bool res = false;
            //assume that player1 always be the first player
            player_list[0].reversi_rowinput(1);
            
            if ( game.CurGameType == (GameType.tictactoe).ToString() ){
                while(game.num_ChessMove < NUM_MAXMOVE[(int)GameType.tictactoe] ){
                    int num_CurPlayrer = this.getPlayerNum(game.num_ChessMove);
                    int row = player_list[num_CurPlayrer].tic_rowinput(num_CurPlayrer);
                    int col = player_list[num_CurPlayrer].tic_colinput(num_CurPlayrer);
                    List<int> list_CurMove = new List<int>() {row, col};

                    // check whether this move is valid
                    while (!history.checkAvailable(list_CurMove)){
                        WriteLine($"This position ({row},{col}) has been placed a chess!!");
                        WriteLine("Please re-enter a valid position!!");
                        row = player_list[num_CurPlayrer].tic_rowinput(num_CurPlayrer);
                        col = player_list[num_CurPlayrer].tic_colinput(num_CurPlayrer);
                        list_CurMove = new List<int>() {row, col};
                    }

                    board.transferrowcoltobox(
                        row,
                        col,
                        this.getPlayerNum(game.num_ChessMove),
                        game.CurGameType
                        );
                    board.ticboard1();
                    int num_GameRes = move.checkresult(row, col, this.getPlayerNum(game.num_ChessMove), game.num_ChessMove);
                    if ( num_GameRes != 0 ) break;
                    game.changeStatus(game, 1);
                }
                
            }else{
                while(game.num_ChessMove < NUM_MAXMOVE[(int)GameType.reversi] ){
                    int row = player_list[this.getPlayerNum(game.num_ChessMove)].reversi_rowinput(this.getPlayerNum(game.num_ChessMove));
                    int col = player_list[this.getPlayerNum(game.num_ChessMove)].reversi_colinput(this.getPlayerNum(game.num_ChessMove));
                    board.transferrowcoltobox(
                        row,
                        col,
                        this.getPlayerNum(game.num_ChessMove),
                        game.CurGameType
                        );
                    board.reversiboard1();
                    // int num_GameRes = move.checkresult(row, col, this.getPlayerNum(game.num_ChessMove), game.num_ChessMove);
                    int num_GameRes = 0;
                    if ( num_GameRes != 0 ) break;
                    game.changeStatus(game, 1);
                }
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
        public int transferrowcoltobox(int row, int col, int status, string gt)
        {
            // WriteLine($"[Transferrowcoltobox]:{status}--{this.CurGameType}");
            //Tic Tac Toe
            if (gt == (GameType.tictactoe).ToString()){
                int cal_row = (row / 2)-1;
                int cal_col = (col / 2)-1;
                int index = cal_row*3 + cal_col;
                tic_c[index] = status == 2 ? "\u202FO\u202F" : status == 1 ? "\u202FX\u202F" : "\u202F\u202F\u202F";
            }else{
                int cal_row = (row / 2)-1;
                int cal_col = (col / 2)-1;
                int index = cal_row*8 + cal_col;
                reversi_c[index] = status == 2 ? "\u202F\u25CB\u202F" : status == 1 ? "\u202F\u25CF\u202F" : "\u202F\u202F\u202F";
            }
            
            // 25CB white circle
            // 25CF black circle


            return 0;
        }
        public void ticboard1()
        {

            Console.WriteLine("\u250c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u2510");
            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502", tic_c[0], tic_c[1], tic_c[2]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");
            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502", tic_c[3], tic_c[4], tic_c[5]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");
            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502", tic_c[6], tic_c[7], tic_c[8]);
            Console.WriteLine("\u2514\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2518");

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

            reversi_c[27] = "\u202FO\u202F";
            reversi_c[28] = "\u202FO\u202F";
            reversi_c[35] = "\u202FX\u202F";
            reversi_c[36] = "\u202FX\u202F";

        }

        
        public void reversiboard1()
        {
            Console.WriteLine("Row: 2, 4, 6, 8, 10, 12, 14, 16");
            Console.WriteLine("Col: 2, 4, 6, 8, 10, 12, 14, 16");
            Console.WriteLine("\u250c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u2510");
            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[0], reversi_c[1], reversi_c[2], reversi_c[3], reversi_c[4], reversi_c[5], reversi_c[6], reversi_c[7]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");

            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[8], reversi_c[9], reversi_c[10], reversi_c[11], reversi_c[12], reversi_c[13], reversi_c[14], reversi_c[15]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");

            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[16], reversi_c[17], reversi_c[18], reversi_c[19], reversi_c[20], reversi_c[21], reversi_c[22], reversi_c[23]);
           Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");

            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[24], reversi_c[25], reversi_c[26], reversi_c[27], reversi_c[28], reversi_c[29], reversi_c[30], reversi_c[31]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");

            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[32], reversi_c[33], reversi_c[34], reversi_c[35], reversi_c[36], reversi_c[37], reversi_c[38], reversi_c[39]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");

            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[40], reversi_c[41], reversi_c[42], reversi_c[43], reversi_c[44], reversi_c[45], reversi_c[46], reversi_c[47]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");

            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[48], reversi_c[49], reversi_c[50], reversi_c[51], reversi_c[52], reversi_c[53], reversi_c[54], reversi_c[55]);
            Console.WriteLine("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");

            Console.WriteLine("\u2502{0}\u2502{1}\u2502{2}\u2502{3}\u2502{4}\u2502{5}\u2502{6}\u2502{7}\u2502",
                reversi_c[56], reversi_c[57], reversi_c[58], reversi_c[59], reversi_c[60], reversi_c[61], reversi_c[62], reversi_c[63]);
            Console.WriteLine("\u2514\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2518");




        }

        // clean the content save in the tic_coordi
        public void initialChessBoard(Game game){
            if (game.CurGameType == (GameType.tictactoe).ToString()){
                for (int i = 0 ; i < 9 ; i ++)
                    tic_c[i] = STR_SPACE;
            }else{
                for (int i = 0 ; i < 64 ; i ++)
                    reversi_c[i] = STR_SPACE;
                
                // default rule
                reversi_c[27] = "\u202F\u25CB\u202F";
                reversi_c[28] = "\u202F\u25CB\u202F";
                reversi_c[35] = "\u202F\u25CF\u202F";
                reversi_c[36] = "\u202F\u25CF\u202F";
            }
        }

        const string STR_SPACE = "\u202F\u202F\u202F";

        public int counter = 1;
        public int k = 0;
        public static string[] tic_c = new string[9]; //saving chess at each position
        public static string[] reversi_c = new string[64]; //saving chess at each position

        private List<int> rowcolstatuslist = new List<int>();
        public List<int> Rowcolstatuslist
        {
            get { return rowcolstatuslist; }
            set { rowcolstatuslist.Add(Convert.ToInt32(value)); }
        }
    }

    class History : Board{
        //Save all moves
        protected Stack<List<int>> forwardTrack = new Stack<List<int>>();
        //Save the moves which are undo by user
        protected Queue<List<int>> backTrack = new Queue<List<int>>();

        public bool checkAvailable(List<int> coor){
            // check whether that position is empty
            // true: available
            // false: there already exist a chess at this position
            return !forwardTrack.Contains(coor);
        }
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
            
            // Help system will show when user enter the specific code.
            HelpSystem:
            while ( rowinput == "help" ){
                // display the help system
                // ....

                Console.Write("Player" + chessstatus + " Enter row coordiantes: ");
                rowinput = Console.ReadLine();
            }

            bool rowresult = Int32.TryParse(rowinput, out tic_rowcoordiantes);
            tic_rowcoordiantes *= 2; 
            int i = Array.BinarySearch(tic_row, tic_rowcoordiantes);


            while (rowresult == false || i < 0)
            {
                Console.Write("please enter a valid row coordiantes: ");
                rowinput = Console.ReadLine();

                // user enter the specific code for help system
                if (rowinput == "help") goto HelpSystem;

                rowresult = Int32.TryParse(rowinput, out tic_rowcoordiantes);
                tic_rowcoordiantes *= 2;
                i = Array.BinarySearch(tic_row, tic_rowcoordiantes);
            }

            return tic_rowcoordiantes;
        }
        override public int tic_colinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter col coordiantes: ");
            string colinput = Console.ReadLine();
            HelpSystem:
            while ( colinput == "help" ){
                // display the help system
                // ....
                this.displayHint();
                Console.Write("Player" + chessstatus + " Enter col coordiantes: ");
                colinput = Console.ReadLine();
            }
            bool colresult = Int32.TryParse(colinput, out tic_colcoordiantes);
            tic_colcoordiantes *= 2;
            int j = Array.BinarySearch(tic_col, tic_colcoordiantes);

            while (colresult == false || j < 0)
            {
                Console.Write("please enter a valid column coordiantes: ");
                colinput = Console.ReadLine();

                // user enter the specific code for help system
                if (colinput == "help") goto HelpSystem;
                
                colresult = Int32.TryParse(colinput, out tic_colcoordiantes);
                tic_colcoordiantes *= 2;
                j = Array.BinarySearch(tic_col, tic_colcoordiantes);
            }

            return tic_colcoordiantes;
        }

        override public int reversi_rowinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter row coordiantes: ");
            string rowinput = Console.ReadLine();
            // Help system will show when user enter the specific code.
            HelpSystem:
            while ( rowinput == "help" ){
                // display the help system
                // ....

                Console.Write("Player" + chessstatus + " Enter row coordiantes: ");
                rowinput = Console.ReadLine();
            }
            bool rowresult = Int32.TryParse(rowinput, out reversi_rowcoordiantes);
            reversi_rowcoordiantes *= 2;
            int i = Array.BinarySearch(reversi_row, reversi_rowcoordiantes);


            while (rowresult == false || i < 0)
            {
                Console.Write("please enter a valid row coordiantes: ");
                rowinput = Console.ReadLine();

                // user enter the specific code for help system
                if (rowinput == "help") goto HelpSystem;

                rowresult = Int32.TryParse(ReadLine(), out reversi_rowcoordiantes);
                reversi_rowcoordiantes *= 2;
                i = Array.BinarySearch(reversi_row, reversi_rowcoordiantes);
            }

            return reversi_rowcoordiantes;
        }
        override public int reversi_colinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter col coordiantes: ");
            string colinput = Console.ReadLine();

            HelpSystem:
            while ( colinput == "help" ){
                // display the help system
                // ....

                Console.Write("Player" + chessstatus + " Enter col coordiantes: ");
                colinput = Console.ReadLine();
            }

            bool colresult = Int32.TryParse(colinput, out reversi_colcoordiantes);
            reversi_colcoordiantes *= 2;
            int j = Array.BinarySearch(reversi_col, reversi_colcoordiantes);

            while (colresult == false || j < 0)
            {

                Console.Write("please enter a valid column coordiantes: ");
                colinput = Console.ReadLine();

                // user enter the specific code for help system
                if (colinput == "help") goto HelpSystem;

                colresult = Int32.TryParse(ReadLine(), out reversi_colcoordiantes);
                reversi_colcoordiantes *= 2;
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

            int cooridint = rand.Next(0, 63);

            int row = 0;
            int col = 0;
            if (cooridint == 0)
            {
                row = 2;
                col = 2;
            }
            else if (cooridint == 1)
            {
                row = 2;
                col = 4;
            }
            else if (cooridint == 2)
            {
                row = 2;
                col = 6;
            }
            else if (cooridint == 3)
            {
                row = 2;
                col = 8;
            }
            else if (cooridint == 4)
            {
                row = 2;
                col = 10;
            }
            else if (cooridint == 5)
            {
                row = 2;
                col = 12;
            }
            else if (cooridint == 6)
            {
                row = 2;
                col = 14;
            }
            else if (cooridint == 7)
            {
                row = 2;
                col = 16;
            }
            else if (cooridint == 8)
            {
                row = 4;
                col = 2;
            }
            else if (cooridint == 9)
            {
                row = 4;
                col = 4;
            }
            else if (cooridint == 10)
            {
                row = 4;
                col = 6;
            }
            else if (cooridint == 11)
            {
                row = 4;
                col = 8;
            }
            else if (cooridint == 12)
            {
                row = 4;
                col = 10;
            }
            else if (cooridint == 13)
            {
                row = 4;
                col = 12;
            }
            else if (cooridint == 14)
            {
                row = 4;
                col = 14;
            }
            else if (cooridint == 15)
            {
                row = 4;
                col = 16;
            }
            else if (cooridint == 16)
            {
                row = 6;
                col = 2;
            }
            else if (cooridint == 17)
            {
                row = 6;
                col = 4;
            }
            else if (cooridint == 18)
            {
                row = 6;
                col = 6;
            }
            else if (cooridint == 19)
            {
                row = 6;
                col = 8;
            }
            else if (cooridint == 20)
            {
                row = 6;
                col = 10;
            }
            else if (cooridint == 21)
            {
                row = 6;
                col = 12;
            }
            else if (cooridint == 22)
            {
                row = 6;
                col = 14;
            }
            else if (cooridint == 23)
            {
                row = 6;
                col = 16;
            }
            else if (cooridint == 24)
            {
                row = 8;
                col = 2;
            }
            else if (cooridint == 25)
            {
                row = 8;
                col = 4;
            }
            else if (cooridint == 26)
            {
                row = 8;
                col = 6;
            }
            else if (cooridint == 27)
            {
                row = 8;
                col = 8;
            }
            else if (cooridint == 28)
            {
                row = 8;
                col = 10;
            }
            else if (cooridint == 29)
            {
                row = 8;
                col = 12;
            }
            else if (cooridint == 30)
            {
                row = 8;
                col = 14;
            }
            else if (cooridint == 31)
            {
                row = 8;
                col = 16;
            }
            else if (cooridint == 32)
            {
                row = 10;
                col = 2;
            }
            else if (cooridint == 33)
            {
                row = 10;
                col = 4;
            }
            else if (cooridint == 34)
            {
                row = 10;
                col = 6;
            }
            else if (cooridint == 35)
            {
                row = 10;
                col = 8;
            }
            else if (cooridint == 36)
            {
                row = 10;
                col = 10;
            }
            else if (cooridint == 37)
            {
                row = 10;
                col = 12;
            }
            else if (cooridint == 38)
            {
                row = 10;
                col = 14;
            }
            else if (cooridint == 39)
            {
                row = 10;
                col = 16;
            }
            else if (cooridint == 40)
            {
                row = 12;
                col = 2;
            }
            else if (cooridint == 41)
            {
                row = 12;
                col = 4;
            }
            else if (cooridint == 42)
            {
                row = 12;
                col = 6;
            }
            else if (cooridint == 43)
            {
                row = 12;
                col = 8;
            }
            else if (cooridint == 44)
            {
                row = 12;
                col = 10;
            }
            else if (cooridint == 45)
            {
                row = 12;
                col = 12;
            }
            else if (cooridint == 46)
            {
                row = 12;
                col = 14;
            }
            else if (cooridint == 47)
            {
                row = 12;
                col = 16;
            }
            else if (cooridint == 48)
            {
                row = 14;
                col = 2;
            }
            else if (cooridint == 49)
            {
                row = 14;
                col = 4;
            }
            else if (cooridint == 50)
            {
                row = 14;
                col = 6;
            }
            else if (cooridint == 51)
            {
                row = 14;
                col = 8;
            }
            else if (cooridint == 52)
            {
                row = 14;
                col = 10;
            }
            else if (cooridint == 53)
            {
                row = 14;
                col = 12;
            }
            else if (cooridint == 54)
            {
                row = 14;
                col = 14;
            }
            else if (cooridint == 55)
            {
                row = 14;
                col = 16;
            }
            else if (cooridint == 56)
            {
                row = 16;
                col = 2;
            }
            else if (cooridint == 57)
            {
                row = 16;
                col = 4;
            }
            else if (cooridint == 58)
            {
                row = 16;
                col = 6;
            }
            else if (cooridint == 59)
            {
                row = 16;
                col = 8;
            }
            else if (cooridint == 60)
            {
                row = 16;
                col = 10;
            }
            else if (cooridint == 61)
            {
                row = 16;
                col = 12;
            }
            else if (cooridint == 62)
            {
                row = 16;
                col = 14;
            }
            else if (cooridint == 63)
            {
                row = 16;
                col = 16;
            }

        }
    }

    class Hint : Game {
        public void demo(){

        }
    }
}