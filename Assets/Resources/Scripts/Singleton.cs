using UnityEngine;

// An abstract class for MonoBehaviours that should implement singleton pattern. 
// Automatically destroys other instances as well as gives a public way to easily access the only instance.
public abstract class Singleton<MB> : MonoBehaviour where MB : Singleton<MB> {
    public static MB Instance {private set; get; }

	protected virtual void OnEnable() {
		if(Instance != null) {
			Destroy(this.gameObject);
			return;
		}
    
        Instance = this as MB;
        DontDestroyOnLoad(this.gameObject);
    }
}