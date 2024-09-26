using CareerBot.Api.Models;
using Microsoft.AspNetCore.Authentication;

namespace CareerBot.Api.Scrapers;

public sealed class HHScraper
{
    private const string HhApiUrl = "https://api.hh.ru/vacancies";
    private const string HhWebUrl = "https://hh.ru/search/vacancy";

    /// <summary>
    /// Получение списка вакансий с hh.ru через API.
    /// </summary>
    /// <param name="searchQuery">Строка поиска вакансий.</param>
    /// <returns>Список вакансий в виде строк.</returns>
    public static async Task<List<Vacancy>> GetVacanciesFromApi(string searchQuery)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0");
            client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Add("Referer", "https://hh.ru/");

            string url = $"{HhApiUrl}?text={searchQuery}&area=1";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(jsonResult))
                {
                    List<Vacancy> vacancies = new List<Vacancy>();
                    foreach (JsonElement item in doc.RootElement.GetProperty("items").EnumerateArray())
                    {
                        var salaryProperty = item.GetProperty("salary");

                        var salary = "Не указана";

                        if (salaryProperty.ValueKind is not JsonValueKind.Null)
                        {
                            string salaryFrom = salaryProperty.GetProperty("from").ValueKind == JsonValueKind.Null
                                ? string.Empty
                                : $"{salaryProperty.GetProperty("from").GetInt32()}";

                            string salaryTo = salaryProperty.GetProperty("to").ValueKind == JsonValueKind.Null
                                ? string.Empty
                                : $"{salaryProperty.GetProperty("to").GetInt32()}";

                            string currency = salaryProperty.GetProperty("currency").ValueKind == JsonValueKind.Null
                                ? string.Empty
                                : $"{salaryProperty.GetProperty("currency").GetString()}";

                            bool? isGross = salaryProperty.GetProperty("gross").ValueKind == JsonValueKind.Null
                                ? null
                                : salaryProperty.GetProperty("gross").GetBoolean();

                            string isGrossString = isGross.HasValue ? "Gross" : "NET";

                            salary = $"{salaryFrom} - {salaryTo} [{currency}] ({isGrossString})";
                        }

                        Vacancy vacancy = new();

                        vacancy.Title = item.GetProperty("name").GetString();
                        vacancy.Company = item.GetProperty("employer").GetProperty("name").GetString();
                        vacancy.Salary = salary;
                        vacancy.Location = item.GetProperty("area").GetProperty("name").GetString();
                        vacancy.PublishedDate = DateTime.Parse(item.GetProperty("published_at").GetString());
                        vacancy.VacancyUrl = item.GetProperty("alternate_url").GetString();
                        vacancy.VacancyId = item.GetProperty("id").GetString();
                        vacancy.Experience = item.GetProperty("experience").GetProperty("name").GetString();
                        vacancy.EmploymentType = item.GetProperty("employment").GetProperty("name").GetString();

                        vacancies.Add(vacancy);
                    }

                    return vacancies;
                }
            }

            throw new Exception("Не удалось получить данные с API hh.ru.");
        }
    }
}
