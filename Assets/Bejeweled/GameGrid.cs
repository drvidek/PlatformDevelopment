using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameGrid : MonoBehaviour
{
    [SerializeField] private GamePiece jewel;
    // Start is called before the first frame update
    void Start()
    {
        FillGrid();
    }

    public void FillGrid()
    {
        for (int x = -4; x < 5; x++)
        {
            for (int y = 4; y > -5; y--)
            {
                Instantiate(jewel, new(x, y), Quaternion.identity, transform).Initialise();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
