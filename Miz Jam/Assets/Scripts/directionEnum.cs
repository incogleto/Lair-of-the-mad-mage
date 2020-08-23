using System;

[Flags]
public enum Direction
{
    None = 0b_0000_0000,
    North = 0b_0000_0001, //1
    East = 0b_0000_0010, //2
    South = 0b_0000_0100, //4
    West = 0b_0000_1000 //8
}