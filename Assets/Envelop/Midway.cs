using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Midway : Venue
{
    private static float INCHES = 1.0f;
    private static float FEET = 12.0f * INCHES;
    private static float WIDTH = 20.0f * FEET + 10.25f * INCHES;
    private static float DEPTH = 41.0f * FEET + 6.0f * INCHES;

    private static float INNER_OFFSET_X = WIDTH / 2.0f - 1.0f * FEET - 8.75f * INCHES;
    private static float OUTER_OFFSET_X = WIDTH / 2.0f - 5.0f * FEET - 1.75f * INCHES;
    private static float INNER_OFFSET_Z = -DEPTH / 2.0f + 15.0f * FEET + 10.75f * INCHES;
    private static float OUTER_OFFSET_Z = -DEPTH / 2.0f + 7.0f * FEET + 8.0f * INCHES;

    private static float SUB_OFFSET_X = 36.0f * INCHES;
    private static float SUB_OFFSET_Z = 20.0f * INCHES;

    public ArrayList COLUMN_POSITIONS;
    public ArrayList SUB_POSITIONS;

    public static float xRange = 228.24f;
    public static float yRange = 141.73f;
    public static float zRange = 329.972f;
    public static float cx = 0f;
    public static float cy = 70.1f;
    public static float cz = 0f;

    // Use this for initialization  
    public void Create()
    {
        COLUMN_POSITIONS = new ArrayList();
        COLUMN_POSITIONS.Add(new Vector3(-OUTER_OFFSET_X, -OUTER_OFFSET_Z, 101));
        COLUMN_POSITIONS.Add(new Vector3(-INNER_OFFSET_X, -INNER_OFFSET_Z, 102));
        COLUMN_POSITIONS.Add(new Vector3(-INNER_OFFSET_X, INNER_OFFSET_Z, 103));
        COLUMN_POSITIONS.Add(new Vector3(-OUTER_OFFSET_X, OUTER_OFFSET_Z, 104));
        COLUMN_POSITIONS.Add(new Vector3(OUTER_OFFSET_X, OUTER_OFFSET_Z, 105));
        COLUMN_POSITIONS.Add(new Vector3(INNER_OFFSET_X, INNER_OFFSET_Z, 106));
        COLUMN_POSITIONS.Add(new Vector3(INNER_OFFSET_X, -INNER_OFFSET_Z, 107));
        COLUMN_POSITIONS.Add(new Vector3(OUTER_OFFSET_X, -OUTER_OFFSET_Z, 108));

        SUB_POSITIONS = new ArrayList();
        SUB_POSITIONS.Add((Vector3) COLUMN_POSITIONS[0] + new Vector3(-SUB_OFFSET_X, -SUB_OFFSET_Z, 0));
        SUB_POSITIONS.Add((Vector3) COLUMN_POSITIONS[3] + new Vector3(-SUB_OFFSET_X, SUB_OFFSET_Z, 0));
        SUB_POSITIONS.Add((Vector3) COLUMN_POSITIONS[4] + new Vector3(SUB_OFFSET_X, SUB_OFFSET_Z, 0));
        SUB_POSITIONS.Add((Vector3) COLUMN_POSITIONS[7] + new Vector3(SUB_OFFSET_X, -SUB_OFFSET_Z, 0));
    }
}
