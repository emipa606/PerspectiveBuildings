# GitHub Copilot Instructions for Perspective: Buildings (Continued)

## Mod Overview and Purpose

**Perspective: Buildings (Continued)** is a RimWorld mod that adds a gizmo to most buildings, allowing players to adjust their positions. This mod enhances the visual perspective of buildings by enabling users to nudge them down, tuck them behind walls, or mirror non-rotatable buildings. It was originally created to enhance the new Fencepost Lights added to Simple Utilities: Walls.

## Key Features and Systems

1. **Building Position Adjustment**: Players can adjust the position of most buildings, giving them better visual placement.
2. **Building Mirroring**: Non-rotatable buildings can be mirrored to create symmetrical or aesthetically pleasing layouts.
3. **Standard Offsets**: Buildings come with a list of standard offsets that the adjuster button iterates through. Modders can customize these offsets for specific buildings.
4. **Graphical Enhancement**: The mod focuses on visual improvements without altering gameplay mechanics.

## Coding Patterns and Conventions

- **Modding Framework**: The mod follows the RimWorld modding framework, utilizing Harmony for patching and XML for defining mod behavior.
- **Harmony Patching**: Harmony is extensively used to inject code into existing game methods, allowing for functionality enhancements without modifying core game files.
- **XML Integration**: XML files are used to define patches and dependencies, ensuring mod compatibility and functionality.

## XML Integration

**Perspective: Buildings (Continued)** uses XML files to define patches and dependencies. Key XML files include:

- **patch.owlchemist.perspectivebuildings.xml**: Contains Harmony patches that modify game behavior.
- **About.xml**: Contains metadata about the mod, such as dependencies and author information.

## Harmony Patching

The mod uses Harmony extensively to patch existing game methods. Key patches include:

- **CeilingUtilities_Graphic_FlickerMulti_DrawWorker**: Adjusts the drawing behavior of certain graphics.
- **PawnRenderer_GetBodyPos**: Modifies the rendering position of pawns.

## Suggestions for Copilot

### General Tips

- **Understand Mod Dependencies**: Ensure you have all required dependencies installed, such as Harmony.
- **Follow Coding Conventions**: Adhere to the mod's coding patterns and conventions for consistency.
- **Test Changes**: Always test your changes in a controlled environment to ensure they don't introduce bugs.

### XML Patches

- **Define Patches Correctly**: Ensure XML patches are correctly defined and point to the correct methods.
- **Test XML Changes**: After making changes to XML files, test the mod to verify that patches are applied correctly.

### Harmony Patches

- **Understand Target Methods**: Ensure you understand the methods you are patching to avoid unintended side effects.
- **Use Prefix/Postfix Appropriately**: Use `Prefix` for modifying method behavior before execution and `Postfix` for after execution.
- **Debugging**: Use logging and debugging tools to troubleshoot issues with Harmony patches.

### Modding Best Practices

- **Documentation**: Refer to the mod's documentation for customizing offsets and other advanced features.
- **Compatibility**: Be aware of compatibility issues with other mods, especially those that modify similar systems.
- **Community Support**: Utilize community resources, such as Discord channels, for support and troubleshooting.

## Conclusion

**Perspective: Buildings (Continued)** is a powerful tool for enhancing the visual perspective of buildings in RimWorld. By following the coding patterns and conventions, utilizing Harmony patching, and integrating XML files correctly, you can effectively contribute to or extend the functionality of this mod.

For more detailed information, please refer to the mod's wiki and documentation. Happy modding!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

