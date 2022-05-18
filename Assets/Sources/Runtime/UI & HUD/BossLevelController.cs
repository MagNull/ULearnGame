using System;
using TMPro;
using UnityEngine;

public class BossLevelController : MonoBehaviour
{
    [SerializeField]
    private int _maxLevel;
    [SerializeField]
    private TextMeshProUGUI _bossLevelText;
    private int _bossLevel = 1;

    public void IncreaseLevel()
    {
        _bossLevel = Mathf.Clamp(_bossLevel + 1, 1, _maxLevel);
        _bossLevelText.text = _bossLevel.ToString();
    }

    public void DecreaseLevel()
    {
        _bossLevel = Mathf.Clamp(_bossLevel - 1, 1, _maxLevel);
        _bossLevelText.text = _bossLevel.ToString();
    }

    public void PassResult()
    {
        PlayerPrefs.SetInt("Boss Level", _bossLevel);
        _bossLevel = 1;
    }
}
