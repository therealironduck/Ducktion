# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Ducktion is a Unity package (`com.therealironduck.ducktion`) providing dependency injection for Unity games. Compatible with Unity 2022.3 LTS and Unity 6.0 LTS.

## Testing

Tests use NUnit via Unity Test Framework. There is no CLI test runner — tests must be run inside the Unity Editor (Window > General > Test Runner).

- **Editor tests** (`Tests/Editor/`): Pure logic tests, no Play mode needed. This is where most tests live.
- **Runtime tests** (`Tests/Runtime/`): Play mode tests for MonoBehaviour integration.

Editor tests extend the `DucktionTest` base class, which provides:
- A pre-configured `container` field (DiContainer instance) created in `SetUp`
- Override `Configure()` to customize test container settings (auto-resolve, log level, singleton mode, event bus)
- `FakeLogger()` helper to swap in a fake logger for assertion
- Automatic `Ducktion.Clear()` in `TearDown`

Test stubs live in `Tests/Editor/Stubs/` — add new fakes/stubs there.

## Architecture

### Assembly Structure

Four assemblies defined via `.asmdef` files:
- **TheRealIronDuck.Ducktion** (`Runtime/`) — core DI container, public API
- **TheRealIronDuck.Ducktion.Editor** (`Editor/`) — custom inspector for DiContainer
- **TheRealIronDuck.Ducktion.Editor.Tests** (`Tests/Editor/`) — editor tests
- **TheRealIronDuck.Ducktion.Tests** (`Tests/Runtime/`) — play mode tests

### Core Components

- **`DiContainer`** (MonoBehaviour): The DI container. Manages service registration, resolution, and lifecycle. Configured via serialized fields in the Inspector or via `Configure()` in code.
- **`Ducktion`** (static class): Singleton accessor — `Ducktion.singleton` returns the active DiContainer.
- **`ServiceDefinition`**: Metadata for a registered service (type, singleton/lazy mode, parameters, tags, callbacks). Uses a fluent API for configuration.
- **`TaggedServices`**: Collection wrapping services sharing a tag. Implements `IReadOnlyCollection<ServiceDefinition>` with `GetServices()` / `GetServices<T>()` for lazy resolution.

### Resolution Flow

1. **Explicit registration**: `container.Register<T>()` returns a `ServiceDefinition` for fluent config
2. **Auto-resolution**: If enabled, unregistered types are instantiated automatically with recursive dependency resolution
3. **Configurators**: Classes implementing `IDiConfigurator` (or extending `MonoDiConfigurator`) register services in bulk during container init
4. **Attribute-based**: `[Resolve]` on fields/properties/methods/constructor params triggers automatic injection; `[ResolveTags("tag")]` injects `TaggedServices`

### Event Bus

Built-in pub/sub via `EventBus`. Events implement the `IEvent` marker interface. Enable with `enableEventBus` on the container.

## Contribution Requirements

- All patches require tests
- One pull request per feature
- Squash intermediate commits before submitting
- Follow SemVer v2.0.0
