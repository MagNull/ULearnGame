using System;
using System.Collections.Generic;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using UnityEngine;
using Zenject;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField]
    private HealthSlot _healthBegin;
    [SerializeField]
    private HealthSlot _healthEnd;
    [SerializeField]
    private HealthSlot _healthPartPrefab;

    private readonly Stack<HealthSlot> _healthCells = new();

    [Inject]
    private void Init(IHealth health)
    {
        FillHealthBar(health.Value);
        health.HealthChanged += OnHealthValueChanged;
    }

    private void FillHealthBar(int healthValue)
    {
        AddHealthSlot(_healthBegin);
        for (var i = 0; i < healthValue - 2; i++)
        {
            AddHealthSlot(_healthPartPrefab);
        }

        AddHealthSlot(_healthEnd);
    }

    private void OnHealthValueChanged(int healthValue)
    {
        if (healthValue < 0)
            throw new Exception("Negative health value");

        if (_healthCells.Count < healthValue)
        {
            AddAdditionalCells(healthValue);
        }
        else
        {
            while (_healthCells.Count > healthValue)
            {
                var slot = _healthCells.Pop();
                slot.Deactivate();
            }
        }
    }

    private void AddAdditionalCells(int healthValue)
    {
        Destroy(_healthCells.Pop().gameObject); // Небезопасное место TODO: Исправить
        
        while (_healthCells.Count < healthValue - 1) 
            AddHealthSlot(_healthPartPrefab);
        
        AddHealthSlot(_healthEnd);
    }

    private void AddHealthSlot(HealthSlot slot)
    {
        var slotView = Instantiate(slot, transform);
        slotView.Activate();
        _healthCells.Push(slotView);
    }
}