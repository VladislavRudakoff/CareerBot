namespace CareerBot.Api.Models;

/// <summary>
/// Вакансия
/// </summary>
public class Vacancy
{
    /// <summary>
    /// Название вакансии.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Название компании.
    /// </summary>
    public string Company { get; set; }

    /// <summary>
    /// Зарплата (диапазон или фиксированная сумма).
    /// </summary>
    public string Salary { get; set; }

    /// <summary>
    /// Местоположение вакансии.
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Дата публикации вакансии.
    /// </summary>
    public DateTime PublishedDate { get; set; }

    /// <summary>
    /// Требования к кандидатам.
    /// </summary>
    public string Requirements { get; set; }

    /// <summary>
    /// Основные обязанности на данной должности.
    /// </summary>
    public string Responsibilities { get; set; }

    /// <summary>
    /// Тип занятости (полная занятость, частичная, удалённая работа и т.д.).
    /// </summary>
    public string EmploymentType { get; set; }

    /// <summary>
    /// Полное описание вакансии.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Требуемый опыт работы.
    /// </summary>
    public string Experience { get; set; }

    /// <summary>
    /// Прямая ссылка на вакансию.
    /// </summary>
    public string VacancyUrl { get; set; }

    /// <summary>
    /// Уникальный идентификатор вакансии.
    /// </summary>
    public string VacancyId { get; set; }

    /// <summary>
    /// Контактная информация работодателя.
    /// </summary>
    public string ContactInfo { get; set; }

    /// <summary>
    /// Дополнительные условия (например, бонусы, соцпакет и т.д.).
    /// </summary>
    public string AdditionalConditions { get; set; }
}

