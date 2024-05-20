using UnityEngine;

namespace ARspace
{
	public class UpDown : MonoBehaviour
	{
		public float radius = 1f;
		public float speed = 1f;

		private float y;
		private float o;
		private float a;

		private void Awake()
		{
			y = transform.localPosition.y;
			o = 0f;
			a = 0f;
		}
		private void Update()
		{
			a += speed*Time.deltaTime;
			o = Mathf.Sin(a)*radius;
			transform.localPosition = new Vector3(transform.localPosition.x, y+o, transform.localPosition.z);
		}
	}
}
