using System;
using System.Collections.Generic;
using Sources.Runtime.Player_Components;
using UnityEngine;

public class CellHealthView : MonoBehaviour
{
    [SerializeField]
    private HealthSlot _healthBegin;
    [SerializeField]
    private HealthSlot _healthEnd;
    [SerializeField]
    private HealthSlot _healthPartPrefab;

    private readonly Stack<HealthSlot> _activeSlots = new();
    private readonly Stack<HealthSlot> _inactiveSlots = new();

    public void Init(int healthValue, Player player)
    {
        FillHealthBar(healthValue);
        player.Damaged += OnDamaged;
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

    private void OnDamaged(int health)
    {
        if(health < 0)
            throw new Exception("Negative health value");
        
        while (_activeSlots.Count > health)
        {
            var slot = _activeSlots.Pop();
            slot.Deactivate();
            _inactiveSlots.Push(slot);
        }
    }

    private void AddHealthSlot(HealthSlot slot)
    {
        var slotView = Instantiate(slot, transform);
        slotView.Activate();
        _activeSlots.Push(slotView);
    }
}