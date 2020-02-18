using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XamarinFormsExample.Helpers;
using XamarinFormsExample.Models;

namespace XamarinFormsExample.Services
{
    public class MarvelApiService
    {
        private static string[] _herois = new string[]
        {
            "Captain America", "Iron Man", "Thor", "Hulk", "Wolverine", "Spider-Man"
        };

        public async Task<List<Personagem>> GetPersonagensAsync()
        {
            var httpClient = new HttpClient();
            List<Personagem> personagens = new List<Personagem>();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string ts = DateTime.Now.Ticks.ToString();
            string publicKey = Constantes.PublicKey;
            string hash = SecurityHelper.GerarHash(ts, publicKey, Constantes.PrivateKey);

            foreach (var heroi in _herois)
            {
                var response = await httpClient.GetAsync(
                    Constantes.ApiBaseUrl + $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
                    $"name={Uri.EscapeUriString(heroi)}").ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    string conteudo = response.Content.ReadAsStringAsync().Result;

                    dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                    var personagem = new Personagem();
                    personagem.Nome = resultado.data.results[0].name;
                    personagem.Descricao = resultado.data.results[0].description;
                    
                    string path = resultado.data.results[0].thumbnail.path;

                    personagem.UrlImagem = "https" + path.Substring(4) + "." + resultado.data.results[0].thumbnail.extension;
                    personagem.UrlWiki = resultado.data.results[0].urls[1].url;

                    personagens.Add(personagem);
                }
            }

            return personagens;
        }
    }
}
