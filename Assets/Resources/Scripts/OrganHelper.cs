using UnityEngine;

public static class OrganHelper {
	
	public static Sprite GetOrganIcon(Organ organ) {
		if(organ == null) 
			return null; 
			 
		string healthyAdd = organ.Healthy ? "" : "Bad"; 
		switch (organ.OrganType)  {
			case Organ.Type.Heart: 
				return Resources.Load<Sprite>("Sprites/Organs/Heart" + healthyAdd) ; 
			case Organ.Type.Lungs: 
				return Resources.Load<Sprite>("Sprites/Organs/Lungs" + healthyAdd); 
			case Organ.Type.Kidney: 
				return Resources.Load<Sprite>("Sprites/Organs/Kidney" + healthyAdd); 
			case Organ.Type.Liver: 
				return Resources.Load<Sprite>("Sprites/Organs/Liver" + healthyAdd); 
			case Organ.Type.Appendix: 
				return Resources.Load<Sprite>("Sprites/Organs/Appedix" + healthyAdd); 
			case Organ.Type.Pancreas: 
				return Resources.Load<Sprite>("Sprites/Organs/Pancreas" + healthyAdd); 
			case Organ.Type.Intestines: 
				return Resources.Load<Sprite>("Sprites/Organs/Intestines" + healthyAdd); 
			case Organ.Type.Bladder: 
				return Resources.Load<Sprite>("Sprites/Organs/Bladder" + healthyAdd); 
			default: 
				Debug.LogError("Unknown organ: " + organ); 
				return null; 
		} 
	} 
} 