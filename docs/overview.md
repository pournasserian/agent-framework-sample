# agent-framework-sample: Repository Overview

## Purpose

`agent-framework-sample` is a set of AI-agent samples. You run them from a Blazor Server web app called **AgentsWebUI** (`0-Agents/AgentsWebUI`). Each sample page (`/agent01`, `/agent02`, …) shows one agent pattern. For example, *Agent 01 – Basic Agent* lets you edit the agent's instructions and message, then run it in normal (Submit) or streaming (Stream) mode against an OpenAI model through OpenRouter. Pages are styled with Bootstrap CSS, and Mermaid (loaded from a CDN) renders diagrams on the pages.

The modules documented in `docs/` cover the **Bootstrap v5.3.3 JavaScript library** copied into the app at `0-Agents/AgentsWebUI/wwwroot/lib/bootstrap/dist/js/`. This library adds client-side behaviour to `data-bs-*` markup, such as modals, dropdowns, tooltips, collapse and carousel. It ships in three builds with the same logic:

| File | Format | Popper.js |
|---|---|---|
| `bootstrap.bundle.js` | UMD (`window.bootstrap`) | Built in |
| `bootstrap.js` | UMD | External (`@popperjs/core` / `window.Popper`) |
| `bootstrap.esm.js` | ES module | External (`import * as Popper`) |

> **Note:** `Components/App.razor` currently loads only `bootstrap.min.css` and `blazor.web.js`. No Razor, C# or HTML file references any of the Bootstrap JS builds. The JS plugins described below are available in `wwwroot` but are not active in the app until a `<script>` tag (usually for `bootstrap.bundle.js`) is added.

## End-to-end Architecture

### Application runtime

```mermaid
graph LR
    Browser["Browser"] -- "HTTP + SignalR circuit" --> Server
    subgraph Server["AgentsWebUI (ASP.NET Core, Blazor Interactive Server)"]
        Program["Program.cs<br/>AddRazorComponents · MapStaticAssets"]
        App["App.razor / Routes"]
        Layout["MainLayout · NavMenu · ReconnectModal"]
        Pages["Pages/Agents/Agent01..N.razor"]
        Program --> App --> Layout --> Pages
    end
    Pages -- "Agent calls (sync / streaming)" --> LLM["LLM provider<br/>(e.g. OpenRouter → OpenAI)"]
    Server -- "static assets" --> WWW
    subgraph WWW["wwwroot"]
        CSS["lib/bootstrap/dist/css<br/>(loaded)"]
        JS["lib/bootstrap/dist/js<br/>(documented modules)"]
        AppCss["app.css"]
    end
    Browser -. "Mermaid ESM (CDN)" .-> CDN["cdn.jsdelivr.net"]
```

### Bootstrap JS module layering

```mermaid
graph TB
    Markup["Razor markup<br/>data-bs-toggle / data-bs-target / data-bs-*"] --> DataAPI["Data-API delegated listeners"]
    UserJS["Programmatic API<br/>bootstrap.X.getOrCreateInstance()"] --> Plugins

    DataAPI --> Plugins
    subgraph Plugins["Plugin modules"]
        subgraph Overlay["bootstrap_overlay_components"]
            Modal & Offcanvas
            Dropdown & Tooltip & Popover
            Toast
        end
        subgraph Interactive["bootstrap_interactive_content_components"]
            Alert & Button & Collapse
            Carousel & Tab & ScrollSpy
        end
    end

    subgraph Core["bootstrap_core_infrastructure"]
        Config --> BaseComponent
        Backdrop
        FocusTrap
        ScrollBarHelper
        Swipe
        TemplateFactory
    end

    Plugins -->|extend| BaseComponent
    Modal & Offcanvas --> Backdrop & FocusTrap & ScrollBarHelper
    Tooltip & Popover --> TemplateFactory
    Dropdown & Tooltip --> Popper["Popper.js"]
    Carousel --> Swipe

    Core --> Substrate["Internal substrate<br/>Data · EventHandler · Manipulator · SelectorEngine · util · sanitizer"]
    Substrate --> DOM["Browser DOM / CSS transitions"]
```

### Component lifecycle (shared by all plugins)

```mermaid
sequenceDiagram
    participant Trigger as User click / JS call
    participant Comp as Plugin (extends BaseComponent)
    participant Cfg as Config
    participant DOM
    Trigger->>Comp: getOrCreateInstance(el, cfg)
    Comp->>Cfg: merge Default ← data-bs-config ← data-bs-* ← JS cfg, then type-check
    Trigger->>Comp: show() / toggle() / close()
    Comp->>DOM: trigger "show.bs.x" (cancelable)
    alt not prevented
        Comp->>DOM: update classes / ARIA, use helpers
        Comp->>Comp: _queueCallback (waits for transitionend)
        Comp->>DOM: trigger "shown.bs.x"
    end
    Trigger->>Comp: dispose() → remove Data entry + ".bs.x" listeners
```

## Core Modules

| Module | Scope | Documentation |
|---|---|---|
| **bootstrap_core_infrastructure** | `Config` and `BaseComponent` base classes, plus the helpers `Backdrop`, `FocusTrap`, `ScrollBarHelper`, `Swipe` and `TemplateFactory` (HTML sanitization). Also covers config merge order, the one-instance-per-element rule, and the jQuery bridge. | [bootstrap_core_infrastructure.md](bootstrap_core_infrastructure.md) |
| **bootstrap_overlay_components** | Components that float over the page: Modal, Offcanvas, Dropdown, Tooltip, Popover and Toast. | [bootstrap_overlay_components.md](bootstrap_overlay_components.md) |
| ↳ modal dialogs | Modal and Offcanvas: backdrop, focus trap, scroll lock, static backdrop | [bootstrap_overlay_components_modal_dialogs.md](bootstrap_overlay_components_modal_dialogs.md) |
| ↳ popper positioned | Dropdown, Tooltip and Popover: Popper placement, triggers, sanitized templates | [bootstrap_overlay_components_popper_positioned.md](bootstrap_overlay_components_popper_positioned.md) |
| ↳ toast | Toast: notifications that hide automatically and pause on hover or focus | [bootstrap_overlay_components_toast.md](bootstrap_overlay_components_toast.md) |
| **bootstrap_interactive_content_components** | Widgets that change content already on the page: Alert, Button, Collapse/accordion, Carousel, Tab and ScrollSpy (uses IntersectionObserver). | [bootstrap_interactive_content_components.md](bootstrap_interactive_content_components.md) |

## Key Operational Notes

- **Popper:** use `bootstrap.bundle.js` unless Popper is loaded separately. Otherwise Dropdown and Tooltip throw an error saying they require Popper.
- **Declarative vs. programmatic use:** most plugins start automatically from `data-bs-*` attributes. Tooltip and Popover must be created in JavaScript.
- **Blazor interop:** Blazor re-renders parts of the page, so plugin instances attached to elements it replaces may need `dispose()` and to be recreated.
- **Security:** `TemplateFactory` sanitizes Tooltip and Popover HTML against an allowlist. Tooltip also ignores `sanitize`, `allowList` and `sanitizeFn` when they are set through `data-bs-*` attributes, so page markup can't turn sanitization off.