using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        await Download();
    }

    static async Task Download()
    {
        var types = new string[] {
            "actions", "blueprints", "collectibleResources", "equipmentItemTypes", "harvestLoots",
            "itemTypes", "itemProperties", "items", "jobsItems", "recipeCategories", "recipeIngredients",
            "recipeResults", "recipes", "resourceTypes", "resources", "states"
        };

        using var http = new HttpClient { BaseAddress = new Uri("https://wakfu.cdn.ankama.com") };

        var response = await http.GetAsync("/gamedata/config.json");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Error fetching config.json");
            return;
        }

        var configContent = await response.Content.ReadAsStringAsync();
        var currentVersion = JsonConvert.DeserializeObject<VersionWakfu>(configContent)?.Version;
        if (string.IsNullOrEmpty(currentVersion))
        {
            Console.WriteLine("Failed to retrieve version");
            return;
        }
        Console.WriteLine("Version: " + currentVersion);

        string outputDirectory = "WakfuJsonFiles_" + currentVersion;
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        foreach (var type in types)
        {
            string url = $"/gamedata/{currentVersion}/{type}.json";
            response = await http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string filePath = Path.Combine(outputDirectory, $"{type}.json");
                await File.WriteAllTextAsync(filePath, content);
                Console.WriteLine($"Downloaded: {type}.json");
            }
            else
            {
                Console.WriteLine($"Failed to download {type}.json");
            }
        }
    }
}

class VersionWakfu
{
    public string Version { get; set; }
}
