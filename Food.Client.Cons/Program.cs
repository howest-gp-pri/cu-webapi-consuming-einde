using Food.Client.Cons.Models;
using System.Net.Http.Json;

namespace Food.Client.Cons
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7011/api/");

                Category[] categories = await httpClient.GetFromJsonAsync<Category[]>("categories");
                PrintCategories(categories);

                Console.WriteLine("Login...");
                Console.WriteLine("Geef uw e-mailadres in aub:");
                string email = Console.ReadLine();
                Console.WriteLine("Geef uw password in aub:");
                string password = Console.ReadLine();

                var applicationUser = new ApplicationUser
                {
                    Email = email,
                    Password = password
                };

                var authResponse = await httpClient.PostAsJsonAsync("auth/login", applicationUser);
                var loginResponse = await authResponse.Content.ReadFromJsonAsync<LoginResponse>();

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResponse.Token);

                var newCategory = new Category
                {
                    Name = "Snacks"
                };

                await httpClient.PostAsJsonAsync("categories", newCategory);
                categories = await httpClient.GetFromJsonAsync<Category[]>("categories");

                PrintCategories(categories);

                Console.ReadLine();
            }
        }

        private static void PrintCategories(Category[] categories)
        {
            foreach (Category category in categories)
            {
                Console.WriteLine($"{category.Id} - {category.Name}");
            }
        }
    }
}
