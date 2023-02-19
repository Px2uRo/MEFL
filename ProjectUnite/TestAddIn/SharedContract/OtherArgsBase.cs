namespace MEFL.Contract;

public abstract class GameotherArgs
{
    public abstract string CustomDotMCPath { get; set; }
    public abstract bool StartInDebugMode { get; set; }
    public abstract string Description { get; set; }

    public abstract string OtherJVMArguments { get; set; }

    public abstract string OtherGameArguments { get; set; }

    public abstract string NativeLibrariesPath { get; set; }

    public abstract string CustomIconPath { get; set; }
    public abstract GamePathType GamePathType { get; set; }

    public abstract void ChangeProperty();
}

public enum GamePathType
{
    DotMCPath,Versions,Custom
}