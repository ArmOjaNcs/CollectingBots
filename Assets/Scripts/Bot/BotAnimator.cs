using UnityEngine;

public class BotAnimator : MonoBehaviour
{
    [SerializeField] private Animator[] _wheels;
    [SerializeField] private ParticleSystem[] _trails;

    private Bot _bot;

    private void Awake()
    {
        _bot = GetComponent<Bot>();
    }

    private void OnEnable()
    {
        _bot.Ride += OnRide;
        _bot.StopRide += OnStopRide;
        _bot.Play += Resume;
        _bot.Pause += Pause;
    }

    private void OnDisable()
    {
        _bot.Ride -= OnRide;
        _bot.StopRide -= OnStopRide;
        _bot.Play -= Resume;
        _bot.Pause -= Pause;
    }

    public void Pause()
    {
        foreach (Animator wheelAnimator in _wheels)
            wheelAnimator.enabled = false;

        foreach (ParticleSystem partical in _trails)
            partical.Pause();
    }

    public void Resume()
    {
        foreach (Animator wheelAnimator in _wheels)
            wheelAnimator.enabled = true;

        foreach (ParticleSystem partical in _trails)
            partical.Play();
    }

    private void OnRide()
    {
        foreach (Animator wheelAnimator in _wheels)
            wheelAnimator.SetBool(GameUtils.BotAnimatorRide, true);

        foreach (ParticleSystem partical in _trails)
            partical.Play();
    }

    private void OnStopRide()
    {
        foreach (Animator wheelAnimator in _wheels)
            wheelAnimator.SetBool(GameUtils.BotAnimatorRide, false);
    }
}