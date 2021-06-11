using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;  //사운드 매니저를 싱글톤으로 사용하겠습니다.

    //오디오 클립을 받아오는 친구들입니다. - (PlayerController.cs와 연동되있습니다.)
    public void Player_Hit_Sound(AudioClip Break_Sound)// Player 유리깨짐 소리
    {
        GameObject _P = new GameObject();
        AudioSource Player_hit_Sound = _P.AddComponent<AudioSource>();
        Player_hit_Sound.clip = Break_Sound;

        Player_hit_Sound.Play();
    }

    public void Player_Hit_Sound2(AudioClip Breath_Sound)// Player 숨소리
    {
        GameObject _B = new GameObject();
        AudioSource Player_hit_Sound2 = _B.AddComponent<AudioSource>();
        Player_hit_Sound2.clip = Breath_Sound;

        Player_hit_Sound2.Play();

        Destroy(_B, Breath_Sound.length);
    }
}