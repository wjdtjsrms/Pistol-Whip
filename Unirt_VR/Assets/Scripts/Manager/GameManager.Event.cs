using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public partial class GameManager : MonoBehaviour
{
    public event Action actPlayerDie;
    public event Action actPlayerDamage; // �÷��̾ �������� ������ �������� �Դ´�, �޺��� ������. ȭ���� ��� ��������, ����Ʈ�� �����.
    public event Action actEnemyDie;
}

public partial class GameManager : MonoBehaviour
{
    public void playerDie(PlayerController obj)
    {
        if(obj is PlayerController) // �� �̺�Ʈ�� PlayerController������ ȣ�� �����ϴ�.
        {
            actPlayerDie?.Invoke(); // null �ƴ϶�� ȣ��
        }  
    }

    public void playerDamage(PlayerController obj)
    {
        if (obj is PlayerController) // �� �̺�Ʈ�� PlayerController������ ȣ�� �����ϴ�.
        {
            actPlayerDamage?.Invoke();
        }
    }

    public void EnemyDie(MonsterCtrl obj)
    {
        if (obj is MonsterCtrl) // �� �̺�Ʈ�� Enemy������ ȣ�� �����ϴ�.
        {
            actEnemyDie?.Invoke();
        }
    }
}
