using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BaseStructureMenu : PauseableObject
{
    [SerializeField] private UIAnimator _animator;
    [SerializeField] private Button _buildNewBase;
    [SerializeField] private TextMeshProUGUI _collectedResourceCount;
    [SerializeField] private TextMeshProUGUI _availableResourceCount;
    [SerializeField] private CanvasGroup _canvasGroup;
    [Inject] Pause _pause;

    private BaseStructure _baseStructure;

    public event Action<BaseStructure> BuildInitiated;

    private void Awake()
    {
        _pause.Register(this);
    }

    private void OnEnable()
    {
        _buildNewBase.onClick.AddListener(BuildNewBase);     
    }

    private void OnDisable()
    {
        _buildNewBase.onClick.RemoveListener(BuildNewBase);
    }

    public override void Stop()
    {
        _canvasGroup.interactable = false;
    }

    public override void Resume()
    {
        _canvasGroup.interactable = true;
    }

    public void SetBaseStructure(BaseStructure baseStructure)
    {
        if(_baseStructure != null)
            UnSubscribe();

        _baseStructure = baseStructure;

        if (_baseStructure.IsCanBuild)
            _buildNewBase.gameObject.SetActive(true);
        else
            _buildNewBase.gameObject.SetActive(false);

        Subscribe();
        SetStartViewValue();
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
    }

    private void Subscribe()
    {
        _baseStructure.CollectedResourcesCountChanged += SetCollectedResourcesCount;
        _baseStructure.AvailableResourcesCountChanged += SetAvailableResourcesCount;
        _baseStructure.NewBaseAcceptedToBuild += OnNewBaseAcceptedToBuild;
    }

    private void UnSubscribe()
    {
        _baseStructure.CollectedResourcesCountChanged -= SetCollectedResourcesCount;
        _baseStructure.AvailableResourcesCountChanged -= SetAvailableResourcesCount;
        _baseStructure.NewBaseAcceptedToBuild -= OnNewBaseAcceptedToBuild;
    }

    private void SetCollectedResourcesCount(int collectedResourceCount)
    {
        _collectedResourceCount.text = GameUtils.CollectedResourcesText + collectedResourceCount.ToString();
    }

    private void SetAvailableResourcesCount(int availableResourceCount)
    {
        _availableResourceCount.text = GameUtils.AvailableResourcesText + availableResourceCount.ToString();
    }

    private void OnNewBaseAcceptedToBuild()
    {
        _buildNewBase.gameObject.SetActive(false);
    }

    private void BuildNewBase()
    {
        BuildInitiated?.Invoke(_baseStructure);
    }
}