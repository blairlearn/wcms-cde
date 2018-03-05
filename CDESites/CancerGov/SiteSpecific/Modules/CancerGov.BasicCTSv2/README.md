## Instructions to Add a New CTS Field
1. Update FormFieldsEnum 
  1. Add a new flag. (e.g. IsVAOnly) Always add to the end of the list.
2. Update CTSSearchParams
  1. Add private “_field”  (e.g. _isVAOnly)
  2. Add public property  (e.g. IsVAOnly)
  3. Update GetFieldAsString to handle new field.  Strings & Numbers should output as .ToString(), complex types will need to output custom strings.
3. Update CTSSearchParamFactory
  1. Add Serialize<FIELDNAME> (e.g. SerializeIsVAOnly) method to convert the value to a query param
  2. Add Parse<FIELDNAME> (e.g. ParseIsVAOnly) method to convert from a query param to the value
4. Add Search Param Tests:
  1. Update CTSSearchParamsComparer to account for new field.
    1. If it is a location field, you will need to update its comparer as well.
  2. Add GetFieldAsString tests to CTSearchParams_Test
  3. Add good parsing tests to CTSSearchParamFactory.Test
  4. Add invalid param test to check proper error handling to CTSSearchParamFactory.TestErrors
  5. Add serializations tests to ensure param is turned into query params properly
*ALL TESTS SHOULD ACCOUNT FOR THE POSSIBLE VALUES FOR THE FIELD AS WELL AS DEFAULTS!*
5. Update BasicCTSManager
  1. Update MapSearchParamsToFilterCriteria to add the new field’s value to the search params. (If the field was set…)
6. Add CTS Manager Tests:
  1.Add tests to the best BasicCTSManager.XXXTests class
    1. For Location params, that would be location search tests
    2. For EVS ID search Disease/Treatment add to the appropriate test partial class, for new EVS ID searches (e.g. Biomarkers) create a new partial test class
    3. For simple params, you would update BasicCTSManager.SearchTests
*If this is a Location Filter, updates TrialVelocityTools.GetFilteredLocations to add the filtering for the UI display. (This support Locations: x including y near you & the details view filtering)*
