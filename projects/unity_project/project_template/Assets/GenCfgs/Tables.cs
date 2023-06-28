//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;



namespace cfg
{ 
public partial class Tables
{
    public Levels.TBLevel TBLevel {get; }
    public Hero.TBHeroInfo TBHeroInfo {get; }

    public Tables(System.Func<string, ByteBuf> loader)
    {
        var tables = new System.Collections.Generic.Dictionary<string, object>();
        TBLevel = new Levels.TBLevel(loader("levels_tblevel")); 
        tables.Add("Levels.TBLevel", TBLevel);
        TBHeroInfo = new Hero.TBHeroInfo(loader("hero_tbheroinfo")); 
        tables.Add("Hero.TBHeroInfo", TBHeroInfo);

        PostInit();
        TBLevel.Resolve(tables); 
        TBHeroInfo.Resolve(tables); 
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        TBLevel.TranslateText(translator); 
        TBHeroInfo.TranslateText(translator); 
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}