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

function App() {
  const [isAuthed, setIsAuthed] = useState(false);
  const [mode, setMode] = useState("login");
  const [authForm, setAuthForm] = useState({ name: "", email: "", password: "" });
  const [skillForm, setSkillForm] = useState(defaultSkill);
  const [questForm, setQuestForm] = useState(defaultQuest);
  const [skills, setSkills] = useState([]);
  const [quests, setQuests] = useState([]);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("Backend: http://localhost:5074");

  const totals = useMemo(() => {
    return quests.reduce(
      (acc, quest) => {
        if (quest.status === 1) {
          acc.xp += quest.experienceReward || 0;
          acc.coins += quest.coinReward || 0;
        }
        return acc;
      },
      { xp: 0, coins: 0 }
    );
  }, [quests]);

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
      const text = await response.text();
      throw new Error(text || `HTTP ${response.status}`);
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
      const [skillsData, questsData] = await Promise.all([
        request("/Skills/by-userId"),
        request("/Quests/by-userId")
      ]);

      setSkills(skillsData || []);
      setQuests(questsData || []);
      setIsAuthed(true);
      setMessage("Данные загружены.");
    } catch {
      setIsAuthed(false);
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

  async function createSkill(event) {
    event.preventDefault();

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

  function getSkillName(skillId) {
    return skills.find((skill) => skill.id === skillId)?.name || "Без навыка";
  }

  return (
    <main className="app-shell">
      <section className="topbar">
        <div>
          <p className="eyebrow">LifeGrind Prototype</p>
          <h1>Real Life RPG</h1>
        </div>
        <button className="ghost-button" onClick={refreshData} disabled={loading}>
          Обновить
        </button>
      </section>

      <section className="status-strip">
        <div>
          <span className="stat-label">Earned XP</span>
          <strong>{totals.xp}</strong>
        </div>
        <div>
          <span className="stat-label">Coins</span>
          <strong>{totals.coins}</strong>
        </div>
        <div>
          <span className="stat-label">Skills</span>
          <strong>{skills.length}</strong>
        </div>
        <div>
          <span className="stat-label">Quests</span>
          <strong>{quests.length}</strong>
        </div>
      </section>

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

      {isAuthed && (
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
                <article className="item-card" key={skill.id}>
                  <div>
                    <h3>{skill.name}</h3>
                    <p>{skill.description || "Описание не задано."}</p>
                  </div>
                  <div className="item-meta">
                    <span>{skill.experience} XP</span>
                    <button className="danger-button" onClick={() => deleteSkill(skill.id)} type="button">
                      Удалить
                    </button>
                  </div>
                </article>
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
                        </div>
                        <div className="quest-actions">
                          <button
                            className="success-button"
                            type="button"
                            onClick={() => questAction(quest.id, "complete")}
                            disabled={quest.status !== 0}
                          >
                            Complete
                          </button>
                          <button
                            className="ghost-button"
                            type="button"
                            onClick={() => questAction(quest.id, "cancel")}
                            disabled={quest.status !== 0}
                          >
                            Cancel
                          </button>
                          <button className="danger-button" type="button" onClick={() => deleteQuest(quest.id)}>
                            Delete
                          </button>
                        </div>
                      </article>
                    ))}
                  </div>
                </section>
              ))}
            </div>
          </section>
        </section>
      )}
    </main>
  );
}

export default App;
