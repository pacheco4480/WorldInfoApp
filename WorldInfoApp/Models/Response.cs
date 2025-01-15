using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldInfoApp.Models
{
    // Classe que representa a resposta de uma operação
    public class Response
    {
        // Propriedade que indica se a operação foi bem-sucedida ou não
        public bool IsSuccess { get; set; }

        // Propriedade para armazenar uma mensagem de erro ou sucesso
        public string Message { get; set; }

        // Propriedade para armazenar o resultado da operação
        public object Result { get; set; }
    }
}
