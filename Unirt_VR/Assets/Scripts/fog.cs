using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fog : MonoBehaviour
{
    private struct ColorSetting
    {
        public Color fog;
        public Color amColor;
        public Color amEquatColor;
    }

    private ColorSetting[] colorSettings = new ColorSetting[6];

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.fog = true;

        colorSettings[0].fog = RenderSettings.fogColor;
        colorSettings[0].amColor = RenderSettings.ambientLight;
        colorSettings[0].amEquatColor = RenderSettings.ambientEquatorColor;

        colorSettings[1].fog = new Color(255 / 255f, 148 / 255f, 93 / 255f, 255 / 255f); //핑크보라색
        colorSettings[1].amColor = new Color(166 / 255f, 0f, 139 / 255f);
        colorSettings[1].amEquatColor = new Color(57 / 255f, 0f, 202 / 255f);

        colorSettings[2].fog = new Color(117 / 255f, 255 / 255f, 76 / 255f, 255 / 255f); //연두색
        colorSettings[2].amColor = new Color(133 / 255f, 78 / 255f, 0f);
        colorSettings[2].amEquatColor = new Color(0f, 63 / 255f, 164 / 255f);

        colorSettings[3].fog = new Color(108 / 255f, 55 / 255f, 248 / 255f, 255 / 255f); //파랑
        colorSettings[3].amColor = new Color(217 / 255f, 206 / 255f, 0f);
        colorSettings[3].amEquatColor = new Color(0f, 115 / 255f, 144 / 255f);

        colorSettings[4].fog = new Color(92 / 255f, 236 / 255f, 187 / 255f, 255 / 255f); //민트
        colorSettings[4].amColor = new Color(171 / 255f, 131 / 255f, 96 / 255f);
        colorSettings[4].amEquatColor = new Color(95 / 255f, 157 / 255f, 243 / 255f);

        colorSettings[5].fog = new Color(245 / 255f, 125 / 255f, 0f, 255 / 255f); //주황핑크
        colorSettings[5].amColor = new Color(0f, 188 / 255f, 47 / 255f);
        colorSettings[5].amEquatColor = new Color(164 / 255f, 0f, 161 / 255f);

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Map")
        {
            StopAllCoroutines();
            StartCoroutine(ChangeColor(colorSettings[index++], colorSettings[index]));
        }
    }

    IEnumerator ChangeColor(ColorSetting colorSetting1, ColorSetting colorSetting2)
    {
        float percent = 0;
        float speed = 1.0f;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            Color fogColor = Color.Lerp(colorSetting1.fog, colorSetting2.fog, percent);
            RenderSettings.fogColor = fogColor;

            Color ambientLight = Color.Lerp(colorSetting1.amColor, colorSetting2.amColor, percent);
            RenderSettings.ambientLight = ambientLight;

            Color ambientEquatorColor = Color.Lerp(colorSetting1.amEquatColor, colorSetting2.amEquatColor, percent);
            RenderSettings.ambientEquatorColor = ambientEquatorColor;
            yield return null;
        }
        yield break;
    }
}
