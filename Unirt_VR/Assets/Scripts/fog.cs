using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Example();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Example()
    {

        RenderSettings.fogColor = Color.blue;
        RenderSettings.fog = true;

        RenderSettings.ambientLight = Color.red;

        RenderSettings.ambientEquatorColor = Color.yellow;
    }
}
