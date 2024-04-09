using UnityEngine;

public class SetScreenRotation : MonoBehaviour
{
    [SerializeField] ScreenOrientation orientation;

    private void Start()
    {
        Screen.orientation = orientation;
    }
}
