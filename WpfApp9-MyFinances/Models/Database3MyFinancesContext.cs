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

    public virtual DbSet<AllTransaction> AllTransactions { get; set; }

    public virtual DbSet<CategoriesExp> CategoriesExps { get; set; }

    public virtual DbSet<CategoriesInc> CategoriesIncs { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

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

        modelBuilder.Entity<AllTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AllTrans__3214EC07EDF58150");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.BalanceAfter).HasColumnType("money");
            entity.Property(e => e.BalanceBefore).HasColumnType("money");
            entity.Property(e => e.DateOfTransaction)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.AllTransactions)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AllTransa__Payme__4E88ABD4");
        });

        modelBuilder.Entity<CategoriesExp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07E4ACCFBD");

            entity.ToTable("CategoriesExp");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DC214940F3").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<CategoriesInc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07E92AEF0D");

            entity.ToTable("CategoriesInc");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DCFB7027FC").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currenci__3214EC07288954E7");

            entity.Property(e => e.CodeLetter).HasMaxLength(3);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC07A14B337B");

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
                .HasConstraintName("FK__Expenses__Catego__5165187F");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Paymen__4F7CD00D");

            entity.HasOne(d => d.Provider).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Expenses__Provid__5070F446");

            entity.HasOne(d => d.SubcategoriesExp).WithMany(p => p.Expenses)
                .HasForeignKey(d => new { d.CategoryId, d.SubCategoryTitle })
                .HasConstraintName("FK__Expenses__52593CB8");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incomes__3214EC07C29D3068");

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
                .HasConstraintName("FK__Incomes__Categor__5535A963");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incomes__Payment__534D60F1");

            entity.HasOne(d => d.Provider).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Incomes__Provide__5441852A");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC070D8A6925");

            entity.Property(e => e.CurrentBalance).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(20);

            entity.HasOne(d => d.Currency).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentMe__Curre__5629CD9C");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC07DEE9FA05");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<SubcategoriesExp>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.Title }).HasName("PK__Subcateg__DBC25C4638AA21D7");

            entity.ToTable("SubcategoriesExp");

            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Category).WithMany(p => p.SubcategoriesExps)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subcatego__Categ__571DF1D5");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transfer__3214EC070C79639A");

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
                .HasConstraintName("FK__Transfers__FromI__5812160E");

            entity.HasOne(d => d.To).WithMany(p => p.TransferTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__ToId__59063A47");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
