//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class HitControll : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject Lefthand;

//    public Transform m_tr;
//    public float distance = 10.0f;
//    public RaycastHit[] hits;
//    public LayerMask l_layerMask = -1;
//    // Start is called before the first frame update
//    void Start()
//    {
//        m_tr = GetComponent<Transform>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//    private void FixedUpdate()
//    {
//        Ray ray = new Ray();

//        ray.origin= m_tr position;
//    }
//    public void Myhand()
//    {
//        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward)),out hit, Mathf.Infinity, touchPlanet.ayerMask)
//    }
//}
