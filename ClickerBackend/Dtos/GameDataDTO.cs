namespace ClickerBackend.Dtos
{
    public record GameDataDTO
    {
        public string UserId { get; init; }
        public string GameId { get; init; }
        public DateTimeOffset? StartTime { init; get; }
        public DateTimeOffset? ClearTime { init; get; }
        public int KillCount { init; get; }
        public int ClickCount { init; get; }
        public long Gold { init; get; }
        public long TotalGold { init; get; }
        public List<UpgradeDTO> Upgrades { init; get; }
    }
}
