using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private MovementProvider movementProvider;
    [SerializeField]
    private float speed = 3.0f; // 이동 속도
  
    
    [SerializeField]
    private GameObject Enemy1;
    [SerializeField]
    private GameObject Enemy2;
    [SerializeField]
    private GameObject Enemy3;
    void Start()
    {
        movementProvider.moveType = MovementProvider.MoveType.FreeMove;
    }
    
    // Start is called before the first frame update
    public void Update()
    {
        playerpos();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint")) 
        {
            movementProvider.moveType = MovementProvider.MoveType.GoFront;
            Enemy1.SetActive(true);
            Enemy2.SetActive(true);
            Enemy3.SetActive(true);
        }
    }
    public void playerpos()
    { 
        if (GameManager.Instance.PlayerPos.z == 133)
        {
            movementProvider.moveType = MovementProvider.MoveType.FreeMove;
            
        }
    }
  
}
