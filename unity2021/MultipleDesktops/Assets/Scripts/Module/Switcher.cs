using System;
using System.Collections.Generic;
using UnityEngine;
using LibMVCS = XTC.FMP.LIB.MVCS;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;

namespace XTC.FMP.MOD.MultipleDesktops.LIB.Unity
{
    public class Switcher
    {
        public LibMVCS.Logger logger { get; set; }

        private class Options
        {
            public GameObject target;
            public float duration;
            public bool visible;
            public Action onFinish;
        }

        private Dictionary<string, Action<Options>> handlerS_ = new Dictionary<string, Action<Options>>();
        private TweenerCore<Vector2, Vector2, VectorOptions> tween_ = null;

        public Switcher()
        {
            handlerS_["push_center_to_left"] = pushCenterToLeft;
            handlerS_["push_center_to_right"] = pushCenterToRight;
            handlerS_["push_center_to_top"] = pushCenterToTop;
            handlerS_["push_center_to_bottom"] = pushCenterToBottom;
            handlerS_["pull_center_from_left"] = pullCenterFromLeft;
            handlerS_["pull_center_from_right"] = pullCenterFromRight;
            handlerS_["pull_center_from_top"] = pullCenterFromTop;
            handlerS_["pull_center_from_bottom"] = pullCenterFromBottom;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="_options.target">目标对象</param>
        /// <param name="_animation">预设动画名</param>
        /// <param name="_options.duration">持续时间</param>
        /// <param name="_visible">完成后是否可见</param>
        /// <param name="_options.onFinish">完成的回调</param>

        public void Do(GameObject _target, string _animation, float _duration, bool _visible, Action _onFinish)
        {
            Action<Options> handler;
            if (!handlerS_.TryGetValue(_animation, out handler))
            {
                logger.Error("handler of animation:{0} not found", _animation);
                return;
            }

            Options options = new Options();
            options.target = _target;
            options.duration = _duration;
            options.visible = _visible;
            options.onFinish = _onFinish;
            handler(options);
        }

        private void pushCenterToLeft(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(-rtTarget.rect.width, 0);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                _options.target.SetActive(_options.visible);
                tween_ = null;
                _options.onFinish();
            });
        }

        private void pushCenterToRight(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(rtTarget.rect.width, 0);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                _options.target.SetActive(_options.visible);
                tween_ = null;
                _options.onFinish();
            });
        }

        private void pushCenterToTop(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(0, rtTarget.rect.height);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                _options.target.SetActive(_options.visible);
                tween_ = null;
                _options.onFinish();
            });
        }

        private void pushCenterToBottom(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(0, -rtTarget.rect.height);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                _options.target.SetActive(_options.visible);
                tween_ = null;
                _options.onFinish();
            });
        }

        private void pullCenterFromLeft(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = new Vector2(-rtTarget.rect.width, 0);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                tween_ = null;
                _options.onFinish();
            });
        }

        private void pullCenterFromRight(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = new Vector2(rtTarget.rect.width, 0);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                tween_ = null;
                _options.onFinish();
            });
        }

        private void pullCenterFromTop(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = new Vector2(0, rtTarget.rect.height);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                tween_ = null;
                _options.onFinish();
            });
        }

        private void pullCenterFromBottom(Options _options)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _options.target.GetComponent<RectTransform>();
            var from = new Vector2(0, -rtTarget.rect.height);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _options.target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _options.duration).OnStart(() =>
            {
                onStart(_options.target);
            }).OnComplete(() =>
            {
                onComplete(_options.target);
                rtTarget.anchoredPosition = to;
                tween_ = null;
                _options.onFinish();
            });
        }

        private void onStart(GameObject _target)
        {
            _target.transform.Find("mask").gameObject.SetActive(true);
        }

        private void onComplete(GameObject _target)
        {
        }
    }
}
