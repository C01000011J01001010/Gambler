using UnityEngine;

public abstract class PopUpBase_FullScreen<T_Class> : PopUpBase<T_Class> where T_Class : MonoBehaviour
{
    public RectTransform viewPortRectTrans;

    protected override void Awake()
    {
        AdjustContentCellSize();
    }

    

    protected virtual void AdjustContentCellSize()
    {
        // x�� ���� �����
        Vector2 contentCellSize = viewPortRectTrans.rect.size; 

        // y�� ��ũ�����̴� �״�� ����
        contentCellSize.y = contentGrid.cellSize.y; 

        // x�࿡�� ���� ������ ��� ������ ������ ����
        contentCellSize.x = contentCellSize.x
            - contentGrid.padding.left
            - contentGrid.padding.right
            - contentGrid.spacing.x * (contentGrid.constraintCount - 1);
        contentCellSize.x /= contentGrid.constraintCount;

        contentGrid.cellSize = contentCellSize;
    }

    protected override void ChangeContentRectTransform()
    {
        //InitAnchor();
        //InitGridRayout();

        if (contentGrid != null)
        {
            Vector2 size = Vector2.zero;

            // �ళ�� : ex) (�ڽİ����� 4, ���ళ���� 4) -> (3/4 + 1 = 1)
            int rowCount = ((ActiveObjList.Count - 1) / (contentGrid.constraintCount)) + 1;
            Debug.Log($"rowCount : {rowCount}");

            int ColumnCount = contentGrid.constraintCount;
            Debug.Log($"ColumnCount : {ColumnCount}");

            // ���� �¿� ���� �� �� ���� ���� x�� ũ�� * ���� ����, �� ������ ������ ����
            size.x = contentGrid.padding.left + contentGrid.padding.right +
                contentGrid.cellSize.x * ColumnCount +
                contentGrid.spacing.x * (ColumnCount - 1);
            Debug.Log($"size.x : {size.x}");

            size.y = contentGrid.padding.top + contentGrid.padding.bottom +
                contentGrid.cellSize.y * rowCount +
                contentGrid.spacing.y * (rowCount - 1);
            Debug.Log($"size.y : {size.y}");

            contentTrans.sizeDelta = size;

            Debug.Log("CONTENT ���㼳�� �Ϸ�");

        }
        else
        {
            Debug.LogWarning("contentGrid null");
        }

    }
}
