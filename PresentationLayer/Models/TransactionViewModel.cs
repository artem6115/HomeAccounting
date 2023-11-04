﻿using DataLayer.Entities;
using DataLayer;
using DataLayer.Models;



namespace PresentationLayer.Models
{
    public class TransactionViewModel
    {
        public List<AccountViewModel> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public Filter Filter { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int NumberOfLastPage { get; set; }
        public string Url { get; set; }
        public string Sum { get; set; }
        
    }
}
