

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LibMVCS = XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.MultipleDesktops.LIB.Proto;
using XTC.FMP.MOD.MultipleDesktops.LIB.MVCS;

namespace XTC.FMP.MOD.MultipleDesktops.LIB.Unity
{
    /// <summary>
    /// 实例类
    /// </summary>
    public class MyInstance : MyInstanceBase
    {
        public class UiReference
        {
            public Button btnBack;
            public RawImage imgSplash;
        }

        private UiReference uiReference_ = new UiReference();

        private Switcher switcher_ = new Switcher();

        public MyInstance(string _uid, string _style, MyConfig _config, MyCatalog _catalog, LibMVCS.Logger _logger, Dictionary<string, LibMVCS.Any> _settings, MyEntryBase _entry, MonoBehaviour _mono, GameObject _rootAttachments)
            : base(_uid, _style, _config, _catalog, _logger, _settings, _entry, _mono, _rootAttachments)
        {
            switcher_.logger = _logger;
        }

        /// <summary>
        /// 当被创建时
        /// </summary>
        /// <remarks>
        /// 可用于加载主题目录的数据
        /// </remarks>
        public void HandleCreated()
        {
            if (null == style_)
            {
                logger_.Error("style not found");
                return;
            }

            var rawImage = rootUI.AddComponent<RawImage>();
            Color color = Color.black;
            if (ColorUtility.TryParseHtmlString(style_.color, out color))
            {
                rawImage.color = color;
            }
            if (!string.IsNullOrEmpty(style_.background))
            {
                loadTextureFromTheme(style_.background, (_texture) =>
                {
                    rawImage.texture = _texture;
                }, () => { });
            }

            uiReference_.imgSplash = rootUI.transform.Find("imgSplash").GetComponent<RawImage>();
            uiReference_.btnBack = rootUI.transform.Find("btnBack").GetComponent<Button>();
            uiReference_.btnBack.gameObject.SetActive(false);
            if (null != style_.backButton)
            {
                uiReference_.btnBack.gameObject.SetActive(true);
                alignByAncor(uiReference_.btnBack.transform, style_.backButton.anchor);
                uiReference_.btnBack.onClick.AddListener(() =>
                {
                    Dictionary<string, object> variableS = new Dictionary<string, object>();
                    logger_.Info(style_.backButton.subjectS.Length.ToString());
                    publishSubjects(style_.backButton.subjectS, variableS);
                });
                loadTextureFromTheme(style_.backButton.image, (_texture) =>
                {
                    uiReference_.btnBack.GetComponent<RawImage>().texture = _texture;
                }, () => { });
            }
            uiReference_.imgSplash.gameObject.SetActive(style_.splash != null);
            if (null != style_.splash)
            {
                loadTextureFromTheme(style_.splash.image, (_texture) =>
                {
                    uiReference_.imgSplash.texture = _texture;
                }, () => { });
            }
        }

        /// <summary>
        /// 当被删除时
        /// </summary>
        public void HandleDeleted()
        {
        }

        /// <summary>
        /// 当被打开时
        /// </summary>
        /// <remarks>
        /// 可用于加载内容目录的数据
        /// </remarks>
        public void HandleOpened(string _source, string _uri)
        {
            rootUI.gameObject.SetActive(true);
            rootWorld.gameObject.SetActive(true);
        }

        /// <summary>
        /// 当被关闭时
        /// </summary>
        public void HandleClosed()
        {
            rootUI.gameObject.SetActive(false);
            rootWorld.gameObject.SetActive(false);
        }

        /// <summary>
        /// 执行切换效果
        /// </summary>
        /// <param name="_animation">预设动画名</param>
        /// <param name="_duration">持续时间</param>
        /// <param name="_visible">切换完成后的可见性</param>
        public void DoSwitch(string _animation, float _duration, bool _visible)
        {
            if (null != style_.splash)
            {
                uiReference_.imgSplash.gameObject.SetActive(true);
            }
            switcher_.Do(rootUI, _animation, _duration, _visible, () =>
            {
                if (null != style_.splash)
                {
                    uiReference_.imgSplash.gameObject.SetActive(false);
                }
            });
        }
    }
}
