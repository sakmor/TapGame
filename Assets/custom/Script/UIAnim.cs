using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim : MonoBehaviour
{
    public Image TargetImage;

    public List<Sprite> IdelAnim = new List<Sprite>();
    public List<Sprite> Atk1Anim = new List<Sprite>();
    public List<Sprite> Atk2Anim = new List<Sprite>();
    [Header("¥Ø«e¼½©ñ")]
    private int mSpriteIndex = 0;
    private int mControlIndex = 0;
    private Coroutine mAnimCo;
    Action OnAnimDone;
    private bool mAtkDone = true;
    private void Start()
    {
        AnimControl();
    }

    public void AnimControl(int controlIndex = 1, Action onDone = null, Action onSound = null)
    {
        if (mAtkDone == false)//(controlIndex > 1 && mControlIndex > 1)
            return;
        onSound?.Invoke();
        OnAnimDone = onDone;
        mControlIndex = controlIndex;
        mSpriteIndex = 0;
        List<Sprite> spriteList = new List<Sprite>();
        float speed = 0.05f;
        switch (controlIndex)
        {
            case 3:
                speed = 0.02f;
                spriteList = Atk2Anim;
                break;
            case 2:
                speed = 0.02f;
                spriteList = Atk1Anim;
                break;
            case 1:
            default:
                spriteList = IdelAnim;
                break;
        }

        if (controlIndex > 1)
            mAtkDone = false;

        if (mAnimCo != null)
            StopCoroutine(mAnimCo);

        if (spriteList != null)
            mAnimCo = StartCoroutine(CoAnim(spriteList, speed));
    }

    IEnumerator CoAnim(List<Sprite> animList, float speed = 0.05f)
    {
        while (animList != null && animList.Count > 0)
        {
            yield return new WaitForSeconds(speed);
            TargetImage.sprite = animList[mSpriteIndex];
            mSpriteIndex++;

            if (mAtkDone == false && mControlIndex == 2 && mSpriteIndex > 1)
            {
                mAtkDone = true;
                OnAnimDone?.Invoke();
            }
            if (animList.Count <= mSpriteIndex)
            {

                if (mAtkDone == false)
                {
                    mAtkDone = true;
                    OnAnimDone?.Invoke();
                }
                if (mControlIndex == 1)
                    mSpriteIndex = 0;
                else
                    AnimControl();
            }
        }
    }
}
