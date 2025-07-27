using Microsoft.EntityFrameworkCore;
using BookExchange.Domain.Core; // Para Student
using BookExchange.Domain.Entities; // Para Book, Subject, ExchangeOffer, Message, Review

namespace BookExchange.Infrastructure.Context
{
    public class BookExchangeContext : DbContext
    {
        public BookExchangeContext(DbContextOptions<BookExchangeContext> options)
            : base(options)
        {
        }

        // DbSets para tus entidades
        public DbSet<Student> Students { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ExchangeOffer> ExchangeOffers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones específicas de Fluent API (opcional pero recomendado para control)

            // Configuración para Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students"); // Nombre de la tabla
                entity.HasKey(e => e.Id); // Clave primaria
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique(); // El email debe ser único
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.RegistrationDate).IsRequired().HasDefaultValueSql("GETDATE()"); // Valor por defecto

                // Relaciones de Student
                entity.HasMany(s => s.BooksPublished)
                      .WithOne(b => b.Owner)
                      .HasForeignKey(b => b.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict); // No eliminar libros si se elimina el estudiante

                entity.HasMany(s => s.ExchangeOffers)
                      .WithOne(eo => eo.Seller)
                      .HasForeignKey(eo => eo.SellerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relaciones para Mensajes (Remitente y Receptor)
                entity.HasMany(s => s.SentMessages)
                      .WithOne(m => m.Sender)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.ReceivedMessages)
                      .WithOne(m => m.Receiver)
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relaciones para Reviews (Quien da y quien recibe)
                entity.HasMany(s => s.GivenReviews)
                      .WithOne(r => r.Reviewer)
                      .HasForeignKey(r => r.ReviewerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.ReceivedReviews)
                      .WithOne(r => r.ReviewedUser)
                      .HasForeignKey(r => r.ReviewedUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Book
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(250);
                entity.Property(e => e.ISBN).IsRequired().HasMaxLength(13);
                entity.HasIndex(e => e.ISBN).IsUnique(); // ISBN debería ser único
                entity.Property(e => e.Condition).IsRequired();

                // Relación con Subject (Many-to-One)
                entity.HasOne(b => b.Subject)
                      .WithMany(s => s.Books)
                      .HasForeignKey(b => b.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict); // No eliminar materia si hay libros asociados

                // Relación con Owner (Many-to-One) - Ya configurada desde Student
                // entity.HasOne(b => b.Owner)
                //       .WithMany(s => s.BooksPublished)
                //       .HasForeignKey(b => b.OwnerId)
                //       .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Subject
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique(); // Nombre de materia único
            });

            // Configuración para ExchangeOffer
            modelBuilder.Entity<ExchangeOffer>(entity =>
            {
                entity.ToTable("ExchangeOffers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)"); // Configura tipo decimal para precisión
                entity.Property(e => e.PostedDate).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Status).IsRequired();

                // Relación con Book (One-to-Many, una oferta para un libro)
                entity.HasOne(eo => eo.Book)
                      .WithMany(b => b.ExchangeOffers)
                      .HasForeignKey(eo => eo.BookId)
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra el libro, se borran las ofertas asociadas

                // Relación con Seller (Many-to-One) - Ya configurada desde Student
                // entity.HasOne(eo => eo.Seller)
                //       .WithMany(s => s.ExchangeOffers)
                //       .HasForeignKey(eo => eo.SellerId)
                //       .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Message
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Messages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.SentDate).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsRead).HasDefaultValue(false);

                // Relaciones con Sender y Receiver (Many-to-One) - Ya configuradas desde Student
                // entity.HasOne(m => m.Sender)
                //       .WithMany(s => s.SentMessages)
                //       .HasForeignKey(m => m.SenderId)
                //       .OnDelete(DeleteBehavior.Restrict);

                // entity.HasOne(m => m.Receiver)
                //       .WithMany(s => s.ReceivedMessages)
                //       .HasForeignKey(m => m.ReceiverId)
                //       .OnDelete(DeleteBehavior.Restrict);

                // Relación con ExchangeOffer (Optional Many-to-One)
                entity.HasOne(m => m.ExchangeOffer)
                      .WithMany(eo => eo.RelatedMessages)
                      .HasForeignKey(m => m.ExchangeOfferId)
                      .IsRequired(false) // Permite que ExchangeOfferId sea nulo
                      .OnDelete(DeleteBehavior.SetNull); // Si se borra la oferta, ExchangeOfferId en mensaje se vuelve nulo
            });

            // Configuración para Review
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Comment).HasMaxLength(500); // Opcional
                entity.Property(e => e.ReviewDate).IsRequired().HasDefaultValueSql("GETDATE()");

                // Relaciones con Reviewer y ReviewedUser (Many-to-One) - Ya configuradas desde Student
                // entity.HasOne(r => r.Reviewer)
                //       .WithMany(s => s.GivenReviews)
                //       .HasForeignKey(r => r.ReviewerId)
                //       .OnDelete(DeleteBehavior.Restrict);

                // entity.HasOne(r => r.ReviewedUser)
                //       .WithMany(s => s.ReceivedReviews)
                //       .HasForeignKey(r => r.ReviewedUserId)
                //       .OnDelete(DeleteBehavior.Restrict);

                // Relación con ExchangeOffer (Optional Many-to-One)
                entity.HasOne(r => r.ExchangeOffer)
                      .WithMany() // No necesitamos una colección de Reviews en ExchangeOffer si solo es un link directo
                      .HasForeignKey(r => r.ExchangeOfferId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuración de enums a strings para mayor legibilidad en la base de datos (opcional)
            modelBuilder.Entity<Book>()
                .Property(b => b.Condition)
                .HasConversion<string>();

            modelBuilder.Entity<ExchangeOffer>()
                .Property(eo => eo.Type)
                .HasConversion<string>();

            modelBuilder.Entity<ExchangeOffer>()
                .Property(eo => eo.Status)
                .HasConversion<string>();

            // Sembrar datos iniciales (Seed Data) - Opcional para pruebas
            // modelBuilder.Entity<Subject>().HasData(
            //     new Subject { Id = 1, Name = "Matemáticas", Description = "Cálculo, Álgebra, Geometría" },
            //     new Subject { Id = 2, Name = "Literatura", Description = "Novela, Poesía, Drama" },
            //     new Subject { Id = 3, Name = "Ciencias de la Computación", Description = "Programación, Algoritmos, Estructuras de Datos" }
            // );
        }
    }
}
