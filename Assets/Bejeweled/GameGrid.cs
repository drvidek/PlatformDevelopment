using UnityEngine;

public abstract class GameGrid : MonoBehaviour
{
    public const int gridSize = 9;

    public const int gridSizeHalf = gridSize / 2;

    [SerializeField] private GamePiece prefabPiece;

    protected GamePiece selectedPiece;

    protected Grid grid;

    protected GamePiece[,] pieces = new GamePiece[gridSize, gridSize];

    protected Vector3Int coordPickup;


    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        FillGrid();
    }

    public void FillGrid()
    {
        for (int x = -gridSizeHalf; x < gridSizeHalf + 1; x++)
        {
            for (int y = gridSizeHalf; y > -gridSizeHalf - 1; y--)
            {
                GamePiece piecePlaced = Instantiate(prefabPiece, new(x, y), Quaternion.identity, transform);
                piecePlaced.Initialise(grid);
                Vector3Int coord = new Vector3Int(x, y);
                AssignPieceToCoord(piecePlaced, coord);
                SnapToCoordCentre(piecePlaced.transform, coord);
            }
        }
    }

    public bool ValidCoord(Vector3Int coord)
    {
        return coord.x >= -gridSizeHalf && coord.x <= gridSizeHalf && coord.y >= -gridSizeHalf && coord.y <= gridSizeHalf;
    }

    public void TrySelectPiece(Vector2 screenPosition)
    {
        if (selectedPiece)
            return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector3Int coord = grid.WorldToCell(pos);
        selectedPiece = GetPieceAtCoord(coord);
        if (selectedPiece)
        {
            coordPickup = coord;
        }
    }

    public void UpdateSelectedPiece(Vector2 screenPosition)
    {
        if (!selectedPiece)
            return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(screenPosition);
        pos.z = 0;
        selectedPiece.transform.position = pos;
    }

    public void DropSelectedPiece()
    {
        if (!selectedPiece)
            return;

        // Find the landing spot
        Vector3 landingPos = selectedPiece.transform.position;

        // Determine which cell matches that spot
        Vector3Int coordLanding = grid.WorldToCell(landingPos);

        // Signal the contents have changed at the given coordinate
        OnGridContentsChanging(coordLanding);

        // Once that's been processed, move the selected piece to the new cell
        SnapToCoordCentre(selectedPiece.transform, coordLanding);

        // Set the moved piece internally
        AssignPieceToCoord(selectedPiece, coordLanding);

        // Signal the contents are done changing
        OnGridContentsChanged(coordLanding);

        // De-select the piece
        selectedPiece = null;
    }

    /// <summary>
    /// Returns the game piece at the grid coordinate given, based on the internal array of pieces. 
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    protected GamePiece GetPieceAtCoord(Vector3Int coord)
    {
        if (!ValidCoord(coord))
            return null;
            
        // We have to offset by half the grid or we'll ask for a negative index
        return pieces[coord.x + gridSizeHalf, coord.y + gridSizeHalf];
    }

    /// <summary>
    /// Set the given piece at the given grid coordinate internally.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="coord"></param>
    protected void AssignPieceToCoord(GamePiece piece, Vector3Int coord)
    {
        pieces[coord.x + gridSizeHalf, coord.y + gridSizeHalf] = piece;
    }

    /// <summary>
    /// Physically place a transform in the centre of the grid square at the coordinate given
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="coord"></param>
    protected void SnapToCoordCentre(Transform transform, Vector3Int coord)
    {
        transform.position = grid.GetCellCenterWorld(coord);
    }

    /// <summary>
    /// What should occur immediately before a piece is assigned to a new location.
    /// </summary>
    /// <param name="coord"></param>
    protected abstract void OnGridContentsChanging(Vector3Int coord);

    /// <summary>
    /// What should occur immediately after a piece has been assigned to a new location.
    /// </summary>
    /// <param name="coord"></param>
    protected abstract void OnGridContentsChanged(Vector3Int coord);
}

