using System.Collections.Generic;
using UnityEngine;

namespace Bejeweled
{
    public class JewelGrid : GameGrid
    {
        private List<Jewel> jewelsScoredInRow = new();
        private List<Jewel> allJewelsScored = new();

        public const int comboMin = 3;
        protected override void OnGridContentsChanging(Vector3Int coord)
        {
            // Move the piece in the new location to the old location //

            // Get the piece which is being displaced at the new cell
            GamePiece displacedPiece = GetPieceAtCoord(coord);
            // Move it physically to the old cell
            SnapToCoordCentre(displacedPiece.transform, coordPickup);

            // Assign the piece to the old cell
            AssignPieceToCoord(displacedPiece, coordPickup);

            // Check neighbours at that cell
            CheckNeighbours(coordPickup);
        }

        protected override void OnGridContentsChanged(Vector3Int coord)
        {
            // Check the new location for neighbours
            CheckNeighbours(coord);
        }

        private Jewel GetJewelAtCoord(Vector3Int coord)
        {
            return GetPieceAtCoord(coord) as Jewel;
        }

        private void CheckNeighbours(Vector3Int coordStart)
        {
            allJewelsScored.Clear();

            Jewel jewelStart = GetJewelAtCoord(coordStart);

            jewelsScoredInRow.Clear();

            CheckNeighbour(coordStart, Vector3Int.right);
            CheckNeighbour(coordStart, Vector3Int.left);

            if (jewelsScoredInRow.Count >= comboMin - 1)
            {
                print($"Match {jewelsScoredInRow.Count + 1} horizontal");
                allJewelsScored.AddRange(jewelsScoredInRow);
            }

            jewelsScoredInRow.Clear();

            CheckNeighbour(coordStart, Vector3Int.up);
            CheckNeighbour(coordStart, Vector3Int.down);

            if (jewelsScoredInRow.Count >= comboMin - 1)
            {
                print($"Match {jewelsScoredInRow.Count + 1} vertical");
                allJewelsScored.AddRange(jewelsScoredInRow);
            }

            allJewelsScored.Add(jewelStart);

            foreach (Jewel jewel in allJewelsScored)
            {
                Destroy(jewel.gameObject);
            }
        }

        private void CheckNeighbour(Vector3Int coordStart, Vector3Int direction)
        {
            Vector3Int coordCheck = coordStart + direction;
            if (!ValidCoord(coordCheck))
            {
                return;
            }

            Jewel jewelA = GetJewelAtCoord(coordStart);
            Jewel jewelB = GetJewelAtCoord(coordCheck);

            if (Jewel.Match(jewelA, jewelB))
            {
                jewelsScoredInRow.Add(jewelB);
                CheckNeighbour(coordCheck, direction);
            }
        }

    }
}