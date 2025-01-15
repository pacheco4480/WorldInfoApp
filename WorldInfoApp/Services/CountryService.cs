using Newtonsoft.Json; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Net.Http; 
using System.Text; 
using System.Threading.Tasks; 
using WorldInfoApp.Models; 

namespace WorldInfoApp.Services
{
    public class CountryService
    {
        // Método assíncrono que obtém dados dos países a partir da API
        public async Task<Response> GetCountriesAsync(string urlBase, string endpoint, IProgress<int> progress)
        {
            try
            {
                // Inicializa um cliente HTTP com a URL base fornecida
                var client = new HttpClient { BaseAddress = new Uri(urlBase) };

                // Faz uma requisição GET para o endpoint fornecido
                var response = await client.GetAsync(endpoint);

                // Lê o conteúdo da resposta como string
                var result = await response.Content.ReadAsStringAsync();

                // Verifica se a requisição foi bem-sucedida
                if (!response.IsSuccessStatusCode)
                {
                    // Retorna uma resposta indicando falha, com a mensagem de erro
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                // Desserializa a string JSON para uma lista de objetos do tipo Country
                var countries = JsonConvert.DeserializeObject<List<Country>>(result);

                // Reporta o progresso de forma incremental
                int totalCountries = countries.Count;
                for (int i = 0; i < totalCountries; i++)
                {
                    // Simula o processamento de cada país
                    await Task.Delay(5); // Simula um atraso no processamento

                    // Reporta o progresso incremental
                    progress?.Report((i + 1) * 100 / totalCountries);
                }

                // Retorna uma resposta indicando sucesso, com a lista de países
                return new Response
                {
                    IsSuccess = true,
                    Result = countries
                };
            }
            catch (Exception ex)
            {
                // Captura exceções e retorna uma resposta indicando falha, com a mensagem de erro
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
