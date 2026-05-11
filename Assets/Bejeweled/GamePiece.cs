using UnityEngine;

public abstract class GamePiece : MonoBehaviour//, ITouchable
{
    private Vector2 offset;

    protected SpriteRenderer spriteRenderer;

    protected Color colorBase;

    protected Grid grid;

    public abstract void Initialise(Grid grid);

    protected virtual void Secure()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        colorBase = spriteRenderer.color;
    }

    public void SnapToGrid()
    {
        Vector3Int cell = grid.WorldToCell(transform.position);
        Vector3 cellCentre = grid.GetCellCenterWorld(cell);
        transform.position = cellCentre;
    }

}
