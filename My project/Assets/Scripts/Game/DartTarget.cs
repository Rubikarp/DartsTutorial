using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum HitType
{
    Out = 0,
    Normal = 1,
    Double = 2,
    Triple = 3,
}

public class DartTarget : MonoBehaviour
{
    [Range(0.0f, 0.5f)] public float CenterInnerRadius = 0.05f;
    [Range(0.2f, 1.5f)] public float CenterOuterRadius = 0.1f;
    [Range(1.0f, 2.0f)] public float TripleInnerRadius = 0.3f;
    [Range(1.2f, 3.5f)] public float TripleOuterRadius = 0.4f;
    [Range(2.0f, 4.5f)] public float DoubleInnerRadius = 0.9f;
    [Range(2.2f, 5.0f)] public float DoubleOuterRadius = 1.0f;

    public int[] HitZoneScore = new int[20] { 13, 4, 18, 1, 20, 5, 12, 9, 14, 11, 8, 16, 7, 19, 3, 17, 2, 15, 10, 6 };

    public bool isDebug;
    public int debugScore;
    public Vector2Int debugID_Score;

    public UnityEvent<string> OnScore;
    [SerializeField] Vector2 hitPos;

    void Start()
    {

    }

    public void Hit(Vector3 pos)
    {
        int score = CadranPosition(pos);
        OnScore?.Invoke("tu as marqué : " + score);
    }

    private int CadranPosition(Vector3 FinalPosition)
    {
        var toCenter = FinalPosition - this.transform.position;
        hitPos = Vector2.one;
        hitPos.x = Vector3.Dot(this.transform.right, toCenter);
        hitPos.y = Vector3.Dot(this.transform.forward, toCenter);

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
        angle = (angle + 360) % 360;
        int HitZone = Mathf.FloorToInt(angle / caseAngleValue);
        int score = HitZoneScore[HitZone];

        return score * (int)scoreMultiplier;
    }

    private void OnValidate()
    {
        CenterInnerRadius = Mathf.Max(CenterInnerRadius, 0.0f);
        CenterOuterRadius = Mathf.Max(CenterOuterRadius, CenterInnerRadius + 0.05f);
        TripleInnerRadius = Mathf.Max(TripleInnerRadius, CenterOuterRadius + 0.05f);
        TripleOuterRadius = Mathf.Max(TripleOuterRadius, TripleInnerRadius + 0.05f);
        DoubleInnerRadius = Mathf.Max(DoubleInnerRadius, TripleOuterRadius + 0.05f);
        DoubleOuterRadius = Mathf.Max(DoubleOuterRadius, DoubleInnerRadius + 0.05f);
    }
    private void OnDrawGizmos()
    {
        var col = Handles.color;

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, CenterInnerRadius);
        Handles.DrawWireDisc(transform.position, transform.up, CenterOuterRadius);
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, TripleInnerRadius);
        Handles.DrawWireDisc(transform.position, transform.up, TripleOuterRadius);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, DoubleInnerRadius);
        Handles.DrawWireDisc(transform.position, transform.up, DoubleOuterRadius);

        Handles.color = col;

        Debug.DrawLine(transform.position, transform.position + (Vector3)hitPos, Color.yellow);
    }
}
