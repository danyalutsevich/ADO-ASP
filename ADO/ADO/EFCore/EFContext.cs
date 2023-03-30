using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.EFCore
{
    public class EFContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedDepartments(modelBuilder);
            SeedProducts(modelBuilder);
            SeedManagers(modelBuilder);
            SeedSales(modelBuilder);

            modelBuilder.Entity<Manager>()
                .HasOne(m => m.MainDep)
                .WithMany(d => d.Workers)
                .HasForeignKey(m => m.Id_main_dep)
                .HasPrincipalKey(d => d.Id);

            modelBuilder.Entity<Manager>()
               .HasOne(m => m.SecDep)
               .WithMany(d => d.SecWorkers)
               .HasForeignKey(m => m.Id_sec_dep);

            modelBuilder.Entity<Sale>()
                 .HasOne(s => s.Product)
                 .WithMany(p => p.Sales)
                 .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Manager>()
                .HasMany(m => m.Products)
                .WithMany(p => p.Managers);
                
              
                

			// modelBuilder.Entity<Sale>()
			//     .HasOne(s => s.Manager)
			//     .WithMany(m => m.Sales);

		}

        #region Seed

        private void SeedSales(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>().HasData(new Sale { Id = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), ProductId = Guid.Parse("DA1E17BB-A90D-4C1B-9F1C-1B1F1B1F1B1F"), ManagerId = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), SaleDt = DateTime.Now, Count = 1 });

        }

        private void SeedDepartments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(new Department { Id = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Name = "IT отдел" });
            modelBuilder.Entity<Department>().HasData(new Department { Id = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Name = "Бухгалтерия" });
            modelBuilder.Entity<Department>().HasData(new Department { Id = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Name = "Служба безопасности" });
            modelBuilder.Entity<Department>().HasData(new Department { Id = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Name = "Отдел кадров" });
            modelBuilder.Entity<Department>().HasData(new Department { Id = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Name = "Канцелярия" });
            modelBuilder.Entity<Department>().HasData(new Department { Id = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Name = "Отдел продаж" });
            modelBuilder.Entity<Department>().HasData(new Department { Id = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Name = "Юридическая служба" });
        }
        private void SeedProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("DA1E17BB-A90D-4C79-B801-5462FB070F57"), Name = "Гвоздь 100мм", Price = 10.50 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("A8E6BE17-5447-4804-AB61-F31ABF5A76D3"), Name = "Шуруп 4х35", Price = 4.25 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("21B0F444-2E4F-47D8-80C1-E69BF1C34CA8"), Name = "Гайка М4", Price = 6.50 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("2DCA5E44-B06D-4613-BB6A-D3BC91430BFE"), Name = "Гровер М4", Price = 5.99 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("64A4DF8A-0733-4BE9-AABA-C01B4EC3612A"), Name = "Болт 4х60", Price = 9.98 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("B6D20749-B495-4B1A-BA1C-80B88E78B7CD"), Name = "Гвоздь 80мм", Price = 19.98 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("7B08197B-C55F-4389-891F-BF12A575DFFB"), Name = "Отвертка PZ2", Price = 35.50 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("870DA1A9-44F4-4018-B7FC-727A2058FAF0"), Name = "Шуруповерт", Price = 799 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("8FF90E21-DCDB-4D55-A557-7C6D57DBB029"), Name = "Молоток", Price = 216.50 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("F7F1E576-AF8D-4749-869E-4A794FE69D42"), Name = "Набор 'Новосел'", Price = 52.40 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("BB29F63D-1261-41F2-89E8-88F44D5EC409"), Name = "Сверло 6х80", Price = 39.98 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("D17A4442-0A71-4673-B450-36929048ADEF"), Name = "Шуруп 5х45", Price = 5.98 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("69B125D7-99CC-42D6-A6FA-46687F333749"), Name = "Винт 'потай' 3х16", Price = 3.98 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("94BC671A-A6B6-417A-BC9F-8AE4871A58EC"), Name = "Дюбель 6х60", Price = 5.50 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("EFC6578A-00B7-4766-A7E3-79CDBA8C294B"), Name = "Органайзер для шурупов", Price = 199 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("9654271B-AB52-4225-A30C-D75054B1733F"), Name = "Лазерный дальномер", Price = 1950 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("F2585221-1ACA-4EFE-A5E8-C2F4534D1F92"), Name = "Дрель электрическая", Price = 990 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("4A550D3B-D1F2-40EF-AE4E-963612C6713A"), Name = "Сварочный аппарат", Price = 2099 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("17DB11D1-F50E-4CF4-9C54-CF1BD45802EA"), Name = "Электроды 3мм", Price = 49.98 });
            modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.Parse("7264D33A-16B9-4E22-B3F1-63D6DAE60078"), Name = "Паяльник 40 Вт", Price = 199.98 });
        }

        private void SeedManagers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("743C93F2-4717-4E81-A093-69903476E176"), Surname = "Носков", Name = "Орест", Secname = "Ярославович", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("63531753-4D76-4A93-AD15-C727FFECA6AB"), Surname = "Никитин", Name = "Станислав", Secname = "Брониславович", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = Guid.Parse("3618D1D1-32DE-40B5-B823-9F82924A3CAF") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("CDE086E1-D25C-4251-A234-10727818EE28"), Surname = "Воронов", Name = "Александр", Secname = "Леонидович", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("0B2BE83A-7FB4-403B-8CE8-37BE257B038C"), Surname = "Евдокимов", Name = "Клим", Secname = "Викторович", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("7585D790-6E5A-4F73-A85C-4F9BD883D811"), Surname = "Жуков", Name = "Влад", Secname = "Виталиевич", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("45489FE7-86C8-4FA1-9D79-A82197566BF3"), Surname = "Кулагин", Name = "Максим", Secname = "Вадимович", Id_main_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_sec_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("0017AAAE-3E22-462D-9031-4276A9788D51"), Surname = "Журавлёв", Name = "Зигмунд", Secname = "Владимирович", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = Guid.Parse("FEA65EE4-A8A0-425B-8F11-3896C1E2197E") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("521C07BE-6FBD-411F-BCCB-93E2672BD50E"), Surname = "Соболев", Name = "Нестор", Secname = "Юхимович", Id_main_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("381C2888-1CB0-41FA-9650-48B953F31EF6"), Surname = "Беляков", Name = "Олег", Secname = "Грегориевич", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = null, Id_chief = Guid.Parse("663C3142-1C9D-4957-800D-F6C6824B9C88") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("E1AC29AD-122E-474D-926A-F93AC636F605"), Surname = "Моисеев", Name = "Конрад", Secname = "Леонидович", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_chief = Guid.Parse("3E229EB8-E99A-455F-8AF3-5871337A092C") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("39D57DFB-8DA7-49C9-AE8D-464509618F02"), Surname = "Гуляев", Name = "Семён", Secname = "Юхимович", Id_main_dep = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("542CB2C1-A8E3-42DB-97FA-B3C79B12A1A9"), Surname = "Назаров", Name = "Сергей", Secname = "Платонович", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("FE7E578E-5FC8-4D80-AD6B-500DDF2506C4"), Surname = "Рожков", Name = "Радислав", Secname = "Дмитриевич", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = Guid.Parse("7A88B1B9-0216-4259-8BA6-C123ABB3C6A8") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("7B8219FC-9FD2-431E-985C-7CAA6E9BD013"), Surname = "Герасимов", Name = "Лука", Secname = "Грегориевич", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_chief = Guid.Parse("3E229EB8-E99A-455F-8AF3-5871337A092C") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("23D52416-D994-4564-A106-1FDF5FECEF25"), Surname = "Куликов", Name = "Заур", Secname = "Иванович", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = Guid.Parse("23DBE38C-0ED4-4E90-8BC7-F168134E8674") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("EE860EE3-6CCA-4EA3-A2F1-FB79F4FC823A"), Surname = "Корнилов", Name = "Ярослав", Secname = "Романович", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = null, Id_chief = Guid.Parse("676D8ED4-8307-4196-9776-107C40C1DF84") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("DD860E7E-C2F0-47A6-BA29-165BE015E5A2"), Surname = "Князев", Name = "Клим", Secname = "Эдуардович", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("267F7528-2D4B-4063-A2C8-98E8F19FB6EE"), Surname = "Кириллов", Name = "Герасим", Secname = "Анатолиевич", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_chief = Guid.Parse("207CDCF2-89AD-49A5-A669-A082FA9CCCBA") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("FEA65EE4-A8A0-425B-8F11-3896C1E2197E"), Surname = "Галкин", Name = "Пётр", Secname = "Максимович", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("D13F3CCA-B9F8-4BC1-96F4-C80583928E55"), Surname = "Бородай", Name = "Люций", Secname = "Львович", Id_main_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_sec_dep = null, Id_chief = Guid.Parse("DC268B00-1727-4381-9878-6DA1BFEF2701") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("5FE63A0F-C1AE-44BE-9397-0F7DB0B95C1F"), Surname = "Спивак", Name = "Оливер", Secname = "Иванович", Id_main_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_sec_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_chief = Guid.Parse("29219DB8-16A0-4046-A7E1-6E455B0559CD") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("DC268B00-1727-4381-9878-6DA1BFEF2701"), Surname = "Ершов", Name = "Владлен", Secname = "Богданович", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = Guid.Parse("868F6394-3CA3-4700-90BB-6B73EC6719A7") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("2FA70965-5BCE-44F0-B6DD-2AF6072EB8B0"), Surname = "Комаров", Name = "Адриан", Secname = "Петрович", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("1166ECDD-63C8-42FC-A68A-C292176A7B04"), Surname = "Веселов", Name = "Роберт", Secname = "Евгеньевич", Id_main_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = Guid.Parse("C5F771FB-A645-4BA1-8155-F3F5002B2B89") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("0989E3A2-3D6D-4BC3-A538-C4055F9A09DD"), Surname = "Данилов", Name = "Добрыня", Secname = "Львович", Id_main_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_sec_dep = null, Id_chief = Guid.Parse("23DBE38C-0ED4-4E90-8BC7-F168134E8674") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("6CBEA09E-E3E4-4DD3-A6C5-ED9CCD986BC0"), Surname = "Журавлёв", Name = "Аким", Secname = "Петрович", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("676D8ED4-8307-4196-9776-107C40C1DF84"), Surname = "Ерёменко", Name = "Кристиан", Secname = "Евгеньевич", Id_main_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = Guid.Parse("7B8219FC-9FD2-431E-985C-7CAA6E9BD013") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("FF559AE5-64B6-459E-9771-CB36130B3B75"), Surname = "Туров", Name = "Станислав", Secname = "Михайлович", Id_main_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_sec_dep = null, Id_chief = Guid.Parse("435EEE28-E5EA-4EC9-9F01-DE884DFD6292") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("1A930DE7-647B-4A32-AD3B-0CAF4528B356"), Surname = "Шумейко", Name = "Абрам", Secname = "Романович", Id_main_dep = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("3618D1D1-32DE-40B5-B823-9F82924A3CAF"), Surname = "Бобылёв", Name = "Всеволод", Secname = "Ярославович", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("66034616-24E5-4E90-815F-476EB0CBB6B1"), Surname = "Гурьева", Name = "Антонина", Secname = "Евгеньевна", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = null, Id_chief = Guid.Parse("FEA65EE4-A8A0-425B-8F11-3896C1E2197E") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("C5F771FB-A645-4BA1-8155-F3F5002B2B89"), Surname = "Павлик", Name = "Ника", Secname = "Эдуардовна", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_chief = Guid.Parse("8939ED0C-BBDB-435E-923E-68158D2153C6") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("15F36ECC-EF25-495F-ADFF-169DB3339B88"), Surname = "Копылова", Name = "Екатерина", Secname = "Дмитриевна", Id_main_dep = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Id_sec_dep = null, Id_chief = Guid.Parse("05E31241-7274-43B5-8B59-9A62D725E54F") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("101BE2B1-C0AF-493E-BBF2-C8D8E4EB826C"), Surname = "Корнейчук", Name = "Нина", Secname = "Платоновна", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = Guid.Parse("2B3170C4-3063-43E6-985D-A38D9E45AF09") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("868F6394-3CA3-4700-90BB-6B73EC6719A7"), Surname = "Гордеева", Name = "Капитолина", Secname = "Станиславовна", Id_main_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_sec_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("05E31241-7274-43B5-8B59-9A62D725E54F"), Surname = "Майборода", Name = "Алёна", Secname = "Александровна", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = Guid.Parse("E1AC29AD-122E-474D-926A-F93AC636F605") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("1ADC048C-E346-47C3-8C35-7AD4FDAA6EB7"), Surname = "Шубина", Name = "Екатерина", Secname = "Викторовна", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("435EEE28-E5EA-4EC9-9F01-DE884DFD6292"), Surname = "Лазарева", Name = "Вера", Secname = "Евгеньевна", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("0889C51E-7728-4ABD-9987-3588D48B54A9"), Surname = "Кобзар", Name = "Полина", Secname = "Львовна", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Id_chief = Guid.Parse("542CB2C1-A8E3-42DB-97FA-B3C79B12A1A9") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("46D73A48-3906-44F4-A4B4-E29F1CC40B4F"), Surname = "Милославска", Name = "Инна", Secname = "Эдуардовна", Id_main_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_sec_dep = null, Id_chief = Guid.Parse("435EEE28-E5EA-4EC9-9F01-DE884DFD6292") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("EFEF5433-7E26-43A3-A737-3BB032D7D88A"), Surname = "Степанова", Name = "Нина", Secname = "Михайловна", Id_main_dep = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Id_sec_dep = null, Id_chief = Guid.Parse("63531753-4D76-4A93-AD15-C727FFECA6AB") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("55FF549E-1489-4B4A-9482-B843CD70C546"), Surname = "Ялова", Name = "Любовь", Secname = "Ивановна", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("79679ED4-0CCD-480A-8D5B-4A68287DE6C4"), Surname = "Макарова", Name = "Полина", Secname = "Васильевна", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = null, Id_chief = Guid.Parse("0B2BE83A-7FB4-403B-8CE8-37BE257B038C") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("29219DB8-16A0-4046-A7E1-6E455B0559CD"), Surname = "Дементьева", Name = "Альбина", Secname = "Ивановна", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("13DED219-A580-4FF8-8211-90A408B0AFA6"), Surname = "Егорова", Name = "Ярослава", Secname = "Романовна", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = null, Id_chief = Guid.Parse("1166ECDD-63C8-42FC-A68A-C292176A7B04") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("2B3170C4-3063-43E6-985D-A38D9E45AF09"), Surname = "Коваленко", Name = "Ольга", Secname = "Владимировна", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("3E229EB8-E99A-455F-8AF3-5871337A092C"), Surname = "Белоусова", Name = "Валерия", Secname = "Петровна", Id_main_dep = Guid.Parse("131EF84B-F06E-494B-848F-BB4BC0604266"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("5319FD22-9BDE-48E5-819D-FE884B70AFD8"), Surname = "Бердник", Name = "Ирина", Secname = "Ивановна", Id_main_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_sec_dep = null, Id_chief = Guid.Parse("39D57DFB-8DA7-49C9-AE8D-464509618F02") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("8939ED0C-BBDB-435E-923E-68158D2153C6"), Surname = "Красинец", Name = "Нелли", Secname = "Ярославовна", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = null, Id_chief = Guid.Parse("743C93F2-4717-4E81-A093-69903476E176") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("663C3142-1C9D-4957-800D-F6C6824B9C88"), Surname = "Баранова", Name = "Флорентина", Secname = "Брониславовна", Id_main_dep = Guid.Parse("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94"), Id_sec_dep = null, Id_chief = Guid.Parse("0017AAAE-3E22-462D-9031-4276A9788D51") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("239450EB-A92F-4093-A74F-EAA38F8ADBE2"), Surname = "Толочко", Name = "Анжелика", Secname = "Борисовна", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = null, Id_chief = Guid.Parse("23D52416-D994-4564-A106-1FDF5FECEF25") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("23DBE38C-0ED4-4E90-8BC7-F168134E8674"), Surname = "Родионова", Name = "Эльвира", Secname = "Фёдоровна", Id_main_dep = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Id_sec_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_chief = Guid.Parse("3E229EB8-E99A-455F-8AF3-5871337A092C") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("7A88B1B9-0216-4259-8BA6-C123ABB3C6A8"), Surname = "Трясило", Name = "Инга", Secname = "Артёмовна", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("789A53AB-A54D-4AF7-94A5-DD288428A37C"), Surname = "Гуляева", Name = "Клара", Secname = "Даниловна", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = null, Id_chief = Guid.Parse("DC268B00-1727-4381-9878-6DA1BFEF2701") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("A93A1B20-155A-43BD-ACEE-87A6088C969E"), Surname = "Исаева", Name = "Марта", Secname = "Борисовна", Id_main_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("E56F5DE6-A1D3-4C3E-A09A-A9B9FA96C9B3"), Surname = "Одинцова", Name = "Зинаида", Secname = "Евгеньевна", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = Guid.Parse("D2469412-0E4B-46F7-80EC-8C522364D099"), Id_chief = Guid.Parse("DD860E7E-C2F0-47A6-BA29-165BE015E5A2") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("207CDCF2-89AD-49A5-A669-A082FA9CCCBA"), Surname = "Соловьёва", Name = "Флорентина", Secname = "Виталиевна", Id_main_dep = Guid.Parse("1EF7268C-43A8-488C-B761-90982B31DF4E"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("C5EE780A-4D53-40FB-A592-C35CFC9455F2"), Surname = "Мирна", Name = "Рада", Secname = "Сергеевна", Id_main_dep = Guid.Parse("8DCC3969-1D93-47A9-8B79-A30C738DB9B4"), Id_sec_dep = null, Id_chief = null });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("D3FCC76B-09A2-4578-A72C-34468DA36C45"), Surname = "Одинцова", Name = "Мальвина", Secname = "Дмитриевна", Id_main_dep = Guid.Parse("624B3BB5-0F2C-42B6-A416-099AAB799546"), Id_sec_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_chief = Guid.Parse("1A930DE7-647B-4A32-AD3B-0CAF4528B356") });
            modelBuilder.Entity<Manager>().HasData(new Manager { Id = Guid.Parse("6FB5BCA3-2CAE-4450-AAB5-E0184FD45BE9"), Surname = "Ткаченко", Name = "Альбина", Secname = "Викторовна", Id_main_dep = Guid.Parse("415B36D9-2D82-4A92-A313-48312F8E18C6"), Id_sec_dep = null, Id_chief = null });
        }
        #endregion
    }
}
