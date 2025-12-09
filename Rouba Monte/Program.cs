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

        private int posicao;

        private int quantCartas;

        private int quantCartasAgora;

        private Queue<int> ranking;

        private List<Carta> monteDoJogador;

        public Jogador(string nome)
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

        public void CartaDaVez(MontedeCompra montedeCompraPartida, List<Jogador> Jogadores, AreadeDescarte mesa, Arquivo logsdapartida)
        {
            bool fimdaCartadaVez = false;

            do
            {
                Carta CartadoMomento = montedeCompraPartida.RemoverMontedeCarta();
                if (montedeCompraPartida == null || montedeCompraPartida.QuantidadeCarta == 0)
                {
                    logsdapartida.Registrar(this.nome + " tentou retirar carta mas monte está vazio.");
                    break;
                }

                logsdapartida.Registrar(this.nome + " retirou do Monte de Compra a carta " +
                                       CartadoMomento.Numero + " de " + CartadoMomento.Naipe + ".");

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

                if (listadeAdversariosCompativeis.Count > 0)
                {
                    logsdapartida.Registrar("Adversários compatíveis: " +
                        string.Join(", ", listadeAdversariosCompativeis.Select(a => a.Nome)) + ".");
                }

                if (listadeAdversariosCompativeis.Count > 1)
                {
                    logsdapartida.Registrar("Vários adversários compatíveis detectados. Aplicando filtro por maior quantidade de cartas.");

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
                            logsdapartida.Registrar("Removido do empate: " + listadeAdversariosCompativeis[i].Nome);
                            listadeAdversariosCompativeis.RemoveAt(i);
                        }
                    }
                }

                if (listadeAdversariosCompativeis.Count == 1)
                {
                    logsdapartida.Registrar(this.nome + " está roubando o monte de " +
                                           listadeAdversariosCompativeis[0].Nome + ".");

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

                    logsdapartida.Registrar(this.nome + " adicionou a carta retirada ao próprio monte.");
                }

                else if (listadeAdversariosCompativeis.Count > 1)
                {
                    logsdapartida.Registrar("Empate entre adversários, sorteando um jogador para ser roubado.");

                    Random jogadorAleatorio = new Random();
                    int indiceJogadorAleatorio = jogadorAleatorio.Next(0, listadeAdversariosCompativeis.Count);

                    Jogador jogadorEscolhido = listadeAdversariosCompativeis[indiceJogadorAleatorio];

                    logsdapartida.Registrar("Jogador sorteado: " + jogadorEscolhido.Nome);

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

                    logsdapartida.Registrar(this.nome + " adicionou a carta retirada ao próprio monte.");
                }

                else
                {
                    logsdapartida.Registrar(this.nome + " está verificando a mesa para roubo.");

                    int indiceEncontrado = -1;

                    for (int i = 0; i < mesa.Cartas.Count; i++)
                    {
                        if (mesa.Cartas[i].Numero == CartadoMomento.Numero)
                        {
                            indiceEncontrado = i;
                            i = mesa.Cartas.Count;
                        }
                    }

                    if (indiceEncontrado != -1)
                    {
                        Carta cartaNaMesa = mesa.Cartas[indiceEncontrado];

                        mesa.Cartas.RemoveAt(indiceEncontrado);

                        MonteDoJogador.Add(cartaNaMesa);
                        MonteDoJogador.Add(CartadoMomento);

                        QuantCartasAgora += 2;

                        logsdapartida.Registrar(this.nome + " roubou a carta " +
                            cartaNaMesa.Numero + " de " + cartaNaMesa.Naipe + " da mesa.");
                    }

                    else if (this.MonteDoJogador.Count > 0 &&
                             this.MonteDoJogador[this.MonteDoJogador.Count - 1].Numero == CartadoMomento.Numero)
                    {
                        this.MonteDoJogador.Add(CartadoMomento);
                        this.QuantCartasAgora++;

                        logsdapartida.Registrar(this.nome + " empilhou a carta " +
                            CartadoMomento.Numero + " de " + CartadoMomento.Naipe + " no próprio monte.");

                        continue;
                    }

                    else
                    {
                        mesa.Cartas.Add(CartadoMomento);
                        fimdaCartadaVez = true;

                        logsdapartida.Registrar(this.nome + " descartou a carta " +
                            CartadoMomento.Numero + " de " + CartadoMomento.Naipe + ".");
                    }
                }
            }
            while (!fimdaCartadaVez);
        }

        public void VisualizarRanking()
        {
            int contagemRanking = 1;

            Console.WriteLine("Rankings do " + nome);

            foreach (int rankingdojogador in ranking)
            {
                Console.WriteLine(contagemRanking + ") " + rankingdojogador);
                contagemRanking++;

            }
        }

        public static List<Jogador> OrdernarListaJogadores(List<Jogador> listadeJogadoresdaPartida)
        {
            if (listadeJogadoresdaPartida == null)
            {
                return new List<Jogador>();
            }
            Quicksort(listadeJogadoresdaPartida, 0, listadeJogadoresdaPartida.Count - 1);

            for (int i = 0; i < listadeJogadoresdaPartida.Count; i++)
            {
                listadeJogadoresdaPartida[i].Posicao = i + 1;
            }

            return listadeJogadoresdaPartida;


        }

        private static void Quicksort(List<Jogador> listadaOrdenacao, int esq, int dir)
        {
            int i = esq, j = dir;
            Jogador pivo = listadaOrdenacao[(esq + dir) / 2];

            while (i <= j)
            {
                while (listadaOrdenacao[i].QuantCartas > pivo.QuantCartas)
                    i++;

                while (listadaOrdenacao[j].QuantCartas < pivo.QuantCartas)
                    j--;

                if (i <= j)
                {
                    Jogador temp = listadaOrdenacao[i];
                    listadaOrdenacao[i] = listadaOrdenacao[j];
                    listadaOrdenacao[j] = temp;

                    i++;
                    j--;
                }
            }

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


public class Carta
{
    private int numero;

    private string naipe;


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
        Random numeroAleatorio = new Random();


        for (int i = 0; i < quantidadeCarta; i++)
        {

            int indicedoVetorNaipes = naipeAleatorio.Next(0, 4);
            int numeroCarta = numeroAleatorio.Next(0, 14);

            Carta CartaGerada = new Carta(numeroCarta, tiposNaipes[indicedoVetorNaipes]);

            cartasparaComprar.Add(CartaGerada);

        }

    }


    public void EmbaralharMontedeCompra()
    {
        if (cartasparaComprar == null || quantidadeCarta <= 0)
        {
            throw new Exception("Monte vazio ou de tamanho inválido");
        }

        Random cartaAleatoria = new Random();

        for (int i = cartasparaComprar.Count - 1; i > 0; i--)
        {
            int j = cartaAleatoria.Next(0, i + 1);

            Carta temp = cartasparaComprar[i];
            cartasparaComprar[i] = cartasparaComprar[j];
            cartasparaComprar[j] = temp;

        }
    }


    public Carta RemoverMontedeCarta()
    {


        int count = cartasparaComprar.Count;
        Carta carta = cartasparaComprar[cartasparaComprar.Count - 1];
        cartasparaComprar.RemoveAt(cartasparaComprar.Count - 1);
        quantidadeCarta--;
        return carta;
    }
}














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

   

    public void ImprimirAreadeDescarte()
    {
        foreach (Carta x in cartas)
        {
            Console.Write("||" + x.Numero + "||");
        }
    }

    public void LimparAreadeDescarte()
    {
        cartas.Clear();
        quantidadeCartasNaMesa = 0;

    }
}



public class Arquivo
{
    /* private List<string> logDaPartida;

    private static readonly string pastaLogs = "logs";
    private static readonly string nomedoArquivo = Path.Combine(pastaLogs, "log_ultimas_partidas.txt");

    // Marcador exclusivo para localizar o início de cada partida
    private const string marcadorPartida = "#### INICIO PARTIDA";

    public Arquivo()
    {
        logDaPartida = new List<string>();

        // Cria pasta logs se não existir
        if (!Directory.Exists(pastaLogs))
        {
            Directory.CreateDirectory(pastaLogs);
        }
    }

    // Adiciona linhas ao log interno
    public void Registrar(string mensagem)
    {
        string linha = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + mensagem;
        logDaPartida.Add(linha);
        Console.WriteLine(linha);
    }

    // Salva a partida no arquivo
    public void SalvarPartida(int numeroPartida)
    {
        List<string> linhasExistentes = new List<string>();

        if (File.Exists(nomedoArquivo))
        {
            linhasExistentes.AddRange(File.ReadAllLines(nomedoArquivo));
        }

        // Adiciona a nova partida com o marcador único
        linhasExistentes.Add("");
        linhasExistentes.Add(marcadorPartida);
        linhasExistentes.Add($"PARTIDA {numeroPartida} - {DateTime.Now:dd/MM/yyyy HH:mm}");
        linhasExistentes.Add("=============================================");
        linhasExistentes.Add("");

        // Conteúdo desta partida
        linhasExistentes.AddRange(logDaPartida);

        // Garante no máximo 5 partidas
        List<string> logFinal = ManterApenas5Partidas(linhasExistentes);

        // Reescreve o arquivo
        File.WriteAllLines(nomedoArquivo, logFinal);

        // Limpa o log interno para a próxima partida
        logDaPartida.Clear();
    }

    // Mantém somente as últimas 5 partidas no arquivo
    private List<string> ManterApenas5Partidas(List<string> linhas)
    {
        List<int> indices = new List<int>();

        // Identifica onde começam as partidas
        for (int i = 0; i < linhas.Count; i++)
        {
            if (linhas[i].StartsWith(marcadorPartida))
            {
                indices.Add(i);
            }
        }

        // Se tiver 5 ou menos, mantém tudo
        if (indices.Count <= 5)
            return linhas;

        // Calcula o índice do começo da 5ª última
        int inicio = indices[indices.Count - 5];

        // Segurança: evita exceções
        if (inicio < 0 || inicio >= linhas.Count)
            return linhas;

        // Retorna somente as 5 últimas partidas
        return linhas.GetRange(inicio, linhas.Count - inicio);
    }
        */
    private List<string> logDaPartida;
    private bool logsNaSessao;


    // Agora o arquivo vai ficar DENTRO DA PASTA /logs
    private static readonly string pastaLogs = "logs";
    private static readonly string nomedoArquivo = Path.Combine(pastaLogs, "log_ultimas_partidas.txt");

    public Arquivo(bool logsNaSessao)
    {
        logDaPartida = new List<string>();

        // Se a pasta "logs" não existir, cria
        if (!Directory.Exists(pastaLogs))
        {
            Directory.CreateDirectory(pastaLogs);
        }

        this.logsNaSessao = logsNaSessao;

    }

    public bool LogsNaSessao
    {
        get { return logsNaSessao; }
        set { logsNaSessao = value; }
    }

    // Adiciona mensagens ao log interno
    public void Registrar(string mensagem)
    {
        string linha = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + mensagem;
        logDaPartida.Add(linha);

        if (logsNaSessao)
        {
            Console.WriteLine(linha);
        }
    }

    // Grava a partida no arquivo, mantendo no máximo 5
    public void SalvarPartida(int numeroPartida)
    {
        List<string> linhasExistentes = new List<string>();

        if (File.Exists(nomedoArquivo))
        {
            linhasExistentes.AddRange(File.ReadAllLines(nomedoArquivo));
        }

        // Cabeçalho da nova partida
        linhasExistentes.Add("");
        linhasExistentes.Add("=============================================");
        linhasExistentes.Add($"   PARTIDA {numeroPartida} - {DateTime.Now:dd/MM/yyyy HH:mm}");
        linhasExistentes.Add("=============================================");
        linhasExistentes.Add("");

        // Conteúdo do log interno
        linhasExistentes.AddRange(logDaPartida);

        // Limita para no máximo 5 partidas
        List<string> logFinal = ManterApenas5Partidas(linhasExistentes);

        // Reescreve o arquivo completo na pasta /logs
        File.WriteAllLines(nomedoArquivo, logFinal);

        // Limpa log interno
        logDaPartida.Clear();
    }

    // Mantém somente as últimas 5 partidas
    private List<string> ManterApenas5Partidas(List<string> linhas)
    {
        const string cabecalho = "=============================================";

        List<int> indicesPartidas = new List<int>();

        for (int i = 0; i < linhas.Count; i++)
        {
            if (linhas[i].StartsWith(cabecalho))
            {
                indicesPartidas.Add(i);
            }
        }

        // Se tiver 5 ou menos, retorna tudo
        if (indicesPartidas.Count <= 5)
        {
            return linhas;
        }

        // Pega as últimas 5
        int inicio = indicesPartidas[indicesPartidas.Count - 5];

        return linhas.GetRange(inicio, linhas.Count - inicio);
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            bool continuarJogando = true, resetarJogadores = true, resetarQuantidadeCartas = true, visualizarRankings = true, logsNaPartida = true;

            int quantCartas = -1, quantJogadores = -1,  quantPartidas = 0;

            List<Jogador> jogadoresDaPartida = new List<Jogador>();

            AreadeDescarte mesadaPartida = new AreadeDescarte();


            Console.WriteLine("Bem-vindo ao Rouba montes");

            Console.WriteLine("Deseja visualizar os logs na sessão atual? (S/N)");

            string respVisualizarLogs = Console.ReadLine().Trim();

            while (respVisualizarLogs == "" ||
                   respVisualizarLogs.Length != 1 ||
                   (respVisualizarLogs != "S" && respVisualizarLogs != "s" &&
                    respVisualizarLogs != "N" && respVisualizarLogs != "n"))
            {
                Console.WriteLine("Entrada inválida! Digite apenas S ou N:");
                respVisualizarLogs = Console.ReadLine().Trim();
            }

            char escolha = respVisualizarLogs.ToUpper()[0];

            logsNaPartida = (escolha == 'S');

            Arquivo logdoJogo = new Arquivo(logsNaPartida);


            do
            {
                quantPartidas++;

                Console.WriteLine("Partida " + quantPartidas);
                logdoJogo.Registrar("Preparamento para a partida de número " + quantPartidas);


                if (resetarQuantidadeCartas)
                {
                    Console.WriteLine("Quantas cartas serão geradas?");

                    while (!int.TryParse(Console.ReadLine(), out quantCartas) || quantCartas < 0)
                    {
                        Console.WriteLine("Valor inválido! Digite um número inteiro positivo:");
                    }
                }
                logdoJogo.Registrar("O Monte de Compra será criado com " + quantCartas + " cartas.");


                if (resetarJogadores)
                {
                    jogadoresDaPartida.Clear();
                    Console.WriteLine("Quantos jogadores irão participar?");

                    while (!int.TryParse(Console.ReadLine(), out quantJogadores) || quantJogadores < 0)
                    {
                        Console.WriteLine("Valor inválido! Digite um número inteiro positivo:");
                    }
                }


                logdoJogo.Registrar("O jogo contará com " + quantJogadores + " no total.");


                MontedeCompra montedeCompras = new MontedeCompra(quantCartas);

                logdoJogo.Registrar("O monte de compra foi criado.");

                montedeCompras.PreencherMontedeCompras();
                logdoJogo.Registrar("O monte de compra foi preenchido.");
                montedeCompras.EmbaralharMontedeCompra();
                logdoJogo.Registrar("O monte de compra foi embaralhado.");


                if (resetarJogadores)
                {
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

                    logdoJogo.Registrar("O jogador que começará a partida será " + jogadoresDaPartida[0].Nome);
                }
                do
                {
                    int i = 0;

                    while (montedeCompras.QuantidadeCarta > 0)
                    {
                        Jogador jogadordaVez = jogadoresDaPartida[i];

                        logdoJogo.Registrar("Agora é a vez de " + jogadordaVez.Nome + " jogar");

                        jogadordaVez.CartaDaVez(montedeCompras, jogadoresDaPartida, mesadaPartida, logdoJogo);

                        i++;

                        if (i >= jogadoresDaPartida.Count)
                        {
                            i = 0;
                        }
                    }

                } while (montedeCompras.QuantidadeCarta != 0);

                mesadaPartida.LimparAreadeDescarte();
                logdoJogo.Registrar("A área de descarte foi limpa");

                foreach (Jogador jogadorEscolhido in jogadoresDaPartida)
                {
                    jogadorEscolhido.QuantCartas = jogadorEscolhido.QuantCartasAgora;

                    jogadorEscolhido.MonteDoJogador.Clear();

                    jogadorEscolhido.QuantCartasAgora = 0;
                }


                jogadoresDaPartida = Jogador.OrdernarListaJogadores(jogadoresDaPartida);

                foreach (Jogador x in jogadoresDaPartida)
                {
                    if (x.Ranking.Count == 5)
                    {
                        x.Ranking.Dequeue();
                    }
                    x.Ranking.Enqueue(x.Posicao);
                }

                int maiorPontuacao = jogadoresDaPartida[0].QuantCartas;

                List<Jogador> ganhadores = new List<Jogador>();

                foreach (Jogador x in jogadoresDaPartida)
                {
                    if (x.QuantCartas == maiorPontuacao)
                    {

                        ganhadores.Add(x);
                    }
                }

                Console.WriteLine("Resultado");

                if (ganhadores.Count == 1)
                {
                    Console.WriteLine("Vitória de " + ganhadores[0].Nome + " com " + ganhadores[0].QuantCartas + " cartas");

                    logdoJogo.Registrar("Vitória individual de " + ganhadores[0].Nome + " com " + ganhadores[0].QuantCartas + " cartas.");
                }

                else
                {
                    Console.WriteLine("Houve empate entre os jogadores:");

                    foreach (Jogador x in ganhadores)
                    {
                        Console.WriteLine(x.Nome + " (" + x.QuantCartas + " cartas)");
                    }

                    List<string> nomes = new List<string>();

                    foreach (Jogador x in ganhadores)
                    {
                        nomes.Add(x.Nome);
                    }

                    string nomesEmpate = string.Join(", ", nomes);

                    logdoJogo.Registrar("Empate entre " + nomesEmpate + " com " + maiorPontuacao + " cartas.");
                }

                foreach (Jogador x in jogadoresDaPartida)
                {
                    Console.WriteLine(x.Posicao + "° lugar — " + x.Nome + " com " + x.QuantCartas + " cartas");
                }

                logdoJogo.SalvarPartida(quantPartidas);

                do
                {
                    Console.WriteLine("Deseja ver os Rankings de algum jogador? (S/N)");
                    string entrada = Console.ReadLine().Trim();

                    while (entrada == "" ||
                           entrada.Length != 1 ||
                           (entrada != "S" && entrada != "s" &&
                            entrada != "N" && entrada != "n"))
                    {
                        Console.WriteLine("Entrada inválida! Digite apenas S ou N:");
                        entrada = Console.ReadLine().Trim();
                    }

                    char respRankings = entrada.ToUpper()[0];

                    if (respRankings == 'S')
                    {
                        visualizarRankings = true;

                        Console.WriteLine("Digite o nome de um jogador");
                        string nomeJogadorRanking = Console.ReadLine();

                        bool encontrado = false;

                        foreach (Jogador jogadorRanking in jogadoresDaPartida)
                        {
                            if (jogadorRanking.Nome == nomeJogadorRanking)
                            {
                                jogadorRanking.VisualizarRanking();
                                encontrado = true;
                                break;
                            }
                        }

                        if (!encontrado)
                        {
                            Console.WriteLine("Jogador não encontrado! Verifique o nome digitado.");
                        }
                    }
                    else
                    {
                        visualizarRankings = false;
                    }

                } while (visualizarRankings);
                Console.WriteLine("Deseja continuar jogando? (S/N)");
                string entradaContinuar = Console.ReadLine().Trim();

                while (entradaContinuar == "" ||
                       entradaContinuar.Length != 1 ||
                       (entradaContinuar != "S" && entradaContinuar != "s" &&
                        entradaContinuar != "N" && entradaContinuar != "n"))
                {
                    Console.WriteLine("Entrada inválida! Digite apenas S ou N:");
                    entradaContinuar = Console.ReadLine().Trim();
                }

                char respContinuar = entradaContinuar.ToUpper()[0];



                if (respContinuar == 'S' || respContinuar == 's')
                {
                    continuarJogando = true;


                    Console.WriteLine("1) Manter Quantidade de Cartas e Jogadores");
                    Console.WriteLine("2) Manter Quantidade de Cartas e Alterar Jogadores");
                    Console.WriteLine("3) Alterar Quantidade de Cartas e Manter Jogadores");
                    Console.WriteLine("4) Alterar Quantidade de Cartas e Jogadores");


                    int opcao = int.Parse(Console.ReadLine());


                    switch (opcao)
                    {
                        case 1:
                            resetarQuantidadeCartas = false;
                            resetarJogadores = false;
                            logdoJogo.Registrar("A quantidade de cartas e os Jogadores serão mantidos para a próxima partida");
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
                            throw new Exception("Opção inválida!");
                    }
                }
                else if (respContinuar == 'N' || respContinuar == 'n')
                {
                    continuarJogando = false;

                    logdoJogo.Registrar("A sessão irá se encerrar com " + quantPartidas + " partidas realizadas");

                }
                else
                {
                    throw new Exception("Resposta inválida!");
                }


            } while (continuarJogando);


            Console.WriteLine("Obrigado por Jogar!");

            logdoJogo.Registrar("O jogo se encerrou");

            Console.ReadLine();
        }
    }
}

