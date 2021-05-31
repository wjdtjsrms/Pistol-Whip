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

    // Update is called once per frame
    void Update()
    {
        
    }

    //void Example()
    //{

    //    RenderSettings.fogColor = Color.blue;
    //    RenderSettings.fog = true;

    //    RenderSettings.ambientLight = Color.red;

    //    RenderSettings.ambientEquatorColor = Color.yellow;
    //}

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

        Color fog5 = new Color(245/255f, 125/255f, 0f, 255/255f); //주황핑크
        Color amColor5 = new Color(0f, 188/255f, 47/255f);
        Color amEquatColor5 = new Color(164/255f, 0f, 161/255f);

        if (other.gameObject.tag == "map1")
        {
            RenderSettings.fogColor = fog2;
            RenderSettings.ambientLight = amColor2;
            RenderSettings.ambientEquatorColor = amEquatColor2;
        }
        else if (other.gameObject.tag == "map2")
        {
            RenderSettings.fogColor = fog3;
            RenderSettings.ambientLight = amColor3;
            RenderSettings.ambientEquatorColor = amEquatColor3;
        }
        else if (other.gameObject.tag == "map3")
        {
            RenderSettings.fogColor = fog4;
            RenderSettings.ambientLight = amColor4;
            RenderSettings.ambientEquatorColor = amEquatColor4;
        }
        else if (other.gameObject.tag == "map4")
        {
            RenderSettings.fogColor = fog5;
            RenderSettings.ambientLight = amColor5;
            RenderSettings.ambientEquatorColor = amEquatColor5;
        }

    }
}
