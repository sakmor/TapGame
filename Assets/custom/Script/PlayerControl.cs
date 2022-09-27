using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameMain Main;

    public UIAnim PlayerAnimControl;
    public UIAnim Sup1AnimControl;
    public UIAnim Sup2AnimControl;
    public int Attack1HP = 1;
    public int Attack2HP = 2;
    public int SUBAttackHP = 3;
    public Button Atk1Btn;
    public Button Atk2Btn;
    public Button Sub3Btn;
    public Image SubCDImage;
    public Text SubCDText;
    public float CDTime = 0;
    public float S_CDTime = 3;

    [Header("Sound")]
    public AudioClip Atk1Sound;
    public AudioClip Atk2Sound;
    public AudioClip ConSound;

    // Start is called before the first frame update
    void Start()
    {
        Atk1Btn.onClick.AddListener(() =>
        {

            PlayerAnimControl.AnimControl(2, () => Main.PlayerAttack(Attack1HP), () => { Main.PlaySound(Atk1Sound); });
        });
        Atk2Btn.onClick.AddListener(() =>
        {
            PlayerAnimControl.AnimControl(3, () => Main.PlayerAttack(Attack2HP), () => { Main.PlaySound(Atk2Sound); });
        });
        Sub3Btn.onClick.AddListener(() =>
        {
            

            Sup1AnimControl.AnimControl(2, () => Main.PlayerAttack(SUBAttackHP), () => { Main.PlaySound(ConSound); });
            Sup2AnimControl.AnimControl(2, () => Main.PlayerAttack(SUBAttackHP), () => { Main.PlaySound(ConSound); });
            CDTime = S_CDTime;
            Sub3Btn.interactable = false;
            SubCDImage.gameObject.SetActive(true);
            StartCoroutine(CO_SubCD());
        });
    }

    IEnumerator CO_SubCD()
    {
        while (CDTime >= 0)
        {
            SubCDText.text = CDTime.ToString("#0");
            SubCDImage.fillAmount = CDTime / S_CDTime;
            yield return new WaitForEndOfFrame();
            CDTime = CDTime - Time.deltaTime;
        }
        Sub3Btn.interactable = true;
        SubCDImage.gameObject.SetActive(false);
    }

}
