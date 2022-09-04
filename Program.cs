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
            // Console.OutputEncoding = System.Text.Encoding.Unicode;
            Game game = new Game();
            Board board = new Board();
            History history = new History();
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
            game.startGame(game, board, history, player_list);
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
        public bool startGame( Game game, Board board, History history, List<Player> player_list){
            //gm: current game mode
            //gt: current game type

            board.initialChessBoard(game, history);
            bool res = false;
            //assume that player1 always be the first player
            player_list[0].reversi_rowinput(1);
            
            if ( game.CurGameType == (GameType.tictactoe).ToString() ){
                while(game.num_ChessMove < NUM_MAXMOVE[(int)GameType.tictactoe] ){
                    
                    int num_CurPlayrer = this.getPlayerNum(game.num_ChessMove);
                    int row = player_list[num_CurPlayrer].tic_rowinput(num_CurPlayrer);
                    int col = player_list[num_CurPlayrer].tic_colinput(num_CurPlayrer);
                    int num_curMove = board.convertCoorSysToOne(row, col, game.CurGameType);
                    // check whether this move is valid
                    while (!history.checkAvailable(num_curMove)){
                        int[] arr_tmp = board.convertCoorSysToTwo(num_curMove, game.CurGameType);
                        WriteLine($"This position ({arr_tmp[0]},{arr_tmp[1]}) has been placed a chess!!");
                        WriteLine("Please re-enter a valid position!!");
                        row = player_list[num_CurPlayrer].tic_rowinput(num_CurPlayrer);
                        col = player_list[num_CurPlayrer].tic_colinput(num_CurPlayrer);
                        num_curMove = board.convertCoorSysToOne(row, col, game.CurGameType);
                    }
                    history.recordHistory(num_curMove, num_CurPlayrer);
                    board.transferrowcoltobox(
                        num_curMove,
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
                    int num_CurPlayrer = this.getPlayerNum(game.num_ChessMove);
                    int row = player_list[num_CurPlayrer].reversi_rowinput(num_CurPlayrer);
                    int col = player_list[num_CurPlayrer].reversi_colinput(num_CurPlayrer);
                    int num_curMove = board.convertCoorSysToOne(row, col, game.CurGameType);

                    // check whether this move is valid
                    while (!history.checkAvailable(num_curMove)){
                        int[] arr_tmp = board.convertCoorSysToTwo(num_curMove, game.CurGameType);
                        WriteLine($"This position ({arr_tmp[0]},{arr_tmp[1]}) has been placed a chess!!");
                        WriteLine("Please re-enter a valid position!!");
                        row = player_list[num_CurPlayrer].reversi_rowinput(num_CurPlayrer);
                        col = player_list[num_CurPlayrer].reversi_colinput(num_CurPlayrer);
                        num_curMove = board.convertCoorSysToOne(row, col, game.CurGameType);
                    }
                    history.recordHistory(num_curMove, num_CurPlayrer);
                    board.transferrowcoltobox(
                        num_curMove,
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

        // Convert two dimension coordinate to one dimension
        public int convertCoorSysToOne(int row, int col, string gt){
            int num_colCount = gt == (GameType.tictactoe).ToString()? 3 : 8 ;
            return ((row/2)-1)*num_colCount + ((col/2)-1);
        }
        // Convert one dimension coordinate to two dimension
        public int[] convertCoorSysToTwo(int coor, string gt){
            int num_colCount = gt == (GameType.tictactoe).ToString()? 3 : 8 ;
            int[] arr_coor = new int[2] { (coor/num_colCount) + 1, (coor%num_colCount) + 1};
            return arr_coor;
        }

        // display the current state of board
        public int transferrowcoltobox(int coor, int status, string gt)
        {
            WriteLine($"[Transferrowcoltobox]:{status}--{this.CurGameType}");
            //Tic Tac Toe
            if (gt == (GameType.tictactoe).ToString()){
                tic_c[coor] = status == 2 ? "\u202FO\u202F" : status == 1 ? "\u202FX\u202F" : "\u202F\u202F\u202F";
            }else{
                reversi_c[coor] = status == 2 ? "\u202F\u25CB\u202F" : status == 1 ? "\u202F\u25CF\u202F" : "\u202F\u202F\u202F";
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
        public void initialChessBoard(Game game, History history){
            if (game.CurGameType == (GameType.tictactoe).ToString()){
                for (int i = 0 ; i < 9 ; i ++)
                    tic_c[i] = STR_SPACE;
            }else{
                for (int i = 0 ; i < 64 ; i ++)
                    reversi_c[i] = STR_SPACE;
                
                int[] arr_defalutCoor = new int[] {27, 28, 35, 36};
                // // set up default rule
                // for (int i = 0; i < arr_defalutCoor.Length; i ++){
                //     reversi_c[arr_defalutCoor[i]] = "\u202F\u25CB\u202F";
                // }
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
        // each move will be recorded by three digit code
        // first digit: player number
        // last two digit: one dimension position
        //Save all moves
        protected Stack<int> forwardTrack = new Stack<int>();
        //Save the moves which are undo by user
        protected Queue<int> backTrack = new Queue<int>();

        public bool checkAvailable(int coor){
            // check whether that position is empty
            // true: available
            // false: there already exist a chess at this position
            WriteLine($"[checkAvailable]: coor:{forwardTrack.Contains(coor)}");
            bool res = true;
            foreach ( int item in forwardTrack ){
                if (item%100 == coor){
                    res = false;
                    return res;
                }
            }
            return res;
        }

        public void recordHistory(int coor, int num_player){
            forwardTrack.Push(coor+100*num_player);
            WriteLine($"[recordHistory]: track:{forwardTrack.Count()}");
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

            bool rowresult = Int32.TryParse(rowinput, out tic_rowcoordiantes);
            tic_rowcoordiantes *= 2; 
            int i = Array.BinarySearch(tic_row, tic_rowcoordiantes);


            while (rowresult == false || i < 0)
            {
                Console.Write("please enter a valid row coordiantes: ");
                rowresult = Int32.TryParse(ReadLine(), out tic_rowcoordiantes);
                tic_rowcoordiantes *= 2;
                i = Array.BinarySearch(tic_row, tic_rowcoordiantes);
            }

            return tic_rowcoordiantes;
        }
        override public int tic_colinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter col coordiantes: ");
            string colinput = Console.ReadLine();
            bool colresult = Int32.TryParse(colinput, out tic_colcoordiantes);
            tic_colcoordiantes *= 2;
            int j = Array.BinarySearch(tic_col, tic_colcoordiantes);

            while (colresult == false || j < 0)
            {
                Console.Write("please enter a valid column coordiantes: ");
                colresult = Int32.TryParse(ReadLine(), out tic_colcoordiantes);
                tic_colcoordiantes *= 2;
                j = Array.BinarySearch(tic_col, tic_colcoordiantes);
            }

            return tic_colcoordiantes;
        }

        override public int reversi_rowinput(int chessstatus)
        {
            Console.Write("Player" + chessstatus + " Enter row coordiantes: ");
            string rowinput = Console.ReadLine();
            bool rowresult = Int32.TryParse(rowinput, out reversi_rowcoordiantes);
            reversi_rowcoordiantes *= 2;
            int i = Array.BinarySearch(reversi_row, reversi_rowcoordiantes);


            while (rowresult == false || i < 0)
            {
                Console.Write("please enter a valid row coordiantes: ");
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
            bool colresult = Int32.TryParse(colinput, out reversi_colcoordiantes);
            reversi_colcoordiantes *= 2;
            int j = Array.BinarySearch(reversi_col, reversi_colcoordiantes);

            while (colresult == false || j < 0)
            {

                Console.Write("please enter a valid column coordiantes: ");
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