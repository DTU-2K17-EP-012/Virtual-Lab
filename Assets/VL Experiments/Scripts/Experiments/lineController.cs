using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Content
{
    public class lineController : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        [SerializeField] private Transform[] points;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            SetupRays();
        }

        public void SetupRays()
        {
            lineRenderer.positionCount = points.Length;
            this.points = points;
        }

        private void Update()
        {
            for (int i = 0; i < points.Length; i++)
            {
                lineRenderer.SetPosition(i, points[i].position);
            }
        }
    }
}