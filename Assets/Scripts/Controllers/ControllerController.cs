using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Controllers
{
    public struct ControlData
    {
        public List<bool> movementKey;

        public List<bool> actionKey;

        public bool firstMovePress;
        public bool firstActionPress;
        public float timerThreshold;
        public float repeatSpeed;
        public float repeatTimer;

        public ControlData(float threshold, float repeatSpeed)
        {
            //left, right, down, up
            movementKey = new() { false, false, false, false };
            //fill, cross, clear
            actionKey = new() { false, false, false};
            firstMovePress = false;
            firstActionPress = false;
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
    }
}
