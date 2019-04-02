# PseudoLocalizer

> Pseudolocalization (or pseudo-localization) is a software testing method used for testing internationalization aspects of software. Instead of translating the text of the software into a foreign language, as in the process of localization, the textual elements of an application are replaced with an altered version of the original language.

IBM suggests that on average, we should expect any English string to inflate by 30% when translating in another language. Using pseudo localization is a way to simulate translation of English UI strings, without waiting for, or going to the effort of doing real translation. Think of it as a fake translation that remains readable to an English speaking developer, and allows them to test for translation related expansion.