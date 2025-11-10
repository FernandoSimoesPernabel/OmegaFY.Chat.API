---
mode: agent
description: Implementar testes de unidade para uma determinada classe existente.
inputs: ClassName
tools: ['codebase', 'editFiles', 'fetch', 'problems']
---

## Objective

Seu objetivo é implementar testes de unidade utilizando xUnit para cobrir 100% dos métodos públicos de uma determinada classe existente no projeto.

## Prerequisites

- Deve ser solicitado o nome da classe que será criado os testes.
- Não criar testes de unidade para classes que tenham dependencias externas que nao possam ser mockadas facilmente (exemplo: acesso a banco de dados, servicos externos, etc).

## Steps

1 - Localize a classe existente no projeto que corresponde ao nome fornecido no input {ClassName}.
2 - Crie uma pasta em OmegaFY.Chat.API.Tests.Unit seguindo a estrutura de pastas do projeto principal, se ainda nao existir.
** Exemplo: Se a classe pertence a OmegaFY.Chat.API.Infra -> MessageBus -> ConcurrentBagInMemoryMessageBus.cs, a pasta deve ser criada em OmegaFY.Chat.API.Tests.Unit -> Infra -> MessageBus -> ConcurrentBagInMemoryMessageBusFacts.cs
** Exemplo: Se a classe pertence a OmegaFY.Chat.API.Domain -> Entities -> Message.cs, a pasta deve ser criada em OmegaFY.Chat.API.Tests.Unit -> Domain -> Entities -> MessageFacts.cs
3 - Crie uma nova classe de teste na pasta de testes correspondente, seguindo a convenção de nomenclatura {ClassName}Facts.cs.
4 - A estrutura de nome dos testes deve seguir o seguinte formato: Method_Scenario_ExpectedBehavior.
** Exemplo: PublishAsync_ShouldHandleConcurrentMessagesFromMultipleSources_WithCorrectlyValuesAndCount
** Exemplo: ReadMessageAync_ShouldReturnMessageAndDecreaseCount_WhenIsNotEmpty
** Exemplo: ProducerConsumer_ShouldProcessAllMessages_UnderParallelLoad
** Exemplo: MakePrivate_PrivatePost_ShouldRemainsPrivate
** Exemplo: Constructor_PassingValidAuthor_ShouldPublishedPost
** Exemplo: Constructor_PassingOutOfRangeBody_ShouldThrowDomainArgumentException
** Exemplo: Constructor_ModifiedPost_ShouldHaveModificationDetailsDateOfModificationEqualsToUtcNow
5 - Implemente os testes de unidade utilizando xUnit para cobrir 100% dos metodos públicos da classe especificada.
6 - Priorizar utilizar [Theory] e [InlineData] ou [MemberData] para criar testes parametrizados quando aplicavel.

### Additional validations

- Confirmar que foi realizado 100% de cobertura na classe especificada.