using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPathGenerator : MonoBehaviour
{
    Vector3 a,b,c,d;

    Vector3 CurrentGameUp;

    List<Vector3> Positions, Directions, Tangents, Cross;

    int Segments = 10;


    void RasterizeBezier(){
        for(int i = 0; i <= Segments; i++){
            Positions.Add(GetValue(i/Segments));
            if(i != 0)
            {
                Directions.Add((Positions[i] - Positions[i-1]).normalized);
            }
            else
            {
                Tangents.Add(CurrentGameUp);
            }
        }
    }

    Vector3 GetValue(float t){
        Vector3 ab , bc , cd, abc, bcd;

        ab = GetT(a,b,t);
        bc = GetT(b,c,t);
        cd = GetT(c,d,t);
        abc = GetT(ab,bc,t);
        bcd = GetT(bc,cd,t);
        return GetT(abc,bcd,t);
    }

    Vector3 GetT(Vector3 a, Vector3 b, float t){
        return a + ((b - a)*t);
    }
}
