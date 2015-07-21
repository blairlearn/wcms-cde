var wa_production_report_suite = 'ncimobile';
var wa_dev_report_suite = 'ncimobile-dev';
var wa_is_production_report_suite = false;
var wa_hier1 = '';
var wa_hier2 = '';
var wa_lang = '';
var page_URL = document.URL;

/*
 * Check hostname for Prod url. If matches, use the production suites
 */
if (page_URL.indexOf('m.cancer.gov') != -1) {
	wa_is_production_report_suite = true;
}

if (wa_is_production_report_suite) {
	s_account += wa_production_report_suite;
}
else {
	s_account += wa_dev_report_suite;
}
var pageNameOverride = location.hostname.toLowerCase() + location.pathname.toLowerCase();
