public class LevelCalculator
{
    private const int ExperiencePerLevel = 100;
    public static int GetLevel(int exp)
    {
        return exp / ExperiencePerLevel + 1;   
    }
    public static int GetExperienceForNextLevel(int exp)
    {
        return ExperiencePerLevel - exp % ExperiencePerLevel; 
    }
}