using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacilityMenu : PauseableObject
{
    [SerializeField] private UIAnimator _animator;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _collectedResourceCount;
    [SerializeField] private TextMeshProUGUI _availableResourceCount;
    [SerializeField] private TextMeshProUGUI _botStatus;
    [SerializeField] private Button _scan;

    private WaitForSeconds _showTime;

    public Button Scan => _scan;

    private protected override void Awake()
    {
        base.Awake();
        _showTime = new WaitForSeconds(GameUtils.TimeForMessage);
    }

    private void Start()
    {
        _animator.Show();
    }

    public void ShowMenu()
    {
        _animator.Show();
    }

    public void HideMenu()
    {
        _animator.Hide();
    }

    public override void Stop()
    {
        _canvasGroup.interactable = false;
    }

    public override void Resume()
    {
        _canvasGroup.interactable = true;
    }

    public void SetCollectedResourcesCount(int collectedResourceCount)
    {
        _collectedResourceCount.text = GameUtils.CollectedResourcesText + collectedResourceCount.ToString();
    }

    public void SetAvailableResourcesCount(int availableResourceCount)
    {
        _availableResourceCount.text = GameUtils.AvailableResourcesText + availableResourceCount.ToString();
    }

    public void SetBotStatus(string status)
    {
        StartCoroutine(ShowBotStatus(status));
    }

    private IEnumerator ShowBotStatus(string status)
    {
        _botStatus.text = status;
        yield return _showTime;
        _botStatus.text = "";
    }
}