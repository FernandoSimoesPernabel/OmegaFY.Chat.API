---
mode: agent
description: Adiciona os arquivos necessarios para implementar um QueryHandler no projeto
inputs: QueryName, Context
tools: ['codebase', 'editFiles', 'fetch', 'problems']
---

## Objective

Seu objetivo é adicionar os arquivos necessários sem codigo para implementar um QueryHandler no projeto.

## Prerequisites

- Deve ser solicitado o nome da Query que deve ser criado e qual contexto ele pertence.
- Exemplos:
** GetMessages pertence ao contexto de Chat.
** GetUserFriendships pertence ao contexto de Users;

## Steps

1 - Dentro de API.Application -> Queries -> {Context} (crie a pasta se ja nao exister o context) -> {QueryName} (crie a pasta se ja nao existir) crie:
1.1 - Dentro da pasta com o {QueryName} crie um arquivo chamado {QueryName}Query.cs implementando IQuery.
1.2 - Dentro da pasta com o {QueryName} crie um arquivo chamado {QueryName}QueryResult.cs implementando IQueryResult.
1.3 - Dentro da pasta com o {QueryName} crie um arquivo chamado {QueryName}QueryHandler.cs implementando QueryHandlerBase<THandler, TQuery, TQueryResult> criados anteriormente.
1.3.1 Deixar criado o construtor com injeção de dependências da classe base e tambem o metodo InternalHandleAsync sem implementação.
1.4 - Dentro da pasta com o {QueryName} crie um arquivo chamado {QueryName}QueryValidator.cs implementando AbstractValidator<{QueryName}Query>.

### Additional validations

- Ao terminar, confirmar que foi criado 3 arquivos na pasta do API.Application -> Queries -> {Context} -> {QueryName}.
- Confirmar que foi criado um arquivo na pasta de Validators chamado {QueryName}QueryValidator.cs implementando AbstractValidator<{QueryName}Query>.