﻿using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players
        public Rigidbody m_Shell;                   // Prefab of the shell
		public Rigidbody m_ShellAlt;                // Prefab of the Alternate shell
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned
        public Slider m_AimSlider;                  // A child of the tank that displays the current launch force
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired
        public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held
        public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time
        public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force

        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press
		private bool m_AltFire;                     // Whether or not the alternate shell has been launched with this button press
		private InputActionReference _fireActionReference;

        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }


        private void Start ()
        {
            // The rate that the launch force charges up is the range of possible forces by the max charge time
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        }

		private bool FireButton (int mode) {

            var action = false;
            
            switch (mode)
            {
                case 0:
                    action = _fireActionReference.action.triggered;
                    break;
                case 1:
                    action = _fireActionReference.action.ReadValue<float>() > 0.5f;
                    break;
                case 2:
                    action = _fireActionReference.action.WasReleasedThisFrame();
                    break;
            }

			return action ;
		}


        private void Update()
        {
            // The slider should have a default value of the minimum launch force
            m_AimSlider.value = m_MinLaunchForce;

            // If the max force has been exceeded and the shell hasn't yet been launched...
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                // ... use the max force and launch the shell
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
            // Otherwise, if the fire button has just started being pressed...
			else if (FireButton (0))
            {
                // ... reset the fired flag and reset the launch force
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                // Change the clip to the charging clip and start it playing
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play ();
            }
            // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
			else if (FireButton (1) && !m_Fired)
            {
                // Increment the launch force and update the slider
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
			else if (FireButton (2) && !m_Fired)
            {
                // ... launch the shell
                Fire ();
            }
        }


        private void Fire()  {
            // Set the fired flag so only Fire is only called once
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody
			Rigidbody shellInstance;
			if (m_AltFire) shellInstance =
				Instantiate (m_ShellAlt, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
			else
			    shellInstance =
					Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction
            shellInstance.linearVelocity = m_CurrentLaunchForce * m_FireTransform.forward; 
			if (m_AltFire)
				shellInstance.linearVelocity *= 1.50f;

            // Change the clip to the firing clip and play it
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            // Reset the launch force.  This is a precaution in case of missing button events
            m_CurrentLaunchForce = m_MinLaunchForce;
        }

        public void SetInputsButtons(InputsButtons inputsButtons)
        {
	        _fireActionReference = inputsButtons.Fire;
        }
    }
}