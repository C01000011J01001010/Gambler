using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PanelBase_ConstContent : MonoBehaviour
{
    // ������ ����
    public RectTransform viewPortRectTrans;
    public RectTransform contentRectTrans;
    public GridLayoutGroup contentGrid;
    public ScrollRect scrollRect;  // ScrollRect ������Ʈ�� ����

    private int childCount;

    private void Start()
    {
        AdjustContentCellSize();
        ChangeContentRectTransform();
        ScrollToTop();
    }

    bool IsPrefabLinked(GameObject obj)
    {
        // ���� ��ü�� ������ �ν��Ͻ����� Ȯ��
        var prefabType = PrefabUtility.GetPrefabAssetType(obj);
        return prefabType != PrefabAssetType.NotAPrefab;
    }

    protected virtual void AdjustContentCellSize()
    {
        // rect.size�� ���������� ������ ���¿��� ����ϸ� �ε�� ���� ũ��� �ٸ� ���� �ҷ��� �� ����(�ε� �� ũ��)
        // �̴� ���̾ƿ� ����� �Ϸ���� �ʾұ⿡ ����� ������ �ش� ������ ���� ó���ϵ��� ��������
        if (IsPrefabLinked(gameObject))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(viewPortRectTrans);
        }
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

    protected virtual void ChangeContentRectTransform()
    {
        childCount = contentRectTrans.childCount;

        if (contentGrid != null)
        {
            Vector2 size = Vector2.zero;

            // �ళ�� : ex) (�ڽİ����� 4, ���ళ���� 4) -> (3/4 + 1 = 1)
            int rowCount = ((childCount - 1) / (contentGrid.constraintCount)) + 1;
            //Debug.Log($"rowCount : {rowCount}");

            int ColumnCount = contentGrid.constraintCount;
            //Debug.Log($"ColumnCount : {ColumnCount}");

            // ���� �¿� ���� �� �� ���� ���� x�� ũ�� * ���� ����, �� ������ ������ ����
            size.x = contentGrid.padding.left + contentGrid.padding.right +
                contentGrid.cellSize.x * ColumnCount +
                contentGrid.spacing.x * (ColumnCount - 1);
            //Debug.Log($"size.x : {size.x}");

            size.y = contentGrid.padding.top + contentGrid.padding.bottom +
                contentGrid.cellSize.y * rowCount +
                contentGrid.spacing.y * (rowCount - 1);
            //Debug.Log($"size.y : {size.y}");

            contentRectTrans.sizeDelta = size;

            //Debug.Log("CONTENT ���㼳�� �Ϸ�");

        }
        else
        {
            Debug.LogWarning("contentGrid null");
        }

    }

    // ��ũ���� ���� �ø��� �Լ�
    public void ScrollToTop()
    {
        // ����Ʈ�� �����Ͽ� ��ũ�ѹٸ� ����
        scrollRect.verticalNormalizedPosition = 1f;  // 1f: �� ��
    }

}

