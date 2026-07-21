import { useEffect, useMemo, useState } from "react";

const API_URL = "/api";

const difficultyOptions = [
  { value: 0, label: "Easy" },
  { value: 1, label: "Medium" },
  { value: 2, label: "Hard" }
];

const statusNames = {
  0: "Active",
  1: "Completed",
  2: "Cancelled"
};

const dateTimeFormatter = new Intl.DateTimeFormat("ru-RU", {
  dateStyle: "medium",
  timeStyle: "short"
});

function formatDateTime(value) {
  return value ? dateTimeFormatter.format(new Date(value)) : "-";
}

function formatApiError(payload, fallback) {
  if (!payload) {
    return fallback;
  }

  const validationErrors = payload.errors || payload.Errors;
  if (validationErrors) {
    return Object.entries(validationErrors)
      .flatMap(([field, messages]) =>
        Array.isArray(messages)
          ? messages.map((message) => `${field}: ${message}`)
          : [`${field}: ${messages}`]
      )
      .join("; ");
  }

  return payload.message || payload.Message || payload.error || payload.Error || fallback;
}

const defaultSkill = {
  name: "",
  description: "",
  experience: 0
};

const defaultQuest = {
  title: "",
  description: "",
  difficulty: 0,
  experienceReward: 25,
  coinReward: 5,
  skillId: ""
};

const defaultProfileForm = {
  name: "",
  email: "",
  oldPassword: "",
  newPassword: ""
};

function getCurrentPage() {
  return window.location.pathname === "/profile" ? "profile" : "dashboard";
}

function App() {
  const [page, setPage] = useState(getCurrentPage);
  const [isAuthed, setIsAuthed] = useState(false);
  const [mode, setMode] = useState("login");
  const [authForm, setAuthForm] = useState({ name: "", email: "", password: "" });
  const [skillForm, setSkillForm] = useState(defaultSkill);
  const [questForm, setQuestForm] = useState(defaultQuest);
  const [editingSkill, setEditingSkill] = useState(null);
  const [editingQuest, setEditingQuest] = useState(null);
  const [user, setUser] = useState(null);
  const [profileForm, setProfileForm] = useState(defaultProfileForm);
  const [skills, setSkills] = useState([]);
  const [quests, setQuests] = useState([]);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("Backend: http://localhost:5074");

  const questGroups = useMemo(
    () => [
      { title: "Active", status: 0, items: quests.filter((quest) => quest.status === 0) },
      { title: "Completed", status: 1, items: quests.filter((quest) => quest.status === 1) },
      { title: "Cancelled", status: 2, items: quests.filter((quest) => quest.status === 2) }
    ],
    [quests]
  );

  useEffect(() => {
    refreshData();
  }, []);

  useEffect(() => {
    const handlePopState = () => setPage(getCurrentPage());

    window.addEventListener("popstate", handlePopState);
    return () => window.removeEventListener("popstate", handlePopState);
  }, []);

  useEffect(() => {
    if (user) {
      setProfileForm({
        name: user.name,
        email: user.email,
        oldPassword: "",
        newPassword: ""
      });
    }
  }, [user]);

  function navigate(path) {
    window.history.pushState({}, "", path);
    setPage(getCurrentPage());
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  async function request(path, options = {}) {
    const response = await fetch(`${API_URL}${path}`, {
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
        ...(options.headers || {})
      },
      ...options
    });

    if (!response.ok) {
      const contentType = response.headers.get("content-type") || "";
      const fallback = `HTTP ${response.status}`;

      if (contentType.includes("application/json")) {
        const payload = await response.json();
        throw new Error(formatApiError(payload, fallback));
      }

      const text = await response.text();
      throw new Error(text || fallback);
    }

    const contentType = response.headers.get("content-type") || "";
    if (contentType.includes("application/json")) {
      return response.json();
    }

    return null;
  }

  async function refreshData() {
    try {
      setLoading(true);
      const [userData, skillsData, questsData] = await Promise.all([
        request("/Users"),
        request("/Skills"),
        request("/Quests")
      ]);

      setUser(userData);
      setSkills(skillsData || []);
      setQuests(questsData || []);
      setIsAuthed(true);
      setMessage("Данные загружены.");
    } catch {
      setIsAuthed(false);
      setUser(null);
      setSkills([]);
      setQuests([]);
      setMessage("Зарегистрируйся или войди, чтобы начать гринд.");
    } finally {
      setLoading(false);
    }
  }

  async function handleAuth(event) {
    event.preventDefault();

    const path = mode === "login" ? "/Users/login" : "/Users";
    const payload =
      mode === "login"
        ? { email: authForm.email, password: authForm.password }
        : authForm;

    try {
      setLoading(true);
      await request(path, {
        method: "POST",
        body: JSON.stringify(payload)
      });

      setAuthForm({ name: "", email: "", password: "" });
      setIsAuthed(true);
      setMessage(mode === "login" ? "Вход выполнен." : "Герой создан.");
      await refreshData();
    } catch (error) {
      setMessage(`Auth error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function updateProfile(event) {
    event.preventDefault();

    try {
      setLoading(true);
      await request("/Users", {
        method: "PUT",
        body: JSON.stringify(profileForm)
      });

      setMessage("Профиль обновлён.");
      await refreshData();
    } catch (error) {
      setMessage(`Profile error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function logout() {
    try {
      setLoading(true);
      await request("/Users/logout", { method: "DELETE" });
      setIsAuthed(false);
      setUser(null);
      setSkills([]);
      setQuests([]);
      setProfileForm(defaultProfileForm);
      navigate("/");
      setMessage("Вы вышли из аккаунта.");
    } catch (error) {
      setMessage(`Logout error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function deleteAccount() {
    if (!window.confirm("Удалить аккаунт вместе со всеми навыками и квестами? Это действие нельзя отменить.")) {
      return;
    }

    try {
      setLoading(true);
      await request("/Users", { method: "DELETE" });
      await request("/Users/logout", { method: "DELETE" });
      setIsAuthed(false);
      setUser(null);
      setSkills([]);
      setQuests([]);
      setProfileForm(defaultProfileForm);
      navigate("/");
      setMessage("Аккаунт удалён.");
    } catch (error) {
      setMessage(`Profile error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function createSkill(event) {
    event.preventDefault();

    if (!skillForm.name.trim()) {
      setMessage("Название навыка не может быть пустым.");
      return;
    }

    try {
      setLoading(true);
      await request("/Skills", {
        method: "POST",
        body: JSON.stringify({
          ...skillForm,
          experience: Number(skillForm.experience)
        })
      });

      setSkillForm(defaultSkill);
      setMessage("Навык добавлен.");
      await refreshData();
    } catch (error) {
      setMessage(`Skill error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function createQuest(event) {
    event.preventDefault();

    if (!questForm.title.trim()) {
      setMessage("Название квеста не может быть пустым.");
      return;
    }

    if (!questForm.skillId) {
      setMessage("Сначала выбери навык для квеста.");
      return;
    }

    try {
      setLoading(true);
      await request("/Quests", {
        method: "POST",
        body: JSON.stringify({
          ...questForm,
          difficulty: Number(questForm.difficulty),
          experienceReward: Number(questForm.experienceReward),
          coinReward: Number(questForm.coinReward)
        })
      });

      setQuestForm(defaultQuest);
      setMessage("Квест создан.");
      await refreshData();
    } catch (error) {
      setMessage(`Quest error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function questAction(questId, action) {
    try {
      setLoading(true);
      await request(`/Quests/${questId}/${action}`, { method: "POST" });
      setMessage(action === "complete" ? "Квест завершен. Награда получена." : "Квест отменен.");
      await refreshData();
    } catch (error) {
      setMessage(`Action error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function deleteSkill(skillId) {
    try {
      setLoading(true);
      await request(`/Skills/${skillId}`, { method: "DELETE" });
      setMessage("Навык удален.");
      await refreshData();
    } catch (error) {
      setMessage(`Delete error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  async function deleteQuest(questId) {
    try {
      setLoading(true);
      await request(`/Quests/${questId}`, { method: "DELETE" });
      setMessage("Квест удален.");
      await refreshData();
    } catch (error) {
      setMessage(`Delete error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  function startSkillEdit(skill) {
    setEditingSkill({
      id: skill.id,
      name: skill.name,
      description: skill.description || "",
      experience: skill.experience
    });
  }

  async function updateSkill(event) {
    event.preventDefault();

    if (!editingSkill.name.trim()) {
      setMessage("Название навыка не может быть пустым.");
      return;
    }

    try {
      setLoading(true);
      await request(`/Skills/${editingSkill.id}`, {
        method: "PUT",
        body: JSON.stringify({
          name: editingSkill.name,
          description: editingSkill.description,
          experience: Number(editingSkill.experience)
        })
      });

      setEditingSkill(null);
      setMessage("Навык обновлён.");
      await refreshData();
    } catch (error) {
      setMessage(`Skill error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  function startQuestEdit(quest) {
    setEditingQuest({
      id: quest.id,
      title: quest.title,
      description: quest.description || "",
      difficulty: quest.difficulty,
      experienceReward: quest.experienceReward,
      coinReward: quest.coinReward
    });
  }

  async function updateQuest(event) {
    event.preventDefault();

    if (!editingQuest.title.trim()) {
      setMessage("Название квеста не может быть пустым.");
      return;
    }

    try {
      setLoading(true);
      await request(`/Quests/${editingQuest.id}`, {
        method: "PUT",
        body: JSON.stringify({
          title: editingQuest.title,
          description: editingQuest.description,
          difficulty: Number(editingQuest.difficulty),
          experienceReward: Number(editingQuest.experienceReward),
          coinReward: Number(editingQuest.coinReward)
        })
      });

      setEditingQuest(null);
      setMessage("Квест обновлён.");
      await refreshData();
    } catch (error) {
      setMessage(`Quest error: ${error.message}`);
    } finally {
      setLoading(false);
    }
  }

  function getSkillName(skillId) {
    return skills.find((skill) => skill.id === skillId)?.name || "Без навыка";
  }

  return (
    <main className="app-shell">
      {isAuthed ? (
        <section className="player-banner">
          <button className="player-identity" type="button" onClick={() => navigate("/profile")}>
            <span className="avatar" aria-hidden="true">{user?.name?.slice(0, 1).toUpperCase() || "U"}</span>
            <span>
              <span className="player-label">Профиль</span>
              <strong>{user?.name || "Герой"}</strong>
              <span className="player-progress">Уровень {user?.level ?? 1} · {user?.experienceForNextLevel ?? 100} XP до следующего</span>
            </span>
          </button>
          <div className="player-stats">
            <div><span>XP</span><strong>{user?.totalExperience ?? 0}</strong></div>
            <div><span>Coins</span><strong>{user?.coins ?? 0}</strong></div>
            <div><span>Skills</span><strong>{skills.length}</strong></div>
            <div><span>Quests</span><strong>{quests.length}</strong></div>
          </div>
          <button className="ghost-button refresh-button" onClick={refreshData} disabled={loading}>Обновить</button>
        </section>
      ) : (
        <section className="topbar">
          <div>
            <p className="eyebrow">LifeGrind Prototype</p>
            <h1>Real Life RPG</h1>
          </div>
        </section>
      )}

      <p className={`message ${isAuthed ? "good" : ""}`}>{loading ? "Загрузка..." : message}</p>

      {!isAuthed && (
        <section className="auth-panel">
          <div className="panel-copy">
            <p className="eyebrow">Start</p>
            <h2>Создай героя или войди</h2>
            <p>
              Минимальный экран для проверки cookie-auth, навыков, квестов и выдачи наград.
            </p>
          </div>

          <form className="form-card" onSubmit={handleAuth}>
            <div className="tabs">
              <button type="button" className={mode === "login" ? "active" : ""} onClick={() => setMode("login")}>
                Login
              </button>
              <button type="button" className={mode === "register" ? "active" : ""} onClick={() => setMode("register")}>
                Register
              </button>
            </div>

            {mode === "register" && (
              <label>
                Name
                <input
                  value={authForm.name}
                  onChange={(event) => setAuthForm({ ...authForm, name: event.target.value })}
                  placeholder="gk"
                />
              </label>
            )}

            <label>
              Email
              <input
                type="email"
                value={authForm.email}
                onChange={(event) => setAuthForm({ ...authForm, email: event.target.value })}
                placeholder="hero@lifegrind.local"
              />
            </label>

            <label>
              Password
              <input
                type="password"
                value={authForm.password}
                onChange={(event) => setAuthForm({ ...authForm, password: event.target.value })}
                placeholder="password"
              />
            </label>

            <button className="primary-button" type="submit" disabled={loading}>
              {mode === "login" ? "Войти" : "Создать героя"}
            </button>
          </form>
        </section>
      )}

      {isAuthed && page === "dashboard" && (
        <section className="dashboard-grid">
          <form className="panel" onSubmit={createSkill}>
            <div className="panel-heading">
              <p className="eyebrow">Skill Forge</p>
              <h2>Новый навык</h2>
            </div>

            <label>
              Название
              <input
                value={skillForm.name}
                onChange={(event) => setSkillForm({ ...skillForm, name: event.target.value })}
                placeholder="English"
                required
              />
            </label>

            <label>
              Описание
              <textarea
                value={skillForm.description}
                onChange={(event) => setSkillForm({ ...skillForm, description: event.target.value })}
                placeholder="Words, speaking, listening"
              />
            </label>

            <label>
              Начальный опыт
              <input
                type="number"
                value={skillForm.experience}
                onChange={(event) => setSkillForm({ ...skillForm, experience: event.target.value })}
              />
            </label>

            <button className="primary-button" type="submit" disabled={loading}>
              Добавить навык
            </button>
          </form>

          <form className="panel" onSubmit={createQuest}>
            <div className="panel-heading">
              <p className="eyebrow">Quest Board</p>
              <h2>Новый квест</h2>
            </div>

            <label>
              Название
              <input
                value={questForm.title}
                onChange={(event) => setQuestForm({ ...questForm, title: event.target.value })}
                placeholder="Выучить 30 слов"
                required
              />
            </label>

            <label>
              Описание
              <textarea
                value={questForm.description}
                onChange={(event) => setQuestForm({ ...questForm, description: event.target.value })}
                placeholder="Записать, повторить, проверить себя"
              />
            </label>

            <div className="split-row">
              <label>
                Сложность
                <select
                  value={questForm.difficulty}
                  onChange={(event) => setQuestForm({ ...questForm, difficulty: event.target.value })}
                >
                  {difficultyOptions.map((option) => (
                    <option key={option.value} value={option.value}>
                      {option.label}
                    </option>
                  ))}
                </select>
              </label>

              <label>
                Навык
                <select
                  value={questForm.skillId}
                  onChange={(event) => setQuestForm({ ...questForm, skillId: event.target.value })}
                >
                  <option value="">Выбрать</option>
                  {skills.map((skill) => (
                    <option key={skill.id} value={skill.id}>
                      {skill.name}
                    </option>
                  ))}
                </select>
              </label>
            </div>

            <div className="split-row">
              <label>
                XP
                <input
                  type="number"
                  value={questForm.experienceReward}
                  onChange={(event) => setQuestForm({ ...questForm, experienceReward: event.target.value })}
                />
              </label>
              <label>
                Coins
                <input
                  type="number"
                  value={questForm.coinReward}
                  onChange={(event) => setQuestForm({ ...questForm, coinReward: event.target.value })}
                />
              </label>
            </div>

            <button className="primary-button" type="submit" disabled={loading || skills.length === 0}>
              Создать квест
            </button>
          </form>

          <section className="panel list-panel">
            <div className="panel-heading">
              <p className="eyebrow">Skills</p>
              <h2>Навыки</h2>
            </div>

            <div className="list-stack">
              {skills.length === 0 && <p className="empty">Пока нет навыков.</p>}
              {skills.map((skill) => (
                editingSkill?.id === skill.id ? (
                  <form className="item-card edit-card" key={skill.id} onSubmit={updateSkill}>
                    <label>
                      Название
                      <input
                        value={editingSkill.name}
                        onChange={(event) => setEditingSkill({ ...editingSkill, name: event.target.value })}
                        required
                      />
                    </label>
                    <label>
                      Описание
                      <textarea
                        value={editingSkill.description}
                        onChange={(event) => setEditingSkill({ ...editingSkill, description: event.target.value })}
                      />
                    </label>
                    <label>
                      Опыт
                      <input
                        type="number"
                        value={editingSkill.experience}
                        onChange={(event) => setEditingSkill({ ...editingSkill, experience: event.target.value })}
                        min="0"
                        required
                      />
                    </label>
                    <div className="inline-actions">
                      <button className="primary-button" type="submit" disabled={loading}>Сохранить</button>
                      <button className="ghost-button" type="button" onClick={() => setEditingSkill(null)} disabled={loading}>Отмена</button>
                    </div>
                  </form>
                ) : (
                  <article className="item-card" key={skill.id}>
                    <div>
                      <h3>{skill.name}</h3>
                      <p>{skill.description || "Описание не задано."}</p>
                    </div>
                    <div className="item-meta">
                      <span className="skill-progress">
                        <strong>{skill.experience} XP</strong>
                        <small>Уровень {skill.level ?? 1} · {skill.experienceForNextLevel ?? 100} XP до следующего</small>
                      </span>
                      <button className="ghost-button" onClick={() => startSkillEdit(skill)} type="button">Изменить</button>
                      <button className="danger-button" onClick={() => deleteSkill(skill.id)} type="button">Удалить</button>
                    </div>
                  </article>
                )
              ))}
            </div>
          </section>

          <section className="panel list-panel quests-panel">
            <div className="panel-heading">
              <p className="eyebrow">Quests</p>
              <h2>Квесты</h2>
            </div>

            <div className="quest-groups">
              {quests.length === 0 && <p className="empty">Пока нет квестов.</p>}
              {questGroups.map((group) => (
                <section className="quest-group" key={group.status}>
                  <div className="quest-group-heading">
                    <h3>{group.title}</h3>
                    <span>{group.items.length}</span>
                  </div>

                  <div className="list-stack">
                    {group.items.length === 0 && <p className="empty">No quests.</p>}
                    {group.items.map((quest) => (
                      editingQuest?.id === quest.id ? (
                        <form className="item-card edit-card" key={quest.id} onSubmit={updateQuest}>
                          <label>
                            Название
                            <input
                              value={editingQuest.title}
                              onChange={(event) => setEditingQuest({ ...editingQuest, title: event.target.value })}
                              required
                            />
                          </label>
                          <label>
                            Описание
                            <textarea
                              value={editingQuest.description}
                              onChange={(event) => setEditingQuest({ ...editingQuest, description: event.target.value })}
                            />
                          </label>
                          <div className="split-row">
                            <label>
                              Сложность
                              <select
                                value={editingQuest.difficulty}
                                onChange={(event) => setEditingQuest({ ...editingQuest, difficulty: event.target.value })}
                              >
                                {difficultyOptions.map((option) => (
                                  <option key={option.value} value={option.value}>{option.label}</option>
                                ))}
                              </select>
                            </label>
                            <label>
                              XP
                              <input
                                type="number"
                                min="1"
                                value={editingQuest.experienceReward}
                                onChange={(event) => setEditingQuest({ ...editingQuest, experienceReward: event.target.value })}
                                required
                              />
                            </label>
                          </div>
                          <label>
                            Coins
                            <input
                              type="number"
                              min="1"
                              value={editingQuest.coinReward}
                              onChange={(event) => setEditingQuest({ ...editingQuest, coinReward: event.target.value })}
                              required
                            />
                          </label>
                          <div className="inline-actions">
                            <button className="primary-button" type="submit" disabled={loading}>Сохранить</button>
                            <button className="ghost-button" type="button" onClick={() => setEditingQuest(null)} disabled={loading}>Отмена</button>
                          </div>
                        </form>
                      ) : (
                        <article className={`item-card quest-card status-${quest.status}`} key={quest.id}>
                          <div>
                            <div className="quest-title-row">
                              <h3>{quest.title}</h3>
                              <span>{statusNames[quest.status] || quest.status}</span>
                            </div>
                            <p>{quest.description || "Описание не задано."}</p>
                            <p className="subtle">
                              {getSkillName(quest.skillId)} · {difficultyOptions[quest.difficulty]?.label || "Unknown"} ·{" "}
                              {quest.experienceReward} XP · {quest.coinReward} coins
                            </p>
                            <p className="subtle">
                              Создан: {formatDateTime(quest.createdAt)}
                              {quest.completedAt && (
                                <> · {quest.status === 1 ? "Завершён" : "Отменён"}: {formatDateTime(quest.completedAt)}</>
                              )}
                            </p>
                          </div>
                          {quest.status === 0 && (
                            <div className="quest-actions">
                              <button className="ghost-button" type="button" onClick={() => startQuestEdit(quest)}>Изменить</button>
                              <button
                                className="success-button"
                                type="button"
                                onClick={() => questAction(quest.id, "complete")}
                              >
                                Complete
                              </button>
                              <button
                                className="ghost-button"
                                type="button"
                                onClick={() => questAction(quest.id, "cancel")}
                              >
                                Cancel
                              </button>
                              <button className="danger-button" type="button" onClick={() => deleteQuest(quest.id)}>
                                Delete
                              </button>
                            </div>
                          )}
                        </article>
                      )
                    ))}
                  </div>
                </section>
              ))}
            </div>
          </section>
        </section>
      )}

      {isAuthed && page === "profile" && (
        <section className="profile-page">
          <div className="page-heading">
            <div>
              <p className="eyebrow">Hero profile</p>
              <h1>{user?.name || "Профиль"}</h1>
              <p className="profile-email">{user?.email}</p>
              <p className="profile-level">Уровень {user?.level ?? 1} · {user?.experienceForNextLevel ?? 100} XP до следующего уровня</p>
            </div>
            <button className="ghost-button" type="button" onClick={() => navigate("/")}>На главную</button>
          </div>

          <div className="profile-grid">
            <form className="panel" onSubmit={updateProfile}>
              <div className="panel-heading">
                <p className="eyebrow">Account</p>
                <h2>Данные аккаунта</h2>
              </div>

              <label>
                Имя
                <input
                  value={profileForm.name}
                  onChange={(event) => setProfileForm({ ...profileForm, name: event.target.value })}
                  required
                />
              </label>

              <label>
                Email
                <input
                  type="email"
                  value={profileForm.email}
                  onChange={(event) => setProfileForm({ ...profileForm, email: event.target.value })}
                  required
                />
              </label>

              <label>
                Текущий пароль
                <input
                  type="password"
                  value={profileForm.oldPassword}
                  onChange={(event) => setProfileForm({ ...profileForm, oldPassword: event.target.value })}
                  required
                />
              </label>

              <label>
                Новый пароль
                <input
                  type="password"
                  value={profileForm.newPassword}
                  onChange={(event) => setProfileForm({ ...profileForm, newPassword: event.target.value })}
                  required
                />
              </label>

              <button className="primary-button" type="submit" disabled={loading}>Сохранить изменения</button>
            </form>

            <section className="panel account-actions">
              <div className="panel-heading">
                <p className="eyebrow">Session</p>
                <h2>Аккаунт</h2>
              </div>
              <p>Выйди на этом устройстве или полностью удали профиль вместе с его данными.</p>
              <button className="ghost-button" type="button" onClick={logout} disabled={loading}>Выйти из аккаунта</button>
              <button className="danger-button" type="button" onClick={deleteAccount} disabled={loading}>Удалить аккаунт</button>
            </section>
          </div>
        </section>
      )}
    </main>
  );
}

export default App;
