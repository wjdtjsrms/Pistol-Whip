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
    private AudioClip player_hit_Clip; //�÷��̾� ��Ʈ ������ҽ�
    [SerializeField]
    private AudioClip player_hit_Clip2; // �÷��̾� ��Ʈ ������ҽ�2

    private void Awake()
    {

    }
}
