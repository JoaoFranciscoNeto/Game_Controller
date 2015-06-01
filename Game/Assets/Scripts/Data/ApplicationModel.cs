using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class ApplicationModel : MonoBehaviour {

    public static Dictionary<IPAddress, Player> controllers = new Dictionary<IPAddress,Player>();

}
