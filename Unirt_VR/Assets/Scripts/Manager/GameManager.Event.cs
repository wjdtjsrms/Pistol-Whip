using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public partial class GameManager : MonoBehaviour
{
    public event Action actPlayerDie;
    public event Action actPlayerDamage; // 플레이어가 데미지를 입으면 데미지를 입는다, 콤보가 깨진다. 화면이 잠시 빨개진다, 이펙트가 생긴다.
    public event Action actEnemyDie;
    public event Action actGameStart;
    public event Action actGameEnd;

}

public partial class GameManager : MonoBehaviour
{
    public void playerDie(UIManager obj)
    {
        if (obj is UIManager) // 이 이벤트는 UIManager에서만 호출 가능하다.
        {
            actPlayerDie?.Invoke(); // null 아니라면 호출
        }
    }

    public void playerDamage(PlayerController obj)
    {
        if (obj is PlayerController) // 이 이벤트는 PlayerController에서만 호출 가능하다.
        {
            actPlayerDamage?.Invoke();
        }
    }

    public void EnemyDie(EnemyCtrl obj)
    {
        if (obj is EnemyCtrl) // 이 이벤트는 Enemy에서만 호출 가능하다.
        {
            actEnemyDie?.Invoke();
        }
    }
    public void GameStart(StartButton obj)
    {
        if (obj is StartButton) // 이 이벤트는 StartButton에서만 호출 가능하다.
        {
            actGameStart?.Invoke();
        }
    }

    public void GameEnd(FinishPoint obj)
    {
        if (obj is FinishPoint) // 이 이벤트는 StartButton에서만 호출 가능하다.
        {
            actGameEnd?.Invoke();
        }
    }

}
