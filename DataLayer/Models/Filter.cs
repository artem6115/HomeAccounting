using System.ComponentModel.DataAnnotations;
using DataLayer.Enum;

namespace DataLayer.Models
{
    public class Filter
    {
        //Свойство для сортировки
        public string? PropetryForSorting { get; set; }
        public DateTime? Date { get; set; }
        public long? AccountId { get; set; }
        public long? CategoryId { get; set; }

        //Выражение дложно быть числом и иметь не более 2 знаков после запятой
        [RegularExpression(@"^([=<>])?\d+(,\d{1,2})?")]
        public string? Value { get; set; }

        //Направление сортировки
        public bool IsForward { get; set; }
        public TypeTransaction TypeTransaction { get; set; } = 0;

        //Строка для поиска
        public string? StringToFind { get; set; }
        //Номер текущей страницы
        public int PageNumber { get; set; }


    }
}