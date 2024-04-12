using System.ComponentModel.DataAnnotations;
using System.Configuration;

// Model for entrymethods which links to the order table
namespace WinterIntex.Models
{
    public class EntryMethods
    {
        [Key]
        public int Entry_Code {  get; set; }
        public string Entry_Description { get; set; }
    }
}
