using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProtoDice : MonoBehaviour
{
    public int targetValue = 6; // Значение, которое должно выпасть
    public float initialForceMin = 5f; // Минимальная сила начального импульса
    public float initialForceMax = 10f; // Максимальная сила начального импульса
    public float initialTorqueMin = 10f; // Минимальный начальный крутящий момент
    public float initialTorqueMax = 20f; // Максимальный начальный крутящий момент
    public float correctionTorque = 10f; // Момент для корректировки ориентации
    public float stopThreshold = 0.1f; // Порог скорости для остановки
    public float correctionSpeed = 2f; // Скорость корректировки

    private Rigidbody rb;
    private bool correcting = false;
    private Quaternion targetRotation;
    private bool _startCorrecting=false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Устанавливаем случайное начальное вращение и импульс для кубика
        Vector3 randomForceDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(initialTorqueMin, initialTorqueMax);

        rb.AddForce(randomForceDirection * Random.Range(initialForceMin, initialForceMax), ForceMode.Impulse);
        rb.AddTorque(randomTorque, ForceMode.Impulse);

        // Рассчитываем целевую ориентацию для кубика
        targetRotation = GetTargetRotation(targetValue);
        StartCoroutine(WaitToCorrecting());
    }

    private IEnumerator WaitToCorrecting()
    {
        yield return new WaitForSeconds(1);
        _startCorrecting = true;
    }
    public void FixedUpdate()
    {
        if (_startCorrecting)
        {
            Correcting();
        }
    }

    void Correcting()
    {
        // Проверяем, если кубик практически перестал двигаться
        if (!correcting && (rb.velocity.magnitude < stopThreshold || rb.angularVelocity.magnitude < stopThreshold))
        {
            correcting = true;
        }

        // Корректируем вращение кубика, чтобы он выпал на нужное значение
        if (correcting)
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

            Vector3 torque = new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z) * correctionTorque * Time.fixedDeltaTime;

            rb.AddTorque(torque, ForceMode.VelocityChange);

            // Постепенное приближение к целевому положению
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * correctionSpeed);

            // Проверка на достижение целевого положения
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                correcting = false;
            }
        }
    }

    Quaternion GetTargetRotation(int value)
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
