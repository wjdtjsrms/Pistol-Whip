using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;  //���� �Ŵ����� �̱������� ����ϰڽ��ϴ�.

    //����� Ŭ���� �޾ƿ��� ģ�����Դϴ�. - (PlayerController.cs�� �������ֽ��ϴ�.)
    public void Player_Hit_Sound(AudioClip Break_Sound)// Player �������� �Ҹ�
    {
        GameObject _P = new GameObject();
        AudioSource Player_hit_Sound = _P.AddComponent<AudioSource>();
        Player_hit_Sound.clip = Break_Sound;

        Player_hit_Sound.Play();
    }

    public void Player_Hit_Sound2(AudioClip Breath_Sound)// Player ���Ҹ�
    {
        GameObject _B = new GameObject();
        AudioSource Player_hit_Sound2 = _B.AddComponent<AudioSource>();
        Player_hit_Sound2.clip = Breath_Sound;

        Player_hit_Sound2.Play();

        Destroy(_B, Breath_Sound.length);
    }
}