namespace SpaceChaser.Core.Highscore
{
    public interface IHighscore // On PlayerProvider
    {
        float High { get; }
        float Current { get; }
        bool Available { get; }
    }
}