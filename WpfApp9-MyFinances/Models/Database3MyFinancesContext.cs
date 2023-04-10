using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfApp9_MyFinances.Models;

public partial class Database3MyFinancesContext : DbContext
{
    public Database3MyFinancesContext()
    {
    }

    public Database3MyFinancesContext(DbContextOptions<Database3MyFinancesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriesExp> CategoriesExps { get; set; }

    public virtual DbSet<CategoriesInc> CategoriesIncs { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<SubcategoriesExp> SubcategoriesExps { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=FENRIR-PC\\SQLEXPRESS;Initial Catalog=Database3-MyFinances;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<CategoriesExp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A669E18F");

            entity.ToTable("CategoriesExp");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DCCAF2D334").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<CategoriesInc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A9BE3C22");

            entity.ToTable("CategoriesInc");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DC4F799279").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC07D163EB07");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("Trigger1-OnExpenseTransaction");
                    tb.HasTrigger("Trigger2-OnExpenseTransactionUpdate");
                    tb.HasTrigger("Trigger3-OnExpenseTransactionDelete");
                });

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DateOfExpense)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.SubCategoryTitle).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Catego__4BAC3F29");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Paymen__49C3F6B7");

            entity.HasOne(d => d.Provider).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Expenses__Provid__4AB81AF0");

            entity.HasOne(d => d.SubcategoriesExp).WithMany(p => p.Expenses)
                .HasForeignKey(d => new { d.CategoryId, d.SubCategoryTitle })
                .HasConstraintName("FK__Expenses__4CA06362");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incomes__3214EC07256B1FC9");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("Trigger4-OnIncomeTransactionInsert");
                    tb.HasTrigger("Trigger5-OnIncomeTransactionUpdate");
                    tb.HasTrigger("Trigger6-OnIncomeTransactionDelete");
                });

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DateOfIncome)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incomes__Categor__4F7CD00D");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incomes__Payment__4D94879B");

            entity.HasOne(d => d.Provider).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Incomes__Provide__4E88ABD4");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC073645C299");

            entity.Property(e => e.CurrentBalance).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(20);
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC07D06AD6E6");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<SubcategoriesExp>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.Title }).HasName("PK__Subcateg__DBC25C46D925133B");

            entity.ToTable("SubcategoriesExp");

            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Category).WithMany(p => p.SubcategoriesExps)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subcatego__Categ__5070F446");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transfer__3214EC07FE1AAE1A");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("Trigger7-OnTransferTransactionInsert");
                    tb.HasTrigger("Trigger8-OnTransferTransactionUpdate");
                    tb.HasTrigger("Trigger9-OnTransferTransactionDelete");
                });

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DateOfTransfer)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);

            entity.HasOne(d => d.From).WithMany(p => p.TransferFroms)
                .HasForeignKey(d => d.FromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__FromI__5165187F");

            entity.HasOne(d => d.To).WithMany(p => p.TransferTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__ToId__52593CB8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
