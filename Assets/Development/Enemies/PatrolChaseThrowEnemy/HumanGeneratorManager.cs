using System;
using UnityEngine;

[System.Serializable]
public class Clothes
{
    public Mesh[] clothes;
}

public class HumanGeneratorManager : MonoBehaviour
{
    public Mesh[] BodyTypeMeshes;
    public Clothes[] skinnyMaleClothes;
    public Clothes[] builtMaleClothes;
    public Clothes[] heavysetMaleClothes;
    public Clothes[] skinnyFemaleClothes;
    public Clothes[] builtFemaleClothes;
    public Clothes[] heavysetFemaleClothes;

    public Mesh[] GenerateHuman()
    {
        int rand = UnityEngine.Random.Range(0, 6);

        int index = 1;

        Mesh[] mesh = new Mesh[6];

        mesh[0] = BodyTypeMeshes[rand];

        switch (rand)
        {
            case 0:
                foreach (Clothes clothes in skinnyMaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)];
                    index++;
                }
                break;
            case 1:
                foreach (Clothes clothes in builtMaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)];
                    index++;
                }
                break;
            case 2:
                foreach (Clothes clothes in heavysetMaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)];
                    index++;
                }
                break;
            case 3:
                foreach (Clothes clothes in skinnyFemaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)];
                    index++;
                }
                break;
            case 4:
                foreach (Clothes clothes in builtFemaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)];
                    index++;
                }
                break;
            case 5:
                foreach (Clothes clothes in heavysetFemaleClothes)
                {
                    mesh[index] = clothes.clothes[UnityEngine.Random.Range(0, clothes.clothes.Length)];
                    index++;
                }
                break;
        }

        return mesh;
    }
}
