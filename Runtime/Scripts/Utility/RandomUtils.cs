using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VenetStudio
{
    public partial class Utility
    {
        public static float GetRandom(float min, float max) => Random.Range(min, max);
        
        public static int GetRandom(int min, int max) => Random.Range(min, max);

    }

}