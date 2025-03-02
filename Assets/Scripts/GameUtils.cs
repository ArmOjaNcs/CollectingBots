using UnityEngine;

public static class GameUtils
{
    public static readonly int Half = 2;
    public static readonly int MaxBotsCount = 3;
    public static readonly int MaxResourcesCount = 50;
    public static readonly int MaxAttemptsCount = 100;
    public static readonly float MinTimeToSpawn = 0.5f;
    public static readonly float MaxTimeToSpawn = 1f;
    public static readonly float TimeToScan = 1.5f;
    public static readonly float TimeForMessage = 1f;
    public static readonly float ScannerColliderLifeTime = 0.5f;
    public static readonly float BotMinDistanceToTarget = 2;
    public static readonly float BotMinDistanceToBaseStructure = 7;
    public static readonly float BaseStructureScale = 2;
    public static readonly float BaseStructureTimeToBuild = 10;
    public static readonly float BaseStructureStartYPositionOnBuild = -5;
    public static readonly float UIAnimationTime = 0.2f;
    public static readonly string Ground = nameof(Ground);
    public static readonly string BotSendedMessage = "Bot has been sended";
    public static readonly string BotsBusyMessage = "All bots are busy";
    public static readonly string CollectedResourcesText = "Collected: ";
    public static readonly string AvailableResourcesText = "Available: ";
    public static readonly int BotAnimatorRide = Animator.StringToHash("Ride");
}