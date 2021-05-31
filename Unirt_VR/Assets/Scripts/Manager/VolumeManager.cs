using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource shootsource;

    public void SetMusicVolme(float volume)// �� ���� ����
    {
        musicsource.volume = volume;
    }
    public void SetEffectVolme(float volume)// �뷡 ���� ����
    {
        shootsource.volume = volume;
    }
    
}
