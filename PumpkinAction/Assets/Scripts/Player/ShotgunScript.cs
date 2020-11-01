﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : MonoBehaviour
{
    Team team;
    public PlayerStats stats;
    public playerInput input;
    public Camera playercam;

    public int damage = 5; //because health is an int
    public float singleshot_range = 6f;
    public float fullshot_range = 3f;
    public int pellets = 10;
    public float spreadAngle = 20f;
    public float hipfire_spreadAngle = 8f;
    public float singleshot_firerate = 0.5f;
    public float fullshot_firerate = 1f;

    public float ads_fov = 60f;
    private float start_fov;

    private float cooldown = 0f;

    public bool infinite_ammo = false;

    // Start is called before the first frame update
    void Start()
    {
        team = GetComponentInParent<TeamTag>().team;
        stats = GetComponentInParent<PlayerStats>();

        start_fov = playercam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.aimDownSights)
        {
            playercam.fieldOfView = ads_fov;
        }
        else
        {
            playercam.fieldOfView = start_fov;
        }

        cooldown -= Time.deltaTime;

        if (input.attack && cooldown <= 0f && stats.seedCount >= 1) //single pellet, perfectly accurate when ADS
        {
            cooldown = singleshot_firerate;
            stats.seedCount -= (infinite_ammo ? 0 : 1);

            float applied_spreadAngle = (input.aimDownSights ? 0f : hipfire_spreadAngle);
            Vector3 hit_direction = CalcHitDirection(applied_spreadAngle, fullshot_range);

            RaycastHit hit;

            if (Physics.Raycast(playercam.transform.position, hit_direction, out hit, singleshot_range))
            {
                Debug.Log("SingleShot hit");

                //TODO (what's here currently doesnt work apparently)
                //if(hit.collider.gameObject.GetComponentInParent<TeamTag>().team != team)
                //{
                //    hit.collider.gameObject.GetComponentInParent<Health>().ApplyDamage(5);
                //}

                Debug.DrawLine(playercam.transform.position, hit.point, Color.yellow, 1f);
            }
            else
            {
                Debug.DrawLine(playercam.transform.position, playercam.transform.position + hit_direction * singleshot_range, Color.yellow, 1f);
            }

        }

        if (input.attack2 && cooldown <= 0f && stats.seedCount >= pellets) //multiple pellets
        {
            cooldown = fullshot_firerate;
            stats.seedCount -= (infinite_ammo ? 0 : pellets);

            for (int i = 0; i < pellets; i++)
            {
                float applied_spreadAngle = (input.aimDownSights ? spreadAngle : spreadAngle + hipfire_spreadAngle);
                Vector3 hit_direction = CalcHitDirection(applied_spreadAngle, fullshot_range);

                RaycastHit hit;

                if (Physics.Raycast(playercam.transform.position, hit_direction, out hit, fullshot_range))
                {
                    Debug.Log("Fullshot Pellet hit");

                    //TODO (what's here currently doesnt work apparently)
                    //if (hit.collider.gameObject.GetComponentInParent<TeamTag>().team != team)
                    //{
                    //    hit.collider.gameObject.GetComponentInParent<Health>().ApplyDamage(5);
                    //}

                    Debug.DrawLine(playercam.transform.position, hit.point, Color.yellow, 1f);
                }
                else
                {
                    Debug.DrawLine(playercam.transform.position, playercam.transform.position + hit_direction * fullshot_range, Color.yellow, 1f);
                }


            }
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            infinite_ammo = !infinite_ammo;
        }
    }

    private Vector3 CalcHitDirection(float spread, float range)
    {
        float rng_pitch = Random.Range(0f, spread);
        float rng_roll = Random.Range(0f, 360f);
        float horizontal_offset = Mathf.Tan(Mathf.Deg2Rad * rng_pitch) * range; //opposite side of triangle with adjacent known (to adjust for range)

        var v3Offset = transform.up * horizontal_offset;
        v3Offset = Quaternion.AngleAxis(rng_roll, playercam.transform.forward) * v3Offset; //v3Offset gets rotated in a random direction around the camera forward axis
        Vector3 hit_direction = Vector3.Normalize(playercam.transform.forward * range + v3Offset); //normalized so that the debug line is accurate

        return hit_direction;
    }
}
