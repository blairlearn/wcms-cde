﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary
{
    internal class DictionaryManager
    {
        // These values should come from the GateKeeper backend processing.
        const String AUDIENCE_PATIENT = "Patient";
        const String AUDIENCE_HEALTHPROF = "Health Professional";


        // Hard-coded returns for initial stub-out

        #region private DictionaryTerm[] CancerTermEnglish = new DictionaryTerm[]

        private DictionaryTerm[] CancerTermEnglish = new DictionaryTerm[] {
            new DictionaryTerm() {
                ID="12345",
                Term="Cancer Term",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12346",
                Term="Cancer Term 2",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12347",
                Term="Cancer Term 3",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12348",
                Term="Cancer Term 4",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12349",
                Term="Cancer Term 5",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12350",
                Term="Cancer Term 6",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12351",
                Term="Cancer Term 7",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12352",
                Term="Cancer Term 8",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12353",
                Term="Cancer Term 9",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12354",
                Term="Cancer Term 10",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
        };

        #endregion

        #region private DictionaryTerm[] CancerTermSpanish = new DictionaryTerm[]

        private DictionaryTerm[] CancerTermSpanish = new DictionaryTerm[] {
            new DictionaryTerm() {
                ID="12345",
                Term="Cáncer término (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12346",
                Term="Cáncer término 2 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12347",
                Term="Cáncer término 3 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12348",
                Term="Cáncer término 4 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12349",
                Term="Cáncer término 5 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12350",
                Term="Cáncer término 6 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12351",
                Term="Cáncer término 7 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12352",
                Term="Cáncer término 8 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12353",
                Term="Cáncer término 9 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12354",
                Term="Cáncer término 10 (en español)",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
        };

        #endregion

        #region private DictionaryTerm[] GeneticTermEnglish = new DictionaryTerm[]

        private DictionaryTerm[] GeneticTermEnglish = new DictionaryTerm[] {
            new DictionaryTerm() {
                ID="12345",
                Term="Genetic Term",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12346",
                Term="Genetic Term 2",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12347",
                Term="Genetic Term 3",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12348",
                Term="Genetic Term 4",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12349",
                Term="Genetic Term 5",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12350",
                Term="Genetic Term 6",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12351",
                Term="Genetic Term 7",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12352",
                Term="Genetic Term 8",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12353",
                Term="Genetic Term 9",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
            new DictionaryTerm() {
                ID="12354",
                Term="Genetic Term 10",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related Term"}}
                }
            },
        };

        #endregion

        #region private DictionaryTerm[] DrugTermEnglish = new DictionaryTerm[]

        private DictionaryTerm[] DrugTermEnglish = new DictionaryTerm[] {
            new DictionaryTerm() {
                ID="12345",
                Term="Drug Term",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12346",
                Term="Drug Term 2",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12347",
                Term="Drug Term 3",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12348",
                Term="Drug Term 4",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12349",
                Term="Drug Term 5",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12350",
                Term="Drug Term 6",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12351",
                Term="Drug Term 7",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12352",
                Term="Drug Term 8",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12353",
                Term="Drug Term 9",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
            new DictionaryTerm() {
                ID="12354",
                Term="Drug Term 10",
                Pronunciation = new Pronunciation{
                    Audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    Key = "KAN-ser"
                },
                DateFirstPublished = "01-01-0001",
                DateLastModified="12-31-9999",
                Images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ Name="Alias 1", Type="US brand name" },
                    new Alias(){ Name="Alias 2", Type="Abbreviation" },
                    new Alias(){ Name="Alias 3", Type="IND code" },
                    new Alias(){ Name="Alias 4", Type="NSC number" },
                    new Alias(){ Name="Alias 5", Type="Synonym" },
                    new Alias(){ Name="Alias 6", Type="Synonym" },
                    new Alias(){ Name="Alias 7", Type="Synonym" },
                    new Alias(){ Name="Alias 8", Type="Synonym" },
                    new Alias(){ Name="Alias 9", Type="Synonym" },
                    new Alias(){ Name="Alias 10", Type="Synonym" }
                }
            },
        };

        #endregion


        public TermReturn GetTerm(String termId, DictionaryType dictionary, Language language, String version)
        {
            TermReturn trmReturn;

            DictionaryTerm term;

            TermReturnMeta meta = new TermReturnMeta();
            meta.Language = language.ToString();


            switch (dictionary)
            {
                case DictionaryType.term:
                    if (language == Language.English)
                    {
                        term = CancerTermEnglish[0];
                        meta.Audience = AUDIENCE_PATIENT;
                        meta.Messages = new string[] { "OK" };
                    }
                    else if (language == Language.Spanish)
                    {
                        term = CancerTermSpanish[0];
                        meta.Audience = AUDIENCE_PATIENT;
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                case DictionaryType.drug:
                    if (language == Language.English)
                    {
                        term = DrugTermEnglish[0];
                        meta.Audience = AUDIENCE_HEALTHPROF;
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                case DictionaryType.genetic:
                    if (language == Language.English)
                    {
                        term = GeneticTermEnglish[0];
                        meta.Audience = AUDIENCE_HEALTHPROF;
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                default:
                    term = null;
                    break;
            }

            trmReturn = new TermReturn()
            {
                Term = term,
                Meta = meta
            };

            return trmReturn;
        }

        public SearchReturn Search(String searchText, SearchType searchType, int offset, int maxResults, DictionaryType dictionary, Language language)
        {
            SearchReturn srchReturn;

            // Sanity check for the offset and maxResults
            if (offset < 0) offset = 0;
            if (maxResults < 10) maxResults = 10;

            DictionaryTerm[] results;

            SearchReturnMeta meta = new SearchReturnMeta();
            meta.Language = language.ToString();
            meta.Offset = 0;

            switch (dictionary)
            {
                case DictionaryType.term:
                    if (language == Language.English)
                    {
                        results = CancerTermEnglish;
                        meta.Audience = AUDIENCE_PATIENT;
                        meta.Messages = new string[] { "OK" };
                    }
                    else if (language == Language.Spanish)
                    {
                        results = CancerTermSpanish;
                        meta.Audience = AUDIENCE_PATIENT;
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                case DictionaryType.drug:
                    if (language == Language.English)
                    {
                        results = DrugTermEnglish;
                        meta.Audience = AUDIENCE_HEALTHPROF;
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                case DictionaryType.genetic:
                    if (language == Language.English)
                    {
                        results = GeneticTermEnglish;
                        meta.Audience = AUDIENCE_HEALTHPROF;
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                default:
                    throw new DictionaryValidationException(string.Format("Unknown dictionary type '{0}'.", dictionary));
                    
            }

            meta.ResultCount = results.Length;


            srchReturn = new SearchReturn()
            {
                Result = results,
                Meta = meta
            };

            return srchReturn;
        
        }

        public SuggestReturn SearchSuggest(String searchText, SearchType searchType, DictionaryType dictionary, Language language)
        {
            SuggestReturn srchReturn;

            DictionarySuggestion[] results = new DictionarySuggestion[] { };

            SuggestReturnMeta meta = new SuggestReturnMeta();

            switch (dictionary)
            {
                case DictionaryType.term:
                    if (language == Language.English)
                    {
                        results = Array.ConvertAll(CancerTermEnglish, term => { return new DictionarySuggestion() { ID = term.ID, Term = term.Term }; });
                        meta.Messages = new string[] { "OK" };
                    }
                    else if (language == Language.Spanish)
                    {
                        results = Array.ConvertAll(CancerTermSpanish, term => { return new DictionarySuggestion() { ID = term.ID, Term = term.Term }; });
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                case DictionaryType.drug:
                    if (language == Language.English)
                    {
                        results = Array.ConvertAll(DrugTermEnglish, term => { return new DictionarySuggestion() { ID = term.ID, Term = term.Term }; });
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                case DictionaryType.genetic:
                    if (language == Language.English)
                    {
                        results = Array.ConvertAll(GeneticTermEnglish, term => { return new DictionarySuggestion() { ID = term.ID, Term = term.Term }; });
                        meta.Messages = new string[] { "OK" };
                    }
                    else
                    {
                        throw new UnsupportedLanguageException(language, dictionary);
                    }
                    break;
                default:
                    throw new DictionaryValidationException(string.Format("Unknown dictionary type '{0}'.", dictionary));
            }

            meta.ResultCount = results.Length;


            srchReturn = new SuggestReturn()
            {
                Result = results,
                Meta = meta
            };

            return srchReturn;

        }
    }
}
