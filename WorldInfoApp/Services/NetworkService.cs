using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Net; 
using System.Text; 
using System.Threading.Tasks; 
using WorldInfoApp.Models; 

namespace WorldInfoApp.Services
{
    // Classe que fornece serviços de rede
    public class NetworkService
    {
        // Método para verificar a conexão com a internet
        public Response CheckConnection()
        {
            var client = new WebClient();

            // Se houver conexão, devolve um objeto do tipo Response indicando sucesso
            try
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSuccess = true
                    };
                }
            }
            // Se não houver conexão, devolve um objeto do tipo Response indicando falha e uma mensagem de erro
            catch
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Configure a sua ligação à Internet"
                };
            }
        }
    }
}
