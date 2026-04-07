---
name: pr-format
description: Formata texto bruto como Pull Request profissional
---

Analise as alterações atuais do repositório (arquivos modificados, diff e contexto) e gere uma Pull Request profissional no formato:

[feat/fix/refactor/chore/nuget - baseado no que foi modificado no arquivo] ([api, application, infra, data, domain, tests, shared] - baseado em quais projetos foi mexido (pode ser mais de um)): [Descrição explicativa do que foi feito no contexto geral]
Ou seja, feat (application, domain, data) - Adiciona nova funcionalidade de autenticação utilizando JWT, permitindo que os usuários façam login e acessem recursos protegidos.

- Lista objetiva de impactos, dependências ou riscos.