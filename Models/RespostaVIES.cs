namespace ValidadorVIES.Models
{
    public class RespostaVIES
    {
        public bool Valido { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
    }

}
