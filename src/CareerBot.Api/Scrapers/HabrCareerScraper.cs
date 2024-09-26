namespace CareerBot.Api.Scrapers;

public sealed class HabrCareerScraper
{
    /// <summary>
    /// Получение списка вакансий с career.habr.com через веб-скраппинг.
    /// </summary>
    /// <param name="searchQuery">Строка поиска вакансий.</param>
    /// <returns>Список вакансий в виде строк.</returns>
    public static async Task<List<Vacancy>> GetVacanciesFromHabrCareer(string searchQuery)
    {
        using HttpClient client = new HttpClient();
        // Устанавливаем user-agent для имитации браузерного запроса
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        // Формируем URL запроса
        string url = $"https://career.habr.com/vacancies?q={searchQuery}&type=all";

        HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string pageContent = await response.Content.ReadAsStringAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pageContent);

            List<Vacancy> vacancies = new List<Vacancy>();

            // Извлекаем все блоки с вакансиями
            var vacancyNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'vacancy-card')]");
            if (vacancyNodes != null)
            {
                foreach (var vacancyNode in vacancyNodes)
                {
                    // Извлекаем заголовок, ссылку на вакансию, компанию и место работы
                    var titleNode = vacancyNode.SelectSingleNode(".//a[contains(@class, 'vacancy-card__title-link')]");
                    var companyNode = vacancyNode.SelectSingleNode(".//a[contains(@class, 'vacancy-card__company-title')]");
                    var locationNode = vacancyNode.SelectSingleNode(".//div[contains(@class, 'vacancy-card__meta')]");

                    Vacancy vacancy = new Vacancy
                    {
                        Title = titleNode?.InnerText.Trim(),
                        VacancyUrl = $"https://career.habr.com{titleNode?.Attributes["href"]?.Value}",
                        Company = companyNode?.InnerText.Trim(),
                        Location = locationNode?.InnerText.Trim(),
                        PublishedDate = DateTime.Now,  // Habr Career не всегда показывает дату публикации на первой странице, так что можно использовать текущую дату
                        Salary = "Не указана", // Информации о зарплате нет в листинге, требуется скраппинг отдельной страницы
                        Experience = "Не указана", // В листинге опыта работы тоже нет
                        EmploymentType = "Не указана" // Эти данные также можно извлечь с подробной страницы вакансии
                    };

                    vacancies.Add(vacancy);
                }
            }
            return vacancies;
        }

        throw new Exception("Не удалось получить данные с Habr Career.");
    }
}
