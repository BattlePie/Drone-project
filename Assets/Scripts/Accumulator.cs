using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Core;

public class Accumulator : MonoBehaviour
{
    [Header("Параметры из блок-схемы (ввод)")]
    public float capacity_Ah = 5f;           // C - ёмкость аккумулятора, А*ч
    public float voltage = 22.2f;            // U - напряжение системы, В
    public float electronics_current = 0.5f; // Iэл - ток потребления электроники, А
    [Range(0f, 100f)]
    public float start_percentage = 5f;      // с каким % заряда стартуем

    [Header("Состояние (вычисляется)")]
    public float charge_Ah;         // Q - текущий заряд, А*ч
    public float charge_percentage; // P - процент заряда, 0-100
    public float total_current;     // I - общий ток в данный момент, А
    public bool is_drone_disabled;  // отключен ли дрон из-за разряда

    [Header("UI (необязательно)")]
    [SerializeField] Text battery_text;

    private Drone drone;

    void Start()
    {
        drone = transform.parent.gameObject.GetComponent<Drone>();

        if (drone == null)
        {
            Debug.LogError(this + ": Accumulator не нашёл Drone на родительском объекте");
        }

        // стартуем с указанного процента заряда, а не с полного
        charge_Ah = capacity_Ah * (start_percentage / 100f);
    }

    void FixedUpdate()
    {
        // если дрон уже отключён - заряд больше не трогаем вообще
        if (is_drone_disabled) return;

        float dt = Time.fixedDeltaTime;

        total_current = CalculateTotalCurrent() + electronics_current;

        if (float.IsNaN(total_current) || float.IsInfinity(total_current))
        {
            Debug.LogError(this + ": total_current сломан (NaN/Infinity), игнорирую этот кадр");
            total_current = 0f;
        }

        charge_Ah -= total_current * dt / 3600f;

        if (float.IsNaN(charge_Ah)) charge_Ah = 0f;

        charge_Ah = Mathf.Clamp(charge_Ah, 0f, capacity_Ah);
        charge_percentage = (charge_Ah / capacity_Ah) * 100f;

        UpdateBatteryDisplay();

        if (charge_percentage <= 0f)
        {
            DisableDrone();
        }
        else if (charge_percentage <= 20f)
        {
            Debug.LogWarning("Низкий заряд аккумулятора: " + Mathf.RoundToInt(charge_percentage) + "%");
        }
    }

    float CalculateTotalCurrent()
    {
        float sum = 0f;
        if (drone == null) return 0f;

        Dictionary<string, Propeller> propellers = drone.GetPropellers();
        if (propellers == null) return 0f;

        foreach (Propeller propeller in propellers.Values)
        {
            sum += propeller.power_drain / voltage;
        }

        return sum;
    }

    void UpdateBatteryDisplay()
    {
        if (battery_text != null)
        {
            battery_text.text = Mathf.RoundToInt(charge_percentage) + "%";
        }
    }

    void DisableDrone()
    {
        is_drone_disabled = true;
        charge_Ah = 0f;
        charge_percentage = 0f;
        Debug.Log("Заряд закончился. Отключение дрона.");

        if (drone == null) return;

        Dictionary<string, Propeller> propellers = drone.GetPropellers();
        if (propellers == null) return;

        foreach (Propeller propeller in propellers.Values)
        {
            propeller.toggle_power(false);
        }
    }
}
