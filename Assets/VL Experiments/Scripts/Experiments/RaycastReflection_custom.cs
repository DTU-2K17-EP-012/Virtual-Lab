using UnityEngine;
using System.Collections;

namespace VirtualLab.Content
{
	[RequireComponent(typeof(LineRenderer))]
	public class RaycastReflection_custom : MonoBehaviour
	{
		//this game object's Transform
		private Transform goTransform;
		//the attached line renderer
		private LineRenderer lineRenderer;

		//a ray
		private Ray ray;
		//a RaycastHit variable, to gather informartion about the ray's collision
		private RaycastHit hit;

		//reflection direction
		private Vector3 inDirection;

		//the number of reflections
		public int nReflections = 2;

		//units for ray distance
		public int rayDistance = 100;


		void Start()
		{
			//get the attached Transform component
			goTransform = this.GetComponent<Transform>();
			//get the attached LineRenderer component
			lineRenderer = this.GetComponent<LineRenderer>();
		}

		void Update()
		{
			ray = new Ray(transform.position, transform.forward);
			lineRenderer.positionCount = 1;
			lineRenderer.SetPosition(0, transform.position);
			float remainingLength = rayDistance;
			for (int i = 0; i < nReflections; i++)
			{
				if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength) && hit.collider.tag == "Mirror")
				{
					lineRenderer.positionCount += 1;
					lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
					remainingLength -= Vector3.Distance(ray.origin, hit.point);
					ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
					if (hit.collider.tag != "Mirror")
						break;
				}
				else
				{
					lineRenderer.positionCount += 1;
					lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
				}
			}
		}
	}
}