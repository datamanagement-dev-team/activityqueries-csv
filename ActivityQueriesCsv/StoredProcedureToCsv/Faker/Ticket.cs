using Bogus;

namespace StoredProcedureToCsv.Faker
{
    public static class TicketFaker
    { 
        public static IEnumerable<dynamic> GetFakeTickets(int recordsToGenerate = 1000000)
        {
            var faker = new Faker<Ticket>()
                .RuleFor(t => t.Id, f => f.IndexFaker + 1)
                .RuleFor(t => t.TicketId, f => Guid.NewGuid())
                .RuleFor(t => t.CustomerId, f => f.Random.Int(1000, 9999))
                .RuleFor(t => t.Date, f => f.Date.Past())
                .RuleFor(t => t.PlacementDate, (f, t) => t.Date.AddMinutes(f.Random.Int(0, 60)))
                .RuleFor(t => t.BetAmount, f => f.Finance.Amount(1, 1000))
                .RuleFor(t => t.IpAddress, f => f.Internet.Ip())
                .RuleFor(t => t.OriginState, f => f.Address.StateAbbr())
                .RuleFor(t => t.TicketType, f => f.PickRandom("Single", "Multiple", "System"))
                .RuleFor(t => t.Status, f => f.PickRandom("Settled", "Pending", "Cancelled"))
                .RuleFor(t => t.CombinationPrice, f => f.Finance.Amount(1, 50))
                .RuleFor(t => t.TotalPrice, (f, t) => t.CombinationPrice * f.Random.Int(1, 5))
                .RuleFor(t => t.NumberOfCombinations, f => f.Random.Int(1, 10))
                .RuleFor(t => t.Payout, f => f.Finance.Amount(0, 2000))
                .RuleFor(t => t.Loss, f => f.Finance.Amount(0, 500))
                .RuleFor(t => t.IsCashout, f => f.Random.Bool())
                .RuleFor(t => t.CashoutType, f => f.PickRandom("Partial", "Full", null))
                .RuleFor(t => t.CashoutAmount, f => f.Random.Decimal(0, 100))
                .RuleFor(t => t.CancellationReason, f => f.Lorem.Word())
                .RuleFor(t => t.InternalStatusSysname, f => f.Lorem.Word())
                .RuleFor(t => t.InternalPayout, f => f.Finance.Amount(0, 2000))
                .RuleFor(t => t.InternalLastFundsTransactionId, f => f.Random.Long(100000, 999999))
                .RuleFor(t => t.InternalBetCostDiscount, f => f.Random.Decimal(0, 100))
                .RuleFor(t => t.InternalBetReturnsBonus, f => f.Random.Decimal(0, 100))
                .RuleFor(t => t.TimestampCreated, f => f.Date.Past())
                .RuleFor(t => t.TimestampUpdated, (f, t) => t.TimestampCreated.AddMinutes(f.Random.Int(0, 60)))
                .RuleFor(t => t.InternalWithdrawableBetCost, f => f.Random.Decimal(0, 500))
                .RuleFor(t => t.InternalSettlementDate, f => f.Date.Recent())
                .RuleFor(t => t.InternalWithdrawablePayout, f => f.Random.Decimal(0, 500))
                .RuleFor(t => t.InternalBonusPayout, f => f.Random.Decimal(0, 100));

            IEnumerable<dynamic> fakeTickets = faker.Generate(recordsToGenerate);
            return fakeTickets;
        }
    }



    public class Ticket
    {
        public int Id { get; set; }
        public Guid TicketId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public DateTime PlacementDate { get; set; }
        public decimal BetAmount { get; set; }
        public string IpAddress { get; set; }
        public string OriginState { get; set; }
        public string TicketType { get; set; }
        public string Status { get; set; }
        public decimal CombinationPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int NumberOfCombinations { get; set; }
        public decimal Payout { get; set; }
        public decimal Loss { get; set; }
        public bool IsCashout { get; set; }
        public string CashoutType { get; set; }
        public decimal? CashoutAmount { get; set; }
        public string CancellationReason { get; set; }
        public string InternalStatusSysname { get; set; }
        public decimal InternalPayout { get; set; }
        public long? InternalLastFundsTransactionId { get; set; }
        public decimal? InternalBetCostDiscount { get; set; }
        public decimal? InternalBetReturnsBonus { get; set; }
        public DateTime TimestampCreated { get; set; }
        public DateTime TimestampUpdated { get; set; }
        public decimal? InternalWithdrawableBetCost { get; set; }
        public DateTime? InternalSettlementDate { get; set; }
        public decimal? InternalWithdrawablePayout { get; set; }
        public decimal? InternalBonusPayout { get; set; }
    }
}
