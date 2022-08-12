using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace TestApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using (var db = new TestContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Master m = new Master();
                m.ID = Guid.NewGuid();
                m.Name = "Master with details";
                db.Set<Master>().Add(m);
                Detail d = new Detail();
                d.ID = Guid.NewGuid();
                d.Name = "Detail";
                d.Master = m;
                db.Set<Detail>().Add(d);

                m = new Master();
                m.ID = Guid.NewGuid();
                m.Name = "Master without details";
                db.Set<Master>().Add(m);

                db.SaveChanges();
            }
            using (var db = new TestContext())
            {
                var r =db.Set<Master>().Select(itm => itm.Details.First()).ToArray();
            }
        }
    }



    public class TestContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=DESKTOP-J3I3K6R;Integrated Security=true;Initial Catalog=EFFirst");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Master>().HasKey(itm => itm.ID);

            modelBuilder.Entity<Detail>().HasKey(itm => itm.ID);
            modelBuilder.Entity<Detail>().HasOne(itm => itm.Master).WithMany(itm => itm.Details);

        }

    }


    public class Master
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Detail> Details { get; set; }
    }

    public class Detail
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public virtual Master Master { get; set; }
    }

}


