
namespace XTC.FMP.MOD.MultipleDesktops.LIB.Unity
{
    public class MySubject : MySubjectBase
    {
        /// <summary>
        /// 使用预设动画进行切换
        /// </summary>
        /// <example>
        /// var data = new Dictionary<string, object>();
        /// data["uid"] = "default";
        /// data["animation"] = "push_to_left";
        /// data["duration"] = 1f;
        /// model.Publish(/XTC/MultipleDesktops/Switch, data);
        /// </example>
        public const string Switch = "/XTC/MultipleDesktops/Switch";
    }
}
