using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Example();
    }


    void OnTriggerEnter(Collider other)
    {
        RenderSettings.fog = true;

        Color fog2 = new Color(255 / 255f, 148 / 255f, 93 / 255f, 255 / 255f); //핑크보라색
        Color amColor2 = new Color(166/ 255f, 0f, 139 / 255f);
        Color amEquatColor2 = new Color(57/255f, 0f, 202/255f);

        Color fog3 = new Color(117 / 255f, 255 / 255f, 76 / 255f, 255 / 255f); //연두색
        Color amColor3 = new Color(133/255f, 78/255f, 0f);
        Color amEquatColor3 = new Color(0f, 63/255f, 164/255f);

        Color fog4 = new Color(108/255f, 55/255f, 248/255f, 255/255f); //파랑
        Color amColor4 = new Color(217/255f, 206/255f, 0f);
        Color amEquatColor4 = new Color(0f, 115/255f, 144/255f);

        Color fog5 = new Color(92/255f, 236 / 255f, 187/255f, 255 / 255f); //민트
        Color amColor5 = new Color(171/255f, 131 / 255f, 96 / 255f);
        Color amEquatColor5 = new Color(95 / 255f, 157/255f, 243 / 255f);

        Color fog6 = new Color(245 / 255f, 125 / 255f, 0f, 255 / 255f); //주황핑크
        Color amColor6 = new Color(0f, 188 / 255f, 47 / 255f);
        Color amEquatColor6 = new Color(164 / 255f, 0f, 161 / 255f);

        // now -> fog2
        // fog2 -> fog3
        // fog3 -> fog4
        // fog4 -> fog5
        // fog5 -> fog6


        if (other.gameObject.tag == "map1")
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColor(RenderSettings.fogColor, fog2, RenderSettings.ambientLight, amColor2, RenderSettings.ambientEquatorColor, amEquatColor2));
        }
        else if (other.gameObject.tag == "map2")
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColor(fog2, fog3, amColor2, amColor3, amEquatColor2, amEquatColor3));
        }
        else if (other.gameObject.tag == "map3")
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColor(fog3, fog4, amColor3, amColor4, amEquatColor3, amEquatColor4));
        }
        else if (other.gameObject.tag == "map4")
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColor(fog4, fog5, amColor4, amColor5, amEquatColor4, amEquatColor5));
        }
        else if (other.gameObject.tag == "map5")
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColor(fog5, fog6, amColor5, amColor6, amEquatColor5, amEquatColor6));
        }

    }

    IEnumerator ChangeColor(Color fogColor1, Color fogColor2, Color ambientLight1, Color ambientLight2, Color ambientEquatorColor1, Color ambientEquatorColor2)
    {
        float percent = 0;
        float speed = 1.0f;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            Color fogColor = Color.Lerp(fogColor1, fogColor2, percent);
            RenderSettings.fogColor = fogColor;

            Color ambientLight = Color.Lerp(ambientLight1, ambientLight2, percent);
            RenderSettings.ambientLight = ambientLight;

            Color ambientEquatorColor = Color.Lerp(ambientEquatorColor1, ambientEquatorColor2, percent);
            RenderSettings.ambientEquatorColor = ambientEquatorColor;
            yield return null;
        }
        yield break;
    }
}
