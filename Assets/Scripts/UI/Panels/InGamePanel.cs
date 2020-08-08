using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : UIPanel
{
    private Transform statsContainer;
    private Text collectableCountText;

    private void Awake()
    {
        statsContainer = transform.Find("StatsContainer");
        collectableCountText = statsContainer.Find("Collectables").Find("CountText").GetComponent<Text>();
    }

    public override void UpdateCollectableText()
    {
        base.UpdateCollectableText();
        collectableCountText.text = LevelManager.Instance.Collectables.ToString();
    }
}
