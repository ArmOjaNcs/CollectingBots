using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIAnimator _menu;
    [SerializeField] private List<UIAnimator> _subElements;
    [Inject] private Pause _pause;

    private void OnEnable()
    {
        _pause.IsPaused += OnPaused;
        _pause.IsUnPaused += OnUnPaused;
    }

    private void OnDisable()
    {
        _pause.IsPaused -= OnPaused;
        _pause.IsUnPaused -= OnUnPaused;
    }

    private void OnPaused()
    {
        _menu.Show();
    }

    private void OnUnPaused()
    {
        _menu.Hide();

        foreach(UIAnimator animator in _subElements)
            animator.Hide();
    }
}