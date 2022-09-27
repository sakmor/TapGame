using UnityEngine;
using UnityEngine.UI;
using static GameDataSql;

public class RankNameLabel : MonoBehaviour
{
    public Text Name;
    public Text Score;

    public void SetData(TapGameData tapGameData)
    {
        Name.text = tapGameData.Name;
        Score.text = tapGameData.Score.ToString();
    }
}
