using System.Collections.Generic;
using UnityEngine;

public class BombGen : MonoBehaviour
{
    public GameObject Foams;
    public GameObject Empty;
    public GameObject Barriers;

    public int size;
    public float offset = 0.22f;

    public void BuildCasings()
    {
        float half = size / 2.0f;
        int sum = 0;
        for (int i = 0; i < size; i++) sum += i + 1;
        GameObject bomb = Instantiate(Empty);
        bomb.name = size + " Row Casing (" + ((sum * 2) - 1) + " Modules)";
        Casing casing = bomb.GetComponent<Casing>();
        Transform visual_transform = casing.Visual;
        KMBombFace front_face = casing.Front.GetComponent<KMBombFace>();
        KMBombFace back_face = casing.Back.GetComponent<KMBombFace>();

        front_face.Anchors = new List<Transform>();
        front_face.Backings = new List<KMModuleBacking>();
        front_face.GetComponent<KMSelectable>().ChildRowLength = size;
        back_face.Anchors = new List<Transform>();
        back_face.Backings = new List<KMModuleBacking>();
        back_face.GetComponent<KMSelectable>().ChildRowLength = size;

        casing.Distance_Collider.size = new Vector3(size * 0.23f, 0.20f, size * 0.23f);
        casing.Selectable_Area.size = new Vector3(size * 0.24f, size * 0.24f, 0.22f);

        casing.Selectable_Area.transform.Translate(0, -0.25f, 0);
        
        casing.Highlight.localScale = new Vector3(1, 1, 1);
        casing.Highlight.Rotate(90, 0, 0);
        casing.Highlight.Translate(new Vector3(0, 0, -0.02f));

        float barrier_width = 0.025f;
        float widget_offset = 0.22f;
        float widget_constant_offset = barrier_width + 0.00275f;

        for (int w = 0; w < size; w++)
        {
            Transform Bface = new GameObject().GetComponent<Transform>();
            Bface.Translate(new Vector3(offset * (w - half + 0.5f), 0.0f, 0.0f));
            Bface.Rotate(-90, 0, 0);
            Bface.SetParent(casing.W_Bottom);
            Bface.localScale = new Vector3(0.12f, 0.03f, 0.17f);
            Bface.name = "Bottom Face";
            bomb.GetComponent<KMBomb>().WidgetAreas.Add(Bface.gameObject);
            
            Transform Lface = new GameObject().GetComponent<Transform>();
            Lface.Translate(new Vector3(w * offset * 0.5f, 0.0f, offset * (w - half + 0.5f)));
            Lface.Rotate(-90, 90, 0);
            Lface.SetParent(casing.W_Left);
            Lface.localScale = new Vector3(0.12f, 0.03f, 0.17f);
            Lface.name = "Left Face";
            bomb.GetComponent<KMBomb>().WidgetAreas.Add(Lface.gameObject);

            Transform Rface = new GameObject().GetComponent<Transform>();
            Rface.Translate(new Vector3(-w * offset * 0.5f, 0.0f, offset * (w - half + 0.5f)));
            Rface.Rotate(-90, -90, 0);
            Rface.SetParent(casing.W_Right);
            Rface.localScale = new Vector3(0.12f, 0.03f, 0.17f);
            Rface.name = "Right Face";
            bomb.GetComponent<KMBomb>().WidgetAreas.Add(Rface.gameObject);
        }

        casing.W_Bottom.Translate(new Vector3(0, 0, size * -widget_offset / 2 - widget_constant_offset), Space.World);
        casing.W_Left.Translate(new Vector3(size * -widget_offset / 2 - widget_constant_offset, 0, 0), Space.World);
        casing.W_Right.Translate(new Vector3(size * widget_offset / 2 + widget_constant_offset, 0, 0), Space.World);

        for (int i = 0; i <= size; i++)
        {
            for (int j = 0; (j <= i + 1) && !i.Equals(size); j++)
            {
                var pos = (-offset / 2) * (i + 1);
                Transform Bar1 = (Instantiate(Barriers) as GameObject).GetComponent<Transform>();
                Bar1.SetParent(visual_transform);
                Bar1.localScale = new Vector3(barrier_width, 0.21f, 0.22f);
                Bar1.Translate(new Vector3(pos + (offset * j), -0, 0 + ((offset / 2) * (size - 1)) - (offset * i)));
            }

            Transform Bar2 = (Instantiate(Barriers) as GameObject).GetComponent<Transform>();
            Bar2.SetParent(visual_transform);
            if (i == 0) Bar2.localScale = new Vector3((size) * 0.22f, 0.22f, barrier_width);
            else Bar2.localScale = new Vector3((size - i + 1) * 0.22f, 0.22f, barrier_width);
            Bar2.Translate(new Vector3(0, 0, offset * (i - half)));
        }

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < x + 1; y++)
            {
                GameObject front_foams = Instantiate(Foams);
                Transform f = front_foams.GetComponent<Transform>();
                f.SetParent(casing.FoamFront);
                f.Translate(new Vector3(offset * (x - half) - (offset * y * 0.5f), offset * (y - half), -0.01f));
                f.Translate(new Vector3(-0.687f, 0.359f, 0.0f));
                f.name = "Bomb_Foam_" + x + "_" + y + "_F";
                Transform f_anchor = new GameObject().GetComponent<Transform>();
                f_anchor.position = f.position;
                f_anchor.parent = f;
                f_anchor.Translate(0, 0.08f, 0);
                f_anchor.Rotate(new Vector3(0, 0, 0));
                f_anchor.name = "Anchor";
                front_face.Anchors.Add(f_anchor);
                front_face.Backings.Add(front_foams.GetComponent<KMModuleBacking>());
                GameObject back_foams = Instantiate(Foams);
                Transform b = back_foams.GetComponent<Transform>();
                b.SetParent(casing.FoamBack);
                b.Translate(new Vector3(offset * (x - half) - (offset * y * 0.5f), offset * (y - half), 0.01f));
                b.Translate(new Vector3(-0.687f, 0.359f, 0.0f));
                b.Rotate(new Vector3(0, 180, 0));
                b.name = "Bomb_Foam_" + x + "_" + y + "_B";
                Transform b_anchor = new GameObject().GetComponent<Transform>();
                b_anchor.position = b.position;
                b_anchor.parent = b;
                b_anchor.Translate(0, -0.08f, 0);
                b_anchor.Rotate(new Vector3(0, 0, 180));
                b_anchor.name = "Anchor";
                back_face.Anchors.Add(b_anchor);
                back_face.Backings.Add(back_foams.GetComponent<KMModuleBacking>());
            }
        }
        bomb.GetComponent<KMBomb>().Scale = 2.2f / size;
    }
}

