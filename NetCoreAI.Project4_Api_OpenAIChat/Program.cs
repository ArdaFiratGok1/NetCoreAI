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
        .AddJsonFile("appsettings.json")
        .Build();

        var apiKey = config["OpenAI:ApiKey"];
        Console.WriteLine("Sorunuzu yazınız: ");

        var prompt = Console.ReadLine();

        using var httpclient = new HttpClient();
        httpclient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new // Bu request body'ler API'lere özel oluyor
        {
            model = "gpt-3.5-turbo",// burayı değiştirebilirsin
            messages = new[]
            {
                new {role="system",content="You are a helpful asisstant"},//kibar ol, mühendis gibi cevap ver
                new {role="user",content=prompt}

            }, 
            max_tokens = 500
        };

        var json=JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json,Encoding.UTF8,"application/json");//API'ye istek atmak için string content haline getiriyorum.

        try
        {
            var response = await httpclient.PostAsync("https://api.openai.com/v1/chat/completions",content);//URL, OpenAI Chat Completions endpointidir.
                                                                                                            //Karşıya prompt gönderip cevap beklendiği için Post işlemi yapılır.
            var responseString = await response.Content.ReadAsStringAsync();//OpenAI'den gelen cevabı string olarak alır.

            if(response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<JsonElement>(responseString);//Json elementine dönüştürür.
                var answer = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();//OpenAI'a özgü, GetProperty ile json içinde yazılı olan özelliklerin karşılık geldiği değeri döndürür
                                                                                                                        //bu durumda JSON formatında gelen cevaptan choices[0].message.content alanı okunur.
                Console.WriteLine("ChatGPT:");
                Console.WriteLine(answer);
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