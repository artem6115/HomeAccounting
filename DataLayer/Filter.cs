using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class Filter
    {
        public string? PropetryForSorting { get; set; }
        public DateTime? Date { get; set; }
        public long? AccountId { get; set; }
        public long? CategoryId { get; set; }
        [RegularExpression(@"^([=<>])?\d+(,\d{1,2})?")]
        public string Value { get; set; }
        public bool MoreValue { get; set; }
        public bool IsForward {get;set;}
        public int TypeTransaction { get; set; }
        public string? StringToFind { get; set; }
        public int PageNumber { get; set; }


    }
}