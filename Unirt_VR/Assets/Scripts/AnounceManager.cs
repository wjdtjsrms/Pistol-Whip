using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AnounceManager : MonoBehaviour
{
    // Start is called before the first frame update



    static public AnounceManager instance;
    // Start is called before the first frame update
    #region singletone
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion singletone

    private int j = 0;

    //Ÿ����Ȯ�� ����


    public AudioSource[] audioSourceAnounce;

    public string[] playAnounceName;//�������� �Ƴ�� ����

    public Sound[] AnounceSonuds;

    public void Update()
    {
        PlaySA();
    }

    public void PlaySA()
    {
        j = 0;
        j = TypeWriterEffect.instance.Diacnt;
        audioSourceAnounce[j+1].Play();
        if(j==5)
        {
            audioSourceAnounce[0].Stop();
            audioSourceAnounce[1].Stop();
            audioSourceAnounce[2].Stop();
            audioSourceAnounce[3].Stop();
            audioSourceAnounce[4].Stop();
            audioSourceAnounce[5].Stop();
        }
    }
    
}