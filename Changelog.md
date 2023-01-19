# Changelog
Keeps all of the changes done in each version. The version numbering is defined as **major_change**.**minor_change**.**bug_fix**
- **Major changes** constitutes a large change in architecture of the mod.
- **Minor changes** is every new feature added without changing the architecture of the mod.
- **Bug fixes** are fixes that might have occured from last update.

## Update 3.1.0
- Separate Universum code from this mod
- Move all hardcoded strings to XML for translators

## Update 3.0.1
- Fix issue with saved settings not being null safe on the first load
- Fix issue with Regrowth: Core

## Update 3.0.0
- Add a light version of the oxygen/decompression system utilizing the EVA tag, as Save Our Ship has to make it compatible with other apparel mods
- Rework the temperature system to behave as outer space
- Change UI to show the status of Vacuum, Airtight, and Leaking instead of Outdoors, Indoors, and Unroofed in space
- Identify vacuum terrain as a leak in the room and apply decompression/suffocation
- Initial work on my outer space framework, Universum, is added to this release built-in to RimNauts. Universum's features include automatic caching for performance and customizability, as every module added through this will be toggleable in the mod settings.