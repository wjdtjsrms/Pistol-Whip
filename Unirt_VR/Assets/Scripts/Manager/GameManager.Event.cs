using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public partial class GameManager : MonoBehaviour
{
    public event Action actPlayerDie;
    public event Action actPlayerDamage; // 플레이어가 데미지를 입으면 데미지를 입는다, 콤보가 깨진다. 화면이 잠시 빨개진다, 이펙트가 생긴다.
    public event Action actEnemyDie;
}

public partial class GameManager : MonoBehaviour
{
    public void playerDie(PlayerController obj)
    {
        if(obj is PlayerController) // 이 이벤트는 PlayerController에서만 호출 가능하다.
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

    public void EnemyDie(MonsterCtrl obj)
    {
        if (obj is MonsterCtrl) // 이 이벤트는 Enemy에서만 호출 가능하다.
        {
            actEnemyDie?.Invoke();
        }
    }
}
