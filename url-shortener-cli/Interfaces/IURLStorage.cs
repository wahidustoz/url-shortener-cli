using System.Collections.Generic;

interface IURLStorage  
{  
    Dictionary<string, string> LoadURLMapping();  
    void SaveURLMapping(Dictionary<string, string> urlMapping);  
}
