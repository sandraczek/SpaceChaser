namespace SpaceChaser.Core.Highscore
{
    public interface IHighscore // On PlayerProvider
    {
        float AllTimeHigh { get; }
        float High { get; }
        float Current { get; }
        bool Available { get; }
    }
}