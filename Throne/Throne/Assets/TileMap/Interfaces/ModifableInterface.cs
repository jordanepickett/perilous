using System.Collections.Generic;
using UnityEngine;

interface ModifableInterface {
    void ModifyVerteces(Vector3 t, bool lowerHeight);
    void AddCliff(Vector3 t, int cliffHeight);
    void AddTextureToTerrain(List<Vector3> t, VertexType brushType, float alpha = 1);
}
