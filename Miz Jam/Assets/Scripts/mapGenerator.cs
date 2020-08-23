using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{
    public class RoomNode
    {
        public RoomNode parentNode;
        public List<RoomNode> childNodes = new List<RoomNode>();
        public int numChildren = 0;
        public Direction connections = 0;
        public Vector3 position;
        public int distanceFromStart = 0;
        public roomManager roomManager;
    }

    public List<RoomNode> roomTree = new List<RoomNode>();
    public List<roomManager> createdRooms = new List<roomManager>();
    public List<Vector3> roomPositions = new List<Vector3>();
    public int numRooms = 10;

    public roomManager basicRoom;
    public door door_N;
    public door door_W;
    public door door_E;
    public door door_S;

    public GameObject wall_NS;
    public GameObject wall_WE;

    public List<roomLayout> layouts;
    public roomLayout bossLayout;

    private Vector2 roomOffset = new Vector2(24, 18);

    // Start is called before the first frame update
    void Start()
    {
        Generate();
        //DebugGen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generate()
    {
        RoomNode startNode = new RoomNode();
        startNode.position = Vector3.zero;
        roomTree.Add(startNode);
        roomPositions.Add(Vector3.zero);
        for(int i = 0; i < numRooms-1; i++)
        {
            RoomNode currentNode = roomTree[UnityEngine.Random.Range(0, roomTree.Count)];
            if(currentNode.numChildren < 4)
            {
                RoomNode newNode = new RoomNode();
                int dir = UnityEngine.Random.Range(0,4);
                switch(dir)
                {
                    case 0:
                        newNode.connections = Direction.North;
                        newNode.position = currentNode.position + new Vector3(0, -roomOffset.y, 0);
                        if(!roomPositions.Contains(newNode.position))
                        {
                            currentNode.connections = currentNode.connections | Direction.South;
                        }
                        break;
                    case 1:
                        newNode.connections = Direction.East;
                        newNode.position = currentNode.position + new Vector3(-roomOffset.x, 0, 0);
                        if(!roomPositions.Contains(newNode.position))
                        {
                            currentNode.connections = currentNode.connections | Direction.West;
                        }
                        break;
                    case 2:
                        newNode.connections = Direction.South;
                        newNode.position = currentNode.position + new Vector3(0, roomOffset.y, 0);
                        if(!roomPositions.Contains(newNode.position))
                        {          
                            currentNode.connections = currentNode.connections | Direction.North;
                        }                        
                        break;
                    case 3:
                        newNode.connections = Direction.West;
                        newNode.position = currentNode.position + new Vector3(roomOffset.x, 0, 0);
                        if(!roomPositions.Contains(newNode.position))
                        {            
                            currentNode.connections = currentNode.connections | Direction.East;
                        }                        
                        break;
                    default:
                        break;
                }
                if(!roomPositions.Contains(newNode.position))
                {
                    roomPositions.Add(newNode.position);
                    currentNode.childNodes.Add(newNode);
                    roomTree.Add(newNode);
                    currentNode.numChildren++;
                    newNode.parentNode = currentNode;
                    newNode.distanceFromStart = currentNode.distanceFromStart + 1;
                }
                else
                {
                    i--;
                }
            }
            else
            {
                i--;
            }
        }
        foreach (var room in roomTree)
        {
            CreateRoom(room);
        }
        CloseRooms();
        PopulateRooms(startNode);
    }

    void CreateRoom(RoomNode node)
    {
        roomManager room = Instantiate(basicRoom, node.position, Quaternion.identity);
        room.transform.parent = transform;
        room.connections = node.connections;
        createdRooms.Add(room);
        node.roomManager = room;
    }

    void CloseRooms()
    {
        foreach (var room in createdRooms)
        {
            if((room.connections & Direction.North) != 0)
            {
                door door = Instantiate(door_N, room.door_N_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
            }
            else
            {
                GameObject wall = Instantiate(wall_NS, room.door_N_Spawn.transform);
                wall.transform.parent = room.transform;
            }
            if((room.connections & Direction.East) != 0)
            {
                door door = Instantiate(door_E, room.door_E_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
            }
            else
            {
                GameObject wall = Instantiate(wall_WE, room.door_E_Spawn.transform);
                wall.transform.parent = room.transform;
            }
            if((room.connections & Direction.South) != 0)
            {
                door door = Instantiate(door_S, room.door_S_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
            }
            else
            {
                GameObject wall = Instantiate(wall_NS, room.door_S_Spawn.transform);
                wall.transform.parent = room.transform;
            }
            if((room.connections & Direction.West) != 0)
            {
                door door = Instantiate(door_W, room.door_W_Spawn.transform);
                door.transform.parent = room.transform;
                room.doors.Add(door);
            }
            else
            {
                GameObject wall = Instantiate(wall_WE, room.door_W_Spawn.transform);
                wall.transform.parent = room.transform;
            }
        }
    }

    void PopulateRooms(RoomNode startNode)
    {
        RoomNode bossRoom = startNode;
        foreach (var room in roomTree)
        {
            if(room.numChildren == 0){
                if(room.distanceFromStart > bossRoom.distanceFromStart)
                {
                    bossRoom = room;
                }
            }   
        }
        roomLayout bossroomLayout = Instantiate(bossLayout, bossRoom.position, Quaternion.identity);
        bossRoom.roomManager.enemies = bossroomLayout.enemies;
        foreach (var room in roomTree)
        {
            if(room != startNode && room != bossRoom)
            {
                roomLayout layout = Instantiate(layouts[Random.Range(0,layouts.Count)], room.roomManager.transform);
                room.roomManager.enemies = layout.enemies;
                room.roomManager.turrets = layout.turrets;
            }
        }
    }

    void DebugGen()
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

