using KitchenLib.Customs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenInferno.Customs.InfernoSetting
{
    public class InfernoSurfaceMaterial : CustomBaseMaterial
    {
        public override void ConvertMaterial(out Material material)
        {
            Material infernoSurface = new Material(Shader.Find("Lake Surface"));
            infernoSurface.SetColor("_Color0", new Color(0.795657754f, 0.517066061f, 0.220359266f));
            infernoSurface.SetColor("_Color1", new Color(0.477f, 0.31f, 0.132f, 0.5f));
            infernoSurface.SetFloat("_AlphaCutoff", 0.5f);
            infernoSurface.SetFloat("_TimeScale", 0.008f);
            infernoSurface.SetFloat("_Scale", 0.0008f);
            infernoSurface.name = "Inferno Surface";

            material = infernoSurface;
        }
    }
}
