using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Content
{
    public class lensManager : MonoBehaviour
    {
        [SerializeField] public float ri = 1.33f;
        public float GetRI()
        {
            return ri;
        }
    }
}