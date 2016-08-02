SharpSteer2
======================================

**This fork of SharpSteer works with Monogame 3.5, and is compatible VS2015**

SharpSteer is a C# port of OpenSteer. Like OpenSteer, the aim of SharpSteer is to help construct steering behaviors for autonomous characters in games and animation.

Like OpenSteer, SharpSteer provides a XNA-based application which demonstrates predefined steering behaviors. The user can quickly prototype, visualize, annotate and debug new steering behaviors by writing a plug-in for this Demo application.

This fork of SharpSteer includes:

 - Proper use of C# features such as extension methods to make the library easier to use.
 - Changes  to improve code quality/neatness.
 - Total separation of the demo and the library applications.
 - Some behaviours mentioned in the [original paper](http://www.red3d.com/cwr/papers/1999/gdc99steer.html) but never implemented in OpenSteer.
 - Good intentions to have 100% unit test coverage (lots of work needed here).
 - ~~Modified to completely remove XNA dependency from the library~~ Depends on Monogame! :D

### Nuget

SharpSteer2 is [available](https://www.nuget.org/packages/SharpSteer2/) as a nuget package. Package releases use [semantic versioning](http://semver.org/).

### Documentation

The original steering behaviours are documented [here](http://www.red3d.com/cwr/papers/1999/gdc99steer.html)
