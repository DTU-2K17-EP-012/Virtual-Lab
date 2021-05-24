using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLab.UI
{
    public class Tutorial : MonoBehaviour
    {
        public GameObject[] rules;

        [SerializeField]
        private int current = 0;

        public void NextRule()
        {
            rules[current].SetActive(false);
            current++;
            current = current % rules.Length;
            rules[current].SetActive(true);
        }

        public void PrevRule()
        {
            rules[current].SetActive(false);
            current--;
            current = (current == -1) ? (rules.Length - 1) : current;
            rules[current].SetActive(true);
        }
    }
}