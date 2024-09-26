namespace CareerBot.Api;

public sealed class VacancyAggregator
{
    /// <summary>
    /// Агрегация вакансий из разных источников (hh.ru и career.habr.com).
    /// </summary>
    /// <param name="searchQuery">Строка поиска вакансий.</param>
    /// <returns>Общий список вакансий из всех источников.</returns>
    public static async Task<List<Vacancy>> GetAllVacancies(string searchQuery)
    {
        // Получаем вакансии с hh.ru
        List<Vacancy> hhVacancies = await HHScraper.GetVacanciesFromApi(searchQuery);

        // Получаем вакансии с Habr Career
        List<Vacancy> habrVacancies = await HabrCareerScraper.GetVacanciesFromHabrCareer(searchQuery);

        // Объединяем списки
        List<Vacancy> allVacancies = [.. hhVacancies, .. habrVacancies];

        return allVacancies;
    }
}
