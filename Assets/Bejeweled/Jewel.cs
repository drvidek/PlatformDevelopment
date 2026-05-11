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

        [SerializeField] public Shape shape;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color[] colors = new Color[6];

        public static bool Match(Jewel jewelA, Jewel jewelB)
        {
            // If either shape is null, there's no match
            if (!jewelA || !jewelB)
                return false;

            return jewelA.shape == jewelB.shape;
        }

        private void OnValidate()
        {
            ApplyShape(shape);
        }

        private void ApplyShape(Shape shape)
        {
            Secure();
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
        public override void Initialise(Grid grid)
        {
            this.grid = grid;
            int shapeNum = Random.Range(0, 6);
            shape = (Shape)shapeNum;
            ApplyShape(shape);
            SnapToGrid();
        }
    }
}
