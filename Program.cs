using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;



namespace JOGODOGALO.V2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Jogador> highscore = new List<Jogador>();
            int opc = 0;


            do
            {
                Console.Clear();
                opc = validacaoMenu(); //Seleciona opção de menú com setas do teclado
                //opc = validacaoInput(textoMenu, opc, 1, 4); **Alternativa de menú inserindo número de alinha
                                
                string nome;
                if (opc == 1) // ************* Menu Jogar***************
                {
                    Console.Clear();

                    int jogadorVS = 0;
                    jogadorVS = validacaoInput(menuJogar, jogadorVS, 1, 2);

                    if (jogadorVS == 1) // ********* JOGADOR VS JOGADOR ***********
                    {
                        Console.Clear();

                        Console.Write("\n\tQual o nome do Jogador X? -> ");
                        string jogadorX = Console.ReadLine();
                        Console.Write("\tQual o nome do Jogador O? -> ");
                        string jogadorO = Console.ReadLine();

                        Jogador jogador1 = new Jogador(jogadorX, " X ", "");
                        Jogador jogador2 = new Jogador(jogadorO, " O ", "");

                        int dificuldade = 0;
                        dificuldade = validacaoInput(dificuldadeMenu, dificuldade, 1, 4);
                        
                        int tamanho = 0;
                        switch (dificuldade) 
                        {
                            case 1:
                                tamanho = 3;
                                break;
                            case 2:
                                tamanho = 5;
                                break;
                            case 3:
                                tamanho = 7;
                                break;
                            case 4:

                                tamanho = validacaoDificuldade(tamanho, 7, 25);                               
                                break;
                        }

                        Board board = new Board();
                        board.novaBoard(tamanho);

                        int resultado = board.jogoJogadorJogador(jogador1, jogador2, board, tamanho, highscore);
                                           
                        switch (resultado)
                        {
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\t  ** {jogador1.Nome} venceu! **");
                                Console.ResetColor();
                                Console.ReadKey();
                                break;
                            case 2:                               
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\t  ** {jogador2.Nome} venceu! **");
                                Console.ResetColor();
                                Console.ReadKey();
                                break;
                            case 3:
                                Console.WriteLine("\tEmpate! :(");
                                Console.ReadKey();
                                break;
                        }

                    }
                    else if (jogadorVS == 2) // ********* JOGADOR VS COMPUTADOR 
                    {
                        Console.Clear();
                        int tamanho = 0;

                        Console.Write("\n\tQual o nome do Jogador X? -> ");
                        string jogadorX = Console.ReadLine();                        
                        string jogadorO = "Computador";

                        Jogador jogador1 = new Jogador(jogadorX, " X ", "");
                        Jogador jogador2 = new Jogador(jogadorO, " O ", "");

                        int dificuldade = 0;

                        dificuldade = validacaoInput(dificuldadeMenu, dificuldade, 1, 4);


                        switch (dificuldade)
                        {
                            case 1:
                                tamanho = 3;
                                break;
                            case 2:
                                tamanho = 5;
                                break;
                            case 3:
                                tamanho = 7;
                                break;
                            case 4:
                                tamanho = validacaoDificuldade(tamanho, 7, 25);
                                break;
                        }

                        Board board = new Board();
                        board.novaBoard(tamanho);

                        int resultado = board.jogoJogadorComputador(jogador1, jogador2, board, tamanho, highscore);

                        switch (resultado)
                        {
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\t  ** {jogador1.Nome} venceu! **");
                                Console.ResetColor();
                                Console.ReadKey();
                                break;
                            case 2:                             
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\t  ** {jogador2.Nome} venceu! **");
                                Console.ResetColor();
                                Console.ReadKey();
                                break;
                            case 3:
                                Console.WriteLine("Empate!");
                                Console.ReadKey();
                                break;
                        }
                    }
                } else if (opc == 2)  // ************* HIGHSCORES ***************
                {
                    Console.Clear();
                    int lugar = 1;
                    int opc_high = 0;

                    opc_high = validacaoInput(menuScores, opc_high, 1, 2);
                    
                    switch (opc_high)
                    {
                        case 1:
                            Console.WriteLine("\n\t************** HIGHSCORES **************");
                            Console.WriteLine("\t    Jogador  -  Simbolo  -  Tempo \n");

                            if (highscore.Count() > 0)
                            {
                                foreach (Jogador jogador in highscore.OrderBy(x => x.Tempo).Take(10))
                                {
                                    Console.WriteLine($"\t{lugar}.\t{jogador.Nome} - {jogador.Simbolo} - {jogador.Tempo}");
                                    lugar++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n\t          Não existem scores");
                            }
                            Console.ReadKey();
                            continue;

                        case 2:
                            Console.WriteLine("\n\t************** HIGHSCORES **************");
                            Console.WriteLine("\t          Jogador  -  Vitorias\n");


                            if (highscore.Count() > 0)
                            {
                                var jogadores = highscore
                                     .GroupBy(jogador => jogador.Nome)
                                     .Select(group => new { Nome = group.Key, Count = group.Count() })
                                     .OrderByDescending(x => x.Count).Take(10);
                                int cont = 1;

                                foreach (var jogador in jogadores)
                                {
                                    Console.WriteLine($"\t{cont}.        {jogador.Nome} - {jogador.Count}");
                                    cont++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n\t          Não existem scores");
                            }
                            Console.ReadKey();
                            continue;
                    }
                    Console.ReadKey();
                    break;
                } else if (opc == 3) // ************* IMPORTAR FICHEIRO DE HIGHSCORES ***************
                {
                    string dir = Diretorio(@"c:\ficheiros\");
                    //****************
                    StreamReader leitura = new StreamReader($@"c:\ficheiros\{dir}");
                    string linha;

                    while ((linha = leitura.ReadLine()) != null)
                    {
                        string[] info = linha.Split('/');

                        if (info.Length == 3)
                        {
                            nome = info[0].Trim();
                            string simbolo = info[1].Trim();
                            string tempo = info[2].Trim();
                            
                            
                            Jogador jogador = new Jogador(nome, simbolo, tempo);
                            highscore.Add(jogador);
                        }
                    }
                    leitura.Close();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n\t*Highscores importados de {dir}*");
                    Console.ResetColor();
                    Console.ReadKey();
                } else
                {
                    Console.WriteLine("\n\tSeleção inválida");
                }

            } while (opc != 4);

        }


        public static string Diretorio(string diretorio_aux)
        {
            string diretorio = diretorio_aux;
            string[] ficheiros;
 

            while (true)
            {
                try
                {
                    if (Directory.Exists(diretorio))
                    {
                        string[] files;
                        int select = 0;
                        string directory = diretorio_aux; 
                        ConsoleKeyInfo key;
                        Console.WriteLine();

                        do
                        {
                            files = Directory.GetFiles(directory);

                            for (int i = 0; i < files.Length; i++)
                            {
                                if (select == i)
                                {
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"\t{Path.GetFileName(files[i])}");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine($"\t{Path.GetFileName(files[i])}");
                                }
                            }

                            key = Console.ReadKey(true); 

                            switch (key.Key)
                            {
                                case ConsoleKey.UpArrow:
                                    if (select > 0)
                                        select--;
                                    break;
                                case ConsoleKey.DownArrow:
                                    if (select < files.Length - 1)
                                        select++;
                                    break;
                                case ConsoleKey.Enter:
                                                                      
                                    return Path.GetFileName(files[select]);
                           
                                case ConsoleKey.Escape:

                                    break;
                            }

                            Console.Clear();
                            textoMenu();
                            Console.WriteLine("\n");
                            
                        } while (true);



                    }
                    else
                    {
                        Console.WriteLine("Diretório não existe...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }

            }
           
        }

        // =============================================================== VALIDAÇÃO INPUT

        public static int validacaoMenu()
        {
            int select = 0;
            string[] menu = { "\n\t1.         Jogar", "\t2.       HighScores", "\t3.   Importar highScores.txt", "\t4.          Sair\n" };

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n\t*****************************");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t********JOGO DO GALO*********");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t************MENU*************");
                Console.ResetColor();

                for (int i = 0; i < menu.Length; i++)
                {
                    if (i == select)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(menu[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(menu[i]);
                    }
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (select > 0)
                        {
                            select--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (select < menu.Length - 1)
                        {
                            select++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        return select+1;
                }
            }
        }


        public static int validacaoInput(Action texto, int input, int min, int max)
        {
            while (true)
            {
                texto();
                try
                {
                    input = int.Parse(Console.ReadLine());
                    if (input >= min && input <= max)
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("\n\tSeleção inválida... Tente denovo.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n\tSeleção inválida... Selecione um número.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        


        public static int validacaoDificuldade(int input, int min, int max)
        {
            while (true)
            {
                try
                {
                    Console.Write("\n\tQual o tamanho da tabela? - >");
                    int tamanho_aux = int.Parse(Console.ReadLine());
                    if (tamanho_aux % 2 != 0 && tamanho_aux >= min && tamanho_aux <= max)
                    {
                        input = tamanho_aux;
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("\n\tNúmero fora da faixa ou par");
                        Console.ReadKey();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n\tInsira um número válido");
                    Console.ReadKey();
                }
            }
        } 

        
        
        // =============================================================== FUNÇÕES DE TEXTO
        // textoMenu(int opc_menu)
        public static void textoMenu()
        {

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\t*****************************");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t********JOGO DO GALO*********");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t************MENU*************");

            Console.ResetColor();
            Console.WriteLine("\n\t1.         Jogar");
            Console.WriteLine("\t2.       HighScores");
            Console.WriteLine("\t3.   Importar highScores.txt");
            Console.WriteLine("\t4.          Sair");
            //Console.Write("\n\tOpção - > ");
          

        }

        public static void menuJogar()
        {
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.WriteLine("\n\t************** JOGAR **************");
            Console.ResetColor();
            Console.WriteLine("\n\t1.   Jogador Vs Jogador");
            Console.WriteLine("\n\t2.   Jogador Vs Computador");
            Console.Write("\n\tOpção - > ");
        }

        public static void dificuldadeMenu()
        {
            Console.WriteLine("\n\t\tEscolha a dificuldade");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\t1. Fácil      (3x3)");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\t2. Moderada   (5x5)");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\t3. Avançada   (7x7)");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\t4. Lendária   (nxn)");
            Console.ResetColor();
            Console.Write("\n\tOpção - > ");
        }

        public static void menuScores()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\t************** HIGHSCORES **************");
            Console.ResetColor();
            Console.WriteLine("\n\t     Ordenar por...");
            Console.WriteLine("\n\t1. Vitória mais rápida");
            Console.WriteLine("\n\t2. Mais vitórias");
            Console.Write("\n\tOpção - > ");
        }
    }

     
}
