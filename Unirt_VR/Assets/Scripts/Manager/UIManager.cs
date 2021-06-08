using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI; // ����� ���� UI
    [SerializeField]
    private GameObject crushHeart; // �������� ������ ������ ���� UI
    [SerializeField]
    private TextMeshProUGUI comboText; // ���� �޺����� ��Ÿ���� UI
    [SerializeField]
    private TextMeshProUGUI waitTimeText; // ȸ������ ���� �ð��� ������ UI

    private YieldInstruction waitOneSecond = new WaitForSeconds(1.0f); // 1�ʸ� ����ϴ� ��ü
    private int nowCombo = 0; // ���� �޺���
    private bool playerCanDie; // �÷��̾��� ���� ���¸� ��Ÿ���� �Ҹ���
}


public partial class UIManager : MonoBehaviour
{

    private void Awake()
    {
        // �ʱ�ȭ�� �����Ѵ�.
        waitTimeText.text = "15";
        comboText.text = "0";
        gameOverUI.gameObject.SetActive(false);
        crushHeart.gameObject.SetActive(false);
        waitTimeText.gameObject.SetActive(false);
    }

    void Start()
    {
        // �ʿ��� �̺�Ʈ �����ʵ��� ����Ѵ�.
        GameManager.Instance.actPlayerDie += () => gameOverUI.SetActive(true);
        GameManager.Instance.actEnemyDie += ComboUp; 
        GameManager.Instance.actPlayerDamage += PlayerGetDamage;
    }

    // �ִ� 8�޺����� �����Ѵ�.
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
        // �̹� �������� ���� ���¶�� ���� ������ �ȴ�.
        if (playerCanDie == true)
        {
            StopAllCoroutines();
            GameManager.Instance.playerDie(this);
        }
        // �������� ǥ���ϴ� ui�� ǥ���Ѵ�.
        crushHeart.gameObject.SetActive(true);
        waitTimeText.gameObject.SetActive(true);
        // �޺��� ���δ�.
        nowCombo = nowCombo == 8 ? 4 : 0;
        comboText.text = nowCombo.ToString();
        // �ٽ� ȸ���Ҷ����� ����Ѵ�.
        StartCoroutine(wait15Second());
    }

    // 15�ʾȿ� �������� �ѹ� �� ������ �״´�.
    IEnumerator wait15Second()
    {
        playerCanDie = true; // �÷��̾�� ���� ���� �� �ִ�.
        // 15�ʸ� ����Ѵ�.
        for (int i = 15; i >= 0; i--)
        {
            yield return waitOneSecond;
            waitTimeText.text = i.ToString();
        }
        // 15�ʰ� �����ٸ� �ٽ� ȸ���Ѵ�.
        crushHeart.gameObject.SetActive(false);
        waitTimeText.gameObject.SetActive(false);
        playerCanDie = false;
        yield break;
    }
}
