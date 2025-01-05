using System;
using System.Collections.Generic;

[Serializable]
public class GameDataDTO
{
    public string userId;
    public string gameId;
    public DateTimeOffset? startTime;
    public DateTimeOffset? clearTime;
    public int killCount;
    public int clickCount;
    public long gold;
    public long totalGold;
    public List<UpgradeDTO> upgrades;
}

