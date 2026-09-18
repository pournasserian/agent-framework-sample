# Bootstrap Overlay Components – Modal Dialogs (`Modal`, `Offcanvas`)

## Introduction

This module covers Bootstrap v5.3.3's two **blocking overlay** components in the AgentsWebUI front end:

| Component | Purpose | Typical markup |
|-----------|---------|----------------|
| `Modal` | Centered dialog over the page. Used for confirmations, forms and detail views. | `.modal > .modal-dialog > .modal-content` |
| `Offcanvas` | Panel that slides in from one edge of the viewport. Used for side navigation, filters and drawers. | `.offcanvas.offcanvas-start/end/top/bottom` |

The two components work the same way. Each one:

- is a subclass of `BaseComponent`
- uses a `Backdrop` to dim and block the rest of the page
- uses a `FocusTrap` to keep keyboard focus inside the dialog
- uses `ScrollBarHelper` to lock page scrolling and make up for the hidden scrollbar's width
- sets the `role="dialog"` and `aria-modal` attributes and returns focus to the trigger element when it closes
- fires cancelable lifecycle events: `show`, `shown`, `hide`, `hidden` and `hidePrevented`

The same code ships in three bundles under `0-Agents/AgentsWebUI/wwwroot/lib/bootstrap/dist/js/`:

| File | Format | Popper |
|------|--------|--------|
| `bootstrap.bundle.js` | UMD (global `bootstrap`) | Bundled inline |
| `bootstrap.js` | UMD | External `@popperjs/core` |
| `bootstrap.esm.js` | ES module (`export { Modal, Offcanvas, … }`) | Imported from `@popperjs/core` |

`Modal` and `Offcanvas` do not use Popper. Their behavior is identical in all three bundles.

### Related documentation

- [bootstrap_overlay_components](bootstrap_overlay_components.md): the parent module, an overview of all overlay components.
- [bootstrap_core_infrastructure](bootstrap_core_infrastructure.md): `BaseComponent`, `Config`, `Backdrop`, `FocusTrap` and `ScrollBarHelper`. This page does not repeat them.
- [bootstrap_overlay_components_popper_positioned](bootstrap_overlay_components_popper_positioned.md): `Dropdown`, `Tooltip` and `Popover`. `Tooltip` watches `hide.bs.modal` so it can close itself when its parent modal closes.
- [bootstrap_overlay_components_toast](bootstrap_overlay_components_toast.md): `Toast`, a non-blocking notification.
- [bootstrap_interactive_content_components](bootstrap_interactive_content_components.md): `Alert`, `Collapse`, `Tab` and the other content components.

---

## Architecture

```mermaid
classDiagram
    class Config {
        +Default
        +DefaultType
        +NAME
        #_getConfig(config)
        #_typeCheckConfig(config)
    }
    class BaseComponent {
        #_element
        #_config
        +dispose()
        #_queueCallback(cb, el, animated)
        +getInstance(el)$
        +getOrCreateInstance(el, cfg)$
    }
    class Modal {
        -_dialog
        -_backdrop : Backdrop
        -_focustrap : FocusTrap
        -_scrollBar : ScrollBarHelper
        -_isShown
        -_isTransitioning
        +toggle(relatedTarget)
        +show(relatedTarget)
        +hide()
        +handleUpdate()
        +dispose()
        -_showElement()
        -_hideModal()
        -_triggerBackdropTransition()
        -_adjustDialog()
        -_resetAdjustments()
    }
    class Offcanvas {
        -_backdrop : Backdrop
        -_focustrap : FocusTrap
        -_isShown
        +toggle(relatedTarget)
        +show(relatedTarget)
        +hide()
        +dispose()
    }
    class Backdrop
    class FocusTrap
    class ScrollBarHelper

    Config <|-- BaseComponent
    BaseComponent <|-- Modal
    BaseComponent <|-- Offcanvas
    Config <|-- Backdrop
    Config <|-- FocusTrap
    Modal *-- Backdrop
    Modal *-- FocusTrap
    Modal *-- ScrollBarHelper
    Offcanvas *-- Backdrop
    Offcanvas *-- FocusTrap
    Offcanvas ..> ScrollBarHelper : creates per show/hide
```

### Dependencies

```mermaid
flowchart LR
    subgraph ThisModule[modal_dialogs]
        M[Modal]
        O[Offcanvas]
    end
    subgraph Core[bootstrap_core_infrastructure]
        BC[BaseComponent]
        BD[Backdrop]
        FT[FocusTrap]
        SB[ScrollBarHelper]
    end
    subgraph DomUtil[DOM utilities]
        EH[EventHandler]
        SE[SelectorEngine]
        MN[Manipulator]
        DT[Data]
        EDT[enableDismissTrigger]
        JQ[defineJQueryPlugin]
    end
    TT[Tooltip / Popover] -.->|listens hide.bs.modal| M

    M --> BC & BD & FT & SB
    O --> BC & BD & FT & SB
    M & O --> EH & SE & EDT & JQ
    BC --> DT & MN
```

---

## Modal

### Configuration

Options are merged in this order, with later sources winning: `Default`, then JSON in `data-bs-config`, then `data-bs-*` attributes, then the constructor object. `Config._typeCheckConfig` validates the result.

| Option | Type | Default | Meaning |
|--------|------|---------|---------|
| `backdrop` | `boolean \| 'static'` | `true` | `true`: a backdrop is shown and clicking it closes the modal. `'static'`: a backdrop is shown but clicking it does not close the modal; the modal plays a "bounce" animation instead. `false`: no backdrop. |
| `focus` | `boolean` | `true` | Turns on the `FocusTrap` once the modal is shown. |
| `keyboard` | `boolean` | `true` | Escape closes the modal. If `false`, Escape plays the static "bounce" animation instead. |

### Public API

| Method | Description |
|--------|-------------|
| `show(relatedTarget?)` | Opens the modal. Does nothing if the modal is already shown or is mid-transition. |
| `hide()` | Closes the modal. Does nothing if the modal is not shown or is mid-transition. |
| `toggle(relatedTarget?)` | Calls `show` or `hide`, depending on the current state. |
| `handleUpdate()` | Recalculates the padding adjustment. Call it after the modal's height changes. |
| `dispose()` | Removes listeners on `window` and the dialog, disposes the backdrop, turns off the focus trap, and then calls `BaseComponent.dispose()`. |
| `Modal.getInstance(el)` / `Modal.getOrCreateInstance(el, cfg)` | Static helpers inherited from `BaseComponent`. |

### Events (namespace `.bs.modal`)

| Event | Cancelable | Fired when | `relatedTarget` |
|-------|-----------|------------|-----------------|
| `show.bs.modal` | yes | At the start of `show()` | trigger element |
| `shown.bs.modal` | – | After the fade-in finishes and focus is trapped | trigger element |
| `hide.bs.modal` | yes | At the start of `hide()` | – |
| `hidden.bs.modal` | – | After the backdrop is removed and scrolling is restored | – |
| `hidePrevented.bs.modal` | yes | Something tried to close a static or non-keyboard modal | – |

### Show / hide sequence

```mermaid
sequenceDiagram
    participant U as User / Trigger
    participant M as Modal
    participant SB as ScrollBarHelper
    participant BD as Backdrop
    participant FT as FocusTrap
    participant DOM

    U->>M: show(relatedTarget)
    M->>DOM: trigger show.bs.modal (cancelable)
    alt defaultPrevented
        M-->>U: abort
    end
    M->>M: _isShown = true, _isTransitioning = true
    M->>SB: hide() (overflow hidden + padding)
    M->>DOM: body.classList.add('modal-open')
    M->>M: _adjustDialog()
    M->>BD: show(callback)
    BD-->>M: callback → _showElement()
    M->>DOM: append to body if detached, display:block,<br/>aria-modal, role=dialog, scrollTop=0
    M->>DOM: reflow + add .show
    M->>M: _queueCallback(on .modal-dialog transition)
    M->>FT: activate() (if config.focus)
    M->>DOM: trigger shown.bs.modal

    U->>M: hide()
    M->>DOM: trigger hide.bs.modal (cancelable)
    M->>FT: deactivate()
    M->>DOM: remove .show
    M->>M: _queueCallback → _hideModal()
    M->>DOM: display:none, aria-hidden, remove role/aria-modal
    M->>BD: hide(callback)
    BD-->>M: remove 'modal-open', reset padding, SB.reset()
    M->>DOM: trigger hidden.bs.modal
```

### State machine

```mermaid
stateDiagram-v2
    [*] --> Hidden
    Hidden --> Showing: show() / show.bs.modal not prevented
    Showing --> Shown: dialog transition end → shown.bs.modal
    Shown --> Hiding: hide() / hide.bs.modal not prevented
    Hiding --> Hidden: element + backdrop transitions end → hidden.bs.modal
    Shown --> StaticBounce: Esc (keyboard=false) or backdrop click (backdrop='static')
    StaticBounce --> Shown: .modal-static removed after transition
    note right of Showing: _isTransitioning = true\nshow()/hide() calls are ignored
```

### Dismissal rules

- **Escape key** (`keydown.dismiss.bs.modal` on the modal element): if `keyboard` is on, the modal closes. Otherwise `_triggerBackdropTransition()` runs.
- **Backdrop click**: the modal pairs a `mousedown` listener with a one-time `click` listener. It only reacts when both events land on the `.modal` element itself, outside `.modal-dialog`. This skips drags that start inside the dialog and clicks on the scrollbar. With `backdrop: 'static'` the click plays the bounce animation. With any other truthy value the modal closes.
- **`[data-bs-dismiss="modal"]`**: registered by `enableDismissTrigger(Modal)`. It finds the closest `.modal`, or the element named in `data-bs-target`, and calls `hide()`.

`_triggerBackdropTransition()` fires `hidePrevented.bs.modal`. It then adds `.modal-static`, and adds `overflow-y: hidden` if the modal does not overflow the viewport. After two dialog transitions it restores the original state and focuses the modal. The method returns early if a bounce is already in progress.

### Scrollbar and overflow compensation

`_adjustDialog()` works out whether the page or the modal is overflowing:

- **Body overflows, modal does not**: adds padding equal to the scrollbar width on the right, or on the left in RTL.
- **Body does not overflow, modal does**: adds the padding on the opposite side.

It runs on `show()`, on `handleUpdate()`, and on `resize.bs.modal` from `window`, but only while the modal is shown and not transitioning. `_resetAdjustments()` clears the padding when the modal hides.

---

## Offcanvas

### Configuration

| Option | Type | Default | Meaning |
|--------|------|---------|---------|
| `backdrop` | `boolean \| 'static'` | `true` | Shows an `.offcanvas-backdrop`. `'static'`: clicking the backdrop fires `hidePrevented` instead of closing. |
| `keyboard` | `boolean` | `true` | Escape closes the panel. If `false`, Escape fires `hidePrevented`. |
| `scroll` | `boolean` | `false` | `false`: body scrolling is locked through `ScrollBarHelper`. `true`: the page can still scroll. |

### How it differs from Modal

| Aspect | Modal | Offcanvas |
|--------|-------|-----------|
| Backdrop class / parent | `modal-backdrop` appended to `body` | `offcanvas-backdrop` appended to the panel's parent node |
| Backdrop animation | Only if the modal has `.fade` | Always animated |
| Backdrop click handling | Handled by the modal element's mousedown/click listeners | Handled by the backdrop's `clickCallback` |
| Show sequencing | The dialog is shown after the backdrop finishes (callback chain) | The backdrop and panel animate at the same time |
| Transition classes | `.show` | `.showing` → `.show`, and `.hiding` when closing |
| ScrollBarHelper | One instance, kept for the component's lifetime | A new instance on each show/hide, and only when `scroll` is `false` |
| Focus trap | Turned on when `focus` is `true` | Turned on when `!scroll \|\| backdrop` |
| Static feedback | Bounce animation (`.modal-static`) | Only the `hidePrevented.bs.offcanvas` event |
| Mid-transition guard | `_isTransitioning` flag | None; only `_isShown` is checked |
| `hide()` extras | – | Calls `element.blur()` |

### Events (namespace `.bs.offcanvas`)

The events are `show`, `shown`, `hide`, `hidden` and `hidePrevented`. `show` and `shown` carry `relatedTarget`, which is the trigger element.

### Lifecycle

```mermaid
sequenceDiagram
    participant U as Trigger
    participant O as Offcanvas
    participant BD as Backdrop (offcanvas-backdrop)
    participant SB as new ScrollBarHelper
    participant FT as FocusTrap

    U->>O: show(relatedTarget)
    O->>O: trigger show.bs.offcanvas (cancelable)
    O->>BD: show()
    opt !scroll
        O->>SB: hide()
    end
    O->>O: aria-modal, role=dialog, add .showing
    O->>O: _queueCallback (always animated)
    opt !scroll || backdrop
        O->>FT: activate()
    end
    O->>O: add .show, remove .showing → shown.bs.offcanvas

    U->>O: hide()
    O->>O: trigger hide.bs.offcanvas (cancelable)
    O->>FT: deactivate()
    O->>O: element.blur()
    O->>O: add .hiding
    O->>BD: hide()
    O->>O: remove .show/.hiding, aria attrs
    opt !scroll
        O->>SB: reset()
    end
    O->>O: trigger hidden.bs.offcanvas
```

### Document and window handlers

| Listener | Behavior |
|----------|----------|
| `click.bs.offcanvas.data-api` on `[data-bs-toggle="offcanvas"]` | Calls `preventDefault` for `<a>` and `<area>` elements and skips disabled triggers. It registers a one-time handler that returns focus to the trigger on `hidden`, closes any other open `.offcanvas.show`, and then calls `toggle(this)`. |
| `load.bs.offcanvas.data-api` on `window` | Finds every `.offcanvas.show` already in the markup, creates an instance for it and calls `show()`. |
| `resize.bs.offcanvas` on `window` | Hides any open responsive offcanvas (such as `.offcanvas-lg`) whose computed `position` is no longer `fixed`, meaning the breakpoint now renders it inline. |
| `enableDismissTrigger(Offcanvas)` | Wires up `[data-bs-dismiss="offcanvas"]`. |

---

## Data API (Modal)

```mermaid
flowchart TD
    A[click on data-bs-toggle=modal] --> B[Resolve target via data-bs-target / href]
    B --> C{Trigger is A or AREA?}
    C -- yes --> D[preventDefault]
    C -- no --> E
    D --> E[one show.bs.modal: if not prevented,<br/>register one hidden.bs.modal → refocus trigger if visible]
    E --> F{.modal.show already open?}
    F -- yes --> G[Hide existing modal]
    F -- no --> H
    G --> H[Modal.getOrCreateInstance target .toggle trigger]
```

Only one modal can be open at a time through the Data API: clicking a second trigger closes the first modal. Offcanvas works the same way, but it skips the hide call when the open panel is the target itself.

---

## Usage in AgentsWebUI

**Declarative usage:**

```html
<button data-bs-toggle="modal" data-bs-target="#agentDetails">Details</button>

<div class="modal fade" id="agentDetails" tabindex="-1" aria-hidden="true"
     data-bs-backdrop="static" data-bs-keyboard="false">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-body">…</div>
      <button class="btn-close" data-bs-dismiss="modal"></button>
    </div>
  </div>
</div>

<div class="offcanvas offcanvas-start" id="agentNav" data-bs-scroll="true">…</div>
```

**Programmatic usage** (UMD global from `bootstrap.bundle.js`):

```js
const modal = bootstrap.Modal.getOrCreateInstance('#agentDetails', { backdrop: true });
document.getElementById('agentDetails')
  .addEventListener('hide.bs.modal', e => { if (hasUnsavedChanges()) e.preventDefault(); });
modal.show();
```

If jQuery is loaded and `<body>` does not have `data-bs-no-jquery`, `defineJQueryPlugin` also registers `$(el).modal('show')` and `$(el).offcanvas('toggle')`.

## Notes and caveats

- **One instance per element.** `Data.set` logs an error if a different component is created on the same element.
- **Tabindex.** Set `tabindex="-1"` on `.modal` and `.offcanvas`. Autofocus and the static bounce both call `element.focus()`.
- **Dynamic modals.** A modal element that is not in the document is appended to `document.body` when it is first shown.
- **Transitions.** Transition timing comes from `executeAfterTransition`: a `transitionend` listener, with a fallback timeout of the CSS duration plus 5 ms. See [bootstrap_core_infrastructure](bootstrap_core_infrastructure.md).
- **Nesting modals.** Nesting is not supported. The Data API closes the modal that is already open, and a single document-level `FocusTrap` handles one trap element at a time.
