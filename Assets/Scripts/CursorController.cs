using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private const int TOP_LAYER = 3;
    private (int x, int y) gamePosition = (0, 0);

    void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = TOP_LAYER;
        Debug.Log($"cursor starting position: ({gameObject.transform.position.x}, {gameObject.transform.position.y})");
    }

    public bool Move(float xSize, float ySize, float boxSize, int xMove = 0, int yMove = 0) {
        bool moved = false;
        if (xMove > 0 && yMove > 0) {
            Debug.Log($"Can't move two directions at once! ({xMove},{yMove})");
        }
        if (xMove != 0) {
            if (gamePosition.x - 1 >= 0 || gamePosition.x + 1 <= xSize) {
                gamePosition.x += xMove;
                moved = true;
            }
        } else if (yMove != 0) {
            if (gamePosition.y - 1 >= 0 || gamePosition.y + 1 <= ySize) {
                gamePosition.y += yMove;
                moved = true;
            }
        }

        if (moved) {
            Vector2 adjustedPos = gameObject.transform.position + new Vector3(xMove * boxSize, yMove * boxSize, 0);
            Debug.Log($"start: ({gameObject.transform.position.x},{gameObject.transform.position.y}) :: moveTo: ({adjustedPos.x},{adjustedPos.y})");
            gameObject.transform.position = adjustedPos;
        }

        return moved;
    }
}
