using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLab.Content.UI
{
    public class LabelController : MonoBehaviour
    {
        // private MaterialPropertyBlock _mpblock;
        private RawImage _img;

        // Start is called before the first frame update
        void Awake()
        {
            _img = GetComponent<RawImage>();
            // _mpblock = new MaterialPropertyBlock();
        }

        private void OnMouseDown()
        {
            _img.materialForRendering.color = Color.red;
        }
    }
}