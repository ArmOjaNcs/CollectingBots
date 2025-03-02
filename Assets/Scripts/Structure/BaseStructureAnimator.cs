using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BaseStructureAnimator : ObjectToSpawn
{
    private Sequence _sequence;
    private Coroutine _buildRoutine;
    private float _currentBuildTime;
    private bool _isBuildStarted;

    public event Action<BaseStructureAnimator> AnimatorFinished;
    
    public BaseStructure CurrentBaseStructure {  get; private set; }

    private void Awake()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOScale(GameUtils.BaseStructureScale, GameUtils.BaseStructureTimeToBuild).From(0)).
            Insert(0, transform.DOMoveY(0, GameUtils.BaseStructureTimeToBuild).From(GameUtils.BaseStructureStartYPositionOnBuild))
            .SetAutoKill(false);
    }

    private void OnEnable()
    {
        _currentBuildTime = 0;
        _buildRoutine = null;
        _isBuildStarted = false;
        CurrentBaseStructure = null;
    }

    public override void Stop()
    {
        _sequence.Pause();

        if (_buildRoutine != null && isActiveAndEnabled)
            StopCoroutine(_buildRoutine);
    }

    public override void Resume()
    {
        _sequence.Play();

        if (_currentBuildTime < GameUtils.BaseStructureTimeToBuild && _isBuildStarted && isActiveAndEnabled)
            _buildRoutine = StartCoroutine(WaitForBuild(GameUtils.BaseStructureTimeToBuild));
    }

    public void BuildStructure(BaseStructure baseStructure)
    {
        transform.position = baseStructure.transform.position;
        CurrentBaseStructure = baseStructure;
        _sequence.Play();
        _buildRoutine = StartCoroutine(WaitForBuild(GameUtils.BaseStructureTimeToBuild));
        _isBuildStarted = true;
    }

    private IEnumerator WaitForBuild(float buildTime)
    {
        while (_currentBuildTime < buildTime)
        {
            _currentBuildTime += Time.deltaTime;
            yield return null;
        }

        AnimatorFinished?.Invoke(this);
        _isBuildStarted = false;
    }
}