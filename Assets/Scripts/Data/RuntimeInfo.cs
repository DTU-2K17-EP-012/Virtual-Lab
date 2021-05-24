using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLab.Data
{
    public class RuntimeInfo : MonoBehaviour
    {
        public static RuntimeInfo Instance;
        public bool LoginPrompted = false;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                SetDefaults();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        private void SetDefaults()
        {
            LoginPrompted = false;
        }
    }
}

