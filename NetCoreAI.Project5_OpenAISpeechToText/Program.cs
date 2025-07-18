using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. appsettings.json'dan API anahtarını okuyoruz
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var apiKey = config["OpenAI:ApiKey"];

        string audioFilePath = "audio3.mp3";//ses dosyasının yolu

        using (var client  = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var form = new MultipartFormDataContent();

            var audioContent = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
            audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio3/mpeg");//mpeg mp3 dosyaları içindir. Burada videonun sadece ses kısmını alacağımız için mp3 kullanacağız.
            form.Add(audioContent, "file", Path.GetFileName(audioFilePath));
            form.Add(new StringContent("whisper-1"), "model");

            Console.WriteLine("Ses Dosyası İşlenniyor...");

            var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);//istek atacağım endpoint

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();//responseString = result burada, chatbotta böyle değildi bakabilirsin ona da
                Console.WriteLine("Transkript: ");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Hata: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());//responseString burada hatayı daha iyi görebilmek için
            }
        }
    }
}