using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Trabalho_de_AED___Rouba_Monte
{
    public class Jogador
    {
        private string nome;

        private int posicao;

        private int quantCartas; //Da última partida

        private Queue<int> ranking; //Stream Reader

        private List<Carta> MonteDoJogador;

        public Jogador(string nome)
        {
            this.nome = nome;
            this.posicao = -1;
            this.quantCartas = 0;
            this.ranking = null;
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public int Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }

        public int QuantCartas
        {
            get { return quantCartas; }
            set { quantCartas = value; }
        }

        public Queue<int> Ranking
        {
            get { return ranking; }
            set { ranking = value; }
        }

        public void CartaDaVez(MontedeCompra MontedeCompraPartida, List<Jogador> Jogadores, string nomedoogadordavez, AreadeDescarte Mesa)
        {

            Carta CartadoMomento = MontedeCompraPartida.RemoverMontedeCarta(MontedeCompraPartida);
            bool FimdaCartadaVez = false;

            int montecomtopoigual = 0;
            foreach (Jogador x in Jogadores)
            {

                Carta Topo = Jogadores[x]
                if (x.Nome != nomedoogadordavez && CartadoMomento == x.MonteDoJogador[MonteDoJogador.Count - 1])
                {
                    montecomtopoigual++;
                }

                else if (montecomtopoigual > 1)
                {
                    if ()
                }


            }

            if (montecomtopoigual == 1)
            {

            }

            else if (montecomtopoigual > 1)
            {

            }

            else 
            { 

                foreach (Carta x in Mesa.Cartas)
                {
                    if (x == cartas)
                    {

                    }
                }

            if (CartadoMomento == MonteDoJogador[MonteDoJogador.Count - 1])
            {

            }

            Mesa.InserirAreaDescarte(CartadoMomento);

            }
        }

        //Método para retirar a carta da vez do monte de compra


    }

    public class Jogadores
    {
        private List<Jogador> participantes;

        public Jogadores(List<Jogador> participantes)
        {
            this.participantes = participantes;
            todos = null;
        }

        public List<Jogador> Participantes
        {
            get { return participantes; }
            set { participantes = value; }
        }

        public void AdicionarJogadores() { }

    }


    public class Carta
    {
        //As cartas são distinguidas apenas pelo valor, não importa o naipe


        private int numero;

        private string naipe; //Só vão ter três: Dama (11), Valete (12) e Rei (13) [Só vão servir como meio visual]


        public Carta(int numero, string naipe)
        {
            this.numero = numero;
            this.naipe = naipe;
        }


        public int Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        public string Naipe
        {
            get { return naipe; }
            set { naipe = value; }
        }

       
    }





    public class MontedeCompra
    {
        private List<Carta> cartasparaComprar;

        private int quantidadeCarta;

        public MontedeCompra(List<Carta> cartasparaComprar, int quantidadeCarta)
        {
            cartasparaComprar = null;
            this.quantidadeCarta = quantidadeCarta;
        }

        public void PreencherMontedeCompras()
        {
            int j = 0;
            for (int i = 0; i <= quantidadeCarta; i++)
            {
                if (j > 13)
                {
                    j = 0
                }

                switch (j)

                {
                    case 1:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);

                        break;

                    case 2:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);

                        break;

                    case 3:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);


                        break;

                    case 4:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);


                        break;

                    case 5:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);


                        break;

                    case 6:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);


                        break;

                    case 7:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);


                        break;

                    case 8:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);


                        break;

                    case 9:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);

                        break;

                    case 10:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);

                        break;

                    case 11:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);

                        break;

                    case 12:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);

                        break;

                    case 13:

                        Carta CartaGerada = new Carta(j, null);

                        cartasparaComprar.Insert(CartaGerada);

                        break;

                    default:
                        throw new Exception("Número Inválido");

                }

                j++;
            }

        }

        public void EmbaralharMontedeCompra()
        {

            if (cartasparaComprar == null || quantidadeCarta < 0)
            {
                throw new Exception("Monte Vazio ou de Tamanho Inválido")
            }

            else
            {

            }
            //Usar ordenação para embaralhar
        }

        public Carta RemoverMontedeCarta()
        {
            Carta cartaRemovida = CartasparaComprar.Remove;
            return cartaRemovida;
        }


        //Método para inserir carta no monte

        public void EmbaralharMonteDeCompra(Carta cartaAdicionar)
        {

        }

        public List<Carta> CartasparaComprar
        {
            get { return cartasparaComprar; }
            set { cartasparaComprar = value; }
        }

        public int QuantidadedeCarta
        {
            get { return quantidadedeCarta; }
            set { quantidadedeCarta = value; }
        }
    }

    /*
    Jogadores Topo:

    Adrielly: |1| Vitor |3|
    

    ----------
    MESA:

    | 11 | |12 |

    ----------

    Luiz: |5|


    CW("Qual monte roubar?")
    
     */

    public class AreadeDescarte
    {
        List<Carta> cartas;

        public AreadeDescarte()
        {
            cartas = null;
        }

        public void InserirAreaDescarte(Carta x)
        {

        }

        public void ImprimirAreadeDescarte()
        {
            Console.WriteLine();

        }

        public List<Carta> Cartas
        {
            get { return cartas; }
            set { cartas = value; }
        }


        //Inicializa vazia


        //Imprimir área de descarte para o jogador

    }



    public class Arquivo
    {


        //Gerado ao fim, não é impresso durante o jogo
        //Somente as 5 últimas rodadas, ou seja, sera preciso reescrever


    }



    //Fazer uma classe para os montes?

    // Monte terá um objeto jogador como atributo




    // Jogador

    internal class Program
    {
        static void Main(string[] args)
        {

            //Ordenar pela quantidade de cartas para o ranking
            //Ordenação via Quicksort?
            /*void Quicksort(int[] array, int esq, int dir) {
              int i = esq, j = dir, pivo = array[(esq+dir)/2];
              while (i <= j) {
              while (array[i] < pivo)
              i++;
              while (array[j] > pivo)
                }
                j--;
              if (i <= j)
             { Trocar(i, j);
              }
            if (esq < j)
            Quicksort(array, esq, j);
            if (i < dir)
            Quicksort(array, i, dir);
            i++; j--; }
*/


            //Pilha para as cartas da área de descarte

            //Pergunta quantidade de Jogadores e Cartas

            bool continuarJogando, resetarJogadores, resetarQuantidadeCartas, visualizarrankings;

            do
            {
                Console.WriteLine("Quantas cartas serão geradas?");
                int quantCartas = int.Parse.(Console.ReadLine());

                while (quantCartas < 0)
                {
                    Console.WriteLine("A quantidade de cartas deve ser positiva");
                    int quantCartas = int.Parse.(Console.ReadLine());

                }

                Console.WriteLine("Quantos jogadores irão participar?");
                int quantJogadores = int.Parse(Console.ReadLine());

                while (quantJogadores < 0)
                {
                    Console.WriteLine("A quantidade de jogadores deve ser positiva");
                    int quantJogadores = int.Parse.(Console.ReadLine());

                }

                MontedeCompra MontedeCompras = new MontedeCompra();

                MontedeCompras.PreencherMontedeCompras();

                for (int i = 0; i <= quantJogadores; i++)
                {
                    Console.WriteLine("Nome do jogador " + i + 1);
                    string nomeJogador = Console.ReadLine()


                    Jogador jogadorNovo = new Jogador(nomeJogador) //COLOCAR NO WHATTSZAP COMO MUDAR O NÚMERO A CADA LOOP!!!!!!!!!!!
                 
                }

                do
                {


                } while (MontedeCompras.QuantidadedeCarta == 0); //Continuar até a área de descarte e o monte de compra estiver vazio

                Console.WriteLine("Quer Continuar Jogando?");
                string resp = Console.ReadLine();

                if (resp == "Sim" || resp == "sim" || resp == "S" || resp == "s")
                {
                    continuarJogando = true;
                }

                else if (resp == "Não" || resp == "não" || resp == "N" || resp == "n")
                {
                    continuarJogando = false;
                }
                else
                {
                    throw new Exception("Resposta Inválida");
                }

                if (continuarJogando)
                {
                    Console.WriteLine("Deseja Alterar a quantidade de Cartas?");

                    Console.WriteLine("Deseja gerar novos jogadores?");
                }


            } while (continuarJogando)


            Console.WriteLine("Obrigado por Jogar!");

            //Antes de jogar novamente, quer ver os rankings, de quem?

            //Quer alterar o número de cartas

            //Quer gerar novos jogadores?

            //Quer parar?
        }
    }
}
