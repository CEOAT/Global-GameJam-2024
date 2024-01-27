using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Camera cam;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Transform objectHit = hit.transform;

        //    worldPosition = cam.ScreenToWorldPoint(screenPosition);
        //    print(worldPosition);
        //    // Do something with the object that was hit by the raycast.
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ant")
                {
                    
                }
            }
        }
        //screenPosition = Input.mousePosition;
        //worldPosition = cam.ScreenToWorldPoint(screenPosition);
        //print(worldPosition);
    }
}
