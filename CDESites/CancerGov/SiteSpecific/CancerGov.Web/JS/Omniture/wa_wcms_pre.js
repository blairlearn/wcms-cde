//TODO:
// - Update values for prod suites
// - Add hiers to correct suites
// - Update check for Spanish values    
var wa_production_report_suite = 'ncidev';
var wa_dev_report_suite = 'ncidev';
var wa_is_production_report_suite = false;
var wa_hier1 = '';
var wa_lang = '';
var page_URL = document.URL;

/*
 * Get language-all report suites baseed on URL
 */
if (page_URL.indexOf('cancer.gov/espanol') != -1) {
	wa_production_report_suite +=  ',ncispanish-all-dev';
	wa_dev_report_suite += ',ncispanish-all-dev';
}
else {
	wa_production_report_suite +=  ',ncienglish-all-dev';
	wa_dev_report_suite += ',ncienglish-all-dev';
}

/*
 * Get default report suites based on URL path
 */
if (page_URL.indexOf('/publications/dictionaries/cancer-terms') != -1) {
	wa_production_report_suite +=  ',ncincidictionary-dev';
	wa_dev_report_suite += ',ncincidictionary-dev';
}
else if (page_URL.indexOf('/publications/dictionaries/cancer-drug') != -1) {
	wa_production_report_suite +=  ',ncidrugdictionary-dev';
	wa_dev_report_suite += ',ncidrugdictionary-dev';
}
else if (page_URL.indexOf('/news-events') != -1) {
	wa_production_report_suite +=  ',ncinews-dev';
	wa_dev_report_suite += ',ncinews-dev';
}
else if (page_URL.indexOf('/research') != -1 ||
	page_URL.indexOf('/grants-training') != -1) {
	wa_production_report_suite +=  ',nciresearch-dev';
	wa_dev_report_suite += ',nciresearch-dev';
}
else if (page_URL.indexOf('/about-nci') != -1) {
	wa_production_report_suite +=  ',nciabout-dev';
	wa_dev_report_suite += ',nciabout-dev';
}
else if (page_URL.indexOf('/about-cancer/treatment/clinical-trials') != -1) {
	wa_production_report_suite +=  ',nciclinicaltrials-dev';
	wa_dev_report_suite += ',nciclinicaltrials-dev';
}
else if (page_URL.indexOf('/types') != -1 ||
	page_URL.indexOf('/about-cancer') != -1) {
	wa_production_report_suite +=  ',ncicancertopics-dev';
	wa_dev_report_suite += ',ncicancertopics-dev';
	wa_hier1 = '';
}
else if (page_URL.indexOf('/espanol/tipos') != -1) {
	wa_production_report_suite +=  ',ncitiposdecancer-dev';
	wa_dev_report_suite += ',ncitiposdecancer-dev';
}
else if (page_URL.indexOf('/espanol/noticias') != -1) {
	wa_production_report_suite +=  ',ncinoticias-dev';
	wa_dev_report_suite += ',ncinoticias-dev';
}
else if (page_URL.indexOf('/espanol/instituto') != -1) {
	wa_production_report_suite +=  ',ncinuestroinstituto-dev';
	wa_dev_report_suite += ',ncinuestroinstituto-dev';
}
else if (page_URL.indexOf('/espanol/cancer') != -1) {
	wa_production_report_suite +=  ',ncielcancer-dev';
	wa_dev_report_suite += ',ncielcancer-dev';
}
else
{
	wa_dev_report_suite += '';
}

/*
 * Check hostname for Prod url. If matches, use the production suites
 */
if (page_URL.indexOf('www.cancer.gov') != -1) {
	wa_is_production_report_suite = true;
}

if (wa_is_production_report_suite) {
	s_account += wa_production_report_suite;
}
else {
	s_account += wa_dev_report_suite;
}
var pageNameOverride = location.hostname.toLowerCase() + location.pathname.toLowerCase();
