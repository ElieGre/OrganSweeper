public class Organ {
	public enum Type {Heart, Lungs, Kidney, Liver, Appendix, Pancreas, Intestines, Bladder} 
	 
	public Type OrganType { private set; get; } 
	public bool Healthy  { private set; get; } 
	 
	public Organ(Type type, bool healthy) {
		OrganType = type; 
		Healthy = healthy; 
	} 
} 
