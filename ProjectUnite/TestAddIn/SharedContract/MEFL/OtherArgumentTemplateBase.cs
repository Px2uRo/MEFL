namespace MEFL;

public abstract class OtherArgumentTemplateBase
{
	public abstract bool IsFavorite { get; set; }

	public abstract string Description { get; set; }

	public abstract string OtherJVMArguments { get; set; }

	public abstract string OtherGameArguments { get; set; }

	public abstract string NativeLibrariesPath { get; set; }

	public abstract string CustomIconPath { get; set; }

	public abstract void ChangeProperty();
}
