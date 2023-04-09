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

        private Dictionary<string, Action<GameObject, float>> handlerS_ = new Dictionary<string, Action<GameObject, float>>();
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
        /// <param name="_target">目标对象</param>
        /// <param name="_animation">预设动画名</param>
        /// <param name="_duration">持续时间</param>

        public void Do(GameObject _target, string _animation, float _duration)
        {
            Action<GameObject, float> handler;
            if (!handlerS_.TryGetValue(_animation, out handler))
            {
                logger.Error("handler of animation:{0} not found", _animation);
                return;
            }

            handler(_target, _duration);
        }

        private void pushCenterToLeft(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(-rtTarget.rect.width, 0);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                _target.SetActive(false);
                tween_ = null;
            });
        }

        private void pushCenterToRight(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(rtTarget.rect.width, 0);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                _target.SetActive(false);
                tween_ = null;
            });
        }

        private void pushCenterToTop(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(0, rtTarget.rect.height);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                _target.SetActive(false);
                tween_ = null;
            });
        }

        private void pushCenterToBottom(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = Vector2.zero;
            var to = new Vector2(0, -rtTarget.rect.height);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                _target.SetActive(false);
                tween_ = null;
            });
        }

        private void pullCenterFromLeft(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = new Vector2(-rtTarget.rect.width, 0);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                _target.SetActive(true);
                tween_ = null;
            });
        }

        private void pullCenterFromRight(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = new Vector2(rtTarget.rect.width, 0);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                tween_ = null;
            });
        }

        private void pullCenterFromTop(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = new Vector2(0, rtTarget.rect.height);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                tween_ = null;
            });
        }

        private void pullCenterFromBottom(GameObject _target, float _duration)
        {
            if (null != tween_)
            {
                tween_.Kill();
            }
            var rtTarget = _target.GetComponent<RectTransform>();
            var from = new Vector2(0, -rtTarget.rect.height);
            var to = new Vector2(0, 0);
            rtTarget.anchoredPosition = from;
            _target.SetActive(true);
            tween_ = rtTarget.DOAnchorPos(to, _duration).OnStart(() =>
            {
                onStart(_target);
            }).OnComplete(() =>
            {
                onComplete(_target);
                rtTarget.anchoredPosition = to;
                tween_ = null;
            });
        }

        private void onStart(GameObject _target)
        {
            _target.transform.Find("mask").gameObject.SetActive(true);
        }

        private void onComplete(GameObject _target)
        {
            _target.transform.Find("mask").gameObject.SetActive(false);
        }
    }
}
