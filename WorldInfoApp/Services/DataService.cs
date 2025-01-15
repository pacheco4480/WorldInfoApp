using System; 
using System.Collections.Generic; 
using System.Data.SQLite; 
using System.IO; 
using WorldInfoApp.Models; 

namespace WorldInfoApp.Services
{
    public class DataService
    {
        private SQLiteConnection connection; // Conexão com o banco de dados SQLite
        private SQLiteCommand command; // Comando SQLite para executar consultas SQL
        private DialogService dialogService; // Serviço para exibir diálogos de mensagem

        // Construtor da classe DataService
        public DataService()
        {
            dialogService = new DialogService(); // Inicializa o serviço de diálogo

            // Verifica se a pasta "Data" existe, se não, cria-a
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            // Define o caminho do arquivo SQLite
            var path = @"Data\Countries.sqlite";

            try
            {
                // Inicializa e abre a conexão com o banco de dados SQLite
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                // Comando SQL para criar a tabela de países, se não existir
                string sqlcommand = "CREATE TABLE IF NOT EXISTS countries (" +
                                    "NameCommon VARCHAR(100), " +
                                    "NameOfficial VARCHAR(100), " +
                                    "Capital VARCHAR(100), " +
                                    "Region VARCHAR(50), " +
                                    "Subregion VARCHAR(50), " +
                                    "Population INT, " +
                                    "FlagPng VARCHAR(250), " +
                                    "FlagSvg VARCHAR(250), " +
                                    "Gini DOUBLE)";

                // Executa o comando SQL
                command = new SQLiteCommand(sqlcommand, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // Exibe mensagem de erro em caso de exceção
                dialogService.ShowMessage("Erro", e.Message);
            }
        }

        // Método para guardar dados no banco de dados
        public void SaveData(List<Country> countries)
        {
            try
            {
                // Itera sobre a lista de países e insere-os na base de dados
                foreach (var country in countries)
                {
                    // Obtém o valor do índice Gini, se existir
                    double giniValue = country.Gini != null && country.Gini.Count > 0 ? country.Gini.Values.First() : 0;

                    // Comando SQL para inserir um país na tabela
                    string sql = "INSERT INTO countries (NameCommon, NameOfficial, Capital, Region, Subregion, Population, FlagPng, FlagSvg, Gini) " +
                                 "VALUES (@NameCommon, @NameOfficial, @Capital, @Region, @Subregion, @Population, @FlagPng, @FlagSvg, @Gini)";

                    // Cria um novo comando SQLite com parâmetros
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NameCommon", country.Name.Common ?? "N/A");
                        command.Parameters.AddWithValue("@NameOfficial", country.Name.Official ?? "N/A");
                        command.Parameters.AddWithValue("@Capital", country.Capital?.FirstOrDefault() ?? "N/A");
                        command.Parameters.AddWithValue("@Region", country.Region ?? "N/A");
                        command.Parameters.AddWithValue("@Subregion", country.Subregion ?? "N/A");
                        command.Parameters.AddWithValue("@Population", country.Population);
                        command.Parameters.AddWithValue("@FlagPng", country.Flags.png ?? "");
                        command.Parameters.AddWithValue("@FlagSvg", country.Flags.svg ?? "");
                        command.Parameters.AddWithValue("@Gini", giniValue);

                        // Executa o comando SQL
                        command.ExecuteNonQuery();
                    }
                }

                // Fecha a conexão com o banco de dados
                connection.Close();
            }
            catch (Exception e)
            {
                // Exibe mensagem de erro em caso de exceção
                dialogService.ShowMessage("Erro", e.Message);
            }
        }

        // Método para obter dados do banco de dados
        public List<Country> GetData()
        {
            List<Country> countries = new List<Country>(); // Lista para armazenar os países

            try
            {
                // Comando SQL para selecionar todos os países da tabela
                string sql = "SELECT NameCommon, NameOfficial, Capital, Region, Subregion, Population, FlagPng, FlagSvg, Gini FROM countries";
                command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader(); // Executa o comando e obtém um leitor de dados

                // Lê os dados do leitor e adiciona-os à lista de países
                while (reader.Read())
                {
                    var giniValue = reader["Gini"] != DBNull.Value ? new Dictionary<string, double> { { "Latest", Convert.ToDouble(reader["Gini"]) } } : null;
                    countries.Add(new Country
                    {
                        Name = new Name
                        {
                            Common = reader["NameCommon"]?.ToString() ?? "N/A",
                            Official = reader["NameOfficial"]?.ToString() ?? "N/A"
                        },
                        Capital = reader["Capital"] != DBNull.Value ? new List<string> { reader["Capital"].ToString() } : null,
                        Region = reader["Region"]?.ToString() ?? "N/A",
                        Subregion = reader["Subregion"]?.ToString() ?? "N/A",
                        Population = reader["Population"] != DBNull.Value ? Convert.ToInt32(reader["Population"]) : 0,
                        Flags = new Flags
                        {
                            png = reader["FlagPng"]?.ToString() ?? "",
                            svg = reader["FlagSvg"]?.ToString() ?? ""
                        },
                        Gini = giniValue
                    });
                }

                // Fecha a conexão com o banco de dados
                connection.Close();
                return countries; // Retorna a lista de países
            }
            catch (Exception e)
            {
                // Exibe mensagem de erro em caso de exceção
                dialogService.ShowMessage("Erro", e.Message);
                return null; // Retorna nulo em caso de erro
            }
        }

        // Método para excluir todos os dados da tabela
        public void DeleteData()
        {
            try
            {
                // Comando SQL para excluir todos os países da tabela
                string sql = "DELETE FROM countries";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    // Executa o comando SQL
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                // Exibe mensagem de erro em caso de exceção
                dialogService.ShowMessage("Erro", e.Message);
            }
        }
    }
}
