using System;
using System.IO;

class Program
{
    // Vetores para armazenar dados dos voos
    static int[] codigos = new int[5];
    static string[] destinos = new string[5];
    static int[] assentosDisponiveis = new int[5];
    static string[,] reservas = new string[5, 50]; // Matriz para armazenar as reservas

    static void Main()
    {
        int opcao;

        // Menu principal do programa
        do
        {
            Console.WriteLine("\n===== MENU PRINCIPAL =====");
            Console.WriteLine("1. Importar dados dos voos");
            Console.WriteLine("2. Realizar reserva");
            Console.WriteLine("3. Cancelar reserva");
            Console.WriteLine("4. Consultar assentos disponíveis");
            Console.WriteLine("5. Relatório de ocupação de voos");
            Console.WriteLine("6. Sair e gerar relatório");
            Console.Write("Escolha uma opção: ");
            opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1: ImportarVoos(); break;
                case 2: RealizarReserva(); break;
                case 3: CancelarReserva(); break;
                case 4: ConsultarAssentos(); break;
                case 5: RelatorioOcupacao(); break;
                case 6: GerarRelatorioFinal(); Console.WriteLine("Encerrando programa."); break;
                default: Console.WriteLine("Opção inválida."); break;
            }

        } while (opcao != 6);
    }


    static void ImportarVoos() //Função que importa os dados dos voos de um arquivo
    {
        string caminho = "voos.txt.txt";

        if (!File.Exists(caminho))
        {
            Console.WriteLine("Arquivo 'voos.txt.txt' não encontrado!");
            return;
        }

        string[] linhas = File.ReadAllLines(caminho);
        for (int i = 0; i < linhas.Length && i < 5; i++)
        {
            string[] partes = linhas[i].Split(';');
            codigos[i] = int.Parse(partes[0]);
            destinos[i] = partes[1];
            assentosDisponiveis[i] = int.Parse(partes[2]);
        }

        Console.WriteLine("Voos importados com sucesso.\n");

        // Mostrar os voos para o usuário
        Console.WriteLine("===== Lista de Voos Disponíveis =====");
        for (int i = 0; i < codigos.Length; i++)
        {
            if (codigos[i] != 0)
            {
                Console.WriteLine($"Código: {codigos[i]} - Destino: {destinos[i]} ");
            }
        }
    }


    static int BuscarIndiceVoo(int codigo)
    {
        for (int i = 0; i < codigos.Length; i++)
        {
            if (codigos[i] == codigo) return i;
        }
        return -1; // Retorna -1 se o voo não for encontrado
    }

    static void RealizarReserva()
    {
        Console.Write("Digite o código do voo: ");
        int codigo = int.Parse(Console.ReadLine());
        int i = BuscarIndiceVoo(codigo);

        if (i == -1)
        {
            Console.WriteLine("Voo não encontrado.");
            return;
        }

        Console.Write("Digite o número do assento (1 a 50): ");
        int assento = int.Parse(Console.ReadLine());

        if (assento < 1 || assento > 50)
        {
            Console.WriteLine("Assento inválido.");
            return;
        }

        if (reservas[i, assento - 1] != null)
        {
            Console.WriteLine("Assento já reservado.");
            return;
        }

        Console.Write("Digite o nome do passageiro: ");
        string nome = Console.ReadLine();
        reservas[i, assento - 1] = nome;
        assentosDisponiveis[i]--;
        Console.WriteLine("Reserva realizada com sucesso!");
    }

    static void CancelarReserva()
    {
        Console.Write("Digite o código do voo: ");
        int codigo = int.Parse(Console.ReadLine());
        int i = BuscarIndiceVoo(codigo);

        if (i == -1)
        {
            Console.WriteLine("Voo não encontrado.");
            return;
        }

        Console.Write("Digite o número do assento a cancelar (1 a 50): ");
        int assento = int.Parse(Console.ReadLine());

        if (assento < 1 || assento > 50)
        {
            Console.WriteLine("Assento inválido.");
            return;
        }

        if (reservas[i, assento - 1] == null)
        {
            Console.WriteLine("Esse assento já está disponível.");
            return;
        }

        reservas[i, assento - 1] = null;
        assentosDisponiveis[i]++;
        Console.WriteLine("Reserva cancelada com sucesso.");
    }

    static void ConsultarAssentos()
    {
        for (int i = 0; i < codigos.Length; i++)
        {
            if (codigos[i] == 0) continue; // ignorar e continuar se não for igual

            Console.WriteLine($"\nVoo {codigos[i]} - Destino: {destinos[i]}");
            Console.Write("Assentos disponíveis: ");
            for (int j = 0; j < 50; j++)
            {
                if (reservas[i, j] == null)
                    Console.Write($"{j + 1} ");
            }
            Console.WriteLine();
        }
    }

    static void RelatorioOcupacao()
    {
        Console.Write("Digite o código do voo para ver ocupação: ");
        int codigo = int.Parse(Console.ReadLine());
        int i = BuscarIndiceVoo(codigo);

        if (i == -1)
        {
            Console.WriteLine("Voo não encontrado.");
            return;
        }

        Console.WriteLine($"\nVoo {codigos[i]} - Destino: {destinos[i]}");
        for (int j = 0; j < 50; j++)
        {
            string status; 

            if (reservas[i, j] == null)
            {
                // Se o assento estiver vazio (null), o status é "Disponível"
                status = "Disponível";
            }
            else
            {
                // Senão, o status é o nome do passageiro
                status = reservas[i, j];
            }

            Console.WriteLine($"Assento {j + 1}: {status}");
        }
    }

    static void GerarRelatorioFinal()
    {
        string caminho = "relatorio.txt";
        using (StreamWriter sw = new StreamWriter(caminho))
        {
            sw.WriteLine("Relatório Final de Ocupação:");
            sw.WriteLine("==============================");

            for (int i = 0; i < codigos.Length; i++)
            {
                if (codigos[i] == 0) continue;

                sw.WriteLine($"\nVoo {codigos[i]} - Destino: {destinos[i]}");
                sw.WriteLine("--- Status dos Assentos ---");

                int assentosOcupados = 0; // Zera o contador para cada voo

                // Loop para verificar cada assento e contar os ocupados
                for (int j = 0; j < 50; j++)
                {
                    if (reservas[i, j] != null)
                    {
                        sw.WriteLine($"Assento {j + 1}: {reservas[i, j]}");
                        assentosOcupados++; 
                    }
                    else
                    {
                        sw.WriteLine($"Assento {j + 1}: Disponível");
                    }
                }

                sw.WriteLine("--- Resumo ---");
                sw.WriteLine($"Total de Assentos Reservados: {assentosOcupados}");
                sw.WriteLine($"Total de Assentos Disponíveis: {assentosDisponiveis[i]}");
                sw.WriteLine("==============================");
            }
        }

        Console.WriteLine("Relatório final salvo como 'relatorio.txt'.");
    }
}
