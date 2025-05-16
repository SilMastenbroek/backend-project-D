# 🧠 AI Developer Assistant – Backend

Deze applicatie is een AI-gestuurde assistent die developers helpt bij het plannen, begrijpen en uitvoeren van programmeertaken. De backend ondersteunt spraakgestuurde interactie, contextanalyse en integratie met Trello voor taakbeheer.

---

## ⚙️ Functionaliteiten

- 🎙️ **Spraakgestuurde interactie** via OpenAI Whisper (of tekstinvoer via console)
- 💬 **Conversaties met een AI-assistent** via OpenAI GPT
- 📌 **Automatische taakselectie en -uitleg** uit Trello
- 🔐 **Toestemming vragen via AIQuickChat** bij gevoelige acties
- 🧩 **Workflow-gebaseerde structuur**: Execute, Debug, Refactor, Create Task
- 📄 **README-parser**: voegt inhoud van een `readme.md` automatisch toe als context

---

## 🧭 Workflow-overzicht

1. **Start console-app**
2. **Selecteer gewenste workflow** (bijv. `Execute Task`)
3. **Console vraagt folderstructuur + Trello-taak**
4. **Indien een README aanwezig is**, wordt deze toegevoegd aan AI-context
5. **AI-assistent ontvangt context en stelt vervolgstappen voor**
6. **Als AI gevoelige data nodig heeft (zoals class/method lines)** → `AIQuickChat` vraagt eerst toestemming van gebruiker
7. **Gebruiker bepaalt hoe de AI verder mag**

---

## 🔧 Installatie

1. Clone dit project
2. Voeg `appsettings.json` toe in de root met de volgende structuur:

```json
{
  "OpenAI": {
    "ApiKey": "YOUR-OPENAI-KEY"
  },
  "Trello": {
    "ApiKey": "YOUR-TRELLO-KEY",
    "Token": "YOUR-TRELLO-TOKEN",
    "BoardId": "YOUR-BOARD-ID"
  },
  "FolderStructure": {
    "Path": "pad/naar/jouw/project"
  }
}
