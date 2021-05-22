using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Text scoreText;
}

public partial class UIManager : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.actPlayerDie += PlayerGameOver;
    }

    void Update()
    {
        hpText.text = "HP : " + GameManager.Instance.HP;
        scoreText.text = "Scroe : " + GameManager.Instance.Score;
    }

    private void PlayerGameOver()
    {
        gameOverText.SetActive(true);
    }
}
