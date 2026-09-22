---
name: pr-format
description: Format raw change notes into a professional Pull Request description
---

Analyze the current repository changes (modified files, diff, and context) and generate a professional Pull Request description using this format:

[feat/fix/refactor/chore/nuget - based on the type of changes] ([api, application, infra, data, domain, tests, shared] - based on impacted projects, one or more): [Clear high-level summary of what was implemented]

Example:
feat (application, domain, data): Adds a new JWT-based authentication flow, allowing users to log in and access protected resources.

Then include:
- An objective list of impacts, dependencies, or risks.
