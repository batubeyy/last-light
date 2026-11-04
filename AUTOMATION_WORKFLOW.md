# AUTOMATION_WORKFLOW.md

## Recommended execution order

1. Create a clean checkpoint commit from the current working prototype.
2. Start Claude Code in the repository root and ask it to read `CLAUDE.md`.
3. Run one sprint at a time. Do not issue “finish the whole game” as one task.
4. For each sprint:
   - planning pass by the main agent
   - implementation by `unity-implementer`
   - validation by `unity-test-engineer`
   - read-only review by `unity-code-reviewer`
   - human Play Mode approval
   - human merge
5. After Sprint 12, invoke `release-manager` to prepare artifacts only.

## First prompt

```text
Read CLAUDE.md and inspect the repository. Do not edit anything yet.
Produce a repository reality-check that compares the actual code, scenes, prefabs, and settings against the documented completed Sprints 1-7.
Report mismatches, uncommitted changes, missing references, Console/build risks, and a proposed Sprint 8 plan with acceptance criteria.
Wait for approval before implementation.
```

## Sprint implementation prompt

```text
Implement only Sprint 8 from CLAUDE.md on a new branch named agent/sprint-8-flashlight-energy.
First inspect the actual project and present the plan. Preserve all completed mechanics.
Delegate focused implementation to unity-implementer, tests to unity-test-engineer, and final read-only review to unity-code-reviewer.
Do not merge, publish, install packages, or modify unrelated files.
Stop when the branch is ready for my manual Unity Play Mode review.
```

## Human approval checklist
- Open the branch in Unity.
- Wait for compilation.
- Confirm zero red Console errors.
- Run the agent-provided manual test list.
- Inspect visual changes.
- Review git diff.
- Only then merge.

## Release preparation prompt

```text
Invoke release-manager for Sprint 12 release preparation.
Prepare a Windows 0.1.0 artifact, release documents, asset licence audit, SHA-256 checksum, itch.io metadata pack, and Game Jolt metadata pack.
Do not upload, set final pricing, enter credentials, accept terms, or publish.
Return the exact human steps that remain.
```
