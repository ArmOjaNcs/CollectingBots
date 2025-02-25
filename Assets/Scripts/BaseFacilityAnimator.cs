using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BaseFacilityAnimator : PauseableObject
{
    private Sequence _sequence;
    private Coroutine _buildRoutine;
    private float _currentBuildTime;

    public event Action FacilityBuilded;

    private protected override void Awake()
    {
        base.Awake();
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOScale(GameUtils.BaseFacilityScale, GameUtils.BaseFacilityTimeToBuild).From(0)).
            Insert(0, transform.DOMoveY(0, GameUtils.BaseFacilityTimeToBuild).From(GameUtils.BaseFacilityStartYPositionOnBuild))
            .SetAutoKill(false);
    }

    private void OnEnable()
    {
        _currentBuildTime = 0;
        _buildRoutine = null;
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

        if (_currentBuildTime < GameUtils.BaseFacilityTimeToBuild && isActiveAndEnabled)
            _buildRoutine = StartCoroutine(WaitForBuild(_currentBuildTime));
    }

    public void BuildFacility()
    {
        _sequence.Play();
        _buildRoutine = StartCoroutine(WaitForBuild(GameUtils.BaseFacilityTimeToBuild));
    }

    private IEnumerator WaitForBuild(float buildTime)
    {
        //while(_currentBuildTime < buildTime)
        //{
        //    _currentBuildTime += Time.deltaTime;
        //    yield return null;
        //    Debug.Log(_currentBuildTime);
        //}
        yield return null;
        FacilityBuilded?.Invoke();
    }
}