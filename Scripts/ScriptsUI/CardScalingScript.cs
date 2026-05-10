using UnityEngine;
using UnityEngine.UI;

public class DynamicGridScaler : MonoBehaviour
{
    public GridLayoutGroup grid;
    public RectTransform container;
    // limits
    public Vector2 minCellSize = new Vector2(40, 70);
    public Vector2 maxCellSize = new Vector2(180, 240);
    // card aspect ratio (width / height, e.g. 2:3)
    public float cardAspect = 2f / 3f;

    // update grid
    public void UpdateGrid(int cardCount)
    {
        if (cardCount <= 0) return;
        // container dimensions
        float width = container.rect.width;
        float height = container.rect.height;
        // column and row calculation
        float preferredWidth = 60f;
        // colum adjuster
        int columns = Mathf.Clamp(
            Mathf.Min(cardCount, Mathf.FloorToInt(width / preferredWidth)),
            2,
            7
        );

        int rows = Mathf.CeilToInt((float)cardCount / columns);
        // spacing adjustment
        float totalSpacingX = grid.spacing.x * (columns - 1);
        float totalSpacingY = grid.spacing.y * (rows - 1);
        // available space
        float availableWidth = width - totalSpacingX;
        float availableHeight = height - totalSpacingY;
        // max cell size
        float maxCellWidth = availableWidth / columns;
        float maxCellHeight = availableHeight / rows;
        // enforce aspect ratio
        float cellWidth = maxCellWidth;
        float cellHeight = cellWidth / cardAspect;
        // fit within height if needed
        if (cellHeight > maxCellHeight)
        {
            cellHeight = maxCellHeight;
            cellWidth = cellHeight * cardAspect;
        }
        // final clamped size
        Vector2 cellSize = new Vector2(
            Mathf.Clamp(cellWidth, minCellSize.x, maxCellSize.x),
            Mathf.Clamp(cellHeight, minCellSize.y, maxCellSize.y)
        );
        // apply to grid
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.cellSize = cellSize;
        // rebuild layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(container);
        // log
        Debug.Log($"Columns: {columns}, Rows: {rows}, Cell: {cellSize}");
    }
}