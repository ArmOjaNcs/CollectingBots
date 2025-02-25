using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsPicked {  get; private set; }

    private void OnEnable()
    {
        IsPicked = false;
        transform.parent = null;
    }

    public void Pick()
    {
        IsPicked = true;
    }
}