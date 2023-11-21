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
        fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R b Kkq - 1 1";
        //BoardManager bm = new BoardManager(fen);

        Board board2 = Board.CreateBoardFromFEN(fen);
        Console.WriteLine(PerftMinimax(board2, 4));
        //return ChessChallenge.API.Move.NullMove;
        return ChessChallenge.API.Move.NullMove;
        BoardManager bm = new BoardManager(fen);
        int v = 1, c1, c2;
        bm.saveFens = false;
        do {
            c1 = bm.MinimaxRoot(v);
            c2 = PerftMinimax(board2, v);
            v++;
        } while (c1 == c2 && v < 5);

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
        int tL = moveList.Length;

        for (int i = 0; i < tL; i++)
        {
            //Console.WriteLine(m);
            ChessChallenge.API.Move m = moveList[i];
            pb.MakeMove(m);
            tC += PerftMinimax(pb, pDepth - 1);
            pb.UndoMove(m);
        }

        if (pDepth == 3) Console.WriteLine(pb.GetFenString() + "\n" + tC); 

        return tC;
    }
}

/*
 * r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R2K3R b kq - 1 1
3559113 CORRECT
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R4K1R b kq - 1 1
3377351 CORRECT
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R4RK1 b kq - 1 1
4119629 CORRECT
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/2KR3R b kq - 1 1
3551583 CORRECT
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R b Kkq - 1 1
3827454 FAILURE
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/2R1K2R b Kkq - 1 1
3814203
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/3RK2R b Kkq - 1 1
3568344
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3KR2 b Qkq - 1 1
3685756
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K1R1 b Qkq - 1 1
3989454
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2NQ3p/PPPBBPPP/R3K2R b KQkq - 1 1
3949570
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N1Q2p/PPPBBPPP/R3K2R b KQkq - 1 1
4477772
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N3Qp/PPPBBPPP/R3K2R b KQkq - 1 1
4669768
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N4Q/PPPBBPPP/R3K2R b KQkq - 0 1
5067173
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2PQ2/2N4p/PPPBBPPP/R3K2R b KQkq - 1 1
4327936
r3k2r/p1ppqpb1/bn2pnp1/3PNQ2/1p2P3/2N4p/PPPBBPPP/R3K2R b KQkq - 1 1
5271134
r3k2r/p1ppqpb1/bn2pQp1/3PN3/1p2P3/2N4p/PPPBBPPP/R3K2R b KQkq - 0 1
3975992
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPP1BPPP/R1B1K2R b KQkq - 1 1
3793390
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N1BQ1p/PPP1BPPP/R3K2R b KQkq - 1 1
4407041
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2PB2/2N2Q1p/PPP1BPPP/R3K2R b KQkq - 1 1
3941257
r3k2r/p1ppqpb1/bn2pnp1/3PN1B1/1p2P3/2N2Q1p/PPP1BPPP/R3K2R b KQkq - 1 1
4370915
r3k2r/p1ppqpb1/bn2pnpB/3PN3/1p2P3/2N2Q1p/PPP1BPPP/R3K2R b KQkq - 1 1
3967365
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPB1PPP/R2BK2R b KQkq - 1 1
3074219
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPB1PPP/R3KB1R b KQkq - 1 1
4095479
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2NB1Q1p/PPPB1PPP/R3K2R b KQkq - 1 1
4066966
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1pB1P3/2N2Q1p/PPPB1PPP/R3K2R b KQkq - 1 1
4182989
r3k2r/p1ppqpb1/bn2pnp1/1B1PN3/1p2P3/2N2Q1p/PPPB1PPP/R3K2R b KQkq - 1 1
4032348
r3k2r/p1ppqpb1/Bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPB1PPP/R3K2R b KQkq - 0 1
3553501
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P1Q1/2N4p/PPPBBPPP/R3K2R b KQkq - 1 1
4514010
r3k2r/p1ppqpb1/bn2pnp1/3PN2Q/1p2P3/2N4p/PPPBBPPP/R3K2R b KQkq - 1 1
4743335
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/5Q1p/PPPBBPPP/RN2K2R b KQkq - 1 1
3996171
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/5Q1p/PPPBBPPP/R2NK2R b KQkq - 1 1
3995761
r3k2r/p1ppqpb1/bn2pnp1/3PN3/Np2P3/5Q1p/PPPBBPPP/R3K2R b KQkq - 1 1
4628497
r3k2r/p1ppqpb1/bn2pnp1/1N1PN3/1p2P3/5Q1p/PPPBBPPP/R3K2R b KQkq - 1 1
4317482
r3k2r/p1ppqpb1/bn2pnp1/3P4/1p2P3/2NN1Q1p/PPPBBPPP/R3K2R b KQkq - 1 1
3288812
r3k2r/p1ppqpb1/bn2pnp1/3P4/1pN1P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 1 1
3494887
r3k2r/p1ppqpb1/bn2pnp1/3P4/1p2P1N1/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 1 1
3415992
r3k2r/p1ppqpb1/bnN1pnp1/3P4/1p2P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 1 1
4083458
r3k2r/p1ppqpb1/bn2pnN1/3P4/1p2P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 0 1
3949417
r3k2r/p1pNqpb1/bn2pnp1/3P4/1p2P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 0 1
4404043
r3k2r/p1ppqNb1/bn2pnp1/3P4/1p2P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 0 1
4164923
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/P1N2Q1p/1PPBBPPP/R3K2R b KQkq - 0 1
4627439
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/1PN2Q1p/P1PBBPPP/R3K2R b KQkq - 0 1
3768824
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2QPp/PPPBBP1P/R3K2R b KQkq - 0 1
3472039
r3k2r/p1ppqpb1/bn1Ppnp1/4N3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 0 1
3835265
r3k2r/p1ppqpb1/bn2pnp1/3PN3/Pp2P3/2N2Q1p/1PPBBPPP/R3K2R b KQkq a3 0 1
4387586
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P1P1/2N2Q1p/PPPBBP1P/R3K2R b KQkq g3 0 1
3338154
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1P/PPPBBP1P/R3K2R b KQkq - 0 1
3819456
r3k2r/p1ppqpb1/bn2Pnp1/4N3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 0 1
4727437
 * 
 * 
 */


/*r2k3r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w K - 2 2
84920
r4k1r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w K - 2 2
83189
r4rk1/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w K - 2 2
84444
2kr3r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w K - 2 2
87068
1r2k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kk - 2 2
92948
2r1k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kk - 2 2
86531
3rk2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kk - 2 2
86620
r3k3/p1ppqpb1/bn2pnp1/3PN3/1p2P2r/2N2Q1p/PPPBBPPP/1R2K2R w Kq - 2 2
91599
r3k3/p1ppqpb1/bn2pnp1/3PN2r/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kq - 2 2
90283
r3k3/p1ppqpb1/bn2pnpr/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kq - 2 2
84150
r3k3/p1ppqpbr/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kq - 2 2
84232
r3kr2/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kq - 2 2
75881
r3k1r1/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kq - 2 2
80075
r3k2r/p1ppqpb1/1n2pnp1/3PN3/1p2P3/2N2Q1p/PPPBbPPP/1R2K2R w Kkq - 0 2
74818
r3k2r/p1ppqpb1/1n2pnp1/3PN3/1p2P3/2Nb1Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
86863
r3k2r/p1ppqpb1/1n2pnp1/3PN3/1pb1P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
87817
r3k2r/p1ppqpb1/1n2pnp1/1b1PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
91311
r3k2r/pbppqpb1/1n2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
92224
r1b1k2r/p1ppqpb1/1n2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
79352
r3k2r/p1pp1pb1/bn2pnp1/2qPN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
104208
r3k2r/p1pp1pb1/bn1qpnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
92015
r2qk2r/p1pp1pb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
84275
r3kq1r/p1pp1pb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
83974
r3k2r/p1ppqp2/bn2pnpb/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
90002
r3kb1r/p1ppqp2/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
82235
r3k2r/p1ppqpb1/b3pnp1/3PN3/np2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
86795
r3k2r/p1ppqpb1/b3pnp1/3PN3/1pn1P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
85466
r3k2r/p1ppqpb1/b3pnp1/3nN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
85713
r1n1k2r/p1ppqpb1/b3pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
78031
r3k2r/p1ppqpb1/bn2p1p1/3PN3/1p2n3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
119483
r3k2r/p1ppqpb1/bn2p1p1/3PN3/1p2P1n1/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
97660
r3k2r/p1ppqpb1/bn2p1p1/3nN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
97648
r3k2r/p1ppqpb1/bn2p1p1/3PN2n/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
95779
r3k2r/p1ppqpbn/bn2p1p1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
91837
r3k1nr/p1ppqpb1/bn2p1p1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 2 2
91970
r3k2r/p1ppqpb1/bn2pnp1/3PN3/4P3/1pN2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
98333
r3k2r/p1ppqpb1/bn2pn2/3PN1p1/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
86635
r3k2r/p2pqpb1/bnp1pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
94025
r3k2r/p1p1qpb1/bn1ppnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
87007
r3k2r/p2pqpb1/bn2pnp1/2pPN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq c6 0 2
87981
r3k2r/p1ppqpb1/bn2pnp1/3PN3/4P3/2p2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
95037
r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q2/PPPBBPpP/1R2K2R w Kkq - 0 2
94098
r3k2r/p1ppqpb1/bn3np1/3pN3/1p2P3/2N2Q1p/PPPBBPPP/1R2K2R w Kkq - 0 2
92922
 */