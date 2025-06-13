using Bogus;

namespace StoredProcedureToCsv.Faker
{
    public static class FundTransactionFaker
    {
        public static IEnumerable<dynamic> GetFakeFundTransactions(int recordsToGenerate = 1000000)
        {
            var faker = new Faker<FundTransaction>()
                .RuleFor(t => t.Id, f => f.IndexFaker + 1)
                .RuleFor(t => t.CustomerId, f => f.Random.Int(1000, 9999))
                .RuleFor(t => t.Date, f => f.Date.Past())
                .RuleFor(t => t.CreatedAt, (f, t) => t.Date.AddMinutes(f.Random.Int(1, 60)))
                .RuleFor(t => t.FundTransactionId, f => f.Random.Long(100000, 999999))
                .RuleFor(t => t.Amount, f => f.Finance.Amount(-1000, 1000))
                .RuleFor(t => t.MeansOfPayment, f => f.PickRandom("Card", "Wallet", "BankTransfer", "Crypto"))
                .RuleFor(t => t.OriginalTransactionType, f => f.PickRandom("Deposit", "Withdrawal", "Bonus", "Adjustment"))
                .RuleFor(t => t.Reason, f => f.Lorem.Sentence(3))
                .RuleFor(t => t.TicketId, f => f.Random.Bool(0.7f) ? Guid.NewGuid() : null)
                .RuleFor(t => t.SessionId, f => f.Random.Bool(0.5f) ? Guid.NewGuid() : null)
                .RuleFor(t => t.TimestampCreated, (f, t) => t.CreatedAt)
                .RuleFor(t => t.InternalWalletActionId, f => f.Random.Long(10000, 99999))
                .RuleFor(t => t.InternalFundTransactionType, f => f.PickRandom("Stake", "Win", "Cashout", "Refund"))
                .RuleFor(t => t.InternalTicketStatus, f => f.PickRandom("Settled", "Cancelled", "Pending"))
                .RuleFor(t => t.TimestampUpdated, (f, t) => t.CreatedAt.AddMinutes(f.Random.Int(1, 60)))
                .RuleFor(t => t.WithdrawableAmount, f => f.Finance.Amount(0, 500))
                .RuleFor(t => t.BonusAmount, f => f.Finance.Amount(0, 300));

            IEnumerable<dynamic> fakeFunds = faker.Generate(recordsToGenerate);
            return fakeFunds;
        }
    }

    public class FundTransaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public long FundTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string MeansOfPayment { get; set; }
        public string OriginalTransactionType { get; set; }
        public string Reason { get; set; }
        public Guid? TicketId { get; set; }
        public Guid? SessionId { get; set; }
        public DateTime TimestampCreated { get; set; }
        public long? InternalWalletActionId { get; set; }
        public string InternalFundTransactionType { get; set; }
        public string InternalTicketStatus { get; set; }
        public DateTime TimestampUpdated { get; set; }
        public decimal WithdrawableAmount { get; set; }
        public decimal BonusAmount { get; set; }
    }
}
