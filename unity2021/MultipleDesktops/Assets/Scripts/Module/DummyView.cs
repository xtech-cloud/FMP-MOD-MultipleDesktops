
using System;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.MultipleDesktops.LIB.Bridge;
using XTC.FMP.MOD.MultipleDesktops.LIB.MVCS;
using XTC.FMP.LIB.MVCS;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XTC.FMP.MOD.MultipleDesktops.LIB.Unity
{
    /// <summary>
    /// 虚拟视图，用于处理消息订阅
    /// </summary>
    public class DummyView : DummyViewBase
    {
        public DummyView(string _uid) : base(_uid)
        {
        }

        protected override void setup()
        {
            base.setup();
            addSubscriber(MySubject.Switch, handleSwitch);
        }

        private void handleSwitch(Model.Status _status, object _data)
        {
            getLogger().Debug("handle {0} with data:{1}", MySubject.Switch, JsonConvert.SerializeObject(_data));
            string uid = "";
            string animation = "";
            float duration = 1f;
            try
            {
                Dictionary<string, object> data = _data as Dictionary<string, object>;
                uid = data["uid"] as string;
                animation = data["animation"] as string;
                duration = (float)data["duration"];
            }
            catch (Exception e)
            {
                getLogger().Exception(e);
            }

            MyInstance instance = null;
            runtime.instances.TryGetValue(uid, out instance);
            if (null == instance)
            {
                getLogger().Error("instance:{0} not found", uid);
                return;
            }

            instance.DoSwitch(animation, duration);
        }
    }
}

