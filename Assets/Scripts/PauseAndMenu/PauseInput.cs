using UnityEngine;

public class PauseInput : MonoBehaviour
{
    private bool _isPaused;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && _isPaused == false)
        {
            Pause.Stop();
            _isPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && _isPaused == true)
        {
            Pause.Resume();
            _isPaused = false;
        }
    }
}