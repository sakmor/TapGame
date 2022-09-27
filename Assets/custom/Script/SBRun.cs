using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SBRun : MonoBehaviour
{
    public RectTransform RTrans;
    static int r = 0;
    void Start()
    {
        r = Random.Range(0, 2);
        float y;
        if (gameObject.name.Contains("L"))
        {
            y = r == 1 ? 0 : -130;
            RTrans.localPosition = new Vector3(RTrans.localPosition.x, y, RTrans.localPosition.z);
        }
        else
        {
            y = r == 1 ? -130 : 0;
            RTrans.localPosition = new Vector3(RTrans.localPosition.x, y, RTrans.localPosition.z);
        }


        RTrans.DOLocalMoveY(y == 0 ? -130 : 0, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

}
