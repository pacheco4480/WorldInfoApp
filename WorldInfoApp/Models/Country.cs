namespace WorldInfoApp.Models
{
    // Classe que representa um país
    public class Country
    {
        // Propriedade para o nome do país
        public Name Name { get; set; }

        // Propriedade para a capital do país, representada como uma lista de strings
        public List<string> Capital { get; set; }

        // Propriedade para a região do país
        public string Region { get; set; }

        // Propriedade para a sub-região do país
        public string Subregion { get; set; }

        // Propriedade para a população do país
        public int Population { get; set; }

        // Propriedade para as bandeiras do país, representadas por URLs
        public Flags Flags { get; set; }

        // Propriedade para o índice de Gini do país, representado por um dicionário
        public Dictionary<string, double> Gini { get; set; }

        // Método para retornar a capital do país ou "N/A" se não disponível
        public string GetDisplayCapital()
        {
            if (Capital != null && Capital.Count > 0)
                return string.Join(", ", Capital);
            return "N/A";
        }

        // Método para retornar a região do país ou "N/A" se não disponível
        public string GetDisplayRegion()
        {
            if (!string.IsNullOrEmpty(Region))
                return Region;
            return "N/A";
        }

        // Método para retornar a sub-região do país ou "N/A" se não disponível
        public string GetDisplaySubregion()
        {
            if (!string.IsNullOrEmpty(Subregion))
                return Subregion;
            return "N/A";
        }

        // Método para retornar a população do país ou "N/A" se não disponível
        public string GetDisplayPopulation()
        {
            if (Population > 0)
                return Population.ToString("N0");
            return "N/A";
        }

        // Método para retornar o índice de Gini do país ou "N/A" se não disponível
        public string GetDisplayGini()
        {
            if (Gini != null && Gini.Count > 0)
                return Gini.Values.First().ToString("F1");
            return "N/A";
        }
    }

    // Classe que representa o nome do país
    public class Name
    {
        // Nome comum do país
        public string Common { get; set; }

        // Nome oficial do país
        public string Official { get; set; }
    }

    // Classe que representa as bandeiras do país
    public class Flags
    {
        // URL da imagem PNG da bandeira
        public string png { get; set; }

        // URL da imagem SVG da bandeira
        public string svg { get; set; }

        // Texto alternativo para a bandeira
        public string alt { get; set; }
    }
}
