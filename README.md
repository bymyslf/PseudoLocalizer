# PseudoLocalizer [![Build Status][travis build]][travis project] [![Build Status][appveyor build]][appveyor project] [![NuGet][nuget badge]][nuget package]

> Pseudolocalization (or pseudo-localization) is a software testing method used for testing internationalization aspects of software. Instead of translating the text of the software into a foreign language, as in the process of localization, the textual elements of an application are replaced with an altered version of the original language.

## Quick start

```C#
using static PseudoLocalizer.PseudoLocalizer;
```

```C#
PseudoLocalize("text");
```
or 

```C#
using PseudoLocalizer;
```

```C#
"text".PseudoLocalize();
```
## Why?

IBM suggests that on average, we should expect any English string to inflate by 30% when translating in another language. Using pseudo localization is a way to simulate translation of English UI strings, without waiting for, or going to the effort of doing real translation. Think of it as a fake translation that remains readable to an English speaking developer, and allows them to test for translation related expansion.

## How it works?

Here are the various elements of the transformation:

* **Start and end markers**
  * All strings are encapsulated in [ ]
* **Transformation of characters applying accents**
  * Formatting placeholders and HTML tags are ignored
* **Padding text**
  * Simulates translation induced expansion by doubling all vowels

## License
MIT

## Other Resources

* [What is Pseudo-Localization?](https://www.globalizationpartners.com/2015/04/17/what-is-pseudo-localization/)
* [Pseudolocalization](https://en.wikipedia.org/wiki/Pseudolocalization)
* [Pseudo Localization @ Netflix](https://medium.com/netflix-techblog/pseudo-localization-netflix-12fff76fbcbe)
* [Pseudolocalization to Catch i18n Errors Early](https://opensource.googleblog.com/2011/06/pseudolocalization-to-catch-i18n-errors.html)


[travis build]: https://travis-ci.org/bymyslf/PseudoLocalizer.svg?branch=master&logo=travis
[travis project]: https://travis-ci.org/bymyslf/PseudoLocalizer
[appveyor build]: https://ci.appveyor.com/api/projects/status/qcol8951b78sraxs?svg=true
[appveyor project]: https://ci.appveyor.com/project/bymyslf/pseudolocalizer
[nuget badge]: https://img.shields.io/nuget/v/PseudoLocalizer.svg
[nuget package]: https://www.nuget.org/packages/PseudoLocalizer
