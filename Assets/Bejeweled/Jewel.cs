using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bejeweled
{
    public class Jewel : GamePiece
    {
        public enum Shape
        {
            Square,
            Circle,
            Diamond,
            Triangle,
            Hexagon,
            Capsule
        }

        [SerializeField] private Shape shape;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color[] colors = new Color[6];
        private Grid grid;
        private Vector3 lastPosition;

        private void OnValidate()
        {
            base.Start();
            ApplyShape(shape);
        }

        private void ApplyShape(Shape shape)
        {
            spriteRenderer.transform.localScale = Vector3.one * 0.75f;
            spriteRenderer.sprite = sprites[(int)shape];
            colorBase = colors[(int)shape];
            spriteRenderer.color = colorBase;

            switch (shape)
            {
                case Shape.Square:
                case Shape.Circle:
                case Shape.Hexagon:
                    break;
                case Shape.Diamond:
                    spriteRenderer.transform.localEulerAngles = new Vector3(0, 0, 45);
                    spriteRenderer.transform.localScale = Vector3.one * 0.6f;
                    break;
                case Shape.Triangle:
                    spriteRenderer.transform.localPosition = new Vector3(0, -0.2f * spriteRenderer.transform.localScale.x, 0);
                    break;
                case Shape.Capsule:
                    spriteRenderer.transform.localScale += Vector3.right * 0.15f;
                    spriteRenderer.transform.localScale -= Vector3.up * 0.3f;
                    break;
            }
        }

        public override void OnTouchEnd(Vector3 screenPosition)
        {
            base.OnTouchEnd(screenPosition);
            if (Physics.Raycast(transform.position + Vector3.back, Vector3.forward, out RaycastHit hit, 3f))
            {
                if (hit.collider.TryGetComponent<Jewel>(out Jewel j))
                    j.SwapTo(lastPosition);
            }
            SnapToGrid();
        }

        public void SwapTo(Vector3 location)
        {
            transform.position = location;
            SnapToGrid();
        }

        private void SnapToGrid()
        {
            Vector3Int cell = grid.WorldToCell(transform.position);
            Vector3 cellCentre = grid.GetCellCenterWorld(cell);
            transform.position = cellCentre;
            lastPosition = transform.position;
        }

        public override void Initialise()
        {
            int shapeNum = Random.Range(0, 6);
            shape = (Shape)shapeNum;
            base.Start();
            grid = GetComponentInParent<Grid>();
            ApplyShape(shape);
            SnapToGrid();
        }
    }
}
