using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json; // Add this namespace for AddJsonFile extension method  

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var apiKey = config["OpenAI:ApiKey"];
        Console.WriteLine("Sorunuzu yazınız: ");

        var chat_messages = new List<Dictionary<string, string>>
{
    new() { ["role"] = "system", ["content"] = "Sen tecrübeli bir acil tıp uzmanısın. Acile gelen her hastayı iyi bir şekilde yönetebiliyorsun. Cevaplarını ona göre ver." }
};


        while (true)
        {
            Console.Write("Siz:");
            var prompt = Console.ReadLine();

            chat_messages.Add(new() { ["role"] = "user", ["content"] = prompt }); // Kullanıcı mesajını listeye ekle

            using var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-4o",
                messages = chat_messages, // Artık tüm geçmişi gönderiyoruz
                max_tokens = 500
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpclient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<JsonElement>(responseString);
                    var answer = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                    Console.WriteLine("ChatGPT:");
                    Console.WriteLine(answer);

                    chat_messages.Add(new() { ["role"] = "assistant", ["content"] = answer }); // GPT cevabını da listeye ekle
                }
                else
                {
                    Console.WriteLine($"Bir hata oluştu: {response.StatusCode}");
                    Console.WriteLine(responseString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}