---
mode: agent
description: Adiciona os arquivos necessarios para implementar um CommandHandler no projeto
inputs: CommandName, Context
tools: ['codebase', 'editFiles', 'fetch', 'problems']
---

## Objective

Seu objetivo é adicionar os arquivos necessários sem codigo para implementar um CommandHandler no projeto.

## Prerequisites

- Deve ser solicitado o nome do Command que deve ser criado e qual contexto ele pertence.
- Exemplos:
** SendMessage pertence ao contexto de Chat.
** AddFriendship pertence ao contexto de Users;

## Steps

1 - Dentro de API.Application -> Commands -> {Context} (crie a pasta se ja nao exister o context) -> {CommandName} (crie a pasta se ja nao existir) crie:
1.1 - Dentro da pasta com o {CommandName} crie um arquivo chamado {CommandName}Command.cs implementando ICommand.
1.2 - Dentro da pasta com o {CommandName} crie um arquivo chamado {CommandName}CommandResult.cs implementando ICommandResult.
1.3 - Dentro da pasta com o {CommandName} crie um arquivo chamado {CommandName}CommandHandler.cs implementando CommandHandlerBase<THandler, TCommand, TCommandResult> criados anteriormente.
1.3.1 -  Deixar criado o construtor com injeção de dependências da classe base e tambem o metodo InternalHandleAsync sem implementação.
1.4 - Dentro da pasta com o {CommandName} crie um arquivo chamado {CommandName}CommandValidator.cs implementando AbstractValidator<{CommandName}Command>.

2 - Dentro da API.Application -> Events -> {Context} (crie a pasta se ja nao exister o context) -> {CommandName} (crie a pasta se ja nao existir) crie:
2.1 - Dentro da pasta com o {CommandName} crie um arquivo de evento com um nome correspondente ao Evento (ex: CreateGroupCommand/GroupCreatedEvent) implementando IEvent.
2.2 - Dentro da pasta com o {CommandName} crie um arquivo de EventHandler com um nome correspondente ao EventHandler (ex: GroupCreatedEvent/GroupCreatedEventHandler) implementando EventHandlerHandlerBase<TEvent> criado anteriormente.
2.2.1 Deixar o metodo HandleAsync sem implementação.

### Additional validations

- Ao terminar, confirmar que foi criado 3 arquivos na pasta do API.Application -> Commands -> {Context} -> {CommandName}.
- Confirmar que foi criado um Event e um EventHandler na pasta de Events do mesmo contexto e command.
- Confirmar que foi criado um arquivo na pasta de Validators chamado {CommandName}CommandValidator.cs implementando AbstractValidator<{CommandName}Command>.