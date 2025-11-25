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

        private int quantCartas;

        private Queue<int> ranking; //Stream Reader

        public Jogador(string nome, int posicao, int quantCartas, Queue<int> ranking)
        {
            this.nome = nome;
            this.posicao = posicao;
            this.quantCartas = quantCartas;
            this.ranking = ranking;
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
    }

    public class Carta
    {
        //As cartas são distinguidas apenas pelo valor, não importa o naipe


        private int numero;

        private string naipe; //Só vão ter três: Dama (11), Valete (12) e Rei (13) [Só vão servir como meio visual]

        //private bool coringa;

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


    //Fazer uma classe para os montes?

    // Monte terá um objeto jogador como atributo

    // Coringa terá checagem via booleano?

    // if (numerotestado == numero || coringacheck == true)


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


        }
    }
}
