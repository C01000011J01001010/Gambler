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
        // x축 값을 사용함
        Vector2 contentCellSize = viewPortRectTrans.rect.size; 

        // y는 스크롤축이니 그대로 유지
        contentCellSize.y = contentGrid.cellSize.y; 

        // x축에서 여백 제외한 모든 공간을 값으로 정함
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

            // 행개수 : ex) (자식개수가 4, 제약개수가 4) -> (3/4 + 1 = 1)
            int rowCount = ((ActiveObjList.Count - 1) / (contentGrid.constraintCount)) + 1;
            Debug.Log($"rowCount : {rowCount}");

            int ColumnCount = contentGrid.constraintCount;
            Debug.Log($"ColumnCount : {ColumnCount}");

            // 셀의 좌우 여백 및 한 행의 셀의 x축 크기 * 셀의 개수, 셀 사이의 여백을 더함
            size.x = contentGrid.padding.left + contentGrid.padding.right +
                contentGrid.cellSize.x * ColumnCount +
                contentGrid.spacing.x * (ColumnCount - 1);
            Debug.Log($"size.x : {size.x}");

            size.y = contentGrid.padding.top + contentGrid.padding.bottom +
                contentGrid.cellSize.y * rowCount +
                contentGrid.spacing.y * (rowCount - 1);
            Debug.Log($"size.y : {size.y}");

            contentTrans.sizeDelta = size;

            Debug.Log("CONTENT 맞춤설정 완료");

        }
        else
        {
            Debug.LogWarning("contentGrid null");
        }

    }
}
