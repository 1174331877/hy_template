//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Hero
{
   
public partial class TBHeroInfo
{
    private readonly Dictionary<int, Hero.HeroInfo> _dataMap;
    private readonly List<Hero.HeroInfo> _dataList;
    
    public TBHeroInfo(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Hero.HeroInfo>();
        _dataList = new List<Hero.HeroInfo>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Hero.HeroInfo _v;
            _v = Hero.HeroInfo.DeserializeHeroInfo(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Hero.HeroInfo> DataMap => _dataMap;
    public List<Hero.HeroInfo> DataList => _dataList;

    public Hero.HeroInfo GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Hero.HeroInfo Get(int key) => _dataMap[key];
    public Hero.HeroInfo this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}