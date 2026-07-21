namespace LifeGrind.Tests;

public class LevelCalculatorTests
{
    [Fact]
    public void GetLevelByExperience()
    {
        var level = LevelCalculator.GetLevel(999);

        Assert.Equal(10,level);
    }
    [Theory]
    [InlineData(100,2,100)]
    [InlineData(999,10,1)]
    [InlineData(753,8,47)]
    public void GetManyTests(
        int experience,
        int expectedLevel,
        int expectedExperienceForNextLevel
    )
    {
        var level = LevelCalculator.GetLevel(experience);
        var ExperienceForNextLevel = LevelCalculator.GetExperienceForNextLevel(experience);

        Assert.Equal(expectedLevel,level);
        Assert.Equal(expectedExperienceForNextLevel,
        ExperienceForNextLevel);
    }
}
