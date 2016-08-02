SharpSteer2
======================================

**This fork of SharpSteer works with Monogame 3.5, and is compatible VS2015**

SharpSteer is a C# port of OpenSteer. Like OpenSteer, the aim of SharpSteer is to help construct steering behaviors for autonomous characters in games and animation. 

The repo contains a handful of demos (which in the source code they are called "Plugins"). It is helpful to know these keyboard shortcuts (which is documented only in the source code) to check them out:
 - `TAB` - switch between different demos.
 - `SPACE` - play/pause the demo.
 - `R` - reset the current demo.
 - `A` - turn on and off the annotations.
 - `S` - select next vehicle.
 - `F` - select next preset framerate.
 - `C` - change camera mode.

![alt text](https://lh3.googleusercontent.com/-Q4fkSJ97VWg/V6AcFZe6OoI/AAAAAAAAL9M/fV7_6yNDIsUYujN_BiOCQ8rKOmc7mU-GgCCo/s720/SharpSteer2-Demo2.png "SharpSteer2 demo")

Like OpenSteer, SharpSteer provides a XNA-based application which demonstrates predefined steering behaviors. The user can quickly prototype, visualize, annotate and debug new steering behaviors by writing a plug-in for this Demo application.

This fork of SharpSteer includes:

 - Proper use of C# features such as extension methods to make the library easier to use.
 - Changes  to improve code quality/neatness.
 - Total separation of the demo and the library applications.
 - Some behaviours mentioned in the [original paper](http://www.red3d.com/cwr/papers/1999/gdc99steer.html) but never implemented in OpenSteer.
 - Good intentions to have 100% unit test coverage (lots of work needed here).
 - ~~Modified to completely remove XNA dependency from the library~~ Depends on Monogame! :D

### Nuget

SharpSteer2 is [available](https://www.nuget.org/packages/SharpSteer2/) as a nuget package. Package releases use [semantic versioning](http://semver.org/). **HOWEVER, that version of SharpSteer2 (some .NET incompatibility) is not compatible with monogame.**

### How to Use my Monogame Game?

Easiest way I can think of is copy the contents of the SharpSteer2 folder to your solution folder, add SharpSteer2.csproj to your solution, then in your monogame project, right click "References" > "Add Reference" and add the SharpSteer2 project to your monogame project.

### Documentation

The original steering behaviours are documented [here](http://www.red3d.com/cwr/papers/1999/gdc99steer.html)
