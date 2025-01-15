using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using System.Windows; 

namespace WorldInfoApp.Services
{
    // Classe que fornece serviços de diálogo
    public class DialogService
    {
        // Método para exibir uma mensagem numa caixa de diálogo
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title); // Exibe a mensagem com um título
        }
    }
}
