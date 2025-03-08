using UnityEngine;

public static class GameUtils
{
    public static readonly int Half = 2;
    public static readonly int Quad = 2;
    public static readonly int Two = 2;
    public static readonly int MaxBotsCount = 3;
    public static readonly int MaxResourcesCount = 150;
    public static readonly int MaxAttemptsCount = 100;
    public static readonly int Ground = 3;
    public static readonly int BotCost = 3;
    public static readonly int MinBotsCountToBuild = 1;
    public static readonly int MaxBotsCountOnBase = 5;
    public static readonly int BaseCost = 5;
    public static readonly float Pi = 3.14f;
    public static readonly float FullAngleInDegrees = 360;
    public static readonly float MultiplierForResourceDiameter = 1.5f;
    public static readonly float BotDiameter = 2.5f;
    public static readonly float RingRadius = 8;
    public static readonly float MinTimeToSpawn = 0.2f;
    public static readonly float MaxTimeToSpawn = 1;
    public static readonly float TimeToScan = 1.5f;
    public static readonly float TimeForMessage = 1f;
    public static readonly float ScannerColliderLifeTime = 0.5f;
    public static readonly float BotMinDistanceToTarget = 2;
    public static readonly float BotMinDistanceToBaseStructure = 7;
    public static readonly float BaseStructureScale = 2;
    public static readonly float BaseStructureTimeToBuild = 10;
    public static readonly float BaseStructureStartYPositionOnBuild = -5;
    public static readonly float BuildViewRotationSpeed = 80;
    public static readonly float UIAnimationTime = 0.2f;
    public static readonly string CollectedResourcesText = "Collected: ";
    public static readonly string AvailableResourcesText = "Available: ";
    public static readonly int BotAnimatorRide = Animator.StringToHash("Ride");
}