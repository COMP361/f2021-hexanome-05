using Models;
using Views;
using Controls;
using UnityEngine;

/// <summary>
/// Base class for model, view and controller
/// </summary>
public abstract class Elfenroads : MonoBehaviour
{
    public static ElfenroadsControl Control;
    public static ElfenroadsView View;
    public static ElfenroadsModel Model;
}
