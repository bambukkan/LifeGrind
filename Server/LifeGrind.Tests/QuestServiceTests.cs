using LifeGrind.CORE.Exceptions;
using Moq;

namespace LifeGrind.Tests;

public class QuestServiceTests
{
    // private readonly IQuestService QuestService;
    // public QuestServiceTests(IQuestService _QuestService)
    // {
    //      QuestService =  _QuestService;
    // }
    [Fact]
    public async Task CompleteQuest_WhenQuestIsAlreadyCompleted()
    {
        var userId = Guid.NewGuid();
        var questId = Guid.NewGuid();
        
        var quest = new QuestEntity
        {
            Id = questId,
            UserId = userId,
            Status = QuestStatus.Completed
        };

        var questRepository = new Mock<IQuestRepository>();

        questRepository
            .Setup(repository => repository.GetQuest(questId))
            .ReturnsAsync(quest);

        var userRepository = new Mock<IUserRepository>();
        var skillRepository = new Mock<ISkillRepository>();
        var transactionManager = new Mock<ITransactionManager>();

        var questService = new QuestService(
            questRepository.Object,
            userRepository.Object,
            skillRepository.Object,
            transactionManager.Object
        );

        await Assert.ThrowsAsync<QuestHasAlreadyCompletedException>(
            () => questService.CompleteQuest(userId,questId)
        );

        userRepository.Verify(
            repository => repository.UpdateUserExpAndCoins(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<decimal>()),
            Times.Never);

        skillRepository.Verify(
            repository => repository.AddExperience(
                It.IsAny<Guid>(),
                It.IsAny<int>()),
            Times.Never);

        questRepository.Verify(
            repository => repository.CompleteQuest(
                It.IsAny<Guid>(),
                It.IsAny<QuestStatus>(),
                It.IsAny<DateTime>()),
            Times.Never);
        transactionManager.Verify(
            manager => manager.ExecuteAsync(It.IsAny<Func<Task>>()),
            Times.Never);
    }

    [Fact]
    public async Task CompleteQuest_WhenQuestIsAlreadyCancelled()
    {
        var userId = Guid.NewGuid();
        var questId = Guid.NewGuid();
        
        var quest = new QuestEntity
        {
            Id = questId,
            UserId = userId,
            Status = QuestStatus.Cancelled
        };

        var questRepository = new Mock<IQuestRepository>();

        questRepository
            .Setup(rep => rep.GetQuest(questId))
            .ReturnsAsync(quest);

        
        var userRepository = new Mock<IUserRepository>();
        var skillRepository = new Mock<ISkillRepository>();
        var transactionManager = new Mock<ITransactionManager>();

        var questService = new QuestService(
            questRepository.Object,
            userRepository.Object,
            skillRepository.Object,
            transactionManager.Object
        );

        await Assert.ThrowsAsync<QuestHasCancelled>(
            () => questService.CompleteQuest(userId,questId)
        );
        userRepository.Verify(
            repository => repository.UpdateUserExpAndCoins(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<decimal>()),
            Times.Never);

        skillRepository.Verify(
            repository => repository.AddExperience(
                It.IsAny<Guid>(),
                It.IsAny<int>()),
            Times.Never);

        questRepository.Verify(
            repository => repository.CompleteQuest(
                It.IsAny<Guid>(),
                It.IsAny<QuestStatus>(),
                It.IsAny<DateTime>()),
            Times.Never);
        transactionManager.Verify(
            manager => manager.ExecuteAsync(It.IsAny<Func<Task>>()),
            Times.Never);
    }

    [Theory]
    [InlineData(90,10,QuestStatus.Active)]
    [InlineData(90,1,QuestStatus.Active)]
    [InlineData(1,10000,QuestStatus.Active)]
    public async Task CompleteQuest_SuccessfullyExecuted(
        int ExperienceReward,
        int CoinReward,
        QuestStatus status
    )
    {
        var userId = Guid.NewGuid();
        var questId = Guid.NewGuid();
        var skillId = Guid.NewGuid();
        
        var quest = new QuestEntity
        {
            Id = questId,
            UserId = userId,
            SkillId = skillId,
            Status = status,
            ExperienceReward = ExperienceReward,
            CoinReward = CoinReward
        };

        var questRepository = new Mock<IQuestRepository>();

        questRepository
            .Setup(rep => rep.GetQuest(questId))
            .ReturnsAsync(quest);
        
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(rep => rep.UpdateUserExpAndCoins(userId,
            ExperienceReward,CoinReward)).Returns(Task.CompletedTask);

        var skillRepository = new Mock<ISkillRepository>();
        skillRepository
            .Setup(rep => rep.AddExperience(skillId,
            ExperienceReward)).Returns(Task.CompletedTask);
        
        questRepository
            .Setup(rep => rep.CompleteQuest(questId,
            QuestStatus.Completed,It.IsAny<DateTime>())).Returns(Task.CompletedTask);

        var transactionManager = new Mock<ITransactionManager>();

        transactionManager 
            .Setup(manager => manager.ExecuteAsync(It.IsAny<Func<Task>>()))
            // Принимает в качестве аргумента любую функцию типа Func<Task> 
            .Returns<Func<Task>>(action =>
            {
                return action();
            });

        var questService = new QuestService(
            questRepository.Object,
            userRepository.Object,
            skillRepository.Object,
            transactionManager.Object
        );

        await questService.CompleteQuest(userId, questId);

        transactionManager.Verify(
            manager => manager.ExecuteAsync(It.IsAny<Func<Task>>()),
            Times.Once);

        userRepository.Verify(
            repository => repository.UpdateUserExpAndCoins(
                userId,
                ExperienceReward,
                CoinReward),
            Times.Once);

        skillRepository.Verify(
            repository => repository.AddExperience(
                skillId,
                ExperienceReward),
            Times.Once);

        questRepository.Verify(
            repository => repository.CompleteQuest(
                questId,
                QuestStatus.Completed,
                It.IsAny<DateTime>()),
            Times.Once);
    }
}
