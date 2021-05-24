using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WTUtils
{
    public class WTTimer
    {
        private float maxTime;
        private float currentTime;
        private bool timeStarted;
        private float lastUpdatedTime;
        private bool clamp;

        public WTTimer(float time, bool startNow = false, bool clamp = true)
        {
            this.maxTime = time;
            this.currentTime = 0;
            lastUpdatedTime = Time.time;
            this.clamp = clamp;
            timeStarted = false;
            if (startNow)
            {
                Start();
            }
        }

        public void Start() { timeStarted = true; lastUpdatedTime = Time.time; }
        public void Pause() => timeStarted = false;
        public void Stop() { timeStarted = false; currentTime = 0; }
        public void Restart() { timeStarted = true; currentTime = 0; lastUpdatedTime = Time.time; }
        private void UpdateTime()
        {
            if (timeStarted)
            {
                currentTime += Time.time - lastUpdatedTime;
                lastUpdatedTime = Time.time;
                if (clamp)
                {
                    currentTime = Mathf.Clamp(currentTime, 0, maxTime);
                }
            }
        }

        public float GetCurrentTime() { UpdateTime(); return currentTime; }
        public float GetCurrentTimePercent() { UpdateTime(); return currentTime / maxTime; }
        public float GetTimeLeft() { UpdateTime(); return maxTime - currentTime; }
        public bool IsTimerDone() { UpdateTime(); return currentTime - maxTime >= 0; }
        public float GetCycles() { UpdateTime(); return currentTime / maxTime; }
        public int GetFullCycles() { UpdateTime(); return Mathf.FloorToInt(currentTime / maxTime); }
        public bool IsRunning() { return timeStarted; }
    }
}

