using UnityEngine;

public class QuestContentPopUp : PopUpBase<QuestContentPopUp>
{
    public QuestDescriptionPanel descriptionPanel {  get; private set; }

    Vector2 defaultSize;
    public void InitPopUp()
    {
        defaultSize = contentGrid.cellSize;
        InitializePool(1);
        GameObject obj = GetObject();
        descriptionPanel = obj.GetComponent<QuestDescriptionPanel>();
        ChangeContentRectTransform();
    }

    public void InitCententCellSize()
    {
        contentGrid.cellSize = defaultSize;
    }

    int sizeY = 150;
    public void AddContentCellSizeY()
    {
        Vector2 size = contentGrid.cellSize;
        size.y += sizeY;
        contentGrid.cellSize = size;
        ChangeContentRectTransform();
    }
}
