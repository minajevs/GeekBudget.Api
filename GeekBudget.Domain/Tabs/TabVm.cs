namespace GeekBudget.Domain.Tabs
{
    public class TabVm
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "EUR";
    }
}
