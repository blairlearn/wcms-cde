var wa_production_report_suite = 'ncicssi-strategicscientificinitiatives';
var wa_dev_report_suite = 'ncicssi-strategicscientificinitiativ-dev';
var wa_is_production_report_suite = false;
var wa_hier1 = '';
var wa_lang = '';
var page_URL = document.URL;

/*
 * Check hostname for Prod url. If matches, use the production suites
 */
if (page_URL.indexOf('cancergenome.nih.gov') != -1 ||
	page_URL.indexOf('tcga.cancer.gov') != -1)
{
	wa_is_production_report_suite = true;
}

if (wa_is_production_report_suite) {
	s_account += wa_production_report_suite;
}
else {
	s_account += wa_dev_report_suite;
}
var pageNameOverride = location.hostname.toLowerCase() + location.pathname.toLowerCase();
