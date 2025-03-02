using System.Collections;
using TMPro;
using UnityEngine;

public class BaseStructureView : MonoBehaviour
{
    [SerializeField] private BaseStructure _baseStructure;
    [SerializeField] private UIAnimator _animator;
    [SerializeField] private TextMeshProUGUI _collectedResourceCount;
    [SerializeField] private TextMeshProUGUI _availableResourceCount;
    [SerializeField] private TextMeshProUGUI _botStatus;

    private WaitForSeconds _showTime;

    private void Awake()
    {
        _showTime = new WaitForSeconds(GameUtils.TimeForMessage);
    }

    private void OnEnable()
    {
        SetStartViewValue();
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        _animator.Show();
    }

    public void HideMenu()
    {
        _animator.Hide();
    }

    private void SetStartViewValue()
    {
        SetCollectedResourcesCount(_baseStructure.CollectedResourcesCount);
        SetAvailableResourcesCount(_baseStructure.AvailableResourcesCount);
        SetBotStatus("");
    }

    private void Subscribe()
    {
        _baseStructure.CollectedResourcesCountChanged += SetCollectedResourcesCount;
        _baseStructure.AvailableResourcesCountChanged += SetAvailableResourcesCount;
        _baseStructure.BotStatusChanged += SetBotStatus;
    }

    private void UnSubscribe()
    {
        _baseStructure.CollectedResourcesCountChanged -= SetCollectedResourcesCount;
        _baseStructure.AvailableResourcesCountChanged -= SetAvailableResourcesCount;
        _baseStructure.BotStatusChanged -= SetBotStatus;
    }

    private void SetCollectedResourcesCount(int collectedResourceCount)
    {
        _collectedResourceCount.text = GameUtils.CollectedResourcesText + collectedResourceCount.ToString();
    }

    private void SetAvailableResourcesCount(int availableResourceCount)
    {
        _availableResourceCount.text = GameUtils.AvailableResourcesText + availableResourceCount.ToString();
    }

    private void SetBotStatus(string status)
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