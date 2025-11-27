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

        private List<Carta> MontedoJogador;

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


        //Método para retirar a carta da vez do monte de compra


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
        List<Carta> cartasparaComprar;

        int QuantidadedeCarta;

        public MontedeCompra(List<Carta> cartasparaComprar)
        {
            cartasparaComprar = null;
        }

        public void InserirMontedeCompra(Carta cartaAdicionar)
        {

        }

        //Método para inserir carta no monte

    }


    public class AreadeDescarte
    {
        List<Carta> cartas;

        public AreadeDescarte()
        {
            cartas = null;
        }

        public void InserirAreaDescarte()
        {

        }

        public void ImprimirAreadeDescarte()
        {

        }


        //Inicializa vazia


        //Imprimir área de descarte para o jogador

    }



    public class Arquivo
    {


        //Gerado ao fim, não é impresso durante o jogo


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

            for (int i = 0; i <= quantCartas; i++)
            {

            }

            for (int i = 0; i <= quantJogadores; i++)
            {
                Console.WriteLine("Nome do jogador " + i + 1);
                string nomeJogador = Console.ReadLine()

                Jogador jogadorNovo = new Jogador(nomeJogador)
            }

            do
            {
                

            }while() //Continuar até a área de descarte e o monte de compra estiver vazio
        }
    }
}
