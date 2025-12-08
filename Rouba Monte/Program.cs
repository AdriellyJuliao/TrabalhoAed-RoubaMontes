using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using Trabalho_de_AED___Rouba_Monte;

namespace Trabalho_de_AED___Rouba_Monte
{
    public class Jogador
    {
        private string nome;

        private int posicao; //Classificação da última partida

        private int quantCartas; //Da última partida

        private int quantCartasAgora; //Do jogo atual

        private Queue<int> ranking; //Stream Reader

        private List<Carta> monteDoJogador;

        public Jogador(string nome) //CONSTRUTOR
        {
            this.nome = nome;
            this.posicao = -1;
            this.quantCartas = 0;
            this.quantCartasAgora = 0;
            this.ranking = new Queue<int>();
            this.monteDoJogador = new List<Carta>();
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

        public List<Carta> MonteDoJogador
        {
            get { return monteDoJogador; }
            set { monteDoJogador = value; }
        }

        public void CartaDaVez(MontedeCompra MontedeCompraPartida, List<Jogador> Jogadores, AreadeDescarte Mesa)
        {
            //Fazer validacao para que o sistema nao procure um jogador que nao tenha um monte pois vai dar erro de out of range
            //Prioridade menor mas seria interessante na hora de digitar o nome dos jogadores nao deixar repetir pois pode dar confusão no metodo CartaDaVez
            bool FimdaCartadaVez = false;

            if (MontedeCompraPartida == null)
            {
                Console.WriteLine("Monte de Compra Esgotado");
                FimdaCartadaVez = true;
            }
            else {

                do
                {
                    Carta CartadoMomento = MontedeCompraPartida.RemoverMontedeCarta();
                    logdoJogo.Registrar(this.nome "Retirou do Monte de Compra a carta " + CartadoMomento.Numero + " de " + CartadoMomento.Naipe + ".");

                    List<Jogador> listadeAdversariosCompativeis = new List<Jogador>();

                    foreach (Jogador x in Jogadores)
                    {
                        if (x.MonteDoJogador.Count > 0 &&
                            CartadoMomento.Numero == x.MonteDoJogador[x.MonteDoJogador.Count - 1].Numero &&
                            x.Nome != this.Nome)
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
                                i = Mesa.Cartas.Count; //Retirado o break, já que o valor do count da mesa tem o mesmo resultado
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

                        else if (this.MonteDoJogador.Count > 0 && this.MonteDoJogador[this.MonteDoJogador.Count - 1].Numero == CartadoMomento.Numero)
                        {
                            this.MonteDoJogador.Add(CartadoMomento);
                            this.QuantCartasAgora++;
                            continue; // continua o do/while
                        }

                        else
                        {
                            // descartar normalmente
                            Mesa.Cartas.Add(CartadoMomento);
                            Mesa.Cartas.Add(CartadoMomento);
                            FimdaCartadaVez = true;
                        }
                    }
                } while (!FimdaCartadaVez);

            }
        }

        public void VisualizarRanking()
        {
            if (ranking.Count > 5)
            {
                throw new Exception("Quantidade de Rankings do jogador (" + nome + ") está fora do limite");
            }
            int contagemRanking = 0;

            Console.WriteLine("Rankings do " + nome);

            foreach (int rankingdojogador in ranking)
            {
                Console.WriteLine(contagemRanking + ") " + rankingdojogador);
                contagemRanking++;

            }
        }

        public static List<Jogador> OrdernarListaJogadores(List<Jogador> listadeJogadoresdaPartida)
        {
            // Usar Quicksort para ordenar a lista de jogadores pela quantidade de cartas
            Quicksort(listadeJogadoresdaPartida, 0, listadeJogadoresdaPartida.Count - 1);

            // Atribuindo as posições de classificação após a ordenação
            for (int i = 0; i < listadeJogadoresdaPartida.Count; i++)
            {
                listadeJogadoresdaPartida[i].Posicao = i + 1;  // Classificação começa de 1
            }

            return listadeJogadoresdaPartida;

        }

        private void Quicksort(List<Jogador> listadaOrdenacao, int esq, int dir)
        {
            int i = esq, j = dir;
            Jogador pivo = listadaOrdenacao[(esq + dir) / 2];

            while (i <= j)
            {
                // Encontrar elemento à esquerda maior que o pivo
                while (listadaOrdenacao[i].QuantCartas < pivo.QuantCartas)
                    i++;

                // Encontrar elemento à direita menor que o pivo
                while (listadaOrdenacao[j].QuantCartas > pivo.QuantCartas)
                    j--;

                // Se encontrou elementos válidos
                if (i <= j)
                {
                    // Trocar os jogadores
                    Jogador temp = listadaOrdenacao[i];
                    listadaOrdenacao[i] = listadaOrdenacao[j];
                    listadaOrdenacao[j] = temp;

                    i++;
                    j--;
                }
            }

            // Recursão nas duas metades
            if (esq < j)
            {
                Quicksort(listadaOrdenacao, esq, j);
            }
            if (i < dir)
            {
                Quicksort(listadaOrdenacao, i, dir);
            }
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


        public Carta RemoverMontedeCarta()
        {
            if(cartasparaComprar.Count == 0)
            {
                throw new Exception("O monte de compra está vazio");
            }
            else 
        
            {
            Carta carta = cartasparaComprar[count - 1];
            cartasparaComprar.RemoveAt(count - 1);
            quantidadeCarta--;
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
        private List<Carta> cartas;

        private int quantidadeCartasNaMesa;

        public AreadeDescarte()
        {
            cartas = new List<Carta>();
            quantidadeCartasNaMesa = 0;
        }

        public List<Carta> Cartas
        {
            get { return cartas; }
            set { cartas = value; }
        }

        public int QuantidadeCartasNaMesa
        {
            get { return quantidadeCartasNaMesa; }
            set { quantidadeCartasNaMesa = value; }
        }

    /*
        public void InserirAreaDescarte(Carta x)
        {
            cartas.Add(x);
        }
    */
        public void ImprimirAreadeDescarte()
        {
            foreach (Carta x in cartas)
            {
              Console.Write("||" + x.Numero + "||");  //Simular o visual de uma carta?
            }

        }

        public void LimparAreadeDescarte()
        {
         cartas.Clear();
        quantidadeCartasNaMesa = 0;

    }




    //Inicializa vazia


    //Imprimir área de descarte para o jogador

}



public class Arquivo
{

    private List<string> logDaPartida;

    private const string nomedoArquivo = "log_ultimas_partidas.txt";

    public Arquivo()
    {
        logDaPartida = new List<string>();
    }

    // Adiciona mensagens ao log interno
    public void Registrar(string mensagem)
    {
        string linha = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + mensagem;
        logDaPartida.Add(linha);
    }

    // Grava a partida no arquivo, mantendo no máximo 5
    public void SalvarPartida(int numeroPartida)
    {
        List<string> linhasExistentes = new List<string>();

        if (File.Exists(nomedoArquivo))
        {
            linhasExistentes.AddRange(File.ReadAllLines(nomedoArquivo));
        }

        // Adiciona o cabeçalho da nova partida
        linhasExistentes.Add("");
        linhasExistentes.Add("=============================================");
        linhasExistentes.Add($"   PARTIDA {numeroPartida} - {DateTime.Now:dd/MM/yyyy HH:mm}");
        linhasExistentes.Add("=============================================");
        linhasExistentes.Add("");

        // Adiciona o conteúdo do log
        linhasExistentes.AddRange(logDaPartida);

        // Limita para no máximo 5 partidas
        List<string> logFinal = ManterApenas5Partidas(linhasExistentes);

        // Reescreve o arquivo inteiro
        File.WriteAllLines(nomedoArquivo, logFinal);

        // Limpa o log interno para a próxima partida
        logDaPartida.Clear();
    }

    // Limita o arquivo para conter somente as últimas 5 partidas
     private List<string> ManterApenas5Partidas(List<string> linhas)
    {
        const string cabecalho = "=============================================";

        // Encontra todas as posições onde começa uma partida
        List<int> indicesPartidas = new List<int>();

        for (int i = 0; i < linhas.Count; i++)
        {
            if (linhas[i].StartsWith(cabecalho))
            {
                indicesPartidas.Add(i);
            }
        }

        if (indicesPartidas.Count <= 5)
        {
            return linhas;
        }

        int inicio = indicesPartidas[indicesPartidas.Count - 5];

        return linhas.GetRange(inicio, linhas.Count - inicio);


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

            bool continuarJogando = true, resetarJogadores = true, resetarQuantidadeCartas = true, visualizarRankings = true;

            int quantCartas, quantJogadores;

            List<Jogador> jogadoresDaPartida = new List<Jogador>();

            AreadeDescarte mesadaPartida = new AreadeDescarte();

            Arquivo logdoJogo = new Arquivo();


            do
            {
                if (resetarQuantidadeCartas)
                {
                    Console.WriteLine("Quantas cartas serão geradas?");
                    quantCartas = int.Parse(Console.ReadLine());

                    while (quantCartas < 0)
                    {
                        Console.WriteLine("A quantidade de cartas deve ser positiva");
                        quantCartas = int.Parse.(Console.ReadLine());

                    }
                }

                logdoJogo.Registrar("O Monte de Compra será criado com " + quantCartas + " cartas.");


                if (resetarJogadores)
                {

                    Console.WriteLine("Quantos jogadores irão participar?");
                    quantJogadores = int.Parse(Console.ReadLine());

                    while (quantJogadores < 0)
                    {
                        Console.WriteLine("A quantidade de jogadores deve ser positiva");
                        quantJogadores = int.Parse(Console.ReadLine());

                    }
                }

                logdoJogo.Registrar("O jogo contará com " + quantJogadores + " no total.");


                MontedeCompra montedeCompras = new MontedeCompra(quantCartas);

                logdoJogo.Registrar("O monte de compra foi criado.");


                montedeCompras.PreencherMontedeCompras();
                logdoJogo.Registrar("O monte de compra foi preenchido.");
                montedeCompras.EmbaralharMontedeCompra();
                logdoJogo.Registrar("O monte de compra foi embaralhado.");





                for (int i = 0; i < quantJogadores; i++)
                {
                    bool nomeigual = false;
                    string nomeJogadorInserir;
                    int contagemJogadoresInseridos = i + 1;
                    do
                    {
                        nomeigual = false;
                        Console.WriteLine("Nome do jogador " + contagemJogadoresInseridos);
                        nomeJogadorInserir = Console.ReadLine();

                        foreach (Jogador jogadoresRegistrados in jogadoresDaPartida)
                        {
                            if (jogadoresRegistrados.Nome == nomeJogadorInserir)
                            {
                                nomeigual = true;

                                Console.WriteLine("O nome " + nomeJogadorInserir + "já foi usado");
                                logdoJogo.Registrar("Não foi possível inserir novamente o nome " + nomeJogadorInserir);

                            }
                        }


                    } while (nomeigual);

                    Jogador jogadorNovo = new Jogador(nomeJogadorInserir);



                    jogadoresDaPartida.Add(jogadorNovo);

                    logdoJogo.Registrar("O " + nomeJogadorInserir + " ingressou no jogo.");


                }

                do
                {

                    foreach (Jogador jogadordaVez in jogadoresDaPartida)
                    {
                        logdoJogo.Registrar("Agora é a vez do " + jogadordaVez.Nome + " jogar");

                        jogadordaVez.CartaDaVez(montedeCompras, jogadoresDaPartida, mesadaPartida);
                    }



                    //Aqui onde vai rolar o jogo em si



                } while (montedeCompras.QuantidadeCarta != 0); //Continuar até o monte de compra estiver vazio

                mesadaPartida.LimparAreadeDescarte();
                logdoJogo.Registrar("A área de descarte foi limpa");


                //Ordenar ranking dos jogadores aqui, após o jogo ter terminado

                foreach (Jogador jogadorEscolhido in jogadoresDaPartida)
                {
                    jogadorEscolhido.QuantCartas = jogadorEscolhido.QuantCartasAgora;

                    jogadorEscolhido.QuantCartasAgora = 0;
                }


                jogadoresDaPartida = Jogador.OrdernarListaJogadores(jogadoresDaPartida);

                do
                {

                    Console.WriteLine("Deseja ver os Rankings de algum jogador?");
                    Console.WriteLine("Digite S ou N");
                    char resp1 = char.Parse(Console.ReadLine());

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
                        string nomeJogadorRanking = Console.ReadLine();


                        foreach (Jogador jogadorRanking in jogadoresDaPartida)
                        {
                            if (jogadorRanking.Nome == nomeJogadorRanking)
                            {
                                jogadorRanking.VisualizarRanking();
                            }
                        }

                    }


                } while (visualizarRankings);





                Console.WriteLine("Quer Continuar Jogando?"); //Colocar qual caractere usar para responder
                Console.WriteLine("Digite S ou N");
                char resp2 = char.Parse(Console.ReadLine());

                if (resp2 == 'S' || resp2 == 's')
                {
                    continuarJogando = true;
                    logdoJogo.Registrar("O jogo irá continuar");

                }

                else if (resp2 == 'N' || resp2 == 'n')
                {
                    continuarJogando = false;
                    logdoJogo.Registrar("O será encerrado");

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

                            logdoJogo.Registrar("A quantidade de cartas e os Jogadores serão Mantidos para a próxima partida");


                            break;

                        case 2:

                            resetarQuantidadeCartas = false;

                            resetarJogadores = true;

                            logdoJogo.Registrar("A quantidade de cartas será mantida mas os Jogadores serão alterados para a próxima partida");


                            break;


                        case 3:

                            resetarQuantidadeCartas = true;

                            resetarJogadores = false;

                            logdoJogo.Registrar("A quantidade de cartas será alterada mas os Jogadores serão mantidos para a próxima partida");

                            break;

                        case 4:

                            resetarQuantidadeCartas = true;

                            resetarJogadores = true;

                            logdoJogo.Registrar("A quantidade de cartas e os Jogadores serão Alterados para a próxima partida");



                            break;


                        default:

                            throw new Exception("Escolha Inválida!");


                            break;




                    }



                }


            } while (continuarJogando)


            Console.WriteLine("Obrigado por Jogar!");

            logdoJogo.Registrar("O jogo se encerrou");




            //Antes de jogar novamente, quer ver os rankings, de quem?

            //Quer alterar o número de cartas

            //Quer gerar novos jogadores?

            //Quer parar?
        }
    }
}

