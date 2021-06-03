using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject crushHeart;
    [SerializeField]
    private TextMeshProUGUI comboText;
    [SerializeField]
    private TextMeshProUGUI waitTimeText;

    private YieldInstruction waitOneSecond = new WaitForSeconds(1.0f);
    private int nowCombo = 0;
    private bool playerCanDie;
}

public partial class UIManager : MonoBehaviour
{
    private void OnEnable()
    {
        waitTimeText.text = "15";
        comboText.text = "0";

        crushHeart.gameObject.SetActive(false);
        waitTimeText.gameObject.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.actPlayerDie += PlayerGameOver;
        GameManager.Instance.actEnemyDie += ComboUp;
        GameManager.Instance.actPlayerDamage += PlayerGetDamage;
    }
    private void ComboUp()
    {
        if(nowCombo <= 8)
        {
            nowCombo++;
            comboText.text = nowCombo.ToString();
        }    
    }
    private void PlayerGetDamage()
    {
        if(playerCanDie == true)
        {
            StopAllCoroutines();
            GameManager.Instance.playerDie(this);
        }
        crushHeart.gameObject.SetActive(true);
        waitTimeText.gameObject.SetActive(true);
        nowCombo = nowCombo == 8 ? 4 : 0;
        comboText.text = nowCombo.ToString();
        StartCoroutine(wait15Second());
    }
    IEnumerator wait15Second()
    {
        playerCanDie = true;
        for (int i = 15; i >= 0; i--)
        {
            yield return waitOneSecond;
            waitTimeText.text = i.ToString();
        }
        crushHeart.gameObject.SetActive(false);
        waitTimeText.gameObject.SetActive(false);
        playerCanDie = false;
        yield break;
    }
    private void PlayerGameOver()
    {
        gameOverText.SetActive(true);
    }

}
