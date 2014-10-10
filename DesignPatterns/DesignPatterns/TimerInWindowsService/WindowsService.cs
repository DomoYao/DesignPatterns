using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace DesignPatterns.TimerInWindowsService
{
    public enum LogLevel
    {
        Error=1,
        Info = 2

    }

    /// <summary>  
    /// 服务定时器管理  
    /// </summary>  
    public abstract class ServiceTimerControl
    {
        #region 私有成员
        /// <summary>  
        /// 定时器  
        /// </summary>  
        private Timer SysTimer { get; set; }
        /// <summary>  
        /// 是否启用定时器  
        /// </summary>  
        private bool _enabledTimer = true;
        /// <summary>  
        /// 服务执行状态, 0-休眠， 1-运行  
        /// </summary>  
        private int _serviceStatus;
        #endregion

        #region 公共属性

        /// <summary>  
        /// 获取服务状态  
        /// </summary>  
        public int ServiceStatus
        {
            get { return _serviceStatus; }
        }

        /// <summary>  
        /// 定时器配置  
        /// </summary>  
        public TimerConfig Config { get; set; }

        /// <summary>  
        /// 时间计算类  
        /// </summary>  
        public TimerControl TimerControl { get; set; }

        /// <summary>  
        /// 配置名称  
        /// </summary>  
        public virtual string ConfigName { get { return (ServiceTimerConfigManager.ServiceConfig == null ? "" : ServiceTimerConfigManager.ServiceConfig.Default); } }
        #endregion

        /// <summary>  
        /// 停止  
        /// </summary>  
        public void Stop()
        {
            _enabledTimer = false;

            if (SysTimer != null) SysTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>  
        /// 开始服务  
        /// </summary>  
        public void Start()
        {
            try
            {
                _enabledTimer = true;
                Config = GetTimerConfig();

                SysTimer = new Timer(TimerProcess, AppDomain.CurrentDomain, Config.Delay, Config.Interval);

                Logger(LogLevel.Info, "服务启动成功！");  
            }
            catch (Exception ex)
            {
                ServiceException(ex);
            }
        }

        /// <summary>  
        /// 单次执行服务程序  
        /// </summary>  
        public void Process()
        {
            try
            {
                //开始处理服务  
                StartService();
            }
            catch (Exception ex) { ServiceException(ex); } // 处理服务执行过程中出现的异常  
        }

        /// <summary>  
        /// 处理间隔服务  
        /// </summary>  
        /// <param name="sender"></param>  
        private void TimerProcess(object sender)
        {
            if (!_enabledTimer) return;

            bool timeIsUp = true;
            if (Config.TimerMode != TimerMode.Interval)
            {
                // 如果定时方式不是定时轮询的话，就构造TimerControl类，该类用来计算每次执行完程序后  
                // 到下次执行服务时需要休眠的时间  
                try
                {
                    TimerControl = new TimerControl(Config);
                    timeIsUp = TimerControl.TimeIsUp;  // 获取是否到了执行服务程序的时间了  
                }
                catch (Exception)
                {
                    // 读取配置出错且TimerControl对象已不存在，则再抛出异常  
                    // 如果上一次读取配置成功，那就就算这次的配置有问题，则也不会停止程序的运行，仍用上一次的数据做为参数  
                    if (TimerControl == null) throw;
                }
            }

            try
            {
                if (timeIsUp) // 时间到了可以执行程序了  
                {
                    // 服务运行了  
                    _serviceStatus = 1;

                    // 设置计时器，在无穷时间后再启用（实际上就是永远不启动计时器了--停止计时器计时）  
                    SysTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    //开始处理服务  
                    StartService();
                }
            }
            catch (Exception ex)
            {
                // 处理服务执行过程中出现的异常  
                ServiceException(ex);
            } 
            finally
            {
                // 如果计时器不为空，则重新设置休眠的时间  
                if (SysTimer != null)
                {
                    if (Config.TimerMode == TimerMode.Interval) // 定时轮询设置  
                    {
                        // 重新启用计时器  
                        SysTimer.Change(Config.Interval, Config.Interval);
                    }
                    else // 定时设置  
                    {
                        // 用cft类计算下一次到期的时间  
                        TimeSpan interval = TimerControl.GetNextTimeUp();
                        // 重新启用计时器  
                        SysTimer.Change(interval, interval);
                    }
                }

                _serviceStatus = 0;
            }
        }

        /// <summary>  
        /// 开始服务  
        /// </summary>  
        protected abstract void StartService();

        /// <summary>  
        /// 记录日志  
        /// </summary>  
        /// <param name="level">错误级别</param>  
        /// <param name="msg"></param>  
        protected virtual void Logger(LogLevel level, string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (level == LogLevel.Error)
            {
                path = Path.Combine(path, "error.log");
                File.AppendAllText(path,msg);
            }
            else if (level == LogLevel.Info)
            {
                path = Path.Combine(path, "Info.log");
                File.AppendAllText(path, msg);
            }
        }

        /// <summary>  
        /// 定时器初始化  
        /// </summary>  
        protected virtual TimerConfig GetTimerConfig()
        {
            var config = ServiceTimerConfigManager.ServiceConfig;
            if (config != null && config.Config.Length > 0)
            {
                // 如果没有配置则默认为第1个  
                if (String.IsNullOrEmpty(ConfigName))
                    return config.Config[0];
                foreach (var c in config.Config) if (String.Compare(c.RefName, ConfigName, StringComparison.OrdinalIgnoreCase) == 0) return c;
            }

            throw new Exception("时间策略配置不正确！");
        }

        /// <summary>  
        /// 系统服务错误  
        /// </summary>  
        /// <param name="ex"></param>  
        protected virtual void ServiceException(Exception ex)
        {
            Logger(LogLevel.Error, "服务异常：" + ex.Message + " \r\n堆栈：" + ex.StackTrace);
        }
    }

    #region 定时服务休眠计算类
    /// <summary>  
    /// 文件生成时间配置  
    /// </summary>  
    public class TimerControl
    {
        #region 私有成员
        private TimerConfig Config { get; set; }
        #endregion

        #region 公共成员方法
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="config">配置参数</param>  
        public TimerControl(TimerConfig config)
        {
            Config = config;
            if (Config == null) throw new Exception("定时器时间配置异常！");

            switch (Config.TimerMode)
            {
                case TimerMode.Date:
                    if (Config.MonthSeq < 1 || Config.MonthSeq > 12)
                        throw new Exception("定时器时间配置异常(月份取值只能是1~12)！");
                    var dt = new DateTime(2012, Config.MonthSeq, 1);  // 之所以选2012，是因为他是闰年，因此2月有29天。  
                    var lastDay = GetLastDayByMonth(dt);
                    if (Config.DaySeq < 1 || Config.DaySeq > lastDay)
                        throw new Exception("定时器时间配置异常(" + Config.MonthSeq + "月份的天数取值只能是1~" + lastDay + ")！");
                    break;
                case TimerMode.Day: break;
                case TimerMode.Month:
                    if (Config.DaySeq < 1 || Config.DaySeq > 31)
                        throw new Exception("定时器时间配置异常(天数取值只能是1~31)！");
                    break;
                case TimerMode.Week:
                    if (Config.DaySeq < 0 || Config.DaySeq > 6)
                        throw new Exception("定时器时间配置异常(星期取值只能是0~6)！");
                    break;
                case TimerMode.LastDayOfMonth:
                    if (Config.DaySeq != 0)
                    {
                        // 如果等于0的话，表示是每个月的最后一天。  
                        if (Config.DaySeq < 1 || Config.DaySeq > 28)
                            throw new Exception("定时器时间配置异常(倒数的天数只能是1~28，即倒数的第1天，第2天。。。有些月份并没有29.30.31天,因此最大只允许倒数第28天)！");
                        Config.DaySeq -= 1;
                    }
                    break;
                case TimerMode.Year:
                    if (Config.DaySeq < 1 || Config.DaySeq > 366)
                        throw new Exception("定时器时间配置异常(天数取值只能是1~366)！");
                    break;
            }
        }

        /// <summary>  
        /// 判断时间是否到了  
        /// </summary>  
        /// <returns>true时间已经到了，false时间还未到</returns>  
        public bool TimeIsUp
        {
            get
            {
                DateTime dt = DateTime.Now;
                if (CheckTimeIsUp(dt.TimeOfDay))
                {
                    switch (Config.TimerMode)
                    {
                        case TimerMode.Day: return true;
                        case TimerMode.Date: return dt.Month == Config.MonthSeq && dt.Day == Config.DaySeq;
                        case TimerMode.Week: return ((int)dt.DayOfWeek) == Config.DaySeq;
                        case TimerMode.Month: return dt.Day == Config.DaySeq;
                        case TimerMode.Year: return dt.DayOfYear == Config.DaySeq;
                        case TimerMode.LastDayOfMonth: return dt.Day == (GetLastDayByMonth(dt) - Config.DaySeq);
                        default: return false;
                    }
                }

                return false;
            }
        }

        /// <summary>  
        /// 时间是否到了  
        /// </summary>  
        /// <returns></returns>  
        private bool CheckTimeIsUp(TimeSpan time)
        {
            var tmp = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
            if (Config.Times == null)
                return (tmp.Ticks == 0);

            return Config.Times.Any(t => t == tmp);
        }

        /// <summary>  
        /// 从现在起到下次时间到还有多少时间  
        /// </summary>  
        /// <returns>时间间隔</returns>  
        public TimeSpan GetNextTimeUp()
        {
            DateTime nextDateTime = GetNextDateTime();    // 保存下一次要执行的时间  
            return nextDateTime - DateTime.Now;
        }

        /// <summary>  
        /// 获取下一次指定配置的时间是多少  
        /// </summary>  
        /// <returns></returns>  
        public DateTime GetNextDateTime()
        {
            var time = GetNextTimeConfig();
            DateTime dt = DateTime.Now;
            DateTime now, target;
            switch (Config.TimerMode)
            {
                case TimerMode.Day:
                    #region 每天指定某时执行一次
                    now = new DateTime(1, 1, 1, dt.Hour, dt.Minute, dt.Second);
                    target = new DateTime(1, 1, 1, time.Hours, time.Minutes, time.Seconds);
                    if (now.Ticks >= target.Ticks) dt = dt.AddDays(1.0); //如果当前时间小于指定时刻，则不需要加天  

                    dt = new DateTime(dt.Year, dt.Month, dt.Day, time.Hours, time.Minutes, time.Seconds);
                    #endregion
                    break;
                case TimerMode.Month:
                    #region 每月指定某天某时执行一次
                    now = new DateTime(1, 1, dt.Day, dt.Hour, dt.Minute, dt.Second);
                    target = new DateTime(1, 1, Config.DaySeq, time.Hours, time.Minutes, time.Seconds);   // 1月有31天，所以可以接受任何合法的Day值（因为在赋值时已判断1~31）  
                    if (now.Ticks >= target.Ticks) dt = dt.AddMonths(1);


                    // 当前月份的指定天数执行过了，因此月份加上一个月，当月份加了一个月之后，很可能当前实现的Day值可能会变小（例：3月31号，加上一个月，则日期会变成，4月30号，而不会变成5月1号）,  
                    // 因此需要判断指定的this.Day是不是比Day大（月份的Day变小的唯一原因是因为月份加了一个月之后，那个月并没有this.Day的天数），如果没有该this.Day的天数。则需要为该月份再加一个月。  
                    // 加一个月份，则那下个月一定可以大于等于this.Day, 看看每个月的天数就可以断定了，  
                    // 因为没有连着两个月的日期小于等于30的，只有连续两个月是31天。其它就是间隔的出现（this.Day最大只可能为31）  
                    // 如此之后，接下来的dt=new DateTime时不为因为dt.Month的月份，因没有this.Day天数而抛异常  
                    if (Config.DaySeq > GetLastDayByMonth(dt)) dt = dt.AddMonths(1);   // 如此是为了确保dt.Month的月份一定有this.Day天(因此如果设置为每个月的31号执行的程序，就只会在1,3,5,7,8,10,12几个月份会执行)  

                    dt = new DateTime(dt.Year, dt.Month, Config.DaySeq, time.Hours, time.Minutes, time.Seconds);
                    #endregion
                    break;
                case TimerMode.LastDayOfMonth:
                    #region 每个月倒数第N天的某时某刻执行一次
                    var lastDaybymonth = GetLastDayByMonth(dt) - Config.DaySeq;
                    now = new DateTime(1, 1, dt.Day, dt.Hour, dt.Minute, dt.Second);
                    target = new DateTime(1, 1, lastDaybymonth, time.Hours, time.Minutes, time.Seconds);  // 1月有31天，所以可以接受任何合法的Day值（因为在赋值时已判断1~31）  
                    if (now.Ticks >= target.Ticks)
                    {
                        dt = dt.AddMonths(1);
                        dt = new DateTime(dt.Year, dt.Month, GetLastDayByMonth(dt) - Config.DaySeq, time.Hours, time.Minutes, time.Seconds);// 根据新月份求新月份的最后一天。  
                    }
                    else
                        dt = new DateTime(dt.Year, dt.Month, lastDaybymonth, time.Hours, time.Minutes, time.Seconds);
                    #endregion
                    break;
                case TimerMode.Week:
                    #region 每星期指定星期某时执行一次
                    var dow = (int)dt.DayOfWeek;
                    now = new DateTime(1, 1, dow + 1, dt.Hour, dt.Minute, dt.Second);
                    target = new DateTime(1, 1, Config.DaySeq + 1, time.Hours, time.Minutes, time.Seconds);

                    dt = now.Ticks >= target.Ticks ? dt.AddDays(Config.DaySeq - dow + 7) : dt.AddDays(Config.DaySeq - dow);

                    dt = new DateTime(dt.Year, dt.Month, dt.Day, time.Hours, time.Minutes, time.Seconds);
                    #endregion
                    break;
                case TimerMode.Date:
                    #region 每年指定某月某日某时执行一次
                    now = new DateTime(4, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

                    // 0004年闰年，可以支持2月29.因此选了0004, 这样就不会在构造Target时异常，  
                    // 因为比较的关键不在年。所以，只要Now和Target的年份一样就可以，设置成什么年份无所谓  
                    target = new DateTime(4, Config.MonthSeq, Config.DaySeq, time.Hours, time.Minutes, time.Seconds);

                    if (now.Ticks >= target.Ticks) dt = dt.AddYears(1);
                    if (Config.MonthSeq == 2 && Config.DaySeq == 29)
                    {
                        // 因为闰年的最大间隔是8年，平时是4年一闰，可是0096年闰完之后，下一个闰年就是0104年,因此。。。  
                        for (int i = 0; i < 8; i++)
                            if (DateTime.IsLeapYear(dt.Year + i))
                            {
                                dt = dt.AddYears(i);
                                break;
                            }
                    }

                    dt = new DateTime(dt.Year, Config.MonthSeq, Config.DaySeq, time.Hours, time.Minutes, time.Seconds);
                    #endregion
                    break;
                case TimerMode.Year:
                    #region 每年指定第N天某时执行一次
                    now = new DateTime(1, 1, 1, dt.Hour, dt.Minute, dt.Second);
                    target = new DateTime(1, 1, 1, time.Hours, time.Minutes, time.Seconds);
                    if (dt.DayOfYear > Config.DaySeq || dt.DayOfYear == Config.DaySeq && now.Ticks >= target.Ticks) dt = dt.AddYears(1);
                    dt = dt.AddDays(Config.DaySeq - dt.DayOfYear);

                    dt = new DateTime(dt.Year, dt.Month, dt.Day, time.Hours, time.Minutes, time.Seconds);
                    #endregion
                    break;
                default:
                    throw new Exception("定时器时间配置异常！");
            }

            return dt;
        }

        /// <summary>  
        /// 获取指定日期所在月份的最后一天  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        private int GetLastDayByMonth(DateTime dt)
        {
            switch (dt.Month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    return DateTime.IsLeapYear(dt.Year) ? 29 : 28;
                default:
                    return 31;
            }
        }

        /// <summary>  
        /// 获取下一个时间点  
        /// </summary>  
        /// <returns></returns>  
        private TimeSpan GetNextTimeConfig()
        {
            if (Config.Times == null || Config.Times.Length == 0)
                return new TimeSpan(0);
            var minData = TimeSpan.MaxValue;        // 最小时间  
            var minExecData = TimeSpan.MaxValue;    // 大于当前时间的最小时间  
            foreach (var t in Config.Times)
            {
                if (DateTime.Now.TimeOfDay < t && minExecData >= t) // 找出比当前时间大的最小时间  
                    minExecData = t;
                if (minData > t)   // 找出最小的一个时间，当前时间不参与运算  
                    minData = t;
            }

            if (minExecData == TimeSpan.MaxValue) // 如果找不到比当前时间大的最小时间，则选择最小时间返回  
                return minData;
            return minExecData;
        }
        #endregion
    }

    #endregion

    #region 系统配置实体类&配置读取类
    /// <summary>  
    /// 时间配置类  
    /// </summary>  
    public class ServiceTimerConfig
    {
        /// <summary>  
        /// 默认配置  
        /// </summary>  
        public string Default { get; set; }
        /// <summary>  
        /// 配置项  
        /// </summary>  
        public TimerConfig[] Config { get; set; }
    }
    /// <summary>  
    /// 时间配置  
    /// </summary>  
    public class TimerConfig
    {
        /// <summary>  
        /// 配置引用名  
        /// </summary>  
        public string RefName { get; set; }

        /// <summary>  
        /// 时间模式  
        /// timeMode取值如下：TimerMode.Month、TimerMode.Week、TimerMode.Week、TimerMode.Day、TimerMode.Date、TimerMode.Year  
        /// </summary>  
        public TimerMode TimerMode { get; set; }

        /// <summary>  
        /// 指定某个时间算法的第几天，第1天就为1  
        /// TimerMode=TimerMode.Month           时， 该DaySeq表示每个月中的第几天  
        /// TimerMode=TimerMode.Week            时， 该DaySeq表示每个星期中的星期几（0-星期天，其它用1-6表示）  
        /// TimerMode=TimerMode.Day             时， 该值不需要设置  
        /// TimerMode=TimerMode.Date            时， 该DaySeq表示每个日期中的天数，如：8月12号，则DaySeq为12，MonthSeq为8  
        /// TimerMode=TimerMode.LastDayOfMonth  时， 该DaySeq表示每个月倒数第几天  
        /// TimerMode=TimerMode.Year            时， 该DaySeq表示每年中的第几天  
        /// </summary>  
        public int DaySeq { get; set; }

        /// <summary>  
        /// 当指定一年中某个月的某个日期时有用到如：指定一年中8月12号，则这里的MonthSeq就应该为8  
        /// </summary>  
        public int MonthSeq { get; set; }

        /// <summary>  
        /// 循环处理时间间隔（单位毫秒）  
        /// </summary>  
        public TimeSpan Interval { get; set; }

        /// <summary>  
        /// 启动延迟时间（单位毫秒）  
        /// </summary>  
        public TimeSpan Delay { get; set; }

        /// <summary>  
        /// 时间设置  
        /// </summary>  
        public TimeSpan[] Times { get; set; }
    }
    /// <summary>  
    /// 服务处理方法  
    /// </summary>  
    public enum TimerMode
    {
        /// <summary>  
        /// 轮询方式  
        /// </summary>  
        Interval = 0,
        /// <summary>  
        /// 一个月中某个天数的指定时间  
        /// </summary>  
        Month = 1,
        /// <summary>  
        /// 一周中的周几的指定时间  
        /// </summary>  
        Week = 2,
        /// <summary>  
        /// 一天中的指定时间  
        /// </summary>  
        Day = 3,
        /// <summary>  
        /// 一年中第几天的指定时间  
        /// </summary>  
        Year = 4,
        /// <summary>  
        /// 一年中的指定日期的指定时间  
        /// </summary>  
        Date = 5,
        /// <summary>  
        /// 每个月倒数第N天  
        /// </summary>  
        LastDayOfMonth,
        /// <summary>  
        /// 未设置  
        /// </summary>  
        NoSet
    }
    /// <summary>  
    /// 读取配置数据  
    /// </summary>  
    public class ServiceTimerConfigManager : IConfigurationSectionHandler
    {
        private static readonly Regex RegEx = new Regex(@"^(?<h>[01]?\d|2[0-3])(?:[:：](?<m>[0-5]\d?))?(?:[:：](?<s>[0-5]\d?))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>  
        /// 请求服务配置  
        /// </summary>  
        public static ServiceTimerConfig ServiceConfig { get; set; }
        /// <summary>  
        /// 静态构造函数  
        /// </summary>  
        static ServiceTimerConfigManager()
        {
            ConfigurationManager.GetSection("ServiceTimerConfig");
        }
        /// <summary>  
        /// 读取自定义配置节  
        /// </summary>  
        /// <param name="parent">父结点</param>  
        /// <param name="configContext">配置上下文</param>  
        /// <param name="section">配置区</param>  
        /// <returns></returns>  
        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            ServiceConfig = new ServiceTimerConfig();
            var config = new List<TimerConfig>();

            foreach (XmlNode node in section.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    switch (node.Name.ToLower())
                    {
                        case "default":
                            ServiceConfig.Default = node.InnerText;
                            break;
                        case "config":
                            var tmp = new TimerConfig();
                            SetTimerConfigValue(tmp, node);
                            config.Add(tmp);
                            break;
                    }
                }
            }
            ServiceConfig.Config = config.ToArray();

            return ServiceConfig;
        }
        /// <summary>  
        /// 设置定时器值  
        /// </summary>  
        /// <param name="config"></param>  
        /// <param name="node"></param>  
        private void SetTimerConfigValue(TimerConfig config, XmlNode node)
        {
            var times = new List<TimeSpan>();

            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.NodeType == XmlNodeType.Element)
                {
                    int tmp;
                    long longTmp;
                    switch (xn.Name.ToLower())
                    {
                        case "refname":
                            config.RefName = xn.InnerText;
                            break;
                        case "timermode":
                            config.TimerMode = (TimerMode)Enum.Parse(typeof(TimerMode), xn.InnerText);
                            break;
                        case "delay":
                            Int64.TryParse(xn.InnerText, out longTmp);
                            config.Delay = new TimeSpan(longTmp * 10 * 1000L);    // Delay配置值为毫秒  
                            break;
                        case "interval":
                            Int64.TryParse(xn.InnerText, out longTmp);        // Interval配置值为毫秒  
                            config.Interval = new TimeSpan(longTmp * 10 * 1000L);
                            break;
                        case "monthseq":    // 月份  
                            Int32.TryParse(xn.InnerText, out tmp);
                            config.MonthSeq = tmp;
                            break;
                        case "dayseq":      // 指定第几天的序号  
                            Int32.TryParse(xn.InnerText, out tmp);
                            config.DaySeq = tmp;
                            break;
                        case "times":
                            //还是用这个函数处理下一级的配置  
                            SetTimerConfigValue(config, xn);  // 设置时间策略  
                            break;
                        case "timevalue":
                            var mc = RegEx.Match(xn.InnerText);
                            if (!mc.Success) throw new Exception("时间配置不正确！");
                            int h;
                            Int32.TryParse(mc.Groups["h"].Value, out h);
                            int m;
                            Int32.TryParse(mc.Groups["m"].Value, out m);
                            int s;
                            Int32.TryParse(mc.Groups["s"].Value, out s);
                            times.Add(new TimeSpan(h, m, s));
                            break;
                    }
                }
            }
            if (times.Count != 0)
                config.Times = times.ToArray();
        }
    }
    #endregion
}