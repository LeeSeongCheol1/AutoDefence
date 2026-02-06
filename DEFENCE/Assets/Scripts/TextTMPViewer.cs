using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    [SerializeField]
    private TextMeshProUGUI textWave;
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;

    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private WaveSystem waveSystem;
    [SerializeField]
    private EnemySpawner enemySpawner;

    private float lastCurrentHP = -1;
    private float lastMaxHP = -1;
    private int lastGold = -1;
    private int lastWave = -1;
    private int lastEnemyCount = -1;

    private void Update()
    {
        UpdateHP();
        UpdateGold();
        UpdateWave();
        UpdateEnemyCount();
    }

    private void UpdateHP()
    {
        // 현재 값과 지난 프레임의 값이 다를 때만 UI 갱신 
        if (playerHP.CurrentHP != lastCurrentHP || playerHP.MaxHP != lastMaxHP)
        {
            textPlayerHP.SetText("{0}/{1}", playerHP.CurrentHP, playerHP.MaxHP);

            lastCurrentHP = playerHP.CurrentHP;
            lastMaxHP = playerHP.MaxHP;
        }
    }

    private void UpdateGold()
    {
        if (playerGold.CurrentGold != lastGold)
        {
            textPlayerGold.SetText("{0}", playerGold.CurrentGold);
            lastGold = playerGold.CurrentGold;
        }
    }

    private void UpdateWave()
    {
        if (waveSystem.CurrentWave != lastWave)
        {
            textWave.SetText("{0}/{1}", waveSystem.CurrentWave, waveSystem.MaxWave);
            lastWave = waveSystem.CurrentWave;
        }
    }

    private void UpdateEnemyCount()
    {
        if (enemySpawner.CurrentEnemyCount != lastEnemyCount)
        {
            textEnemyCount.SetText("{0}/{1}", enemySpawner.CurrentEnemyCount, 60);
            lastEnemyCount = enemySpawner.CurrentEnemyCount;
        }
    }
}