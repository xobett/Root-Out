using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            var playerCharacterCtrlr = player.GetComponent<CharacterController>();
            var lastCheckpointSaved = player.GetComponent<CheckpointUpdater>().lastCheckpoint;

            playerCharacterCtrlr.enabled = false;
            player.transform.position = lastCheckpointSaved;
            playerCharacterCtrlr.enabled = true;
        }
    }
}
