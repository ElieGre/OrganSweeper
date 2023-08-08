using UnityEngine;

public static class OrganHelper {
	
	// Takes a string of an organ's name, returns its icon.
    public static Sprite GetOrganIconByName(string organ, bool healthy) {
		string healthyAdd = healthy ? "" : "Bad";
        switch (organ) {
            case "heart":
                return Resources.Load<Sprite>("Sprites/Organs/Heart" + healthyAdd) ;
            case "lungs":
                return Resources.Load<Sprite>("Sprites/Organs/Lungs" + healthyAdd);
            case "kidney":
                return Resources.Load<Sprite>("Sprites/Organs/Kidney" + healthyAdd);
            case "liver":
                return Resources.Load<Sprite>("Sprites/Organs/Liver" + healthyAdd);
            case "appendix":
                return Resources.Load<Sprite>("Sprites/Organs/Appedix" + healthyAdd);
            case "pancreas":
                return Resources.Load<Sprite>("Sprites/Organs/Pancreas" + healthyAdd);
            case "intestines":
                return Resources.Load<Sprite>("Sprites/Organs/Intestines" + healthyAdd);
            case "bladder":
                return Resources.Load<Sprite>("Sprites/Organs/Bladder" + healthyAdd);
            case "bloodPack":
                return Resources.Load<Sprite>("Sprites/Organs/BloodPack" + healthyAdd);
            case "empty":
                return null;
            default:
                Debug.LogError("Unknown organ: " + organ);
				return null;
        }
    }
}
