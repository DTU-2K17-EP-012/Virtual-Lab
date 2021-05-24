using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Content
{
    [RequireComponent(typeof(LineRenderer))]
    public class RaycastRefraction : MonoBehaviour
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
        // RI1 - the refractive index of the first medium(air)
        [SerializeField] public float RI1 = 1;
        //RI2 - the refractive index of the second medium(glass)
        private float RI2 = 1.33f;
        //refracted vector
        private Vector3 refractedRay;
        //to change ray color
        public Material ray_shader_goal;
        public Material ray_shader_initial;

        //surfNorm - the normal of the interface between the two mediums(for example the normal returned by a raycast)
        //incident - the incoming Vector3 to be refracted
        public static Vector3 Refract(float RI1, float RI2, Vector3 surfNorm, Vector3 incident)
        {
            surfNorm.Normalize(); //should already be normalized, but normalize just to be sure
            incident.Normalize();

            return (RI1 / RI2 * Vector3.Cross(surfNorm, Vector3.Cross(-surfNorm, incident)) - surfNorm * Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(surfNorm, incident) * (RI1 / RI2 * RI1 / RI2), Vector3.Cross(surfNorm, incident)))).normalized;
        }
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
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength) && hit.collider.tag == "medium")
                {
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    remainingLength -= Vector3.Distance(ray.origin, hit.point);
                    //access lens attribute
                    RI2 = hit.collider.GetComponent<lensManager>().GetRI();
                    refractedRay = Refract(RI1, RI2, hit.normal, ray.direction);
                    ray = new Ray(hit.point, refractedRay);
                    if (hit.collider.tag != "medium")
                        break;


                }
                else if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength) && hit.collider.tag == "Mirror")
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
                    lineRenderer.material = ray_shader_initial;
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
                }
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength) && hit.collider.tag == "goal")
                {
                    lineRenderer.material = ray_shader_goal;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    remainingLength -= Vector3.Distance(ray.origin, hit.point);
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                    if (hit.collider.tag == "goal")
                        break;

                }
            }
        }
    }
}