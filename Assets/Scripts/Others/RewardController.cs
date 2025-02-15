using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    public enum RewardType {
        SHARDS,
        REWARD,
    }
    
    private const string DEFAULT_REWARD_TEXT = "Congratulations! Press E to continue!!";

    [SerializeField]
    private GameObject rewardImageGameObject, rewardTextGameObject;

    private GameObject rewardCanvas;
    private EnemyBossController enemyBoss;
    private PlayerController player;
    private TextMeshProUGUI rewardText;
    private Image rewardImageComponent;

    private bool isRewardClaimed = false;
    public bool isPopUpActive = false;
    private RewardType rewardType;

    // Start is called before the first frame update
    void Start()
    {
        rewardCanvas = transform.GetChild(0).gameObject;
        rewardCanvas.SetActive(false);
        enemyBoss = GameObject.Find("Enemy").GetComponent<EnemyBossController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rewardText = rewardTextGameObject.GetComponent<TextMeshProUGUI>();
        rewardImageComponent = rewardImageGameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        claimReward();
        removePopup();
    }

    private void removePopup()
    {
        if (isPopUpActive && isRewardClaimed)
        {
            isPopUpActive = false;
            isRewardClaimed = false;
            // Destroy(gameObject);
            rewardCanvas.SetActive(false);
            Time.timeScale = 1f;
            if (rewardType == RewardType.REWARD) {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void claimReward()
    {
        if (isPopUpActive && Input.GetKeyDown(KeyCode.E)) {
            isRewardClaimed = true;
            if (rewardType == RewardType.SHARDS) {
                player.hasClaimedShards = true;
            } else {
                player.hasClaimedReward = true;
            }
        }
    }

    public void createRewardPopup(string rewardTextString = DEFAULT_REWARD_TEXT, Sprite rewardImageSprite = null, RewardType rewardType = RewardType.SHARDS)
    {
        if (enemyBoss.isDead && !isRewardClaimed && !isPopUpActive) {
            Time.timeScale = 0f;
            rewardText.text = rewardTextString;
            rewardImageComponent.sprite = rewardImageSprite;
            rewardCanvas.SetActive(true);
            isPopUpActive = true;
            this.rewardType = rewardType;
        }
    }
}
