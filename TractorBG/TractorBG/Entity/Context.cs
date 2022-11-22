using System.Data.Entity;

namespace TractorBG.Entity
{
    /*
     * Class Context is the class that implements the database into the application
     * The class inherits DbContext so we can use functions like add , remove , 
     * find and others.
     */
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tractor> Tractors { get; set; }

        /*
         * This is the primary constructor that connects the database to the application
         * The base implements the connection string 
         * Inside the constructor Users and Tractors set the database tables
         */
        public Context() : base("Server = localhost\\sqlexpress; Database=TractorBG;Trusted_Connection=True;")
        {
            Users = this.Set<User>();
            Tractors = this.Set<Tractor>();
        }
    }
}
