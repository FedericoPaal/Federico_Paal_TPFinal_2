using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class RaycastLaser : MonoBehaviour
{
    private LineRenderer laserLine;

    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider) 
            {
                laserLine.SetPosition(0, new Vector3(0, 0, hit.distance));
            }
            else
            {
                laserLine.SetPosition(1, new Vector3(0, 0, 500));
            }
        }
    }



}
