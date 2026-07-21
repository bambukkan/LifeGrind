# LifeGrind

LifeGrind — учебный ASP.NET Core pet-проект, превращающий реальные
задачи пользователя в RPG-систему.

Пользователь создаёт навыки и квесты. Выполненный квест даёт общий
опыт, опыт связанного навыка и монеты.

Главная идея проекта: реальные полезные действия становятся квестами,
жизненные направления становятся навыками, а пользователь видит прогресс
своего "персонажа" через опыт, монеты, уровни и будущие награды.

Основные сущности:
- UserEntity
- SkillEntity
- QuestEntity

Связи:
- User имеет много Skills
- User имеет много Quests
- Skill имеет много Quests
- Quest принадлежит User и связан с одним Skill

Текущий MVP:
- пользователь;
- навыки пользователя;
- квесты пользователя;
- связь квеста с навыком;
- выполнение квеста;
- начисление опыта и монет.

Пока не усложнять проект будущими механиками без прямой необходимости:
- достижения;
- магазин;
- инвентарь;
- привычки;
- гильдии;
- рейтинги;
- сезоны;
- боссы и большие проекты.

Договорённости по DTO и связям:
- EF-сущности и навигационные свойства не принимать в request DTO;
- UserId берётся из JWT claim "UserId";
- SkillId при создании квеста может быть в CreateQuestRequest, потому что
  пользователь выбирает навык на клиенте;
- сервер должен проверять ownership: пользователь не может изменять,
  удалять, завершать или отменять чужие skills/quests;
- ownership проверять через конкретную сущность по id:
  сначала получить skill/quest по skillId/questId, затем сравнить UserId;
  не проверять владение через FirstOrDefault по одному только userId;
- EF-сущности не возвращать клиенту напрямую, позже сделать response DTO.

Договорённости по аутентификации:
- используется JWT в HttpOnly cookie с именем "Access-cookies";
- JwtBearer читает токен из cookie через OnMessageReceived;
- регистрация сразу логинит пользователя и ставит cookie;
- логин также ставит cookie;
- access token не нужно возвращать в body ответа;
- защищённые действия помечать [Authorize];
- текущего пользователя брать из claim "UserId";
- UserId не принимать от клиента в request DTO для пользовательских сценариев.

Договорённости по квестам:
- создание квеста задаёт UserId и SkillId;
- клиент не должен присылать награду при завершении квеста;
- награда берётся из самого QuestEntity;
- CompleteQuest и CancelQuest должны быть разными сценариями;
- CompleteQuest принудительно ставит статус Completed и начисляет награду;
- CancelQuest принудительно ставит статус Cancelled и не начисляет награду;
- нельзя повторно начислять награду за уже завершённый квест;
- нельзя отменять уже завершённый или уже отменённый квест без явного решения.
- публичного endpoint для прямого начисления опыта/монет быть не должно;
- UserRepository.UpdateUserExpAndCoins можно оставлять как внутренний метод,
  который вызывается из CompleteQuest.

Договорённости по API:
- обычные пользовательские GET /Skills и GET /Quests должны возвращать
  данные текущего пользователя, а не все записи из базы;
- получение всех skills/quests допустимо только как dev/admin-сценарий,
  но не как обычный endpoint пользователя;
- route id лучше писать с constraint, например {skillId:guid} и
  {questId:guid};
- id ресурса брать из route через [FromRoute], тело запроса — через
  [FromBody].

Архитектурный ориентир:
- сначала довести базовый игровой цикл до конца;
- не добавлять новые сущности только "на будущее";
- держать логику начисления наград в сервисном сценарии завершения квеста;
- репозитории должны работать с хранением, а не решать игровую экономику.

Это учебный проект. Пользователь хочет писать код самостоятельно.

При проверке кода:
- отвечай кратко и только на поставленный вопрос;
- сначала оцени подход;
- укажи конкретные ошибки;
- не переписывай весь код без прямой просьбы;
- не добавляй лишнюю архитектуру;
- не предлагай микросервисы, CQRS или MediatR без необходимости;
- учитывай общую концепцию проекта.

Учесть, что это ещё только начало, чтобы проверить, как вообще будет
работать базовый цикл. Сущностей и игровых механизмов позже может стать
больше, но сейчас важно не перегрузить проект раньше времени.

## Current implementation notes

### Response DTOs
- API responses use `UserResponse`, `SkillResponse`, and `QuestResponse`;
  EF entities and navigation properties are not returned to the client.
- Controllers map entities to response DTOs with private `ToResponse` methods.
- `UserResponse` contains `Id`, `Name`, `Email`, `TotalExperience`, and `Coins`.
- `SkillResponse` contains its own fields only; it does not expose `UserId`.
- `QuestResponse` contains `SkillId`, which the client needs to connect a
  quest with its skill, but does not expose `UserId`.

### Current-user endpoint
- `GET /Users` is the current-user endpoint (`GetMe` action), not a list of
  all users. It reads `UserId` from the JWT claim and returns `UserResponse`.
- `UserService.GetMe` throws `EntityNotFoundException` when a valid token
  refers to a deleted or missing user.

### Validation and API errors
- FluentValidation validators are registered from the API assembly.
- `ValidationFilter` is registered globally through `AddControllers` and
  returns HTTP 400 with a property-to-errors dictionary for invalid request DTOs.
- `GlobalExceptionMiddlware` converts `DomainException` to HTTP 400 and
  `EntityNotFoundException` to HTTP 404. Unexpected exceptions become HTTP 500.
- Response DTOs do not need FluentValidation; validate only input DTOs.
- Do not use `NotEmpty()` for `QuestDifficulty`: `Easy` is enum value `0` and
  would be rejected. Use `IsInEnum()` for this field instead.

### Client
- The Vite React client sends requests through `/api`, proxied to
  `http://localhost:5074`, and includes cookies with `credentials: "include"`.
- It uses the response fields from `GET /Skills` and `GET /Quests` directly.
- The dashboard currently calculates earned XP and coins from completed quests;
  it does not yet call `GET /Users`.

### Known next steps, not mandatory for this MVP pass
- Learn and add a transaction around `CompleteQuest`, because it changes the
  user, skill, and quest in one business action.
- Decide whether completed or cancelled quests should be immutable for updates.
- Add a unique email constraint and registration check before treating auth as
  production-ready.
- `GetQuestByUserId` is unused and can later be removed from the quest repository.
- After confirming every public endpoint returns response DTOs, the
  `ReferenceHandler.IgnoreCycles` JSON setting can be removed.
