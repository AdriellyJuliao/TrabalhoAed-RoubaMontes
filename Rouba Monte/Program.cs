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

        private int quantCartasAgora; //Do jogo atual


        private Queue<int> ranking; //Stream Reader

        private List<Carta> MonteDoJogador;

        public Jogador(string nome)
        {
            this.nome = nome;
            this.posicao = -1;
            this.quantCartas = 0;
            this.quantCartasAgora = 0;
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

        public int QuantCartasAgora
        {
            get { return quantCartasAgora; }
            set { quantCartasAgora = value; }
        }

        public Queue<int> Ranking
        {
            get { return ranking; }
            set { ranking = value; }
        }

        public void CartaDaVez(MontedeCompra MontedeCompraPartida, List<Jogador> Jogadores, string nomedoogadordavez, AreadeDescarte Mesa)
        {

            Carta CartadoMomento = MontedeCompraPartida.RemoverMontedeCarta(MontedeCompraPartida); //Puxar carta do monte de compra
            bool FimdaCartadaVez = false; //Verifica se o turno dessa carta tá rolando

            int montecomtopoigual = 0; //Caso outros tenham o mesmo valor de carta no topo
            foreach (Jogador x in Jogadores)
            {

                if (x.Nome != nomedoogadordavez && CartadoMomento == x.MonteDoJogador[MonteDoJogador.Count - 1]) //Na regra se mais de um monte do tipo jogador for igual a carta da vez...
                {
                    montecomtopoigual++; //... Então tem que escolher aleatoriamente depois, precisa armazena a posição delas
                }




            }

            if (montecomtopoigual == 1) //Se for um rouba o monte do jogador
            {

            }

            else if (montecomtopoigual > 1) //Se tiver mais rouba aleatoriamente conforme a quantidade
            {

            }

            else //Caso nenhum jogador tenha o monte igual a carta da vez
            {

                foreach (Carta x in Mesa.Cartas) //Ver a área de descarte
                {
                    if (x == cartas)
                    {

                    }
                }

                if (CartadoMomento == MonteDoJogador[MonteDoJogador.Count - 1]) //Senão tiver então o jogador tenta usar no próprio monte
                {

                }

                Mesa.InserirAreaDescarte(CartadoMomento); //Se ainda não conseguiu, por fim, colocar na área de descarte

            }
        }


        /*REGRAS QUANTO A CARTA DA VEZ
        
        Os jogadores, dispostos em um círculo ao redor da mesa de jogo (--FILA IMPLICITA?--), jogam em sequência, em sentido horário. As jogadas 
        prosseguem da seguinte forma: 

        • O jogador que tem a vez de jogar retira a carta de cima do monte de compras e a mostra aos outros jogadores; vamos 
          chamar essa carta de carta da vez. 

        • Se a carta da vez for igual à carta do topo de um monte de um outro jogador, o jogador "rouba" esse monte, 
          colocando-o em seu próprio monte, coloca a carta da vez no topo do seu monte, face para cima, e continua a jogada 
          (ou seja, retira outra carta do monte de compras e repete o processo).  Duas cartas são consideradas iguais se tiverem 
           o mesmo valor. Caso a carta da vez seja igual ao topo de dois ou mais montes, deve-se roubar apenas o maior monte 
          (monte com mais cartas). Se houver empate em relação ao tamanho dos montes, deve-se escolher aleatoriamente 
          um dos montes para roubar.  

        • Se o teste acima falhar, o jogador verifica se a carta da vez é igual a alguma carta presente na área de descarte. Caso 
          seja, o jogador retira essa carta da área de descarte colocando-a no seu monte, juntamente com a carta da vez no 
          topo, com as faces voltadas para cima, e continua a jogada (ou seja, retira outra carta do monte de compras e repete o processo). 

        • Se o teste acima falhar, o jogador verifica se a carta da vez é igual a carta do topo de seu próprio monte. Caso seja, 
          o jogador coloca a carta da vez no topo de seu próprio monte, com a face para cima, e continua a jogada (ou seja, 
          retira outra carta do monte de compras e repete o processo). 

        • Se a carta da vez for diferente das cartas da área de descarte e das cartas nos topos dos montes, o jogador a coloca 
          na área de descarte, com a face para cima, e a jogada se encerra (ou seja, o próximo jogador efetua a sua jogada). 
          
          Note que esse é o único caso em que o jogador não continua a jogada.    

        */

        //Método para retirar a carta da vez do monte de compra


    }

    public class Jogadores  //Usar uma classe ou uma lista de jogadadores somente?
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
                if (j > 13) //Caso j seja maior que a quantidade de variações de cartas sera preenchido com novas cartas na mesma ordem
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
                        break;

                }

                j++; //Aumenta o valor j em relação ao número da carta
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
            cartas.Insert(x);
        }

        public void ImprimirAreadeDescarte()
        {
            foreach (Carta x in cartas)
            {
                Console.Write("||") //Simular o visual de uma carta?
                Console.Write(x.Numero "||" );
            }

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

            Queue <Jogador> FiladeJogadores;

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


                   FiladeJogadores.Enqueue(jogadorNovo);
                 
                }

                do 
                {





                    //Aqui onde vai rolar o jogo em si






                } while (MontedeCompras.QuantidadedeCarta == 0); //Continuar até a área de descarte e o monte de compra estiver vazio

                Console.WriteLine("Deseja ver os Rankings de algum jogador?");
                string resp1 = Console.ReadLine();

                Console.WriteLine("Quer Continuar Jogando?");
                string resp2 = Console.ReadLine();

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

                    Console.WriteLine("1) Continuar com a mesma Quantidade de Cartas e Jogadores");
                    Console.WriteLine("2) Continuar com a mesma Quantidade de Cartas e Alterar Jogadores");
                    Console.WriteLine("3) Alterar a quantidade de Cartas e manter os mesmos Jogadores");
                    Console.WriteLine("4) Alterar a quantidade de Cartas e Jogadores");
                    int opcoesContinuar = int.Parse(Console.ReadLine());

                    switch (opcoesContinuar)

                    {
                        case 1:

                            resetarQuantidadeCartas = false;

                            resetarJogadores = false;

                            break;

                        case 2:

                            resetarQuantidadeCartas = false;

                            resetarJogadores = true;

                            break;


                        case 3:

                            resetarQuantidadeCartas = true;

                            resetarJogadores = false;
                            break;

                        case 4:

                            resetarQuantidadeCartas = true;

                            resetarJogadores = true;


                            break;


                        default:

                            throw new Exception("Escolha Inválida!");


                            break;




                    }



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
