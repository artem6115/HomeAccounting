﻿namespace DataLayer
{
    public class Filter
    {
        public string? PropetryForSorting { get; set; }
        public DateTime? Date { get; set; }
        public long? AccountId { get; set; }
        public long? CategoryId { get; set; }
        public double? Value { get; set; }
        public bool IsForward {get;set;}
    }
}