using System;
using Sources.Runtime;
using TMPro;
using UnityEngine;

public class BossLevelController : MonoBehaviour
{
    [SerializeField]
    private int _maxLevel;
    [SerializeField]
    private TextMeshProUGUI _bossLevelText;
    [SerializeField]
    private int _bossLevel;

    private int BossLevel
    {
        set
        {
            _bossLevel = value;
            _bossLevelText.text = _bossLevel.ToString();
        }
    }

    public void IncreaseLevel()
    {
        BossLevel = Mathf.Clamp(_bossLevel + 1, 1, _maxLevel);
    }

    public void DecreaseLevel()
    {
        BossLevel = Mathf.Clamp(_bossLevel - 1, 1, _maxLevel); ;
    }

    public void PassResult()
    {
        PlayerPrefs.SetInt(PlayerPrefsConstants.BOSS_LEVEL, _bossLevel);
    }

    private void OnEnable()
    {
        BossLevel = PlayerPrefs.GetInt(PlayerPrefsConstants.BOSS_LEVEL);
    }
}
