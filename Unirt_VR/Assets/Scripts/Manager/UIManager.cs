using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI; // 사망시 나올 UI
    [SerializeField]
    private GameObject crushHeart; // 데미지를 입으면 나오는 상태 UI
    [SerializeField]
    private TextMeshProUGUI comboText; // 현재 콤보수를 나타내는 UI
    [SerializeField]
    private TextMeshProUGUI waitTimeText; // 회복까지 남은 시간이 나오는 UI

    private YieldInstruction waitOneSecond = new WaitForSeconds(1.0f); // 1초를 대기하는 객체
    private int nowCombo = 0; // 현재 콤보수
    private bool playerCanDie; // 플레이어의 현재 상태를 나타내는 불리언
}


public partial class UIManager : MonoBehaviour
{

    private void Awake()
    {
        // 초기화를 진행한다.
        waitTimeText.text = "15";
        comboText.text = "0";
        gameOverUI.gameObject.SetActive(false);
        crushHeart.gameObject.SetActive(false);
        waitTimeText.gameObject.SetActive(false);
    }

    void Start()
    {
        // 필요한 이벤트 리스너들을 등록한다.
        GameManager.Instance.actPlayerDie += () => gameOverUI.SetActive(true);
        GameManager.Instance.actEnemyDie += ComboUp; 
        GameManager.Instance.actPlayerDamage += PlayerGetDamage;
    }

    // 최대 8콤보까지 증가한다.
    private void ComboUp()
    {
        if (nowCombo < 8)
        {
            nowCombo++;
            comboText.text = nowCombo.ToString();
        }
    }

    private void PlayerGetDamage()
    {
        // 이미 데미지를 입은 상태라면 게임 오버가 된다.
        if (playerCanDie == true)
        {
            StopAllCoroutines();
            GameManager.Instance.playerDie(this);
        }
        // 데미지를 표시하는 ui를 표시한다.
        crushHeart.gameObject.SetActive(true);
        waitTimeText.gameObject.SetActive(true);
        // 콤보가 깎인다.
        nowCombo = nowCombo == 8 ? 4 : 0;
        comboText.text = nowCombo.ToString();
        // 다시 회복할때까지 대기한다.
        StartCoroutine(wait15Second());
    }

    // 15초안에 데미지를 한번 더 입으면 죽는다.
    IEnumerator wait15Second()
    {
        playerCanDie = true; // 플레이어는 이제 죽을 수 있다.
        // 15초를 대기한다.
        for (int i = 15; i >= 0; i--)
        {
            yield return waitOneSecond;
            waitTimeText.text = i.ToString();
        }
        // 15초가 지났다면 다시 회복한다.
        crushHeart.gameObject.SetActive(false);
        waitTimeText.gameObject.SetActive(false);
        playerCanDie = false;
        yield break;
    }
}
