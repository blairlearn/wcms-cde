
/**
 * GlossifierProxySkeleton.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis2 version: 1.5.1  Built on : Oct 19, 2009 (10:59:00 EDT)
 * 
 * Fleshed out by:
 * @author holewr
 */
package glossproxy.gov.cancer;

import java.io.InputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.List;

import org.jdom.Document;
import org.jdom.Element;
import org.jdom.input.SAXBuilder;
import org.apache.commons.configuration.*;
       
/**
 *  GlossifierProxySkeleton java skeleton for the axisService
 *  This web service acts as a proxy for the real glossifier service.
 *  This allows clients to connect to the real service even though it is
 *  running on a different domain than the website.
 */
public class GlossifierProxySkeleton{

	private static final String PROPS_FILE_NAME = "glossifierproxy.properties";
	private static PropertiesConfiguration props = null;
	
//TODO: use log file     
    /**
     * Auto generated method signature
     * 
     * @param glossify
     * @return GlossifyResponse
     */
    
	public glossproxy.gov.cancer.GlossifyResponse glossify
              (glossproxy.gov.cancer.Glossify glossify)
    {
		//System.out.println("Entering glossify");		
		String urlString = "http://pdqupdate.cancer.gov/u/glossify"; //default url
		try {
			props = new PropertiesConfiguration(PROPS_FILE_NAME);
			if (props != null)
				urlString = props.getString("webservice.url");
		}
		catch (Exception e) {
			System.out.println("Props file didn't load: " + e.getLocalizedMessage());
		}
		//System.out.println("glossify: urlString = " + urlString);		
        glossproxy.gov.cancer.GlossifyResponse glossResponse = new glossproxy.gov.cancer.GlossifyResponse();
		String fragment = glossify.getFragment();
		byte[] bytes;
		if (fragment != null) {
			Charset cs = Charset.forName("UTF-8");
			bytes = fragment.getBytes(cs);
		}
		else {
			bytes = new byte[]{};
		}
		String fragment2 = new String(bytes);
//System.out.println("[glossify] fragment2: " + fragment2);
		glossproxy.gov.cancer.ArrayOfString dictionaries = glossify.getDictionaries();
		glossproxy.gov.cancer.ArrayOfString languages = glossify.getLanguages();
		
		//set up soap command
		String soapCommand = buildSoapCommand(fragment2, dictionaries, languages);
		//System.out.println("glossify: soapCommand = " + soapCommand);		
		try {
			//set up request
			URL url = new URL(urlString);
			HttpURLConnection conn = (HttpURLConnection)url.openConnection();
			conn.setRequestMethod("POST");
			conn.setDoOutput(true);
			conn.setDoInput(true);
			conn.setRequestProperty("Content-Type", "text/xml");
			conn.setRequestProperty("SOAPAction", "cips.nci.nih.gov/cdr/glossify");
			int soapLen = soapCommand.length();
			conn.setRequestProperty("Content-Length", String.valueOf(soapLen));
			 
			//make connection and get results
			conn.connect();
			//System.out.println("glossify: connected");		
			OutputStreamWriter out = new OutputStreamWriter(conn.getOutputStream());
			//System.out.println("glossify: got outputStreamWriter");		
			out.write(soapCommand, 0, soapLen);
			out.flush();
			InputStream read = conn.getInputStream();
			//System.out.println("glossify: got inputStream");		
			//parse the xml
			SAXBuilder saxBuilder = new SAXBuilder();
			Document doc = saxBuilder.build(read);
			Element env = doc.getRootElement();
			Element body = null;
			Element response = null;
			Element result = null;
			if (env != null) {
				System.out.println("got element: " + env.getName());
				List<?> bList = env.getChildren();
				if (bList != null) {
				//	System.out.println("got list");
					body = (Element)bList.get(0);
				}
				if (body != null) {
			    //    System.out.println("got element: " + body.getName());
					List<?> respList = body.getChildren();
					if (respList != null) {
						System.out.println("got responselist");
						response = (Element)respList.get(0);
					}
				}
				if (response != null) {
			    //    System.out.println("got element: " + response.getName());
					List<?> restList = response.getChildren();
					if (restList != null) {
						System.out.println("got resultlist");
						result = (Element)restList.get(0);
					}
				}
				if (result != null) {
				//	System.out.println("got element: " + result.getName());
					List<?> termList = result.getChildren();
					if (termList != null) {
						//get array of terms data
						System.out.println("got termList, size= " + termList.size());
						glossproxy.gov.cancer.Term[] termArray = buildTermsArray(termList);
						//put the terms data into the glossifyResponse
		                glossproxy.gov.cancer.ArrayOfTerm arrayOfTerm = new glossproxy.gov.cancer.ArrayOfTerm();
		                arrayOfTerm.setTerm(termArray);
		                glossResponse.setGlossifyResult(arrayOfTerm);
					}
				 }
			 }
			 read.close();
			 conn.disconnect();
		 }
		 catch (Exception e) {
			 System.out.println("Exception with web service: " + e.getLocalizedMessage());
		 }
		 //System.out.println("glossify: returning");		
         return glossResponse;
    }

	/**
	 * Create the SOAP command
	 * @param fragment - the text to be glossified
	 * @param dictionaries - the dictionaries to use
	 * @param languages - languages to use
	 * @return String soapCommand
	 */
	private String buildSoapCommand(String fragment,
									glossproxy.gov.cancer.ArrayOfString dictionaries,
									glossproxy.gov.cancer.ArrayOfString languages) {
		//set up first part of soap command with fragment
		String soapCommand = "<?xml version=\"1.0\"?>" +
		"<soapenv:Envelope " +
		"xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' " +
		"xmlns:m='cips.nci.nih.gov/cdr'> " +
		"<soapenv:Header/>" +
		"<soapenv:Body>" +
			"<m:glossify>" +
				"<m:fragment><![CDATA[" + fragment + "]]></m:fragment>" +
				"<m:dictionaries>";
		//add dictionaries
		for (String theString : dictionaries.getString()) {
			soapCommand = soapCommand.concat("<m:string>" + theString + "</m:string>");
		}
		//middle part
		soapCommand = soapCommand.concat(
				"</m:dictionaries>" +
				"<m:languages>");
		//add languages
		for (String theString : languages.getString()) {
			soapCommand = soapCommand.concat("<m:string>" + theString + "</m:string>");
		}
		//end of command
		soapCommand = soapCommand.concat(
				"</m:languages>" +
			"</m:glossify>" +
		"</soapenv:Body>" +
		"</soapenv:Envelope>");
   	 	System.out.print(soapCommand);
   	 	return soapCommand;
	}
	
	/**
	 * Build an array of term data
	 * @param termList - array of term Elements
	 * @return term array
	 */
	private glossproxy.gov.cancer.Term[] buildTermsArray(List<?> termList) {
		glossproxy.gov.cancer.Term[] termArray = new glossproxy.gov.cancer.Term[termList.size()];
		int i = 0;
    	 for (Object resultObj : termList) {
//    		 System.out.println("term # " + i);
    		 Element xterm = (Element)resultObj;
    		 String start = "0";
    		 String len = "0";
    		 String docId = "";
    		 String dictionary = "";
    		 String language = "";
    		 String first = "true";
    		 List<?> childList = xterm.getChildren();
    		 for (Object childObj : childList) {
    			 Element child = (Element) childObj;
    			 if (child.getName().equalsIgnoreCase("start")) {
    				 start = child.getText();
//            		 System.out.println("start = " + start);
    			 }
    			 if (child.getName().equalsIgnoreCase("length")) {
    				 len = child.getText();
//            		 System.out.println("length = " + len);
    			 }
    			 if (child.getName().equalsIgnoreCase("docId")) {
    				 docId = child.getText();
//            		 System.out.println("docId = " + docId);
    			 }
    			 if (child.getName().equalsIgnoreCase("dictionary")) {
    				 dictionary = child.getText();
//            		 System.out.println("dictionary = " + dictionary);
    			 }
    			 if (child.getName().equalsIgnoreCase("language")) {
    				 language = child.getText();
//            		 System.out.println("language = " + language);
    			 }
    			 if (child.getName().equalsIgnoreCase("firstOccurrence")) {
    				 first = child.getText();
//            		 System.out.println("first = " + first);
    			 }
    		 }
    		 glossproxy.gov.cancer.Term term = new glossproxy.gov.cancer.Term();
    		 term.setStart(Integer.parseInt(start));
    		 term.setLength(Integer.parseInt(len));
    		 term.setDocId(docId);
    		 term.setDictionary(dictionary);
    		 term.setLanguage(language);
    		 term.setFirstOccurrence(Boolean.parseBoolean(first));
    		 termArray[i] = term;
    		 i++;
    	 }
    	 return termArray;
	}
}
    