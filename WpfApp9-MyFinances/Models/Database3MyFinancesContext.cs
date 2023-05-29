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

    public virtual DbSet<Exchange> Exchanges { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Periodicity> Periodicities { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<RecurringCharge> RecurringCharges { get; set; }

    public virtual DbSet<SubcategoriesExp> SubcategoriesExps { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=FENRIR-PC\\SQLEXPRESS;Initial Catalog=Database3-MyFinances;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");
      //=> optionsBuilder.UseSqlServer("Data Source=WINSRVR2019;Initial Catalog=MyFinancesCopyTest;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=False;Trust Server Certificate=False;Command Timeout=0");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<AllTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AllTrans__3214EC072FE02BAD");

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
                .HasConstraintName("FK__AllTransa__Payme__571DF1D5");
        });

        modelBuilder.Entity<CategoriesExp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A2C3EA88");

            entity.ToTable("CategoriesExp");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DC530E4A59").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<CategoriesInc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07E796B395");

            entity.ToTable("CategoriesInc");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DC7EEB7DF6").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currenci__3214EC07E650E4DD");

            entity.Property(e => e.CodeLetter).HasMaxLength(3);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Exchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exchange__3214EC07F3442E1A");

            entity.ToTable("Exchange", tb =>
                {
                    tb.HasTrigger("Trigger10-OnExchangeTransactionInsert");
                    tb.HasTrigger("Trigger11-OnExchangeTransactionUpdate");
                    tb.HasTrigger("Trigger12-OnExchangeTransactionDelete");
                });

            entity.Property(e => e.AmountFrom).HasColumnType("money");
            entity.Property(e => e.AmountTo)
                .HasComputedColumnSql("([AmountFrom]*[ExchangeRate])", false)
                .HasColumnType("decimal(29, 6)");
            entity.Property(e => e.DateOfExchange)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(9, 2)");

            entity.HasOne(d => d.CurrencyIdFromNavigation).WithMany(p => p.ExchangeCurrencyIdFromNavigations)
                .HasForeignKey(d => d.CurrencyIdFrom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exchange__Curren__59FA5E80");

            entity.HasOne(d => d.CurrencyIdToNavigation).WithMany(p => p.ExchangeCurrencyIdToNavigations)
                .HasForeignKey(d => d.CurrencyIdTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exchange__Curren__5AEE82B9");

            entity.HasOne(d => d.From).WithMany(p => p.ExchangeFroms)
                .HasForeignKey(d => d.FromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exchange__FromId__5812160E");

            entity.HasOne(d => d.To).WithMany(p => p.ExchangeTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exchange__ToId__59063A47");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC078AD613C8");

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
                .HasConstraintName("FK__Expenses__Catego__5DCAEF64");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Paymen__5BE2A6F2");

            entity.HasOne(d => d.Provider).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Provid__5CD6CB2B");

            entity.HasOne(d => d.SubcategoriesExp).WithMany(p => p.Expenses)
                .HasForeignKey(d => new { d.CategoryId, d.SubCategoryTitle })
                .HasConstraintName("FK__Expenses__5EBF139D");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incomes__3214EC073DA0B36B");

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
                .HasConstraintName("FK__Incomes__Categor__619B8048");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incomes__Payment__5FB337D6");

            entity.HasOne(d => d.Provider).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Incomes__Provide__60A75C0F");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC07041163DF");

            entity.Property(e => e.CurrentBalance).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(20);

            entity.HasOne(d => d.Currency).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentMe__Curre__628FA481");
        });

        modelBuilder.Entity<Periodicity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Periodic__3214EC0738A55703");

            entity.ToTable("Periodicity");

            entity.HasIndex(e => e.Title, "UQ__Periodic__2CB664DC836D22AC").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(20);
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC0747649E5E");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<RecurringCharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recurrin__3214EC07C801BE55");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DateOfStart)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.PeriodicityText).HasMaxLength(100);
            entity.Property(e => e.SubCategoryTitle).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recurring__Categ__6383C8BA");

            entity.HasOne(d => d.Currency).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recurring__Curre__656C112C");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Recurring__Payme__66603565");

            entity.HasOne(d => d.Periodicity).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.PeriodicityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recurring__Perio__6754599E");

            entity.HasOne(d => d.Provider).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recurring__Provi__6477ECF3");

            entity.HasOne(d => d.SubcategoriesExp).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => new { d.CategoryId, d.SubCategoryTitle })
                .HasConstraintName("FK__RecurringCharges__68487DD7");
        });

        modelBuilder.Entity<SubcategoriesExp>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.Title }).HasName("PK__Subcateg__DBC25C4670B39A9D");

            entity.ToTable("SubcategoriesExp");

            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Category).WithMany(p => p.SubcategoriesExps)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subcatego__Categ__693CA210");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transfer__3214EC07EC0E17B3");

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
                .HasConstraintName("FK__Transfers__FromI__6A30C649");

            entity.HasOne(d => d.To).WithMany(p => p.TransferTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__ToId__6B24EA82");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
