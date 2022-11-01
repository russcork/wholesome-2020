using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string InteractTooltip;
    public bool IsInteractable = true;

    public void InteractableEnter(PlayerController player)
    {
        BroadcastMessage("OnInteractableEnter", player, SendMessageOptions.DontRequireReceiver);
    }

    public void InteractableExit(PlayerController player)
    {
        BroadcastMessage("OnInteractableExit", player, SendMessageOptions.DontRequireReceiver);
    }
    
    public void DoInteract(PlayerController player)
    {
        BroadcastMessage("OnInteract", player);
    }
}
