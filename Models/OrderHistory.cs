using SQLite;
using System;

namespace MauiAppBookPJ.Models
{
    public class OrderHistory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string BookTitles { get; set; } 
        public string PdfPath { get; set; }     
        public DateTime CreatedAt { get; set; } 
    }
}
