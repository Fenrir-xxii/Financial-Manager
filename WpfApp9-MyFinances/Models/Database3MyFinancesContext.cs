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
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07F74A0F0E");

            entity.ToTable("CategoriesExp");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DC5C121685").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<CategoriesInc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0723053F5F");

            entity.ToTable("CategoriesInc");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DCFDA91408").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC076AA04B1B");

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
                .HasConstraintName("FK__Expenses__Catego__4AB81AF0");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Paymen__48CFD27E");

            entity.HasOne(d => d.Provider).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Expenses__Provid__49C3F6B7");

            entity.HasOne(d => d.SubcategoriesExp).WithMany(p => p.Expenses)
                .HasForeignKey(d => new { d.CategoryId, d.SubCategoryTitle })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__4BAC3F29");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incomes__3214EC0718D26BCB");

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
                .HasConstraintName("FK__Incomes__Categor__4E88ABD4");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incomes__Payment__4CA06362");

            entity.HasOne(d => d.Provider).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Incomes__Provide__4D94879B");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC07155E07DE");

            entity.Property(e => e.CurrentBalance).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(20);
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC07B5DC62C2");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<SubcategoriesExp>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.Title }).HasName("PK__Subcateg__DBC25C4666676421");

            entity.ToTable("SubcategoriesExp");

            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Category).WithMany(p => p.SubcategoriesExps)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subcatego__Categ__4F7CD00D");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transfer__3214EC07B95BE3DA");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("Trigger7-OnTransferTransactionInsert");
                    tb.HasTrigger("Trigger8-OnTransferTransactionUpdate");
                    tb.HasTrigger("Trigger9-OnTransferTransactionDelete");
                });

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(100);

            entity.HasOne(d => d.From).WithMany(p => p.TransferFroms)
                .HasForeignKey(d => d.FromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__FromI__5070F446");

            entity.HasOne(d => d.To).WithMany(p => p.TransferTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__ToId__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
