using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class NPCInteractionScript : MonoBehaviour
{
    private TextMeshProUGUI interactionTextComponent;
    private Animator anim;
    private EnemyBossController ebc;
    private PlayerController pc;

    private bool canPlayerInteract = false;
    private int currentDialogueIndex = 0;
    private bool isTalking = false;
    private string[] dialogues;

    [SerializeField]
    private GameObject dialoguePanel, interactionLabel;
    [SerializeField]
    private string[] dialoguesBeforeBossDefeat, dialoguesAfterBossDefeat;
    
    void Start()
    {
        currentDialogueIndex = 0;
        anim = GetComponentInParent<Animator>();
        dialogues = dialoguesBeforeBossDefeat;
        ebc = GameObject.Find("Enemy").GetComponent<EnemyBossController>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        if (interactionLabel != null) interactionLabel.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        ApplyAnimations();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            interactionLabel.SetActive(true);
            canPlayerInteract = true;
            if (ebc.isDead && pc.hasClaimedShards) {
                dialogues = dialoguesAfterBossDefeat;
            } else {
                dialogues = dialoguesBeforeBossDefeat;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            canPlayerInteract = false;
            dialoguePanel.SetActive(false);
            interactionLabel.SetActive(false);
            isTalking = false;
            currentDialogueIndex = 0;
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPlayerInteract) {
            Interact();
        }
    }

    private void Interact()
    {
        isTalking = true;
        if (interactionLabel.activeSelf) interactionLabel.SetActive(false);
        dialoguePanel.SetActive(true);
        interactionTextComponent = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
        if (currentDialogueIndex < dialogues.Length) {
            interactionTextComponent.text = dialogues[currentDialogueIndex++];
        } else {
            isTalking = false;
            dialoguePanel.SetActive(false);
            interactionLabel.SetActive(true);
            currentDialogueIndex = 0;
        }
    }

    private void ApplyAnimations()
    {
        anim.SetBool("isTalking", isTalking);
    }
}
