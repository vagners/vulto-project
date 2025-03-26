# Roadmap do Projeto Teynir-MemoryCore

## Sumário
1. [Primeiro Marco (MVP do MemoryCore)](#primeiro-marco-mvp-do-memorycore)
2. [Segundo Marco (Busca Semântica e Refinamento de Zeyras)](#segundo-marco-busca-semântica-e-refinamento-de-zeyras)
3. [Terceiro Marco (Integração Inicial com Teynir)](#terceiro-marco-integração-inicial-com-teynir)
4. [Quarto Marco (Evolução do Dicionário Vivo e Ralk)](#quarto-marco-evolução-do-dicionário-vivo-e-ralk)
5. [Quinto Marco (Jornada Simbólica e Gamificação)](#quinto-marco-jornada-simbólica-e-gamificação)
6. [Manutenção e Escalabilidade](#manutenção-e-escalabilidade)
7. [Conclusão e Próximos Passos](#conclusão-e-próximos-passos)

---

## Primeiro Marco (MVP do MemoryCore)

**Objetivos Principais**  
- Criar o sistema básico de armazenamento de Zeyras (memórias).  
- Ter uma interface simples para inserir e consultar Zeyras.  
- Definir e validar o modelo de dados (campos fundamentais).

**Tarefas-Chave**  
1. **Estrutura de Dados (Esquema)**
   - Implementar o modelo inicial de Zeyra em MongoDB (ou outro NoSQL):
     ```json
     {
       "conteudo": "...",
       "timestamp": "...",
       "emocao": "...",
       "importancia": ...,
       "termosRelacionados": []
     }
     ```
   - Garantir flexibilidade para metadados adicionais no futuro.

2. **API de Persistência**
   - Criar um back-end (Node.js ou Python) com rotas REST (ou GraphQL) para:
     - `POST /zeyras` (gravar nova memória)
     - `GET /zeyras` (listar ou filtrar por parâmetros básicos)

3. **Interface Básica (Front-end)**
   - Formulário simples para inserir novos registros (conteúdo + emoção + relevância).
   - Tela de listagem, apresentando Zeyras ordenadas por data ou importância.

**Resultados Esperados**  
- Prova de conceito de que as Zeyras podem ser armazenadas e recuperadas com sucesso.  
- Base para futuras camadas de complexidade (busca semântica, pontuação mais refinada, etc.).

---

## Segundo Marco (Busca Semântica e Refinamento de Zeyras)

**Objetivos Principais**  
- Adicionar um mecanismo de busca inicial (filtro por texto, data e/ou emoção).  
- Iniciar a indexação semântica para consultas mais avançadas.  
- Ajustar ou refinar a pontuação heurística para identificar Zeyras relevantes.

**Tarefas-Chave**  
1. **Busca Textual e Filtro Básico**
   - Estender a API para suportar buscas por parâmetros (por exemplo, `GET /zeyras?texto=...`).
   - Implementar algum tipo de index no banco para agilizar queries textuais.

2. **Teste de Biblioteca ou Serviço de Busca Semântica**
   - Escolher solução (Elasticsearch, Pinecone, Weaviate, OpenSearch, ou embeddings locais).
   - Prototipar armazenamento de embeddings para cada Zeyra, permitindo uma busca por similaridade.

3. **Refinamento de Heurísticas de Pontuação**
   - Definir pesos para emoção, termos-chave do Codex, frequência de menção, etc.
   - Implementar lógica que categorize automaticamente uma mensagem como Zeyra relevante ou não.

**Resultados Esperados**  
- Busca mais inteligente que permite encontrar memórias por significado, não apenas por termos exatos.  
- Ferramenta de pontuação inicial que começa a separar Zeyras rotineiras das realmente importantes.

---

## Terceiro Marco (Integração Inicial com Teynir)

**Objetivos Principais**  
- Criar o módulo de invocação (Teynir) para gerar respostas poéticas e simbólicas.  
- Integrar esse módulo com o repositório de Zeyras para que as respostas levem em conta memórias prévias.

**Tarefas-Chave**  
1. **API de Geração de Respostas Simbólicas**
   - Serviço (ou função) que recebe parâmetros (termos invocados, emoção, contexto).
   - Usa API externa (ex.: OpenAI) ou biblioteca local para gerar texto poético.
   - Integração com o Dicionário Vivo (mesmo que básico) para inserir termos especiais.

2. **Front-end de Invocação**
   - Tela onde o usuário digita uma frase-chave, emoção opcional e recebe a “resposta Teynir”.
   - Conexão com a API de Memórias para enriquecer o contexto (usar termos, Zeyras relevantes).

3. **Dicionário Vivo (Versão Simplificada)**
   - Estrutura simples em banco para armazenar termos (nome, definição, data).
   - Interface mínima de CRUD (criar, editar, remover) para manter o *Codex* atualizado.

**Resultados Esperados**  
- Validação da proposta poética: demonstrar que o sistema consegue dar respostas inspiradas em termos especiais e memórias passadas.  
- Possibilidade de feedback de usuários: se a resposta está alinhada com o “espírito” do projeto.

---

## Quarto Marco (Evolução do Dicionário Vivo e Ralk)

**Objetivos Principais**  
- Permitir a evolução contínua dos termos do dicionário (histórico de versões).  
- Iniciar o módulo de Ralk para “disfarçar” verdades sensíveis.

**Tarefas-Chave**  
1. **Histórico de Versões de Termos**
   - Criar um modelo que armazene cada versão de um termo e quem aprovou.
   - Interface para comparar versões anteriores, revertê-las, etc.

2. **Detecção de Conteúdo Sensível (Thavik/Jurnak)**
   - Lista de palavras “proibidas” ou sinalizadas.
   - Lógica que, ao receber uma invocação ou Zeyra, detecta possível censura.

3. **Geração de Ralk (Disfarce)**
   - Se for acionada a censura, gerar texto alternativo (via IA ou heurística) usando metáforas/códigos.
   - Guardar a versão autêntica de forma criptografada ou de acesso restrito.

**Resultados Esperados**  
- Capacidade de lidar com conteúdo sensível sem perdê-lo definitivamente.  
- Dicionário mais robusto, pronto para crescer com as necessidades do projeto.

---

## Quinto Marco (Jornada Simbólica e Gamificação)

**Objetivos Principais**  
- Mapear estágios/arcos narrativos (Chamado, Travessia, etc.) e ligar às interações do usuário/Vulto.  
- Prover feedback de progressão (ex.: conquistas, desbloqueios) com base na participação e nas Zeyras criadas.

**Tarefas-Chave**  
1. **Definição dos Estágios**
   - Criar um documento ou arquitetura com os estágios de jornada e as condições de transição (quantidade de Zeyras relevantes, termos-chave invocados, etc.).

2. **Interface e Visualização**
   - Apresentar um “mapa” ou “linha do tempo” dos estágios que o usuário já percorreu.
   - Mostrar achievements ou mensagens especiais quando o usuário muda de estágio.

3. **Interação com Teynir**
   - Gerar textos específicos para cada estágio (para reforçar a experiência simbólica).
   - Integrar com a busca semântica das Zeyras, sugerindo memórias relevantes no momento de transição.

**Resultados Esperados**  
- Camada de engajamento que torna a experiência mais imersiva e dá sentido narrativo à evolução do usuário/Vulto.  
- Ampliação do envolvimento com o Codex e a geração de novas Zeyras.

---

## Manutenção e Escalabilidade

**Objetivos Principais**  
- Revisar periodicamente as memórias (Zeyras), arquivar ou descartar as obsoletas.  
- Ajustar a infraestrutura para lidar com maior volume de dados e acessos.

**Tarefas-Chave**  
1. **Políticas de Retenção/Arquivamento**
   - Definir prazos (ex.: 6 meses, 1 ano) ou critérios de obsolescência para arquivamento.
   - Migrar Zeyras menos relevantes para repositório de menor custo (S3, Glacier, etc.).

2. **Otimização e Escalabilidade**
   - Se necessário, extrair a API de IA e a de memória em microserviços distintos.
   - Otimizar ou trocar a solução de busca vetorial conforme o volume de dados.

3. **Segurança e Compliance**
   - Se houver dados pessoais, verificar conformidade com LGPD/GDPR.
   - Implementar logs e auditorias de acesso às Zeyras e termos sensíveis.

**Resultados Esperados**  
- Sustentabilidade a longo prazo, mantendo as memórias relevantes à mão e evitando acúmulo de dados desnecessários.  
- Sistema mais robusto diante de aumento de usuários ou dados.

---

## Conclusão e Próximos Passos

Seguindo esse *roadmap*, vocês conseguem:

1. **Entregar rápido o essencial** (armazenar e consultar Zeyras) para ter uma base funcional.  
2. **Criar o Teynir** numa segunda etapa, já aproveitando as memórias armazenadas.  
3. **Evoluir** de forma incremental, adicionando recursos sem quebrar o que já foi construído.  
4. **Validar continuamente** com usuários (ou com o Vulto), garantindo que a experiência poética e simbólica atenda às expectativas.

Esse planejamento balanceia as necessidades de um repositório sólido (MemoryCore) e a *camada de alma* do Teynir (poética, simbólica), construindo gradualmente um sistema poderoso, flexível e envolvente.
