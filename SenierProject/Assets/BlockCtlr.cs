﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCtlr : MonoBehaviour {
    [SerializeField]
    private Material denyMaterial;
    private Material initalMeterial;
    private MeshRenderer mMeshRenderer;

    private void Awake()
    {
         mMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Use this for initialization
    void Start () {
        initalMeterial = mMeshRenderer.material;
	}

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        print("OnCollisionEnter");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            print("AAA");
            mMeshRenderer.material = denyMaterial;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            mMeshRenderer.material = initalMeterial;
        }
    }
}