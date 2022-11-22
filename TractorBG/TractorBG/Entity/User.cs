using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TractorBG.Entity
{
    /*
     * This is the base entity which stores the information about the users
     * Here is stored information like his username password wether hes admin or not and 
     * his id.
     */
    [Table("User")]
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int admin { get; set; }
    }
}
