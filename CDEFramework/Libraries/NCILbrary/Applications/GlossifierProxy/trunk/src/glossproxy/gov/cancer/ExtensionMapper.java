
/**
 * ExtensionMapper.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis2 version: 1.5.1  Built on : Oct 19, 2009 (10:59:34 EDT)
 */

            package glossproxy.gov.cancer;
            /**
            *  ExtensionMapper class
            */
        
        public  class ExtensionMapper{

          public static java.lang.Object getTypeObject(java.lang.String namespaceURI,
                                                       java.lang.String typeName,
                                                       javax.xml.stream.XMLStreamReader reader) throws java.lang.Exception{

              
                  if (
                  "cancer.gov/glossproxy".equals(namespaceURI) &&
                  "ArrayOfString".equals(typeName)){
                   
                            return  glossproxy.gov.cancer.ArrayOfString.Factory.parse(reader);
                        

                  }

              
                  if (
                  "cancer.gov/glossproxy".equals(namespaceURI) &&
                  "Term".equals(typeName)){
                   
                            return  glossproxy.gov.cancer.Term.Factory.parse(reader);
                        

                  }

              
                  if (
                  "cancer.gov/glossproxy".equals(namespaceURI) &&
                  "ArrayOfTerm".equals(typeName)){
                   
                            return  glossproxy.gov.cancer.ArrayOfTerm.Factory.parse(reader);
                        

                  }

              
             throw new org.apache.axis2.databinding.ADBException("Unsupported type " + namespaceURI + " " + typeName);
          }

        }
    