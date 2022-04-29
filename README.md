# FastSLQL

This DBMS is based on SLQL: https://github.com/ZikkeyLS/SLQL

Network version is under development: https://github.com/ZikkeyLS/FastSLQL-NetworkAPI

### Main features
* **Type independent** - you regulate what types should be written;
* **Really fast** - can write above 200-300+ elements/sec on average computer;
* **Simple to use** - represent easy structure and built-in constructors, etc.;

## How to use

  **Initialize FSLQL**

    FSLQL.Load(string dbDirectory = "")
    
  Represent loader of FastSLQL. Load all db's or creates folder.
  
  **Send command**

    string[] FSLQL.SendCommand(string command)
    
  Send SLQL command and return result. (Read SLQL documentation to get more info)
  
  0 index - status. (except GET) GET command - all indexes = elements
  
  **Settings**

    FSLQLSettings.SetLogging(bool result)
    
  Sets logging level of result of SendCommand. If true - status, if false - empty string (except GET command)
  
  **Constructors**

    CommandConstructor.TIL
    CommandConstructor.EVL
    ElementConstructor
    StructureConstructor
 
  Used to simplify/shortcut creation of command.
  
  **Parsing**

    bool ParameterParse.IsElement(string value)
    string[] ParameterParse.ParseElement(string element)
    {Type} ParameterParse.Parse{Type}(string parameter)
    
  ParseElement - returns splitted parameters of element
  Parse{Type} - returns parameter of {Type} (string, bool, int, float, array(unparsed strings array), etc.)
