using UnityEngine;
using Zenject;

public class PauseInput : MonoBehaviour
{
    [Inject] private Pause _pause;
    private bool _isPaused;

    private bool IsPauseButtonDown => Input.GetKeyDown(KeyCode.Escape);

    private void Update()
    {
        ReadPauseButton();
    }

    private void ReadPauseButton()
    {
        if (IsPauseButtonDown)
        {
            if (_isPaused == false)
            {
                _pause.Stop();
                _isPaused = true;
            }
            else
            {
                _pause.Resume();
                _isPaused = false;
            }
        }
    }
}