using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TractorBG.Entity
{
    /*
     * This is the Tractor entity which stores the information about the tractor that is 
     * going to be used and manipolated in different actions. It stores information like
     * brand model year etc.
     */
    [Table("Tractor")]
    public class Tractor
    {
        [Key]
        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string fileName { get; set; }
        public string description { get; set; }
    }
}
