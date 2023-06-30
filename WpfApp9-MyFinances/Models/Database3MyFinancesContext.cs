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

    public virtual DbSet<GivingLoan> GivingLoans { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Periodicity> Periodicities { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<ReceivingLoan> ReceivingLoans { get; set; }

    public virtual DbSet<RecurringCharge> RecurringCharges { get; set; }

    public virtual DbSet<SubcategoriesExp> SubcategoriesExps { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Data Source=FENRIR-PC\\SQLEXPRESS;Initial Catalog=Database3-MyFinances;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");
        => optionsBuilder.UseSqlServer("Data Source=WINSRVR2019;Initial Catalog=MyFinancesCopyTest;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=False;Trust Server Certificate=False;Command Timeout=0");
        //=> optionsBuilder.UseSqlServer("Data Source=FENRIR-PC\\SQLEXPRESS;Initial Catalog=Database3-MyFinances-Test;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<AllTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AllTrans__3214EC074E5A4611");

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
                .HasConstraintName("FK__AllTransa__Payme__5CD6CB2B");
        });

        modelBuilder.Entity<CategoriesExp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC072A2273EF");

            entity.ToTable("CategoriesExp");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DCB5C5BC9E").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<CategoriesInc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC072FD23CC7");

            entity.ToTable("CategoriesInc");

            entity.HasIndex(e => e.Title, "UQ__Categori__2CB664DCC884F542").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currenci__3214EC07B0C4B8C3");

            entity.Property(e => e.CodeLetter).HasMaxLength(3);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Exchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exchange__3214EC07A3D72109");

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
                .HasConstraintName("FK__Exchange__Curren__5FB337D6");

            entity.HasOne(d => d.CurrencyIdToNavigation).WithMany(p => p.ExchangeCurrencyIdToNavigations)
                .HasForeignKey(d => d.CurrencyIdTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exchange__Curren__60A75C0F");

            entity.HasOne(d => d.From).WithMany(p => p.ExchangeFroms)
                .HasForeignKey(d => d.FromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exchange__FromId__5DCAEF64");

            entity.HasOne(d => d.To).WithMany(p => p.ExchangeTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exchange__ToId__5EBF139D");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC07604BCE21");

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
                .HasConstraintName("FK__Expenses__Catego__6383C8BA");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Paymen__619B8048");

            entity.HasOne(d => d.Provider).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Provid__628FA481");

            entity.HasOne(d => d.SubcategoriesExp).WithMany(p => p.Expenses)
                .HasForeignKey(d => new { d.CategoryId, d.SubCategoryTitle })
                .HasConstraintName("FK__Expenses__6477ECF3");
        });

        modelBuilder.Entity<GivingLoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GivingLo__3214EC076F19C5FA");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("Trigger13-OnGivingLoanTransactionInsert");
                    tb.HasTrigger("Trigger14-OnGivingLoanTransactionUpdate");
                    tb.HasTrigger("Trigger15-OnGivingLoanTransactionDelete");
                });

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DateOfLoan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.GivingLoans)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GivingLoa__Payme__656C112C");

            entity.HasOne(d => d.Provider).WithMany(p => p.GivingLoans)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GivingLoa__Provi__66603565");

            entity.HasOne(d => d.ReceivingLoan).WithMany(p => p.GivingLoans)
                .HasForeignKey(d => d.ReceivingLoanId)
                .HasConstraintName("FK__GivingLoa__Recei__6754599E");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incomes__3214EC07ECB3C18D");

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
                .HasConstraintName("FK__Incomes__Categor__6A30C649");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Incomes__Payment__68487DD7");

            entity.HasOne(d => d.Provider).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Incomes__Provide__693CA210");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentM__3214EC07B90FE645");

            entity.Property(e => e.CurrentBalance).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(20);

            entity.HasOne(d => d.Currency).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentMe__Curre__6B24EA82");
        });

        modelBuilder.Entity<Periodicity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Periodic__3214EC077BAD5BC6");

            entity.ToTable("Periodicity");

            entity.HasIndex(e => e.Title, "UQ__Periodic__2CB664DCC02CDDEB").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(20);
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC07AFA9FA5E");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<ReceivingLoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Receivin__3214EC071E265869");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("Trigger16-OnReceivingLoanTransactionInsert");
                    tb.HasTrigger("Trigger17-OnReceivingLoanTransactionUpdate");
                    tb.HasTrigger("Trigger18-OnReceivingLoanTransactionDelete");
                });

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DateOfLoan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);

            entity.HasOne(d => d.GivingLoan).WithMany(p => p.ReceivingLoans)
                .HasForeignKey(d => d.GivingLoanId)
                .HasConstraintName("FK__Receiving__Givin__6E01572D");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.ReceivingLoans)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receiving__Payme__6C190EBB");

            entity.HasOne(d => d.Provider).WithMany(p => p.ReceivingLoans)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receiving__Provi__6D0D32F4");
        });

        modelBuilder.Entity<RecurringCharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recurrin__3214EC07215A400F");

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
                .HasConstraintName("FK__Recurring__Categ__6EF57B66");

            entity.HasOne(d => d.Currency).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recurring__Curre__70DDC3D8");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Recurring__Payme__71D1E811");

            entity.HasOne(d => d.Periodicity).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.PeriodicityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recurring__Perio__72C60C4A");

            entity.HasOne(d => d.Provider).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recurring__Provi__6FE99F9F");

            entity.HasOne(d => d.SubcategoriesExp).WithMany(p => p.RecurringCharges)
                .HasForeignKey(d => new { d.CategoryId, d.SubCategoryTitle })
                .HasConstraintName("FK__RecurringCharges__73BA3083");
        });

        modelBuilder.Entity<SubcategoriesExp>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.Title }).HasName("PK__Subcateg__DBC25C4683922406");

            entity.ToTable("SubcategoriesExp");

            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Category).WithMany(p => p.SubcategoriesExps)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subcatego__Categ__74AE54BC");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transfer__3214EC0772E33649");

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
                .HasConstraintName("FK__Transfers__FromI__75A278F5");

            entity.HasOne(d => d.To).WithMany(p => p.TransferTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__ToId__76969D2E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
