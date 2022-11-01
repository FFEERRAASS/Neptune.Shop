using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NewwProject.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AboutusFp> AboutusFps { get; set; }
        public virtual DbSet<CategoryFp> CategoryFps { get; set; }
        public virtual DbSet<ConmsgFp> ConmsgFps { get; set; }
        public virtual DbSet<ContactusFp> ContactusFps { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<FavouriteFp> FavouriteFps { get; set; }
        public virtual DbSet<HandcraftFp> HandcraftFps { get; set; }
        public virtual DbSet<HomeFp> HomeFps { get; set; }
        public virtual DbSet<NewsOffer> NewsOffers { get; set; }
        public virtual DbSet<OrdersFp> OrdersFps { get; set; }
        public virtual DbSet<OrdersdoneFp> OrdersdoneFps { get; set; }
        public virtual DbSet<ReviewFp> ReviewFps { get; set; }
        public virtual DbSet<RoleFp> RoleFps { get; set; }
        public virtual DbSet<UserFp> UserFps { get; set; }
        public virtual DbSet<UserloginFp> UserloginFps { get; set; }
        public virtual DbSet<VendorsalesFp> VendorsalesFps { get; set; }
        public virtual DbSet<VisaFp> VisaFps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("USER ID=JOR16_User16;PASSWORD=feras124949;DATA SOURCE=94.56.229.181:3488/traindb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("JOR16_USER16")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<AboutusFp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ABOUTUS_FP");

                entity.Property(e => e.About)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("ABOUT");

                entity.Property(e => e.AboutDate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ABOUT_DATE");

                entity.Property(e => e.IconPath)
                    .HasMaxLength(240)
                    .IsUnicode(false)
                    .HasColumnName("ICON_PATH");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(240)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");
            });

            modelBuilder.Entity<CategoryFp>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("SYS_C00282966");

                entity.ToTable("CATEGORY_FP");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.CategoryImage)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORY_IMAGE");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORY_NAME");
            });

            modelBuilder.Entity<ConmsgFp>(entity =>
            {
                entity.HasKey(e => e.ContId)
                    .HasName("SYS_C00297025");

                entity.ToTable("CONMSG_FP");

                entity.Property(e => e.ContId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CONT_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FullName)
                    .HasMaxLength(155)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.Msg)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MSG");

                entity.Property(e => e.Subject)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("SUBJECT");
            });

            modelBuilder.Entity<ContactusFp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CONTACTUS_FP");

                entity.Property(e => e.Copyright)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("COPYRIGHT");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.IconPath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ICON_PATH");

                entity.Property(e => e.Locations)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LOCATIONS");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.Socialmedia)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("SOCIALMEDIA");
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.ToTable("EMAIL");

                entity.Property(e => e.EmailId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("EMAIL_ID");

                entity.Property(e => e.EmailMsg)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL_MSG");

                entity.Property(e => e.EmailSubject)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL_SUBJECT");

                entity.Property(e => e.SenderReciver)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SENDER_RECIVER");

                entity.Property(e => e.UserFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_FK");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithMany(p => p.Emails)
                    .HasForeignKey(d => d.UserFk)
                    .HasConstraintName("FK_USER01");
            });

            modelBuilder.Entity<FavouriteFp>(entity =>
            {
                entity.HasKey(e => e.FavId)
                    .HasName("SYS_C00294538");

                entity.ToTable("FAVOURITE_FP");

                entity.Property(e => e.FavId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FAV_ID");

                entity.Property(e => e.Handfk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HANDFK");

                entity.Property(e => e.Userfk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USERFK");

                entity.HasOne(d => d.HandfkNavigation)
                    .WithMany(p => p.FavouriteFps)
                    .HasForeignKey(d => d.Handfk)
                    .HasConstraintName("FK_HANDS");

                entity.HasOne(d => d.UserfkNavigation)
                    .WithMany(p => p.FavouriteFps)
                    .HasForeignKey(d => d.Userfk)
                    .HasConstraintName("FK_USERS");
            });

            modelBuilder.Entity<HandcraftFp>(entity =>
            {
                entity.HasKey(e => e.HandId)
                    .HasName("SYS_C00281407");

                entity.ToTable("HANDCRAFT_FP");

                entity.Property(e => e.HandId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("HAND_ID");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.Descriptions)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTIONS");

                entity.Property(e => e.Handcraft)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HANDCRAFT");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Price)
                    .HasColumnType("FLOAT")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Sale)
                    .HasColumnType("FLOAT")
                    .HasColumnName("SALE");

                entity.Property(e => e.UserFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_FK");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.HandcraftFps)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("SYS_C00282969");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithMany(p => p.HandcraftFps)
                    .HasForeignKey(d => d.UserFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USER3");
            });

            modelBuilder.Entity<HomeFp>(entity =>
            {
                entity.HasKey(e => e.HomeId)
                    .HasName("SYS_C00297044");

                entity.ToTable("HOME_FP");

                entity.Property(e => e.HomeId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("HOME_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Footerparagraph2)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("FOOTERPARAGRAPH2");

                entity.Property(e => e.Footerparagraph3)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("FOOTERPARAGRAPH3");

                entity.Property(e => e.Homeparagraph1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPARAGRAPH1");

                entity.Property(e => e.Homeparagraph2)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPARAGRAPH2");

                entity.Property(e => e.Homeparagraph3)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPARAGRAPH3");

                entity.Property(e => e.Location1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION1");

                entity.Property(e => e.Location2)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION2");

                entity.Property(e => e.Photo1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO1");

                entity.Property(e => e.Photo2)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO2");

                entity.Property(e => e.Photo3)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO3");

                entity.Property(e => e.Photo4)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO4");

                entity.Property(e => e.Photo5)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO5");

                entity.Property(e => e.Photo6)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO6");

                entity.Property(e => e.Photo7)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO7");
            });

            modelBuilder.Entity<NewsOffer>(entity =>
            {
                entity.HasKey(e => e.OffersId)
                    .HasName("SYS_C00297053");

                entity.ToTable("NEWS_OFFERS");

                entity.Property(e => e.OffersId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("OFFERS_ID");

                entity.Property(e => e.Homeparagraph1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPARAGRAPH1");

                entity.Property(e => e.Homeparagraph2)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPARAGRAPH2");
            });

            modelBuilder.Entity<OrdersFp>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("SYS_C00281418");

                entity.ToTable("ORDERS_FP");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.HandcraftFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HANDCRAFT_FK");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.UserFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_FK");

                entity.HasOne(d => d.HandcraftFkNavigation)
                    .WithMany(p => p.OrdersFps)
                    .HasForeignKey(d => d.HandcraftFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HANDCRAFT");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithMany(p => p.OrdersFps)
                    .HasForeignKey(d => d.UserFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USER4");
            });

            modelBuilder.Entity<OrdersdoneFp>(entity =>
            {
                entity.HasKey(e => e.Odone)
                    .HasName("SYS_C00281422");

                entity.ToTable("ORDERSDONE_FP");

                entity.Property(e => e.Odone)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ODONE");

                entity.Property(e => e.DateShop)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_SHOP");

                entity.Property(e => e.HandcraftFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HANDCRAFT_FK");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.UserFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_FK");

                entity.HasOne(d => d.HandcraftFkNavigation)
                    .WithMany(p => p.OrdersdoneFps)
                    .HasForeignKey(d => d.HandcraftFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HANDCRAFT1");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithMany(p => p.OrdersdoneFps)
                    .HasForeignKey(d => d.UserFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USER5");
            });

            modelBuilder.Entity<ReviewFp>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("SYS_C00281400");

                entity.ToTable("REVIEW_FP");

                entity.Property(e => e.ReviewId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("REVIEW_ID");

                entity.Property(e => e.Checks)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CHECKS");

                entity.Property(e => e.Rate)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RATE");

                entity.Property(e => e.Review)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REVIEW");

                entity.Property(e => e.UserFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_FK");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithMany(p => p.ReviewFps)
                    .HasForeignKey(d => d.UserFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USER2");
            });

            modelBuilder.Entity<RoleFp>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("SYS_C00281379");

                entity.ToTable("ROLE_FP");

                entity.Property(e => e.RoleId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ROLE_ID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ROLE_NAME");
            });

            modelBuilder.Entity<UserFp>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("SYS_C00281384");

                entity.ToTable("USER_FP");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("USER_ID");

                entity.Property(e => e.CraftName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CRAFT_NAME");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.Gender)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GENDER");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LAST_NAME");

                entity.Property(e => e.RoleFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_FK");

                entity.HasOne(d => d.RoleFkNavigation)
                    .WithMany(p => p.UserFps)
                    .HasForeignKey(d => d.RoleFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ROLE");
            });

            modelBuilder.Entity<UserloginFp>(entity =>
            {
                entity.HasKey(e => e.UloginId)
                    .HasName("SYS_C00281387");

                entity.ToTable("USERLOGIN_FP");

                entity.HasIndex(e => e.Username, "SYS_C00281388")
                    .IsUnique();

                entity.Property(e => e.UloginId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ULOGIN_ID");

                entity.Property(e => e.IsAccepted)
                    .HasColumnType("NUMBER")
                    .HasColumnName("IS_ACCEPTED");

                entity.Property(e => e.RoleFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_FK");

                entity.Property(e => e.UserFk)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_FK");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USER_PASSWORD");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");

                entity.HasOne(d => d.RoleFkNavigation)
                    .WithMany(p => p.UserloginFps)
                    .HasForeignKey(d => d.RoleFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ROLE0");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithMany(p => p.UserloginFps)
                    .HasForeignKey(d => d.UserFk)
                    .HasConstraintName("FK_USER");
            });

            modelBuilder.Entity<VendorsalesFp>(entity =>
            {
                entity.HasKey(e => e.VsalesId)
                    .HasName("SYS_C00282968");

                entity.ToTable("VENDORSALES_FP");

                entity.Property(e => e.VsalesId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("VSALES_ID");

                entity.Property(e => e.Sales)
                    .HasColumnType("FLOAT")
                    .HasColumnName("SALES");
            });

            modelBuilder.Entity<VisaFp>(entity =>
            {
                entity.HasKey(e => e.VisaId)
                    .HasName("SYS_C00281395");

                entity.ToTable("VISA_FP");

                entity.Property(e => e.VisaId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("VISA_ID");

                entity.Property(e => e.Balance)
                    .HasColumnType("FLOAT")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.Cvv)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("CVV");

                entity.Property(e => e.ExpiredDate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EXPIRED_DATE");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IBAN");
            });

            modelBuilder.HasSequence("RESTAURANT").IncrementsBy(2);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
