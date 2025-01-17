﻿using UnityEngine;
using UnityEngine.UI;

namespace Tanks
{
    public class TankShooting : MonoBehaviour
    {
        private const string FIRE_BUTTON = "Fire1";

        public Rigidbody shell;
        public Transform fireTransform;
        public Slider aimSlider;
        public AudioSource shootingAudio;
        public AudioClip chargingClip;
        public AudioClip fireClip;
        public float minLaunchForce = 15f;
        public float maxLaunchForce = 30f;
        public float maxChargeTime = 0.75f;

        private float currentLaunchForce;
        private float chargeSpeed;
        private bool fired;

        private void OnEnable()
        {
            currentLaunchForce = minLaunchForce;
            aimSlider.value = minLaunchForce;
        }

        private void Start()
        {
            chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        private void Update()
        {
            // TODO: Only allow owner of this tank to shoot

            aimSlider.value = minLaunchForce;

            if (currentLaunchForce >= maxLaunchForce && !fired)
            {
                currentLaunchForce = maxLaunchForce;
                Fire();
            }
            else if (Input.GetButtonDown(FIRE_BUTTON))
            {
                fired = false;
                currentLaunchForce = minLaunchForce;

                shootingAudio.clip = chargingClip;
                shootingAudio.Play();
            }
            else if (Input.GetButton(FIRE_BUTTON) && !fired)
            {
                currentLaunchForce += chargeSpeed * Time.deltaTime;

                aimSlider.value = currentLaunchForce;
            }
            else if (Input.GetButtonUp(FIRE_BUTTON) && !fired)
            {
                Fire();
            }
        }

        private void Fire()
        {
            fired = true;

            // TODO: Instantiate the projectile on all clients
            Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation);
            shellInstance.velocity = currentLaunchForce * fireTransform.forward;

            shootingAudio.clip = fireClip;
            shootingAudio.Play();

            currentLaunchForce = minLaunchForce;
        }
    }
}