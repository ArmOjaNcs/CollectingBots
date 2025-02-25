using UnityEngine;

public static class GameUtils
{
    public static readonly int Half = 2;
    public static readonly int MaxBotsCount = 3;
    public static readonly int MaxResourcesCount = 50;
    public static readonly float MinTimeToSpawn = 1f;
    public static readonly float MaxTimeToSpawn = 3f;
    public static readonly float TimeForMessage = 1f;
    public static readonly float BotMinDistanceToTarget = 2;
    public static readonly float BotMinDistanceToBaseFacility = 7;
    public static readonly float BaseFacilityScale = 2;
    public static readonly float BaseFacilityTimeToBuild = 10;
    public static readonly float BaseFacilityStartYPositionOnBuild = -5;
    public static readonly float UIAnimationTime = 0.2f;
    public static readonly string Ground = nameof(Ground);
    public static readonly string BotSendedMessage = "Bot has been sended";
    public static readonly string BotsBusyMessage = "All bots are busy";
    public static readonly string CollectedResourcesText = "Collected: ";
    public static readonly string AvailableResourcesText = "Available: ";
    public static readonly int BotAnimatorRide = Animator.StringToHash("Ride");
}