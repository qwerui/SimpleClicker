namespace ClickerBackend.Dtos
{
    public record UpgradeDTO
    {
        public string GameId { get; init; }
        public int UpgradeId { init; get; }
        public int Amount { init; get; }
    }
}
