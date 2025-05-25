using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using ValidadorVIES.Models;
using System.Text.RegularExpressions;

namespace ValidadorVIES.Services
{
    public class VIESService
    {
        private readonly HttpClient _httpClient;

        public VIESService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RespostaVIES> ValidarContribuinteAsync(string pais, string nif)
        {
            var soapEnvelope = $@"
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                              xmlns:urn=""urn:ec.europa.eu:taxud:vies:services:checkVat:types"">
               <soapenv:Header/>
               <soapenv:Body>
                  <urn:checkVat>
                     <urn:countryCode>{pais}</urn:countryCode>
                     <urn:vatNumber>{nif}</urn:vatNumber>
                  </urn:checkVat>
               </soapenv:Body>
            </soapenv:Envelope>";

            var content = new StringContent(soapEnvelope, System.Text.Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "");

            var response = await _httpClient.PostAsync("https://ec.europa.eu/taxation_customs/vies/services/checkVatService", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var doc = XDocument.Parse(responseContent);
            XNamespace ns = "urn:ec.europa.eu:taxud:vies:services:checkVat:types";

            var nome = doc.Descendants(ns + "name").FirstOrDefault()?.Value.Trim() ?? "";
            var moradaCompleta = doc.Descendants(ns + "address").FirstOrDefault()?.Value.Replace("\n", ", ").Trim() ?? "";

            string rua = "", localidade = "", codigopostal = "";

            // Extrair código postal e remover o que está à frente
            var match = Regex.Match(moradaCompleta, @"(\d{4}-\d{3})");
            if (match.Success)
            {
                codigopostal = match.Groups[1].Value;

                // Corta a morada onde começa o código postal
                var partes = moradaCompleta.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length >= 2)
                {
                    rua = partes[0].Trim();
                    localidade = partes.Length > 1 ? partes[1].Trim() : "";
                }
            }
            else
            {
                rua = moradaCompleta; // fallback
            }

            return new RespostaVIES
            {
                Valido = doc.Descendants(ns + "valid").FirstOrDefault()?.Value.Trim().ToLower() == "true",
                Nome = nome,
                Rua = rua,
                Localidade = localidade,
                CodigoPostal = codigopostal
            };
        }
    }
}
