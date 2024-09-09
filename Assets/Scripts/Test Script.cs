using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Grid grid;
    bool placed = false;
    void Update()
    {
        if (!placed)
        {
            Vector3 mouseToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseToWorldPoint.z = 0;
            Vector3 mouseCell = grid.CellToWorld(grid.WorldToCell(mouseToWorldPoint));
            mouseCell.x += (0.5f * grid.cellSize.x);
            mouseCell.y += (0.5f * grid.cellSize.y);
            this.transform.position = mouseCell;

            if (Input.GetMouseButtonDown(0))
            {
                placed = true;
            }
        }
    }
}
