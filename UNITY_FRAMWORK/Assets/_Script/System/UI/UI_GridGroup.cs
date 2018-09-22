using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_GridGroup : MonoBehaviour {

    public Transform parent;
    public float width        = 100f;
    public float height       = 100f;
    public float peddingX     = 10;
    public float peddingY     = 10;
    public Vector3 startArray = new Vector3(300, -200, 0);
    public int length;
  

    [HideInInspector]
    public IList<Rect> lsRect = new List<Rect>();                   //그리드의 좌표
    [HideInInspector]
    public IList<Vector3> lsCenter = new List<Vector3>();           //그리드이 센터 
    [HideInInspector]
    public  IList<Vector3> lsPostions = new List<Vector3>();         //그리드의 tranformPosition;
    [HideInInspector]
    public IList<RectTransform> lsrcTransforms= new List<RectTransform>(); //slot그리드에 배치될 오브젝트들
   
    [HideInInspector]
    public IList<UI_Panel_Item> lsItems = new List<UI_Panel_Item>();  //이동시켜야할 panel


    

    private void Start()
    {
        //배열의 좌표에 오브젝트를 넣는다.
        SetRectTransform();
        for (int i = 0; i < length; i++)
        {
            //rect -> rectTranform으로 적용
            lsrcTransforms[i].anchoredPosition = new Vector2(lsCenter[i].x, lsCenter[i].y);

        }
    }

   public void SetRectTransform()
    {
        length = parent.childCount;

        for (int i = 0; i < length; i++)
        {
            Rect rt = new Rect(startArray.x + width * ((i % 4) + 1) + peddingX * (i % 4),
                               startArray.y - height * ((i / 4) + 1) - peddingY * (i / 4),
                               10f, 10f);

            Vector3 center = new Vector3(rt.x - (width * 0.5f), startArray.y + rt.y + (height * 0.5f), 0);
            //slots
            RectTransform child = parent.GetChild(i).GetComponentInChildren<RectTransform>();

            string name = parent.GetChild(i).GetComponentInChildren<RectTransform>().name;
            lsRect.Add(rt);
            lsCenter.Add(center);
            lsrcTransforms.Add(child);


        }
    }



}
