﻿using UnityEngine;
using Photon.Pun;
using System.Collections;

/* THIS CODE IS JUST FOR PREVIEW AND TESTING */
// Feel free to use any code and picking on it, I cannot guaratnee it will fit into your project
public class ExplodingProjectile : MonoBehaviourPun
{
    public GameObject impactPrefab;
    public GameObject explosionPrefab;
    public float thrust;
    public LayerMask blockingLayer;
    public Rigidbody thisRigidbody;

    public int reflectCount = 2;

    public GameObject particleKillGroup;
    private Collider thisCollider;

    public bool LookRotation = true;
    public bool Missile = false;
    public Transform missileTarget;
    public float projectileSpeed;
    public float projectileSpeedMultiplier;

    public bool ignorePrevRotation = false;

    public bool explodeOnTimer = false;
    public float explosionTimer;
    float timer;

    private Vector3 previousPosition;
    Transform target;

    public float damage = 25;   // 공격력

    // Use this for initialization
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        if (Missile)
        {
            missileTarget = GameObject.FindWithTag("Target").transform;
        }
        thisCollider = GetComponent<Collider>();
        previousPosition = transform.position;

        thisRigidbody.AddForce(transform.forward * 2000);
    }

    // Update is called once per frame
    void Update()
    {
        /*     if(Input.GetButtonUp("Fire2"))
             {
                 Explode();
             }*/
        timer += Time.deltaTime;
        if (timer >= explosionTimer && explodeOnTimer == true)
        {
            Explode();
        }

    }

    void FixedUpdate()
    {
        if (Missile)
        {
            projectileSpeed += projectileSpeed * projectileSpeedMultiplier;
            //   transform.position = Vector3.MoveTowards(transform.position, missileTarget.transform.position, 0);

            transform.LookAt(missileTarget);

            thisRigidbody.AddForce(transform.forward * projectileSpeed);
        }

        if (LookRotation && timer >= 0.05f)
        {
            transform.rotation = Quaternion.LookRotation(thisRigidbody.velocity);
        }

        CheckCollision(previousPosition);

        previousPosition = transform.position;
    }
    Vector3 temp;
    void CheckCollision(Vector3 prevPos)
    {
        RaycastHit hit;
        Vector3 direction = transform.position - prevPos;
        Ray ray = new Ray(prevPos, direction);
        float dist = Vector3.Distance(transform.position, prevPos);
        
        if (Physics.Raycast(ray, out hit, dist, 1 << LayerMask.NameToLayer("Wall")))
        {
            if(reflectCount > 0)
            {

                temp = Vector3.Reflect(thisRigidbody.velocity, hit.normal);
                thisRigidbody.velocity = temp;

                Vector3 dir = hit.point - transform.position;
                Vector3 dirY = new Vector3(0, dir.y, 0);

                Quaternion targetRot = Quaternion.LookRotation(dirY);

                transform.rotation = targetRot;

                reflectCount--;
            }
            else
            {
                transform.position = hit.point;
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                Vector3 pos = hit.point;
                Instantiate(impactPrefab, pos, rot);
                if (!explodeOnTimer && Missile == false)
                {
                    Destroy(gameObject);
                }
                else if (Missile == true)
                {
                    thisCollider.enabled = false;
                    particleKillGroup.SetActive(false);
                    thisRigidbody.velocity = Vector3.zero;
                    Destroy(gameObject, 5);
                }
            }

            

            if (hit.transform.tag == "BreakeWall")
            {
                print(hit.transform.GetComponent<PlaceBlockCtrl>().hp);
                hit.transform.GetComponent<PlaceBlockCtrl>().Hit();

                transform.position = hit.point;
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                Vector3 pos = hit.point;
                Instantiate(impactPrefab, pos, rot);
                if (!explodeOnTimer && Missile == false)
                {
                    Destroy(gameObject);
                }
                else if (Missile == true)
                {
                    thisCollider.enabled = false;
                    particleKillGroup.SetActive(false);
                    thisRigidbody.velocity = Vector3.zero;
                    Destroy(gameObject, 5);
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            IDamageable target = other.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                print("ismine : " + photonView.IsMine);
                if (photonView.IsMine)
                {
                    Debug.Log("요긴되?");
                    target.OnDamage(damage);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {



        if (collision.gameObject.tag != "FX")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
            if (ignorePrevRotation)
            {
                rot = Quaternion.Euler(0, 0, 0);
            }
            Vector3 pos = contact.point;
            Instantiate(impactPrefab, pos, rot);
            if (!explodeOnTimer && Missile == false && photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }




}