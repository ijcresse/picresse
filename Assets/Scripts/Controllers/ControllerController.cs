using System;
using System.Collections.Generic;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public struct ControlData
    {
        public List<bool> movementKey;

        public List<bool> actionKey;

        public bool firstPausePress;
        public bool isPaused;

        public bool firstMovePress;
        public bool firstActionPress;

        public float timerThreshold;
        public float repeatSpeed;
        public float repeatTimer;

        private int lastMove;
        (int x, int y) lastUpdatedPositions;
        private int lastAction;
        private int lastActionResult;

        //TODO: figure out why Constants isn't getting recognized by ControlData
        public const int KEY_LEFT = 0;
        public const int KEY_RIGHT = 1;
        public const int KEY_UP = 2;
        public const int KEY_DOWN = 3;

        public const int KEY_CLEAR = 0;
        public const int KEY_FILL = 1;
        public const int KEY_CROSS = 2;

        public ControlData(float threshold, float repeatSpeed)
        {
            //left, right, down, up
            movementKey = new() { false, false, false, false };
            //fill, cross, hint
            actionKey = new() { false, false, false };

            firstPausePress = false;
            isPaused = false;

            firstMovePress = false;
            firstActionPress = false;

            lastMove = -1;
            lastUpdatedPositions = (0, 0);
            lastAction = -1;
            lastActionResult = -1;

            repeatTimer = 0.0f;
            timerThreshold = threshold;
            this.repeatSpeed = repeatSpeed + threshold;
        }

        public int GetMove()
        {
            return movementKey.IndexOf(true);
        }

        public void Move(Action<int> moveFunc)
        {
            int move = GetMove();
            if (move != -1)
            {
                if (repeatTimer == 0f)
                {
                    firstMovePress = true;
                }
                repeatTimer += Time.deltaTime;
            } else
            {
                repeatTimer = 0f;
            }

            if (firstMovePress || lastMove != move)
            {
                moveFunc(move);
            }
            else if (repeatTimer > timerThreshold)
            {
                if (repeatTimer > repeatSpeed)
                {
                    repeatTimer = timerThreshold;
                    moveFunc(move);
                }
            }
            firstMovePress = false;
            lastMove = move;
        }

        public int GetAction()
        {
            return actionKey.IndexOf(true);
        }

        public void Action(
            int x, int y, 
            Func<int, int, int, int> setCellState, 
            Action<int, int, int> overwriteCellState)
        {
            int action = GetAction();
            if (action != -1)
            {
                if (firstActionPress)
                {
                    lastActionResult = setCellState(x, y, action);
                    firstActionPress = false;
                //don't trigger update if it's the same action on the same box as last time
                } else if ((lastUpdatedPositions.x != x || lastUpdatedPositions.y != y) || action != lastAction)
                {
                    if (GetMove() != -1)
                    {
                        overwriteCellState(x, y, lastActionResult);
                    } else
                    {
                        lastActionResult = setCellState(x, y, action);
                    }
                }
                lastUpdatedPositions = (x, y);
                lastAction = action;
            }
        }

        public void SetKeyState()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movementKey[KEY_LEFT] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                movementKey[KEY_LEFT] = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movementKey[KEY_RIGHT] = true;
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                movementKey[KEY_RIGHT] = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movementKey[KEY_UP] = true;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                movementKey[KEY_UP] = false;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movementKey[KEY_DOWN] = true;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                movementKey[KEY_DOWN] = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                actionKey[KEY_FILL] = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                actionKey[KEY_FILL] = false;
                firstActionPress = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                actionKey[KEY_CROSS] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                actionKey[KEY_CROSS] = false;
                firstActionPress = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                actionKey[KEY_CLEAR] = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                actionKey[KEY_CLEAR] = false;
                firstActionPress = true;
            }
            //TODO: add hint mechanic

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!firstPausePress)
                {
                    firstPausePress = true;
                    isPaused = !isPaused;
                }
            } else if (Input.GetKeyUp(KeyCode.Escape))
            {
                firstPausePress = false;
            }
        }
    }
}
