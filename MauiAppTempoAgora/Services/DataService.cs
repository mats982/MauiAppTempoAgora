using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao (string cidade) 
        {
            Tempo? t = null;

            string chave = "b5d88e3f73c3858728ad94791acae63f";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}&lang=pt_br";


            using (HttpClient httpClient = new HttpClient()) 
            {
                HttpResponseMessage resp = await httpClient.GetAsync(url);

                if (resp.IsSuccessStatusCode) 
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; // Fecha objeto do tempo 
                } // Fecha if para saber se o status do servidor foi de sucesso

                else if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Cidade não encontrada. Verifique o nome e tente novamente.");
                }
                else
                {
                    throw new Exception("Erro ao buscar dados do servidor.");
                }
            } // Fecha using


            return t;
        }
    }
}
