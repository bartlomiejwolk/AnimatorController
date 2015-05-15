# AnimatorController

*AnimatorController* extension for Unity. Allows update Unity *Animator* component
fields with property values of other components.

Licensed under MIT license. See LICENSE file in the project root folder.   
Extension versions below 1.0.0 are considered to be pre/alpha and may not work properly.

![AnimatorController](/Resources/cover_screenshot.png?raw=true)

## Features

* Select source component properties with dropdown.
* Update Animator params of type int, float, boolean and trigger.
* Update multiple parameters at once.
* Synchronize trigger animator property with *OnTriggerEnter* and *OnTriggerExit* messages.

## Resources

Nothing here.

## Quick Start

1. Clone repository into the *Assets* folder.
2. Select game object in the hierarchy window and from the *Component* menu
   select *AnimatorController* to add component to the selected game object.
3. Select Animator component to be updated.
4. Set the *Source Type* dropdown to *Property*, type animator parameter name
   and select *Trigger* checkbox if it's a trigger.
5. Drag component which property value will update the animator parameter into
   the *Source* field.
6. From *Property* dropdown select property that will be used to update
   the animator parameter.
7. When you enter play mode, the specified animator property will be updated
   in each frame to the current value of the selected property.

## Help

Just create an issue and I'll do my best to help.

## Contributions

Pull requests, ideas, questions or any feedback at all are welcome.

## Versioning

Example: `v0.2.3f1`

- `0` Introduces breaking changes.
- `2` Major release. Adds new features.
- `3` Minor release. Bug fixes and refactoring.
- `f1` Quick fix.