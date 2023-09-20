using System;
using System.Collections.Generic;
using APIV2.Models;
using Microsoft.EntityFrameworkCore;

namespace APIV2.Data;

public partial class GambleonContext : DbContext
{
    public GambleonContext()
    {
    }

    public GambleonContext(DbContextOptions<GambleonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<BettingGame> BettingGames { get; set; }

    public virtual DbSet<BettingHistory> BettingHistories { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameType> GameTypes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-OV8UPS7;Database=gambleon2;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3213E83FF351F388");

            entity.ToTable("Address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address1)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.PostalCode).HasColumnName("postalCode");
        });

        modelBuilder.Entity<BettingGame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BettingG__3213E83F8FF9B4D6");

            entity.ToTable("BettingGame");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GameId).HasColumnName("gameId");
            entity.Property(e => e.PlannedTime)
                .HasColumnType("datetime")
                .HasColumnName("plannedTime");
            entity.Property(e => e.WinnerId).HasColumnName("winnerId");

            entity.HasOne(d => d.Game).WithMany(p => p.BettingGames)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_BettingGame.gameId");
        });

        modelBuilder.Entity<BettingHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BettingH__3213E83F2E3FCCBE");

            entity.ToTable("BettingHistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BettingAmount).HasColumnName("bettingAmount");
            entity.Property(e => e.BettingGameId).HasColumnName("bettingGameId");
            entity.Property(e => e.BettingResult).HasColumnName("bettingResult");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdTime");
            entity.Property(e => e.Outcome).HasColumnName("outcome");
            entity.Property(e => e.WalletId).HasColumnName("walletId");

            entity.HasOne(d => d.BettingGame).WithMany(p => p.BettingHistories)
                .HasForeignKey(d => d.BettingGameId)
                .HasConstraintName("FK_BettingHistory.bettingGameId");

            entity.HasOne(d => d.Wallet).WithMany(p => p.BettingHistories)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_BettingHistory.walletId");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Characte__3213E83F33487E14");

            entity.ToTable("Character");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GameId).HasColumnName("gameId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Odds).HasColumnName("odds");

            entity.HasOne(d => d.Game).WithMany(p => p.Characters)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Character.gameId");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Game__3213E83F863394A0");

            entity.ToTable("Game");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Desc)
                .HasMaxLength(1023)
                .HasColumnName("desc");
            entity.Property(e => e.GameImage)
                .HasMaxLength(255)
                .HasColumnName("gameImage");
            entity.Property(e => e.GameTypeId).HasColumnName("gameTypeId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.GameType).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameTypeId)
                .HasConstraintName("FK_Game.gameTypeId");
        });

        modelBuilder.Entity<GameType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameType__3213E83FFE434C08");

            entity.ToTable("GameType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GameType1)
                .HasMaxLength(255)
                .HasColumnName("gameType");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3213E83F96BE294E");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("actionTime");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.WalletId).HasColumnName("walletId");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_Transaction.walletId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F4BAA6B19");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E61644E0E7151").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.AddressId).HasColumnName("addressId");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("lastName");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .HasColumnName("passwordHash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(500)
                .HasColumnName("passwordSalt");
            entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
            entity.Property(e => e.UserTypeId).HasColumnName("userTypeId");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Address).WithMany(p => p.Users)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_User.addressId");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .HasConstraintName("FK_User.userTypeId");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserType__3213E83F3FA30001");

            entity.ToTable("UserType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserType1)
                .HasMaxLength(255)
                .HasColumnName("userType");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3213E83F462EBCA8");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Wallet.userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
