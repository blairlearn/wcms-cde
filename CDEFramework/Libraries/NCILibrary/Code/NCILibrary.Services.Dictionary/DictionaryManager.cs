using System;
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
                id="12345",
                term="Cancer term",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12346",
                term="Cancer term 2",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12347",
                term="Cancer term 3",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12348",
                term="Cancer term 4",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12349",
                term="Cancer term 5",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12350",
                term="Cancer term 6",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12351",
                term="Cancer term 7",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12352",
                term="Cancer term 8",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12353",
                term="Cancer term 9",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12354",
                term="Cancer term 10",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
        };

        #endregion

        #region private DictionaryTerm[] CancerTermSpanish = new DictionaryTerm[]

        private DictionaryTerm[] CancerTermSpanish = new DictionaryTerm[] {
            new DictionaryTerm() {
                id="12345",
                term="Cáncer término (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12346",
                term="Cáncer término 2 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12347",
                term="Cáncer término 3 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12348",
                term="Cáncer término 4 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12349",
                term="Cáncer término 5 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12350",
                term="Cáncer término 6 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12351",
                term="Cáncer término 7 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12352",
                term="Cáncer término 8 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12353",
                term="Cáncer término 9 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12354",
                term="Cáncer término 10 (en español)",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="es", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="es", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
        };

        #endregion

        #region private DictionaryTerm[] GeneticTermEnglish = new DictionaryTerm[]

        private DictionaryTerm[] GeneticTermEnglish = new DictionaryTerm[] {
            new DictionaryTerm() {
                id="12345",
                term="Genetic term",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12346",
                term="Genetic term 2",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12347",
                term="Genetic term 3",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12348",
                term="Genetic term 4",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12349",
                term="Genetic term 5",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12350",
                term="Genetic term 6",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12351",
                term="Genetic term 7",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12352",
                term="Genetic term 8",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12353",
                term="Genetic term 9",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
            new DictionaryTerm() {
                id="12354",
                term="Genetic term 10",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Related = new RelatedItems() {
                    DrugSummary = new RelatedDrugSummary[] { new RelatedDrugSummary(){ Language = "en", Text="Related Drug Summary", Url="http://www.cancer.gov/publications/dictionaries/cancer-terms?cdrid=45693"} },
                    External = new RelatedExternalLink[] { new RelatedExternalLink(){ Language="en", Text="Great Googly Moogly!", Url="http://www.google.com/"}},
                    Summary = new RelatedSummary[]{ new RelatedSummary(){ Language="en", Text="A Summary", Url="http://www.cancer.gov/types/lung/patient/non-small-cell-lung-treatment-pdq"}},
                    Term = new RelatedTerm[]{new RelatedTerm(){ Dictionary="Term", Termid="12345", Text="A related term"}}
                }
            },
        };

        #endregion

        #region private DictionaryTerm[] DrugTermEnglish = new DictionaryTerm[]

        private DictionaryTerm[] DrugTermEnglish = new DictionaryTerm[] {
            new DictionaryTerm() {
                id="12345",
                term="Drug term",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12346",
                term="Drug term 2",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12347",
                term="Drug term 3",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12348",
                term="Drug term 4",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12349",
                term="Drug term 5",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12350",
                term="Drug term 6",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12351",
                term="Drug term 7",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12352",
                term="Drug term 8",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12353",
                term="Drug term 9",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
                }
            },
            new DictionaryTerm() {
                id="12354",
                term="Drug term 10",
                pronunciation = new Pronunciation{
                    audio = "http://www.cancer.gov/PublishedContent/Media/CDR/media/705333.mp3",
                    key = "KAN-ser"
                },
                dateFirstPublished = "01-01-0001",
                dateLastModified="12-31-9999",
                images = new String[] { "http://www.cancer.gov/images/cdr/live/CDR761781-750.jpg" },
                Definition = new Definition() {
                    Text = "Term Definition",
                    Html = "<p>Term Definition in HTML</p>"
                },
                Aliases = new Alias[]{
                    new Alias(){ name="Alias 1", type="US brand name" },
                    new Alias(){ name="Alias 2", type="Abbreviation" },
                    new Alias(){ name="Alias 3", type="IND code" },
                    new Alias(){ name="Alias 4", type="NSC number" },
                    new Alias(){ name="Alias 5", type="Synonym" },
                    new Alias(){ name="Alias 6", type="Synonym" },
                    new Alias(){ name="Alias 7", type="Synonym" },
                    new Alias(){ name="Alias 8", type="Synonym" },
                    new Alias(){ name="Alias 9", type="Synonym" },
                    new Alias(){ name="Alias 10", type="Synonym" }
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
                        term = null;
                        meta.Audience = string.Empty;
                        meta.Messages = new string[] { "Not a supported language for the term dictionary." };
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
                        term = null;
                        meta.Audience = string.Empty;
                        meta.Messages = new string[] { "Not a supported language for the drug dictionary." };
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
                        term = null;
                        meta.Audience = string.Empty;
                        meta.Messages = new string[] { "Not a supported language for the drug dictionary." };
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
                        results = new DictionaryTerm[] { };
                        meta.Audience = string.Empty;
                        meta.Messages = new string[] { "Not a supported language for the term dictionary." };
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
                        results = new DictionaryTerm[] { };
                        meta.Audience = string.Empty;
                        meta.Messages = new string[] { "Not a supported language for the drug dictionary." };
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
                        results = new DictionaryTerm[] { };
                        meta.Audience = string.Empty;
                        meta.Messages = new string[] { "Not a supported language for the drug dictionary." };
                    }
                    break;
                default:
                    results = new DictionaryTerm[] { };
                    break;
            }

            meta.ResultCount = results.Length;


            srchReturn = new SearchReturn()
            {
                Result = results,
                Meta = meta
            };

            return srchReturn;
        
        }
    }
}
