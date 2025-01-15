using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WorldInfoApp.Models;

namespace WorldInfoApp.Views
{
    public partial class CountryDetailsWindow : Window
    {
        // Construtor que recebe um objeto do tipo Country
        public CountryDetailsWindow(Country country)
        {
            InitializeComponent();
            DataContext = country; // Define o DataContext da janela para o país fornecido

            // Define os valores dos TextBlocks e da imagem com base nas propriedades do país
            CountryNameTextBlock.Text = country.Name.Common;
            OfficialNameTextBlock.Text = country.Name.Official;
            CapitalTextBlock.Text = country.GetDisplayCapital();
            RegionTextBlock.Text = country.GetDisplayRegion();
            SubregionTextBlock.Text = country.GetDisplaySubregion();
            PopulationTextBlock.Text = country.GetDisplayPopulation();
            FlagImage.Source = new BitmapImage(new Uri(country.Flags.png));
            GiniTextBlock.Text = country.GetDisplayGini();
        }

        // Evento que ocorre quando a imagem da bandeira falha ao carregar
        private void FlagImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                // Define uma imagem padrão quando a imagem da bandeira falha ao carregar
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/default_flag.png", UriKind.Absolute));
            }
        }
    }
}
