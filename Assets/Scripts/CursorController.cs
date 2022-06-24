using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private const int TOP_LAYER = 3;
    private (int x, int y) gamePosition = (0, 0);
    private Vector2 screenStartPosition;

    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = TOP_LAYER;
        Debug.Log($"cursor starting position: ({gameObject.transform.position.x}, {gameObject.transform.position.y})");
        screenStartPosition = gameObject.transform.position;
    }

    public void Move(float xSize, float ySize, float boxSize, int xMove = 0, int yMove = 0) {
        if (xMove == 0 && yMove == 0) {
            Debug.Log("Error: asked to move in no direction at all.");
        }
        if (xMove != 0 && yMove != 0) {
            Debug.Log($"Can't move two directions at once! ({xMove},{yMove})");
        }

        Vector2 adjustedPos = screenStartPosition;
        if (xMove != 0) {
            if (xMove + gamePosition.x < 0) {
                gamePosition.x = (int) xSize - 1;
                adjustedPos = new Vector2(screenStartPosition.x + (boxSize * (xSize - 1)), gameObject.transform.position.y);
            } else if (xMove + gamePosition.x >= xSize) {
                gamePosition.x = 0;
                adjustedPos = new Vector2(screenStartPosition.x, gameObject.transform.position.y);
            } else {
                gamePosition.x += xMove;
                adjustedPos = new Vector2(gameObject.transform.position.x + xMove * boxSize, gameObject.transform.position.y);
            }
        }
        if (yMove != 0) {
            if (yMove + gamePosition.y < 0) {
                gamePosition.y = (int) ySize - 1;
                adjustedPos = new Vector2(gameObject.transform.position.x, screenStartPosition.y - (boxSize * (ySize - 1)));
            } else if (yMove + gamePosition.y >= ySize) {
                gamePosition.y = 0;
                adjustedPos = new Vector2(gameObject.transform.position.x, screenStartPosition.y);
            } else {
                gamePosition.y += yMove;
                adjustedPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - yMove * boxSize);
            }
        }
        gameObject.transform.position = adjustedPos;
        Debug.Log($"cursorCtrl.Move: gamePosition: ({gamePosition.x}, {gamePosition.y}). screenPosition: ({adjustedPos.x},{adjustedPos.y})");
    }
}
