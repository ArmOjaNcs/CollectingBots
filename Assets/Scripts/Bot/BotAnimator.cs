using UnityEngine;

public class BotAnimator : MonoBehaviour
{
    [SerializeField] private Animator[] _wheels;

    private Bot _bot;

    private void Awake()
    {
        _bot = GetComponent<Bot>();
    }

    private void OnEnable()
    {
        _bot.Ride += OnRide;
        _bot.StopRide += OnStopRide;
    }

    private void OnDisable()
    {
        _bot.Ride -= OnRide;
        _bot.StopRide -= OnStopRide;
    }

    private void OnRide()
    {
        foreach (Animator wheelAnimator in _wheels)
            wheelAnimator.SetBool(GameUtils.BotAnimatorRide, true);
    }

    private void OnStopRide()
    {
        foreach (Animator wheelAnimator in _wheels)
            wheelAnimator.SetBool(GameUtils.BotAnimatorRide, false);
    }
}