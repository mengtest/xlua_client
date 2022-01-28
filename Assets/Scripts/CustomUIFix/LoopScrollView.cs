using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public enum ScrollType{
    Horizontal,
    Virtical
}

//�ɸ���Item��ScrollView��ֻ֧�ֵ��򻬶���ˮƽ/��ֱ������
public class LoopScrollView : MonoBehaviour
{
    [Header("�̶�Item����")]
    public int fixedCount = 10;

    //����item������
    public int totalCount = 20;

    [Header("ItemԤ��")]
    public GameObject celltempObj;

    //itemԤ�ƴ�С
    public Vector2 cellSize = new Vector2(100,100);

    private RectTransform contentRectTrans;
    private ScrollRect scrollRect;
    private ScrollType scrollType = ScrollType.Horizontal;
    private GridLayoutGroup gridLayout;

    //ScrollView����ʾ�ĵ�һ��item������
    private int headRow = 0;
    //ScrollView����ʾ�ĵ�һ��item������
    private int headIndex = 0;
    //���һ��item������
    private int tailRow = 0;
    //ScrollView����ʾ�����һ��item������
    private int tailIndex = 0;

    //ÿ��/����ʾrowCount��item
    [Header("ÿ��/����ʾ��item����")]
    public int constriantCount = 0;
    //�����/����
    private int maxRowColumn = 0;

    //����item�б�
    private List<RectTransform> itemsPool = new List<RectTransform>();

    private bool init_suc = true;

    //ÿ��item�ĸ߶�/ÿ��item�ĳ���
    private float sizeXY = 0;

    private Vector2 leftUpperAnchor = new Vector2(0, 1);
    private Vector2 rightUpperAnchor = new Vector2(1, 1);
    private Vector2 leftBottomAnchor = new Vector2(0, 0);


    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        if(scrollRect == null)
        {
            Debug.LogError("[loopscrollview] Not Find ScrollRect Component.");
            init_suc = false;
            return;
        }
        contentRectTrans = scrollRect.content;
        scrollRect.onValueChanged.AddListener((Vector2 vec) => OnScrollMove(vec));
        if (scrollRect.horizontal)
        {
            scrollType = ScrollType.Horizontal;

            contentRectTrans.anchorMin = leftBottomAnchor;
            contentRectTrans.anchorMax = leftUpperAnchor;
        }
        else
        {
            scrollType = ScrollType.Virtical;

            contentRectTrans.anchorMin = leftUpperAnchor;
            contentRectTrans.anchorMax = rightUpperAnchor;
        }
       // Debug.Log("[loopscrollview] scrollType = "+ scrollType.ToString());

        InitLayout();
        tailRow = Mathf.CeilToInt(fixedCount / constriantCount) - 1;
        maxRowColumn = Mathf.CeilToInt(totalCount / constriantCount) - 1;
        InitItems();
        InitContentSize();
    }


    //��ʼ��fixedCount��item
    private void InitItems()
    {
        if(celltempObj == null)
        {
            Debug.LogError("[loopscrollview] celltempObj null");
            init_suc = false;
            return;
        }
        for (int i = 0; i < fixedCount; i ++)
        {
            GameObject obj = Instantiate(celltempObj, contentRectTrans);
            obj.SetActive(true);
            RectTransform trans = obj.GetComponent<RectTransform>();
            itemsPool.Add(trans);
            InitItem(trans, i);
        }
    }

    //��ʼ��GridLayoutGroup����
    private void InitLayout()
    {
        gridLayout = contentRectTrans.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            Debug.LogError("[loopscrollview] Content has no GridLayoutGroup Component.");
            init_suc = false;
            return;
        }
        gridLayout.cellSize = cellSize;
       
        if (scrollType == ScrollType.Horizontal)
        {
            gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
            gridLayout.childAlignment = TextAnchor.UpperLeft;

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            sizeXY = gridLayout.padding.left + cellSize.x + gridLayout.spacing.x;
        }
        else if (scrollType == ScrollType.Virtical)
        {
            gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            gridLayout.childAlignment = TextAnchor.UpperLeft;

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            sizeXY = gridLayout.padding.top + cellSize.y + gridLayout.spacing.y;
        }
        gridLayout.constraintCount = constriantCount;
    }

    //��ʼ��Content size
    private void InitContentSize()
    {
        Vector2 size = new Vector2(0, 0);
        //������/��
        if (scrollType == ScrollType.Virtical)
        {
            
            size.y = gridLayout.padding.top + (maxRowColumn + 1) * (gridLayout.cellSize.y + gridLayout.spacing.y) + gridLayout.padding.bottom;
        }
        else
        {
            float width = gridLayout.padding.left + (maxRowColumn + 1) * (gridLayout.cellSize.x + gridLayout.spacing.x) + gridLayout.padding.right;
            size.x = width;
        }
        //Debug.Log("[loopscrollview] size = " + size.ToString());
        contentRectTrans.sizeDelta = size;
    }

    //item��ʾ�߼�
    private void InitItem(RectTransform trans,int idx)
    {
        trans.name = (idx).ToString();
        trans.GetComponentInChildren<TextMeshProUGUI>().text = idx.ToString();

        float posx = 0;
        float posy = 0;
        if(scrollType == ScrollType.Virtical)
        {
            int n = idx % constriantCount;
            posx = gridLayout.padding.left + n * (gridLayout.cellSize.x + gridLayout.spacing.x);

            int row = Mathf.CeilToInt(idx / constriantCount);
            posy = gridLayout.padding.top + row * (gridLayout.cellSize.y + gridLayout.spacing.y);
            posy = -posy;
        }
        else
        {
            int n = idx % constriantCount;
            posy = gridLayout.padding.top + n * (gridLayout.cellSize.y + gridLayout.spacing.y);
            posy = -posy;

            int column = Mathf.CeilToInt(idx / constriantCount);
            posx = gridLayout.padding.left + column * (gridLayout.cellSize.x + gridLayout.spacing.x);
        }

        trans.anchoredPosition = new Vector2(posx, posy);
    }

    private void OnScrollMove(Vector2 vec)
    {
        if(!init_suc)
        {
            Debug.LogError("[loopscrollview] init failed.");
            return;
        }
        if(itemsPool.Count <= 0)
        {
            Debug.LogError("[loopscrollview] Item Pool empty");
            return;
        }
        if(scrollType == ScrollType.Virtical)
        {
            OnVirticleScrollMove(vec);
        }
        else
        {
            OnHorizontalScrollMove(vec);
        }
    }

    //���»���
    private void OnVirticleScrollMove(Vector2 vec)
    {
        // Debug.Log("[loopscrollview]  contentRectTrans.anchoredPosition = " + contentRectTrans.anchoredPosition.y.ToString());
        //���ϻ�
        while (contentRectTrans.anchoredPosition.y >= (headRow + 1) * sizeXY && tailRow < maxRowColumn)
        {
            //Debug.LogError("���ϻ� headRow = "+ headRow.ToString());
            //����headIndex��item�ƶ������
            int itemCount = constriantCount;
            if (headRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }

            int startIdx = (tailRow + 1) * constriantCount;
            for (int i = 0; i < itemCount; i ++)
            {
                RectTransform item = itemsPool[0];
                itemsPool.Remove(item);
                itemsPool.Add(item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow + 1;
            tailRow = tailRow + 1;
            //Debug.Log("headRow = " + headRow.ToString() + "tailRow = "+ tailRow.ToString() + "maxRowColumn = "+maxRowColumn.ToString());
        }

        //���»�
       while(contentRectTrans.anchoredPosition.y <= headRow * sizeXY && headRow > 0)
        {
            //Debug.LogError("���»�");
            //�����һ�е�item�ƶ���ǰ��
            int itemCount = constriantCount;
            if (tailRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }
            int startIdx = (headRow - 1) * constriantCount;
            for (int i = 0; i < itemCount; i++)
            {
                RectTransform item = itemsPool[itemsPool.Count-1];
                itemsPool.Remove(item);
                itemsPool.Insert(0,item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow - 1;
            tailRow = tailRow - 1;
        }
    }
    
    //���һ���
   private void OnHorizontalScrollMove(Vector2 vec)
   {
        //���һ�
        while (-contentRectTrans.anchoredPosition.x >= (headRow + 1) * sizeXY && tailRow < maxRowColumn)
        {
            //Debug.LogError("���һ� headRow = " + headRow.ToString());
            //����headRow��item�ƶ������
            int itemCount = constriantCount;
            if (headRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }

            int startIdx = (tailRow + 1) * constriantCount;
            for (int i = 0; i < itemCount; i++)
            {
                RectTransform item = itemsPool[0];
                itemsPool.Remove(item);
                itemsPool.Add(item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow + 1;
            tailRow = tailRow + 1;
            //Debug.Log("headRow = " + headRow.ToString() + "tailRow = " + tailRow.ToString() + "maxRowColumn = " + maxRowColumn.ToString());
        }

        //����
        while (-contentRectTrans.anchoredPosition.x <= headRow * sizeXY && headRow > 0)
        {
            //Debug.LogError("����");
            //�����һ�е�item�ƶ���ǰ��
            int itemCount = constriantCount;
            if (tailRow >= maxRowColumn)
            {
                itemCount = totalCount - maxRowColumn * constriantCount;
            }
            int startIdx = (headRow - 1) * constriantCount;
            for (int i = 0; i < itemCount; i++)
            {
                RectTransform item = itemsPool[itemsPool.Count - 1];
                itemsPool.Remove(item);
                itemsPool.Insert(0, item);
                InitItem(item, startIdx + i);
            }
            headRow = headRow - 1;
            tailRow = tailRow - 1;
        }
    }

}
