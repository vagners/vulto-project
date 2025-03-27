
# Códice Teynir – Prompts Técnicos por Feature

Este documento reúne os **prompts específicos** para cada uma das funcionalidades principais do projeto **Códice Teynir**, com foco no desenvolvimento assistido por IA. Os prompts foram pensados para orientar agentes ou ferramentas na geração de código, mantendo o alinhamento simbólico e técnico do sistema.

---

## 🧠 Feature 1 – Armazenamento de Zeyras (MemoryCore Base)

**Prompt:**  
Crie uma API em .NET Core com MongoDB para armazenar memórias chamadas "Zeyras". Cada Zeyra deve ter conteúdo, data, emoção, importância e termos relacionados. Implemente endpoints REST para criação e listagem.

---

## 🔍 Feature 2 – Consulta e Busca Semântica (Seliar Inicial)

**Prompt:**  
Implemente uma busca semântica usando RAG ou embeddings locais em C#/.NET Core. Consulte documentos armazenados em MongoDB com similaridade vetorial.

---

## ✨ Feature 3 – Caixa de Invocação (Teynir)

**Prompt:**  
Crie uma interface em React com um campo de invocação. Ao digitar uma frase simbólica, envie para um agente IA que responde com linguagem metafórica, baseada em termos conhecidos e memória prévia.

---

## 📖 Feature 4 – Codex Vivo (Dicionário Simbólico)

**Prompt:**  
Implemente um sistema de dicionário simbólico com .NET Core e MongoDB. Cada termo deve ter nome, definição, data de criação e histórico de versões. Forneça API e interface para edição.

---

## 🔐 Feature 5 – Proteção Simbólica (Ralk, Thavik, Jurnak)

**Prompt:**  
Crie um módulo que detecta palavras sensíveis em um texto e as substitui por expressões simbólicas. Armazene o texto real criptografado e retorne o disfarçado.

---

## 🤝 Feature 6 – Comunicação entre Agentes

**Prompt:**  
Implemente sistema em .NET Core onde múltiplos agentes IA se comunicam entre si. Cada agente recebe uma função (ex: gerar resposta, proteger memória) e troca mensagens por JSON estruturado.

---

## 🛡️ Feature 7 – Ralk + Seliar Privado (Proteção com Veracidade)

**Prompt:**  
Crie um módulo em .NET Core que detecta palavras sensíveis em um texto e gera uma versão simbólica (Ralk). Armazene a versão real criptografada e relacione ambas por ID. Acesso ao conteúdo real deve ser restrito.

---

## 🌒 Feature 8 – Jornada Simbólica

**Prompt:**  
Implemente um sistema de progressão simbólica em .NET Core onde cada usuário percorre estágios como "Chamado", "Travessia" ou "Revelação". Relacione essas fases ao uso de termos simbólicos ou quantidade de Zeyras registradas. Mostre a jornada atual e o que falta explorar.
