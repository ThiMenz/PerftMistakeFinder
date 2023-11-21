using System;
using System.Collections.Generic;
using ChessChallenge.API;
using ChessBot;
public class MyBot : IChessBot
{
    public MyBot() {
        ChessBot.BOT_MAIN.MiluvaBotStart();
    }
    public ChessChallenge.API.Move Think(Board board, ChessChallenge.API.Timer timer) 
    {
        string fen = board.GetFenString();
        fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
        BoardManager bm = new BoardManager(fen);

        Board board2 = Board.CreateBoardFromFEN(fen);
        //Console.WriteLine(board2.GetLegalMoves().Length);
        //return ChessChallenge.API.Move.NullMove;

        int v = 1, c1, c2;
        bm.saveFens = false;
        do {
            c1 = bm.MinimaxRoot(v);
            c2 = PerftMinimax(board2, v);
            v++;
        } while (c1 == c2 && v < 7);

        bm.saveFens = true;
        bm.MinimaxRoot(1);
        FindPerftError(v - 1, bm, bm.possibleFens);


        //Console.WriteLine(c1);
        //Console.WriteLine(c2);
        //Console.WriteLine(v - 1);

        return ChessChallenge.API.Move.NullMove;
    }

    public void FindPerftError(int v, BoardManager bm, List<string> fens)
    {
        Console.WriteLine("- - - " + v + " - - - ");
        bm.saveFens = false;
        string tS = "";
        foreach (string s in bm.possibleFens)
        {
            Console.WriteLine(s);
            Board tb = Board.CreateBoardFromFEN(s);
            bm.LoadFenString(s);

            if (PerftMinimax(tb, v) != bm.MinimaxRoot(v))
            {
                //Console.WriteLine(s);
                tS = s;
                continue;
            }
        }

        if (v == 1)
        {
            Console.WriteLine(tS);
            return;
        }

        bm.saveFens = true;
        bm.LoadFenString(tS);
        bm.MinimaxRoot(1);
        FindPerftError(v - 1, bm, bm.possibleFens);
    }

    public int PerftMinimax(Board pb, int pDepth)
    {
        if (pDepth == 0) return 1;
        int tC = 0;
        ChessChallenge.API.Move[] moveList = pb.GetLegalMoves();
        foreach (ChessChallenge.API.Move m in moveList)
        {
            pb.MakeMove(m);
            tC += PerftMinimax(pb, pDepth - 1);
            pb.UndoMove(m);
        }
        return tC;
    }
}