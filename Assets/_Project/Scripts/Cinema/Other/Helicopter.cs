using UnityEngine;

namespace ARspace
{
	public class Helicopter : MonoBehaviour
	{
		public Transform target;
		public float speed = 10f;

		private Transform route;
		private float startTime;

		private int curr;
		private int next;

		private void Awake()
		{
			route = transform;//.Find("Route");
			curr = 0;
			next = 0;
		}
		private void Update()
		{
			if(curr == next)
			{
				if(++next >= route.childCount) next = 0;
			}
			if(curr != next)
			{
			//	target.Translate((route.GetChild(next).position-target.position)*speed*Time.deltaTime, Space.Self);
				target.Translate(target.forward*speed*Time.deltaTime, Space.World);
				target.rotation = Quaternion.Lerp(target.rotation, Quaternion.LookRotation(route.GetChild(next).position-target.position), Time.deltaTime*.5f);
				if((route.GetChild(next).position-target.position).magnitude < .5f) curr = next;
			}
		}
	}
}
