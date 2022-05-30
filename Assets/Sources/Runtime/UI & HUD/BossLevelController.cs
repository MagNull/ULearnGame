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
        get => _bossLevel;
        set
        {
            _bossLevel = Mathf.Clamp(value, 1, _maxLevel);;
            _bossLevelText.text = _bossLevel.ToString();
        }
    }

    public void IncreaseLevel() => BossLevel++;

    public void DecreaseLevel() => BossLevel--;

    public void PassResult() => PlayerPrefs.SetInt(PlayerPrefsConstants.BOSS_LEVEL, _bossLevel);

    private void OnEnable() => BossLevel = PlayerPrefs.GetInt(PlayerPrefsConstants.BOSS_LEVEL);
}
