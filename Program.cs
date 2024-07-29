using System.Text.Json;

namespace Quotes_Generator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            bool anotherJoke = true;
            string url = $"https://api.api-ninjas.com/v1/quotes?category={Category()}";
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("No API key found. Get your api key from: https://api-ninjas.com/profile.");
                Console.Write("Api Key: ");
                apiKey = Console.ReadLine();
            }

            while (anotherJoke)
            {
                Console.Clear();
                Console.WriteLine("WHAT IS THE QUOTE OF THE DAY?\n");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        var json = await response.Content.ReadAsStringAsync();
                        List<Quote> quotes = JsonSerializer.Deserialize<List<Quote>>(json);
                        if (quotes != null)
                        {
                            var quote = quotes[0];
                            Console.WriteLine($"{quote.quote}\n-{quote.author}");
                        }
                        else
                        {
                            Console.WriteLine("Something happened");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                Console.WriteLine("\nDo you want to here another quote?");
                string input = Console.ReadLine().ToLower();

                if (input != "yes")
                {
                    anotherJoke = false;
                }
            }
        }

        static string Category()
        {
            Random random = new Random();
            List<string> list = new List<string>();
            int randomCategory;

            list.AddRange(new string[] { "age", "alone", "amazing", "anger", "architecture", "art", "attitude",
    "beauty", "best", "birthday", "business", "car", "change", "communication",
    "computers", "cool", "courage", "dad", "dating", "death", "design", "dreams",
    "education", "environmental", "equality", "experience", "failure", "faith",
    "family", "famous", "fear", "fitness", "food", "forgiveness", "freedom",
    "friendship", "funny", "future", "god", "good", "government", "graduation",
    "great", "happiness", "health", "history", "home", "hope", "humor",
    "imagination", "inspirational", "intelligence", "jealousy", "knowledge",
    "leadership", "learning", "legal", "life", "love", "marriage", "medical",
    "men", "mom", "money", "morning", "movies", "success"});
            randomCategory = random.Next(0, list.Count);
            return list[randomCategory];
        }
    }

    public class Quote
    {
        public string quote { get; set; }
        public string author { get; set; }
        public string category { get; set; }
    }
}
