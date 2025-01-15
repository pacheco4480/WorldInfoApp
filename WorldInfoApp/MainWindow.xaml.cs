using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WorldInfoApp.Models;
using WorldInfoApp.Services;
using WorldInfoApp.Views;

namespace WorldInfoApp
{
    public partial class MainWindow : Window
    {
        // Coleção observável de países exibida na interface
        public ObservableCollection<Country> Countries { get; set; }

        // Serviços utilizados na aplicação
        private readonly CountryService countryService;
        private readonly DataService dataService;
        private readonly DialogService dialogService;
        private readonly NetworkService networkService;
        private List<Country> allCountries;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Countries = new ObservableCollection<Country>();
            countryService = new CountryService();
            dataService = new DataService();
            dialogService = new DialogService();
            networkService = new NetworkService();
            LoadCountries(); // Carrega os países ao iniciar a aplicação
        }

        // Evento que ocorre quando a imagem falha ao carregar
        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/default_flag.png", UriKind.Absolute));
            }
        }

        // Método assíncrono para carregar os países
        private async void LoadCountries()
        {
            ProgressPanel.Visibility = Visibility.Visible;
            LoadingProgressBar.Value = 0;
            ProgressText.Text = "0%";

            var connection = networkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                dialogService.ShowMessage("Erro", connection.Message);
                var offlineCountries = dataService.GetData();
                allCountries = offlineCountries;
                DisplayCountries(offlineCountries);

                ProgressPanel.Visibility = Visibility.Collapsed;
                return;
            }

            // Configuração de progresso
            var progress = new Progress<int>(value =>
            {
                LoadingProgressBar.Value = value;
                ProgressText.Text = $"{value}%";

                if (value == 100)
                {
                    ProgressPanel.Visibility = Visibility.Collapsed;
                }
            });

            // Chamada ao serviço para obter países
            var response = await countryService.GetCountriesAsync("https://restcountries.com/v3.1/", "all", progress);

            if (!response.IsSuccess)
            {
                dialogService.ShowMessage("Erro", response.Message);
                var offlineCountries = dataService.GetData();
                allCountries = offlineCountries;
                DisplayCountries(offlineCountries);
                ProgressPanel.Visibility = Visibility.Collapsed;
                return;
            }

            var countries = (List<Country>)response.Result;
            dataService.DeleteData();
            dataService.SaveData(countries);
            allCountries = countries;
            DisplayCountries(countries);
        }

        // Método para exibir os países na interface
        private void DisplayCountries(List<Country> countries)
        {
            Countries.Clear();

            // Aplicar filtro de região
            var selectedRegion = ((ComboBoxItem)RegionFilter.SelectedItem).Content.ToString();
            var filteredCountries = selectedRegion == "All Regions"
                                    ? countries
                                    : countries.Where(c => c.Region == selectedRegion).ToList();

            // Aplicar ordenação
            var selectedOrder = ((ComboBoxItem)SortOrderComboBox.SelectedItem).Content.ToString();
            var sortedCountries = selectedOrder == "A-Z"
                                    ? filteredCountries.OrderBy(c => c.Name.Common).ToList()
                                    : filteredCountries.OrderByDescending(c => c.Name.Common).ToList();

            // Adicionar países filtrados e ordenados à coleção observável
            foreach (var country in sortedCountries)
            {
                Countries.Add(country);
            }
        }

        // Evento para abrir a janela de detalhes do país ao clicar em um país
        private void CountryCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement && frameworkElement.DataContext is Country country)
            {
                var detailsWindow = new CountryDetailsWindow(country);
                detailsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                detailsWindow.ShowDialog();
            }
        }

        // Evento que ocorre ao mudar a seleção do filtro de região
        private void RegionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allCountries != null)
            {
                DisplayCountries(allCountries);
            }
        }
    }
}
