using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Controllers
{
    public struct ControlData
    {
        public List<bool> movementKey;

        public List<bool> actionKey;

        public bool firstPress;
        public float timerThreshold;
        public float repeatSpeed;
        public float repeatTimer;

        public ControlData(float threshold, float repeatSpeed)
        {
            //left, right, down, up
            movementKey = new() { false, false, false, false };
            //fill, cross
            actionKey = new() { false, false };
            firstPress = false;
            repeatTimer = 0.0f;
            timerThreshold = threshold;
            this.repeatSpeed = repeatSpeed + threshold;
        }

        public int GetMove()
        {
            return movementKey.IndexOf(true);
        }

        public int GetAction()
        {
            return actionKey.IndexOf(true);
        }

        public bool CanRepeat()
        {
            if (!firstPress)
            {
                if (repeatTimer > timerThreshold)
                {
                    if (repeatTimer % repeatSpeed == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Clear()
        {
            movementKey = Enumerable.Repeat(false, movementKey.Count).ToList();
            actionKey = Enumerable.Repeat(false, actionKey.Count).ToList();
            firstPress = false;
            repeatTimer = 0.0f;
        }
    }
}
