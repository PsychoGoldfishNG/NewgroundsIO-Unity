### Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!--
# Changelog

### [0.0.0] - unreleased

#### Added
- (new functionality)

### Changed)
- changes in existing functionality

### Deprecated
- soon to be removed functionality

### Removed
- removed functionality

### Fixed
- bug fixes

### Security
- security vulnerabilities
-->

<a name="latest"></a>
## [2.0.2] – 2024-01-30

### Fixed
- Fixed the missing method error encountered when building while [managed stripping level](https://docs.unity3d.com/Manual/ManagedCodeStripping.html) 
was set to the default *Low*.  
Now the runtime assembly should be preserved.

### Sample
- Added a *Back* button to the second scene to be able to return to the main menu and reload a save slot without reload.
- Fixed a couple of invalid scene references, causing some buttons not showing up or initializing properly.