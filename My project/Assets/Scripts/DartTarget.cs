using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    Out = 0,
    Normal = 1,
    Double = 2,
    Triple = 3,
}

public class DartTarget : MonoBehaviour
{
    public float CenterInnerRadius = 0.05f;
    public float CenterOuterRadius = 0.1f;
    public float TripleInnerRadius = 0.3f;
    public float TripleOuterRadius = 0.4f;
    public float DoubleInnerRadius = 0.9f;
    public float DoubleOuterRadius = 1.0f;

    public int[] HitZoneScore = new int[20] { 13, 4, 18, 1, 20, 5, 12, 9, 14, 11, 8, 16, 7, 19, 3, 17, 2, 15, 10, 6 };

    public bool isDebug;
    public int debugScore;
    public Vector2Int debugID_Score;

    void Start()
    {

    }

    void Update()
    {

    }

    int CadranPosition(Vector3 FinalPosition)
    {
        Vector2 hitPos = Vector2.one;
        hitPos.x = Vector3.Dot(FinalPosition - this.transform.position, this.transform.right);
        hitPos.y = Vector3.Dot(FinalPosition - this.transform.position, this.transform.up);
        float Distance = hitPos.magnitude;

        if (Distance < CenterInnerRadius)
        {
            return 50;
        }
        if (Distance < CenterOuterRadius)
        {
            return 25;
        }

        HitType scoreMultiplier = HitType.Normal;
        if (DoubleInnerRadius < Distance && Distance < DoubleOuterRadius)
        {
            scoreMultiplier = HitType.Double;
        }
        if (TripleInnerRadius < Distance && Distance < CenterOuterRadius)
        {
            scoreMultiplier = HitType.Triple;
        }
        if (Distance > DoubleOuterRadius)
        {
            scoreMultiplier = HitType.Out;
        }

        //angle d'arrivé : angles de chaques case arronis inf
        float caseAngleValue = 360 / HitZoneScore.Length;
        float angle = Mathf.Atan2(hitPos.y, hitPos.x) * Mathf.Rad2Deg;
        angle += caseAngleValue / 2;
        int HitZone = Mathf.FloorToInt(angle / caseAngleValue);
        int score = HitZoneScore[HitZone];

        return score * (int)scoreMultiplier;
    }
}
