using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIAnimator _menu;
    [SerializeField] private List<UIAnimator> _subElements;

    private void OnEnable()
    {
        Pause.IsPaused += OnPaused;
        Pause.IsUnPaused += OnUnPaused;
    }

    private void OnDisable()
    {
        Pause.IsPaused -= OnPaused;
        Pause.IsUnPaused -= OnUnPaused;
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