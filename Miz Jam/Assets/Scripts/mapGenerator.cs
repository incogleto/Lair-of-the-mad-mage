using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{
    public roomManager basicRoom;
    public door door_N;
    public door door_W;
    public door door_E;
    public door door_S;

    public GameObject wall_NS;
    public GameObject wall_WE;

    public List<roomLayout> layouts;

    private Vector2 roomOffset = new Vector2(24, 18);

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generate()
    {
        for(int x = 0; x < 3; x++)
        {
            for(int y = 0; y < 3; y++)
            {
                roomManager room = Instantiate(basicRoom, new Vector3(roomOffset.x * x, roomOffset.y * y), Quaternion.identity);
                room.transform.parent = transform;
                door door = Instantiate(door_N, room.door_N_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
                door = Instantiate(door_W, room.door_W_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
                door = Instantiate(door_S, room.door_S_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
                door = Instantiate(door_E, room.door_E_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
                roomLayout layout = Instantiate(layouts[Random.Range(0,layouts.Count)], room.transform);
                room.enemies = layout.enemies;
            }
        }
    }
}
