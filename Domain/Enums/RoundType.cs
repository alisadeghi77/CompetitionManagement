namespace Domain.Enums;

public enum RoundType
{
    /// <summary>
    /// 64 teams remaining
    /// 32 games
    /// </summary>
    RoundOf64 = 64,
    
    /// <summary>
    /// 32 teams remaining
    /// 16 games
    /// </summary>
    RoundOf32 = 32,
    
    /// <summary>
    /// 16 teams remaining
    /// 8 games
    /// </summary>
    RoundOf16 = 16,
    
    /// <summary>
    /// Quarter-finals (8 teams remaining)
    /// 4 games
    /// </summary>
    QuarterFinal = 8,
    
    /// <summary>
    /// Semi-finals (4 teams remaining)
    /// 2 games
    /// </summary>
    SemiFinal = 4,
    
    /// <summary>
    /// Third place playoff (optional)
    /// 1 game
    /// </summary>
    ThirdPlacePlayoff = 3,
    
    /// <summary>
    /// Final match (2 teams remaining)
    /// 1 game
    /// </summary>
    Final = 2,
}