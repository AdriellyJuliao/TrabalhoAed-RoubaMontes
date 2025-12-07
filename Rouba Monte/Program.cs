using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Trabalho_de_AED___Rouba_Monte
{
    public class Jogador
    {
        private string nome;

        private int posicao; //Classificação da última partida

        private int quantCartas; //Da última partida

        private int quantCartasAgora; //Do jogo atual


        private Queue<int> ranking; //Stream Reader

        private List<Carta> MonteDoJogador;

        public Jogador(string nome) //CONSTRUTOR
        {
            this.nome = nome;
            this.posicao = -1;
            this.quantCartas = 0;
            this.quantCartasAgora = 0;
            this.ranking = null;
            this.MonteDoJogador = new List<Carta>();
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

        public void CartaDaVez(MontedeCompra MontedeCompraPartida, List<Jogador> Jogadores, AreadeDescarte Mesa)
        {
            //Fazer validacao para que o sistema nao procure um jogador que nao tenha um monte pois vai dar erro de out of range
            //Prioridade menor mas seria interessante na hora de digitar o nome dos jogadores nao deixar repetir pois pode dar confusão no metodo CartaDaVez
            bool FimdaCartadaVez = false;

            do
            {
                Carta CartadoMomento = MontedeCompraPartida.RemoverMontedeCarta();
                List<Jogador> listadeAdversariosCompativeis = new List<Jogador>();

                foreach (Jogador x in Jogadores)
                {
                    if (x.MonteDoJogador.Count > 0 &&
                        CartadoMomento.Numero == x.MonteDoJogador[x.MonteDoJogador.Count - 1].Numero &&
                        x.nome != Nome)
                    {
                        listadeAdversariosCompativeis.Add(x);
                    }

                }

                // --- SE EXISTEM VÁRIOS ADVERSÁRIOS COMPATÍVEIS ---
                if (listadeAdversariosCompativeis.Count > 1)
                {
                    int maiorQuantidadeCartas = int.MinValue;

                    foreach (Jogador x in listadeAdversariosCompativeis)
                    {
                        if (x.MonteDoJogador.Count > maiorQuantidadeCartas)
                        {
                            maiorQuantidadeCartas = x.MonteDoJogador.Count;
                        }
                    }

                    for (int i = listadeAdversariosCompativeis.Count - 1; i >= 0; i--)
                    {
                        if (listadeAdversariosCompativeis[i].MonteDoJogador.Count != maiorQuantidadeCartas)
                        {
                            listadeAdversariosCompativeis.RemoveAt(i);
                        }
                    }
                }

                // --- SOMENTE 1 ADVERSÁRIO ---
                if (listadeAdversariosCompativeis.Count == 1)
                {
                    while (listadeAdversariosCompativeis[0].MonteDoJogador.Count > 0)
                    {
                        Carta carta = listadeAdversariosCompativeis[0].MonteDoJogador[0];
                        MonteDoJogador.Add(carta);
                        QuantCartasAgora++;

                        listadeAdversariosCompativeis[0].MonteDoJogador.RemoveAt(0);
                        listadeAdversariosCompativeis[0].QuantCartasAgora--;
                    }

                    MonteDoJogador.Add(CartadoMomento);
                    QuantCartasAgora++;
                }

                // --- AINDA EXISTEM VÁRIOS APÓS O FILTRO (EMPATE) ---
                else if (listadeAdversariosCompativeis.Count > 1)
                {
                    Random jogadorAleatorio = new Random();
                    int indiceJogadorAleatorio = jogadorAleatorio.Next(0, listadeAdversariosCompativeis.Count);

                    Jogador jogadorEscolhido = listadeAdversariosCompativeis[indiceJogadorAleatorio];

                    while (jogadorEscolhido.MonteDoJogador.Count > 0)
                    {
                        Carta carta = jogadorEscolhido.MonteDoJogador[0];
                        MonteDoJogador.Add(carta);
                        QuantCartasAgora++;

                        jogadorEscolhido.MonteDoJogador.RemoveAt(0);
                        jogadorEscolhido.QuantCartasAgora--;
                    }

                    MonteDoJogador.Add(CartadoMomento);
                    QuantCartasAgora++;
                }

                // --- ROUBAR DA ÁREA DE DESCARTE (comparando apenas o número) ---
                else
                {
                    // procurar carta com mesmo número na mesa
                    int indiceEncontrado = -1;

                    for (int i = 0; i < Mesa.Cartas.Count; i++)
                    {
                        if (Mesa.Cartas[i].Numero == CartadoMomento.Numero)
                        {
                            indiceEncontrado = i;
                            break;
                        }
                    }

                    if (indiceEncontrado != -1)
                    {
                        // pegar a carta
                        Carta cartaNaMesa = Mesa.Cartas[indiceEncontrado];

                        // remover da mesa
                        Mesa.Cartas.RemoveAt(indiceEncontrado);

                        // adicionar as duas cartas ao jogador
                        MonteDoJogador.Add(cartaNaMesa);
                        MonteDoJogador.Add(CartadoMomento);

                        QuantCartasAgora += 2;
                    }
                    else
                    {
                        // descartar normalmente
<<<<<<< HEAD
                        Mesa.Mesa.Cartas.Add(CartadoMomento);
=======
                        Mesa.Mesa.Cartas.Add(CartadoMomento);;
>>>>>>> d8253765fde61dc26ec4ffc45b46fca1dadbb5f1
                        FimdaCartadaVez = true;
                    }
                }
            } while (!FimdaCartadaVez);
<<<<<<< HEAD


        }

        public void VisualizarRanking()
        {
            if(ranking.Count <0 || ranking.Count >5)
            {
                throw new Exception("Quantidade de Rankings do jogador (" + nome + ") está fora do limite")
            }
            int contagemRanking = 0
            foreach(int rankingdojogador in ranking)
            {
                Console.WriteLine(contagemRanking + ") " + rankingdojogador);

            }
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
=======
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
>>>>>>> d8253765fde61dc26ec4ffc45b46fca1dadbb5f1








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

        public MontedeCompra(int quantidadeCarta)
        {
            cartasparaComprar = new List<Carta>();
            this.quantidadeCarta = quantidadeCarta;
        }

<<<<<<< HEAD
=======

        public void PreencherMontedeCompras()
        {
            string[] tiposNaipes = { "Copas", "Ouros", "Paus", "Espadas" };
            Random naipeAleatorio = new Random();

            int definidordeNumeroCarta = 1;

            for (int i = 0; i < quantidadeCarta; i++)
            {
                // Reinicia numeração após 13
                if (definidordeNumeroCarta > 13)
                    definidordeNumeroCarta = 1;

                // Sorteia o naipe
                int indicedoVetorNaipes = naipeAleatorio.Next(0, 4);

                // Cria a carta
                Carta CartaGerada = new Carta(definidordeNumeroCarta, tiposNaipes[indicedoVetorNaipes]);

                cartasparaComprar.Add(CartaGerada);

                definidordeNumeroCarta++;
            }

        }
        

        public void EmbaralharMontedeCompra()
        {
            // Verificação básica
            if (cartasparaComprar == null || quantidadeCarta <= 0)
            {
                throw new Exception("Monte vazio ou de tamanho inválido");
            }

            // Algoritmo para embaralhar, pegando uma carta aleatória e trocando de posição
            Random cartaAleatoria = new Random();

            for (int i = cartasparaComprar.Count - 1; i > 0; i--)
            {
                // Sorteia um índice entre 0 e i
                int j = cartaAleatoria.Next(0, i + 1);

                // Troca as cartas de posição
                Carta temp = cartasparaComprar[i];
                cartasparaComprar[i] = cartasparaComprar[j];
                cartasparaComprar[j] = temp;

            }
        }


        public Carta RemoverMontedeCarta()
        {
            if(cartasparaComprar.Count == 0)
            {
                throw new Exception("O monte de compra está vazio");
            }
            else { 
                Carta carta = cartasparaComprar[cartasparaComprar.Count - 1];
            cartasparaComprar.RemoveAt(cartasparaComprar.Count - 1);
            return carta;
            }
        }


        //Método para inserir carta no monte



>>>>>>> d8253765fde61dc26ec4ffc45b46fca1dadbb5f1
        public List<Carta> CartasparaComprar
        {
            get { return cartasparaComprar; }
            set { cartasparaComprar = value; }
        }

        public int QuantidadeCarta
        {
            get { return quantidadeCarta; }
            set { quantidadeCarta = value; }
        }


        public void PreencherMontedeCompras()
        {
            string[] tiposNaipes = { "Copas", "Ouros", "Paus", "Espadas" };
            Random naipeAleatorio = new Random();

            int definidordeNumeroCarta = 1;

            for (int i = 0; i < quantidadeCarta; i++)
            {
                // Reinicia numeração após 13
                if (definidordeNumeroCarta > 13)
                    definidordeNumeroCarta = 1;

                // Sorteia o naipe
                int indicedoVetorNaipes = naipeAleatorio.Next(0, 4);

                // Cria a carta
                Carta CartaGerada = new Carta(definidordeNumeroCarta, tiposNaipes[indicedoVetorNaipes]);

                cartasparaComprar.Add(CartaGerada);

                definidordeNumeroCarta++;
            }

        }


        public void EmbaralharMontedeCompra()
        {
            // Verificação básica
            if (cartasparaComprar == null || quantidadeCarta <= 0)
            {
                throw new Exception("Monte vazio ou de tamanho inválido");
            }

            // Algoritmo para embaralhar, pegando uma carta aleatória e trocando de posição
            Random cartaAleatoria = new Random();

            for (int i = cartasparaComprar.Count - 1; i > 0; i--)
            {
                // Sorteia um índice entre 0 e i
                int j = cartaAleatoria.Next(0, i + 1);

                // Troca as cartas de posição
                Carta temp = cartasparaComprar[i];
                cartasparaComprar[i] = cartasparaComprar[j];
                cartasparaComprar[j] = temp;

            }
        }


        public Carta RemoverMontedeCarta() //Remove o topo do monte
        {
            if (cartasparaComprar.Count == 0)
            {
                throw new Exception("O monte de compra está vazio");
            }
            else
            {
                Carta carta = cartasparaComprar[cartasparaComprar.Count - 1];
                cartasparaComprar.RemoveAt(cartasparaComprar.Count - 1);
                return carta;
            }
        }


        //Método para inserir carta no monte



       
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

        public List<Carta> Cartas
        {
            get { return cartas; }
            set { cartas = value; }
        }

        public void InserirAreaDescarte(Carta x)
        {
            cartas.Add(x);
        }

        public void ImprimirAreadeDescarte()
        {
            foreach (Carta x in cartas)
            {
                Console.Write("||") //Simular o visual de uma carta?
                Console.Write(x.Numero "||");
            }

        }




        //Inicializa vazia


        //Imprimir área de descarte para o jogador

    }



    public class Arquivo
    {

        /*O programa deve gerar um arquivo texto com o log das ações executadas em cada partida. Exemplo: "O baralho foi 
          criado com X cartas. Jogadores da partida: [nomes dos jogadores]. Em seguida, indique qual jogador iniciará. Após isso, 
          registre a carta retirada por cada jogador do monte, e continue registrando todas as ações subsequentes ao longo da 
          partida, detalhando cada evento ocorrido."*/

        //Gerado ao fim, não é impresso durante o jogo
        //Somente as 5 últimas rodadas, ou seja, será preciso reescrever toda vez que terminar uma partida


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

            bool continuarJogando, resetarJogadores, resetarQuantidadeCartas, visualizarRankings;

            int quantCartas, quantJogadores;

            Lista<Jogador> jogadoresDaPartida;

            do
            {
                if (resetarQuantidadeCartas)
                {
                    Console.WriteLine("Quantas cartas serão geradas?");
                    quantCartas = int.Parse.(Console.ReadLine());

                    while (quantCartas < 0)
                    {
                        Console.WriteLine("A quantidade de cartas deve ser positiva");
                        quantCartas = int.Parse.(Console.ReadLine());

                    }
                }

                if (resetarJogadores)
                {

                    Console.WriteLine("Quantos jogadores irão participar?");
                    quantJogadores = int.Parse(Console.ReadLine());

                    while (quantJogadores < 0)
                    {
                        Console.WriteLine("A quantidade de jogadores deve ser positiva");
                        quantJogadores = int.Parse.(Console.ReadLine());

                    }
                }

                MontedeCompra MontedeCompras = new MontedeCompra(quantCartas);

                MontedeCompras.PreencherMontedeCompras();
                MontedeCompras.EmbaralharMontedeCompra();

                AreadeDescarte MesadaPartida = new AreadeDescarte();

                for (int i = 0; i <= quantJogadores; i++)
                {
                    Console.WriteLine("Nome do jogador " + i++);
                    string nomeJogador = Console.ReadLine()


                    Jogador jogadorNovo = new Jogador(nomeJogador);


                    jogadoresDaPartida.Add(jogadorNovo);

                }

                do
                {

                    foreach (Jogador jogadordaVez in jogadoresDaPartida)
                    {
                        jogadordaVez.CartaDaVez(MontedeCompras, jogadoresDaPartida, MesadaPartida);
                    }



                    //Aqui onde vai rolar o jogo em si



                } while (MontedeCompras.QuantidadedeCarta != 0); //Continuar até o monte de compra estiver vazio

                //Ordenar ranking dos jogadores aqui, após o jogo ter terminado

                foreach(Jogador jogadorEscolhido in jogadoresDaPartida)
                {
                    jogadorEscolhido.QuantCartas = jogadorEscolhido.QuantCartasAgora;
                }

                foreach (Jogador x in FiladeJogadores) //Será Armazenado sempre a quantidade da última partida
                {
                    x.QuantCartas = x.QuantCartasAgora;
                }

                do
                {

                    Console.WriteLine("Deseja ver os Rankings de algum jogador?");
                    Console.WriteLine("Digite S ou N");
                    char resp1 = Console.ReadLine();

                    if (resp1 == 'S' || resp1 == 's')
                    {
                        visualizarRankings = true;
                    }

                    else if (resp1 == 'N' || resp1 == 'n')
                    {
                        visualizarRankings = false;
                    }
                    else
                    {
                        throw new Exception("Resposta Inválida");
                    }



                    if (visualizarRankings)
                    {

                        Console.WriteLine("Digite o nome de um jogador");

                    }


                } while (visualizarRankings);





                Console.WriteLine("Quer Continuar Jogando?"); //Colocar qual caractere usar para responder
                Console.WriteLine("Digite S ou N");
                char resp2 = Console.ReadLine();

                if (resp2 == 'S' || resp2 == 's')
                {
                    continuarJogando = true;
                }

                else if (resp2 == 'N' || resp2 == 'n')
                {
                    continuarJogando = false;
                }
                else
                {
                    throw new Exception("Resposta Inválida");
                }

                if (continuarJogando)
                {

                    Console.WriteLine("1) Manter Cartas e Jogadores");
                    Console.WriteLine("2) Manter Cartas e Alterar Jogadores");
                    Console.WriteLine("3) Alterar Cartas e Manter Jogadores");
                    Console.WriteLine("4) Alterar Cartas e Jogadores");
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

