using UnityEngine;

namespace ARspace
{
    public class GyroRotate : MonoBehaviour
    {
        public static GyroRotate Instance { get; private set; }

        private Vector3 parentLocalRotation = new Vector3(90, -90, 0);
        private Quaternion fix = new Quaternion(0f, 0f, 1f, 0f);

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (SystemInfo.supportsGyroscope)
            {
                transform.parent.localEulerAngles = parentLocalRotation;

                Input.gyro.enabled = true;
                Input.gyro.updateInterval = 1f / 120f;
            }
        }

        private void Update()
        {
            if (SystemInfo.supportsGyroscope)
                transform.localRotation = Quaternion.Lerp(transform.localRotation,
                    !TouchRotate.Instance.Full ? Input.gyro.attitude * fix : Quaternion.Inverse(Quaternion.Euler(parentLocalRotation)),
                    Time.deltaTime * 6f);
        }
    }
}