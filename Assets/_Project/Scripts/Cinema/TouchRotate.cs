using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    public static TouchRotate Instance { get; private set; }
    public bool Full { get; private set; }

    public float rotateSpeed = 10f;

    private void Awake()
    {
        Instance = this;
        Full = true;
    }

    private void Update()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                float x = transform.localEulerAngles.x + Input.GetTouch(0).deltaPosition.y * (1080f / Screen.height) * rotateSpeed * Time.deltaTime;
                if (x < 180f && x > 80f) x = 80f;
                if (x > 180f && x < 280f) x = 280f;

                transform.Rotate(new Vector3(0f, -Input.GetTouch(0).deltaPosition.x * (1920f / Screen.width) * rotateSpeed * Time.deltaTime, 0f),
                    Space.World);
                if (Full) transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }

            if (!Full) transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        else if (Input.GetMouseButton(0))
        {
            float x = transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * rotateSpeed * (20 * 1080 / Screen.height) * Time.deltaTime;
            if (x < 180f && x > 80f) x = 80f;
            if (x > 180f && x < 280f) x = 280f;

            transform.Rotate(new Vector3(0f, -Input.GetAxis("Mouse X") * rotateSpeed * (20 * 1920 / Screen.width) * Time.deltaTime, 0f), Space.World);
            if (Full) transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }
}