#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class DoubleSidedColliderGenerator : EditorWindow
{
    GameObject targetObject;

    [MenuItem("Tools/Generate Double-Sided Mesh Collider")]
    static void ShowWindow()
    {
        var window = GetWindow<DoubleSidedColliderGenerator>();
        window.titleContent = new GUIContent("Double-Sided Collider");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("원본 메쉬가 있는 GameObject를 지정하세요", EditorStyles.boldLabel);
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("1) 새로운 반전 메쉬 생성") && targetObject != null)
        {
            GenerateInvertedMesh(targetObject);
        }

        if (GUILayout.Button("2) 두 개의 MeshCollider 붙이기") && targetObject != null)
        {
            AddDoubleSidedColliders(targetObject);
        }
    }

    void GenerateInvertedMesh(GameObject go)
    {
        // 1) MeshFilter 또는 SkinnedMeshRenderer에서 메시 가져오기
        MeshFilter mf = go.GetComponent<MeshFilter>();
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();

        Mesh original = null;
        bool isSkinned = false;
        if (mf != null)
        {
            original = mf.sharedMesh;
        }
        else if (smr != null)
        {
            original = smr.sharedMesh;
            isSkinned = true;
        }
        else
        {
            Debug.LogError("[Double-Sided] MeshFilter나 SkinnedMeshRenderer가 없습니다.");
            return;
        }

        if (original == null)
        {
            Debug.LogError("[Double-Sided] 원본 Mesh가 할당되어 있지 않습니다.");
            return;
        }

        // 2) 메시 복제
        Mesh invertedMesh = Object.Instantiate(original);
        invertedMesh.name = original.name + "_Inverted";

        // 3) 삼각형 인덱스 반전 (역순으로)
        int subMeshCount = invertedMesh.subMeshCount;
        for (int s = 0; s < subMeshCount; s++)
        {
            int[] triangles = invertedMesh.GetTriangles(s);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // (a, b, c) → (a, c, b)로 뒤집기
                int tmp = triangles[i + 1];
                triangles[i + 1] = triangles[i + 2];
                triangles[i + 2] = tmp;
            }
            invertedMesh.SetTriangles(triangles, s);
        }

        // 4) 노멀 반전 (선택 사항: 충돌과는 별개지만, 시각적으로도 뒤집고 싶다면)
        Vector3[] normals = invertedMesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -normals[i];
        invertedMesh.normals = normals;

        // 5) 프로젝트에 Asset으로 저장
        string folderPath = "Assets/GeneratedMeshAssets";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "GeneratedMeshAssets");
        }

        string assetPath = folderPath + "/" + invertedMesh.name + ".asset";
        AssetDatabase.CreateAsset(invertedMesh, assetPath);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Double-Sided Collider", $"반전 메시가 생성되어 저장되었습니다:\n{assetPath}", "확인");
    }

    void AddDoubleSidedColliders(GameObject go)
    {
        // 1) 이미 MeshCollider가 있다면 제거 (선택)
        MeshCollider[] existing = go.GetComponents<MeshCollider>();
        foreach (var mc in existing)
            DestroyImmediate(mc);

        // 2) 원본 SharedMesh 가져오기
        MeshFilter mf = go.GetComponent<MeshFilter>();
        SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
        Mesh original = mf != null ? mf.sharedMesh : (smr != null ? smr.sharedMesh : null);
        if (original == null)
        {
            Debug.LogError("[Double-Sided] 원본 메시를 찾을 수 없습니다.");
            return;
        }

        // 3) Inverted 메쉬 Asset 경로 유추
        string invertedName = original.name + "_Inverted";
        string[] guids = AssetDatabase.FindAssets(invertedName + " t:Mesh", new[] { "Assets/GeneratedMeshAssets" });
        if (guids.Length == 0)
        {
            Debug.LogError($"[Double-Sided] \"{invertedName}\" 메쉬 에셋을 찾을 수 없습니다.\n먼저 1) 반전 메쉬를 생성하세요.");
            return;
        }

        string invPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        Mesh inverted = AssetDatabase.LoadAssetAtPath<Mesh>(invPath);

        // 4) GameObject에 두 개의 MeshCollider 추가
        MeshCollider mcA = go.AddComponent<MeshCollider>();
        mcA.sharedMesh = original;
        mcA.convex = false;  // 내부 구조라면 Convex는 끄는 것이 맞음

        MeshCollider mcB = go.AddComponent<MeshCollider>();
        mcB.sharedMesh = inverted;
        mcB.convex = false;

        EditorUtility.DisplayDialog("Double-Sided Collider", "원본과 반전 메시 모두에 MeshCollider가 붙었습니다.", "확인");
    }
}
#endif
