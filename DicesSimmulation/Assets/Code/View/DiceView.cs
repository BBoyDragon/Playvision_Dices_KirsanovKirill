using System;
using System.Collections;
using System.Collections.Generic;
using Code.Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.View
{
    public class DiceView : MonoBehaviour
    {
        public float initialForceMin = 10f; // Минимальная сила начального импульса
        public float initialForceMax = 15f; // Максимальная сила начального импульса
        public float initialTorqueMin = 10f; // Минимальный начальный крутящий момент
        public float initialTorqueMax = 15f; // Максимальный начальный крутящий момент
        public float correctionTorque = 25f; // Момент для корректировки ориентации
        public float correctionSpeed = 2f;

        private Rigidbody rb;
        private bool _wasRolled = false;
        private Quaternion targetRotation;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Roll(DiceModel diceModel)
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            targetRotation = GetTargetRotation(diceModel.value);

            Vector3 randomForceDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
            Vector3 randomTorque =
                new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized *
                Random.Range(initialTorqueMin, initialTorqueMax);

            rb.AddForce(randomForceDirection * Random.Range(initialForceMin, initialForceMax), ForceMode.Impulse);
            rb.AddTorque(randomTorque, ForceMode.Impulse);
            _wasRolled = true;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<BoardView>(out BoardView boardView))
            {
                if (_wasRolled)
                {
                    _wasRolled = false;
                    StartCoroutine(Correcting());
                }
            }
        }

        private IEnumerator Correcting()
        {
            while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
            {
                Quaternion currentRotation = transform.rotation;
                Quaternion deltaRotation = targetRotation * Quaternion.Inverse(currentRotation);

                if (deltaRotation.w < 0)
                {
                    deltaRotation.x = -deltaRotation.x;
                    deltaRotation.y = -deltaRotation.y;
                    deltaRotation.z = -deltaRotation.z;
                    deltaRotation.w = -deltaRotation.w;
                }

                Vector3 torque = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z) * correctionTorque *
                                 Time.fixedDeltaTime;

                rb.AddTorque(torque, ForceMode.VelocityChange);
                transform.rotation =
                    Quaternion.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * correctionSpeed);
                yield return null;
            }

            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

        private Quaternion GetTargetRotation(int value)
        {
            switch (value)
            {
                case 1: return Quaternion.Euler(0, transform.rotation.y, 0);
                case 2: return Quaternion.Euler(0, transform.rotation.y, 90);
                case 3: return Quaternion.Euler(90, transform.rotation.y, 0);
                case 4: return Quaternion.Euler(270, transform.rotation.y, 0);
                case 5: return Quaternion.Euler(0, transform.rotation.y, 270);
                case 6: return Quaternion.Euler(180, transform.rotation.y, 0);
                default: return Quaternion.identity;
            }
        }
    }
}