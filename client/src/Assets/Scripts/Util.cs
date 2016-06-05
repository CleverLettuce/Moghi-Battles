using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {

	public static string decodeMapName(string mapName)
    {
        string decoded = "unknown map";
        switch (mapName)
        {
            case "sacredforest": decoded = "Sacred Forest"; break;
        }

        return decoded;
    }
}
