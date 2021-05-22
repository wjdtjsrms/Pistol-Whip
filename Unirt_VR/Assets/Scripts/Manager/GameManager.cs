using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public partial class GameManager : MonoBehaviour
{
    public GameObject playerGameObject;
    private PlayerController player;
    private static GameManager instance;

    public bool isGameOver
    {
        get;
        private set;
    }
    public int Score
    {
        get;
        private set;
    }
    public int HP
    {
        get;
        private set;
    }
}
