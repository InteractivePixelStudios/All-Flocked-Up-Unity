using System;
using UnityEngine;

[System.Serializable]
public class Clothes
{
    public SkinnedMeshRenderer[] clothes;
}

public class HumanGeneratorManager : MonoBehaviour
{
    public SkinnedMeshRenderer[] BodyTypeMeshes;
    public Clothes[] skinnyMaleClothes;
    public Clothes[] builtMaleClothes;
    public Clothes[] heavysetMaleClothes;
    public Clothes[] skinnyFemaleClothes;
    public Clothes[] builtFemaleClothes;
    public Clothes[] heavysetFemaleClothes;

    public Mesh[] GenerateHuman()
    {
        int rand = UnityEngine.Random.Range(0, BodyTypeMeshes.Length);

        int index = 1;

        Mesh[] mesh = new Mesh[8];

        mesh[0] = BodyTypeMeshes[rand].sharedMesh;

        switch (rand)
        {
            case 0:
                foreach (Clothes clothes in skinnyMaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)].sharedMesh;
                    index++;
                }
                break;
            case 1:
                foreach (Clothes clothes in skinnyFemaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)].sharedMesh;
                    index++;
                }
                break;
            case 2:
                foreach (Clothes clothes in heavysetFemaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)].sharedMesh;
                    index++;
                }
                break;
        }

        return mesh;
    }
}
