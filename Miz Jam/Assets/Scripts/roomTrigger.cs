using UnityEngine;

public class roomTrigger : MonoBehaviour
{
    public roomManager room;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            foreach (var item in room.enemies)
            {
                item.BroadcastMessage("Activate");
            }
            foreach (var item in room.turrets)
            {
                item.BroadcastMessage("Activate");
            }
            if(room.doorsOpen && room.enemiesPresent)
            {
                foreach (var item in room.doors)
                {
                        item.SendMessage("Close");
                }
                room.doorsOpen = false;
                room.audioSource.PlayOneShot(room.closeSfx);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            foreach (var item in room.turrets)
            {
                item.BroadcastMessage("Deactivate");
            }
        }
    }
}