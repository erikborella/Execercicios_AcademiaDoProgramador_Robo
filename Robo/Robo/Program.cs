using System;

namespace Robo
{
    class Program
    {
        static char[] direcoesPosiveis = { 'N', 'L', 'S', 'O' };
        static char[] comandoPosiveis = { 'E', 'D', 'M' };

        static void Main(string[] args)
        {


            int maxTamanhoX = 0;
            int maxTamanhoY = 0;

            LerTamanhosMaximos(ref maxTamanhoX, ref maxTamanhoY);

            while (true)
            {

                int posicaoX = 0;
                int posicaoY = 0;
                string direcao = "";

                LerPosicaoInicialDoRobo(ref posicaoX, ref posicaoY, maxTamanhoX, maxTamanhoY, ref direcao);

                char[] comandos = LerComandos();

                foreach (char comando in comandos)
                {
                    switch (comando)
                    {
                        case 'E':
                            ExecutarComandoE(ref direcao);
                            break;
                        case 'D':
                            ExecutarComandoD(ref direcao);
                            break;
                        case 'M':
                            ExecutarComandoM(ref posicaoX, ref posicaoY, maxTamanhoX, maxTamanhoY, direcao);
                            break;
                    }
                }

                Console.WriteLine("O robo parou em: ");
                Console.WriteLine($"{posicaoX} {posicaoY} {direcao}");
                PausarConsole();

                Console.Clear();
                Console.Write("Digite S para sair, ou qualquer outra coisa para continuar: ");

                if (ApertouParaSair())
                {
                    break;
                }
            }
        }

        private static bool ApertouParaSair()
        {
            return Console.ReadLine().Equals("S", StringComparison.OrdinalIgnoreCase);
        }

        private static void PausarConsole()
        {
            Console.Write("Digite qualquer coisa para continuar: ");
            Console.ReadLine();
        }

        private static void LerTamanhosMaximos(ref int maxTamahoX, ref int maxTamahoY)
        {
            bool temDoisNumeros = true;
            string[] tamanhosSeparados;

            do
            {
                Console.WriteLine("Digite o tamanho maximo do terreno: ");
                string tamanhoMaximos = Console.ReadLine();
                tamanhosSeparados = tamanhoMaximos.Split(' ');

                temDoisNumeros = tamanhosSeparados.Length == 2;

                if (!temDoisNumeros)
                {
                    MostrarMensagemErro("Voce precisa digitir dois numero separados por espaco!");
                }

                maxTamahoX = Convert.ToInt32(tamanhosSeparados[0]);
                maxTamahoY = Convert.ToInt32(tamanhosSeparados[1]);

                if (maxTamahoX == 0 || maxTamahoY == 0)
                {
                    MostrarMensagemErro("Nenhum dos valores podem ser 0");
                    PausarConsole();
                    continue;
                }
            } while (!temDoisNumeros);


        }

        private static void MostrarMensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }

        private static void LerPosicaoInicialDoRobo(ref int posicaoX, ref int posicaoY, int maxPosicaoX, int maxPosicaoY, ref string direcao)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Digite a posicao inicial do robo e para aonde ele está apontando: ");
                string posicoes = Console.ReadLine();
                string[] posicoesSeparadas = posicoes.Split(" ");

                if (posicoesSeparadas.Length != 3)
                {
                    MostrarMensagemErro("Voce tem que digitar 3 valores, 2 sao as posicoes inciais e o utlimo e a direcao");
                    PausarConsole();
                    continue;
                }

                posicaoX = Convert.ToInt32(posicoesSeparadas[0]);
                posicaoY = Convert.ToInt32(posicoesSeparadas[1]);

                if (!EhPosicaoValida(posicaoX, maxPosicaoX))
                {
                    MostrarMensagemErro("Voce digitou a posicao de X fora dos limites");
                    PausarConsole();
                    continue;
                }

                if (!EhPosicaoValida(posicaoY, maxPosicaoY))
                {
                    MostrarMensagemErro("Voce digitou a posicao de Y fora dos limites");
                    PausarConsole();
                    continue;
                } 

                bool direcaoEhValida = false;

                foreach (char direcaoPossivel in direcoesPosiveis)
                {
                    if (posicoesSeparadas[2] == direcaoPossivel.ToString())
                    {
                        direcaoEhValida = true;
                        break;
                    }
                }

                if (!direcaoEhValida)
                {
                    MostrarMensagemErro("Voce digitou uma direcao invalida, as possiveis são: 'N', 'L', 'S', 'O'");
                    PausarConsole();

                    continue;
                }

                direcao = posicoesSeparadas[2];
                break;
            }
        }

        private static char[] LerComandos()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Digite os comandos, todos juntos: ");

                char[] comandos = Console.ReadLine().ToCharArray();

                if (comandos.Length == 0)
                {
                    MostrarMensagemErro("Digite alguma coisa!");
                    PausarConsole();
                    continue;
                }

                bool todosOsComandosEstaoCertos = true;

                foreach(char comando in comandos)
                {
                    bool comandoEstaCerto = false;

                    foreach(char comandoPossivel in comandoPosiveis)
                    {
                        if (comandoPossivel == comando)
                        {
                            comandoEstaCerto = true;
                            break;
                        }
                    }

                    if (!comandoEstaCerto)
                    {
                        todosOsComandosEstaoCertos = false;
                        break;
                    }
                }

                if (!todosOsComandosEstaoCertos)
                {
                    MostrarMensagemErro("Voce digitou algum comando errado, tente novamente");
                    PausarConsole();
                    continue;
                }

                return comandos;
            }
        }

        private static int PegarPosicaoDaDirecao(string direcao)
        {
            switch(direcao)
            {
                case "N":
                    return 0;
                case "L":
                    return 1;
                case "S":
                    return 2;
                case "O":
                    return 3;

                default: 
                    return 0;
            }
        }

        /*
         * mod  2, 4 = 2
         * mod  4, 4 = 0
         * mod  5, 4 = 1
         * mod -1, 4 = 3
         */
        private static int Mod(int n1, int n2)
        {
            int r = n1 % n2;

            if (r < 0)
            {
                return r + n2;
            } 
            else
            {
                return r;
            }
        }
        private static void ExecutarComandoE(ref string direcao)
        {
            int posicaoDaDirecao = PegarPosicaoDaDirecao(direcao);
            int posicaoDaDirecaoEm90g = Mod(posicaoDaDirecao - 1, 4);

            direcao = direcoesPosiveis[posicaoDaDirecaoEm90g].ToString();
        }

        private static void ExecutarComandoD(ref string direcao)
        {
            int posicaoDaDirecao = PegarPosicaoDaDirecao(direcao);
            int posicaoDaDirecaoEm90g = Mod(posicaoDaDirecao + 1, 4);


            direcao = direcoesPosiveis[posicaoDaDirecaoEm90g].ToString();
        }

        private static bool EhPosicaoValida(int pos, int maxPos)
        {
            if (pos < 0 || pos > maxPos)
            {
                return false;
            }
            return true;
        }

        private static void ExecutarComandoM(ref int posicaoX, ref int posicaoY, int maxPosicaoX, int maxPosicaoY, string direcao)
        {
            switch (direcao)
            {
                case "N":
                    if (EhPosicaoValida(posicaoY + 1, maxPosicaoY))
                        posicaoY++;
                    break;
                    
                case "L":
                    if (EhPosicaoValida(posicaoX + 1, maxPosicaoX))
                        posicaoX++;
                    break;
                    
                case "S":
                    if (EhPosicaoValida(posicaoY - 1, maxPosicaoY))
                        posicaoY--;
                    break;
                    
                case "O":
                    if (EhPosicaoValida(posicaoX - 1, maxPosicaoX))
                        posicaoX--;
                    break;
            }
        }
    }
}
