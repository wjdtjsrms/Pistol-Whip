using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public partial class GameManager : MonoBehaviour
{
    public event Action actPlayerDie; // �÷��̾� �����
    public event Action actPlayerDamage; // �÷��̾ ������ �Ծ�����
    public event Action actEnemyDie; // �ֳʹ� �����

    public event Action actGameStart; // ���� ó�� ����
    public event Action actGameEnd; // ���� ��
    public event Action actGamePause; // ���� �Ͻ� ����
    public event Action actGameRestart; // ���� �簳
}

public partial class GameManager : MonoBehaviour
{
    public void playerDie(UIManager obj)
    {
        if (obj is UIManager) // �� �̺�Ʈ�� UIManager������ ȣ�� �����ϴ�.
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

    public void EnemyDie(EnemyCtrl obj)
    {
        if (obj is EnemyCtrl) // �� �̺�Ʈ�� Enemy������ ȣ�� �����ϴ�.
        {
            actEnemyDie?.Invoke();
        }
    }
    public void GameStart(StartButton obj)
    {
        if (obj is StartButton) // �� �̺�Ʈ�� StartButton������ ȣ�� �����ϴ�.
        {
            actGameStart?.Invoke();
        }
    }

    public void GameEnd(FinishPoint obj)
    {
        if (obj is FinishPoint) // �� �̺�Ʈ�� StartButton������ ȣ�� �����ϴ�.
        {
            actGameEnd?.Invoke();
        }
    }

    public void GamePause(PopUpMenu obj)
    {
        if (obj is PopUpMenu) // �� �̺�Ʈ�� PopUpMenu������ ȣ�� �����ϴ�.
        {
            actGamePause?.Invoke();
        }
    }

    public void GameRestart(ContinueButton obj)
    {
        if (obj is ContinueButton) // �� �̺�Ʈ�� ContinueButton������ ȣ�� �����ϴ�.
        {
            actGameRestart?.Invoke();
        }
    }

}
