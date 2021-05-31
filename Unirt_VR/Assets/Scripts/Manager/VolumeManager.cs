using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource shootsource;

    public void SetMusicVolme(float volume)// ¼¦ º¼·ý Á¶Àý
    {
        musicsource.volume = volume;
    }
    public void SetEffectVolme(float volume)// ³ë·¡ º¼·ý Á¶Àý
    {
        shootsource.volume = volume;
    }
    
}
