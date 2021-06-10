using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager  : MonoBehaviour
{
    [SerializeField]
    private static AudioManager instance;
    private static List<AudioSource> Player_Hit = new List<AudioSource>();

    [SerializeField]
    private AudioClip player_hit_Clip; //플레이어 히트 오디오소스
    [SerializeField]
    private AudioClip player_hit_Clip2; // 플레이어 히트 오디오소스2

    private void Awake()
    {

    }
}
