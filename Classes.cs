using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOGODOGALO.V2
{
    internal class Board
    {
        static string[,] board;

        public void novaBoard(int tamanho)
        {
            board = new string[tamanho, tamanho];

            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    board[i, j] = " - ";
                }
            }
        }

        public void printBoard(int linha, int coluna)
        {
            int tamanho = board.GetLength(0);
            Console.WriteLine();
            for (int i = 0; i < tamanho; i++)
            {
                Console.Write("\t ");
                for (int j = 0; j < tamanho; j++)
                {
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    if (i == linha && j == coluna)
                    {                       
                        if (board[i, j] == " X " || board[i, j] == " O ")
                        {
                            Console.BackgroundColor = ConsoleColor.Red;

                        } else                    
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                        }
                        Console.Write(board[i, j]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(board[i, j]);
                    }
                    Console.ResetColor();
                }
                Console.WriteLine("\n");
            }
        }



        public bool validacao(string simbolo)
        {
            int tamanho = board.GetLength(0);


            for (int i = 0; i < tamanho; i++)
            {
                bool linha = true;
                bool coluna = true;
                for (int j = 0; j < tamanho; j++)
                {
                    if (board[i, j] != simbolo) // linhas
                        linha = false;
                    if (board[j, i] != simbolo) // colunas
                        coluna = false;
                }
                if (linha || coluna)
                    return true;
            }

            // Diagonais
            bool diagonal1 = true;
            bool diagonal2 = true;
            for (int i = 0; i < tamanho; i++)
            {
                if (board[i, i] != simbolo) // Verifica diagonal primária
                    diagonal1 = false;
                if (board[i, tamanho - 1 - i] != simbolo) // Verifica diagonal secundária
                    diagonal2 = false;
            }
            if (diagonal1 || diagonal2)
                return true;
            return false; // Se chegou aqui é inválida
        }

        public bool empate()
        {

            int tamanho = board.GetLength(0);

            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    if (board[i, j] == " - ")
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public bool jogada(Jogador jogador, int linha, int coluna)
        {
            int tamanho = board.GetLength(0);
            bool jogadaValida = false;
            Random random = new Random();

            do
            {
                if (linha >= 0 && linha < board.GetLength(0) && coluna >= 0 && coluna < board.GetLength(0))
                {
                    if (board[linha, coluna] == " - ")
                    {
                        board[linha, coluna] = jogador.Simbolo;
                        jogadaValida = true;
                        return true;
                    }
                    else
                    {

                        while (jogador.Nome == "Computador" && !jogadaValida)
                        {

                            int pc_linha = random.Next(0, tamanho);
                            int pc_coluna = random.Next(0, tamanho);
                            printBoard(pc_linha, pc_coluna);

                            if (board[pc_linha, pc_coluna] == " - ")
                            {
                                board[pc_linha, pc_coluna] = jogador.Simbolo;
                                jogadaValida = true;
                                return true;
                            }
                        }
                       

                        Console.WriteLine("\n\tJogada inválida.");
                        Console.ReadKey();
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("JOGADA INVÁLIDA. COORDENADA FORA DA BOARD. JOGUE NOVAMENTE.");

                    return false;
                }
            } while (!jogadaValida);
        }

        public void adiciona_score(Jogador jogador, List<Jogador> highscore, string score)
        {
            jogador.Tempo = score;
            highscore.Add(jogador);
        }



        public int jogoJogadorJogador(Jogador jogador1, Jogador jogador2, Board board, int tamanho,List <Jogador> highscore)
        {
            
            Stopwatch tempoResultado = new Stopwatch();
            Random random = new Random();
            tempoResultado.Start();

            int linha1 = 0, coluna1 = 0;
            int turn = 2;

            while (!board.validacao(" X ") && !board.validacao(" O ") || !board.empate())
            {
                Console.Clear();
                if (turn % 2 == 0)
                {
                    Console.WriteLine($"\n\tVez de {jogador1.Nome} com{jogador1.Simbolo}");
                } else
                {
                    Console.WriteLine($"\n\tVez de {jogador2.Nome} com{jogador2.Simbolo}");
                }
                
                board.printBoard(linha1, coluna1);


                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (coluna1 > 0)
                            coluna1--;
                        continue;
                    case ConsoleKey.RightArrow:
                        if (coluna1 < tamanho - 1)
                            coluna1++;
                        continue;
                    case ConsoleKey.UpArrow:
                        if (linha1 > 0)
                            linha1--;
                        continue;
                    case ConsoleKey.DownArrow:
                        if (linha1 < tamanho - 1)
                            linha1++;
                        continue;
                    case ConsoleKey.Enter:


                        if (turn % 2 == 0)
                        {
                            
                            if (board.jogada(jogador1, linha1, coluna1))
                            {
                                if (board.validacao(" X "))
                                {
                                    Console.Clear();
                                    board.printBoard(linha1, coluna1);
                                    tempoResultado.Stop();

                                    TimeSpan segundos = tempoResultado.Elapsed;
                                    string f_tempo = segundos.ToString(@"mm\:ss\s");
                                    jogador1.Tempo = f_tempo;

                                    board.adiciona_score(jogador1, highscore, f_tempo);
                                    using (StreamWriter escrita = new StreamWriter(@"c:\ficheiros\highscores.txt", true))
                                    {
                                        escrita.WriteLine($"{jogador1.Nome}/{jogador1.Simbolo}/{jogador1.Tempo}");
                                    }

                                    return 1;
                                }
                                if (board.empate())
                                {
                                    return 3;
                                }
                                turn++;
                            }
                            else
                            {
                                Console.WriteLine("\t\nCoordenada inválida.");
                            }


                        }
                        else
                        {
                            
                            if (board.jogada(jogador2, linha1, coluna1))
                            {

                                if (board.validacao(" O "))
                                {
                                    Console.Clear();
                                    board.printBoard(linha1, coluna1);
                                    tempoResultado.Stop();

                                    TimeSpan segundos = tempoResultado.Elapsed;
                                    string f_tempo = segundos.ToString(@"mm\:ss\s");
                                    jogador2.Tempo = f_tempo;

                                    board.adiciona_score(jogador2, highscore, f_tempo);
                                    using (StreamWriter escrita = new StreamWriter(@"c:\ficheiros\highscores.txt", true))
                                    {
                                        escrita.WriteLine($"{jogador2.Nome}/{jogador2.Simbolo}/{jogador2.Tempo}");
                                    }

                                    return 2;
                                }
                                if (board.empate())
                                {
                                    return 3;
                                }
                                turn++;
                            }
                            else
                            {
                                Console.WriteLine("\n\tCoordenada inválida.");
                            }
                         
                        }
                        continue;
                }
                return -1;
            }
            return 1;

        }

        public int jogoJogadorComputador(Jogador jogador1, Jogador jogador2, Board board, int tamanho, List<Jogador> highscore)
        {

            Stopwatch tempoResultado = new Stopwatch();
            Random random = new Random();
            tempoResultado.Start();

            int linha1 = 0, coluna1 = 0;
            int turn = 2;

            while (!board.validacao(" X ") && !board.validacao(" O ") || !board.empate())
            {
                Console.Clear();
                board.printBoard(linha1, coluna1);


                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (coluna1 > 0)
                            coluna1--;
                        continue;
                    case ConsoleKey.RightArrow:
                        if (coluna1 < tamanho - 1)
                            coluna1++;
                        continue;
                    case ConsoleKey.UpArrow:
                        if (linha1 > 0)
                            linha1--;
                        continue;
                    case ConsoleKey.DownArrow:
                        if (linha1 < tamanho - 1)
                            linha1++;
                        continue;
                    case ConsoleKey.Enter:


                        if (turn % 2 == 0)
                        {
                            if (board.jogada(jogador1, linha1, coluna1))
                            {
                                if (board.validacao(" X "))
                                {
                                    Console.Clear();
                                    board.printBoard(linha1, coluna1);
                                    tempoResultado.Stop();

                                    TimeSpan segundos = tempoResultado.Elapsed;
                                    string f_tempo = segundos.ToString(@"mm\:ss\s");
                                    jogador1.Tempo = f_tempo;

                                    board.adiciona_score(jogador1, highscore, f_tempo);
                                    using (StreamWriter escrita = new StreamWriter(@"c:\ficheiros\highscores.txt", true))
                                    {
                                        escrita.WriteLine($"{jogador1.Nome}/{jogador1.Simbolo}/{jogador1.Tempo}");
                                    }

                                    return 1;
                                }
                                // SE FALHOU NA VALIDAÇÃO MAS BOARD CHEIA == EMPATE
                                if (board.empate())
                                {
                                    return 3;
                                }
                                turn++;
                            }
                            else
                            {
                                Console.WriteLine("\t\nCoordenada inválida.");
                            }


                        }
                        else
                        {
                            int pc_linha = random.Next(0, tamanho);
                            int pc_coluna = random.Next(0, tamanho);
                            if (board.jogada(jogador2, pc_linha, pc_coluna))
                            {

                                if (board.validacao(" O "))
                                {
                                    Console.Clear();
                                    board.printBoard(pc_linha, pc_coluna);
                                    tempoResultado.Stop();

                                    return 2;
                                }

                                if (board.empate())
                                {
                                    return 3;
                                }
                                turn++;
                            }
                            else
                            {
                                Console.WriteLine("\t\nCoordenada inválida.");
                            }

                        }
                        continue;
                }
                return -1;
            }
            return 1;

        }
    }

    public class Jogador
    {
        public string Nome { get; set; }
        public string Simbolo { get; set; }

        public string Tempo { get; set; }

        public Jogador(string nome, string simbolo, string tempo)
        {
            Nome = nome;
            Simbolo = simbolo;
            Tempo = tempo;
        }
    }
    
}

