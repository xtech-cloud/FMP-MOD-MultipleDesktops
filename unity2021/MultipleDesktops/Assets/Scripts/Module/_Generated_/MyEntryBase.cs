
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.83.0.  DO NOT EDIT!
//*************************************************************************************

using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using UnityEngine;
using Newtonsoft.Json;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.MultipleDesktops.LIB.Bridge;
using XTC.FMP.MOD.MultipleDesktops.LIB.MVCS;

namespace XTC.FMP.MOD.MultipleDesktops.LIB.Unity
{
    /// <summary>
    /// 模块入口基类
    /// </summary>
    public class MyEntryBase : Entry
    {
        /// <summary>
        /// 模块名
        /// </summary>
        public const string ModuleName = "XTC_MultipleDesktops";

        protected MonoBehaviour mono_;
        protected MyConfig config_;
        protected MyCatalog catalog_;
        protected LibMVCS.Logger logger_;
        protected Dictionary<string, LibMVCS.Any> settings_;
        protected MyRuntime runtime_ { get; set; }
        protected DummyView viewDummy_ { get; set; }
        protected DummyModel modelDummy_ { get; set; }

        public DummyModel getDummyModel()
        {
            return modelDummy_;
        }

        public DummyView getDummyView()
        {
            return viewDummy_;
        }

        public Options NewOptions()
        {
            return new Options();
        }

        /// <summary>
        /// 手动注入
        /// </summary>
        /// <remarks>
        /// 此函数会在宿主程序加载模块后调用，并注入需要的实现
        /// </remarks>
        /// <param name="_mono"></param>
        /// <param name="_logger"></param>
        /// <param name="_config"></param>
        /// <param name="_catalog"></param>
        /// <param name="_settings"></param>
        public void UniInject(MonoBehaviour _mono, Options _options, LibMVCS.Logger _logger, LibMVCS.Config _config, Dictionary<string, LibMVCS.Any> _settings)
        {
            mono_ = _mono;
            settings_ = _settings;
            logger_ = _logger;

            string xml = _config.getField(ModuleName+".xml").AsString();
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            MemoryStream ms = new MemoryStream(bytes);
            var xs = new XmlSerializer(typeof(MyConfig));
            config_ = xs.Deserialize(ms) as MyConfig;

            string json = _config.getField(ModuleName + ".json").AsString();
            catalog_ = JsonConvert.DeserializeObject<MyCatalog>(json);

            if (!string.IsNullOrEmpty(config_.grpc.address))
            {
                try
                {
                    var channel = GrpcChannel.ForAddress(config_.grpc.address, new GrpcChannelOptions
                    {
                        HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                    });
                    _options.setChannel(channel);
                }
                catch (System.Exception ex)
                {
                    logger_.Exception(ex);
                }
            }

            runtime_ = new MyRuntime(mono_, config_, catalog_, settings_, logger_, this);
        }

        /// <summary>
        /// 注册虚拟视图和数据
        /// </summary>
        public void RegisterDummy()
        {
            viewDummy_ = new DummyView(DummyView.NAME);
            framework_.getStaticPipe().RegisterView(viewDummy_);
            viewDummy_.runtime = runtime_;

            modelDummy_ = new DummyModel(DummyModel.NAME);
            framework_.getStaticPipe().RegisterModel(modelDummy_);
            modelDummy_.runtime = runtime_;
        }

        /// <summary>
        /// 注销虚拟视图和数据
        /// </summary>
        public void CancelDummy()
        {
            framework_.getStaticPipe().CancelView(viewDummy_);
            framework_.getStaticPipe().CancelModel(modelDummy_);
        }

        public virtual void Preload(System.Action<int> _onProgress, System.Action<string> _onFinish)
        {        
            logger_.Info("ready to preload {0}", ModuleName);
            mono_.StartCoroutine(loadUAB((_root) =>
            {
                processRoot(_root);
                runtime_.Preload(_onProgress, () =>
                {
                    createInstances(() =>
                    {
                        publishPreloadSubjects();
                        _onFinish(ModuleName);
                    });
                });
            }, (_err) =>
            {
                logger_.Error(_err.getMessage());
            }));
        }

        /// <summary>
        /// 处理实例化的根对象
        /// </summary>
        /// <param name="_root">根对象</param>
        protected void processRoot(GameObject _root)
        {
            if (null == _root)
            {
                logger_.Error("parse [ExportRoot] failed, it is null");
                return;
            }
            logger_.Debug("ready to parse {0}.{1}", ModuleName, _root.name);

            // 从设置中获取主画布参数
            LibMVCS.Any anyMainCanvas;
            if (!settings_.TryGetValue("canvas.main", out anyMainCanvas))
            {
                logger_.Error("the canvas.main not found in settings");
                return;
            }
            Transform mainCanvas = anyMainCanvas.AsObject() as Transform;
            if (null == mainCanvas)
            {
                logger_.Error("the mainCanvas is null");
                return;
            }

            // 从设置中获取主世界参数
            LibMVCS.Any anyMainWorld;
            if (!settings_.TryGetValue("world.main", out anyMainWorld))
            {
                logger_.Error("the world.main not found in settings");
                return;
            }
            Transform mainWorld = anyMainWorld.AsObject() as Transform;
            if (null == mainWorld)
            {
                logger_.Error("the mainWorld is null");
                return;
            }

            // 查找UI的挂载槽
            Transform slotUi = mainCanvas.Find(config_.ui.slot);
            if (null == slotUi)
            {
                logger_.Error("the slotUi is null");
                return;
            }

            // 查找World的挂载槽
            Transform slotWorld = mainWorld.Find(config_.world.slot);
            if (null == slotWorld)
            {
                logger_.Error("the slotWorld is null");
                return;
            }

            runtime_.ProcessRoot(_root, slotUi, slotWorld);

            logger_.Debug("process {0} success", _root.name);
        }


        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="_onFinish"></param>
        protected void createInstances(System.Action _onFinish)
        {
            if(config_.instances.Length <= 0)
            {
                _onFinish();
                return;
            }

            int finished = 0;
            foreach (var instance in config_.instances)
            {
                runtime_.CreateInstanceAsync(instance.uid, instance.style, instance.uiRoot, instance.uiSlot, instance.worldRoot, instance.worldSlot, (_instance)=>
                {
                    finished += 1;
                    if(finished >= config_.instances.Length)
                    {
                        _onFinish();
                    }
                });
            }
        }

        /// <summary>
        /// 发布预加载中的主题
        /// </summary>
        protected void publishPreloadSubjects()
        {
            foreach (var subject in config_.preload.subjects)
            {
                var data = new Dictionary<string, object>();
                foreach (var parameter in subject.parameters)
                {
                    if (parameter.type.Equals("string"))
                        data[parameter.key] = parameter.value;
                    else if (parameter.type.Equals("int"))
                        data[parameter.key] = int.Parse(parameter.value);
                    else if (parameter.type.Equals("float"))
                        data[parameter.key] = float.Parse(parameter.value);
                    else if (parameter.type.Equals("bool"))
                        data[parameter.key] = bool.Parse(parameter.value);
                }
                modelDummy_.Publish(subject.message, data);
            }
        }

        /// <summary>
        /// 加载UnityAssetBundle
        /// </summary>
        /// <param name="_onFinish"></param>
        /// <param name="_onError"></param>
        /// <returns></returns>
        private IEnumerator loadUAB(System.Action<GameObject> _onFinish, System.Action<LibMVCS.Error> _onError)
        {
            LibMVCS.Any anyUAB;
            if (!settings_.TryGetValue("uab." + ModuleName, out anyUAB))
            {
                _onError(LibMVCS.Error.NewNullErr("uab not found"));
                yield break;
            }

            object objUAB = anyUAB.AsObject();
            if (null == objUAB)
            {
                _onError(LibMVCS.Error.NewNullErr("uab from Any convert to object is null"));
                yield break;
            }

            GameObject uab = objUAB as GameObject;
            if (null == objUAB)
            {
                _onError(LibMVCS.Error.NewNullErr("uab from object convert to GameObject is null"));
                yield break;
            }
            _onFinish(uab);
        }

    }
}
