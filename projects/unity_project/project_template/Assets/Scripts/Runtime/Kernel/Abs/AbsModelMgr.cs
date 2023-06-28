using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LT_Kernel
{
    public abstract class AbsModelMgr : AbsMgr, IClear
    {
        private Dictionary<Type, AbsModel> models = new Dictionary<Type, AbsModel>();

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit();
            RegisterModels();
        }

        public override void OnRemove()
        {
            for (int i = 0; i < models.Keys.Count; i++)
            {
                var item = models.ElementAt(i);
                item.Value?.OnRemove();
            }
            base.OnRemove();
        }

        public abstract void RegisterModels();

        public T RegisterModel<T>() where T : AbsModel
        {
            T ret = default;
            Type modelType = typeof(T);
            if (!models.ContainsKey(modelType))
            {
                ret = Activator.CreateInstance<T>();
                models.Add(modelType, ret);
                ret.OnInit();
                ret.OnInitFinish();
            }
            return ret;
        }

        public T RetriveModel<T>() where T : AbsModel
        {
            Type modelType = typeof(T);
            models.TryGetValue(modelType, out var model);
            return model as T;
        }

        public void UnRegisterModel<T>() where T : AbsModel
        {
            Type modelType = typeof(T);
            if (models.TryGetValue(modelType, out var model))
            {
                model.OnRemove();
                models.Remove(modelType);
            }
        }

        public void OnClear()
        {
            for (int i = 0; i < models.Keys.Count; i++)
            {
                var item = models.ElementAt(i);
                item.Value?.OnClear();
            }
        }
    }
}
