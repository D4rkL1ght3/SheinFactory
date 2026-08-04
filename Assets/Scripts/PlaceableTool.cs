using UnityEngine;

public class PlaceableTool : MonoBehaviour
{
    public enum Direction
    {
        Down,
        Right,
        Up,
        Left
    }

    [Header("Tool Data")]
    [SerializeField] protected Vector2Int gridPosition;

    [SerializeField] protected Direction outputDirection = Direction.Down;

    // Called by the PlacementManager immediately after spawning.
    public virtual void Initialize(Vector2Int gridPos, int rotationIndex)
    {
        gridPosition = gridPos;

        outputDirection = (Direction)rotationIndex;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public Direction GetOutputDirection()
    {
        return outputDirection;
    }

    // Returns the neighboring tile where this tool outputs materials.
    public Vector2Int GetOutputGridPosition()
    {
        switch (outputDirection)
        {
            case Direction.Up:
                return gridPosition + Vector2Int.up;

            case Direction.Right:
                return gridPosition + Vector2Int.right;

            case Direction.Down:
                return gridPosition + Vector2Int.down;

            case Direction.Left:
                return gridPosition + Vector2Int.left;

            default:
                return gridPosition;
        }
    }

    // Called when a material enters this tool.
    public virtual bool ReceiveMaterial(MaterialItem item)
    {
        return false;
    }

    // Called when this tool outputs a material.
    protected virtual void OutputMaterial(GameObject material)
    {

    }
}